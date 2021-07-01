Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models

Namespace BL
    Public Class PartInspectionOrderer
        Private Property DataSource As AcePumpContext
        Private Property DeliveryTicketID As Integer

        Public Sub New(dataSource As AcePumpContext, deliveryTicketId As Integer)
            Me.DataSource = dataSource
            Me.DeliveryTicketID = deliveryTicketId
        End Sub

        Public Sub Order()
            Dim pumpTemplateId As Integer = DataSource.DeliveryTickets _
                                                .Where(Function(x) x.PumpFailed IsNot Nothing AndAlso x.DeliveryTicketID = DeliveryTicketID) _
                                                .Select(Function(x) x.PumpFailed.PumpTemplateID) _
                                                .DefaultIfEmpty(0) _
                                                .FirstOrDefault()

            Dim templatePartDefs As List(Of TemplatePartDef) = DataSource.TemplatePartDefs.Where(Function(x) x.PumpTemplateID = pumpTemplateId).OrderBy(Function(x) x.SortOrder).ToList()
            Dim inspectionsToVisit As List(Of VisitableInspection) = (From pi In DataSource.PartInspections.Where(Function(x) x.DeliveryTicketID = DeliveryTicketID).OrderBy(Function(x) x.Sort)
                                                                      Select New VisitableInspection With {
                .Visited = False,
                .Inspection = pi
            }).ToList()

            Dim counter As Integer = 0
            For Each def As TemplatePartDef In templatePartDefs

                Dim inspectionToVisit = inspectionsToVisit.FirstOrDefault(Function(x) x.Inspection.TemplatePartDefID = def.TemplatePartDefID And x.Visited = False And Not x.Inspection.ParentAssemblyID.HasValue())
                If inspectionToVisit IsNot Nothing Then
                    changeSortForInspection(inspectionToVisit, inspectionsToVisit, counter)
                End If
            Next
        End Sub

        Private Sub changeSortForInspection(currentInspection As VisitableInspection, allInspections As List(Of VisitableInspection), ByRef currentSort As Integer)
            currentSort += 1
            currentInspection.Inspection.Sort = currentSort

            If currentInspection.Inspection.IsSplitAssembly Then
                Dim assemblyChildren As List(Of VisitableInspection) = allInspections.Where(Function(x) x.Inspection.ParentAssemblyID IsNot Nothing AndAlso x.Inspection.ParentAssemblyID = currentInspection.Inspection.PartInspectionID AndAlso x.Visited = False).ToList()

                For Each child As VisitableInspection In assemblyChildren
                    changeSortForInspection(child, allInspections, currentSort)
                Next

            End If
            currentInspection.Visited = True
        End Sub

        Private Class VisitableInspection
            Public Property Inspection As PartInspection
            Public Property Visited As Boolean = False
        End Class

    End Class
End Namespace