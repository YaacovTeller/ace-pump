Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports System.Data.Entity
Imports Kendo.Mvc.UI
Imports AcePump.Domain.Models
Imports Kendo.Mvc.Extensions
Imports System.Data.Entity.Infrastructure
Imports Yesod.Mvc
Imports Yesod.Ef
Imports System.Data.SqlClient

Namespace Areas.Employees.Controllers
    Public Class PumpController
        Inherits AcePumpControllerBase

        Private _PumpManager As KendoGridRequestManager(Of Pump, PumpDisplayDto)
        Friend ReadOnly Property PumpManager As KendoGridRequestManager(Of Pump, PumpDisplayDto)
            Get
                If _PumpManager Is Nothing Then
                    _PumpManager = New KendoGridRequestManager(Of Pump, PumpDisplayDto)(
                        DataSource,
                        Function(x As Pump) New PumpDisplayDto With {
                            .PumpID = x.PumpID,
                            .LeaseID = If(x.InstalledInWell IsNot Nothing, x.InstalledInWell.LeaseID, Nothing),
                            .Lease = If(x.InstalledInWell IsNot Nothing, x.InstalledInWell.Lease.LocationName, Nothing),
                            .Well = If(x.InstalledInWell IsNot Nothing, x.InstalledInWell.WellNumber, Nothing),
                            .InstalledInWellID = x.InstalledInWellID,
                            .CustomerID = If(x.InstalledInWell IsNot Nothing, x.InstalledInWell.CustomerID, Nothing),
                            .Customer = If(x.InstalledInWell IsNot Nothing, If(x.InstalledInWell.Customer IsNot Nothing, x.InstalledInWell.Customer.CustomerName, Nothing), Nothing),
                            .PumpTemplate = x.PumpTemplate.ConciseSpecificationSummary,
                            .PumpTemplateID = x.PumpTemplateID,
                            .PumpNumber = x.PumpNumber,
                            .ShopLocationID = x.ShopLocationID,
                            .ShopLocationPrefix = x.ShopLocation.Prefix
                        },
                        Nothing,
                        Me
                    )
                End If
                Return _PumpManager
            End Get
        End Property

        '
        ' GET: /Pump/[Index]

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
        <HttpGet()>
        Public Function Index() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Pump/List

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
        <HttpPost()>
        Public Function List(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return PumpManager.List(req)
        End Function

        '
        ' GET: /Pump/Details/id

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
        <HttpGet()>
        Public Function Details(id As Integer) As ActionResult
            Dim pump As Pump = DataSource.Pumps.FirstOrDefault(Function(x) x.PumpID = id)

            If pump IsNot Nothing Then
                Return View(PumpManager.ModelMapper.Convert(pump))
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' POST: /Pump/History/List

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
        <HttpPost()>
        Public Function HistoryList(pumpId As Integer, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim failures = From ticket In DataSource.DeliveryTickets
                           Where ticket.PumpFailedID.HasValue AndAlso ticket.PumpFailedID.Value = pumpId
                           Select New PumpHistoryViewModel With {
                               .PumpID = ticket.PumpFailedID.Value,
                               .DeliveryTicketID = ticket.DeliveryTicketID,
                               .HistoryDate = If(ticket.PumpFailedDate.HasValue, ticket.PumpFailedDate.Value, ticket.TicketDate.Value),
                               .HistoryType = "Failed"
                           }

            Dim dispatches = From ticket In DataSource.DeliveryTickets
                             Where ticket.PumpDispatchedID.HasValue AndAlso ticket.PumpDispatchedID.Value = pumpId
                             Select New PumpHistoryViewModel With {
                                 .PumpID = ticket.PumpDispatchedID.Value,
                                 .DeliveryTicketID = ticket.DeliveryTicketID,
                                 .HistoryDate = If(ticket.PumpDispatchedDate.HasValue, ticket.PumpDispatchedDate.Value, ticket.TicketDate.Value),
                                 .HistoryType = "Dispatched"
                             }

            Dim history = System.Linq.Queryable.Union(failures, dispatches)

            Return Json(history.ToDataSourceResult(req))
        End Function

        '
        ' GET: /Pump/Create

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
        <HttpGet()>
        Public Function Create() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Pump/Create

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
        <HttpPost()>
        Public Function Create(model As PumpDisplayDto) As ActionResult
            If String.IsNullOrWhiteSpace(model.PumpNumber) Or model.ShopLocationID = 0 Then
                ModelState.AddModelError("PumpNumber", "Both Shop and Pump Number fields are required.")
            End If

            If Not ModelState.IsValid Then
                Return View(model)
            End If

            If IsDuplicatePumpNumber(model.PumpNumber, model.ShopLocationID) Then
                ModelState.AddModelError("PumpNumber", "This pump number already exists. Cannot add duplicate pump number.")
                Return View(model)
            End If

            Try
                Dim newPump As New Pump() With {
                                                .ShopLocationID = model.ShopLocationID,
                                                .PumpNumber = model.PumpNumber.Trim(),
                                                .InstalledInWellID = model.InstalledInWellID,
                                                .PumpTemplateID = model.PumpTemplateID
                                            }

                DataSource.Pumps.Add(newPump)
                DataSource.SaveChanges()
                Return RedirectToAction("Details", New With {.id = newPump.PumpID})

            Catch ex As DbUpdateException When TypeOf ex.InnerException?.InnerException Is SqlException
                Dim sqlEx As SqlException = DirectCast(ex.InnerException.InnerException, SqlException)
                If sqlEx.Number = 2601 Or sqlEx.Number = 2627 Then
                    ModelState.AddModelError("PumpNumber", "An error has occured and this pump number has already been taken. Please reload the page to get the newest available pump number.")
                    Return View(model)
                End If
                Throw
            End Try
        End Function

        Function IsDuplicatePumpNumber(pumpNumber As String, shopLocationID As Integer, Optional originalShopLocationID As Integer = 0) As Boolean
            If shopLocationID = originalShopLocationID Then
                Return False
            End If

            Return DataSource.Pumps.Any(Function(x) x.PumpNumber = pumpNumber And x.ShopLocationID = shopLocationID)
        End Function

        '
        ' POST: /Pump/GetFiltered

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
        <HttpPost()>
        Public Function GetFiltered(customerId As Integer?, leaseId As Integer?, wellId As Integer?, term As String) As ActionResult
            Dim pumps As DbQuery(Of Pump) = DataSource.Pumps

            If Not String.IsNullOrEmpty(term) Then
                pumps = From p In DataSource.Pumps
                        Let num = p.ShopLocation.Prefix & p.PumpNumber
                        Where num.Contains(term)
                        Select p
            End If

            If customerId.HasValue Then
                pumps = pumps.Where(Function(p) p.InstalledInWell IsNot Nothing AndAlso p.InstalledInWell.CustomerID = customerId.Value)
            End If

            If leaseId.HasValue Then
                pumps = pumps.Where(Function(p) p.InstalledInWell.LeaseID = leaseId.Value)
            End If

            If wellId.HasValue Then
                pumps = pumps.Where(Function(p) p.InstalledInWell.WellID = wellId.Value)
            End If

            Return Json(pumps.Select(Function(p) New With {
                                     .PumpId = p.PumpID,
                                     .PumpNumber = p.ShopLocation.Prefix & p.PumpNumber})
                         )
        End Function

        '
        ' POST: /Pump/InstalledInWell

        <HttpPost()>
        Public Function InstalledInWell(wellId As Integer) As ActionResult
            Dim installed = DataSource.Pumps.Where(Function(p) p.InstalledInWell.WellID = wellId) _
                                    .Select(Function(p) New With {
                                             .PumpId = p.PumpID,
                                             .Prefix = p.ShopLocation.Prefix,
                                             .PumpNumber = p.PumpNumber}) _
                                     .FirstOrDefault()

            If installed IsNot Nothing Then
                Return Json(New With {.Success = True,
                                      .PumpId = installed.PumpId,
                                      .Prefix = installed.Prefix,
                                      .PumpNumber = installed.PumpNumber})
            Else
                Return Json(New With {.Success = False})
            End If

        End Function

        '
        ' POST: /Pump/LookupPumpTemplate

        <HttpPost()>
        Public Function LookupPumpTemplate(pumpId As Integer) As ActionResult
            Dim lookupPump As Pump = DataSource.Pumps _
                                     .Include(Function(p) p.PumpTemplate) _
                                     .FirstOrDefault(Function(x) x.PumpID = pumpId)
            If lookupPump IsNot Nothing Then
                Return Json(New With {.Success = True,
                                      .pumpTemplateId = lookupPump.PumpTemplateID,
                                      .conciseSpec = lookupPump.PumpTemplate.ConciseSpecificationSummary})
            Else
                Return Json(New With {.Success = False})
            End If
        End Function

        '
        'POST: /Pump/UpdateTemplate

        <HttpPost()>
        Public Function UpdateTemplate(pumpId As Integer, pumpTemplateId As Integer) As ActionResult
            Dim pumpToChange As Pump = DataSource.Pumps.Find(pumpId)
            Dim pumpTemplateToUpdate As PumpTemplate = DataSource.PumpTemplates.Find(pumpTemplateId)

            If pumpToChange IsNot Nothing And pumpTemplateToUpdate IsNot Nothing Then
                If pumpToChange.PumpTemplateID = pumpTemplateId Then
                    Return Json(New With {.Success = True,
                                          .conciseSpec = pumpTemplateToUpdate.ConciseSpecificationSummary})

                Else
                    pumpToChange.PumpTemplateID = pumpTemplateId

                    DataSource.SaveChanges()

                    Return Json(New With {.Success = True,
                                          .conciseSpec = pumpTemplateToUpdate.ConciseSpecificationSummary})
                End If
            End If

            Return Json(New With {.Success = False})
        End Function

        '
        'POST: /Pump/LastPull/id

        <HttpPost()>
        Public Function LastPull(id As Integer, currentTicketDate As Date?) As ActionResult
            Dim ticketFound As LastPullInfoViewModel = (From ticket In DataSource.DeliveryTickets
                                                        Where (currentTicketDate.HasValue AndAlso ticket.PumpDispatchedID = id)
                                                        Select New LastPullInfoViewModel With {
                                                             .DateFound = If(ticket.PumpDispatchedDate.HasValue, ticket.PumpDispatchedDate.Value, ticket.TicketDate.Value),
                                                             .PumpID = ticket.PumpDispatchedID.Value,
                                                             .DeliveryTicketID = ticket.DeliveryTicketID
                                                         }) _
                                                        .Where(Function(x) x.DateFound.Value < currentTicketDate) _
                                                        .OrderByDescending(Function(x) x.DateFound) _
                                                        .FirstOrDefault()

            If ticketFound Is Nothing OrElse Not ticketFound.DateFound.HasValue Then
                Return Json(New With {.Success = False,
                                      .Errors = "Could not find last pull date for this pump."})
            Else
                Return Json(New With {.Success = True, ticketFound})
            End If
        End Function

        '
        ' GET: /Pump/Edit

        <HttpGet()>
        Public Function Edit(id As Integer) As ActionResult
            Dim pumpFound = DataSource.Pumps.Find(id)

            Dim pump As PumpDisplayDto = DataSource.Pumps _
                                         .Where(Function(x) x.PumpID = id) _
                                         .Select(PumpManager.ModelMapper.Selector) _
                                         .SingleOrDefault
            If pump IsNot Nothing Then
                Return View(pump)
            End If

            Return RedirectToAction("Index")
        End Function

        '
        ' POST: /Pump/Edit

        <HttpPost()>
        Public Function Edit(model As PumpDisplayDto) As ActionResult
            If Not ModelState.IsValid Then
                Return View(model)
            End If

            Dim pump As Pump = DataSource.Pumps.Find(model.PumpID)
            If IsDuplicatePumpNumber(model.PumpNumber, model.ShopLocationID, pump.ShopLocationID) Then
                ModelState.AddModelError("PumpNumber", "This pump number already exists. Cannot add duplicate pump number.")
                Return View(model)
            End If

            If DataSource.Pumps.LoadChanges(model, pump) Then
                DataSource.SaveChanges()
                Return RedirectToAction("Details", New With {.id = model.PumpID})
            End If

            Return RedirectToAction("Index")
        End Function

        '
        ' POST: /Pump/Delete

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
        <HttpPost()>
        Public Function Delete(id As Integer) As ActionResult
            Dim pump As Pump = DataSource.Pumps.Find(id)

            If pump IsNot Nothing Then
                Dim referencedTicketIDs As List(Of Integer) = DataSource.DeliveryTickets.Where(Function(x) x.PumpDispatchedID = pump.PumpID Or x.PumpFailed.PumpID = pump.PumpID) _
                                                                    .Select(Function(x) x.DeliveryTicketID).ToList()

                If referencedTicketIDs.Count > 0 Then
                    ModelState.AddModelError("PumpID", "Could not delete this pump, because the following delivery tickets use this pump: " & String.Join(",", referencedTicketIDs))
                Else
                    DataSource.Pumps.Remove(pump)
                    DataSource.SaveChanges()
                End If
            Else
                ModelState.AddModelError("PumpID", "Could not find that pump to delete.")
            End If

            Return Json({}, ModelState)
        End Function
    End Class
End Namespace