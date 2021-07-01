Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class CountySalesTaxRateViewModel
        Public Property CountySalesTaxRateID As Integer
        Public Property CountyName As String
        <DisplayFormat(DataFormatString:="{0:p3}")> _
        <UIHint("Percent3Decimals")> _
        Public Property SalesTaxRate As Decimal
    End Class
End Namespace