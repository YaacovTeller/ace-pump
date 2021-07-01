Imports AcePump.Domain.DataSource
Imports AcePump.Web.WidgetQueries.Models
Imports Yesod.Widgets.Queries
Imports Yesod.Widgets.Models

Namespace WidgetQueries
    <SupportedWidgetId("AvgDailyRepairCost")> _
    <SupportedWidgetId("AvgDailyPumpCost")> _
    Public Class AverageDailyRepairCost
        Inherits AcePumpDirectionChangeQueryBase(Of Decimal)

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Function GetValue(query As AcePumpQueryModel) As Decimal
            Dim totalDays As Integer = (query.EndDate - query.StartDate).TotalDays + 1
            If totalDays <= 0 Then
                Return 0D
            End If

            Dim cost As IQueryable(Of TicketLineItemCost)
            If query.AdditionalParameters("includeNonPumpCost") Then
                cost = Common.TicketLineItemCost(query)
            Else
                cost = Common.TicketInspectionCost(query)
            End If

            Dim totalCost As Decimal = cost.Select(Function(x) x.Cost).DefaultIfEmpty(0D).Sum()
            Dim avgCost As Decimal = totalCost / totalDays

            Return avgCost
        End Function

        Protected Overrides Function BuildResult(oldValue As Decimal, newValue As Decimal) As Yesod.Widgets.Models.DirectionChangeResponseDataModel
            Dim diff As Decimal = newValue - oldValue

            Return New DirectionChangeResponseDataModel() With {
                .Amount = newValue,
                .AmountChanged = Math.Abs(diff),
                .PositiveResult = (newValue < oldValue),
                .Direction = If(newValue < oldValue, Direction.Down, Direction.Up)
            }
        End Function
    End Class
End Namespace