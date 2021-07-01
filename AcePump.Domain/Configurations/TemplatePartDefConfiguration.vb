Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class TemplatePartDefConfiguration
        Inherits EntityTypeConfiguration(Of TemplatePartDef)

        Public Sub New()
            [Property](Function(t) t.PartTemplateID).IsOptional()
            [Property](Function(t) t.PumpTemplateID).IsOptional()
            [Property](Function(t) t.Quantity)
        End Sub
    End Class
End Namespace