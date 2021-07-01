Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class DeliveryTicketSignatureViewModel
        Public Property DeliveryTicketID As Integer
        Public Property CustomerName As String
        Public Property Lease As String
        Public Property Well As String
        Public Property TicketDate As Date?
        Public Property CloseTicket As Boolean?

        Public Property LineItems As IEnumerable(Of DeliveryTicketSignatureLineItemGridRowModel)
        Public Property SalesTaxRate As Decimal

        <Required(errormessage:="The signature name field is required.")> _
        Public Property SignatureName As String
        Public Property SignatureDate As Date?
        Public Property SignatureCompanyName As String

        Public Property Signature As Byte()
        <Required(errormessage:="A signature is required.")> _
        Public Property SignatureBase64 As String
    End Class
End Namespace