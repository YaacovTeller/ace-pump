Imports AcePump.Domain.Models

Namespace BL.Runtimes
    Public Class ExistingRuntimeQuery
        Public Property PumpID As Integer
        Public Property TemplatePartDefID As Integer
        Public Property RuntimeIfExists As PartRuntime
        Public Property RuntimeSegmentsIfExist As IEnumerable(Of PartRuntimeSegment)
        Public Property SegmentIfExists As PartRuntimeSegment
    End Class
End Namespace