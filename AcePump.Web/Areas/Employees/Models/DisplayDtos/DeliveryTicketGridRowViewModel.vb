Namespace Areas.Employees.Models.DisplayDtos
    Public Class DeliveryTicketGridRowViewModel
        Public Property DeliveryTicketID As Integer
        Public Property TicketDate As Date?
        Public Property WellNumber As String
        Public Property CustomerID As Integer?
        Public Property LocationName As String
        Public Property CustomerName As String
        Public Property IsClosed As Boolean
        Public Property IsSigned As Boolean
        Public Property ReasonStillOpen As String
        Public Property Quote As Boolean
        Public Property IsDownloadedToQb As Boolean
        Public Property IsReadyForQb As Boolean
        Public Property InvoiceStatus As Integer
    End Class
End Namespace