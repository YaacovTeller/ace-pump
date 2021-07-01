Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class DeliveryTicketImageUploadGridRow
        Public Property DeliveryTicketImageUploadID As Integer
        Public Property LargeImagePath As String
        Public Property SmallImagePath As String
        Public Property UploadedOn As Date
        Public Property UploadedBy As String
        Public Property Note As String
    End Class
End Namespace