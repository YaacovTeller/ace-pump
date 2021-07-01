Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("LeasesWhereThisHappens")> _
    Public Class LeasesWhereThisHappens
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim numberToShow As Integer = query.AdditionalParameters("NumberToShow")

            Dim inspections As IQueryable(Of PartInspection) = Common.FailedInspections(query)

            Dim grouped As IQueryable(Of ChartDataPoint(Of String)) = From inspection In inspections
                                                            Group By inspection.DeliveryTicket.Well.Lease.LocationName Into g = Group
                                                            Order By g.Count() Descending
                                                            Select New ChartDataPoint(Of String) With {
                                                                .Category = LocationName,
                                                                .Value = g.Count()
                                                                }
            manager.AddDataSet(grouped.Take(numberToShow))
        End Sub
    End Class
End Namespace