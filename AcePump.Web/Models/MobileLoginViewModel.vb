Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class MobileLoginViewModel
        <Required()> _
        <Display(Name:="User name")> _
        Public Property UserName As String

        <Required()> _
        <DataType(DataType.Password)> _
        <Display(Name:="Password")> _
        Public Property Password As String

        <Display(Name:="Remember me?")> _
        Public Property RememberMe As Boolean
    End Class
End Namespace