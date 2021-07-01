Imports AcePump.Domain.Models
Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Common.Soris
Imports Yesod.Ef
Imports Kendo.Mvc.UI
Imports Kendo.Mvc.Extensions
Imports System.Data.Entity.Infrastructure


Namespace Areas.Employees.Controllers
    Public Class LeaseController
        Inherits AcePumpControllerBase

        '
        ' POST: /Lease/StartsWith

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
        <HttpPost()> _
        Public Function StartsWith(text As String) As ActionResult
            Return Json(DataSource.LeaseLocations.Where(Function(x) x.LocationName.StartsWith(text)) _
                                                 .Select(Function(x) New With {
                                                             .LeaseId = x.LeaseID,
                                                             .LeaseName = x.LocationName
                                                         }) _
                                                 .AsEnumerable() _
                                                 .OrderBy(Function(x) x.LeaseName, New NaturalStringComparer())
                        )
        End Function

        '
        ' POST: /Lease/GetFiltered

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
        <HttpPost()> _
        Public Function GetFiltered(term As String, customerId As Integer?, includeNonCustomerLeases As Boolean?) As ActionResult
            Dim leaseLocations As IQueryable(Of Lease) = DataSource.LeaseLocations
            Dim locationsCustomerHasLeases As IQueryable(Of Lease) = From well In DataSource.WellLocations
                                                                     Where customerId.HasValue AndAlso well.CustomerID.HasValue AndAlso customerId.Value = well.CustomerID
                                                                     Select well.Lease

            If Not String.IsNullOrEmpty(term) Then
                leaseLocations = leaseLocations.Where(Function(l) l.LocationName.Contains(term))
            End If

            If Not includeNonCustomerLeases.HasValue OrElse Not includeNonCustomerLeases.Value Then
                leaseLocations = leaseLocations.Where(Function(l) locationsCustomerHasLeases.Contains(l))
            End If

            Return Json(leaseLocations _
                        .Select(Function(x) New LeaseSelectorRowModel With {
                                    .LeaseID = x.LeaseID,
                                    .LeaseName = x.LocationName,
                                    .CustomerHasWellsAtLease = locationsCustomerHasLeases.Contains(x)
                                }) _
                        .AsEnumerable() _
                        .OrderBy(Function(x) x.LeaseName, New NaturalStringComparer())
                        )
        End Function

        '
        ' POST: /Lease/List

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
        <HttpPost()> _
        Public Function List(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim leaseLocations As DbQuery(Of Lease) = DataSource.LeaseLocations

            Return Json(leaseLocations.Select(Function(l) New With {
                                                  .LeaseID = l.LeaseID,
                                                  .LocationName = l.LocationName}) _
            .ToDataSourceResult(req)
                        )
        End Function

        '
        ' GET: /Lease/[Index]

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
        <HttpGet()> _
        Public Function Index() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Lease/KendoCreate

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
        <HttpPost()> _
        Public Function KendoCreate(model As LeaseGridRowModel) As ActionResult
            If ModelState.IsValid Then
                InternalCreate(model)
            End If

            Return Json({model}.ToDataSourceResult(New DataSourceRequest, ModelState))
        End Function

        '
        ' POST: /Lease/JsonCreate

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
        <HttpPost()> _
        Public Function JsonCreate(model As LeaseGridRowModel) As ActionResult
            If ModelState.IsValid Then
                InternalCreate(model)
            End If

            Return Json(model, ModelState)
        End Function

        Private Sub InternalCreate(model As LeaseGridRowModel)
            If DataSource.LeaseLocations.Any(Function(x) x.LocationName = model.LocationName) Then
                ModelState.AddModelError("LocationName", "There is already a lease with that name.")
            Else
                Dim lease As Lease = DataSource.LeaseLocations.LoadNew(model)
                DataSource.SaveChanges()

                model.LeaseID = lease.LeaseID
            End If
        End Sub

        '
        ' POST: /Lease/Edit

        <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
        <HttpPost()> _
        Public Function Edit(model As LeaseGridRowModel, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            If ModelState.IsValid Then
                Dim lease As Lease = DataSource.LeaseLocations.SingleOrDefault(Function(l) l.LeaseID = model.LeaseID)

                If lease IsNot Nothing Then
                    If DataSource.LeaseLocations.Any(Function(x) x.LocationName = model.LocationName) Then
                        ModelState.AddModelError("LocationName", "There is already a lease with that name.")
                    Else
                        lease.LocationName = model.LocationName
                        DataSource.SaveChanges()
                    End If
                End If
            End If

            Return Json({model}.ToDataSourceResult(req, ModelState))
        End Function

    End Class
End Namespace