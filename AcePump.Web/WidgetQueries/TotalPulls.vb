Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports Yesod.Widgets.Queries
Imports Yesod.Widgets.Models

Namespace WidgetQueries
    <SupportedWidgetId("TotalPulls")> _
    Public Class TotalPulls
        Inherits AcePumpDirectionChangeQueryBase(Of Integer)

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Function GetValue(query As AcePumpQueryModel) As Integer
            Dim tickets As IQueryable(Of DeliveryTicket) = Common.Tickets(query)
            Dim pulls = From ticket In tickets
                        Where ticket.PumpFailedID.HasValue

            Dim total As Integer = pulls.Count()
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