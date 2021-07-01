Namespace Models
    Public Class QbInvoice
        Public Property CustomerRefListID As String
        Public Property PONumber As String
        Public Property FOB As String
        Public Property Other As String
        Public Property ItemSalesTaxRefListID As String
        Public Property DueDate As Date?
        Public Property ShipDate As Date?
        Public Property ClassRefFullName As String
        Public Property CustomFieldList As Dictionary(Of String, String)
        Public Property AdditionalLineList As List(Of Tuple(Of String, String))
        Public Property LineItems As New List(Of QbLineItem)
        Public Property TxnID As String
        Public Property InvoiceRefNumber As String
        Public Property AddedToQuickbooks As Boolean
        Public Property ErrorStatusMessages As New List(Of String)

        Public Property LastRequestID As Object
        Public Property EditSequence As String
        Public Property TxnDate As Date
        Public Property SubTotal As Decimal
        Public Property Tax As Decimal

        Public Property DataExtLeaseAndWell As String
    End Class
End Namespace