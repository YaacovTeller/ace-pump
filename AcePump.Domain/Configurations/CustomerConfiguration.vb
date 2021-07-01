Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class CustomerConfiguration
        Inherits EntityTypeConfiguration(Of Customer)

        Public Sub New()
            [Property](Function(x) x.DefaultSalesTaxRate).HasPrecision(6, 5)

            HasRequired(Function(x) x.QbInvoiceClass).WithMany(Function(x) x.Customers).HasForeignKey(Function(x) x.QbInvoiceClassID)
        End Sub
    End Class
End Namespace