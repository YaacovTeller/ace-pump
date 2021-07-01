Imports AcePump.Domain.DataSource
Imports AcePump.Rdlc.Builder

Namespace ReportDefinitions
    Public Class TearDownReportDefinition
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
            Dim tearDownReportModel = DataSource.DeliveryTickets _
                                                             .Where(Function(x) x.DeliveryTicketID = DeliveryTicketID) _
                                                             .Select(Function(x) New With {
                                                                        .DeliveryTicketID = DeliveryTicketID,
                                                                        .PumpFailedNumber = If(x.PumpFailed IsNot Nothing, x.PumpFailed.ShopLocation.Prefix & x.PumpFailed.PumpNumber, ""),
                                                                        .PumpFailedTemplateVerbose = If(x.PumpFailed IsNot Nothing, If(x.PumpFailed.PumpTemplate IsNot Nothing, x.PumpFailed.PumpTemplate.VerboseSpecificationSummary, ""), ""),
                                                                        .TicketDate = x.TicketDate,
                                                                        .RepairMode = x.RepairMode,
                                                                        .CustomerID = x.CustomerID
                                                             }) _
                                                             .SingleOrDefault()

            If tearDownReportModel Is Nothing Then
                Throw New ArgumentException($"No delivery ticket found with id: {DeliveryTicketID}")
            End If

            builder.AddDataSetFromObject("RepairTicketDetails", tearDownReportModel)

            builder.AddDataSet("TearDownItems", DataSource.PartInspections _
                                    .Where(Function(x) x.DeliveryTicketID = DeliveryTicketID) _
                                    .OrderBy(Function(x) x.Sort) _
                                    .Select(Function(x) New With {
                                        .PartInspectionID = x.PartInspectionID,
                                        .Result = x.Result,
                                        .ReasonRepaired = x.ReasonRepaired,
                                        .OriginalPartTemplateNumber = If(x.PartFailed IsNot Nothing, x.PartFailed.Number, ""),
                                        .PartDescription = If(x.PartFailed IsNot Nothing, x.PartFailed.Description, ""),
                                        .Quantity = If(x.Quantity.HasValue, x.Quantity.Value, 0)}))

            builder.AddDataSetFromObject("CustomerDetails", DataSource.Customers.Find(tearDownReportModel.CustomerID))
        End Sub

        Public Sub SetRdlcPath(builder As RdlcBuilder) Implements IReportDefinition.SetRdlcPath
            builder.SetReportPath("TearDownReport.rdlc")
        End Sub

        Private Sub IReportDefinition_SaveAsName(builder As RdlcBuilder) Implements IReportDefinition.SetSaveAsName
            builder.SaveAsName = String.Format("Tear Down " & "{0}.pdf", DeliveryTicketID)
        End Sub

        Public Sub AddParams(builder As RdlcBuilder) Implements IReportDefinition.AddParams

        End Sub
    End Class
End Namespace

