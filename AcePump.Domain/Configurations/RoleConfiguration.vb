Imports System.Data.Entity.ModelConfiguration
Imports Yesod.LinqProviders

Namespace Configurations
    Public Class RoleConfiguration
        Inherits EntityTypeConfiguration(Of AccountDataStoreRole)

        Public Sub New()
            ToTable("Roles")

            HasMany(Function(x) x.Users) _
                .WithMany(Function(x) x.Roles) _
                .Map(Sub(config)
                         config.ToTable("UserRoles")
                         config.MapLeftKey("RoleID")
                         config.MapRightKey("UserID")
                     End Sub)
        End Sub
    End Class
End Namespace