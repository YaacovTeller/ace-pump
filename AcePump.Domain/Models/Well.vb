Namespace Models
    Public Class Well
        Public Property WellID As Integer

        Public Property LeaseID As Integer
        Public Overridable Property Lease As Lease

        Public Property CustomerID As Integer?
        Public Overridable Property Customer As Customer

        Public Overridable Property DeliveryTickets As ICollection(Of DeliveryTicket)

        Public Property WellNumber As String

        Public Property Inactive As Boolean

        Public Property APINumber As String
    End Class
End Namespace