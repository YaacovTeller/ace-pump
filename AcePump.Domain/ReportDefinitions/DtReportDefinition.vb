Imports System.Data.Entity
Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports DelegateDecompiler
Imports AcePump.Rdlc.Builder

Namespace ReportDefinitions
    Public Class DtReportDefinition
        Implements IReportDefinition

        Private Function TryParseDate(ByVal from As String) As DateTime?
            Dim buffer As DateTime

            If from IsNot Nothing AndAlso DateTime.TryParse(from, buffer) Then
                Return buffer
            Else
                Return Nothing
            End If
        End Function

        Private Property DataSource As AcePumpContext
        Private Property DeliveryTicketID As Integer

        Public Sub New(ds As AcePumpContext, id As Integer)
            Me.DataSource = ds
            DeliveryTicketID = id
        End Sub

        Public Sub SetProperties(builder As RdlcBuilder) Implements IReportDefinition.SetProperties
            builder.SetEnableExternalImages(True)
        End Sub

        Public Sub LoadDatasets(builder As RdlcBuilder) Implements IReportDefinition.LoadDatasets
            Dim model = DataSource.DeliveryTickets _
                                         .Include(Function(x) x.PumpFailed) _
                                         .Include(Function(x) x.PumpDispatched) _
                                         .Where(Function(x) x.DeliveryTicketID = DeliveryTicketID) _
                                         .AsEnumerable() _
                                         .Select(Function(x) New With {
                                             .DeliveryTicketID = x.DeliveryTicketID,
                                             .CustomerID = x.CustomerID,
                                             .LeaseID = If(x.Well IsNot Nothing AndAlso x.Well.Lease IsNot Nothing, x.Well.Lease.LeaseID, New Integer?()),
                                             .WellID = If(x.Well IsNot Nothing, x.WellID, Nothing),
                                             .CustomerName = If(x.Customer IsNot Nothing, x.Customer.CustomerName, Nothing),
                                             .PumpFailedID = x.PumpFailedID,
                                             .PumpFailedNumber = If(x.PumpFailed IsNot Nothing, x.PumpFailed.ShopLocation.Prefix & x.PumpFailed.PumpNumber, Nothing),
                                             .PumpFailedDate = x.PumpFailedDate,
                                             .PumpDispatchedID = x.PumpDispatchedID,
                                             .PumpDispatchedNumber = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.ShopLocation.Prefix & x.PumpDispatched.PumpNumber, Nothing),
                                             .PumpDispatchedDate = x.PumpDispatchedDate,
                                             .PumpDispatchedTemplateID = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.PumpTemplateID, Nothing),
                                             .PumpDispatchedConciseTemplate = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.PumpTemplate.ConciseSpecificationSummary, Nothing),
                                             .LeaseLocation = If(x.Well IsNot Nothing AndAlso x.Well.Lease IsNot Nothing, x.Well.Lease.LocationName, Nothing),
                                             .WellNumber = If(x.Well IsNot Nothing, x.Well.WellNumber, Nothing),
                                             .TicketDate = x.TicketDate,
                                             .ShipVia = x.ShipVia,
                                             .LastPull = TryParseDate(x.LastPull),
                                             .OrderDate = x.OrderDate,
                                             .OrderedBy = x.OrderedBy,
                                             .PONumber = x.PONumber,
                                             .ShipDate = x.ShipDate,
                                             .IsClosed = (x.CloseTicket.HasValue AndAlso x.CloseTicket.Value),
                                             .HoldDown = x.HoldDown,
                                             .Stroke = x.Stroke,
                                             .CompletedBy = x.CompletedBy,
                                             .ReasonStillOpen = x.ReasonStillOpen,
                                             .RepairedBy = x.RepairedBy,
                                             .SalesTaxRate = x.SalesTaxRate,
                                             .Notes = x.Notes,
                                             .InvBarrel = x.InvBarrel,
                                             .InvSVCages = x.InvSVCages,
                                             .InvDVCages = x.InvDVCages,
                                             .InvSVSeats = x.InvSVSeats,
                                             .InvDVSeats = x.InvDVSeats,
                                             .InvSVBalls = x.InvSVBalls,
                                             .InvDVBalls = x.InvDVBalls,
                                             .InvHoldDown = x.InvHoldDown,
                                             .InvValveRod = x.InvValveRod,
                                             .InvPlunger = x.InvPlunger,
                                             .InvPTVCages = x.InvPTVCages,
                                             .InvPDVCages = x.InvPDVCages,
                                             .InvPTVSeats = x.InvPTVSeats,
                                             .InvPDVSeats = x.InvPDVSeats,
                                             .InvPTVBalls = x.InvPTVBalls,
                                             .InvPDVBalls = x.InvPDVBalls,
                                             .InvRodGuide = x.InvRodGuide,
                                             .InvTypeBallandSeat = x.InvTypeBallandSeat,
                                             .Signature = x.Signature,
                                             .SignatureDate = x.SignatureDate,
                                             .SignatureName = x.SignatureName,
                                             .SignatureCompanyName = x.SignatureCompanyName,
                                             .Quote = (x.Quote.HasValue AndAlso x.Quote.Value),
                                             .PlungerBarrelWear = x.PlungerBarrelWear}) _
                                         .SingleOrDefault()

            If model Is Nothing Then
                Throw New ArgumentException($"No delivery ticket found with id: {DeliveryTicketID}")
            End If

            builder.AddDataSetFromObject("DeliveryTicketShippingDetails", model)

            builder.AddDataSetFromObject("CustomerDetails", DataSource.Customers.Find(model.CustomerID))

            builder.AddDataSet("DeliveryTicketLineItems", DataSource.LineItems.Where(Function(x) x.DeliveryTicketID = DeliveryTicketID) _
                                                                .Select(Function(x) New With {
                                                                    .DeliveryTicketID = x.DeliveryTicketID,
                                                                    .LineItemID = x.LineItemID,
                                                                    .PartTemplateID = x.PartTemplateID,
                                                                    .PartTemplateNumber = If(x.PartTemplate IsNot Nothing, x.PartTemplate.Number, ""),
                                                                    .CollectSalesTax = x.CollectSalesTax,
                                                                    .UnitPrice = x.UnitPrice,
                                                                    .UnitDiscount = x.UnitDiscount,
                                                                    .CustomerDiscount = x.CustomerDiscount,
                                                                    .Quantity = x.Quantity,
                                                                    .UnitPriceAfterDiscount = Udf.ClrRound_10_4(x.UnitPriceAfterDiscount.Computed()),
                                                                    .LineTotal = Udf.ClrRound_10_4(x.LineTotal.Computed()),
                                                                    .SalesTaxAmount = Udf.ClrRound_10_4(x.SalesTaxAmount.Computed()),
                                                                    .ItemNumber = x.PartTemplate.Number,
                                                                    .Description = x.Description,
                                                                    .SortOrder = x.SortOrder}) _
                                                                .OrderBy(Function(x) x.SortOrder) _
                                                                .Decompile())
        End Sub

        Public Sub SetRdlcPath(builder As RdlcBuilder) Implements IReportDefinition.SetRdlcPath
            builder.SetReportPath("DeliveryTicketReport.rdlc")
        End Sub

        Private Sub IReportDefinition_SaveAsName(builder As RdlcBuilder) Implements IReportDefinition.SetSaveAsName
            builder.SaveAsName = String.Format("Delivery Ticket " & "{0}.pdf", DeliveryTicketID)
        End Sub

        Public Sub AddParams(builder As RdlcBuilder) Implements IReportDefinition.AddParams
            Dim spec = (From p In DataSource.Pumps
                        From d In DataSource.DeliveryTickets
                        Where d.DeliveryTicketID = DeliveryTicketID
                        Where p.PumpID = d.PumpDispatchedID
                        Select p.PumpTemplate.VerboseSpecificationSummary) _
                      .DefaultIfEmpty("") _
                      .FirstOrDefault()

            builder.AddParameter("DispatchedPumpTemplateVerbose", spec)
        End Sub
    End Class
End Namespace

