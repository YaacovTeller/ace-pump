
Imports System.Threading
Imports System.Threading.Tasks
Imports AcePump.QbApi

Namespace UI

    ''' <summary> 
    ''' A class used by Tasks to report progress or completion updates back to the UI. 
    ''' </summary> 
    Public NotInheritable Class UIProgressReporter
        Implements QbApi.IProgressReporter

        ''' <summary> 
        ''' The underlying scheduler for the UI's synchronization context. 
        ''' </summary> 
        Private ReadOnly m_scheduler As TaskScheduler

        Public Property PredifinedProgressAction As Action(Of UIProgressDto)

        ''' <summary> 
        ''' Initializes a new instance of the <see cref="ProgressReporter"/> class.
        ''' This should be run on a UI thread. 
        ''' </summary> 
        Public Sub New()
            Me.m_scheduler = TaskScheduler.FromCurrentSynchronizationContext()
        End Sub

        ''' <summary> 
        ''' Gets the task scheduler which executes tasks on the UI thread. 
        ''' </summary> 
        Public ReadOnly Property Scheduler() As TaskScheduler
            Get
                Return Me.m_scheduler
            End Get
        End Property

        Public Function ReportPredefinedProgressAsync(progress As UIProgressDto) As Task
            Return ReportProgressAsync(Sub() PredifinedProgressAction.Invoke(progress))
        End Function

        Public Sub UpdateProgress(progress As UIProgressDto)
            ReportPredefinedProgressAsync(progress)
        End Sub

        ''' <summary> 
        ''' Reports the progress to the UI thread. This method should be called from the task.
        ''' Note that the progress update is asynchronous with respect to the reporting Task.
        ''' For a synchronous progress update, wait on the returned <see cref="Task"/>. 
        ''' </summary> 
        ''' <param name="action">The action to perform in the context of the UI thread.
        ''' Note that this action is run asynchronously on the UI thread.</param> 
        ''' <returns>The task queued to the UI thread.</returns> 
        Public Function ReportProgressAsync(action As Action) As Task
            Return Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, Me.m_scheduler)
        End Function

        ''' <summary> 
        ''' Registers a UI thread handler for when the specified task finishes execution,
        ''' whether it finishes with success, failiure, or cancellation. 
        ''' </summary> 
        ''' <param name="task">The task to monitor for completion.</param> 
        ''' <param name="action">The action to take when the task has completed, in the context of the UI thread.</param> 
        ''' <returns>The continuation created to handle completion. This is normally ignored.</returns> 
        Public Function RegisterContinuation(task As Task, action As Action, token As CancellationToken) As Task
            Return task.ContinueWith(Sub() action(), token, TaskContinuationOptions.None, Me.m_scheduler)
        End Function

        Public Sub Update(progress As ProgressDto) Implements IProgressReporter.Update
            Dim dto As New UIProgressDto With {.Data = progress.Data}
            Select Case progress.Stage
                Case QbProgressStage.Connecting
                    dto.Stage = UIProgressStage.ConnectingToQb
                Case QbProgressStage.ProcessingNewInvoices
                    dto.Stage = UIProgressStage.ProcessingNewInvoices
                Case QbProgressStage.ProcessingModifiedInvoices
                    dto.Stage = UIProgressStage.ProcessingModifiedInvoices
                Case QbProgressStage.ProcessingTicketsAdditionalData
                    dto.Stage = UIProgressStage.ProcessingTicketAdditionalData
                Case QbProgressStage.SavingTickets
                    dto.Stage = UIProgressStage.SavingTickets
                Case Else                                
                    Throw New ArgumentException("Unrecognized stage: " & progress.Stage)
            End Select
            UpdateProgress(dto)
        End Sub
    End Class

End Namespace