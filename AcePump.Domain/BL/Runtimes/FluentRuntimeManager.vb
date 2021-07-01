Namespace BL.Runtimes
    Public Class FluentRuntimeManager
        Private Property Manager As IRuntimeManager

        Friend Sub New(manager As IRuntimeManager)
            Me.Manager = manager
        End Sub

        ''' <summary>
        ''' Sets the IRuntime start date to startDate. Automatically splits the runtime into two
        ''' separate runtimes if it was previously set with a different start date from a different
        ''' event.
        ''' </summary>
        ''' <param name="startDate">The new start date to set on the runtime.</param>
        Function SetStartDate(startDate As Date, startedById As Integer) As FluentRuntimeManager
            Manager.SetStartDate(startDate, startedById)

            Return Me
        End Function

        ''' <summary>
        ''' Sets the IRuntime finish date to finishDate. Automatically splits the runtime
        ''' into two separate runtimes if it was previously set with a different finish date
        ''' from a different event.
        ''' </summary>
        Function SetEndDate(endDate As Date, endedById As Integer) As FluentRuntimeManager
            Manager.SetEndDate(endDate, endedById)

            Return Me
        End Function

        ''' <summary>
        ''' Clears any finish date set on the IRuntime.  Automatically joins this runtime with
        ''' the next runtime if there is no start date on the next runtime.
        ''' </summary>
        Function RemoveEndDate() As FluentRuntimeManager
            Manager.RemoveEndDate()

            Return Me
        End Function

        ''' <summary>
        ''' Clears any start date set on the IRuntime.  Automatically joins this runtime with
        ''' the previous runtime if there is no end date on the previous runtime.
        ''' </summary>
        Function RemoveStartDate() As FluentRuntimeManager
            Manager.RemoveStartDate()

            Return Me
        End Function

        ''' <summary>
        ''' Calculates the number of days that passed in this runtime.  Does not throw if there is
        ''' no start date or end date.
        ''' </summary>
        Function CalculateRuntimeLength() As FluentRuntimeManager
            Manager.CalculateRuntimeLength()

            Return Me
        End Function
    End Class
End Namespace