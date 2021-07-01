Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class AcePumpProfileConfiguration
        Inherits EntityTypeConfiguration(Of AcePumpProfile)

        Public Sub New()
            HasKey(Function(x) x.UserID)
            HasRequired(Function(x) x.User).WithRequiredDependent().WillCascadeOnDelete(True)

            HasOptional(Function(x) x.Customer).WithMany().HasForeignKey(Function(x) x.CustomerID)

            HasMany(Function(x) x.CustomerAccess).WithMany().Map(Sub(map)
                                                                     map.ToTable("UsernameCustomerAccess")
                                                                     map.MapRightKey("CustomerID")
                                                                     map.MapLeftKey("UserID")
                                                                 End Sub)
        End Sub
    End Class
End Namespace