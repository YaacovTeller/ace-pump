'' TEMPORARY CP PATCH ''
Imports System.ComponentModel.DataAnnotations
'' END TEMPORARY CP PATCH ''

Namespace Areas.Customers.Models
    Public Class LineItemsGridRowViewModel
        Public Property DeliveryTicketID As Integer
        Public Property LineItemID As Integer
        Public Property PartTemplateID As Integer
        '' TEMPORARY CP PATCH (only the <UIHint>)''
        <UIHint("Integer")> _
        Public Property Quantity As Decimal
        Public Property ItemNumber As String
        Public Property Description As String
        Public Property Sort As Integer
    End Class
End Namespace

