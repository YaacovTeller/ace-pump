Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class LeaseGridRowModel
        Public Property LeaseID As Integer

        <Required()> _
        Public Property LocationName As String
    End Class
End Namespace