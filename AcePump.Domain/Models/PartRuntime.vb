Namespace Models
    Public Class PartRuntime
        Implements IRuntime

        Public Property PartRuntimeID As Integer

        Public Property PumpID As Integer
        Public Overridable Property Pump As Pump

        Public Property TemplatePartDefID As Integer
        Public Overridable Property TemplatePartDef As TemplatePartDef

        Public Overridable Property Segments As ICollection(Of PartRuntimeSegment)

        Public Property Start As Date? Implements IRuntime.Start
        Public Property Finish As Date? Implements IRuntime.Finish
        Public Property TotalDaysInGround As Integer? Implements IRuntime.LengthInDays

        Public Property RuntimeStartedByTicketID As Integer? Implements IRuntime.StartedByID
        ''' <summary>
        ''' The delivery ticket which was issued when this runtime started.
        ''' </summary>
        Public Overridable Property RuntimeStartedByTicket As DeliveryTicket

        Public Property RuntimeEndedByInspectionID As Integer? Implements IRuntime.EndedByID
        ''' <summary>
        ''' The part inspection where the part was found broken.  Ended the rutnime.
        ''' </summary>
        Public Overridable Property RuntimeEndedByInspection As PartInspection
    End Class
End Namespace