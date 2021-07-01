Imports System.ComponentModel.DataAnnotations.Schema
Imports DelegateDecompiler

Namespace Models
    Public Class LineItem
        Public Property LineItemID As Integer

        Public Property DeliveryTicketID As Integer?
        Public Overridable Property DeliveryTicket As DeliveryTicket

        Public Property PartTemplateID As Integer?
        Public Overridable Property PartTemplate As PartTemplate

        Public Property PartInspectionID As Integer?
        Public Overridable Property PartInspection As PartInspection

        Public Property SortOrder As Integer?

        Public Property CollectSalesTax As Boolean?
        Public Property Quantity As Decimal
        Public Property Description As String
        Public Property UnitPrice As Decimal
        Public Property UnitDiscount As Decimal

        Public Property CustomerDiscount As Decimal?

        ''' <summary>
        ''' The per unit price after discount is applied for this line.
        ''' </summary>
        <NotMapped()> _
        <Computed()> _
        Public ReadOnly Property UnitPriceAfterDiscount As Decimal
            Get
                Return Math.Round(
                    If(CustomerDiscount.HasValue,
                            UnitPrice * (1 - CustomerDiscount.Value),
                            UnitPrice * (1 - UnitDiscount)),
                    2
                )
            End Get
        End Property

        ''' <summary>
        ''' The amount charged in sales tax for the line.
        ''' </summary>
        <NotMapped()> _
        <Computed()> _
        Public ReadOnly Property SalesTaxAmount As Decimal
            Get
                Return Math.Round(
                    Quantity _
                            * UnitPriceAfterDiscount _
                            * DeliveryTicket.SalesTaxRate,
                    2
                )
            End Get
        End Property

        ''' <summary>
        ''' The total cost of the line after discount.  EXCLUDES tax.
        ''' </summary>
        <NotMapped()> _
        Public ReadOnly Property LineTotal As Decimal
            Get
                Return Math.Round(
                    Quantity * UnitPriceAfterDiscount,
                    2
                )
            End Get
        End Property
    End Class
End Namespace