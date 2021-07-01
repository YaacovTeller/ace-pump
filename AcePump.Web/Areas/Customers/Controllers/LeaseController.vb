Imports AcePump.Domain.Models
Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports System.Runtime.Remoting.Contexts
Imports Kendo.Mvc.UI
Imports Kendo.Mvc.Extensions
Imports System.Data.Entity.Infrastructure
Imports AcePump.Common.Soris

Namespace Areas.Customers.Controllers

    <Authorize()> _
    Public Class LeaseController
        Inherits AcePumpControllerBase

        '
        ' GET: /Lease/Index

        Function Index() As ActionResult
            Return RedirectToAction("Index", "Pump")
        End Function

        '
        ' POST: /Lease/GetFiltered

        <Authorize(Roles:=AcePumpSecurityRoles.Customer)> _
        <HttpPost()> _
        Public Function GetFiltered(term As String) As ActionResult
            Dim accessibleCustomerIds As List(Of Integer) = HttpContext.AcePumpUser.Profile.CustomerAccessList.Values.ToList()
            Dim accessibleLeases As IQueryable(Of Lease) = DataSource.WellLocations _
                                                                .Where(Function(x) accessibleCustomerIds.Contains(x.CustomerID)) _
                                                                .Select(Function(x) x.Lease)

            If Not String.IsNullOrEmpty(term) Then
                accessibleLeases = accessibleLeases.Where(Function(l) l.LocationName.Contains(term))
            End If

            Return Json(accessibleLeases _
                            .Distinct() _
                            .OrderBy(Function(l) l.LocationName) _
                            .Select(Function(c) New With {
                                        .Id = c.LeaseID,
                                        .Name = c.LocationName
                                    }) _
                            .AsEnumerable() _
                            .OrderBy(Function(x) x.Name, New NaturalStringComparer())
                        )
        End Function
    End Class
End Namespace
