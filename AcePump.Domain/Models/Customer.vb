Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class Customer
        Public Property CustomerID As Integer

        Public Property QuickbooksID As String

        Public Overridable Property DeliveryTickets As ICollection(Of DeliveryTicket)
        Public Overridable Property Wells As ICollection(Of Well)

        Public Overridable Property PartSpecials As ICollection(Of CustomerPartSpecial)

        Public Property APINumberRequired As Boolean

        Public Property DefaultSalesTaxRate As Decimal?

        Public Property CountySalesTaxRateID As Integer?
        Public Overridable Property CountySalesTaxRate As CountySalesTaxRate

        Public Property UsesQuickbooksRunningInvoice As Boolean

        Public Property QbInvoiceClassID As Integer
        Public Overridable Property QbInvoiceClass As QbInvoiceClass

        Public Property UsesInventory As Boolean
        Public Property PayUpFront As Boolean?

        Public Property CustomerName As String
        Public Property Address1 As String
        Public Property Address2 As String
        Public Property City As String
        Public Property State As String
        Public Property Zip As String
        Public Property Website As String
        Public Property Phone As String
    End Class
End Namespace