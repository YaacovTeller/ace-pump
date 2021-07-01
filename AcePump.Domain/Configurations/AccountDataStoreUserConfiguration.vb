Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models
Imports Yesod.LinqProviders

Namespace Configurations
    Public Class AccountDataStoreUserConfiguration
        Inherits EntityTypeConfiguration(Of AccountDataStoreUser)

        Public Sub New()
            ToTable("Users")
        End Sub
    End Class
End Namespace