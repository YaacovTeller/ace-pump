Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("AveragePartRuntime")> _
    Public Class AveragePartRuntime
        Inherits AcePumpDirectionChangeQueryBase(Of Integer)

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Function GetValue(query As AcePumpQueryModel) As Integer
            Dim runtimes As IQueryable(Of PartRuntime) = Common.PartRuntimesByRuntime(query)
            Dim avg As Double = Common.CalculateAverage(runtimes)

            Return Convert.ToInt32(Math.Round(avg))
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