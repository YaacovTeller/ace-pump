Namespace Areas.Employees.Models.DisplayDtos
    Public Class DeliveryTicketSignatureLineItemGridRowModel
        Public Property Quantity As Decimal
        Public Property Item As String

        Public Property UnitPrice As Decimal
        Public Property LineIsTaxable As Boolean
        Public ReadOnly Property LineTotal As Decimal
            Get
                Return UnitPrice * Quantity
            End Get
        End Property
    End Class
End Namespace
