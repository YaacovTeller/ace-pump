Imports AcePump.Common
Imports AcePump.Web.Areas.Customers.Models
Imports AcePump.Web.Controllers
Imports Kendo.Mvc.Extensions
Imports Kendo.Mvc.UI

Namespace Areas.Customers.Controllers

    <Authorize(Roles:=AcePumpSecurityRoles.Customer)>
    Public Class PartController
        Inherits AcePumpControllerBase

        Private _CustomerAccessIDs As List(Of Integer)
        Private ReadOnly Property CustomerAccessIDs As List(Of Integer)
            Get
                If _CustomerAccessIDs Is Nothing Then
                    _CustomerAccessIDs = HttpContext.AcePumpUser().Profile.CustomerAccessList.Values.ToList()
                End If

                Return _CustomerAccessIDs
            End Get
        End Property

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
                                                                             .Where(Function(x) CustomerAccessIDs.Contains(x.CustomerID)) _
                                                                             .GroupBy(Function(x) New With {x.PartTemplateID,
                                                                                                            x.CustomerID}) _
                                                                             .Select(Function(x) New With {.Count = x.Count,
                                                                                                           .Part = x.FirstOrDefault()}) _
                                                                             .Select(Function(x) New PartGridViewModel With {
                                                                                .QuantityAvailable = x.Count,
                                                                                .PartID = x.Part.PartID,
                                                                                .Description = x.Part.PartTemplate.Description,
                                                                                .Number = x.Part.PartTemplate.Number
                                                                                })

            Return Json(partsGrouped.ToDataSourceResult(req, ModelState))
        End Function

        '
        ' POST: /Part/ListPartsInUseForPump

        <HttpPost()>
        Public Function ListPartsInUseForPump(pumpID As Integer, currentTicketDate As Date?) As ActionResult
            Dim mostCurrentInspections = DataSource.PartInspections.Where(Function(x) CustomerAccessIDs.Contains(x.DeliveryTicket.CustomerID)) _
                                                                   .Where(Function(x) currentTicketDate.HasValue AndAlso x.DeliveryTicket.PumpFailedID = pumpID AndAlso x.DeliveryTicket.TicketDate < currentTicketDate) _
                                                                   .Where(Function(x) x.Result = "Replace" Or x.Result = "Convert") _
                                                                   .GroupBy(Function(x) x.TemplatePartDefID) _
                                                                   .Select(Function(x) New With {.ReplacedWithInventoryPartID = x.OrderByDescending(Function(g) g.DeliveryTicket.TicketDate) _
                                                                                                                                 .FirstOrDefault() _
                                                                                                                                 .ReplacedWithInventoryPartID,
                                                                                                 .TemplatePartDefID = x.Key})
            Return Json(mostCurrentInspections)
        End Function

    End Class
End Namespace
