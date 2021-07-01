Namespace Models
    Public Class PumpRuntime
        Implements IRuntime

        Public Property PumpRuntimeID As Integer

        Public Property PumpID As Integer
        Public Overridable Property Pump As Pump

        Public Property Start As Date? Implements IRuntime.Start
        Public Property Finish As Date? Implements IRuntime.Finish
        Public Property LengthInDays As Integer? Implements IRuntime.LengthInDays

        Public Property RuntimeStartedByTicketID As Integer? Implements IRuntime.StartedByID
        ''' <summary>
        ''' The delivery ticket which was issued when this runtime started.
        ''' </summary>
        Public Overridable Property RuntimeStartedByTicket As DeliveryTicket

        Public Property RuntimeEndedByTicketID As Integer? Implements IRuntime.EndedByID
        ''' <summary>
        ''' The delivery ticket which was issued when this runtime ended.
        ''' </summary>
        Public Overridable Property RuntimeEndedByTicket As DeliveryTicket
    End Class
End Namespace