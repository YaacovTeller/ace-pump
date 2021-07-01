Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel

Namespace Areas.Employees.Models.DisplayDtos
    Public Class AssemblyModelBase
        Public Property AssemblyID As Integer

        Public Property Description As String

        <DisplayName("Assembly Number")> _
        Public Property AssemblyNumber As String

        <ForeignKey("PartCategoryID")> _
        <UIHint("PartCategory")> _
        Public Property Category As String

        <DisplayFormat(DataFormatString:="{0:p}")> _
        <UIHint("DecimalPercent")> _
        <Range(0.0, 0.999)> _
        Public Property Discount As Decimal

        <DisplayFormat(DataFormatString:="{0:p}")> _
        <UIHint("DecimalPercent")> _
        <Range(0.0, 0.999)> _
        Public Property Markup As Decimal

        <DisplayFormat(DataFormatString:="{0:c}")> _
        <DisplayName("Resale Price")> _
        Public Property ResalePrice As Decimal

        <DisplayFormat(DataFormatString:="{0:c}")> _
        <DisplayName("Total Parts Resale Price")> _
        Public Property TotalPartsResalePrice As Decimal

        <DisplayFormat(DataFormatString:="{0:c}")> _
        <DisplayName("Total Parts Cost")> _
        Public Property TotalPartsCost As Decimal
    End Class
End Namespace