Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports Kendo.Mvc.UI
Imports AcePump.Domain.Models
Imports Kendo.Mvc.Extensions
Imports System.Data.Entity.Infrastructure
Imports AcePump.Web.Areas.Customers.Models


Namespace Areas.Customers.Controllers
    <Authorize()> _
        Public Class PumpController
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
        ' GET: /Customers/Pump

        Function Index() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Pump/Search

        <Authorize(Roles:=AcePumpSecurityRoles.Customer)> _
        <HttpPost()> _
        Public Function Search(leaseId As Integer?, wellId As Integer?, term As String) As ActionResult
            Dim pumps As DbQuery(Of Pump) = ApplyFilter(term, CustomerAccessIDs, leaseId, wellId)

            Return Json(pumps.Select(Function(p) New PumpViewModel With {
                                     .PumpID = p.PumpID,
                                     .ShopLocationPrefix = p.ShopLocation.Prefix,
                                     .PumpNumber = p.PumpNumber})
                                     )

        End Function

        '
        ' POST: /Pump/GridSearch

        <Authorize(Roles:=AcePumpSecurityRoles.Customer)> _
        <HttpPost()> _
        Public Function GridSearch(<DataSourceRequest()> req As DataSourceRequest, leaseId As Integer?, wellId As Integer?) As ActionResult
            Dim pumps As DbQuery(Of Pump) = ApplyFilter("", CustomerAccessIDs, leaseId, wellId)

            Return (Json(pumps.Select(Function(x) New PumpGridRowViewModel With {
                                                .PumpID = x.PumpID,
                                                .ShopLocationPrefix = x.ShopLocation.Prefix,
                                                .PumpNumber = x.PumpNumber,
                                                .PumpTemplate = x.PumpTemplate.ConciseSpecificationSummary,
                                                .Well = x.InstalledInWell.WellNumber,
                                                .Lease = x.InstalledInWell.Lease.LocationName
                                            }) _
                                        .ToDataSourceResult(req)
                        ))
        End Function

        Private Function ApplyFilter(term As String, customerIds As List(Of Integer), leaseId As Integer?, wellId As Integer?) As DbQuery(Of Pump)
            Dim pumps As DbQuery(Of Pump) = DataSource.Pumps

            If customerIds.Count > 0 Then
                pumps = pumps.Where(Function(p) p.InstalledInWell IsNot Nothing AndAlso p.InstalledInWell.CustomerID.HasValue AndAlso customerIds.Contains(p.InstalledInWell.CustomerID))
            End If

            If Not String.IsNullOrEmpty(term) Then
                pumps = From p In pumps
                        Let num = p.ShopLocation.Prefix & p.PumpNumber
                        Where num.Contains(term)
                        Select p
            End If

            If wellId.HasValue Then
                pumps = pumps.Where(Function(p) p.InstalledInWellID = wellId.Value)
            End If

            If leaseId.HasValue Then
                pumps = pumps.Where(Function(p) p.InstalledInWell.LeaseID = leaseId.Value)
            End If

            Return pumps
        End Function

        '
        ' GET: /Pump/PumpHistory

        <Authorize(Roles:=AcePumpSecurityRoles.Customer)> _
        <HttpGet()> _
        Public Function PumpHistory(pumpId As Integer?) As ActionResult
            Dim model As PumpViewModel = Nothing

            If pumpId.HasValue Then
                model = DataSource.Pumps _
                            .Select(Function(x) New PumpViewModel() With {
                                        .PumpID = x.PumpID,
                                        .ShopLocationPrefix = x.ShopLocation.Prefix,
                                        .PumpNumber = x.PumpNumber
                                    }) _
                            .SingleOrDefault(Function(x) x.PumpID = pumpId.Value)
            End If

            Return View(model)

        End Function

        '
        ' POST: /Pump/HistoryList

        <Authorize(Roles:=AcePumpSecurityRoles.Customer)> _
        <HttpPost()> _
        Public Function HistoryList(pumpId As Integer?, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            If pumpId.HasValue Then
                Return Json(DataSource.DeliveryTickets _
                                .Where(Function(x) _
                                           (x.PumpFailedID.HasValue AndAlso x.PumpFailedID.Value = pumpId.Value) _
                                           Or (x.PumpDispatchedID.HasValue AndAlso x.PumpDispatchedID.Value = pumpId.Value)) _
                                .Select(Function(x) New PumpHistoryGridRowViewModel With {
                                    .PumpID = If(x.PumpFailedID.HasValue AndAlso x.PumpFailedID.Value = pumpId.Value, x.PumpFailedID.Value, x.PumpDispatchedID.Value),
                                    .ShopLocationPrefix = If(x.PumpFailedID.HasValue AndAlso x.PumpFailedID.Value = pumpId.Value, x.PumpFailed.ShopLocation.Prefix, x.PumpDispatched.ShopLocation.Prefix),
                                    .PumpNumber = If(x.PumpFailedID.HasValue AndAlso x.PumpFailedID.Value = pumpId.Value, x.PumpFailed.PumpNumber, x.PumpDispatched.PumpNumber),
                                    .DeliveryTicketID = x.DeliveryTicketID,
                                    .HistoryDate = If(x.PumpFailedID.HasValue AndAlso x.PumpFailedID.Value = pumpId.Value, x.PumpFailedDate, x.PumpDispatchedDate),
                                    .HistoryType = If(x.PumpFailedID.HasValue AndAlso x.PumpFailedID.Value = pumpId.Value, "Failed", "Dispatched"),
                                    .ShowDeliveryTicket = x.CustomerID.HasValue AndAlso CustomerAccessIDs.Contains(x.CustomerID.Value)
                                }) _
                                .ToDataSourceResult(req)
                            )

            Else
                Return Json(Nothing)
            End If
        End Function


    End Class
End Namespace
