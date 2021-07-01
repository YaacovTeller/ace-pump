Imports AcePump.Domain.Models
Imports System.Data.Entity
Imports AcePump.Domain.DataSource

Namespace BL.Runtimes
    Friend Class PartRuntimeSegmentManager
        Inherits RuntimeManagerBase(Of PartRuntimeSegment)

        Private Property RuntimeID As Integer

        Protected Overrides ReadOnly Property RuntimeSet As IDbSet(Of PartRuntimeSegment)
            Get
                Return DataSource.PartRuntimeSegments
            End Get
        End Property

        Public Sub New(runtime As PartRuntimeSegment, dataSource As AcePumpContext)
            MyBase.New(runtime, dataSource)
        End Sub

        Public Overrides Sub CreateIfNotExists()
            If Not Exists() Then
                If RuntimeID = 0 Then
                    Throw New InvalidOperationException("Cannot create a PartRuntimeSegment. Invalid RuntimeID!")
                End If

                Runtime = New PartRuntimeSegment With {
                    .RuntimeID = RuntimeID
                }

                DataSource.PartRuntimeSegments.Add(Runtime)
            End If
        End Sub

        Protected Overrides Function FindAdjacents() As AdjacentRuntimes
            Dim adjacents As New AdjacentRuntimes()
            If Not Runtime.Finish.HasValue Then
                adjacents.After = DataSource.PartRuntimeSegments.FirstOrDefault(Function(x) _
                                                                             x.RuntimeID = Runtime.RuntimeID _
                                                                             And x.PartRuntimeSegmentID <> Runtime.PartRuntimeSegmentID _
                                                                             And Not x.Start.HasValue)
            End If

            If Not Runtime.Start.HasValue Then
                adjacents.Before = DataSource.PartRuntimeSegments.FirstOrDefault(Function(x) _
                                                                             x.RuntimeID = Runtime.RuntimeID _
                                                                             And x.PartRuntimeSegmentID <> Runtime.PartRuntimeSegmentID _
                                                                             And Not x.Finish.HasValue)
            End If

            Return adjacents
        End Function

        Protected Overrides Function CreateNewRuntimeBasedOn(basedOn As PartRuntimeSegment) As PartRuntimeSegment
            Return New PartRuntimeSegment() With {
                .RuntimeID = basedOn.RuntimeID
            }
        End Function
    End Class
End Namespace