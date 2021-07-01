Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class CustomerPartSpecialConfiguration
        Inherits EntityTypeConfiguration(Of CustomerPartSpecial)

        Public Sub New()
            [Property](Function(x) x.Discount).HasPrecision(5, 4)
        End Sub
    End Class
End Namespace