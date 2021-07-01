Imports AcePump.Domain.Models
Imports System.Data.Entity
Imports AcePump.Domain.DataSource

Namespace BL.Runtimes
    Friend Class PumpRuntimeManager
        Inherits RuntimeManagerBase(Of PumpRuntime)

        Private Property PumpID As Integer

        Protected Overrides ReadOnly Property RuntimeSet As IDbSet(Of PumpRuntime)
            Get
                Return DataSource.PumpRuntimes
            End Get
        End Property

        Public Sub New(runtime As PumpRuntime, dataSource As AcePumpContext)
            MyBase.New(runtime, dataSource)
        End Sub

        Public Sub New(runtime As PumpRuntime, pumpId As Integer, dataSource As AcePumpContext)
            Me.New(runtime, dataSource)

            Me.PumpID = pumpId
        End Sub

        Public Overrides Sub CreateIfNotExists()
            If Not Exists() Then
                If PumpID = 0 Then
                    Throw New InvalidOperationException("Cannot create a PumpRuntime. Invalid PumpID!")
                End If

                Runtime = New PumpRuntime With {
                    .PumpID = PumpID
                }

                DataSource.PumpRuntimes.Add(Runtime)
            End If
        End Sub

        Protected Overrides Function CreateNewRuntimeBasedOn(basedOn As PumpRuntime) As PumpRuntime
            Return New PumpRuntime() With {
                .PumpID = basedOn.PumpID
            }
        End Function

        Protected Overrides Function FindAdjacents() As AdjacentRuntimes
            Dim adjacents As New AdjacentRuntimes()
            If Not Runtime.Finish.HasValue Then
                adjacents.After = DataSource.PumpRuntimes.FirstOrDefault(Function(x) _
                                                                             x.PumpID = Runtime.PumpID _
                                                                             And x.PumpRuntimeID <> Runtime.PumpRuntimeID _
                                                                             And Not x.Start.HasValue)
            End If

            If Not Runtime.Start.HasValue Then
                adjacents.Before = DataSource.PumpRuntimes.FirstOrDefault(Function(x) _
                                                                             x.PumpID = Runtime.PumpID _
                                                                             And x.PumpRuntimeID <> Runtime.PumpRuntimeID _
                                                                             And Not x.Finish.HasValue)
            End If

            Return adjacents
        End Function
    End Class
End Namespace