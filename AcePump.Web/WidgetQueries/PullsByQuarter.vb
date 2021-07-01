Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries
Imports System.Globalization

Namespace WidgetQueries
    <SupportedWidgetId("PullsByQuarter")> _
    Public Class PullsByQuarter
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overloads Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim tickets As IQueryable(Of DeliveryTicket) = Common.Tickets(query)

            Dim result As IQueryable(Of ChartDataPoint(Of String))
            If query.AdditionalParameters("period") = TimePeriod.Quarter Then
                Dim quartered = From ticket In tickets
                                Select New With {
                                    .Quarter = Math.Ceiling(ticket.TicketDate.Value.Month / 3D),
                                    .Ticket = ticket
                                }

                result = (From ticket In quartered
                          Group By ticket.Quarter Into g = Group
                          Select New ChartDataPoint(Of String) With {
                              .Category = Quarter,
                              .Value = g.Count()
                          }) _
                          .AsEnumerable() _
                          .Select(Function(x) New ChartDataPoint(Of String) With {
                                      .Category = "Q" & x.Category,
                                      .Value = x.Value
                                  }) _
                          .AsQueryable()

            ElseIf query.AdditionalParameters("period") = TimePeriod.Month Then
                Dim monthed = From ticket In tickets
                              Select New With {
                                  .Month = ticket.TicketDate.Value.Month,
                                  .Ticket = ticket
                              }

                result = (From ticket In monthed
                          Group By ticket.Month Into g = Group
                          Select New ChartDataPoint(Of String) With {
                              .Category = Month,
                              .Value = g.Count()
                          }) _
                          .AsEnumerable() _
                          .Select(Function(x) New ChartDataPoint(Of String) With {
                                      .Category = GetMonthName(x.Category),
                                      .Value = x.Value
                                  }) _
                          .AsQueryable()

            Else
                Throw New InvalidOperationException("unrecognized time period: " & query.AdditionalParameters("period").ToString())
            End If

            manager.AddDataSet(result)
        End Sub

        Private Function GetMonthName(monthNumber As String) As String
            Dim buffer As Integer
            If Integer.TryParse(monthNumber, buffer) Then
                Return CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(buffer)
            Else
                Return String.Empty
            End If
        End Function
    End Class
End Namespace