Imports AcePump.Domain.Models
Imports AcePump.Domain.DataSource

Namespace BL.Runtimes
    Public Class RuntimeManagerFactory
        Public Shared Function GetManager(runtime As IRuntime, dataSource As AcePumpContext) As IRuntimeManager
            Dim asObject As Object = CObj(runtime)

            Dim runtimeType As Type = runtime.GetType()
            If GetType(PartRuntimeSegment).IsAssignableFrom(runtimeType) Then
                Return GetManager(DirectCast(asObject, PartRuntimeSegment), dataSource)
            ElseIf GetType(PartRuntime).IsAssignableFrom(runtimeType) Then
                Return GetManager(DirectCast(asObject, PartRuntime), dataSource)
            ElseIf GetType(PumpRuntime).IsAssignableFrom(runtimeType) Then
                Return GetManager(DirectCast(asObject, PumpRuntime), dataSource)
            Else
                Throw New ArgumentException("unrecognized TRuntime")
            End If
        End Function

        Public Shared Function GetManager(runtime As PartRuntime, dataSource As AcePumpContext) As IRuntimeManager
            Return New PartRuntimeManager(runtime, dataSource)
        End Function

        Public Shared Function GetManager(runtime As PartRuntime, pumpId As Integer, templatePartDefId As Integer, dataSource As AcePumpContext) As IRuntimeManager
            Return New PartRuntimeManager(runtime, pumpId, templatePartDefId, dataSource)
        End Function

        Public Shared Function GetManager(runtime As PumpRuntime, dataSource As AcePumpContext) As IRuntimeManager
            Return New PumpRuntimeManager(runtime, dataSource)
        End Function

        Public Shared Function GetManager(runtime As PumpRuntime, pumpId As Integer, dataSource As AcePumpContext) As IRuntimeManager
            Return New PumpRuntimeManager(runtime, pumpId, dataSource)
        End Function

        Public Shared Function GetManager(runtime As PartRuntimeSegment, dataSource As AcePumpContext) As IRuntimeManager
            Return New PartRuntimeSegmentManager(runtime, dataSource)
        End Function
    End Class
End Namespace