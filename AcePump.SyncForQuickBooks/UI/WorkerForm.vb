Imports System.Threading
Imports System.Threading.Tasks
Imports System.Security.AccessControl
Imports System.IO
Imports AcePump.SyncForQuickBooks.UI.Tasks

Namespace UI
    Public Class WorkerForm
        Private _cancellationTokenSource As CancellationTokenSource
        Private _progressReporter As UIProgressReporter
        Private _processFailed As Boolean
        Private _processCompletedSuccessfully As Boolean
        Private _doneSoFar As Integer
        Private _task As IUITask
        Private _taskType As Type

        Public Sub New(taskType As Type)

            ' This call is required by the designer.
            InitializeComponent()

            _taskType = taskType
        End Sub
        Public Sub UpdateProgress(progress As UIProgressDto)
            Dim totalWeightOfAllStages As Integer = _task.ChangeOnProgressList.Sum(Function(x) x.ProgressWeightOutOf100)
            Dim buffer As Integer = totalWeightOfAllStages / 100 * 20
            totalWeightOfAllStages += buffer
            WorkerProgressBar.Maximum = totalWeightOfAllStages
            Dim currentUIChangeOnProgress = _task.ChangeOnProgressList.FirstOrDefault(Function(x) x.ProgressStage = progress.Stage)
            If currentUIChangeOnProgress IsNot Nothing Then
                OkButton.Visible = currentUIChangeOnProgress.OkButtonVisible
                ProgressCancelButton.Enabled = currentUIChangeOnProgress.ProgressCancelButtonEnabled
                StatusLabel.Text = currentUIChangeOnProgress.StatusLabelText

                Select Case currentUIChangeOnProgress.WorkerProgressBarValue
                    Case UIChangeProgressBarState.Zero
                        WorkerProgressBar.Value = 0
                    Case UIChangeProgressBarState.Maximum
                        WorkerProgressBar.Value = WorkerProgressBar.Maximum
                    Case UIChangeProgressBarState.AddPercentage
                        Dim currentWeight As Integer = currentUIChangeOnProgress.ProgressWeightOutOf100 / 100 * totalWeightOfAllStages

                        If progress.Data.Count > 0 Then
                            Dim totalTickets As Integer = DirectCast(progress.Data("TotalTickets"), Integer)
                            Dim upto As Integer = DirectCast(progress.Data("Upto"), Integer)
                            currentWeight = currentWeight / totalTickets * upto
                            StatusLabel.Text = String.Format(StatusLabel.Text & "{0} of {1} total.", upto, totalTickets)
                        End If

                        _doneSoFar = currentWeight + _doneSoFar
                        If _doneSoFar > WorkerProgressBar.Maximum Then
                            _doneSoFar = WorkerProgressBar.Maximum
                        End If
                        WorkerProgressBar.Value = _doneSoFar
                        'Debug.WriteLine("Stage: " & currentUIChangeOnProgress.StatusLabelText & _
                        '                " Weight: " & currentWeight & _
                        '                " doneSoFar: " & _doneSoFar & _
                        '                " Workerbar upto: " & WorkerProgressBar.Value)
                End Select
            End If
        End Sub

        Private Sub ProgressCancelButton_Click(sender As Object, e As System.EventArgs) Handles ProgressCancelButton.Click
            CancelDownloadWithForm()
        End Sub

        Private Function CancelDownloadWithForm()
            Using c As New CancelDialog
                Dim result As DialogResult = c.ShowDialog()
                If result = DialogResult.Cancel Then
                    _progressReporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.Canceling})
                    _cancellationTokenSource.Cancel()
                    Return True
                End If
            End Using
            Return False
        End Function

        Private Sub WorkerForm_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If _cancellationTokenSource.IsCancellationRequested Or _processFailed Or _processCompletedSuccessfully Then
                Return
            End If

            Dim operationWasCanceled As Boolean
            operationWasCanceled = CancelDownloadWithForm()

            If Not operationWasCanceled Then
                e.Cancel = True
            End If
        End Sub

        Private Sub StartBackgroundTask()
            _cancellationTokenSource = New CancellationTokenSource()
            Dim ct As CancellationToken = _cancellationTokenSource.Token

            _progressReporter = New UIProgressReporter()
            _progressReporter.PredifinedProgressAction = Sub(x) UpdateProgress(x)

            _task = Activator.CreateInstance(_taskType, _progressReporter, ct)


            Dim currentTask = Task.Factory.StartNew(_task.TaskAction, ct)

            _progressReporter.RegisterContinuation(currentTask, Sub()
                                                                    If currentTask.IsFaulted Then
                                                                        If currentTask.Exception.Flatten().InnerExceptions.Any(Function(x) TypeOf x Is TaskCanceledException) Then
                                                                            UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.Canceled})
                                                                        Else
                                                                            UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.Failure})
                                                                            _processFailed = True
                                                                            Dim path As String = SaveExceptionToFile(currentTask.Exception)
                                                                            Using exceptionForm As New ExceptionForm
                                                                                exceptionForm.SetExceptionTextAndPath("A fatal error occured, the operation could not be completed." & vbNewLine &
                                                                                                                                     "Please try again." & vbNewLine &
                                                                                                                                     "If this error continues to occur, please click on the following link to view the log file and send it to cases@sorissoftware.fogbugz.com.",
                                                                                                                                     path)
                                                                                exceptionForm.ShowDialog()
                                                                            End Using
                                                                        End If
                                                                    ElseIf currentTask.IsCanceled Then
                                                                        UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.Canceled})
                                                                    Else
                                                                        _processCompletedSuccessfully = True
                                                                    End If
                                                                End Sub,
                                                   CancellationToken.None)
        End Sub

        Private Function SaveExceptionToFile(ByVal ticketsException As AggregateException) As String
            Dim path As String = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SorisSoftware", "QB Integration", "ErrorLogs")
            Directory.CreateDirectory(path)
            If GrantAccess(path) Then
                path = System.IO.Path.Combine(path, "ErrorLog_" & Today.ToString("MM_dd_yyyy") & ".log")

                Using writer As New StreamWriter(path, True)
                    writer.WriteLine(ticketsException.ToString)
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine)
                End Using

                Return path
            Else
                Return "Could not get permission to write the file!"
            End If
        End Function

        Private Function GrantAccess(fullPath As String) As Boolean
            Dim dInfo As New DirectoryInfo(fullPath)
            Dim dSecurity As DirectorySecurity = dInfo.GetAccessControl()
            dSecurity.AddAccessRule(New FileSystemAccessRule("everyone", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit Or InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow))
            dInfo.SetAccessControl(dSecurity)
            Return True
        End Function

        Private Sub WorkerForm_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
            StartBackgroundTask()
        End Sub

        Private Sub OkButton_Click(sender As System.Object, e As System.EventArgs) Handles OkButton.Click
            Close()
        End Sub






    End Class

End Namespace