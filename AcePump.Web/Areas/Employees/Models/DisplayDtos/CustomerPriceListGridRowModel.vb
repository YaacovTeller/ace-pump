Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Areas.Employees.Models.DisplayDtos
    Public Class CustomerPriceListGridRowModel

        Public Property PartTemplateID As Integer
        Public Property CustomerID As Integer

        Public Property PartTemplateNumber As String
        Public Property Description As String
        Public Property Active As Boolean

        <ForeignKey("PartCategoryID")> _
        <UIHint("PartCategory")> _
        Public Property Category As String

        <DisplayFormat(DataFormatString:="{0:c}")> _
        Public Property Cost As Decimal

        <DisplayFormat(DataFormatString:="{0:p}")> _
        <UIHint("DecimalPercent")> _
        <Range(0.0, 0.999)> _
        Public Property Discount As Decimal

        <DisplayFormat(DataFormatString:="{0:p}")> _
        <UIHint("DecimalPercent")> _
        <Range(0.0, 0.999)> _
        Public Property Markup As Decimal

        <DisplayFormat(DataFormatString:="{0:p}")> _
        <UIHint("DecimalPercent")> _
        Public Property CustomerDiscount As Decimal?

        <DisplayFormat(DataFormatString:="{0:c}")> _
        Public ReadOnly Property ResalePrice As Decimal
            Get
                Dim percentOfResaleWhichIsMarkup As Decimal = 1D - CDec(Markup)

                Return CDec(Cost) / percentOfResaleWhichIsMarkup
            End Get
        End Property

        <DisplayFormat(DataFormatString:="{0:c}")> _
        Public ReadOnly Property ListPrice As Decimal
            Get
                Dim percentOfListPriceWhichWillBeDiscounted As Decimal = 1D - If(CustomerDiscount.HasValue, CustomerDiscount, Discount)

                Return ResalePrice / percentOfListPriceWhichWillBeDiscounted
            End Get
        End Property

        <DisplayFormat(DataFormatString:="{0:p}")> _
        <UIHint("DecimalPercent")> _
        Public Property CurrentDiscount As Decimal?
            Get
                Return If(CustomerDiscount.HasValue, CustomerDiscount.Value, Discount)
            End Get

            Set(value As Decimal?)
                CustomerDiscount = value
            End Set
        End Property

        Public ReadOnly Property HasCustomerDiscount As Boolean
            Get
                Return CustomerDiscount.HasValue
            End Get
        End Property
    End Class
End Namespace