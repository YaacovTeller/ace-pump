Imports AcePump.Domain.Models
Imports AcePump.Web.Api.ActionFilters
Imports AcePump.Common
Imports AcePump.Web.Api.Models
Imports Yesod.Mvc

Namespace Api.Controllers
    <UseTokenAuth>
    Public Class QuickbooksController
        Inherits AcePumpApiControllerBase

        Private Property DeliveryTicketDtoMapper As New ModelMapper(Of DeliveryTicket, DeliveryTicketModel)(
            Function(x As DeliveryTicket) New DeliveryTicketModel With {
                .DeliveryTicketID = x.DeliveryTicketID,
                .LeaseAndWell = If(x.Well IsNot Nothing, If(x.Well.Lease IsNot Nothing, x.Well.Lease.LocationName & " " & x.Well.WellNumber, x.Well.WellNumber), ""),
                .CustomerID = x.CustomerID,
                .CustomerQuickbooksID = If(x.Customer IsNot Nothing, x.Customer.QuickbooksID, ""),
                .CustomerName = If(x.Customer IsNot Nothing, x.Customer.CustomerName, Nothing),
                .CustomerUsesRunningInvoices = x.Customer IsNot Nothing AndAlso x.Customer.UsesQuickbooksRunningInvoice,
                .PumpFailedDate = x.PumpFailedDate,
                .PumpFailedNumber = If(x.PumpFailed IsNot Nothing, x.PumpFailed.ShopLocation.Prefix & x.PumpFailed.PumpNumber, Nothing),
                .PumpDispatchedNumber = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.ShopLocation.Prefix & x.PumpDispatched.PumpNumber, Nothing),
                .TicketDate = x.TicketDate,
                .PONumber = x.PONumber,
                .ShipDate = x.ShipDate,
                .OrderedBy = x.OrderedBy,
                .LastPull = x.LastPull,
                .Notes = x.Notes,
                .TypeAndSizeOfPump = If(x.PumpDispatched IsNot Nothing, If(x.PumpDispatched.PumpTemplate IsNot Nothing, x.PumpDispatched.PumpTemplate.ConciseSpecificationSummary, ""), ""),
                .HoldDown = x.HoldDown,
                .CountySalesTaxRateName = If(x.CountySalesTaxRate IsNot Nothing, x.CountySalesTaxRate.CountyName, ""),
                .CountySalesTaxRateQuickbooksID = If(x.CountySalesTaxRate IsNot Nothing, x.CountySalesTaxRate.QuickbooksID, ""),
                .SalesTaxRate = x.SalesTaxRate,
                .InvoiceClassFullName = If(x.Customer IsNot Nothing, If(x.Customer.QbInvoiceClass IsNot Nothing, x.Customer.QbInvoiceClass.FullName, ""), ""),
                .InvBarrel = x.InvBarrel,
                .InvPlunger = x.InvPlunger,
                .InvSVSeats = x.InvSVSeats,
                .InvSVBalls = x.InvSVBalls,
                .InvTVCages = x.InvPTVCages,
                .InvTVBalls = x.InvPTVBalls,
                .InvTVSeats = x.InvPTVSeats,
                .InvHoldDown = x.InvHoldDown,
                .InvValveRod = x.InvValveRod,
                .InvTypeBallAndSeat = x.InvTypeBallandSeat,
                .InvRodGuide = x.InvRodGuide,
                .InvSVCages = x.InvSVCages,
                .LineItems = x.LineItems.OrderBy(Function(y) y.SortOrder) _
                                                    .Select(Function(y) New LineItemModel With {
                                                                .LineItemID = y.LineItemID,
                                                                .PartTemplateID = y.PartTemplateID,
                                                                .PartTemplateNumber = If(y.PartTemplate IsNot Nothing, y.PartTemplate.Number, ""),
                                                                .PartTemplateQuickbooksID = If(y.PartTemplate IsNot Nothing, y.PartTemplate.QuickbooksID, ""),
                                                                .Quantity = y.Quantity,
                                                                .Description = y.Description,
                                                                .UnitPrice = (y.UnitPrice * (1 - y.UnitDiscount)),
                                                                .LineIsTaxable = y.CollectSalesTax.HasValue AndAlso y.CollectSalesTax.Value
                                                            })
            }
        )

        Function GetLatestDeliveryTickets() As List(Of DeliveryTicketModel)
            Return DataSource.DeliveryTickets _
                            .Where(Function(x) x.InvoiceStatus = AcePumpInvoiceStatuses.ReadyForQuickbooks And x.CloseTicket = True) _
                            .Where(Function(x) x.CustomerID IsNot Nothing) _
                            .Select(DeliveryTicketDtoMapper.Selector) _
                            .ToList()
        End Function

        Function GetReadyTicketsCount() As Integer
            Return DataSource.DeliveryTickets _
                            .Where(Function(x) x.InvoiceStatus = AcePumpInvoiceStatuses.ReadyForQuickbooks And x.CloseTicket = True) _
                            .Where(Function(x) x.CustomerID IsNot Nothing) _
                            .Select(DeliveryTicketDtoMapper.Selector) _
                            .Count()
        End Function

        Sub PostCustomerQuickbooksIDs(qbCustomerModel As List(Of CustomerModel))
            Dim intersection = From qbInfo In qbCustomerModel
                               From customer In DataSource.Customers
                               Where customer.CustomerID = qbInfo.CustomerID
                               Select New With {.qbInfo = qbInfo,
                                                .customer = customer}

            For Each result In intersection
                result.customer.QuickbooksID = result.qbInfo.QbID
            Next

            DataSource.SaveChanges()
        End Sub

        Sub PostPartQuickbooksIDs(qbPartInfos As List(Of PartTemplateModel))
            Dim intersection = From qbInfo In qbPartInfos
                               From partTemplate In DataSource.PartTemplates
                               Where partTemplate.PartTemplateID = qbInfo.PartTemplateID
                               Select New With {.qbInfo = qbInfo,
                                                .partTemplate = partTemplate}

            For Each result In intersection
                result.partTemplate.QuickbooksID = result.qbInfo.QbID
            Next

            DataSource.SaveChanges()
        End Sub

        Sub PostSingleCustomerQuickbooksID(qbCustomerModel As CustomerModel)
            Dim customer As Customer = DataSource.Customers.Find(qbCustomerModel.CustomerID)
            If customer IsNot Nothing Then
                customer.QuickbooksID = qbCustomerModel.QbID

                DataSource.SaveChanges()
            End If
        End Sub

        Sub PostSinglePartQuickbooksID(qbPartTemplateModel As PartTemplateModel)
            Dim partTemplate As PartTemplate = DataSource.PartTemplates.Find(qbPartTemplateModel.PartTemplateID)
            If partTemplate IsNot Nothing Then
                partTemplate.QuickbooksID = qbPartTemplateModel.QbID

                DataSource.SaveChanges()
            End If
        End Sub

        Sub PostSingleDeliveryTicketQuickbooksID(invoiceModel As InvoiceModel)
            Dim deliveryticket As DeliveryTicket = DataSource.DeliveryTickets.Find(invoiceModel.DeliveryTicketID)
            If deliveryticket IsNot Nothing Then
                deliveryticket.QuickbooksID = invoiceModel.QbID
                deliveryticket.QuickbooksInvoiceNumber = invoiceModel.QbInvoiceNumber
                deliveryticket.InvoiceStatus = AcePumpInvoiceStatuses.InQuickbooks

                DataSource.SaveChanges()
            End If
        End Sub

        Sub PostSingleCountySalesTaxRateQuickbooksID(countySalesTaxRateModel As CountySalesTaxRateModel)
            Dim countySalesTaxRate As CountySalesTaxRate = DataSource.CountySalesTaxRates.FirstOrDefault(Function(x) x.CountyName = countySalesTaxRateModel.CountyName)
            If countySalesTaxRate IsNot Nothing Then
                countySalesTaxRate.QuickbooksID = countySalesTaxRateModel.QbID

                DataSource.SaveChanges()
            End If
        End Sub
    End Class
End Namespace
