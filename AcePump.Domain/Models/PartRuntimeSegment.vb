Namespace Models
    Public Class PartRuntimeSegment
        Implements IRuntime

        Public Property PartRuntimeSegmentID As Integer

        Public Property RuntimeID As Integer
        Public Overridable Property Runtime As PartRuntime

        Public Property Start As Date? Implements IRuntime.Start
        Public Property Finish As Date? Implements IRuntime.Finish
        Public Property LengthInDays As Integer? Implements IRuntime.LengthInDays

        Public Property SegmentStartedByTicketID As Integer? Implements IRuntime.StartedByID
        ''' <summary>
        ''' The delivery ticket which was issued when this segment started.
        ''' </summary>
        Public Overridable Property SegmentStartedByTicket As DeliveryTicket

        Public Property SegmentEndedByTicketID As Integer? Implements IRuntime.EndedByID
        ''' <summary>
        ''' The delivery ticket which was issued when this segment ended.
        ''' </summary>
        Public Overridable Property SegmentEndedByTicket As DeliveryTicket
    End Class
End Namespace