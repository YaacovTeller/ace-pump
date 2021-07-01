Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Areas.Employees.Models.DisplayDtos
    Public Class CustomerPriceListGridRowUpdateModel

        Public Property PartTemplateID As Integer
        Public Property CustomerID As Integer

        <DisplayFormat(DataFormatString:="{0:p}")> _
        <UIHint("DecimalPercent")> _
        <Range(0.0, 0.999)> _
        Public Property Discount As Decimal

        <DisplayFormat(DataFormatString:="{0:p}")> _
        <UIHint("DecimalPercent")> _
        Public Property CustomerDiscount As Decimal?

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