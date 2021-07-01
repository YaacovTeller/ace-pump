Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class WellGridRowModel
        Public Property WellID As Integer

        <Required()>
        Public Property WellNumber As String
        Public Property APINumber As String
        Public Property APINumberRequired As Boolean
        Public Property IgnoreNoAPINumber As Boolean?

        <UIHint("Lease")>
        <AdditionalMetadata("LeaseNameProperty", "Lease")>
        Public Property LeaseID As Integer
        Public Property Lease As String

        <UIHint("Customer")>
        <AdditionalMetadata("CustomerNameProperty", "CustomerName")>
        Public Property CustomerID As Integer
        Public Property CustomerName As String

        Public Property Inactive As Boolean
    End Class
End Namespace