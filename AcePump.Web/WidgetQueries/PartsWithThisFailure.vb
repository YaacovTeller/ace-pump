Imports AcePump.Domain.DataSource
Imports AcePump.Web.WidgetQueries.Models
Imports AcePump.Domain.Models
Imports System.Data.Entity.Infrastructure
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("PartsWithThisFailure")> _
    Public Class PartsWithThisFailure
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overloads Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim inspections As IQueryable(Of PartInspection) = Common.FailedInspections(query) _
                                                                .Where(Function(x) x.Result = "Replace" Or x.Result = "Convert")

            Dim groupedByPart = From inspection In inspections
                                Group By inspection.PartFailed Into g = Group
                                Order By g.Count Descending
                                Select New PartDataPoint(Of String) With {
                                      .PartTemplateID = PartFailed.PartTemplateID,
                                      .Category = PartFailed.Description,
                                      .Value = g.Count
                                  }
            Dim numberToShow As Integer = query.AdditionalParameters("NumberToShow")

            manager.AddDataSet(groupedByPart.Take(numberToShow))
        End Sub
    End Class
End Namespace