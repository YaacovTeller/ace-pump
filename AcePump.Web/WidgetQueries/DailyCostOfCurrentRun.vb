Imports AcePump.Domain.DataSource
Imports System.Data.Entity
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries
Imports AcePump.Domain.Models

Namespace WidgetQueries
    <SupportedWidgetId("DailyCostOfCurrentRun")> _
    Public Class DailyCostOfCurrentRun
        Inherits AcePumpDirectionChangeQueryBase(Of Decimal)

        Private Property Query As AcePumpQueryModel

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Function GetValue(query As AcePumpQueryModel) As Decimal
            Me.Query = query
        End Function

        Protected Overrides Function BuildResult(oldValue As Decimal, newValue As Decimal) As Yesod.Widgets.Models.DirectionChangeResponseDataModel
            Dim lastTwoTicketDays = (From ticket In DataSource.DeliveryTickets.Include(Function(x) x.LineItems)
                                                 Where ticket.WellID.HasValue AndAlso ticket.WellID.Value = Query.WellID.Value
                                                 Where ticket.PumpFailedID.HasValue Or ticket.PumpDispatchedID.HasValue
                                                 Group By ticket.TicketDate Into g = Group
                                                 Order By g.FirstOrDefault().TicketDate Descending
                                                 Take 2).AsEnumerable() _
                                                 .Select(Function(l) New With {
                                                             .TicketDate = l.TicketDate,
                                                             .GroupOnDate = l.g,
                                                             .HasPumpDispatched = l.g.Any(Function(x) x.PumpDispatched IsNot Nothing),
                                                             .TotalCost = l.g.Aggregate(0D, Function(seed As Decimal, y As DeliveryTicket) seed + CalculateTicketCost(y))
                                                         })

            Dim dailyCostLastRun As Decimal, dailyCostCurrentRun As Decimal
            If lastTwoTicketDays.Count = 2 Then
                Dim lastRunLength As Integer = (lastTwoTicketDays(0).TicketDate.Value - lastTwoTicketDays(1).TicketDate.Value).TotalDays
                Dim lastRunCost As Decimal = lastTwoTicketDays(1).TotalCost
                dailyCostLastRun = lastRunCost / lastRunLength

                Dim currentRunLength As Integer = (Date.Now - lastTwoTicketDays(0).TicketDate.Value).TotalDays
                Dim currentRunCost As Decimal = lastTwoTicketDays(0).TotalCost
                If currentRunLength = 0 Then currentRunLength = 1
                dailyCostCurrentRun = currentRunCost / currentRunLength
            ElseIf lastTwoTicketDays.Count = 1 AndAlso lastTwoTicketDays(0).HasPumpDispatched Then
                Dim currentRunLength As Integer = (Date.Now - lastTwoTicketDays(0).TicketDate.Value).TotalDays
                Dim currentRunCost As Decimal = lastTwoTicketDays(0).TotalCost
                If currentRunLength = 0 Then currentRunLength = 1
                dailyCostCurrentRun = currentRunCost / currentRunLength
            End If

            Dim diff As Decimal = dailyCostCurrentRun - dailyCostLastRun

            Return New DirectionChangeResponseDataModel() With {
                .Amount = dailyCostCurrentRun,
                .AmountChanged = Math.Abs(diff),
                .PositiveResult = (dailyCostCurrentRun < dailyCostLastRun),
                .Direction = If(dailyCostCurrentRun < dailyCostLastRun, Direction.Down, Direction.Up)
            }
        End Function

        Private Function CalculateTicketCost(ticket As DeliveryTicket) As Decimal
            Return ticket.LineItems.Select(Function(x) x.LineTotal).DefaultIfEmpty(0D).Sum()
        End Function
    End Class
End Namespace