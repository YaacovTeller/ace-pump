Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Models
    Public Class AcePumpProfileModel
        Public Property UserID As Integer

        <UIHint("_CustomerID")>
        <DisplayName("Customer")>
        Public Property CustomerID As Integer?

        <UIHint("_CustomerAccess")>
        <DisplayName("Customer Access List")>
        Public Property CustomerAccessList As Integer?
    End Class
End Namespace