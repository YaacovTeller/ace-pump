Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class AssemblyConfiguration
        Inherits EntityTypeConfiguration(Of Assembly)

        Public Sub New()
            [Property](Function(x) x.Discount).HasPrecision(5, 4)
            [Property](Function(x) x.Markup).HasPrecision(5, 4)
        End Sub
    End Class
End Namespace