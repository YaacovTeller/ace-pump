Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports Kendo.Mvc.UI
Imports Kendo.Mvc.Extensions

Namespace Areas.Employees.Controllers
    <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
    Public Class PartController
        Inherits AcePumpControllerBase

        '
        ' GET: /Part/Inventory

        <HttpGet()>
        Public Function Inventory() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Part/InventoryList

        <HttpPost()>
        Public Function InventoryList(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim usedParts = From inspection In DataSource.PartInspections
                            Where inspection.ReplacedWithInventoryPartID.HasValue
                            Select inspection.ReplacedWithInventoryPart

            Dim partsGrouped As IQueryable(Of PartGridViewModel) = DataSource.Parts _
                                                                             .Where(Function(x) Not usedParts.Contains(x)) _
                                                                             .GroupBy(Function(x) New With {x.PartTemplateID,
                                                                                                            x.CustomerID}) _
                                                                             .Select(Function(x) New With {.Count = x.Count,
                                                                                                           .Part = x.FirstOrDefault()}) _
                                                                             .Select(Function(x) New PartGridViewModel With {
                                                                                .QuantityAvailable = x.Count,
                                                                                .PartID = x.Part.PartID,
                                                                                .PartTemplateID = x.Part.PartTemplateID,
                                                                                .Description = x.Part.PartTemplate.Description,
                                                                                .Number = x.Part.PartTemplate.Number,
                                                                                .CustomerID = x.Part.CustomerID,
                                                                                .CustomerName = x.Part.Customer.CustomerName
                                                                                         })

            Return Json(partsGrouped.ToDataSourceResult(req, ModelState))
        End Function

        '
        ' POST: /Part/IsAvailableInInventory

        <HttpPost()>
        Public Function IsAvailableInInventory(id As Integer, customerID As Integer) As ActionResult
            Dim usedParts = From inspection In DataSource.PartInspections
                            Where inspection.ReplacedWithInventoryPartID.HasValue
                            Select inspection.ReplacedWithInventoryPart

            Return Json(New With {.Success = True,
                                  .Available = DataSource.Parts _
                                                         .Where(Function(x) x.PartTemplateID = id AndAlso x.CustomerID = customerID) _
                                                         .Where(Function(x) Not usedParts.Contains(x)) _
                                                         .Any()})
        End Function

        '
        ' POST: /Part/ListPartsAvailableInInventory

        <HttpPost()>
        Public Function ListPartsAvailableInInventory(partTemplateIDs As List(Of Integer), customerID As Integer) As ActionResult
            If partTemplateIDs Is Nothing Then partTemplateIDs = New List(Of Integer)

            Dim usedParts = From inspection In DataSource.PartInspections
                            Where inspection.ReplacedWithInventoryPartID.HasValue
                            Select inspection.ReplacedWithInventoryPart

            Dim result = DataSource.Parts.Where(Function(x) Not usedParts.Contains(x) And x.CustomerID = customerID) _
                                         .Where(Function(x) partTemplateIDs.Contains(x.PartTemplateID)) _
                                         .Select(Function(x) New With {.PartTemplateID = x.PartTemplateID}) _
                                         .Distinct()
            Return Json(result)
        End Function

        '
        ' POST: /Part/ListPartsInUseForPump

        <HttpPost()>
        Public Function ListPartsInUseForPump(pumpID As Integer, customerID As Integer, currentTicketDate As Date?) As ActionResult
            Dim mostCurrentInspections = DataSource.PartInspections.Where(Function(x) currentTicketDate.HasValue _
                                                                              AndAlso x.DeliveryTicket.CustomerID = customerID _
                                                                              AndAlso x.DeliveryTicket.PumpFailedID = pumpID _
                                                                              AndAlso x.DeliveryTicket.TicketDate < currentTicketDate) _
                                                                   .Where(Function(x) x.Result = "Replace" Or x.Result = "Convert") _
                                                                   .GroupBy(Function(x) x.TemplatePartDefID) _
                                                                   .Select(Function(x) New With {
                                                                            .TemplatePartDefID = x.Key,
                                                                            .ReplacedWithInventoryPartID = x.OrderByDescending(Function(g) g.DeliveryTicket.TicketDate) _
                                                                                                                                    .FirstOrDefault() _
                                                                                                                                    .ReplacedWithInventoryPartID
                                                                               })


            Return Json(mostCurrentInspections)
        End Function
    End Class
End Namespace