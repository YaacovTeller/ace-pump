Imports AcePump.QbApi.Models
Imports Interop.QBFC13
Imports System.Threading

Namespace Context
    ''' <summary>
    ''' Unit of Work pattern for QB API.
    ''' </summary>
    Public Class QbContext
        Implements IDisposable

        Private Property NewInvoices As New List(Of QbInvoice)
        Private Property ModifiedInvoices As New List(Of QbInvoice)
        Private Property Reporter As IProgressReporter
        Private Property QbCompanyFile As String
        Private Property Token As CancellationToken

        Public Sub New(reporter As IProgressReporter, token As CancellationToken, qbCompanyFile As String)
            Me.Reporter = reporter
            Me.Token = token
            Me.QbCompanyFile = qbCompanyFile
        End Sub

        Public Sub AddNewInvoice(invoice As QbInvoice)
            NewInvoices.Add(invoice)
        End Sub

        Public Sub AddModifiedInvoice(invoice As QbInvoice)
            ModifiedInvoices.Add(invoice)
        End Sub

        Public Sub SaveChanges()
            Reporter.Update(New ProgressDto With {.Stage = QbProgressStage.Connecting})
            Using session As New QbSession(QbCompanyFile)
                SaveNewInvoices(session)
                SaveModifiedInvoices(session)
            End Using
        End Sub

        Private Sub SaveNewInvoices(session As QbSession)
            If NewInvoices.Any() Then
                session.ClearMessagesetRequest()
                AddInvoicesToRequest(session.MessageSetRequest)
                Dim response = session.SessionManager.DoRequests(session.MessageSetRequest)
                ReadInvoiceResponseData(response, NewInvoices)

                session.ClearMessagesetRequest()
                Dim addedInvoices As List(Of QbInvoice) = NewInvoices.Where(Function(x) x.AddedToQuickbooks = True).ToList
                BuildDataExtRequests(session.MessageSetRequest, addedInvoices)

                Dim xml = session.MessageSetRequest.ToXMLString()
                Dim dataExtResponseMessageSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)
                ReadInvoiceResponseData(dataExtResponseMessageSet, addedInvoices)
            End If
        End Sub

        Private Sub SaveModifiedInvoices(session As QbSession)
            If ModifiedInvoices.Any() Then
                session.ClearMessagesetRequest()
                AddModifiedInvoicesToRequest(session.MessageSetRequest)
                Dim response = session.SessionManager.DoRequests(session.MessageSetRequest)
                ReadInvoiceResponseData(response, ModifiedInvoices)

                session.ClearMessagesetRequest()
                Dim addedInvoices As List(Of QbInvoice) = ModifiedInvoices.Where(Function(x) x.AddedToQuickbooks = True).ToList
                BuildDataExtRequests(session.MessageSetRequest, ModifiedInvoices)

                Dim xml = session.MessageSetRequest.ToXMLString()
                Dim dataExtResponseMessageSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)
                ReadInvoiceResponseData(dataExtResponseMessageSet, ModifiedInvoices)
            End If
        End Sub

        Private Sub AddModifiedInvoicesToRequest(messageSetRequest As IMsgSetRequest)
            Dim totalModifiedInvoices = ModifiedInvoices.Count
            Dim upto As Integer = 0

            For Each invoice As QbInvoice In ModifiedInvoices
                Token.ThrowIfCancellationRequested()
                upto = upto + 1
                Dim progress As New ProgressDto With {.Stage = QbProgressStage.ProcessingModifiedInvoices,
                                                      .Data = New Dictionary(Of String, Object) From {{"Upto", upto},
                                                                                                      {"TotalTickets", totalModifiedInvoices}
                                                                                                     }
                                                     }
                Reporter.Update(progress)
                Dim invoiceModRequest As IInvoiceMod = messageSetRequest.AppendInvoiceModRq()

                SetModInvoiceProperties(invoiceModRequest, invoice)
                SetModInvoiceLineItems(invoiceModRequest, invoice)
                invoice.LastRequestID = messageSetRequest.RequestList.Count() - 1
            Next
        End Sub

        Private Sub SetModInvoiceProperties(invoiceModRequest As IInvoiceMod, invoice As QbInvoice)
            invoiceModRequest.TxnID.SetValue(invoice.TxnID)
            invoiceModRequest.EditSequence.SetValue(invoice.EditSequence)
            invoiceModRequest.CustomerRef.ListID.SetValue(invoice.CustomerRefListID)
            invoiceModRequest.IsPending.SetValue(False)
            invoiceModRequest.PONumber.SetValue(If(invoice.PONumber IsNot Nothing, Left(invoice.PONumber, 25), ""))
            invoiceModRequest.FOB.SetValue(If(invoice.FOB IsNot Nothing, Left(invoice.FOB, 13), ""))
            invoiceModRequest.IsToBePrinted.SetValue(True)
            invoiceModRequest.IsToBeEmailed.SetValue(False)
            invoiceModRequest.Other.SetValue(If(invoice.Other IsNot Nothing, Left(invoice.Other, 29), ""))
            invoiceModRequest.ItemSalesTaxRef.ListID.SetValue(invoice.ItemSalesTaxRefListID)
            invoiceModRequest.TxnDate.SetValue(invoice.TxnDate)

            If Not String.IsNullOrEmpty(invoice.ClassRefFullName) Then
                invoiceModRequest.ClassRef.FullName.SetValue(invoice.ClassRefFullName)
            End If

            If invoice.DueDate IsNot Nothing Then
                invoiceModRequest.DueDate.SetValue(invoice.DueDate)
            End If

            If invoice.ShipDate IsNot Nothing Then
                invoiceModRequest.ShipDate.SetValue(invoice.ShipDate)
            End If
        End Sub

        Private Sub SetModInvoiceLineItems(invoiceModRequest As IInvoiceMod, invoice As QbInvoice)
            For Each lineItem As QbLineItem In invoice.LineItems
                Dim orInvoiceLineMod As IORInvoiceLineMod = invoiceModRequest.ORInvoiceLineModList.Append()
                If String.IsNullOrEmpty(lineItem.TxnLineID) Then
                    orInvoiceLineMod.InvoiceLineMod.TxnLineID.SetValue("-1")
                Else
                    orInvoiceLineMod.InvoiceLineMod.TxnLineID.SetValue(lineItem.TxnLineID)
                End If

                If Not String.IsNullOrEmpty(lineItem.ItemRefListID) Then
                    orInvoiceLineMod.InvoiceLineMod.ItemRef.ListID.SetValue(lineItem.ItemRefListID)
                End If

                If lineItem.Quantity > 0 Then
                    orInvoiceLineMod.InvoiceLineMod.Quantity.SetValue(lineItem.Quantity)
                End If

                If Not String.IsNullOrEmpty(lineItem.Desc) Then
                    orInvoiceLineMod.InvoiceLineMod.Desc.SetValue(Left(lineItem.Desc, 4000))
                End If

                If lineItem.PriceLevelRate > 0 Then
                    orInvoiceLineMod.InvoiceLineMod.ORRatePriceLevel.Rate.SetValue(Decimal.Round(lineItem.PriceLevelRate, 2))
                End If

                If Not String.IsNullOrEmpty(lineItem.SalesTaxCodeRefFullName) Then
                    orInvoiceLineMod.InvoiceLineMod.SalesTaxCodeRef.FullName.SetValue(lineItem.SalesTaxCodeRefFullName)
                End If
            Next
        End Sub

        Public Function GetInvoice(invoiceNumber As String, customerQuickbooksID As String) As QbInvoice
            Dim foundInvoice As QbInvoice = Nothing
            Using session As New QbSession(QbCompanyFile)
                Dim invoiceQuery As IInvoiceQuery = session.MessageSetRequest.AppendInvoiceQueryRq()
                invoiceQuery.IncludeLineItems.SetValue(True)
                invoiceQuery.OwnerIDList.Add("0")
                invoiceQuery.ORInvoiceQuery.RefNumberList.Add(invoiceNumber)
                Dim xml As String = session.MessageSetRequest.ToXMLString()
                Dim responseMessageSet = session.SessionManager.DoRequests(session.MessageSetRequest)
                If (responseMessageSet IsNot Nothing) Then
                    Dim xmlREsponse As String = responseMessageSet.ToXMLString()
                    Dim responseList As IResponseList = responseMessageSet.ResponseList
                    If (responseList IsNot Nothing) Then
                        For responseCounter = 0 To responseList.Count - 1
                            Dim response As IResponse = responseList.GetAt(responseCounter)
                            If (response.StatusCode = 0) Then
                                If (Not response.Detail Is Nothing) Then
                                    Dim responseType As ENResponseType = CType(response.Type.GetValue(), ENResponseType)
                                    If responseType = ENResponseType.rtInvoiceQueryRs Then
                                        Dim invoiceRetList As IInvoiceRetList = CType(response.Detail, IInvoiceRetList)
                                        Dim invoiceRet As IInvoiceRet = CType(invoiceRetList.GetAt(0), IInvoiceRet)

                                        If invoiceRet.CustomerRef IsNot Nothing AndAlso invoiceRet.CustomerRef.ListID.GetValue() = customerQuickbooksID Then
                                            foundInvoice = New QbInvoice
                                            foundInvoice.CustomerRefListID = customerQuickbooksID
                                            foundInvoice.TxnID = invoiceRet.TxnID.GetValue
                                            foundInvoice.EditSequence = invoiceRet.EditSequence.GetValue
                                            foundInvoice.TxnDate = invoiceRet.TxnDate.GetValue
                                            foundInvoice.InvoiceRefNumber = invoiceRet.RefNumber.GetValue()
                                            foundInvoice.Other = If(invoiceRet.Other IsNot Nothing, invoiceRet.Other.GetValue(), "")
                                            foundInvoice.FOB = If(invoiceRet.FOB IsNot Nothing, invoiceRet.FOB.GetValue(), "")
                                            foundInvoice.SubTotal = If(invoiceRet.Subtotal IsNot Nothing, invoiceRet.Subtotal.GetValue(), 0)
                                            foundInvoice.Tax = If(invoiceRet.SalesTaxTotal IsNot Nothing, invoiceRet.SalesTaxTotal.GetValue(), 0)
                                            foundInvoice.ClassRefFullName = If(invoiceRet.ClassRef IsNot Nothing, invoiceRet.ClassRef.FullName.GetValue(), "")
                                            For lineCounter = 0 To invoiceRet.ORInvoiceLineRetList.Count - 1
                                                If invoiceRet.ORInvoiceLineRetList.GetAt(lineCounter).InvoiceLineRet IsNot Nothing Then
                                                    Dim lineRet As IInvoiceLineRet = invoiceRet.ORInvoiceLineRetList.GetAt(lineCounter).InvoiceLineRet
                                                    foundInvoice.LineItems.Add(New QbLineItem With {.TxnLineID = lineRet.TxnLineID.GetValue,
                                                                                                         .ItemRefListID = If(lineRet.ItemRef IsNot Nothing, lineRet.ItemRef.ListID.GetValue, ""),
                                                                                                         .Desc = If(lineRet.Desc IsNot Nothing, lineRet.Desc.GetValue(), ""),
                                                                                                         .Quantity = If(lineRet.Quantity IsNot Nothing, lineRet.Quantity.GetValue(), 0),
                                                                                                         .PriceLevelRate = If(lineRet.ORRate IsNot Nothing, lineRet.ORRate.Rate.GetValue(), 0),
                                                                                                         .SalesTaxCodeRefFullName = If(lineRet.SalesTaxCodeRef IsNot Nothing, lineRet.SalesTaxCodeRef.FullName.GetValue(), "")}
                                                                                    )
                                                End If

                                            Next

                                            If invoiceRet.DataExtRetList IsNot Nothing Then
                                                For dataExtCounter = 0 To invoiceRet.DataExtRetList.Count - 1
                                                    Dim dataExtRet As IDataExtRet = invoiceRet.DataExtRetList.GetAt(dataExtCounter)
                                                    If dataExtRet.DataExtName IsNot Nothing AndAlso dataExtRet.DataExtName.GetValue() = QbCustomFieldTexts.LeaseAndWell Then
                                                        foundInvoice.DataExtLeaseAndWell = dataExtRet.DataExtValue.GetValue()
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            End Using
            Return foundInvoice
        End Function

        Private Sub AddInvoicesToRequest(messageSetRequest As IMsgSetRequest)
            Dim totalTickets = NewInvoices.Count
            Dim upto As Integer = 0

            For Each invoice As QbInvoice In NewInvoices
                Token.ThrowIfCancellationRequested()
                upto = upto + 1
                Dim progress As New ProgressDto With {.Stage = QbProgressStage.ProcessingNewInvoices,
                                                      .Data = New Dictionary(Of String, Object) From {{"Upto", upto},
                                                                                                      {"TotalTickets", totalTickets}
                                                                                                     }
                                                     }
                Reporter.Update(progress)
                Dim invoiceAddRequest As IInvoiceAdd = messageSetRequest.AppendInvoiceAddRq()

                SetNewInvoiceProperties(invoiceAddRequest, invoice)
                SetNewInvoiceLineItems(invoiceAddRequest, invoice)
                SetNewInvoiceAdditionalLineTexts(invoiceAddRequest, invoice)
                invoice.LastRequestID = messageSetRequest.RequestList.Count() - 1
            Next
        End Sub

        Private Sub SetNewInvoiceProperties(invoiceAddRequest As IInvoiceAdd, invoice As QbInvoice)
            invoiceAddRequest.CustomerRef.ListID.SetValue(invoice.CustomerRefListID)
            invoiceAddRequest.IsPending.SetValue(False)
            invoiceAddRequest.PONumber.SetValue(If(invoice.PONumber IsNot Nothing, Left(invoice.PONumber, 25), ""))
            invoiceAddRequest.FOB.SetValue(If(invoice.FOB IsNot Nothing, Left(invoice.FOB, 13), ""))
            invoiceAddRequest.IsToBePrinted.SetValue(True)
            invoiceAddRequest.IsToBeEmailed.SetValue(False)
            invoiceAddRequest.Other.SetValue(If(invoice.Other IsNot Nothing, Left(invoice.Other, 29), ""))
            invoiceAddRequest.ItemSalesTaxRef.ListID.SetValue(invoice.ItemSalesTaxRefListID)
            invoiceAddRequest.TxnDate.SetValue(Date.Today)

            If Not String.IsNullOrEmpty(invoice.ClassRefFullName) Then
                invoiceAddRequest.ClassRef.FullName.SetValue(invoice.ClassRefFullName)
            End If

            If invoice.DueDate IsNot Nothing Then
                invoiceAddRequest.DueDate.SetValue(invoice.DueDate)
            End If

            If invoice.ShipDate IsNot Nothing Then
                invoiceAddRequest.ShipDate.SetValue(invoice.ShipDate)
            End If
        End Sub

        Private Sub SetNewInvoiceLineItems(invoiceAddRequest As IInvoiceAdd, invoice As QbInvoice)
            For Each lineItem As QbLineItem In invoice.LineItems
                Dim orInvoiceLineAdd As IORInvoiceLineAdd = invoiceAddRequest.ORInvoiceLineAddList.Append()
                orInvoiceLineAdd.InvoiceLineAdd.ItemRef.ListID.SetValue(lineItem.ItemRefListID)

                orInvoiceLineAdd.InvoiceLineAdd.Quantity.SetValue(lineItem.Quantity)
                orInvoiceLineAdd.InvoiceLineAdd.Desc.SetValue(Left(lineItem.Desc, 4000))
                orInvoiceLineAdd.InvoiceLineAdd.ORRatePriceLevel.Rate.SetValue(Decimal.Round(lineItem.PriceLevelRate, 2))
                orInvoiceLineAdd.InvoiceLineAdd.SalesTaxCodeRef.FullName.SetValue(lineItem.SalesTaxCodeRefFullName)
            Next
        End Sub

        Private Sub SetNewInvoiceAdditionalLineTexts(invoiceAddRequest As IInvoiceAdd, invoice As QbInvoice)
            For Each additionalLine As Tuple(Of String, String) In invoice.AdditionalLineList
                If Not String.IsNullOrWhiteSpace(additionalLine.Item1) Then
                    Dim orInvoiceLineAddLast As IORInvoiceLineAdd = invoiceAddRequest.ORInvoiceLineAddList.Append()
                    orInvoiceLineAddLast.InvoiceLineAdd.Desc.SetValue(additionalLine.Item1 & additionalLine.Item2)
                End If
            Next
        End Sub

        Private Sub ReadInvoiceResponseData(ByVal responseMessageSet As IMsgSetResponse, invoices As List(Of QbInvoice))
            If (responseMessageSet IsNot Nothing) Then
                Dim xml As String = responseMessageSet.ToXMLString()
                Dim responseList As IResponseList = responseMessageSet.ResponseList
                If (responseList IsNot Nothing) Then
                    'There is one response for each request, this includes invoices and DataExt ONLY.
                    Dim currentInvoice As QbInvoice = Nothing
                    Dim upto As Integer = -1
                    For responseCounter = 0 To responseList.Count - 1
                        If currentInvoice Is Nothing OrElse responseCounter > currentInvoice.LastRequestID Then
                            upto += 1
                            currentInvoice = invoices.Item(upto)
                            Dim progress As New ProgressDto With {.Stage = QbProgressStage.SavingTickets,
                                                                  .Data = New Dictionary(Of String, Object) From {{"Upto", upto + 1}, {"TotalTickets", invoices.Count}}
                                                                 }
                            Reporter.Update(progress)
                        End If

                        Dim response As IResponse = responseList.GetAt(responseCounter)
                        If (response.StatusCode = 0) Then
                            If (Not response.Detail Is Nothing) Then
                                Dim responseType As ENResponseType = CType(response.Type.GetValue(), ENResponseType)
                                If (responseType = ENResponseType.rtInvoiceAddRs) Then
                                    Dim invoiceRet As IInvoiceRet = CType(response.Detail, IInvoiceRet)
                                    If currentInvoice IsNot Nothing Then
                                        currentInvoice.TxnID = invoiceRet.TxnID.GetValue()
                                        currentInvoice.InvoiceRefNumber = invoiceRet.RefNumber.GetValue()

                                        currentInvoice.AddedToQuickbooks = True
                                    End If
                                ElseIf (responseType = ENResponseType.rtInvoiceModRs) Then
                                    Dim invoiceRet As IInvoiceRet = CType(response.Detail, IInvoiceRet)
                                    If currentInvoice IsNot Nothing Then
                                        currentInvoice.TxnID = invoiceRet.TxnID.GetValue()
                                        currentInvoice.InvoiceRefNumber = invoiceRet.RefNumber.GetValue()

                                        currentInvoice.AddedToQuickbooks = True
                                    End If

                                End If
                            End If
                        Else
                            If (currentInvoice IsNot Nothing) Then
                                currentInvoice.ErrorStatusMessages.Add(response.StatusMessage)
                            End If
                        End If
                    Next responseCounter
                End If
            End If
        End Sub

        Private Sub BuildDataExtRequests(messageSetRequest As IMsgSetRequest, addedInvoices As List(Of QbInvoice))
            Dim totalAddedInvoices = addedInvoices.Count
            Dim upto As Integer = 0

            For Each invoice In addedInvoices
                Token.ThrowIfCancellationRequested()
                upto = upto + 1
                Dim progress As New ProgressDto With {.Stage = QbProgressStage.ProcessingTicketsAdditionalData,
                                                      .Data = New Dictionary(Of String, Object) From {{"Upto", upto},
                                                                                                      {"TotalTickets", totalAddedInvoices}
                                                                                                     }
                                                     }
                Reporter.Update(progress)
                For Each customField As KeyValuePair(Of String, String) In invoice.CustomFieldList
                    Dim key As String = customField.Key
                    If String.IsNullOrEmpty(customField.Value) Then
                        Dim dataExtDelRq As IDataExtDel = messageSetRequest.AppendDataExtDelRq
                        dataExtDelRq.OwnerID.SetValue("0")
                        dataExtDelRq.DataExtName.SetValue(customField.Key)

                        dataExtDelRq.ORListTxn.TxnDataExt.TxnID.SetValue(invoice.TxnID)
                        dataExtDelRq.ORListTxn.TxnDataExt.TxnDataExtType.SetValue(ENTxnDataExtType.tdetInvoice)
                    Else
                        Dim dataExtModRq As IDataExtMod = messageSetRequest.AppendDataExtModRq
                        dataExtModRq.OwnerID.SetValue("0")
                        dataExtModRq.DataExtName.SetValue(key)
                        dataExtModRq.DataExtValue.SetValue(Left(Trim(customField.Value), 30))

                        dataExtModRq.ORListTxn.TxnDataExt.TxnID.SetValue(invoice.TxnID)
                        dataExtModRq.ORListTxn.TxnDataExt.TxnDataExtType.SetValue(ENTxnDataExtType.tdetInvoice)
                    End If
                Next
                invoice.LastRequestID = messageSetRequest.RequestList.Count - 1
            Next
        End Sub

        Private Sub Dispose() Implements IDisposable.Dispose

        End Sub
    End Class
End Namespace