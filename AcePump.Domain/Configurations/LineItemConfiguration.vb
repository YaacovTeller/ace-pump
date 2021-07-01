Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class LineItemConfiguration
        Inherits EntityTypeConfiguration(Of LineItem)

        Public Sub New()
            HasOptional(Function(x) x.PartInspection).WithMany() _
                .HasForeignKey(Function(x) x.PartInspectionID) _
                .WillCascadeOnDelete(False)

            [Property](Function(x) x.CustomerDiscount).HasPrecision(7, 4)
            [Property](Function(x) x.UnitDiscount).HasPrecision(5, 4)
            [Property](Function(x) x.UnitPrice).HasPrecision(18, 2)
            [Property](Function(x) x.Quantity).HasPrecision(18, 2)

        End Sub
    End Class
End Namespace