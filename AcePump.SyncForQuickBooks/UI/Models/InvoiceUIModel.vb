Namespace UI.Models
    Public Class InvoiceUIModel
        Public Property QbInvoiceNumber As String
        Public Property QbTransactionID As String
        Public Property CustomerQuickbooksID As String
        Public Property QbEditSequence As String
        Public Property QbTransactionDate As Date
        Public Property QbInvoiceLineItems As New List(Of InvoiceLineItemUIModel)
        Public Property PumpFailedNumber As String
        Public Property PumpDispatchedNumber As String
        Public Property LeaseAndWell As String
        Public Property SubTotal As Decimal
        Public Property Tax As Decimal
        Public Property QbInvoiceClassFullName As String
    End Class
End Namespace