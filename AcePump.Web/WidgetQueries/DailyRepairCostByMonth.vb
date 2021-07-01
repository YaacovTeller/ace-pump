Imports AcePump.Domain.DataSource
Imports AcePump.Web.WidgetQueries.Models
Imports Yesod.Widgets.Queries
Imports Yesod.Widgets.Models

Namespace WidgetQueries
    <SupportedWidgetId("DailyRepairCostPastMonth")> _
    <SupportedWidgetId("DailyRepairCostPast6Months")> _
    <SupportedWidgetId("DailyRepairCostPast12Months")> _
    Public Class DailyRepairCostByMonth
        Inherits AcePumpChartQueryBase(Of DailyRepairCostDataPoint)

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overloads Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of DailyRepairCostDataPoint))
            query.EndDate = New Date(Long.Parse(query.AdditionalParameters("staticEndDate")))
            query.StartDate = New Date(Long.Parse(query.AdditionalParameters("staticStartDate")))

            Dim cost As IQueryable(Of TicketLineItemCost) = Common.TicketLineItemCost(query)

            Dim result = (From line In cost
                          Group By line.LineItem.DeliveryTicket.TicketDate Into g = Group
                          Order By TicketDate Ascending
                          Select New DailyRepairCostDataPoint With {
                            .Category = TicketDate,
                            .TotalCost = g.Select(Function(x) x.Cost).DefaultIfEmpty(0D).Sum(),
                            .TotalRepairCost = g.Where(Function(x) x.LineItem.PartInspectionID.HasValue).Select(Function(x) x.Cost).DefaultIfEmpty(0D).Sum()
                          })

            SetDbTimeout(query)

            manager.AddDataSet(result, "Data", "TotalRepairCost|Repair Cost", "TotalCost|Total Cost")
        End Sub

        Private Sub SetDbTimeout(query As AcePumpQueryModel)
            If (query.EndDate - query.StartDate).TotalDays > (30 * 7) Then ' more than 7 months
                DirectCast(DataSource, System.Data.Entity.DbContext).Database.CommandTimeout = 120
            End If
        End Sub

        Public Class DailyRepairCostDataPoint
            Public Property Category As Date
            Public Property TotalRepairCost As Decimal
            Public Property TotalCost As Decimal
        End Class
    End Class
End Namespace