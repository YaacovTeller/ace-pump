Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq.Expressions
Imports AcePump.Common
Imports AcePump.Domain
Imports AcePump.Domain.Models
Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Web.Controllers
Imports DelegateDecompiler
Imports Kendo.Mvc.Extensions
Imports Kendo.Mvc.UI
Imports Yesod.Ef
Imports Yesod.Ef.CustomColumns
Imports Yesod.Kendo

Namespace Areas.Employees.Controllers
    <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
    Public Class PartTemplateController
        Inherits AcePumpControllerBase

        Private ReadOnly Property PartModelSelector As Expression(Of Func(Of PartTemplate, PartTemplateModel))
            Get
                Return Function(partTemplate As PartTemplate) New PartTemplateModel() With {
                    .PartTemplateID = partTemplate.PartTemplateID,
                    .AssemblyID = If(partTemplate.RelatedAssemblyID.HasValue, partTemplate.RelatedAssemblyID.Value, New Nullable(Of Integer)()),
                    .AssemblyPartNumber = If(partTemplate.RelatedAssembly IsNot Nothing, partTemplate.RelatedAssembly.AssemblyNumber, ""),
                    .Manufacturer = partTemplate.Manufacturer,
                    .ManufacturerPartNumber = partTemplate.ManufacturerPartNumber,
                    .Material = If(partTemplate.Material IsNot Nothing, partTemplate.Material.MaterialName, ""),
                    .Number = partTemplate.Number,
                    .Description = partTemplate.Description,
                    .SoldByOption = If(partTemplate.SoldByOption IsNot Nothing, partTemplate.SoldByOption.Description, ""),
                    .Category = If(partTemplate.PartCategory IsNot Nothing, partTemplate.PartCategory.CategoryName, ""),
                    .Active = partTemplate.Active,
                    .Cost = partTemplate.Cost,
                    .Discount = partTemplate.Discount,
                    .Markup = partTemplate.Markup,
                    .Taxable = partTemplate.Taxable,
                    .PriceLastUpdated = partTemplate.PriceLastUpdated
                }
            End Get
        End Property

        Private ReadOnly Property PartGridRowModelSelector As Expression(Of Func(Of PartTemplate, PartTemplateGridRowModel))
            Get
                Return Function(p) New PartTemplateGridRowModel() With {
                                        .PartTemplateID = p.PartTemplateID,
                                        .Number = p.Number,
                                        .Description = p.Description,
                                        .SoldByOption = If(p.SoldByOption IsNot Nothing, p.SoldByOption.Description, Nothing),
                                        .SoldByOptionID = If(p.SoldByOptionID.HasValue, p.SoldByOptionID.Value, 0),
                                        .Cost = p.Cost,
                                        .Discount = p.Discount,
                                        .Markup = p.Markup,
                                        .Category = If(p.PartCategory.CategoryName, ""),
                                        .Active = p.Active,
                                        .Taxable = p.Taxable,
                                        .IsAssembly = If(p.RelatedAssembly IsNot Nothing, True, False),
                                        .ListPrice = Udf.ClrRound_10_4(p.ListPrice.Computed()),
                                        .ResalePrice = Udf.ClrRound_10_4(p.ResalePrice.Computed()),
                                        .PriceLastUpdated = p.PriceLastUpdated,
                                        .Manufacturer = p.Manufacturer,
                                        .ManufacturerPartNumber = p.ManufacturerPartNumber
                                    }

            End Get
        End Property

        '
        ' GET: /PartTemplate/Details/id

        <HttpGet()>
        Public Function Details(id As Integer) As ActionResult
            Dim partTemplate As PartTemplate = DataSource.PartTemplates _
                                .Include(Function(x) x.RelatedAssembly) _
                                .Include(Function(x) x.PartCategory) _
                                .Include(Function(x) x.Material) _
                                .Include(Function(x) x.SoldByOption) _
                                .SingleOrDefault(Function(x) x.PartTemplateID = id)

            If partTemplate IsNot Nothing Then
                Dim model As PartTemplateModel = GenerateViewModel(partTemplate)

                Return View(model)
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' POST: /PartTemplate/Create

        <HttpPost()>
        Public Function Create(model As PartTemplateGridRowModel) As ActionResult
            model.Number = model.Number.Trim()

            If ModelState.IsValid Then
                Dim hasConflict As Boolean = DataSource.PartTemplates.Any(Function(x) x.Number = model.Number)
                If hasConflict Then
                    Dim msg As String = String.Format("There is already a part with this number. Part numbers must be unique.")
                    ModelState.AddModelError("Number", msg)
                ElseIf (model.Cost / (1 - model.Markup) / (1 - model.Discount)) > 1000000 Then
                    ModelState.AddModelError("Price", "The resale price is above one million, which is not possible. Please change one of the values.")
                Else
                    Dim partTemplate As PartTemplate = DataSource.PartTemplates.LoadNew(model)

                    DataSource.SaveChanges()

                    model.PartTemplateID = partTemplate.PartTemplateID
                    model.Category = If(partTemplate.PartCategory IsNot Nothing, partTemplate.PartCategory.CategoryName, "")
                    model.SoldByOption = If(partTemplate.SoldByOption IsNot Nothing, partTemplate.SoldByOption.Description, "")
                    model.Taxable = partTemplate.Taxable
                    model.ListPrice = partTemplate.ListPrice
                    model.ResalePrice = partTemplate.ResalePrice
                    model.PriceLastUpdated = Today
                End If
            End If

            Return Json({model}.ToDataSourceResult(New DataSourceRequest, ModelState))
        End Function

        '
        ' GET: /PartTemplate/Edit

        <HttpGet()>
        Public Function Edit(id As Integer) As ActionResult
            Dim r As ActionResult = Details(id)

            Dim asView = TryCast(r, ViewResult)
            If (asView IsNot Nothing) Then
                asView.ViewName = "Edit"
            End If

            Return r
        End Function

        '
        ' POST: /PartTemplate/Edit

        <HttpPost()>
        Public Function Edit(model As PartTemplateModel) As ActionResult
            If Not ModelState.IsValid Then
                Return View(model)
            End If

            If DataSource.PartTemplates.LoadChanges(model) Then
                Dim updated As PartTemplate = DataSource.PartTemplates.Find(model.PartTemplateID)
                updated.RelatedAssemblyID = model.AssemblyID
                DataSource.SaveChanges()

                Return RedirectToAction("Details", New With {.id = model.PartTemplateID})

            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' POST: /PartTemplate/JsonEdit

        <HttpPost()>
        Public Function JsonEdit(model As PartTemplateGridRowModel) As ActionResult
            model.Number = model.Number.Trim

            If ModelState.IsValid Then
                Dim partTemplate As PartTemplate = DataSource.PartTemplates.Find(model.PartTemplateID)
                Dim hasConflict As Boolean
                Dim priceChanged As Boolean
                If partTemplate.Number <> model.Number Then
                    hasConflict = DataSource.PartTemplates.Any(Function(x) x.Number = model.Number And x.PartTemplateID <> model.PartTemplateID)
                End If

                If model.Cost <> partTemplate.Cost Then
                    priceChanged = True
                End If

                If hasConflict Then
                    Dim msg As String = String.Format("There is already a part with this number. Part numbers must be unique.")
                    ModelState.AddModelError("Number", msg)
                ElseIf (model.Cost / (1 - model.Markup) / (1 - model.Discount)) > 1000000 Then
                    ModelState.AddModelError("Price", "The resale price is above one million, which is not possible. Please change one of the values.")
                Else
                    DataSource.PartTemplates.LoadChanges(model)

                    If priceChanged Then
                        partTemplate.PriceLastUpdated = Today
                    End If

                    DataSource.SaveChanges()

                    model.SoldByOption = If(partTemplate.SoldByOption IsNot Nothing, partTemplate.SoldByOption.Description, "")
                    model.Category = If(partTemplate.PartCategory IsNot Nothing, partTemplate.PartCategory.CategoryName, "")
                    model.Taxable = partTemplate.Taxable
                    model.ListPrice = partTemplate.ListPrice
                    model.ResalePrice = partTemplate.ResalePrice
                    model.PriceLastUpdated = partTemplate.PriceLastUpdated
                End If
            Else
                ViewData.ModelState.AddModelError("PartTemplateID", "Could not find that partTemplate to edit.")
            End If

            Return Json({model}.ToDataSourceResult(New DataSourceRequest(), ModelState))
        End Function


        ' POST: /PartTemplate/BulkEditCost

        <HttpPost()>
        Public Function BulkEditCost(<DataSourceRequest()> req As DataSourceRequest, <Bind(Prefix:="models")> parts As IEnumerable(Of PartTemplateGridRowModel)) As ActionResult
            If parts Is Nothing Then
                ModelState.AddModelError("parts", "There are no parts to change cost.")
            Else
                'Dim customerIds As IQueryable(Of Integer) = parts.Select(Function(x) x.CustomerID).AsQueryable()
                Dim partTemplateIds As IQueryable(Of Integer) = parts.Select(Function(x) x.PartTemplateID).AsQueryable()
                DataSource.PartTemplates _
                    .Where(Function(x) partTemplateIds.Contains(x.PartTemplateID)) _
                    .Load()
                '        If DataSource.PartTemplates.LoadChanges(parts) Then
                For Each viewModel As PartTemplateGridRowModel In parts
                    Dim updated As PartTemplate = DataSource.PartTemplates.Where(Function(x) x.PartTemplateID.Equals(viewModel.PartTemplateID))
                    updated.Cost = viewModel.Cost
                Next

                If ModelState.IsValid Then
                    DataSource.SaveChanges()
                End If
            End If

            Return Json({}, ModelState)
        End Function
        '
        ' GET: /PartTemplate/[Index]

        <HttpGet()>
        Public Function Index() As ActionResult
            Return View()
        End Function

        '
        ' POST: /PartTemplate/List

        <HttpPost()>
        Public Function List(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            req.SetDefaultSort("PartTemplateID")
            Dim result = DataSource.PartTemplates _
                            .Include(Function(x) x.PartCategory) _
                            .Select(PartGridRowModelSelector) _
                            .Decompile() _
                            .ToDataSourceResultExt(req)
            Return Json(result)
        End Function

        '
        ' POST: /PartTemplate/StartsWith

        <HttpPost()>
        Public Function StartsWith(text As String) As ActionResult
            Return Json(DataSource.PartTemplates _
                            .Where(Function(w) w.Number.StartsWith(text)) _
                            .Where(Function(w) w.Active = True) _
                            .Select(Function(w) New SelectPartByNumberViewModel With {
                                .PartTemplateID = w.PartTemplateID,
                                .PartTemplateNumber = w.Number
                                })
                        )
        End Function

        '
        ' POST: /PartTemplate/Contains

        <HttpPost()>
        Public Function Contains(categoryId As Integer?, term As String) As ActionResult
            Dim filtered As DbQuery(Of PartTemplate) = DataSource.PartTemplates.Where(Function(x) x.Number.Contains(term))

            If categoryId.HasValue Then
                filtered = filtered.Where(Function(x) x.PartCategoryID = categoryId.Value)
            End If
            Return Json(filtered _
                            .Where(Function(w) w.Active = True) _
                            .Select(Function(w) New PartTemplateDescriptionDisplayDto With {
                                       .PartTemplateID = w.PartTemplateID,
                                       .PartTemplateDescription = If(w.Number, "") & " " & If(w.Description, "")
                                    })
                        )
        End Function

        '
        ' GET: /Part/Lookup

        <HttpGet()>
        Public Function Lookup(id As Integer) As ActionResult
            Dim partTemplate As PartTemplate = DataSource.PartTemplates.Find(id)

            If partTemplate IsNot Nothing Then
                Return Json(New With {
                                .PartTemplateNumber = partTemplate.Number,
                                .Description = partTemplate.Description,
                                .Taxable = partTemplate.Taxable,
                                .Cost = partTemplate.Cost,
                                .ResaleValue = partTemplate.Cost / (1 - partTemplate.Markup)
                            },
                            JsonRequestBehavior.AllowGet)

            Else
                Return Json(Nothing, JsonRequestBehavior.AllowGet)
            End If
        End Function

        '
        ' POST: /PartTemplate/ConvertToAssembly

        <HttpPost()>
        Public Function ConvertToAssembly(id As Integer) As ActionResult
            Dim partToConvert As PartTemplate = DataSource.PartTemplates.Find(id)

            If partToConvert IsNot Nothing Then
                If partToConvert.RelatedAssembly Is Nothing Then
                    Dim assembly As New Assembly With {
                        .PartCategoryID = partToConvert.PartCategoryID,
                        .Markup = partToConvert.Markup,
                        .Discount = partToConvert.Discount,
                        .Description = partToConvert.Description,
                        .AssemblyNumber = partToConvert.Number
                    }

                    partToConvert.RelatedAssembly = assembly

                    DataSource.SaveChanges()

                    Return Json(New With {.newAssemblyID = assembly.AssemblyID}, ModelState)
                Else
                    ModelState.AddModelError("partTemplateID", "This part is already an assembly.")
                End If
            Else
                ModelState.AddModelError("partTemplateID", "Could not find that part.")
            End If

            Return Json({}, ModelState)
        End Function

        '
        ' POST: /PartTemplate/ExcelExport

        <HttpPost()>
        Public Function ExcelExport(contentType As String, base64 As String, fileName As String) As ActionResult
            Dim fileContents = Convert.FromBase64String(base64)

            Return File(fileContents, contentType, fileName)
        End Function

        Private _PartModelSelectorLambda As Func(Of PartTemplate, PartTemplateModel)
        Private Function GenerateViewModel(part As PartTemplate) As PartTemplateModel
            If _PartModelSelectorLambda Is Nothing Then
                _PartModelSelectorLambda = PartModelSelector.Compile()
            End If

            Return _PartModelSelectorLambda(part)
        End Function

        Private _PartGridRowSelectorLambda As Func(Of PartTemplate, PartTemplateGridRowModel)
        Private Function GenerateGridRowModel(part As PartTemplate) As PartTemplateGridRowModel
            If _PartGridRowSelectorLambda Is Nothing Then
                _PartGridRowSelectorLambda = PartGridRowModelSelector.Compile()
            End If

            Return _PartGridRowSelectorLambda(part)
        End Function
    End Class
End Namespace