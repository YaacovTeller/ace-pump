Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class CustomerViewModel
        Public Property CustomerID As Integer
        Public Property CustomerName As String
        Public Property Address1 As String
        Public Property Address2 As String
        Public Property City As String
        Public Property State As String
        Public Property Zip As String
        Public Property Phone As String
        Public Property Website As String
        Public Property APINumberRequired As Boolean

        <Required()> _
        Public Property CountySalesTaxRateID As Integer?
        Public Property CountyName As String

        <Required()> _
        Public Property UsesQuickbooksRunningInvoice As Boolean

        <Required()> _
        Public Property QbInvoiceClassID As Integer
        Public Property QbInvoiceClassName As String
        Public Property UsesInventory As Boolean
        Public Property PayUpFront As Boolean
    End Class
End Namespace