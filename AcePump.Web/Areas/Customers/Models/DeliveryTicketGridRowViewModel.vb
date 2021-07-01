Namespace Areas.Customers.Models
    Public Class DeliveryTicketGridRowViewModel
        Public Property DeliveryTicketID As Integer
        Public Property TicketDate As Date?
        Public Property IsClosed As Boolean
        Public Property WellNumber As String
        Public Property Lease As String
        Public Property PumpFailedNumber As String
        Public Property PumpFailedDate As Date?
        Public Property PumpDispatchedNumber As String
        Public Property PumpDispatchedTemplateVerbose As String
        Public Property ShowDeliveryTicket As Boolean
        Public Property ShowRepairTicket As Boolean
    End Class
End Namespace