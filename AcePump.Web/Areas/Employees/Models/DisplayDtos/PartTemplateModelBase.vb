Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public MustInherit Class PartTemplateModelBase
        Public Property Number As String
        Public Property Description As String
        Public Property Active As Boolean
        Public Property Taxable As Boolean

        Public Property Manufacturer As String
        Public Property ManufacturerPartNumber As String


        <UIHint("SoldByOption")>
        Public Property SoldByOptionID As Integer
        Public Property SoldByOption As String

        <ForeignKey("PartCategoryID")>
        <UIHint("PartCategory")>
        Public Property Category As String

        <DisplayFormat(DataFormatString:="{0:c}")>
        <Range(0.0, 99999.99)>
        <UIHint("Cost")>
        Public Property Cost As Decimal

        <DisplayFormat(DataFormatString:="{0:p}")>
        <UIHint("DecimalPercent")>
        <Range(0.0, 0.999)>
        Public Property Discount As Decimal

        <DisplayFormat(DataFormatString:="{0:p}")>
        <UIHint("DecimalPercent")>
        <Range(0.0, 0.999)>
        Public Property Markup As Decimal

        <DisplayFormat(DataFormatString:="{0:c}")>
        Public Property ResalePrice As Decimal

        <DisplayFormat(DataFormatString:="{0:c}")>
        Public Property ListPrice As Decimal

        <DisplayFormat(DataFormatString:="{0:d}")>
        Public Property PriceLastUpdated As Date = Today
    End Class
End Namespace