Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class LineItemsGridRowViewModel
        Public Property LineItemID As Integer
        Public Property DeliveryTicketID As Integer

        Public Property AddedFromRepairTicket As Boolean
        Public Property Quantity As Decimal

        <Required()>
        <Range(1, Integer.MaxValue, ErrorMessage:="You must enter a part")>
        <UIHint("PartTemplateNumber")>
        <AdditionalMetadata("PartTemplateNumberProperty", "PartTemplateNumber")>
        Public Property PartTemplateID As Integer
        Public Property PartTemplateNumber As String
        Public Property Description As String

        Public Property CollectSalesTax As Boolean?

        <UIHint("Currency")> _
        Public Property UnitPrice As Decimal

        <UIHint("DecimalPercent")> _
        <Range(0.0, 0.999)> _
        Public Property UnitDiscount As Decimal

        <UIHint("DecimalPercent")> _
        <Range(0.0, 0.999)> _
        Public Property CustomerDiscount As Decimal?

        Public Property UnitPriceAfterDiscount As Decimal
        Public Property LineTotal As Decimal
        Public Property SalesTaxAmount As Decimal?

        Public Property SortOrder As Integer?

        Public ReadOnly Property HasCustomerDiscount As Boolean
            Get
                Return CustomerDiscount.HasValue
            End Get
        End Property

    End Class
End Namespace

