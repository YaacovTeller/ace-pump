Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports AcePump.Web.WidgetQueries.Models
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("TpFailingPart")> _
    Public Class TopFailingParts
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim numberToShow As Integer = query.AdditionalParameters("NumberToShow")

            Dim inspections As IQueryable(Of PartInspection) = Common.FailedInspections(query).Where(Function(x) x.Result <> "OK")

            Dim grouped As IQueryable(Of PartDataPoint(Of String))
            If query.AdditionalParameters("partType") = PartQueryType.ByCategory Then
                grouped = From inspection In inspections
                          Group By inspection.PartFailed.PartCategory.CategoryName Into g = Group
                          Order By g.Count() Descending
                          Select New PartDataPoint(Of String) With {
                              .Category = CategoryName,
                              .Value = g.Count()
                          }

            ElseIf query.AdditionalParameters("partType") = PartQueryType.ByPart Then
                grouped = From inspection In inspections
                          Group By inspection.PartFailed Into g = Group
                          Order By g.Count() Descending
                          Select New PartDataPoint(Of String) With {
                              .Category = PartFailed.Description,
                              .PartTemplateID = PartFailed.PartTemplateID,
                              .Value = g.Count()
                          }
            Else
                Throw New InvalidOperationException("unknown query type")
            End If

            manager.AddDataSet(grouped.Take(numberToShow))
        End Sub
    End Class
End Namespace