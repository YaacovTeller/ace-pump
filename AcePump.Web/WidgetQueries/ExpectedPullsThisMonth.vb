Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports Yesod.Widgets.Queries
Imports Yesod.Widgets.Models

Namespace WidgetQueries
    <SupportedWidgetId("ExpectedPullsThisMonth")> _
    Public Class ExpectedPullsThisMonth
        Inherits AcePumpDirectionChangeQueryBase(Of Integer)

        Private Property FirstRun As Boolean = True

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Function GetValue(query As AcePumpQueryModel) As Integer
            If FirstRun Then
                FirstRun = False

                query.StartDate = Today
            End If

            Dim firstDayOfThisMonth As Date = New Date(query.StartDate.Year, query.StartDate.Month, 1)
            Dim firstDayOfThreeMonthsAgo As Date = firstDayOfThisMonth.AddMonths(-3)
            Dim lastDayOfLastMonth As Date = firstDayOfThisMonth.AddDays(-1)

            query.StartDate = firstDayOfThreeMonthsAgo
            query.EndDate = lastDayOfLastMonth
            Dim tickets As IQueryable(Of DeliveryTicket) = Common.Tickets(query)
            Dim pulls = From ticket In tickets
                        Where ticket.PumpFailedID.HasValue

            Dim total As Integer = pulls.Count() / 3
            Return total
        End Function

        Protected Overrides Function BuildResult(oldValue As Integer, newValue As Integer) As DirectionChangeResponseDataModel
            Return New DirectionChangeResponseDataModel() With {
                .Amount = newValue,
                .AmountChanged = newValue - oldValue,
                .Direction = If(newValue < oldValue, Direction.Down, Direction.Up),
                .PositiveResult = (newValue < oldValue)
            }
        End Function
    End Class
End Namespace