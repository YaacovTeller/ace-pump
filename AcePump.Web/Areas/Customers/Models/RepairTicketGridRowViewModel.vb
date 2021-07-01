Namespace Areas.Customers.Models
    Public Class RepairTicketGridRowViewModel
        Public Property DeliveryTicketID As Integer
        Public Property PartInspectionID As Integer
        Public Property Quantity As Integer?
        Public Property OriginalPartTemplateNumber As String
        Public Property PartDescription As String
        Public Property Result As String
        Public Property ReplacementQuantity As Integer?
        Public Property ReplacementPartTemplateNumber As String
        Public Property ReasonRepaired As String
        Public Property SortOrder As Integer?
        Public Property OriginalCustomerOwnedPartID As Integer?
        Public Property ReplacedWithInventoryPartID As Integer?
        Public Property TemplatePartDefID As Integer
    End Class
End Namespace