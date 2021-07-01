Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports AcePump.Rdlc.Builder

Namespace ReportDefinitions
    Public Class PumpTemplateReportDefinition
        Implements IReportDefinition

        Private Property DataSource As AcePumpContext
        Private Property PumpTemplateID As Integer

        Public Sub New(ds As AcePumpContext, id As Integer)
            Me.DataSource = ds
            PumpTemplateID = id
        End Sub

        Public Sub SetProperties(builder As RdlcBuilder) Implements IReportDefinition.SetProperties

        End Sub

        Public Sub LoadDatasets(builder As RdlcBuilder) Implements IReportDefinition.LoadDatasets
            Dim model = DataSource.PumpTemplates.SingleOrDefault(Function(x) x.PumpTemplateID = PumpTemplateID)
            If model Is Nothing Then
                Throw New ArgumentException($"No pump template found with id: {PumpTemplateID}")
            End If

            builder.AddDataSetFromObject("PumpTemplateDetails", model)

            builder.AddDataSet("TemplatePartList", DataSource.TemplatePartDefs.Where(Function(x) x.PumpTemplateID = PumpTemplateID) _
                                                                                   .OrderBy(Function(x) x.SortOrder) _
                                                                                   .Select(Function(x As TemplatePartDef) New With {
                                                                                       .Cost = x.PartTemplate.Cost,
                                                                                       .Description = x.PartTemplate.Description,
                                                                                       .PartTemplateID = x.PartTemplateID,
                                                                                       .PartTemplateNumber = x.PartTemplate.Number,
                                                                                       .PumpTemplateID = x.PumpTemplateID,
                                                                                       .Quantity = x.Quantity,
                                                                                       .ResaleValue = Udf.ClrRound_10_4(x.PartTemplate.Cost / (1 - x.PartTemplate.Markup)),
                                                                                       .SortOrder = If(x.SortOrder.HasValue, x.SortOrder.Value, 0),
                                                                                       .TemplatePartDefID = x.TemplatePartDefID,
                                                                                       .TotalResaleValue = Udf.ClrRound_10_4(x.Quantity * x.PartTemplate.Cost / (1 - x.PartTemplate.Markup))}
                                                                                    ))
        End Sub

        Public Sub SetRdlcPath(builder As RdlcBuilder) Implements IReportDefinition.SetRdlcPath
            builder.SetReportPath("PumpTemplateReport.rdlc")
        End Sub

        Private Sub IReportDefinition_SaveAsName(builder As RdlcBuilder) Implements IReportDefinition.SetSaveAsName
            builder.SaveAsName = String.Format("Pump Template " & "{0}.pdf", PumpTemplateID)
        End Sub

        Public Sub AddParams(builder As RdlcBuilder) Implements IReportDefinition.AddParams

        End Sub
    End Class
End Namespace

