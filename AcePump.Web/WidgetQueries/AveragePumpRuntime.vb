Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports System.Data.Entity.SqlServer
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries
Imports AcePump.Web.WidgetQueries.Models

Namespace WidgetQueries
    <SupportedWidgetId("AveragePumpRuntime")> _
    Public Class AveragePumpRuntime
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overloads Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim runtimes As IQueryable(Of PumpRuntime) = Common.PumpRuntimes(query)
            Dim data As IQueryable(Of ChartDataPoint(Of String))

            If query.AdditionalParameters("runtimeType") = RuntimeType.ByLease Then
                data = From r In runtimes
                       Where r.RuntimeStartedByTicket IsNot Nothing And r.RuntimeStartedByTicket.Well IsNot Nothing And r.RuntimeStartedByTicket.Well.Lease IsNot Nothing
                       Group By r.RuntimeStartedByTicket.Well.Lease Into g = Group
                       Select New LeaseDataPoint With {
                           .LeaseID = Lease.LeaseID,
                           .Category = Lease.LocationName,
                           .Value = g.Average(Function(x) CDec(If(x.LengthInDays.HasValue,
                                                                  x.LengthInDays.Value,
                                                                  If(x.Start.HasValue,
                                                                     SqlFunctions.DateDiff("d", x.Start.Value, Now),
                                                                     0)))
                                              )
                           }

            ElseIf query.AdditionalParameters("runtimeType") = RuntimeType.ByWell Then
                data = From r In runtimes
                       Where r.RuntimeStartedByTicket IsNot Nothing And r.RuntimeStartedByTicket.Well IsNot Nothing
                       Group By r.RuntimeStartedByTicket.Well Into g = Group
                       Select New WellDataPoint With {
                           .WellID = Well.WellID,
                           .Category = Well.WellNumber,
                           .Value = g.Average(Function(x) CDec(If(x.LengthInDays.HasValue,
                                                                  x.LengthInDays.Value,
                                                                  If(x.Start.HasValue,
                                                                     SqlFunctions.DateDiff("d", x.Start.Value, Now),
                                                                     0)))
                                              )
                           }
            Else
                Throw New InvalidOperationException("unknown runtime type: " & query.AdditionalParameters("runtimeType").ToString())
            End If

            manager.AddDataSet(data)
        End Sub
    End Class
End Namespace