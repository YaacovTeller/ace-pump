Imports AcePump.Domain.Models
Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports Kendo.Mvc.UI
Imports Kendo.Mvc.Extensions
Imports System.Data.Entity
Imports AcePump.Web.Areas.Customers.Models


Namespace Areas.Customers.Controllers

    <Authorize()> _
    Public Class LineItemController
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
        ' GET: /RepairTicket/Index
        Function Index() As ActionResult
            Return RedirectToAction("Index", "DeliveryTicket")
        End Function
        '
        ' POST: /DeliveryTicket/List

        <HttpPost()> _
        Public Function List(deliveryTicketId As Integer?, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim lineItems As IQueryable(Of LineItem) = DataSource.LineItems _
                                                                .Where(Function(x) x.DeliveryTicketID = deliveryTicketId _
                                                                           And CustomerAccessIDs.Contains(x.DeliveryTicket.CustomerID))
            'sorting happens in the grid
            Dim r = lineItems _
                            .Select(Function(t) New LineItemsGridRowViewModel() With {
                                .DeliveryTicketID = t.DeliveryTicketID,
                                .Sort = t.SortOrder,
                                .LineItemID = t.LineItemID,
                                .PartTemplateID = t.PartTemplateID,
                                .Quantity = t.Quantity,
                                .ItemNumber = t.PartTemplate.Number,
                                .Description = t.Description
                            })

            Return Json(r.ToDataSourceResult(req))
        End Function

    End Class
End Namespace
