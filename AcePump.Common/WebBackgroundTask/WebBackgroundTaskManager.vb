Imports System.Collections.Concurrent

Namespace WebBackgroundTask
    Public Class WebBackgroundTaskManager
        Private Shared Property ActiveTasks As New ConcurrentDictionary(Of Guid, WebBackgroundTask)

        Public Shared Function GetProgress(id As Guid) As WebBackgroundTaskModel
            Dim info As WebBackgroundTask = Nothing
            If ActiveTasks.TryGetValue(id, info) Then
                Return New WebBackgroundTaskModel() With {
                    .Id = info.Id.ToString("N"),
                    .OperationsComplete = info.OperationsComplete,
                    .TotalOperations = info.TotalOperations,
                    .PercentComplete = info.PercentComplete
                }

            Else
                Return Nothing
            End If
        End Function

        Public Shared Function MonitorTask() As WebBackgroundTask
            Dim task As New WebBackgroundTask()

            ActiveTasks.TryAdd(task.Id, task)
            Return task
        End Function
    End Class
End Namespace