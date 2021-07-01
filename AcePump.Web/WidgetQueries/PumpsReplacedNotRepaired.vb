Imports AcePump.Domain.DataSource
Imports AcePump.Web.WidgetQueries.Models
Imports AcePump.Domain.Models
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("PumpsReplacedNotRepaired")> _
    Public Class PumpsReplacedNotRepaired
        Inherits AcePumpDirectionChangeQueryBase(Of Integer)

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overrides Function GetValue(query As AcePumpQueryModel) As Integer
            Dim inspections As IQueryable(Of PartInspection) = Common.FailedInspections(query)
            Dim unbilledReplacements = From inspection In inspections
                                       Group Join lineItem In DataSource.LineItems On lineItem.PartInspectionID Equals inspection.PartInspectionID Into g = Group
                                       From nullableLineItem In g.DefaultIfEmpty()
                                       Where nullableLineItem Is Nothing And (inspection.Result = "Convert" Or inspection.Result = "Replace")

            Return unbilledReplacements.Select(Function(x) x.inspection.DeliveryTicketID).Distinct().Count()
        End Function

        Protected Overrides Function BuildResult(oldValue As Integer, newValue As Integer) As DirectionChangeResponseDataModel
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