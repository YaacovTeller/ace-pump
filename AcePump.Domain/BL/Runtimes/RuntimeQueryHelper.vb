Imports AcePump.Domain.Models
Imports AcePump.Domain.DataSource
Imports System.Data.Entity

Namespace BL.Runtimes
    Friend Class RuntimeQueryHelper
        Private Property DataSource As AcePumpContext

        Public Sub New(dataSource As AcePumpContext)
            Me.DataSource = dataSource
        End Sub

        ''' <summary>
        ''' Gets all the runtimes for the specified pump ID which intersect with the event date.
        ''' </summary>
        Public Function GetPartRuntimes(pumpId As Integer, eventDate As Date) As IQueryable(Of ExistingRuntimeQuery)
            Dim templatePartDefsInPump As IQueryable(Of Integer) = DataSource.Pumps _
                                                                    .Where(Function(x) x.PumpID = pumpId) _
                                                                    .SelectMany(Function(x) x.PumpTemplate.Parts.Select(Function(y) y.TemplatePartDefID))

            Dim candidatePartRuntimes As IQueryable(Of PartRuntime) = DataSource.PartRuntimes _
                                                                        .Where(Function(x) _
                                                                                   x.PumpID = pumpId _
                                                                                   And (Not x.Start.HasValue OrElse x.Start.Value <= eventDate) _
                                                                                   And (Not x.Finish.HasValue OrElse x.Finish.Value >= eventDate)
                                                                               )

            Dim existingRuntimes As IQueryable(Of ExistingRuntimeQuery) = From templatePartDef In DataSource.TemplatePartDefs
                                                                          Group Join runtime In candidatePartRuntimes On templatePartDef.TemplatePartDefID Equals runtime.TemplatePartDefID Into matchingRuntimes = Group
                                                                          Where templatePartDefsInPump.Contains(templatePartDef.TemplatePartDefID)
                                                                          Select New ExistingRuntimeQuery With {
                                                                              .RuntimeIfExists = matchingRuntimes.FirstOrDefault(),
                                                                              .RuntimeSegmentsIfExist = matchingRuntimes.SelectMany(Function(x) x.Segments),
                                                                              .TemplatePartDefID = templatePartDef.TemplatePartDefID,
                                                                              .PumpID = pumpId
                                                                          }

            Return existingRuntimes
        End Function

        ''' <summary>
        ''' Checks if the eventDate occured during an existing pump runtime for the pumpId.  If 
        ''' it does, returns that runtime.  Otherwise creates a new runtime for the pumpId.
        ''' </summary>
        ''' <returns>An IRuntimeManager exposing methods to modify the runtime that was found.</returns>
        Public Function ManageRuntimeEventOccuredIn(pumpId As Integer, eventDate As Date) As IRuntimeManager
            Dim runtime As PumpRuntime = FindRuntimeEventOccuredIn(pumpId, eventDate)

            Return RuntimeManagerFactory.GetManager(runtime, pumpId, DataSource)
        End Function

        ''' <summary>
        ''' Checks if the eventDate occured during an existing part runtime for the pumpId and
        ''' templatePartDefId combination.  If it does, returns that runtime.  Otherwise creates a new runtime for
        ''' the pumpId partId.
        ''' </summary>
        ''' <returns>An IRuntimeManager exposing methods to modify the runtime that was found.</returns>
        Public Function ManageRuntimeEventOccuredIn(pumpId As Integer, templatePartDefId As Integer, eventDate As Date) As IRuntimeManager
            Dim runtime As PartRuntime = FindRuntimeEventOccuredIn(pumpId, templatePartDefId, eventDate)

            Return RuntimeManagerFactory.GetManager(runtime, pumpId, templatePartDefId, DataSource)
        End Function

        ''' <summary>
        ''' Checks if the eventDate occured during an existing part runtime for the pumpId and
        ''' templatePartDefId combination.  Does NOT create the runtime if it does not exist.
        ''' </summary>
        Private Function FindRuntimeEventOccuredIn(pumpId As Integer, templatePartDefId As Integer, eventDate As Date) As PartRuntime
            Dim runtime As PartRuntime =
                (From r In DataSource.PartRuntimes
                 Where r.PumpID = pumpId
                 Where r.TemplatePartDefID = templatePartDefId
                 Where Not r.Start.HasValue OrElse r.Start.Value <= eventDate
                 Where Not r.Finish.HasValue OrElse r.Finish.Value >= eventDate) _
                .FirstOrDefault()

            Return runtime
        End Function

        ''' <summary>
        ''' Checks if the eventDate occured during an existing pump runtime for the pumpId.
        ''' Does NOT create the runtime if it is not found.
        ''' </summary>
        Private Function FindRuntimeEventOccuredIn(pumpId As Integer, eventDate As Date) As PumpRuntime
            Dim runtime As PumpRuntime =
                (From r In DataSource.PumpRuntimes
                 Where r.PumpID = pumpId
                 Where Not r.Start.HasValue OrElse r.Start.Value <= eventDate
                 Where Not r.Finish.HasValue OrElse r.Finish.Value >= eventDate) _
                .FirstOrDefault()

            Return runtime
        End Function
    End Class
End Namespace