Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class WellModel
        Public Property WellID As Integer

        <Required()> _
        <MaxLength(20)> _
        Public Property WellNumber As String
        Public Property APINumber As String
        Public Property APINumberRequired As Boolean
        Public Property IgnoreNoAPINumber As Boolean?

        Public Property CustomerID As Integer
        Public Property CustomerName As String

        Public Property LeaseID As Integer
        Public Property LeaseName As String

        Public Property Inactive As Boolean
    End Class
End Namespace