Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class PumpTemplateConfiguration
        Inherits EntityTypeConfiguration(Of PumpTemplate)

        Public Sub New()
            [Property](Function(p) p.Barrel.Length).HasColumnName("BarrelLength")
            [Property](Function(p) p.Barrel.Type).HasColumnName("BarrelType")
            [Property](Function(p) p.Barrel.Material).HasColumnName("BarrelMaterial")
            [Property](Function(p) p.Barrel.Washer).HasColumnName("BarrelWasher")

            [Property](Function(p) p.Plunger.Length).HasColumnName("PlungerLength")
            [Property](Function(p) p.Plunger.Material).HasColumnName("PlungerMaterial")
            [Property](Function(p) p.Plunger.Fit).HasColumnName("PlungerFit")

            [Property](Function(p) p.Seating.Location).HasColumnName("SeatingLocation")
            [Property](Function(p) p.Seating.Type).HasColumnName("SeatingType")
        End Sub
    End Class
End Namespace