Imports AcePump.Common
Imports AcePump.Common.Soris
Imports AcePump.Domain.Models
Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Web.Controllers
Imports Kendo.Mvc.Extensions
Imports Kendo.Mvc.UI
Imports Yesod.Ef
Imports Yesod.Mvc

Namespace Areas.Employees.Controllers
    <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
    Public Class WellController
        Inherits AcePumpControllerBase

        Private Property WellGridRowMapper As New ModelMapper(Of Well, WellGridRowModel)(
            Function(x As Well) New WellGridRowModel With {
                .WellID = x.WellID,
                .WellNumber = x.WellNumber,
                .APINumber = x.APINumber,
                .APINumberRequired = x.Customer.APINumberRequired,
                .LeaseID = x.LeaseID,
                .Lease = x.Lease.LocationName,
                .CustomerID = x.CustomerID,
                .CustomerName = x.Customer.CustomerName,
                .Inactive = x.Inactive
            }
        )

        '
        ' POST: /Well/GetMergeInfo

        <HttpPost()>
        Public Function GetMergeInfo(id As Integer) As ActionResult
            Dim well = (From w In DataSource.WellLocations
                        Group Join p In DataSource.Pumps On p.InstalledInWellID Equals w.WellID Into installedPumps = Group
                        From installedPumpOrNull In installedPumps.DefaultIfEmpty()
                        Where w.WellID = id
                        Select New With {
                            .WellID = w.WellID,
                            .LeaseName = w.Lease.LocationName,
                            .WellNumber = w.WellNumber,
                            .InstalledPumpID = If(installedPumpOrNull IsNot Nothing, installedPumpOrNull.PumpID, Nothing),
                            .InstalledPumpNumber = If(installedPumpOrNull IsNot Nothing, installedPumpOrNull.PumpNumber, Nothing),
                            .Tickets = From ticket In w.DeliveryTickets
                                       Select ticket.DeliveryTicketID
                        }) _
                       .FirstOrDefault()

            If (well IsNot Nothing) Then
                Return Json(well)

            Else
                Return New HttpStatusCodeResult(400)
            End If
        End Function

        '
        ' POST: /Well/Merge

        <HttpPost()>
        Public Function Merge(targetWellId As Integer, targetWellMergeOption As String, otherWellId As Integer, otherWellMergeOption As String) As ActionResult
            Dim targetWellInstalledPump = DataSource.Pumps.FirstOrDefault(Function(x) x.InstalledInWellID = targetWellId)
            Dim otherWellInstalledPump = DataSource.Pumps.FirstOrDefault(Function(x) x.InstalledInWellID = otherWellId)

            If targetWellInstalledPump IsNot Nothing And targetWellMergeOption = "OverwriteTarget" And otherWellInstalledPump IsNot Nothing And otherWellMergeOption = "OverwriteTarget" Then
                Return New HttpStatusCodeResult(400)

            Else
                Dim tickets = DataSource.DeliveryTickets.Where(Function(x) x.WellID = otherWellId)
                For Each ticket In tickets
                    ticket.WellID = targetWellId
                Next

                If targetWellInstalledPump IsNot Nothing And targetWellMergeOption = "SetNull" Then
                    targetWellInstalledPump.InstalledInWellID = Nothing
                End If

                If otherWellInstalledPump IsNot Nothing And otherWellMergeOption = "SetNull" Then
                    otherWellInstalledPump.InstalledInWellID = Nothing

                ElseIf otherWellInstalledPump IsNot Nothing And otherWellMergeOption = "OverwriteTarget" Then
                    otherWellInstalledPump.InstalledInWellID = targetWellId
                End If

                Dim otherWell = DataSource.WellLocations.Find(otherWellId)
                DataSource.WellLocations.Remove(otherWell)

                DataSource.SaveChanges()

                Return New HttpStatusCodeResult(204)
            End If
        End Function

        '
        ' GET: /Well/[Index]

        <HttpGet()>
        Public Function Index() As ActionResult
            Return View()
        End Function

        '        
        ' GET: /Well/Create

        <HttpGet()>
        Public Function Create() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Well/Create

        <HttpPost()>
        Public Function Create(model As WellModel) As ActionResult
            If Not ModelState.IsValid() Then
                Return View(model)
            End If

            model.WellNumber = model.WellNumber.Trim()
            InternalCreate(model)

            If ModelState.IsValid Then
                Return RedirectToAction("Index")
            Else
                Return View(model)
            End If
        End Function

        '
        ' POST: /Well/JsonCreate

        <HttpPost()>
        Public Function JsonCreate(model As WellModel) As ActionResult
            If Not ModelState.IsValid() Then
                Return Json(model, ModelState)
            End If

            InternalCreate(model)

            Return Json(model, ModelState)
        End Function

        Private Sub InternalCreate(model As WellModel)
            checkWellConflicts(New CheckWellConflictModel With {
                .WellID = model.WellID,
                .WellNumber = model.WellNumber,
                .LeaseID = model.LeaseID,
                .CustomerID = model.CustomerID,
                .APINumber = model.APINumber,
                .IgnoreNoAPINumber = model.IgnoreNoAPINumber})

            If ModelState.IsValid Then
                Dim created As Well = DataSource.WellLocations.LoadNew(model)
                DataSource.SaveChanges()

                model.WellID = created.WellID
            End If
        End Sub

        Private Sub checkWellConflicts(model As CheckWellConflictModel)
            model.WellNumber = model.WellNumber.Trim()
            Dim conflicts = DataSource.WellLocations _
                                .Where(Function(x) (x.LeaseID = model.LeaseID And x.WellNumber = model.WellNumber And model.WellID <> x.WellID) Or (x.APINumber = model.APINumber And Not String.IsNullOrEmpty(x.APINumber) And model.WellID <> x.WellID)) _
                                .Select(Function(x) New With {
                                            .LeaseName = x.Lease.LocationName,
                                            .WellNumber = x.WellNumber,
                                            .ApiNumberConflict = (x.APINumber = model.APINumber),
                                            .WellNumberConflict = (x.LeaseID = model.LeaseID And x.WellNumber = model.WellNumber)
                                        }) _
                                .ToList()

            If conflicts.Any() Then
                Dim wellNumberConflict = conflicts.FirstOrDefault(Function(x) x.WellNumberConflict)
                Dim apiNumberConflict = conflicts.FirstOrDefault(Function(x) x.ApiNumberConflict)

                If wellNumberConflict IsNot Nothing Then
                    Dim msg As String = String.Format("There is already a well with this number at the {0} lease.", wellNumberConflict.LeaseName)
                    ModelState.AddModelError("WellNumber", msg)
                End If

                If apiNumberConflict IsNot Nothing Then
                    Dim msg As String = String.Format("Sorry, Well # {0} at the {1} Lease already has API Number {2}", apiNumberConflict.WellNumber, apiNumberConflict.LeaseName, model.APINumber)
                    ModelState.AddModelError("APINumber", msg)
                End If

            ElseIf String.IsNullOrEmpty(model.APINumber) Then
                Dim customer As Customer = DataSource.Customers.Find(model.CustomerID)
                If customer Is Nothing Then
                    ModelState.AddModelError("CustomerID", "Could not find chosen customer.")
                Else
                    If customer.APINumberRequired And (model.IgnoreNoAPINumber = False Or Not model.IgnoreNoAPINumber.HasValue) Then
                        ModelState.AddModelError("APINumber", "API Number is required for wells at " & customer.CustomerName)
                    End If
                End If
            End If
        End Sub

        '
        ' POST: /Well/List/customerId

        <HttpPost()>
        Public Function List(customerId As Integer?, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim wells As IQueryable(Of Well) = DataSource.WellLocations
            If customerId.HasValue Then
                wells = wells.Where(Function(x) x.CustomerID.Value = customerId.Value)
            End If


            Return Json(wells.Select(Function(w) New WellGridRowModel With {
                                            .WellID = w.WellID,
                                            .WellNumber = w.WellNumber,
                                            .APINumber = w.APINumber,
                                            .APINumberRequired = If(w.Customer Is Nothing, False, w.Customer.APINumberRequired),
                                            .Lease = w.Lease.LocationName,
                                            .LeaseID = w.LeaseID,
                                            .CustomerID = If(w.CustomerID.HasValue, w.CustomerID.Value, 0),
                                            .CustomerName = If(w.Customer Is Nothing, "", w.Customer.CustomerName),
                                            .Inactive = w.Inactive
                                         }) _
                             .ToDataSourceResult(req)
                        )
        End Function

        '
        ' GET: /Well/GetFiltered
        <HttpPost()>
        Public Function GetFiltered(term As String, customerId As Integer?, leaseId As Integer?, includeNonCustomerWells As Boolean?, includeOnlyActiveWells As Boolean?) As ActionResult
            Dim wells As IQueryable(Of Well) = DataSource.WellLocations

            If leaseId.HasValue Then
                wells = wells.Where(Function(w) w.LeaseID = leaseId.Value)
            End If

            If Not String.IsNullOrEmpty(term) Then
                wells = wells.Where(Function(w) w.WellNumber.Contains(term))
            End If

            If Not includeNonCustomerWells.HasValue OrElse Not includeNonCustomerWells.Value And customerId.HasValue Then
                wells = wells.Where(Function(w) w.CustomerID.HasValue AndAlso w.CustomerID.Value = customerId.Value)
            End If

            If includeOnlyActiveWells.HasValue AndAlso includeOnlyActiveWells = True Then
                wells = wells.Where(Function(w) w.Inactive = False)
            End If

            Dim projection = wells.Select(Function(w) New WellSelectorRowModel With {
                                        .WellID = w.WellID,
                                        .WellNumber = w.WellNumber,
                                        .LeaseID = w.LeaseID,
                                        .LeaseName = w.Lease.LocationName,
                                        .CustomerID = If(w.CustomerID.HasValue, w.CustomerID.Value, 0),
                                        .CustomerName = w.Customer.CustomerName,
                                        .CustomerOwnsWell = customerId.HasValue AndAlso w.CustomerID.HasValue AndAlso customerId.Value = w.CustomerID.Value
                                    }) _
                                    .AsEnumerable() _
                                    .OrderBy(Function(x) x.WellNumber, New NaturalStringComparer())

            Return Json(projection)
        End Function

        '
        ' POST: /Well/Transfer

        <HttpPost()>
        Public Function Transfer(wellId As Integer, targetCustomerId As Integer)
            Dim targetCustomer As Customer = DataSource.Customers.Find(targetCustomerId)
            If targetCustomer Is Nothing Then
                ModelState.AddModelError("Customer", "Could not transfer to that customer because it was not found!")
            End If

            Dim well As Well = DataSource.WellLocations.Find(wellId)
            If well Is Nothing Then
                ModelState.AddModelError("Well", "Could not transfer the well because it was not found!")
            Else
                well.Customer = targetCustomer
                DataSource.SaveChanges()
            End If

            Return Json(Nothing, ModelState)
        End Function

        '
        ' POST: /Well/Edit

        <HttpPost()>
        Public Function Edit(model As WellGridRowModel, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            If Not ModelState.IsValid Then
                Return Json({model}.ToDataSourceResult(req, ModelState))
            End If

            checkWellConflicts(New CheckWellConflictModel With {
                .WellID = model.WellID,
                .LeaseID = model.LeaseID,
                .WellNumber = model.WellNumber,
                .APINumber = model.APINumber,
                .CustomerID = model.CustomerID
                               })

            If ModelState.IsValid() Then
                Dim well As Well = DataSource.WellLocations.SingleOrDefault(Function(w) w.WellID = model.WellID)

                If well IsNot Nothing Then
                    well.WellNumber = model.WellNumber.Trim()
                    well.APINumber = model.APINumber
                    well.CustomerID = If(model.CustomerID = 0, Nothing, model.CustomerID)
                    well.LeaseID = model.LeaseID
                    well.Inactive = model.Inactive

                    DataSource.SaveChanges()
                End If

                Return Json({WellGridRowMapper.Convert(well)}.ToDataSourceResult(req, ModelState))
            Else
                Return Json({model}.ToDataSourceResult(req, ModelState))
            End If
        End Function

        Private Class CheckWellConflictModel
            Public Property WellID As Integer
            Public Property WellNumber As String
            Public Property LeaseID As Integer
            Public Property CustomerID As Integer
            Public Property APINumber As String
            Public Property IgnoreNoAPINumber As Boolean?
        End Class
    End Class
End Namespace