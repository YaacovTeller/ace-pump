Imports AcePump.Domain.Models
Imports AcePump.Domain.DataSource
Imports System.Data.Entity

Namespace BL.Runtimes
    Friend MustInherit Class RuntimeManagerBase(Of TRuntime As {IRuntime, Class})
        Implements IRuntimeManager

        Protected Property Runtime As TRuntime
        Protected Property DataSource As AcePumpContext

        Protected MustOverride ReadOnly Property RuntimeSet As IDbSet(Of TRuntime)

        Public Sub New(runtime As TRuntime, dataSource As AcePumpContext)
            Me.Runtime = runtime
            Me.DataSource = dataSource
        End Sub

        Public Overridable Function Exists() As Boolean Implements IRuntimeManager.Exists
            Return Runtime IsNot Nothing
        End Function

        Public MustOverride Sub CreateIfNotExists() Implements IRuntimeManager.CreateIfNotExists

        Public Overridable Sub SetStartDate(startDate As Date, startedById As Integer) Implements IRuntimeManager.SetStartDate
            If Not Exists() Then
                CreateIfNotExists()
            End If

            Dim split As TRuntime = Nothing
            If Runtime.StartedByID.HasValue AndAlso Runtime.StartedByID.Value <> startedById Then
                split = CreateNewRuntimeBasedOn(Runtime)
                split.Start = Runtime.Start
                split.StartedByID = Runtime.StartedByID
            End If

            Runtime.Start = startDate
            Runtime.StartedByID = startedById
            CalculateRuntimeLength()

            If split IsNot Nothing Then
                CompleteSplit(Runtime, split)
            End If
        End Sub

        Public Overridable Sub SetEndDate(endDate As Date, endedById As Integer) Implements IRuntimeManager.SetEndDate
            If Not Exists() Then
                CreateIfNotExists()
            End If

            Dim split As TRuntime = Nothing
            If Runtime.EndedByID.HasValue AndAlso Runtime.EndedByID.Value <> endedById Then
                split = CreateNewRuntimeBasedOn(Runtime)
                split.Finish = Runtime.Finish
                split.EndedByID = Runtime.EndedByID
            End If

            Runtime.Finish = endDate
            Runtime.EndedByID = endedById
            CalculateRuntimeLength()

            If split IsNot Nothing Then
                CompleteSplit(Runtime, split)
            End If
        End Sub

        Public Overridable Sub RemoveEndDate() Implements IRuntimeManager.RemoveEndDate
            If Not Exists() Then
                CreateIfNotExists()
            End If

            Runtime.EndedByID = Nothing
            Runtime.Finish = Nothing
            Runtime.LengthInDays = Nothing

            MergeWithAdjacents()
        End Sub

        Public Overridable Sub RemoveStartDate() Implements IRuntimeManager.RemoveStartDate
            If Not Exists() Then
                CreateIfNotExists()
            End If

            Runtime.StartedByID = Nothing
            Runtime.Start = Nothing
            Runtime.LengthInDays = Nothing

            MergeWithAdjacents()
        End Sub

        Public Overridable Sub CalculateRuntimeLength() Implements IRuntimeManager.CalculateRuntimeLength
            If Not Exists() Then
                CreateIfNotExists()
            End If

            If Runtime.Start.HasValue And Runtime.Finish.HasValue Then
                Runtime.LengthInDays = (Runtime.Finish.Value - Runtime.Start.Value).TotalDays
            End If
        End Sub

        Protected Overridable Sub MergeWithAdjacents()
            Dim adjacents As AdjacentRuntimes = FindAdjacents()
            If adjacents.After IsNot Nothing Then
                Runtime.Finish = adjacents.After.Finish
                Runtime.EndedByID = adjacents.After.EndedByID

                CompleteMerge(Runtime, adjacents.After)
            End If

            If adjacents.Before IsNot Nothing Then
                Runtime.Start = adjacents.Before.Start
                Runtime.StartedByID = adjacents.Before.StartedByID

                CompleteMerge(Runtime, adjacents.Before)
            End If

            CalculateRuntimeLength()
        End Sub

        Protected Overridable Sub CompleteMerge(merged As TRuntime, toDelete As TRuntime)
            RuntimeSet.Remove(toDelete)
        End Sub

        Protected Overridable Sub CompleteSplit(original As TRuntime, toAdd As TRuntime)
            RuntimeSet.Add(toAdd)
        End Sub

        Protected MustOverride Function FindAdjacents() As AdjacentRuntimes

        ''' <summary>
        ''' Create a new runtime of TRuntime type which identifies the same item as the basedOn
        ''' runtime but does not specify and start or finish information.
        ''' </summary>
        Protected MustOverride Function CreateNewRuntimeBasedOn(basedOn As TRuntime) As TRuntime
    End Class
End Namespace