Imports AcePump.Domain.DataSource
Imports AcePump.Web.WidgetQueries.Models
Imports AcePump.Domain.Models
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("TotalSpent")> _
    <SupportedWidgetId("SpentOnPumpRepair")> _
    Public Class TotalSpent
        Inherits AcePumpDirectionChangeQueryBase(Of Decimal)

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Function GetValue(query As AcePumpQueryModel) As Decimal
            Dim queriedDeliveryTickets As IQueryable(Of TicketLineItemCost)
            If query.AdditionalParameters("includeNonPumpCost") Then
                queriedDeliveryTickets = Common.TicketLineItemCost(query)
            Else
                queriedDeliveryTickets = Common.TicketInspectionCost(query)
            End If

            Dim total As Decimal = queriedDeliveryTickets _
                                        .Select(Function(x) x.Cost) _
                                        .DefaultIfEmpty(0D) _
                                        .Sum()

            Return total
        End Function

        Protected Overrides Function BuildResult(oldValue As Decimal, newValue As Decimal) As DirectionChangeResponseDataModel
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