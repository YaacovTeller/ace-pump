Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Models
    Public Class AcePumpProfileModel
        Public Property UserID As Integer

        <UIHint("_CustomerID")> _
        <DisplayName("Customer")> _
        Public Property CustomerID As Integer?
    End Class
End Namespace