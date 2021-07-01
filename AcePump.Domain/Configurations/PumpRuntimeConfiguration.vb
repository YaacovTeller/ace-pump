Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class PumpRuntimeConfiguration
        Inherits EntityTypeConfiguration(Of PumpRuntime)

        Public Sub New()
            HasOptional(Function(x) x.RuntimeEndedByTicket).WithMany().HasForeignKey(Function(x) x.RuntimeEndedByTicketID).WillCascadeOnDelete(False)
            HasOptional(Function(x) x.RuntimeStartedByTicket).WithMany().HasForeignKey(Function(x) x.RuntimeStartedByTicketID).WillCascadeOnDelete(False)
        End Sub
    End Class
End Namespace
