Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class SoldByOptionConfiguration
        Inherits EntityTypeConfiguration(Of SoldByOption)

        Public Sub New()
            ToTable("Types_SoldByOption")
            [Property](Function(x) x.SoldByOptionID).HasColumnName("ItemTypeID")
            [Property](Function(x) x.Description).HasColumnName("DisplayText")
        End Sub
    End Class
End Namespace