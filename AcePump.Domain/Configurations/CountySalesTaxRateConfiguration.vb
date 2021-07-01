Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class CountySalesTaxRateConfiguration
        Inherits EntityTypeConfiguration(Of CountySalesTaxRate)

        Public Sub New()
            [Property](Function(x) x.SalesTaxRate).HasPrecision(6, 5)
        End Sub
    End Class
End Namespace