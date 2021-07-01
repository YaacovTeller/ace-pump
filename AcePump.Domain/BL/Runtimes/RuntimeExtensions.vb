Imports System.Runtime.CompilerServices
Imports AcePump.Domain.Models
Imports AcePump.Domain.DataSource

Namespace BL.Runtimes
    Public Module RuntimeExtensions
        <Extension()>
        Public Function Manage(runtime As IRuntime, dataSource As AcePumpContext) As FluentRuntimeManager
            Dim runtimeManager As IRuntimeManager = RuntimeManagerFactory.GetManager(runtime, dataSource)
            Dim fluentManager As New FluentRuntimeManager(runtimeManager)

            Return fluentManager
        End Function
    End Module
End Namespace