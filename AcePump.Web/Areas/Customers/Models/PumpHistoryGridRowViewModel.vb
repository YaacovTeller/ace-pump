Namespace Areas.Customers.Models
    Public Class PumpHistoryGridRowViewModel
        Public Property PumpID As Integer
        Public Property ShopLocationPrefix As String
        Public Property PumpNumber As String
        Public Property DeliveryTicketID As Integer
        Public Property HistoryDate As Date?
        Public Property HistoryType As String
        Public Property ShowDeliveryTicket As Boolean
    End Class
End Namespace