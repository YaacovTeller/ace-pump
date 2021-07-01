Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class PumpDisplayDto
        Public Property PumpID As Integer
        <Required(ErrorMessage:="Both Shop and Pump Number fields are required.")>
        <RegularExpression("^[0-9]*$", ErrorMessage:="Pump Number must be a number")>
        Public Property PumpNumber As String
        Public Property PumpNumberInputMode As String
        Public Property LeaseID As Integer?
        Public Property Lease As String
        Public Property InstalledInWellID As Integer?
        Public Property Well As String
        Public Property CustomerID As Integer?
        Public Property Customer As String

        <Required()>
        Public Property PumpTemplateID As Integer
        Public Property PumpTemplate As String
        <Required(ErrorMessage:="Shop Location is required.")>
        Public Property ShopLocationID As Integer
        Public Property ShopLocationPrefix As String
    End Class
End Namespace