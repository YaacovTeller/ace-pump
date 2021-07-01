Imports AcePump.Domain.Models

Namespace BL
    Public Class LineItemService
        Private Property LineItems As IQueryable(Of LineItem)

        Public Sub New(lineItems As IQueryable(Of LineItem))
            Me.LineItems = lineItems
        End Sub

        Public Function GetExtendedInfo() As IQueryable(Of LineItemInfoExtended)
            Dim withUnitPrice = From lineItem In LineItems
                                Select New With {
                                    .LineItem = lineItem,
                                    .UnitPriceAfterDiscount = If(lineItem.CustomerDiscount.HasValue,
                                                                 lineItem.UnitPrice * (1 - If(lineItem.CustomerDiscount.HasValue, lineItem.CustomerDiscount.Value, 0D)),
                                                                 lineItem.UnitPrice * (1 - lineItem.UnitDiscount))
                                }

            Return From info In withUnitPrice
                   Select New LineItemInfoExtended With {
                       .LineItem = info.LineItem,
                       .UnitPriceAfterDiscount = info.UnitPriceAfterDiscount,
                       .LineTotal = info.UnitPriceAfterDiscount * info.LineItem.Quantity,
                       .SalesTaxAmount = info.UnitPriceAfterDiscount * info.LineItem.Quantity * info.LineItem.DeliveryTicket.SalesTaxRate
                   }
        End Function
    End Class
End Namespace