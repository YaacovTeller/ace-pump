Imports AcePump.Web.Controllers
Imports System.Linq.Expressions
Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Common
Imports Kendo.Mvc.Extensions
Imports Kendo.Mvc.UI
Imports System.Data.Entity
Imports AcePump.Domain.Models
Imports Yesod.Ef
Imports AcePump.Domain

Namespace Areas.Employees.Controllers
    <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
    Public Class AssemblyController
        Inherits AcePumpControllerBase

        Private ReadOnly Property AssemblyModelSelector As Expression(Of Func(Of AssemblyWithRelatedPart, AssemblyModel))
            Get
                Return Function(record As AssemblyWithRelatedPart) New AssemblyModel() With {
                    .AssemblyID = record.Assembly.AssemblyID,
                    .AssemblyNumber = record.Assembly.AssemblyNumber,
                    .Description = record.Assembly.Description,
                    .Category = If(record.Assembly.PartCategory IsNot Nothing, record.Assembly.PartCategory.CategoryName, ""),
                    .Discount = If(record.Assembly.Discount.HasValue, record.Assembly.Discount.Value, 0D),
                    .Markup = If(record.Assembly.Markup.HasValue, record.Assembly.Markup.Value, 0D),
                    .TotalPartsCost = If(record.Assembly.Parts.Any(), record.Assembly.Parts.Sum(Function(x) x.PartTemplate.Cost * x.PartsQuantity), 0D),
                    .TotalPartsResalePrice = If(record.Assembly.Parts.Any(), record.Assembly.Parts.Sum(Function(x) x.PartTemplate.Cost * x.PartsQuantity / (1D - x.PartTemplate.Markup)), 0D),
                    .ResalePrice = Udf.ClrRound_10_4(record.PartTemplate.Cost / (1D - record.PartTemplate.Markup)),
                    .RelatedPartTemplateID = record.PartTemplate.PartTemplateID
                }
            End Get
        End Property

        Private ReadOnly Property AssemblyPartListRowSelector As Expression(Of Func(Of AssemblyPartDef, AssemblyPartListRowViewModel))
            Get
                Return Function(x) New AssemblyPartListRowViewModel() With {
                                                               .AssemblyID = x.AssemblyID,
                                                               .AssemblyPartDefID = x.AssemblyPartDefID,
                                                               .Cost = x.PartTemplate.Cost * x.PartsQuantity,
                                                               .Description = x.PartTemplate.Description,
                                                               .PartTemplateNumber = x.PartTemplate.Number,
                                                               .PartTemplateID = If(x.PartTemplateID.HasValue, x.PartTemplateID.Value, 0),
                                                               .PartsQuantity = x.PartsQuantity,
                                                               .ResaleValue = Udf.ClrRound_10_4(x.PartTemplate.Cost / (1 - x.PartTemplate.Markup)),
                                                               .TotalResaleValue = Udf.ClrRound_10_4(x.PartsQuantity * (x.PartTemplate.Cost / (1 - x.PartTemplate.Markup))),
                                                               .SortOrder = x.SortOrder
                                                           }
            End Get
        End Property

        '
        ' GET: /Assembly/Details/id

        <HttpGet()> _
        Public Function Details(id As Integer) As ActionResult
            Dim assembly As AssemblyWithRelatedPart = AssemblyDbQuery(id)

            If assembly IsNot Nothing Then
                Dim model As AssemblyModel = GetAssemblyModel(assembly)

                Return View(model)

            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' GET: /Assembly/Create

        <HttpGet()> _
        Public Function Create() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Assembly/Create

        <HttpPost()> _
        Public Function Create(model As AssemblyModel) As ActionResult
            If ModelState.IsValid Then
                Dim hasConflict As Boolean = DataSource.PartTemplates.Any(Function(x) x.Number = model.AssemblyNumber)
                If hasConflict Then
                    ModelState.AddModelError("AssemblyNumber", "A part already exists with this assembly number. Assembly numbers must be unique.")
                Else
                    Dim assembly As Assembly = DataSource.Assemblies.LoadNew(model)
                    DataSource.PartTemplates.Add(New PartTemplate With {
                        .RelatedAssembly = assembly,
                        .Markup = model.Markup,
                        .Discount = model.Discount,
                        .Description = model.Description,
                        .Number = model.AssemblyNumber,
                        .Active = True,
                        .PriceLastUpdated = Today
                    })
                    DataSource.SaveChanges()

                    Return RedirectToAction("Edit", New With {.id = assembly.AssemblyID})
                End If
            End If

            Return View(model)
        End Function

        '
        ' POST: /Assembly/PartList/id

        <HttpPost()> _
        Public Function PartList(id As Integer, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return Json(DataSource.AssemblyPartDefs.Where(Function(x) x.AssemblyID = id) _
                                                   .Select(AssemblyPartListRowSelector) _
                                                   .ToDataSourceResult(req))
        End Function

        '
        ' POST: /Assembly/AddPart

        <HttpPost()> _
        Public Function AddPart(model As AssemblyPartListRowViewModel, Optional partDefIDTarget As Integer = 0) As ActionResult
            Dim insertPosition As Integer

            If partDefIDTarget > 0 Then
                insertPosition = DataSource.AssemblyPartDefs _
                                            .Where(Function(x) x.AssemblyPartDefID = partDefIDTarget) _
                                            .Select(Function(x) x.SortOrder) _
                                            .DefaultIfEmpty(0) _
                                            .FirstOrDefault()
                insertPosition = insertPosition + 1
            Else
                insertPosition = DataSource.AssemblyPartDefs _
                            .Where(Function(x) x.AssemblyPartDefID = model.AssemblyPartDefID) _
                            .Select(Function(x) x.SortOrder) _
                            .DefaultIfEmpty(0) _
                            .Min()
            End If

            MoveUpTemplatePartSort(model.AssemblyID, insertPosition)

            Dim partDef As AssemblyPartDef = DataSource.AssemblyPartDefs.LoadNew(model)
            partDef.SortOrder = insertPosition

            DataSource.SaveChanges()

            Return Json({GetAssemblyPartListModel(partDef)}.ToDataSourceResult(New DataSourceRequest()))
        End Function

        Private Sub MoveUpTemplatePartSort(assemblyID As Integer, startFrom As Integer)
            For Each partDef As AssemblyPartDef In DataSource.AssemblyPartDefs _
                                                .Where(Function(x) x.AssemblyID = assemblyID) _
                                                .OrderBy(Function(x) x.SortOrder)
                If partDef.SortOrder >= startFrom Then
                    partDef.SortOrder = partDef.SortOrder + 1
                End If
            Next
        End Sub

        '
        ' POST: /Assembly/UpdatePart

        <HttpPost()> _
        Public Function UpdatePart(model As AssemblyPartListRowViewModel) As ActionResult
            If DataSource.AssemblyPartDefs.LoadChanges(model) Then
                DataSource.SaveChanges()

                Dim updated As AssemblyPartDef = DataSource.AssemblyPartDefs.Find(model.AssemblyPartDefID)
                model = GetAssemblyPartListModel(updated)

            Else
                ViewData.ModelState.AddModelError("AssemblyPartDefID", "Could not find that part on the assembly to update.")
            End If

            Return Json({model}.ToDataSourceResult(New DataSourceRequest(), ModelState))
        End Function

        '
        ' POST: /Assembly/UpdatePartPosition

        <HttpPost()> _
        Public Function UpdatePartPosition(partDefIDToChange As Integer, partDefIDTarget As Integer) As ActionResult
            Dim toChange As AssemblyPartDef = DataSource.AssemblyPartDefs.Find(partDefIDToChange)
            Dim targetPosition As Integer

            If toChange Is Nothing Then
                Return Json(New With {.Success = False})
            End If

            If partDefIDTarget = 0 Then
                targetPosition = DataSource.AssemblyPartDefs _
                                            .Where(Function(x) x.AssemblyID = toChange.AssemblyID) _
                                            .Select(Function(x) x.SortOrder) _
                                            .DefaultIfEmpty(0) _
                                            .Min()
            Else
                Dim target As AssemblyPartDef = DataSource.AssemblyPartDefs.Find(partDefIDTarget)

                If target Is Nothing Then
                    Return Json(New With {.Success = False})
                Else
                    targetPosition = target.SortOrder + 1
                End If
            End If

            MoveUpTemplatePartSort(toChange.AssemblyID, targetPosition)

            toChange.SortOrder = targetPosition

            DataSource.SaveChanges()

            Return Json(New With {.Success = True})
        End Function

        '
        ' POST: /Assembly/SwapParts

        <HttpPost()> _
        Public Function SwapParts(firstPartDefId As Integer, secondPartDefId As Integer) As ActionResult
            Dim firstPartDef As AssemblyPartDef = DataSource.AssemblyPartDefs.SingleOrDefault(Function(x) x.AssemblyPartDefID = firstPartDefId)
            Dim secondPartDef As AssemblyPartDef = DataSource.AssemblyPartDefs.SingleOrDefault(Function(x) x.AssemblyPartDefID = secondPartDefId)

            If firstPartDef IsNot Nothing And secondPartDef IsNot Nothing Then
                Dim firstPartDefOriginalOrder As Integer = firstPartDef.SortOrder
                firstPartDef.SortOrder = secondPartDef.SortOrder
                secondPartDef.SortOrder = firstPartDefOriginalOrder

                DataSource.SaveChanges()

                Return Json(New With {.Success = True})

            Else
                Return Json(New With {.Success = False})
            End If
        End Function

        '
        ' POST: /Assembly/RemovePart

        <HttpPost()> _
        Public Function RemovePart(AssemblyPartDefID As Integer) As ActionResult
            Dim def As AssemblyPartDef = DataSource.AssemblyPartDefs.Find(AssemblyPartDefID)

            If def IsNot Nothing Then
                DataSource.AssemblyPartDefs.Remove(def)
                DataSource.SaveChanges()
            End If

            Return Json(New With {.Success = (def IsNot Nothing)})
        End Function

        '
        ' GET: /Assembly/Edit/id

        <HttpGet()> _
        Public Function Edit(id As Integer) As ActionResult
            Dim assembly As AssemblyWithRelatedPart = AssemblyDbQuery(id)

            If assembly IsNot Nothing Then
                Return View(GetAssemblyModel(assembly))

            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' POST: /Assembly/Edit

        <HttpPost()> _
        Public Function Edit(model As AssemblyModel) As ActionResult
            If Not ModelState.IsValid Then
                Return View(model)
            End If

            Dim record As AssemblyWithRelatedPart = AssemblyDbQuery(model.AssemblyID)
            If record IsNot Nothing Then
                Dim hasConflict As Boolean
                If record.Assembly.AssemblyNumber <> model.AssemblyNumber Then
                    hasConflict = DataSource.PartTemplates.Any(Function(x) x.Number = model.AssemblyNumber)
                End If

                If hasConflict Then
                    ModelState.AddModelError("AssemblyNumber", "A part already exists with this assembly number. Assembly numbers must be unique.")
                Else
                    If IsNumeric(model.Category) Then
                        record.Assembly.PartCategoryID = Integer.Parse(model.Category)
                    End If

                    record.Assembly.AssemblyNumber = model.AssemblyNumber
                    record.Assembly.Markup = model.Markup
                    record.Assembly.Discount = model.Discount
                    record.Assembly.Description = model.Description
                    record.PartTemplate.Number = model.AssemblyNumber
                    record.PartTemplate.Markup = model.Markup
                    record.PartTemplate.Discount = model.Discount
                    record.PartTemplate.Description = model.Description

                    record.PartTemplate.Cost = record.Assembly.Parts.Sum(Function(x) x.PartsQuantity * x.PartTemplate.Cost)
                    record.PartTemplate.PriceLastUpdated = Today

                    DataSource.SaveChanges()

                    Return RedirectToAction("Details", New With {.id = model.AssemblyID})
                End If
            Else
                ModelState.AddModelError("AssemblyID", "could not find that assembly to edit.")
            End If

            Return View(model)
        End Function

        '
        ' POST: /Assembly/StartsWith

        <HttpPost()> _
        Public Function StartsWith(term As String) As ActionResult
            If String.IsNullOrEmpty(term) Then
                term = Request.Form("filter[filters][0][value]")
            End If

            Return Json(DataSource.Assemblies _
                            .Where(Function(w) w.AssemblyNumber.StartsWith(term)) _
                            .Select(Function(x) New With {
                                        .AssemblyID = x.AssemblyID,
                                        .AssemblyNumber = x.AssemblyNumber
                                    })
                        )
        End Function

        '
        ' GET: /Assembly/[Index]

        <HttpGet()> _
        Public Function Index() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Assembly/List

        <HttpPost()> _
        Public Function List(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return Json(DataSource.AssembliesWithRelatedParts _
                            .Select(AssemblyModelSelector) _
                            .ToDataSourceResult(req)
                        )
        End Function

        '
        ' POST: /Assembly/EnsureCanDelete

        <HttpPost()> _
        Public Function EnsureCanDelete(id As Integer) As ActionResult
            CanDelete(id)

            Return Json({}, ModelState)
        End Function

        Private Sub CanDelete(id As Integer)
            Dim assemblyToDelete As AssemblyWithRelatedPart = DataSource.AssembliesWithRelatedParts.FirstOrDefault(Function(x) x.Assembly.AssemblyID = id)

            If assemblyToDelete Is Nothing Then
                ModelState.AddModelError("AssemblyID", "Could not find that assembly.")
            Else
                Const LINE_ITEM As Integer = 1
                Const INSPECTION As Integer = 2
                Const SPECIAL As Integer = 3
                Const PART_DEF As Integer = 4

                Dim lineItemConflicts As IQueryable(Of DeleteConflict) = DataSource.LineItems _
                                                                            .Where(Function(x) x.PartTemplateID.HasValue AndAlso x.PartTemplateID.Value = assemblyToDelete.PartTemplate.PartTemplateID) _
                                                                            .Select(Function(x) New DeleteConflict With {
                                                                                        .Text = x.DeliveryTicketID,
                                                                                        .Id = x.DeliveryTicketID,
                                                                                        .Type = LINE_ITEM
                                                                                    })

                Dim inspectionConflicts As IQueryable(Of DeleteConflict) = DataSource.PartInspections _
                                                                            .Where(Function(x) x.ParentAssemblyID = id _
                                                                                                Or x.PartFailedID = assemblyToDelete.PartTemplate.PartTemplateID _
                                                                                                Or x.PartReplacedID = assemblyToDelete.PartTemplate.PartTemplateID) _
                                                                            .Select(Function(x) New DeleteConflict With {
                                                                                        .Text = x.DeliveryTicketID,
                                                                                        .Id = x.DeliveryTicketID,
                                                                                        .Type = INSPECTION
                                                                                    })

                Dim customerSpecialConflicts As IQueryable(Of DeleteConflict) = DataSource.CustomerPartSpecials _
                                                                                .Where(Function(x) x.PartTemplateID = assemblyToDelete.PartTemplate.PartTemplateID) _
                                                                                .Select(Function(x) New DeleteConflict With {
                                                                                            .Text = x.Customer.CustomerName,
                                                                                            .Id = x.CustomerID,
                                                                                            .Type = SPECIAL
                                                                                        })

                Dim templatePartDefConflicts As IQueryable(Of DeleteConflict) = DataSource.TemplatePartDefs _
                                                                            .Where(Function(x) x.PartTemplateID = assemblyToDelete.PartTemplate.PartTemplateID) _
                                                                            .Select(Function(x) New DeleteConflict With {
                                                                                        .Text = x.PumpTemplateID,
                                                                                        .Id = x.PumpTemplateID,
                                                                                        .Type = PART_DEF
                                                                                    })

                Dim deleteConflicts As List(Of DeleteConflict) = lineItemConflicts _
                                                                    .Concat(inspectionConflicts) _
                                                                    .Concat(customerSpecialConflicts) _
                                                                    .Concat(templatePartDefConflicts) _
                                                                    .ToList()

                If deleteConflicts.Any(Function(x) x.Type = LINE_ITEM) Then
                    ModelState.AddModelError("lineItems", String.Join(",", deleteConflicts.Where(Function(x) x.Type = LINE_ITEM).Select(Function(x) x.Id).Distinct()))
                End If

                If deleteConflicts.Any(Function(x) x.Type = INSPECTION) Then
                    ModelState.AddModelError("inspections", String.Join(",", deleteConflicts.Where(Function(x) x.Type = INSPECTION).Select(Function(x) x.Id).Distinct()))
                End If

                If deleteConflicts.Any(Function(x) x.Type = SPECIAL) Then
                    ModelState.AddModelError("customerSpecials", String.Join(",", deleteConflicts.Where(Function(x) x.Type = SPECIAL).Select(Function(x) x.Id)))
                End If

                If deleteConflicts.Any(Function(x) x.Type = PART_DEF) Then
                    ModelState.AddModelError("templatePartDefs", String.Join(",", deleteConflicts.Where(Function(x) x.Type = PART_DEF).Select(Function(x) x.Id)))
                End If
            End If
        End Sub

        Private Class DeleteConflict
            Public Property Text As String
            Public Property Id As Integer
            Public Property Type As Integer
        End Class

        '
        ' POST: /Assembly/Delete

        <HttpPost()> _
        Public Function Delete(id As Integer) As ActionResult
            CanDelete(id)

            If ModelState.IsValid Then
                Dim assemblyPartDefsToDelete As List(Of AssemblyPartDef) = DataSource.AssemblyPartDefs.Where(Function(x) x.AssemblyID = id).ToList
                For Each partDef In assemblyPartDefsToDelete
                    DataSource.AssemblyPartDefs.Remove(partDef)
                Next

                Dim assemblyToDelete As AssemblyWithRelatedPart = DataSource.AssembliesWithRelatedParts _
                                                                  .FirstOrDefault(Function(x) x.Assembly.AssemblyID = id)
                For Each partDef As AssemblyPartDef In assemblyToDelete.Assembly.Parts
                    DataSource.AssemblyPartDefs.Remove(partDef)
                Next
                DataSource.PartTemplates.Remove(assemblyToDelete.PartTemplate)
                DataSource.Assemblies.Remove(assemblyToDelete.Assembly)

                DataSource.SaveChanges()
            End If

            Return Json({}, ModelState)
        End Function


        Private _ModelConversionLambda As Func(Of AssemblyWithRelatedPart, AssemblyModelBase)
        Private Function GetAssemblyModel(assembly As AssemblyWithRelatedPart) As AssemblyModel
            If _ModelConversionLambda Is Nothing Then
                _ModelConversionLambda = AssemblyModelSelector.Compile()
            End If

            Return _ModelConversionLambda(assembly)
        End Function

        Private _AssemblyPartListConversionLambda As Func(Of AssemblyPartDef, AssemblyPartListRowViewModel)
        Private Function GetAssemblyPartListModel(partDef As AssemblyPartDef) As AssemblyPartListRowViewModel
            If _AssemblyPartListConversionLambda Is Nothing Then
                _AssemblyPartListConversionLambda = AssemblyPartListRowSelector.Compile()
            End If

            Return _AssemblyPartListConversionLambda(partDef)
        End Function

        Private Function AssemblyDbQuery(id As Integer) As AssemblyWithRelatedPart
            Return DataSource.AssembliesWithRelatedParts.FirstOrDefault(Function(x) x.Assembly.AssemblyID = id)
        End Function
    End Class
End Namespace