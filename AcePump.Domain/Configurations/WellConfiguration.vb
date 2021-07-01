Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class WellConfiguration
        Inherits EntityTypeConfiguration(Of Well)

        Public Sub New()
            [Property](Function(w) w.LeaseID).IsOptional()
        End Sub
    End Class
End Namespace