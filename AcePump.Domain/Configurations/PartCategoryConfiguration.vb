Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class PartCategoryConfiguration
        Inherits EntityTypeConfiguration(Of PartCategory)

        Public Sub New()
        End Sub
    End Class
End Namespace