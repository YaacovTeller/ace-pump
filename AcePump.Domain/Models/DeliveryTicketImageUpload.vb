Namespace Models
    Public Class DeliveryTicketImageUpload
        Public Property DeliveryTicketImageUploadID As Integer

        Public Property DeliveryTicketID As Integer
        Public Overridable Property DeliveryTicket As DeliveryTicket

        Public Property LargeImageName As String
        Public Property SmallImageName As String

        Public Property MimeType As String

        Public Property UploadedOn As Date
        Public Property UploadedBy As String
        Public Property Note As String
    End Class
End Namespace