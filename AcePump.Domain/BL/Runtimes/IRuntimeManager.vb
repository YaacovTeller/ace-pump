Namespace BL.Runtimes
    Public Interface IRuntimeManager
        ''' <summary>
        ''' Creates the runtime if it does not already exist and stores it in the DataSource.
        ''' Does not SaveChanges to the DataSource.
        ''' </summary>
        Sub CreateIfNotExists()
        
        ''' <summary>
        ''' Determines if a matching runitme exists in the DataSource.
        ''' </summary>
        Function Exists() As Boolean

        ''' <summary>
        ''' Sets the IRuntime start date to startDate. Automatically splits the runtime into two
        ''' separate runtimes if it was previously set with a different start date from a different
        ''' event.
        ''' </summary>
        ''' <param name="startDate">The new start date to set on the runtime.</param>
        Sub SetStartDate(startDate As Date, startedById As Integer)

        ''' <summary>
        ''' Sets the IRuntime finish date to finishDate. Automatically splits the runtime
        ''' into two separate runtimes if it was previously set with a different finish date
        ''' from a different event.
        ''' </summary>
        Sub SetEndDate(endDate As Date, endedById As Integer)

        ''' <summary>
        ''' Clears any finish date set on the IRuntime.  Automatically joins this runtime with
        ''' the next runtime if there is no start date on the next runtime.
        ''' </summary>
        Sub RemoveEndDate()

        ''' <summary>
        ''' Clears any start date set on the IRuntime.  Automatically joins this runtime with
        ''' the previous runtime if there is no end date on the previous runtime.
        ''' </summary>
        Sub RemoveStartDate()

        ''' <summary>
        ''' Calculates the number of days that passed in this runtime.  Does not throw if there is
        ''' no start date or end date.
        ''' </summary>
        Sub CalculateRuntimeLength()
    End Interface
End Namespace