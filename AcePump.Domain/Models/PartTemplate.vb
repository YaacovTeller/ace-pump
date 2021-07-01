Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models
    Public Class PartTemplate
        Public Property PartTemplateID As Integer

        Public Property PartCategoryID As Integer?
        Public Overridable Property PartCategory As PartCategory

        Public Property RelatedAssemblyID As Integer?
        Public Overridable Property RelatedAssembly As Assembly

        Public Property SoldByOptionID As Integer?
        Public Overridable Property SoldByOption As SoldByOption

        Public Property MaterialID As Integer?
        Public Overridable Property Material As Material

        Public Overridable Property CustomersWithSpecials As ICollection(Of CustomerPartSpecial)

        Public Property Number As String

        Public Property QuickbooksID As String

        Public Property Description As String
        Public Property Active As Boolean
        Public Property Manufacturer As String
        Public Property ManufacturerPartNumber As String

        Public Property Cost As Decimal
        Public Property Markup As Decimal
        Public Property Discount As Decimal
        Public Property Taxable As Boolean

        Public Property PriceLastUpdated As DateTime

        ''' <summary>
        ''' The price after discount Ace Pump will actually sell the part for.  This is calculated based
        ''' on the cost and markup.
        ''' </summary>
        <NotMapped()>
        Public ReadOnly Property ResalePrice As Decimal
            Get
                Return Math.Round(
                    Cost _
                        / If(Markup > 0D, 1D - Markup, 1D),
                    2
                )
            End Get
        End Property

        ''' <summary>
        ''' The price Ace Pump lists as the "official" price.  This is calculated based on the resale and discount.
        ''' </summary>
        <NotMapped()>
        Public ReadOnly Property ListPrice As Decimal
            Get
                Return Math.Round(
                    Cost _
                        / If(Markup > 0D, 1D - Markup, 1D) _
                        / If(Discount > 0D, 1D - Discount, 1D),
                    2
                )
            End Get
        End Property
    End Class
End Namespace