Imports AcePump.Domain.Models
Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports Kendo.Mvc.UI
Imports Kendo.Mvc.Extensions
Imports System.Data.Entity.Infrastructure
Imports AcePump.Common.Soris

Namespace Areas.Customers.Controllers
    <Authorize()> _
    Public Class WellController
        Inherits AcePumpControllerBase

        '
        ' GET: /Well/Index
        Function Index() As ActionResult
            Return RedirectToAction("Index", "Pump")
        End Function

        '
        ' GET: /Well/GetFiltered
        <Authorize(Roles:=AcePumpSecurityRoles.Customer)> _
        <HttpPost()> _
        Public Function GetFiltered(term As String, leaseId As Integer?) As ActionResult
            Dim accessibleCustomerIds As List(Of Integer) = HttpContext.AcePumpUser.Profile.CustomerAccessList.Values.ToList()
            Dim accessibleWells As IQueryable(Of Well) = DataSource.WellLocations _
                                                                .Where(Function(x) x.CustomerID.HasValue AndAlso accessibleCustomerIds.Contains(x.CustomerID.Value))

            If leaseId.HasValue Then
                accessibleWells = accessibleWells.Where(Function(w) w.LeaseID = leaseId.Value)
            End If

            If Not String.IsNullOrEmpty(term) Then
                accessibleWells = accessibleWells.Where(Function(w) w.WellNumber.Contains(term))
            End If

            Return Json(accessibleWells _
                            .OrderBy(Function(w) w.Lease.LocationName) _
                            .Select(Function(w) New With {
                                        .WellId = w.WellID,
                                        .WellNumber = w.WellNumber,
                                        .LeaseAndWell = w.Lease.LocationName & " " & w.WellNumber
                                    }) _
                            .AsEnumerable() _
                            .OrderBy(Function(x) x.WellNumber, New NaturalStringComparer())
                        )
        End Function

    End Class
End Namespace
