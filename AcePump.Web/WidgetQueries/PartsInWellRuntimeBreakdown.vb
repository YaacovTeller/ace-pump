Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports System.Data.Entity.SqlServer
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries
Imports AcePump.Web.WidgetQueries.Models

Namespace WidgetQueries
    <SupportedWidgetId("PartsInWellRuntimeBreakdown")> _
    Public Class PartsInWellRuntimeBreakdown
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overloads Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim runtimes As IQueryable(Of PartRuntime) = Common.PartRuntimesByRuntime(query)

            Dim groupedByDispatch = From r In runtimes
                                    Group By r.RuntimeStartedByTicket Into g = Group
                                    Select New With {
                                        .Dispatch = RuntimeStartedByTicket,
                                        .Runtimes = g.Select(Function(x) New PartDataPoint(Of String) With {
                                                       .PartTemplateID = x.TemplatePartDef.PartTemplateID,
                                                       .Category = x.TemplatePartDef.PartTemplate.Description,
                                                       .Value = If(x.TotalDaysInGround.HasValue,
                                                                   x.TotalDaysInGround.Value,
                                                                   If(x.Start.HasValue And x.Finish.HasValue,
                                                                      SqlFunctions.DateDiff("d", x.Start, x.Finish),
                                                                      If(x.Finish.HasValue,
                                                                         SqlFunctions.DateDiff("d", x.Start, Now),
                                                                         0)
                                                                    )
                                                                )
                                                   })
                                    }

            For Each [set] In groupedByDispatch
                manager.AddDataSet([set].Runtimes, "Dispatch on " & [set].Dispatch.TicketDate.Value.ToShortDateString())
            Next
        End Sub
    End Class
End Namespace