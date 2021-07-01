Namespace Models
    Public Class QbInvoiceClass
        Public Property QbInvoiceClassID As Integer
        Public Property FullName As String

        Public Overridable Property Customers As ICollection(Of Customer)
    End Class
End Namespace