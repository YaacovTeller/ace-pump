Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports System.Data.Entity.Infrastructure
Imports Kendo.Mvc.Extensions
Imports Kendo.Mvc.UI
Imports AcePump.Domain.Models
Imports Yesod.Ef

Namespace Areas.Employees.Controllers
    <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
    Public Class PartCategoryController
        Inherits AcePumpControllerBase

        '
        ' GET: /PartCategory/[Index]

        <HttpGet()> _
        Public Function Index() As ActionResult
            Return View()
        End Function

        '
        ' POST: /PartCategory/List

        <HttpPost()> _
        Public Function List(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return Json(DataSource.PartCategories.Select(Function(x) New PartCategoryGridRowModel With {
                                                             .PartCategoryID = x.PartCategoryID,
                                                             .CategoryName = x.CategoryName
                                                         }) _
                                                 .ToDataSourceResult(req)
                        )
        End Function

        '
        ' POST: /PartCategory/Edit

        <HttpPost()> _
        Public Function Edit(model As PartCategoryGridRowModel) As ActionResult
            If DataSource.PartCategories.LoadChanges(model) Then
                DataSource.SaveChanges()
            Else
                ModelState.AddModelError("PartCategoryID", "Could not find the category to update.")
            End If

            Return Json({model}.ToDataSourceResult(New DataSourceRequest, ModelState))
        End Function

        '
        ' POST: /PartCategory/Create

        <HttpPost()> _
        Public Function Create(model As PartCategoryGridRowModel) As ActionResult
            Dim category As PartCategory = DataSource.PartCategories.LoadNew(model)
            DataSource.SaveChanges()
            model.PartCategoryID = category.PartCategoryID

            Return Json({model}.ToDataSourceResult(New DataSourceRequest, ModelState))
        End Function

        '
        ' POST: /PartCategory/Delete

        <HttpPost()> _
        Public Function Delete(model As PartCategoryGridRowModel) As ActionResult
            Dim category As PartCategory = DataSource.PartCategories.Find(model.PartCategoryID)

            If category IsNot Nothing Then
                DataSource.PartCategories.Remove(category)
                DataSource.SaveChanges()

            Else
                ModelState.AddModelError("PartCategoryID", "Could not find that category to delete.")
            End If

            Return Json({}.ToDataSourceResult(New DataSourceRequest(), ModelState))
        End Function

        '
        ' POST: /PartCategory/StartsWith

        <HttpPost()> _
        Public Function StartsWith(term As String) As ActionResult
            If String.IsNullOrEmpty(term) Then
                term = Request.Form("filter[filters][0][value]")
            End If

            Dim categoryQuery As DbQuery(Of PartCategory) = DataSource.PartCategories

            If Not String.IsNullOrEmpty(term) Then
                categoryQuery = categoryQuery.Where(Function(x) x.CategoryName.StartsWith(term))
            End If

            Return Json(categoryQuery.Select(Function(w) New With {
                                        .CategoryID = w.PartCategoryID,
                                        .CategoryName = w.CategoryName
                                        })
                        )
        End Function
    End Class
End Namespace