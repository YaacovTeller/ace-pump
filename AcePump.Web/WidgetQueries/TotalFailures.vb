Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports Yesod.Widgets.Queries
Imports Yesod.Widgets.Models

Namespace WidgetQueries
    <SupportedWidgetId("TotalFailures")> _
    Public Class TotalFailures
        Inherits AcePumpDirectionChangeQueryBase(Of Integer)

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Function GetValue(query As AcePumpQueryModel) As Integer
            Dim inspections As IQueryable(Of PartInspection) = Common.FailedInspections(query)

            If query.AdditionalParameters.ContainsKey("PartTemplateID") Then
                inspections = inspections.Where(Function(i) i.PartFailedID.HasValue _
                                                    AndAlso i.PartFailedID.Value = query.PartTemplateID _
                                                    AndAlso i.ReasonRepaired <> "OK" _
                                                    AndAlso i.Result <> "OK")
            End If

            Return inspections.Count()
        End Function

        Protected Overrides Function BuildResult(oldValue As Integer, newValue As Integer) As DirectionChangeResponseDataModel
            Return New DirectionChangeResponseDataModel() With {
                .Amount = newValue,
                .AmountChanged = Math.Abs(newValue - oldValue),
                .Direction = If(newValue < oldValue, Direction.Down, Direction.Up),
                .PositiveResult = (newValue < oldValue)
            }
        End Function
    End Class
End Namespace