Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class PartTemplateConfiguration
        Inherits EntityTypeConfiguration(Of PartTemplate)

        Public Sub New()
            [Property](Function(p) p.Cost).HasPrecision(10, 2)
            [Property](Function(p) p.Markup).HasPrecision(5, 4)
            [Property](Function(p) p.Discount).HasPrecision(5, 4)

            HasOptional(Function(x) x.RelatedAssembly).WithMany().HasForeignKey(Function(x) x.RelatedAssemblyID)
        End Sub
    End Class
End Namespace