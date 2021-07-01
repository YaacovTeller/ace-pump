Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class PumpConfiguration
        Inherits EntityTypeConfiguration(Of Pump)

        Public Sub New()
            [Property](Function(p) p.PumpTemplateID).IsOptional()
            [HasOptional](Function(p) p.InstalledInWell).WithMany().HasForeignKey(Function(p) p.InstalledInWellID)
            [Property](Function(p) p.PumpNumber).HasMaxLength(300).HasUniqueIndexAnnotation("UQ_PumpNumberPerShopLocation", 0)
            [Property](Function(p) p.ShopLocationID).HasUniqueIndexAnnotation("UQ_PumpNumberPerShopLocation", 1)
        End Sub
    End Class
End Namespace