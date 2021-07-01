Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class PartRuntimeSegmentConfiguration
        Inherits EntityTypeConfiguration(Of PartRuntimeSegment)

        Public Sub New()
            HasOptional(Function(x) x.SegmentStartedByTicket).WithMany().HasForeignKey(Function(x) x.SegmentStartedByTicketID).WillCascadeOnDelete(False)
            HasOptional(Function(x) x.SegmentEndedByTicket).WithMany().HasForeignKey(Function(x) x.SegmentEndedByTicketID).WillCascadeOnDelete(False)
            HasRequired(Function(x) x.Runtime).WithMany(Function(x) x.Segments).HasForeignKey(Function(x) x.RuntimeID).WillCascadeOnDelete(False)
        End Sub
    End Class
End Namespace