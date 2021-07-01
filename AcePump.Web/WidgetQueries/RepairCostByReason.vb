Imports AcePump.Domain.DataSource
Imports AcePump.Web.WidgetQueries.Models
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("RepairCostByReason")> _
    Public Class RepairCostByReason
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overloads Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim cost As IQueryable(Of TicketLineItemCost) = Common.TicketLineItemCost(query)

            manager.AddDataSet(From line In cost
                   Where line.LineItem.PartInspection IsNot Nothing AndAlso line.LineItem.PartInspection.ReasonRepaired <> "OK"
                   Group By line.LineItem.PartInspection.ReasonRepaired Into g = Group
                   Select New ChartDataPoint(Of String) With {
                       .Category = ReasonRepaired,
                       .Value = g.Select(Function(x) x.Cost).DefaultIfEmpty(0D).Sum()
                   })
        End Sub
    End Class
End Namespace