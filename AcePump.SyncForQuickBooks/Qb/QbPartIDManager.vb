Imports Interop.QBFC13
Imports System.Threading
Imports AcePump.SyncForQuickBooks.Qb.Models
Imports AcePump.SyncForQuickBooks.PtpApi.Models


Namespace Qb

    ''' <remarks>Consider refactoring to use AcePumpIntegration.QbContext like is used for adding / modifying invoices.</remarks>

    Public Class QbPartIDManager
        Public Shared Function GetQbPartIDLookup(deliveryTickets As List(Of DeliveryTicketModel), token As CancellationToken) As List(Of QbItemModel)
            Dim lineItems As List(Of LineItemModel) = deliveryTickets.SelectMany(Function(x) x.LineItems).ToList

            Dim lineItemPartTemplateNumbers As List(Of String) = lineItems.Where(Function(x) String.IsNullOrEmpty(x.PartQuickbooksID)) _
                    .Select(Function(x) x.PartTemplateNumber) _
                    .Distinct() _
                    .ToList
            If lineItemPartTemplateNumbers.Count > 0 Then
                Using session As New QbSession(My.Settings.QBCompanyFile.ToString())
                    token.ThrowIfCancellationRequested()
                    Dim itemQueryRequest As IItemQuery

                    For Each name As String In lineItemPartTemplateNumbers
                        token.ThrowIfCancellationRequested()
                        itemQueryRequest = session.MessageSetRequest.AppendItemQueryRq()
                        itemQueryRequest.ORListQuery.ListFilter.ORNameFilter.NameFilter.Name.SetValue(name)
                        itemQueryRequest.ORListQuery.ListFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcContains)
                        itemQueryRequest.IncludeRetElementList.Add("ListID")
                        itemQueryRequest.IncludeRetElementList.Add("Name")
                    Next

                    Dim responseMsgSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)

                    token.ThrowIfCancellationRequested()
                    Return ProcessItemQueryResponse(responseMsgSet)
                End Using
            Else
                Return Nothing
            End If
        End Function

        Public Shared Function ProcessItemQueryResponse(responseMsgSet As IMsgSetResponse) As List(Of QbItemModel)
            Dim lookupList As New List(Of QbItemModel)
            If responseMsgSet IsNot Nothing Then
                Dim responseList As IResponseList
                responseList = responseMsgSet.ResponseList
                Dim xml As String = responseMsgSet.ToXMLString
                If responseList IsNot Nothing Then
                    For j = 0 To responseList.Count - 1
                        Dim response As IResponse = responseList.GetAt(j)
                        If (response.StatusCode >= 0) Then
                            If (Not response.Detail Is Nothing) Then
                                Dim responseType As ENResponseType = CType(response.Type.GetValue(), ENResponseType)
                                If (responseType = ENResponseType.rtItemQueryRs) Then
                                    Dim itemRetList As IORItemRetList = CType(response.Detail, IORItemRetList)

                                    Dim itemRet As IORItemRet
                                    Dim listID As String
                                    Dim name As String
                                    Dim description As String = ""
                                    For i = 0 To itemRetList.Count - 1
                                        itemRet = itemRetList.GetAt(i)

                                        If (itemRet.ItemServiceRet IsNot Nothing) Then
                                            listID = itemRet.ItemServiceRet.ListID.GetValue()
                                            name = itemRet.ItemServiceRet.Name.GetValue()
                                            description = If(itemRet.ItemServiceRet.ORSalesPurchase IsNot Nothing,
                                                             If(itemRet.ItemServiceRet.ORSalesPurchase.SalesOrPurchase IsNot Nothing,
                                                                itemRet.ItemServiceRet.ORSalesPurchase.SalesOrPurchase.Desc.GetValue(), ""), "")
                                        ElseIf (itemRet.ItemInventoryRet IsNot Nothing) Then
                                            listID = itemRet.ItemInventoryRet.ListID.GetValue()
                                            name = itemRet.ItemInventoryRet.Name.GetValue()
                                            If itemRet.ItemInventoryRet.SalesDesc IsNot Nothing Then
                                                description = itemRet.ItemInventoryRet.SalesDesc.GetValue()
                                            End If
                                        ElseIf (itemRet.ItemNonInventoryRet IsNot Nothing) Then
                                            listID = itemRet.ItemNonInventoryRet.ListID.GetValue()
                                            name = itemRet.ItemNonInventoryRet.Name.GetValue()
                                            If itemRet.ItemNonInventoryRet.ORSalesPurchase IsNot Nothing AndAlso itemRet.ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.Desc IsNot Nothing Then
                                                description = itemRet.ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.Desc.GetValue()
                                            End If
                                        Else
                                            listID = ""
                                            name = ""
                                            description = ""
                                        End If
                                        If Not String.IsNullOrEmpty(listID) Then
                                            lookupList.Add(New QbItemModel With {.ListID = listID,
                                                                                 .Name = name,
                                                                                 .Description = description})
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Next j
                End If
            End If

            Return lookupList
        End Function

        Public Shared Function GetAllQbParts() As List(Of QbItemModel)
            Dim lookupList As List(Of QbItemModel)
            Using session As New QbSession(My.Settings.QBCompanyFile.ToString())
                Dim itemQueryRequest As IItemQuery = session.MessageSetRequest.AppendItemQueryRq()

                itemQueryRequest.IncludeRetElementList.Add("ListID")
                itemQueryRequest.IncludeRetElementList.Add("Name")
                itemQueryRequest.IncludeRetElementList.Add("Desc")
                itemQueryRequest.IncludeRetElementList.Add("ORSalesPurchase")
                itemQueryRequest.IncludeRetElementList.Add("SalesOrPurchase")
                itemQueryRequest.IncludeRetElementList.Add("SalesDesc")

                Dim responseMsgSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)
                lookupList = ProcessItemQueryResponse(responseMsgSet)
            End Using

            Return lookupList
        End Function

        Public Shared Function FindPartInQb(nameToFind As String) As List(Of QbItemModel)
            Dim lookupList As List(Of QbItemModel)

            Using session As New QbSession(My.Settings.QBCompanyFile.ToString())
                Dim itemQueryRequest As IItemQuery = session.MessageSetRequest.AppendItemQueryRq()

                itemQueryRequest.ORListQuery.ListFilter.ORNameFilter.NameFilter.Name.SetValue(nameToFind)
                itemQueryRequest.ORListQuery.ListFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcContains)
                itemQueryRequest.IncludeRetElementList.Add("ListID")
                itemQueryRequest.IncludeRetElementList.Add("Name")

                Dim responseMsgSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)
                lookupList = ProcessItemQueryResponse(responseMsgSet)
            End Using
            Return lookupList
        End Function

        Public Shared Function GetIncomeAccountList() As Dictionary(Of String, String)
            Using session As New QbSession(My.Settings.QBCompanyFile.ToString())
                Dim accountQuery As IAccountQuery = session.MessageSetRequest.AppendAccountQueryRq()
                accountQuery.ORAccountListQuery.AccountListFilter.AccountTypeList.Add(ENAccountType.atIncome)

                Dim responseMsgSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)
                Return ProcessAccountQueryResponse(responseMsgSet)
            End Using
        End Function

        Public Shared Function GetAssetAccountList() As Dictionary(Of String, String)
            Using session As New QbSession(My.Settings.QBCompanyFile.ToString())
                Dim accountQuery As IAccountQuery = session.MessageSetRequest.AppendAccountQueryRq()
                accountQuery.ORAccountListQuery.AccountListFilter.AccountTypeList.Add(ENAccountType.atFixedAsset)
                accountQuery.ORAccountListQuery.AccountListFilter.AccountTypeList.Add(ENAccountType.atOtherAsset)
                accountQuery.ORAccountListQuery.AccountListFilter.AccountTypeList.Add(ENAccountType.atOtherCurrentAsset)

                Dim responseMsgSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)
                Return ProcessAccountQueryResponse(responseMsgSet)
            End Using
        End Function

        Public Shared Function ProcessAccountQueryResponse(responseMsgSet As IMsgSetResponse) As Dictionary(Of String, String)
            Dim accountList As New Dictionary(Of String, String)

            If responseMsgSet IsNot Nothing Then
                Dim responseList As IResponseList
                responseList = responseMsgSet.ResponseList
                If responseList IsNot Nothing Then
                    For j = 0 To responseList.Count - 1
                        Dim response As IResponse = responseList.GetAt(j)
                        If (response.StatusCode >= 0) Then
                            If (Not response.Detail Is Nothing) Then
                                Dim responseType As ENResponseType = CType(response.Type.GetValue(), ENResponseType)
                                If (responseType = ENResponseType.rtAccountQueryRs) Then
                                    Dim accountRetList As IAccountRetList = CType(response.Detail, IAccountRetList)
                                    Dim accountRet As IAccountRet
                                    Dim accountDescription As String

                                    For i = 0 To accountRetList.Count - 1
                                        accountRet = accountRetList.GetAt(i)
                                        accountDescription = If(accountRet.Desc IsNot Nothing, " - " & accountRet.Desc.GetValue(), "")
                                        accountList.Add(accountRet.ListID.GetValue(), accountRet.Name.GetValue() & accountDescription)
                                    Next

                                End If
                            End If
                        End If
                    Next
                End If
            End If
            Return accountList
        End Function
    End Class
End Namespace