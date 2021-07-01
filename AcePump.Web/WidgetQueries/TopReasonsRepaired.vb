Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("TpReasonsRepaired")> _
    Public Class TopReasonsRepaired
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim numberToShow As Integer = query.AdditionalParameters("NumberToShow")

            Dim inspections As IQueryable(Of PartInspection) = Common.FailedInspections(query)

            If query.AdditionalParameters.ContainsKey("PartTemplateID") Then
                inspections = inspections.Where(Function(x) x.PartFailedID.HasValue _
                                                    AndAlso x.PartFailedID.Value = query.PartTemplateID.Value)
            End If

            inspections = inspections.Where(Function(x) x.Result <> "OK" And x.ReasonRepaired <> "OK")

            Dim result = From inspection In inspections
                           Group By inspection.ReasonRepaired Into g = Group
                           Order By g.Count() Descending
                           Select New ChartDataPoint(Of String) With {
                               .Category = ReasonRepaired,
                               .Value = g.Count()
                           }
                           Take numberToShow
            manager.AddDataSet(result)
        End Sub
    End Class
End Namespace