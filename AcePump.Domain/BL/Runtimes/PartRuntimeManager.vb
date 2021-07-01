Imports AcePump.Domain.Models
Imports System.Data.Entity
Imports AcePump.Domain.DataSource

Namespace BL.Runtimes
    Friend Class PartRuntimeManager
        Inherits RuntimeManagerBase(Of PartRuntime)

        Private Property PumpID As Integer
        Private Property TemplatePartDefID As Integer

        Protected Overrides ReadOnly Property RuntimeSet As IDbSet(Of PartRuntime)
            Get
                Return DataSource.PartRuntimes
            End Get
        End Property

        Public Sub New(runtime As PartRuntime, dataSource As AcePumpContext)
            MyBase.New(runtime, dataSource)
        End Sub

        Public Sub New(runtime As PartRuntime, pumpId As Integer, templatePartDefId As Integer, dataSource As AcePumpContext)
            Me.New(runtime, dataSource)

            Me.PumpID = pumpId
            Me.TemplatePartDefID = templatePartDefId
        End Sub

        Public Overrides Sub CreateIfNotExists()
            If Not Exists() Then
                If PumpID = 0 Or TemplatePartDefID = 0 Then
                    Throw New InvalidOperationException("Cannot create a PartRuntime. Invalid valid PumpID or TemplatePartDefID!")
                End If

                Runtime = New PartRuntime With {
                    .PumpID = PumpID,
                    .TemplatePartDefID = TemplatePartDefID
                }

                DataSource.PartRuntimes.Add(Runtime)
            End If
        End Sub

        Protected Overrides Sub CompleteMerge(merged As PartRuntime, toDelete As PartRuntime)
            If toDelete.Segments.Count > 0 Then
                For Each segment As PartRuntimeSegment In toDelete.Segments
                    segment.Runtime = merged
                Next
            End If

            MyBase.CompleteMerge(merged, toDelete)
        End Sub

        Protected Overrides Sub CompleteSplit(original As PartRuntime, toAdd As PartRuntime)
            If original.Segments.Count > 0 Then
                For Each segment As PartRuntimeSegment In original.Segments.ToList()
                    Dim segmentStartsBeforeOriginal As Boolean = original.Start.HasValue AndAlso segment.Start.HasValue AndAlso original.Start.Value > segment.Start.Value
                    Dim segmentEndsAfterOriginal As Boolean = original.Finish.HasValue AndAlso segment.Finish.HasValue AndAlso original.Finish.Value < segment.Finish.Value
                    Dim segmentBelongsToAdded As Boolean = segmentEndsAfterOriginal Or segmentStartsBeforeOriginal

                    If segmentBelongsToAdded Then
                        segment.Runtime = toAdd
                    End If
                Next
            End If

            MyBase.CompleteSplit(original, toAdd)
        End Sub

        Protected Overrides Function CreateNewRuntimeBasedOn(basedOn As PartRuntime) As PartRuntime
            Return New PartRuntime() With {
                .Segments = New List(Of PartRuntimeSegment),
                .TemplatePartDefID = basedOn.TemplatePartDefID,
                .PumpID = basedOn.PumpID
            }
        End Function

        Protected Overrides Function FindAdjacents() As AdjacentRuntimes
            Dim adjacents As New AdjacentRuntimes()
            If Not Runtime.Finish.HasValue Then
                adjacents.After = DataSource.PartRuntimes.FirstOrDefault(Function(x) _
                                                                             x.PumpID = Runtime.PumpID _
                                                                             And x.TemplatePartDefID = Runtime.TemplatePartDefID _
                                                                             And x.PartRuntimeID <> Runtime.PartRuntimeID _
                                                                             And Not x.Start.HasValue)
            End If

            If Not Runtime.Start.HasValue Then
                adjacents.Before = DataSource.PartRuntimes.FirstOrDefault(Function(x) _
                                                                             x.PumpID = Runtime.PumpID _
                                                                             And x.TemplatePartDefID = Runtime.TemplatePartDefID _
                                                                             And x.PartRuntimeID <> Runtime.PartRuntimeID _
                                                                             And Not x.Finish.HasValue)
            End If

            Return adjacents
        End Function

        Public Overrides Sub CalculateRuntimeLength()
            If Runtime.Start.HasValue And Runtime.Finish.HasValue Then
                Dim runtimesToAccumulate As List(Of PartRuntimeSegment) =
                (From r In DataSource.PartRuntimeSegments
                 Where r.RuntimeID = Runtime.PartRuntimeID) _
                .ToList()

                Dim accumulatedDays As Integer = 0
                For Each r As PartRuntimeSegment In runtimesToAccumulate
                    Dim start As Date = r.Start.Value
                    Dim [end] As Date = If(r.Finish.HasValue, r.Finish.Value, Today)

                    accumulatedDays += ([end] - start).TotalDays
                Next

                Runtime.TotalDaysInGround = accumulatedDays
                Runtime.Start = If(runtimesToAccumulate.Any(), runtimesToAccumulate.First().Start, Nothing)
            End If
        End Sub
    End Class
End Namespace