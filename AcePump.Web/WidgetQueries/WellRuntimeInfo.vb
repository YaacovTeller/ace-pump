Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("WellRuntimeInfo")> _
    <SupportedWidgetId("LeaseRuntimeInfo")> _
    Public Class WellRuntimeInfo
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overloads Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim runtimes As IQueryable(Of PartRuntime) = Common.PartRuntimesByRuntime(query)

            Throw New NotImplementedException()
        End Sub
    End Class
End Namespace