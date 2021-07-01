Imports AcePump.Domain.DataSource
Imports AcePump.Web.WidgetQueries.Models
Imports AcePump.Domain.Models
Imports System.Data.Entity.Infrastructure
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("AssociatedFailures")> _
    Public Class AssociatedFailures
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overloads Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim inspections As IQueryable(Of PartInspection) = Common.FailedInspections(query)

            Dim allInspectionsInTicket As DbQuery(Of PartInspection) = inspections.SelectMany(Function(x) x.DeliveryTicket.Inspections)

            Dim groupedByReason = From inspection In allInspectionsInTicket
                                  Where inspection.ReasonRepaired <> query.ReasonRepaired _
                                  And inspection.ReasonRepaired <> "OK"
                                  Group By inspection.ReasonRepaired Into g = Group
                                  Order By g.Count Descending
                                  Select New ChartDataPoint(Of String) With {
                                      .Category = ReasonRepaired,
                                      .Value = g.Count
                                  }
            Dim numberToShow As Integer = query.AdditionalParameters("NumberToShow")

            manager.AddDataSet(groupedByReason.Take(numberToShow))
        End Sub
    End Class
End Namespace