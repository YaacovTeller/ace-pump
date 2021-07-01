Imports System.Threading
Imports System.Threading.Tasks
Imports AcePump.SyncForQuickBooks.PtpApi
Imports AcePump.SyncForQuickBooks.UI.Models

Namespace UI.Tasks
    Public Class UICountReadyDeliveryticketsTask
        Implements IUITask

        Private _changeOnProgressList As List(Of UIChangeOnProgress) = Nothing
        Private _taskAction As Action = Nothing
        Private Shared _runningInvoices As New List(Of InvoiceUIModel)
        Private _reporter As UIProgressReporter
        Private _token As CancellationToken

        Public ReadOnly Property ChangeOnProgressList As List(Of UIChangeOnProgress) Implements IUITask.ChangeOnProgressList
            Get
                If _changeOnProgressList Is Nothing Then
                    _changeOnProgressList = New List(Of UIChangeOnProgress) From {
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.Start,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Operation Begun",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.Zero},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.Canceled,
                                             .OkButtonVisible = True,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Operation Cancelled",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.Maximum},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.Canceling,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Canceling....",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 0},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.Failure,
                                             .OkButtonVisible = True,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Operation Failed!",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.Zero},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.NoTicketsToProcess,
                                             .OkButtonVisible = True,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "No tickets ready!",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.Maximum},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.ConnectingToApi,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Connecting to Pump Tracking Programm online.",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 40},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.ReadingTicketsFromApi,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Reading tickets from Pump Tracking Programm.",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 40},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.Complete,
                                             .OkButtonVisible = True,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Operation Complete!",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.Maximum,
                                             .ProgressWeightOutOf100 = 10}
                }
                    Return _changeOnProgressList
                Else
                    Return _changeOnProgressList
                End If
            End Get
        End Property

        Public Sub New(reporter As UIProgressReporter, token As CancellationToken)
            _reporter = reporter
            _token = token
        End Sub

        Public ReadOnly Property TaskAction As Action Implements IUITask.TaskAction
            Get
                If _taskAction Is Nothing Then
                    _taskAction = Sub() CountReadyDeliveryTickets(_reporter, _token)
                    Return _taskAction
                Else
                    Return _taskAction
                End If
            End Get
        End Property

        Public Shared Sub CountReadyDeliveryTickets(reporter As UIProgressReporter, token As CancellationToken)
            reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.Start})

            Dim readyTickets As Integer

            Dim downloadTask As Task = Task.Factory.StartNew(Sub() readyTickets = ApiConnector.CountReadyDeliverytickets(reporter, token), token)
            downloadTask.Wait()

            If readyTickets = 0 Then
                reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.NoTicketsToProcess})

            Else
                reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.Complete})

                MessageBox.Show("There are " & readyTickets & " tickets ready to process.")
            End If
        End Sub
    End Class
End Namespace
