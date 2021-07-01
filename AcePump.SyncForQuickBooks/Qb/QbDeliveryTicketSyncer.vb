Imports AcePump.QbApi.Context
Imports System.Threading
Imports AcePump.SyncForQuickBooks.UI
Imports AcePump.QbApi.Models
Imports AcePump.SyncForQuickBooks.UI.Models
Imports AcePump.SyncForQuickBooks.PtpApi.Models

Namespace Qb
    Public Class QbDeliveryTicketSyncer
        Public Shared Sub SyncDeliveryTicketsWithQb(validContexts As IEnumerable(Of QbSyncContext), runningInvoices As List(Of InvoiceUIModel), reporter As UIProgressReporter, token As CancellationToken)
            token.ThrowIfCancellationRequested()

            Using ctx As New QbContext(reporter, token, My.Settings.QBCompanyFile)
                reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.BuildingInvoiceListsFromTickets})
                Dim ticketsToAdd As List(Of QbSyncContext) = validContexts.Where(Function(x) String.IsNullOrEmpty(x.DeliveryTicket.RunningInvoiceNumber)).ToList()

                Dim result = From context In validContexts
                             Where Not String.IsNullOrEmpty(context.DeliveryTicket.RunningInvoiceNumber)
                             Group By context.DeliveryTicket.RunningInvoiceNumber Into g = Group
                             Select New QbModifyTicketGroupInfo With {
                                 .InvoiceNumber = RunningInvoiceNumber,
                                 .GroupedContexts = g.ToList(),
                                 .LookupInvoice = runningInvoices.FirstOrDefault(Function(x) x.QbInvoiceNumber = RunningInvoiceNumber)
                             }

                Dim groupedByInvoiceNumber As List(Of QbModifyTicketGroupInfo) = result.ToList

                AddNewTickets(ctx, ticketsToAdd)
                AddRunningTickets(ctx, groupedByInvoiceNumber)

                ctx.SaveChanges()

                For Each syncContext As QbSyncContext In ticketsToAdd
                    syncContext.DeliveryTicket.QuickbooksID = syncContext.Invoice.TxnID
                    syncContext.DeliveryTicket.QuickbooksInvoiceNumber = syncContext.Invoice.InvoiceRefNumber
                    syncContext.AddedToQuickbooks = syncContext.Invoice.AddedToQuickbooks
                    syncContext.Messages.AddRange(syncContext.Invoice.ErrorStatusMessages)
                Next

                For Each group As QbModifyTicketGroupInfo In groupedByInvoiceNumber
                    For Each syncContext In group.GroupedContexts
                        syncContext.DeliveryTicket.QuickbooksID = group.ModifyInvoice.TxnID
                        syncContext.DeliveryTicket.QuickbooksInvoiceNumber = group.ModifyInvoice.InvoiceRefNumber
                        syncContext.AddedToQuickbooks = group.ModifyInvoice.AddedToQuickbooks
                        syncContext.Messages.AddRange(group.ModifyInvoice.ErrorStatusMessages)
                    Next
                Next
            End Using
        End Sub

        Private Shared Sub AddNewTickets(ctx As QbContext, ticketsToAdd As List(Of QbSyncContext))
            For Each context As QbSyncContext In ticketsToAdd
                context.Invoice = New QbInvoice()

                AddNewInvoiceHeaders(context)
                AddNewInvoiceCustomFields(context)
                AddNewInvoiceAdditionalLines(context)
                AddNewInvoiceLines(context)

                ctx.AddNewInvoice(context.Invoice)
            Next
        End Sub

        Private Shared Sub AddNewInvoiceLines(ByVal context As QbSyncContext)
            For Each lineItem As LineItemModel In context.DeliveryTicket.LineItems.Where(Function(x) Not String.IsNullOrEmpty(x.PartQuickbooksID))
                context.Invoice.LineItems.Add(New QbLineItem With {.ItemRefListID = lineItem.PartQuickbooksID,
                                                                   .Quantity = lineItem.Quantity,
                                                                   .Desc = lineItem.Description,
                                                                   .PriceLevelRate = lineItem.UnitPrice,
                                                                   .SalesTaxCodeRefFullName = If(lineItem.LineIsTaxable, "Tax", "Non")})
            Next
        End Sub

        Private Shared Sub AddNewInvoiceCustomFields(ByVal context As QbSyncContext)
            context.Invoice.CustomFieldList = New Dictionary(Of String, String) From {
                                                                                          {QbCustomFieldTexts.LeaseAndWell, context.DeliveryTicket.LeaseAndWell},
                                                                                          {QbCustomFieldTexts.HoldDown, context.DeliveryTicket.HoldDown},
                                                                                          {QbCustomFieldTexts.TypeAndSizeOfPump, context.DeliveryTicket.TypeAndSizeOfPump},
                                                                                          {QbCustomFieldTexts.InvBarrel, context.DeliveryTicket.InvBarrel},
                                                                                          {QbCustomFieldTexts.InvPlunger, context.DeliveryTicket.InvPlunger},
                                                                                          {QbCustomFieldTexts.InvSVSeats, context.DeliveryTicket.InvSVSeats},
                                                                                          {QbCustomFieldTexts.InvSVBalls, context.DeliveryTicket.InvSVBalls},
                                                                                          {QbCustomFieldTexts.InvHoldDown, context.DeliveryTicket.InvHoldDown},
                                                                                          {QbCustomFieldTexts.InvValveRod, context.DeliveryTicket.InvValveRod},
                                                                                          {QbCustomFieldTexts.InvTVCages, context.DeliveryTicket.InvTVCages},
                                                                                          {QbCustomFieldTexts.InvTVSeats, context.DeliveryTicket.InvTVSeats},
                                                                                          {QbCustomFieldTexts.InvTVBalls, context.DeliveryTicket.InvTVBalls}
                                                                                      }

        End Sub

        Private Shared Sub AddNewInvoiceAdditionalLines(ByVal context As QbSyncContext)
            context.Invoice.AdditionalLineList = New List(Of Tuple(Of String, String)) From {
                                                                                                New Tuple(Of String, String)(QbAdditionalLineHeaders.RodGuide, context.DeliveryTicket.InvRodGuide),
                                                                                                New Tuple(Of String, String)(QbAdditionalLineHeaders.TypeBallAndSeat, context.DeliveryTicket.InvTypeBallAndSeat),
                                                                                                New Tuple(Of String, String)(QbAdditionalLineHeaders.OrderedBy, context.DeliveryTicket.OrderedBy),
                                                                                                New Tuple(Of String, String)(QbAdditionalLineHeaders.InvSVCages, context.DeliveryTicket.InvSVCages),
                                                                                                New Tuple(Of String, String)(QbAdditionalLineHeaders.LastPull, context.DeliveryTicket.LastPullAsFormattedDate),
                                                                                                New Tuple(Of String, String)(QbAdditionalLineHeaders.DeliveryTicketID, context.DeliveryTicket.DeliveryTicketID.ToString),
                                                                                                New Tuple(Of String, String)(QbAdditionalLineHeaders.Memo, context.DeliveryTicket.Notes)
                                                                                            }

            For Each lineItem As LineItemModel In context.DeliveryTicket.LineItems.Where(Function(x) String.IsNullOrEmpty(x.PartQuickbooksID))

                Dim invalidItemSummary As String = lineItem.Quantity.ToString() & " " & lineItem.PartTemplateNumber & " " &
                                                   lineItem.Description & " " &
                                                   Decimal.Round(lineItem.UnitPrice, 2).ToString & " " &
                                                   If(lineItem.LineIsTaxable, "Tax", "Non")

                context.Invoice.AdditionalLineList.Add(New Tuple(Of String, String)(QbAdditionalLineHeaders.LineItem, invalidItemSummary))
            Next
        End Sub

        Private Shared Sub AddNewInvoiceHeaders(syncContext As QbSyncContext)
            syncContext.Invoice.CustomerRefListID = syncContext.DeliveryTicket.CustomerQuickbooksID
            syncContext.Invoice.PONumber = syncContext.DeliveryTicket.PONumber
            syncContext.Invoice.FOB = syncContext.DeliveryTicket.PumpDispatchedNumber
            syncContext.Invoice.Other = syncContext.DeliveryTicket.PumpFailedNumber
            syncContext.Invoice.ItemSalesTaxRefListID = syncContext.DeliveryTicket.CountySalesTaxRateQuickbooksID
            syncContext.Invoice.DueDate = syncContext.DeliveryTicket.ShipDate
            syncContext.Invoice.ShipDate = syncContext.DeliveryTicket.PumpFailedDate
            syncContext.Invoice.ClassRefFullName = syncContext.DeliveryTicket.InvoiceClassFullName
        End Sub

        Private Shared Sub AddRunningTickets(ctx As QbContext, groupedByInvoiceNumber As IEnumerable(Of QbModifyTicketGroupInfo))
            For Each group In groupedByInvoiceNumber
                If group.LookupInvoice IsNot Nothing Then
                    Dim invoiceToModify As New QbInvoice

                    AddRunningInvoiceHeader(invoiceToModify, group)
                    AddFirstTimeLineHeader(invoiceToModify, group)
                    AddExistingLineItems(invoiceToModify, group)

                    For Each syncContext As QbSyncContext In group.GroupedContexts

                        AddRunningTicketLineHeader(invoiceToModify, syncContext)
                        AddRunningTicketLineItems(invoiceToModify, syncContext)
                        AddRunningTicketAdditionalLines(invoiceToModify, syncContext)

                    Next

                    group.ModifyInvoice = invoiceToModify


                    ctx.AddModifiedInvoice(invoiceToModify)
                End If
            Next
        End Sub

        Private Shared Sub AddFirstTimeLineHeader(ByVal invoiceToModify As QbInvoice, ByVal group As QbModifyTicketGroupInfo)
            If Not IsAlreadyModifiedInvoice(group.LookupInvoice) Then
                Dim header As String = FormatLineGroupHeader(group.LookupInvoice.LeaseAndWell,
                                                             group.LookupInvoice.PumpDispatchedNumber,
                                                             group.LookupInvoice.PumpFailedNumber,
                                                             group.LookupInvoice.QbTransactionDate)
                invoiceToModify.LineItems.Add(New QbLineItem With {.Desc = header})
            End If
        End Sub

        Private Shared Sub AddRunningTicketAdditionalLines(ByVal invoiceToModify As QbInvoice, ByVal syncContext As QbSyncContext)
            Dim additionalLines As New Dictionary(Of String, String) From {
                                                                            {QbAdditionalLineHeaders.RodGuide, syncContext.DeliveryTicket.InvRodGuide},
                                                                            {QbAdditionalLineHeaders.TypeBallAndSeat, syncContext.DeliveryTicket.InvTypeBallAndSeat},
                                                                            {QbAdditionalLineHeaders.OrderedBy, syncContext.DeliveryTicket.OrderedBy},
                                                                            {QbAdditionalLineHeaders.InvSVCages, syncContext.DeliveryTicket.InvSVCages},
                                                                            {QbAdditionalLineHeaders.LastPull, syncContext.DeliveryTicket.LastPullAsFormattedDate},
                                                                            {QbAdditionalLineHeaders.DeliveryTicketID, syncContext.DeliveryTicket.DeliveryTicketID.ToString},
                                                                            {QbAdditionalLineHeaders.Memo, syncContext.DeliveryTicket.Notes}
                                                                          }

            For Each pair As KeyValuePair(Of String, String) In additionalLines
                If Not String.IsNullOrEmpty(pair.Value) Then
                    invoiceToModify.LineItems.Add(New QbLineItem With {.Desc = pair.Key & pair.Value})
                End If
            Next
        End Sub

        Private Shared Sub AddRunningTicketLineItems(invoiceToModify As QbInvoice, syncContext As QbSyncContext)
            Dim ticketSubTotal As Decimal = 0
            Dim ticketTax As Decimal = 0
            For Each lineItem As LineItemModel In syncContext.DeliveryTicket.LineItems.Where(Function(x) Not String.IsNullOrEmpty(x.PartQuickbooksID))
                invoiceToModify.LineItems.Add(New QbLineItem With {.ItemRefListID = lineItem.PartQuickbooksID,
                                                                   .Quantity = lineItem.Quantity,
                                                                   .Desc = lineItem.Description,
                                                                   .PriceLevelRate = lineItem.UnitPrice,
                                                                   .SalesTaxCodeRefFullName = If(lineItem.LineIsTaxable, "Tax", "Non")})
                ticketSubTotal += (lineItem.UnitPrice * lineItem.Quantity)
                ticketTax += If(lineItem.LineIsTaxable, (lineItem.UnitPrice * lineItem.Quantity) * syncContext.DeliveryTicket.SalesTaxRate, 0)
            Next

            For Each lineItem As LineItemModel In syncContext.DeliveryTicket.LineItems.Where(Function(x) String.IsNullOrEmpty(x.PartQuickbooksID))
                Dim invalidItemSummary As String = lineItem.Quantity.ToString() & " " & lineItem.PartTemplateNumber & " " &
                                                   lineItem.Description & " " &
                                                   Decimal.Round(lineItem.UnitPrice, 2).ToString & " " &
                                                   If(lineItem.LineIsTaxable, "Tax", "Non")
                invoiceToModify.LineItems.Add(New QbLineItem With {.Desc = invalidItemSummary})
            Next


            Dim ticketTotals As String = FormatInvoiceTotals(ticketSubTotal, ticketTax)

            invoiceToModify.LineItems.Add(New QbLineItem With {.Desc = ticketTotals})
        End Sub

        Private Shared Sub AddRunningTicketLineHeader(invoiceToModify As QbInvoice, syncContext As QbSyncContext)
            invoiceToModify.ItemSalesTaxRefListID = syncContext.DeliveryTicket.CountySalesTaxRateQuickbooksID
            invoiceToModify.TxnDate = Date.Today
            invoiceToModify.ShipDate = syncContext.DeliveryTicket.PumpFailedDate

            invoiceToModify.LineItems.Add(New QbLineItem With {.Desc = " "})

            Dim ticketHeader As String = FormatLineGroupHeader(syncContext.DeliveryTicket.LeaseAndWell,
                                                             syncContext.DeliveryTicket.PumpDispatchedNumber,
                                                             syncContext.DeliveryTicket.PumpFailedNumber,
                                                             Date.Today)

            invoiceToModify.LineItems.Add(New QbLineItem With {.Desc = ticketHeader})
        End Sub

        Private Shared Sub AddExistingLineItems(ByVal invoiceToModify As QbInvoice, ByVal group As QbModifyTicketGroupInfo)
            For Each lineItemUIModel As InvoiceLineItemUIModel In group.LookupInvoice.QbInvoiceLineItems
                invoiceToModify.LineItems.Add(New QbLineItem With {.TxnLineID = lineItemUIModel.QbTransactionLineID,
                                                                   .ItemRefListID = lineItemUIModel.QbItemListID,
                                                                   .Quantity = lineItemUIModel.Quantity,
                                                                   .Desc = lineItemUIModel.Description,
                                                                   .PriceLevelRate = lineItemUIModel.Price,
                                                                   .SalesTaxCodeRefFullName = lineItemUIModel.SalesTaxRateName})
            Next

            If Not IsAlreadyModifiedInvoice(group.LookupInvoice) Then
                Dim totals As String = FormatInvoiceTotals(group.LookupInvoice.SubTotal, group.LookupInvoice.Tax)
                invoiceToModify.LineItems.Add(New QbLineItem With {.Desc = totals})
            End If
        End Sub

        Private Shared Sub AddRunningInvoiceHeader(ByVal invoiceToModify As QbInvoice, ByVal group As QbModifyTicketGroupInfo)
            invoiceToModify.CustomerRefListID = group.LookupInvoice.CustomerQuickbooksID
            invoiceToModify.ClassRefFullName = If(Not String.IsNullOrEmpty(group.LookupInvoice.QbInvoiceClassFullName),
                                                  group.LookupInvoice.QbInvoiceClassFullName,
                                                  group.GroupedContexts.First().DeliveryTicket.InvoiceClassFullName)
            invoiceToModify.TxnID = group.LookupInvoice.QbTransactionID
            invoiceToModify.TxnDate = Today
            invoiceToModify.EditSequence = group.LookupInvoice.QbEditSequence
            invoiceToModify.PONumber = ""
            invoiceToModify.Other = ""

            invoiceToModify.CustomFieldList = New Dictionary(Of String, String) From {{QbCustomFieldTexts.LeaseAndWell, ""},
                {QbCustomFieldTexts.HoldDown, ""},
                {QbCustomFieldTexts.TypeAndSizeOfPump, ""},
                {QbCustomFieldTexts.InvBarrel, ""},
                {QbCustomFieldTexts.InvPlunger, ""},
                {QbCustomFieldTexts.InvSVSeats, ""},
                {QbCustomFieldTexts.InvSVBalls, ""},
                {QbCustomFieldTexts.InvHoldDown, ""},
                {QbCustomFieldTexts.InvValveRod, ""},
                {QbCustomFieldTexts.InvTVCages, ""},
                {QbCustomFieldTexts.InvTVSeats, ""},
                {QbCustomFieldTexts.InvTVBalls, ""}}
        End Sub

        Shared Function GetInvoiceFor(qbInvoiceNumber As String, customerQuickbooksID As String, reporter As UIProgressReporter, token As CancellationToken) As RunningInvoiceSearchResult
            Using ctx As New QbContext(reporter, token, My.Settings.QBCompanyFile)
                Dim foundInvoice As QbInvoice = ctx.GetInvoice(qbInvoiceNumber, customerQuickbooksID)
                If foundInvoice IsNot Nothing Then
                    Dim result As New RunningInvoiceSearchResult With {.Status = RunningInvoiceSearchStatus.Found,
                                                                       .FoundInvoiceUIModel = New InvoiceUIModel With {.CustomerQuickbooksID = foundInvoice.CustomerRefListID,
                                                                                                                       .QbTransactionID = foundInvoice.TxnID,
                                                                                                                       .QbEditSequence = foundInvoice.EditSequence,
                                                                                                                       .QbTransactionDate = foundInvoice.TxnDate,
                                                                                                                       .QbInvoiceNumber = foundInvoice.InvoiceRefNumber,
                                                                                                                       .PumpFailedNumber = foundInvoice.Other,
                                                                                                                       .PumpDispatchedNumber = foundInvoice.FOB,
                                                                                                                       .LeaseAndWell = foundInvoice.DataExtLeaseAndWell,
                                                                                                                       .SubTotal = foundInvoice.SubTotal,
                                                                                                                       .Tax = foundInvoice.Tax,
                                                                                                                       .QbInvoiceClassFullName = foundInvoice.ClassRefFullName}}
                    For Each item In foundInvoice.LineItems
                        result.FoundInvoiceUIModel.QbInvoiceLineItems.Add(New InvoiceLineItemUIModel With {.QbTransactionLineID = item.TxnLineID,
                                                                                                           .QbItemListID = item.ItemRefListID,
                                                                                                           .Description = item.Desc,
                                                                                                           .Quantity = item.Quantity,
                                                                                                           .Price = item.PriceLevelRate,
                                                                                                           .SalesTaxRateName = item.SalesTaxCodeRefFullName
                                                                                                            })
                    Next
                    Return result
                Else
                    Return New RunningInvoiceSearchResult With {.Status = RunningInvoiceSearchStatus.Failed,
                                                                .FoundInvoiceUIModel = Nothing}
                End If
            End Using
        End Function

        Private Class QbModifyTicketGroupInfo
            Public Property InvoiceNumber As String
            Public Property LookupInvoice As InvoiceUIModel
            Public Property ModifyInvoice As QbInvoice
            Public Property GroupedContexts As List(Of QbSyncContext)
        End Class

        Private Shared Function IsAlreadyModifiedInvoice(lookupInvoice As InvoiceUIModel) As Boolean
            If String.IsNullOrEmpty(lookupInvoice.LeaseAndWell) Then
                If String.IsNullOrEmpty(lookupInvoice.PumpFailedNumber) Then
                    If String.IsNullOrEmpty(lookupInvoice.PumpDispatchedNumber) Then
                        Return True
                    End If
                End If
            End If
            Return False
        End Function

        Private Shared Function FormatLineGroupHeader(leaseAndWell As String, dispatched As String, failed As String, invoiceDate As Date) As String
            Dim header As String = ""
            Dim pump As String = ""
            If Not String.IsNullOrEmpty(leaseAndWell) Then
                header = leaseAndWell & vbNewLine
            End If

            If Not String.IsNullOrEmpty(dispatched) Then
                pump += "PUMP OUT " & dispatched & "  "
            End If

            If Not String.IsNullOrEmpty(failed) Then
                pump += "PUMP REPAIRED " & failed
            End If

            If Not String.IsNullOrEmpty(pump) Then
                pump += vbNewLine
            End If

            header = header & pump & invoiceDate.ToShortDateString

            Return header
        End Function

        Private Shared Function FormatInvoiceTotals(ticketSubTotal As Decimal, ticketTax As Decimal) As String
            Return "SUB TOTAL: " & ticketSubTotal.ToString("c") & vbNewLine & _
                   "TAX: " & ticketTax.ToString("c") & vbNewLine & _
                   "TOTAL: " & (ticketSubTotal + ticketTax).ToString("c")
        End Function
    End Class
End Namespace