Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class PartInspectionConfiguration
        Inherits EntityTypeConfiguration(Of PartInspection)

        Public Sub New()
            [Property](Function(r) r.PartInspectionID).HasDatabaseGeneratedOption(ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)

            HasOptional(Function(p) p.PartFailed).WithMany().HasForeignKey(Function(p) p.PartFailedID)
            HasOptional(Function(p) p.PartReplaced).WithMany().HasForeignKey(Function(p) p.PartReplacedID)
            HasOptional(Function(p) p.ParentAssembly).WithMany().HasForeignKey(Function(p) p.ParentAssemblyID)
            HasOptional(Function(p) p.ReplacedWithInventoryPart).WithMany().HasForeignKey(Function(p) p.ReplacedWithInventoryPartID)
        End Sub
    End Class
End Namespace