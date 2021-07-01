Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class PartRuntimeConfiguration
        Inherits EntityTypeConfiguration(Of PartRuntime)

        Public Sub New()
            HasOptional(Function(x) x.RuntimeStartedByTicket).WithMany().HasForeignKey(Function(x) x.RuntimeStartedByTicketID).WillCascadeOnDelete(False)
            HasOptional(Function(x) x.RuntimeEndedByInspection).WithMany().HasForeignKey(Function(x) x.RuntimeEndedByInspectionID).WillCascadeOnDelete(False)
        End Sub
    End Class
End Namespace