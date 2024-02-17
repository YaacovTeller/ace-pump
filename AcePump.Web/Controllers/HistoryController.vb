Imports AcePump.Domain.DataSource
Imports AcePump.Web.Models
Imports AcePump.Domain.Models
Imports System.Data.Entity
Imports AcePump.Domain.BL
Imports Kendo.Mvc.Extensions
Imports Yesod.Widgets
Imports Yesod.Widgets.Models
Imports Yesod.Kendo
Imports DataSourceRequest = Kendo.Mvc.UI.DataSourceRequest
Imports Kendo.Mvc.UI
Imports AcePump.Common.Soris
Imports DelegateDecompiler

Namespace Controllers
    <Authorize()> _
    Public Class HistoryController
        Inherits AcePumpControllerBase

        '
        ' GET: /History/CustomerDashboard

        <HttpGet()> _
        Public Function CustomerDashboard(queryStringConfig As WidgetManagerConfigurationModel) As ActionResult
            Return View(GetDashboardConfiguration(queryStringConfig))
        End Function

        '
        ' GET: /History/ReasonRepairedDashboard

        <HttpGet()>
        Public Function ReasonRepairedDashboard(reasonRepaired As String, queryStringConfig As WidgetManagerConfigurationModel) As ActionResult
            Dim config As DashboardConfigurationModel = GetDashboardConfiguration(queryStringConfig)

            config.ManagerConfiguration.ReasonRepaired = reasonRepaired

            Return View(config)
        End Function

        '
        ' GET: /History/LeaseDashboard

        <HttpGet()>
        Public Function LeaseDashboard(id As Integer?, locationName As String, queryStringConfig As WidgetManagerConfigurationModel) As ActionResult
            Dim lease As Lease = Nothing

            If id.HasValue Then
                lease = DataSource.LeaseLocations.Find(id)
            ElseIf Not String.IsNullOrEmpty(locationName) Then
                lease = DataSource.LeaseLocations.FirstOrDefault(Function(x) x.LocationName = locationName)
            End If

            Dim config As DashboardConfigurationModel = GetDashboardConfiguration(queryStringConfig)
            config.ManagerConfiguration.WellSelectionType = SelectionType.None

            If lease IsNot Nothing Then
                config.ManagerConfiguration.LeaseID = lease.LeaseID
                config.ManagerConfiguration.LeaseName = lease.LocationName
            End If

            Return View(config)
        End Function

        '
        ' GET: /History/WellDashboard

        <HttpGet()>
        Public Function WellDashboard(id As Integer?, wellNumber As String, queryStringConfig As WidgetManagerConfigurationModel) As ActionResult
            Dim well As Well = Nothing

            If id.HasValue Then
                well = DataSource.WellLocations _
                        .Include(Function(x) x.Lease) _
                        .SingleOrDefault(Function(x) x.WellID = id.Value)

            ElseIf Not String.IsNullOrEmpty(wellNumber) Then
                well = DataSource.WellLocations _
                        .Include(Function(x) x.Lease) _
                        .SingleOrDefault(Function(x) x.WellNumber = wellNumber)
            End If

            Dim config As DashboardConfigurationModel = GetDashboardConfiguration(queryStringConfig)
            If well IsNot Nothing Then
                config.ManagerConfiguration.LeaseID = well.Lease.LeaseID
                config.ManagerConfiguration.LeaseName = well.Lease.LocationName
                config.ManagerConfiguration.WellID = well.WellID
                config.ManagerConfiguration.WellNumber = well.WellNumber
            End If

            Return View(config)
        End Function

        '
        ' GET: /History/PartDashboard

        <HttpGet()>
        Public Function PartDashboard(partTemplateNumber As String, partTemplateId As Integer?, queryStringConfig As WidgetManagerConfigurationModel) As ActionResult
            Dim config As DashboardConfigurationModel = GetDashboardConfiguration(queryStringConfig)

            Dim partTemplate As PartTemplate = DataSource.PartTemplates _
                                .Where(Function(p) (partTemplateId.HasValue AndAlso p.PartTemplateID = partTemplateId.Value) Or (Not String.IsNullOrEmpty(partTemplateNumber) AndAlso p.Number = partTemplateNumber)) _
                                .FirstOrDefault()

            If partTemplate IsNot Nothing Then
                config.ManagerConfiguration.PartTemplateID = partTemplate.PartTemplateID
                config.ManagerConfiguration.PartTemplateNumber = partTemplate.Number
            End If

            Return View(config)
        End Function

        '
        ' POST: /History/GlobalQuery

        <HttpPost()>
        Public Function GlobalQuery(request As AcePumpWidgetRequestGroup) As ActionResult
            Dim globalResponse As WidgetGlobalResponse = ProcessGlobalRequest(request)
            globalResponse.notes = GetDesignChanges(request)

            Return Json(globalResponse)
        End Function

        ''' <summary>
        ''' Load all significant design changes that match the global query (ie for this customer/lease/well)
        ''' </summary>
        Private Function GetDesignChanges(request As AcePumpWidgetRequestGroup) As List(Of CategoryChartNote)
            Dim designChangeTickets As IQueryable(Of DeliveryTicket) = DataSource.DeliveryTickets _
                                                                                    .Where(Function(ticket) ticket.IsSignificantDesignChange.HasValue AndAlso ticket.IsSignificantDesignChange.Value)

            If request.CustomerIDs.Any() Then
                designChangeTickets = designChangeTickets.Where(Function(x) x.CustomerID.HasValue AndAlso request.CustomerIDs.Contains(x.CustomerID.Value))
            End If

            If request.LeaseID.HasValue Then
                designChangeTickets = designChangeTickets.Where(Function(x) x.WellID.HasValue AndAlso x.Well.LeaseID = request.LeaseID.Value)
            End If

            If request.CategoryID.HasValue Then
                designChangeTickets = designChangeTickets.Where(Function(x) x.WellID.HasValue AndAlso x.WellID.Value = request.WellID.Value)
            End If

            designChangeTickets.Include(Function(x) x.Well.Lease.LocationName) _
                               .Include(Function(x) x.Well.WellNumber)
            Dim designChanges = designChangeTickets.AsEnumerable().Select(Function(ticket) New CategoryChartNote With {
                                     .value = ticket.TicketDate.Value,
                                     .valueType = "date",
                                     .link = Url.Action("Details", "DeliveryTicket", New With {.id = ticket.DeliveryTicketID}),
                                     .text = (ticket.Well.Lease.LocationName & ", " & ticket.Well.WellNumber).Trim()
                                 }) _
                                .ToList()

            Return designChanges
        End Function

        '
        ' POST: /History/WidgetQuery

        <HttpPost()>
        Public Function WidgetQuery(group As AcePumpWidgetRequestGroup) As ActionResult
            If ModelState.IsValid Then
                Dim response As New WidgetResponseGroup

                If Not response.Errors.Any() Then
                    Dim manager As New WidgetRequestGroupManager(Of AcePumpContext)(DataSource)
                    response.Responses = manager.ProcessRequests(group).Responses
                End If

                Return Json(response)

            Else
                Dim msgBuilder As New StringBuilder()
                For Each item In ModelState
                    If item.Value.Errors.Count > 0 Then
                        For Each [error] In item.Value.Errors
                            msgBuilder.Append((String.Format("{0}: {1}{2}", item.Key, [error].ErrorMessage, vbCrLf)))
                        Next
                    End If
                Next

                Response.StatusCode = 500
                Return Content(msgBuilder.ToString())
            End If
        End Function

        '
        ' POST: /History/CategorySearch

        <HttpPost()>
        Public Function CategorySearch(term As String) As ActionResult
            If String.IsNullOrEmpty(term) Then
                term = Request.Form("filter[filters][0][value]")
            End If

            Dim categoryQuery As IQueryable(Of PartCategory) = DataSource.PartCategories

            If Not String.IsNullOrEmpty(term) Then
                categoryQuery = categoryQuery.Where(Function(x) x.CategoryName.StartsWith(term))
            End If

            Return Json(categoryQuery.Select(Function(w) New With {
                                          .CategoryID = w.PartCategoryID,
                                          .CategoryName = w.CategoryName
                                      }))
        End Function

        '
        ' POST: /History/PartSearch

        <HttpPost()>
        Public Function PartSearch(categoryId As Integer?, term As String) As ActionResult
            Dim filtered As IQueryable(Of PartTemplate) = DataSource.PartTemplates.Where(Function(x) x.Number.Contains(term))

            If categoryId.HasValue Then
                filtered = filtered.Where(Function(x) x.PartCategoryID = categoryId.Value)
            End If
            Return Json(filtered.Select(Function(w) New With {
                                      .PartTemplateID = w.PartTemplateID,
                                      .PartDescription = If(w.Number, "") & " " & If(w.Description, "")
                                 })
                        )
        End Function

        '
        ' POST: /History/LeaseSearch

        <HttpPost()>
        Public Function LeaseSearch(term As String, customerId As Integer?) As ActionResult
            Dim wells As IQueryable(Of Well) = DataSource.WellLocations.Where(Function(x) x.Lease.LocationName.Contains(term)) _
                                               .Where(Function(x) Not x.Lease.IgnoreInReporting.HasValue OrElse Not x.Lease.IgnoreInReporting.Value)
            Dim accessbileCustomerIds As List(Of Integer) = GetAccessibleCustomerIds(customerId)

            If accessbileCustomerIds.Any() Then
                wells = wells.Where(Function(x) x.CustomerID.HasValue AndAlso accessbileCustomerIds.Contains(x.CustomerID.Value))
            End If

            Return Json(wells.Select(Function(x) New With {
                                         .Id = x.LeaseID,
                                         .Name = x.Lease.LocationName
                                     }) _
                             .Distinct() _
                                    .AsEnumerable() _
                                    .OrderBy(Function(x) x.Name, New NaturalStringComparer())
                        )
        End Function

        '
        ' POST: /History/WellSearch

        <HttpPost()>
        Public Function WellSearch(term As String, customerId As Integer?, leaseId As Integer?) As ActionResult
            Dim wells As IQueryable(Of Well) = DataSource.WellLocations.Where(Function(x) x.WellNumber.Contains(term))
            Dim accessbileCustomerIds As List(Of Integer) = GetAccessibleCustomerIds(customerId)

            If accessbileCustomerIds.Any() Then
                wells = wells.Where(Function(x) x.CustomerID.HasValue AndAlso accessbileCustomerIds.Contains(x.CustomerID.Value))
            End If

            If leaseId.HasValue Then
                wells = wells.Where(Function(w) w.LeaseID = leaseId.Value)
            End If

            Return Json(wells.Select(Function(x) New With {
                                         .WellId = x.WellID,
                                         .WellNumber = x.WellNumber
                                     }) _
                                    .AsEnumerable() _
                                    .OrderBy(Function(x) x.WellNumber, New NaturalStringComparer())
                        )
        End Function

        '
        ' POST: /History/DeliveryTickets

        <HttpPost()>
        Public Function DeliveryTickets(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim common As RecordGridCommonFilters = ExtractCommonFiltersFromDataSourceRequest(req)
            Dim tickets As IQueryable(Of DeliveryTicket) = From ticket In DataSource.DeliveryTickets
                                                           Where ticket.TicketDate >= common.StartDate And ticket.TicketDate <= common.EndDate

            If common.CustomerIDs.Any() Then
                tickets = tickets.Where(Function(x) x.CustomerID.HasValue AndAlso common.CustomerIDs.Contains(x.CustomerID.Value))
            End If

            If common.LeaseID.HasValue Then
                tickets = tickets.Where(Function(x) x.Well IsNot Nothing AndAlso x.Well.LeaseID = common.LeaseID.Value)
            End If

            If common.WellID.HasValue Then
                tickets = tickets.Where(Function(x) x.WellID.HasValue AndAlso x.WellID.Value = common.WellID.Value)
            End If

            Return Json(tickets.Select(Function(ticket) New DeliveryTicketsDashboardGridRowModel With {
                                           .DeliveryTicketID = ticket.DeliveryTicketID,
                                           .CustomerName = If(ticket.Customer IsNot Nothing, ticket.Customer.CustomerName, ""),
                                           .WellNumber = If(ticket.Well IsNot Nothing, ticket.Well.WellNumber, ""),
                                           .LeaseName = If(ticket.Well IsNot Nothing, ticket.Well.Lease.LocationName, ""),
                                           .TicketDate = ticket.TicketDate,
                                           .IsSignificantDesignChange = ticket.IsSignificantDesignChange.HasValue And ticket.IsSignificantDesignChange = True,
                                           .Cost = ticket.LineItems _
                                                    .Select(Function(x) x.UnitPrice * (1 - x.UnitDiscount) _
                                                                           * x.Quantity _
                                                                           * If(x.CollectSalesTax.HasValue AndAlso x.CollectSalesTax.Value,
                                                                              x.DeliveryTicket.SalesTaxRate + 1D, 1D)) _
                                                    .DefaultIfEmpty() _
                                                    .Sum()
                                       }) _
                               .ToDataSourceResult(req))
        End Function

        '
        ' POST: /History/RepairTickets

        <HttpPost()>
        Public Function RepairTickets(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim common As RecordGridCommonFilters = ExtractCommonFiltersFromDataSourceRequest(req)
            Dim inspections = From inspection In DataSource.PartInspections
                              Group Join lineItem In DataSource.LineItems On inspection.PartInspectionID Equals lineItem.PartInspectionID Into lineItemGroup = Group
                              From lineItemIfExists In lineItemGroup.DefaultIfEmpty()
                              Where inspection.DeliveryTicket.TicketDate >= common.StartDate And inspection.DeliveryTicket.TicketDate <= common.EndDate
                              Where inspection.Result = "Replace" Or inspection.Result = "Convert"

            If common.CustomerIDs.Any() Then
                inspections = inspections.Where(Function(x) x.inspection.DeliveryTicket.CustomerID.HasValue AndAlso common.CustomerIDs.Contains(x.inspection.DeliveryTicket.CustomerID.Value))
            End If

            If common.LeaseID.HasValue Then
                inspections = inspections.Where(Function(x) x.inspection.DeliveryTicket.Well IsNot Nothing AndAlso x.inspection.DeliveryTicket.Well.LeaseID = common.LeaseID.Value)
            End If

            If common.WellID.HasValue Then
                inspections = inspections.Where(Function(x) x.inspection.DeliveryTicket.WellID.HasValue AndAlso x.inspection.DeliveryTicket.WellID.Value = common.WellID.Value)
            End If

            If common.PartTemplateID.HasValue Then
                inspections = inspections.Where(Function(x) x.inspection.PartFailedID.HasValue AndAlso x.inspection.PartFailedID.Value = common.PartTemplateID.Value)
            End If

            If common.PartCategoryID.HasValue Then
                inspections = inspections.Where(Function(x) x.inspection.PartFailedID.HasValue AndAlso x.inspection.PartFailed.PartCategoryID = common.PartCategoryID.Value)
            End If

            If Not String.IsNullOrEmpty(common.ReasonRepaired) Then
                inspections = inspections.Where(Function(x) x.inspection.ReasonRepaired = common.ReasonRepaired)
            End If

            req.SetDefaultSort("PartInspectionID")
            Return Json(inspections.Select(Function(x) New PartInspectionDashboardGridRowModel With {
                                               .PartInspectionID = x.inspection.PartInspectionID,
                                               .DeliveryTicketID = x.inspection.DeliveryTicketID,
                                               .CustomerName = If(x.inspection.DeliveryTicket.Customer IsNot Nothing, x.inspection.DeliveryTicket.Customer.CustomerName, ""),
                                               .ApiNumber = If(x.inspection.DeliveryTicket.Well IsNot Nothing, x.inspection.DeliveryTicket.Well.APINumber, ""),
                                               .WellNumber = If(x.inspection.DeliveryTicket.Well IsNot Nothing, x.inspection.DeliveryTicket.Well.WellNumber, ""),
                                               .LeaseName = If(x.inspection.DeliveryTicket.Well IsNot Nothing, x.inspection.DeliveryTicket.Well.Lease.LocationName, ""),
                                               .PartDescription = If(x.inspection.PartFailed IsNot Nothing, x.inspection.PartFailed.Description, ""),
                                               .ReasonRepaired = x.inspection.ReasonRepaired,
                                               .TicketDate = x.inspection.DeliveryTicket.TicketDate,
                                               .IsSignificantDesignChange = x.inspection.DeliveryTicket.IsSignificantDesignChange.HasValue And x.inspection.DeliveryTicket.IsSignificantDesignChange = True,
                                               .Cost = If(x.lineItemIfExists IsNot Nothing, x.lineItemIfExists.LineTotal.Computed(), 0D)
                                           }) _
                                   .Decompile() _
                                   .ToDataSourceResult(req))
        End Function

        Private Function ExtractCommonFiltersFromDataSourceRequest(req As DataSourceRequest) As RecordGridCommonFilters
            Dim common As New RecordGridCommonFilters()
            If req.Filters.Count = 0 Then Return common

            RecursiveExtractCommonFilters(common, req.Filters)

            Return common
        End Function

        Private Sub RecursiveExtractCommonFilters(common As RecordGridCommonFilters, filters As IList(Of Kendo.Mvc.IFilterDescriptor))
            Dim i As Integer = 0
            While i < filters.Count
                Dim compositeFilter As Kendo.Mvc.CompositeFilterDescriptor = TryCast(filters(i), Kendo.Mvc.CompositeFilterDescriptor)
                If compositeFilter IsNot Nothing Then
                    RecursiveExtractCommonFilters(common, compositeFilter.FilterDescriptors)

                    If compositeFilter.FilterDescriptors.Count = 0 Then
                        filters.Remove(compositeFilter)
                        i -= 1
                    End If

                Else
                    Dim filter As Kendo.Mvc.FilterDescriptor = filters(i)

                    If filter.Member = "StartDate" Then
                        common.StartDate = Date.Parse(filter.Value)
                    ElseIf filter.Member = "EndDate" Then
                        common.EndDate = Date.Parse(filter.Value)
                    ElseIf filter.Member = "CustomerID" Then
                        Dim filterValue As String = filter.Value
                        If filterValue.StartsWith("""") Then filterValue = filterValue.Remove(0, 1)
                        If filterValue.EndsWith("""") Then filterValue = filterValue.Remove(filterValue.Length - 1, 1)
                        Dim idStrings As String() = CStr(filterValue).Split(",")
                        For Each idString As String In idStrings
                            common.CustomerIDs.Add(Integer.Parse(idString))
                        Next

                    ElseIf filter.Member = "LeaseID" Then
                        common.LeaseID = Integer.Parse(filter.Value)
                    ElseIf filter.Member = "WellID" Then
                        common.WellID = Integer.Parse(filter.Value)
                    ElseIf filter.Member = "PartTemplateID" Then
                        common.PartTemplateID = Integer.Parse(filter.Value)
                    ElseIf filter.Member = "CategoryID" Then
                        common.PartCategoryID = Integer.Parse(filter.Value)
                    ElseIf filter.Member = "ReasonRepaired" Then
                        common.ReasonRepaired = filter.Value
                    Else
                        i += 1
                        Continue While 'And Do Not Remove Filter
                    End If

                    filters.Remove(filter)
                    i -= 1
                End If

                i += 1
            End While
        End Sub

        Private Function GetAccessibleCustomerIds(requestedId As Integer?) As List(Of Integer)
            Dim ids As List(Of Integer)
            If Not CurrentUserHasAccessToCustomerId(requestedId) Then
                ids = HttpContext.AcePumpUser.Profile.CustomerAccessList.Values.ToList()

            ElseIf requestedId.HasValue Then
                ids = New List(Of Integer) From {requestedId.Value}

            Else
                ids = New List(Of Integer)
            End If

            Return ids
        End Function

        Private Function GetDashboardConfiguration(queryStringConfig As WidgetManagerConfigurationModel) As DashboardConfigurationModel
            Dim profile As AcePumpProfile = HttpContext.AcePumpUser().Profile

            Dim mainConfig As New DashboardConfigurationModel() With {
                .ManagerConfiguration = New WidgetManagerConfigurationModel() With {
                    .CustomerSelectionType = If(Not profile.CustomerID.HasValue,
                                                SelectionType.Any,
                                                If(profile.CustomerAccessList.Count > 1, SelectionType.AccessListOnly, SelectionType.None)
                                             ),
                    .LeaseSelectionType = SelectionType.Any,
                    .WellSelectionType = SelectionType.Any,
                    .CustomerName = If(profile.Customer IsNot Nothing, profile.Customer.CustomerName, ""),
                    .CustomersIDsThatMayBeAccessed = profile.CustomerAccessList,
                    .CustomerID = profile.CustomerID
                }
            }

            MergeQuerystringConfiguration(mainConfig, queryStringConfig)

            Return mainConfig
        End Function

        Public Sub MergeQuerystringConfiguration(mainConfig As DashboardConfigurationModel, queryStringConfig As WidgetManagerConfigurationModel)
            If mainConfig.ManagerConfiguration.CustomerSelectionType = SelectionType.Any Then
                mainConfig.ManagerConfiguration.CustomerID = If(queryStringConfig.CustomerID.HasValue, queryStringConfig.CustomerID.Value, mainConfig.ManagerConfiguration.CustomerID)
                mainConfig.ManagerConfiguration.CustomerName = If(Not String.IsNullOrEmpty(queryStringConfig.CustomerName), queryStringConfig.CustomerName, mainConfig.ManagerConfiguration.CustomerName)
            End If

            If mainConfig.ManagerConfiguration.LeaseSelectionType = SelectionType.Any Then
                mainConfig.ManagerConfiguration.LeaseID = If(queryStringConfig.LeaseID.HasValue, queryStringConfig.LeaseID, mainConfig.ManagerConfiguration.LeaseID)
                mainConfig.ManagerConfiguration.CustomerName = If(Not String.IsNullOrEmpty(queryStringConfig.LeaseName), queryStringConfig.LeaseName, mainConfig.ManagerConfiguration.LeaseName)
            End If

            If mainConfig.ManagerConfiguration.WellSelectionType = SelectionType.Any Then
                mainConfig.ManagerConfiguration.WellID = If(queryStringConfig.WellID.HasValue, queryStringConfig.WellID, mainConfig.ManagerConfiguration.WellID)
                mainConfig.ManagerConfiguration.WellNumber = If(Not String.IsNullOrEmpty(queryStringConfig.WellNumber), queryStringConfig.WellNumber, mainConfig.ManagerConfiguration.WellNumber)
            End If

            mainConfig.ManagerConfiguration.ReasonRepaired = If(Not String.IsNullOrEmpty(queryStringConfig.ReasonRepaired), queryStringConfig.ReasonRepaired, mainConfig.ManagerConfiguration.ReasonRepaired)

            mainConfig.ManagerConfiguration.CategoryID = If(queryStringConfig.WellID.HasValue, queryStringConfig.CategoryID, mainConfig.ManagerConfiguration.CategoryID)
            mainConfig.ManagerConfiguration.CategoryName = If(Not String.IsNullOrEmpty(queryStringConfig.CategoryName), queryStringConfig.CategoryName, mainConfig.ManagerConfiguration.CategoryName)

            mainConfig.ManagerConfiguration.PartTemplateID = If(queryStringConfig.PartTemplateID.HasValue, queryStringConfig.PartTemplateID, mainConfig.ManagerConfiguration.PartTemplateID)
            mainConfig.ManagerConfiguration.PartTemplateNumber = If(Not String.IsNullOrEmpty(queryStringConfig.PartTemplateNumber), queryStringConfig.PartTemplateNumber, mainConfig.ManagerConfiguration.PartTemplateNumber)

            mainConfig.ManagerConfiguration.StartDate = If(queryStringConfig.StartDate.HasValue, queryStringConfig.StartDate, mainConfig.ManagerConfiguration.StartDate)
            mainConfig.ManagerConfiguration.EndDate = If(queryStringConfig.EndDate.HasValue, queryStringConfig.EndDate, mainConfig.ManagerConfiguration.EndDate)
        End Sub

        ''' <summary>
        ''' Verifies that any well / customer / lease requests are accessible by the currently logged on user and provides
        ''' info from the DB about each of them.
        ''' </summary>
        Private Function ProcessGlobalRequest(request As AcePumpWidgetRequestGroup) As WidgetGlobalResponse
            Dim globalResponse As WidgetGlobalResponse
            If request.WellID.HasValue Then
                globalResponse = DataSource.WellLocations _
                                            .Where(Function(x) x.WellID = request.WellID.Value) _
                                            .Select(Function(x) New WidgetGlobalResponse With {
                                                        .CustomerID = x.CustomerID.Value,
                                                        .CustomerName = x.Customer.CustomerName,
                                                        .LeaseName = x.Lease.LocationName,
                                                        .WellNumber = x.WellNumber
                                                    }) _
                                            .FirstOrDefault()

                If Not CurrentUserHasAccessToCustomerId(globalResponse.CustomerID) Then
                    globalResponse.Errors.Add(String.Format("Sorry, you are not authorized to view data at {0}, Well #{1}",
                                                               globalResponse.LeaseName,
                                                               globalResponse.WellNumber
                                                              )
                                                )
                End If

            ElseIf request.LeaseID.HasValue Then
                globalResponse = DataSource.LeaseLocations _
                    .Where(Function(x) x.LeaseID = request.LeaseID.Value) _
                    .Select(Function(x) New WidgetGlobalResponse With {
                                .LeaseName = x.LocationName
                            }) _
                    .FirstOrDefault()

            ElseIf request.CustomerIDs.Any() Then
                globalResponse = DataSource.Customers _
                    .Where(Function(x) request.CustomerIDs.Contains(x.CustomerID)) _
                    .Select(Function(x) New WidgetGlobalResponse With {
                                .CustomerID = x.CustomerID,
                                .CustomerName = x.CustomerName
                            }) _
                    .FirstOrDefault()

                If Not CurrentUserHasAccessToCustomerId(globalResponse.CustomerID) Then
                    globalResponse.Errors.Add(String.Format("Sorry, you are not authorized to view data for {0}",
                                                           globalResponse.CustomerName
                                                          )
                                            )
                End If

            Else
                globalResponse = New WidgetGlobalResponse()
            End If

            Return globalResponse
        End Function

        Private Function CurrentUserHasAccessToCustomerId(customerId As Integer?) As Boolean
            Return Not HttpContext.AcePumpUser().Profile.CustomerID.HasValue _
                        OrElse (customerId.HasValue AndAlso HttpContext.AcePumpUser().Profile.CustomerAccessList.ContainsValue(customerId))
        End Function
    End Class

    Public Class RecordGridCommonFilters
        Public Property StartDate As Date
        Public Property EndDate As Date

        Public Property CustomerIDs As New List(Of Integer)
        Public Property LeaseID As Integer?
        Public Property WellID As Integer?
        Public Property ReasonRepaired As String
        Public Property PartTemplateID As Integer?
        Public Property PartCategoryID As Integer?
    End Class
End Namespace