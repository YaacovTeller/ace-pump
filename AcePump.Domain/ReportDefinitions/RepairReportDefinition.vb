Imports System.Data.Entity
Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports AcePump.Rdlc.Builder

Namespace ReportDefinitions
    Public Class RepairReportDefinition
        Implements IReportDefinition

        Private Property DataSource As AcePumpContext
        Private Property DeliveryTicketID As Integer

        Public Sub New(ds As AcePumpContext, id As Integer)
            Me.DataSource = ds
            DeliveryTicketID = id
        End Sub

        Public Sub SetProperties(builder As RdlcBuilder) Implements IReportDefinition.SetProperties

        End Sub

        Public Sub LoadDatasets(builder As RdlcBuilder) Implements IReportDefinition.LoadDatasets
            Dim model = DataSource.DeliveryTickets _
                                       .Include(Function(h) h.PumpFailed) _
                                       .Include(Function(h) h.PumpDispatched) _
                                       .Select(Function(x) New With {
                                         .DeliveryTicketID = x.DeliveryTicketID,
                                         .CustomerID = x.CustomerID,
                                         .PumpFailedID = x.PumpFailedID,
                                         .HoldDown = x.HoldDown,
                                         .LeaseAndWell = If(x.Well IsNot Nothing AndAlso x.Well.Lease IsNot Nothing, x.Well.Lease.LocationName, Nothing) & "  " & If(x.Well IsNot Nothing, x.Well.WellNumber, Nothing),
                                         .PumpDispatchedNumber = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.ShopLocation.Prefix & x.PumpDispatched.PumpNumber, Nothing),
                                         .PumpFailedNumber = If(x.PumpFailed IsNot Nothing, x.PumpFailed.ShopLocation.Prefix & x.PumpFailed.PumpNumber, Nothing),
                                         .TicketDate = x.TicketDate,
                                         .PumpFailedTemplateVerbose = If(x.PumpFailed IsNot Nothing, x.PumpFailed.PumpTemplate.VerboseSpecificationSummary, Nothing),
                                         .PlungerBarrelWear = x.PlungerBarrelWear}) _
                                       .SingleOrDefault(Function(x) x.DeliveryTicketID = DeliveryTicketID)

            If model Is Nothing Then
                Throw New ArgumentException($"No delivery ticket found with id: {DeliveryTicketID}")
            End If


            builder.AddDataSetFromObject("RepairTicketDetails", model)

            Dim parts = DataSource.PartInspections _
                                        .Where(Function(x) x.DeliveryTicketID = DeliveryTicketID) _
                                        .Select(Function(x) New With {
                                            .PartInspectionID = x.PartInspectionID,
                                            .DeliveryTicketID = x.DeliveryTicketID,
                                            .Result = x.Result,
                                            .ReasonRepaired = x.ReasonRepaired,
                                            .OriginalPartTemplateNumber = If(x.PartFailed IsNot Nothing, x.PartFailed.Number, ""),
                                            .PartDescription = If(x.PartFailed IsNot Nothing, x.PartFailed.Description, ""),
                                            .Quantity = If(x.Quantity.HasValue, x.Quantity.Value, 0),
                                            .CanBeRepresentedAsAssembly = If(x.PartFailed IsNot Nothing, (x.PartFailed.RelatedAssemblyID.HasValue AndAlso x.PartFailed.RelatedAssemblyID.Value > 0), False),
                                            .PartReplacedID = If(x.PartReplacedID.HasValue, x.PartReplacedID.Value, 0),
                                            .ReplacementPartTemplateNumber = If(x.PartReplaced IsNot Nothing, x.PartReplaced.Number, ""),
                                            .ReplacementQuantity = If(x.ReplacementQuantity.HasValue, x.ReplacementQuantity.Value, 0),
                                            .ReplacedWithInventoryPartID = x.ReplacedWithInventoryPartID,
                                            .Sort = x.Sort,
                                            .TemplatePartDefID = x.TemplatePartDefID,
                                            .OriginalCustomerOwnedPartID = 0}) _
                                        .OrderBy(Function(x) x.Sort) _
                                        .ToList()

            Dim mostCurrentInspections = DataSource.PartInspections.Where(Function(x) x.DeliveryTicket.CustomerID = model.CustomerID _
                                                            AndAlso x.DeliveryTicket.PumpFailedID = model.PumpFailedID _
                                                            AndAlso x.DeliveryTicket.TicketDate < model.TicketDate) _
                                                            .Where(Function(x) x.Result = "Replace" Or x.Result = "Convert") _
                                                            .GroupBy(Function(x) x.TemplatePartDefID) _
                                                            .Select(Function(x) New With {.ReplacedWithInventoryPartID = x.OrderByDescending(Function(g) g.DeliveryTicket.TicketDate).FirstOrDefault().ReplacedWithInventoryPartID,
                                                                                          .TemplatePartDefID = x.Key}) _
                                                            .ToList()
            For Each part In parts
                Dim inventoryPart = mostCurrentInspections.FirstOrDefault(Function(x) x.TemplatePartDefID = part.TemplatePartDefID)
                ' case 3773
                If inventoryPart IsNot Nothing AndAlso inventoryPart.ReplacedWithInventoryPartID IsNot Nothing Then
                    part.OriginalCustomerOwnedPartID = inventoryPart.ReplacedWithInventoryPartID
                End If
            Next

            builder.AddDataSet("PartInspectionItems", parts)

            builder.AddDataSetFromObject("CustomerDetails", DataSource.Customers.Find(model.CustomerID))
        End Sub

        Public Sub SetRdlcPath(builder As RdlcBuilder) Implements IReportDefinition.SetRdlcPath
            builder.SetReportPath("RepairTicketReport.rdlc")
        End Sub

        Private Sub IReportDefinition_SaveAsName(builder As RdlcBuilder) Implements IReportDefinition.SetSaveAsName
            builder.SaveAsName = String.Format("Repair Ticket " & "{0}.pdf", DeliveryTicketID)
        End Sub

        Public Sub AddParams(builder As RdlcBuilder) Implements IReportDefinition.AddParams

        End Sub
    End Class
End Namespace

