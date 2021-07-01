Namespace Models
    Public Interface IRuntime
        Property Start As Date?
        Property Finish As Date?
        Property LengthInDays As Integer?

        Property StartedByID As Integer?
        Property EndedByID As Integer?
    End Interface
End Namespace