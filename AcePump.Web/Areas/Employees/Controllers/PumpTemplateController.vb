Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Common
Imports AcePump.Domain
Imports AcePump.Web.Controllers
Imports System.Data.Entity
Imports Kendo.Mvc.Extensions
Imports Kendo.Mvc.UI
Imports AcePump.Domain.Models
Imports AcePump.Domain.ReportDefinitions
Imports Yesod.Mvc
Imports Yesod.Ef
Imports Yesod.Kendo


Namespace Areas.Employees.Controllers
    <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
    Public Class PumpTemplateController
        Inherits AcePumpControllerBase

        Private _TemplatePartDefManager As KendoGridRequestManager(Of TemplatePartDef, TemplatePartListRowViewModel)
        Private ReadOnly Property TemplatePartDefManager As KendoGridRequestManager(Of TemplatePartDef, TemplatePartListRowViewModel)
            Get
                If _TemplatePartDefManager Is Nothing Then
                    _TemplatePartDefManager = New KendoGridRequestManager(Of TemplatePartDef, TemplatePartListRowViewModel)(
                        DataSource,
                        Function(x As TemplatePartDef) New TemplatePartListRowViewModel With {
                            .Cost = x.PartTemplate.Cost,
                            .Description = x.PartTemplate.Description,
                            .PartTemplateID = x.PartTemplateID,
                            .PartTemplateNumber = x.PartTemplate.Number,
                            .PumpTemplateID = x.PumpTemplateID,
                            .Quantity = x.Quantity,
                            .ResaleValue = Udf.ClrRound_10_4(x.PartTemplate.Cost / (1 - x.PartTemplate.Markup)),
                            .SortOrder = If(x.SortOrder.HasValue, x.SortOrder.Value, 0),
                            .TemplatePartDefID = x.TemplatePartDefID,
                            .TotalResaleValue = Udf.ClrRound_10_4(x.Quantity * x.PartTemplate.Cost / (1 - x.PartTemplate.Markup))
                        },
                        Function(x) x.PumpTemplateID,
                        Me
                    )
                End If

                Return _TemplatePartDefManager
            End Get
        End Property

        '
        ' GET: /Pump/Template/[Index]

        <HttpGet()> _
        Public Function Index() As ActionResult
            Return View()
        End Function

        '
        ' GET: /Pump/Template/Details/id

        <HttpGet()>
        Public Function Details(id As Integer) As ActionResult
            Dim pumpTemplate As PumpTemplate = DataSource _
                                                .PumpTemplates _
                                                .Include(Function(x) x.Parts.Select(Function(y) y.PartTemplate)) _
                                                .SingleOrDefault(Function(x) x.PumpTemplateID = id)

            If pumpTemplate IsNot Nothing Then
                Dim model As PumpTemplateViewModel = GenerateViewModel(pumpTemplate)

                Return View(model)
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        ''
        '' GET: /Pump/Template/DetailsJSON/id

        '<HttpGet()>
        'Public Function DetailsJSON(id As Integer) As JsonResult

        '    Dim pumpTemplate As PumpTemplate = DataSource _
        '                                        .PumpTemplates _
        '                                        .Include(Function(x) x.Parts) _
        '                                        .SingleOrDefault(Function(x) x.PumpTemplateID = id)
        '    If pumpTemplate IsNot Nothing Then
        '        Return Json(pumpTemplate.Parts.ToList(), "json")
        '    Else
        '        '    Return RedirectToAction("Index")
        '    End If
        'End Function

        '
        ' POST: /Pump/Template/StartsWith

        <HttpPost()> _
        Public Function StartsWith(text As String) As ActionResult
            If String.IsNullOrEmpty(text) Then
                text = Request.Form("filter[filters][0][value]")
            End If

            Dim matches = DataSource.PumpTemplates.Select(Function(x) New StartsWithProjection With {
                                                            .Id = x.PumpTemplateID,
                                                            .Display = x.ConciseSpecificationSummary
                                                          }) _
                                                  .Where(Function(x) x.Id.StartsWith(text))

            Return Json(matches)
        End Function

        Private Class StartsWithProjection
            Public Property Id As String
            Public Property Display As String
        End Class

        Private Function GenerateViewModel(pumpTemplate As PumpTemplate) As PumpTemplateViewModel
            Dim model As New PumpTemplateViewModel() With {
                .BallsAndSeats = pumpTemplate.BallsAndSeats,
                .Barrel = pumpTemplate.Barrel,
                .Collet = pumpTemplate.Collet,
                .HoldDownType = pumpTemplate.HoldDownType,
                .KnockOut = pumpTemplate.KnockOut,
                .LowerExtension = pumpTemplate.LowerExtension,
                .OnOffTool = pumpTemplate.OnOffTool,
                .Plunger = pumpTemplate.Plunger,
                .PonyRods = pumpTemplate.PonyRods,
                .PumpBoreBasic = pumpTemplate.PumpBoreBasic,
                .PumpTemplateID = pumpTemplate.PumpTemplateID,
                .PumpType = pumpTemplate.PumpType,
                .Seating = pumpTemplate.Seating,
                .SpecialtyItems = pumpTemplate.SpecialtyItems,
                .StandingValve = pumpTemplate.StandingValve,
                .StandingValveCages = pumpTemplate.StandingValveCages,
                .Strainers = pumpTemplate.Strainers,
                .TopSeals = pumpTemplate.TopSeals,
                .TravellingCages = pumpTemplate.TravellingCages,
                .TubingSize = pumpTemplate.TubingSize,
                .UpperExtension = pumpTemplate.UpperExtension,
                .ConciseSpecificationSummary = pumpTemplate.ConciseSpecificationSummary,
                .VerboseSpecificationSummary = pumpTemplate.VerboseSpecificationSummary,
                .MarkupRate = If(pumpTemplate.Markup.HasValue, pumpTemplate.Markup.Value, 0D),
                .DiscountRate = If(pumpTemplate.Discount.HasValue, pumpTemplate.Discount.Value, 0D)
            }

            Dim percentOfResaleWhichIsMarkup As Decimal
            Dim resaleValue As Decimal
            For Each partDef As TemplatePartDef In pumpTemplate.Parts
                percentOfResaleWhichIsMarkup = 1D - partDef.PartTemplate.Markup
                resaleValue = partDef.PartTemplate.Cost / percentOfResaleWhichIsMarkup

                model.TotalPartCost += partDef.PartTemplate.Cost * partDef.Quantity
                model.TotalPartResale += (resaleValue * partDef.Quantity)
            Next

            percentOfResaleWhichIsMarkup = 1D - model.MarkupRate
            model.ResalePrice = model.TotalPartCost / percentOfResaleWhichIsMarkup

            Dim percentOfListPriceWhichWillBeDiscounted As Decimal = 1D - model.DiscountRate
            model.ListPrice = model.ResalePrice / percentOfListPriceWhichWillBeDiscounted

            Return model
        End Function

        '
        ' GET: /Pump/Template/Edit/id

        <HttpGet()> _
        Public Function Edit(id As Integer) As ActionResult
            Dim r As ActionResult = Details(id)

            Dim asView = TryCast(r, ViewResult)
            If (asView IsNot Nothing) Then
                asView.ViewName = "Edit"
            End If

            Return r
        End Function

        '
        ' POST: /Pump/Template/Edit

        <HttpPost()>
        Public Function Edit(model As PumpTemplateViewModel) As ActionResult
            If Not ModelState.IsValid Then
                Return View(model)
            End If

            If DataSource.PumpTemplates.LoadChanges(model) Then
                Dim toEdit As PumpTemplate = DataSource.PumpTemplates _
                       .SingleOrDefault(Function(m) m.PumpTemplateID = model.PumpTemplateID)

                toEdit.Barrel = model.Barrel
                toEdit.Seating = model.Seating
                toEdit.Plunger = model.Plunger
                toEdit.Discount = model.DiscountRate
                toEdit.Markup = model.MarkupRate
                toEdit.ConciseSpecificationSummary = GenerateName(toEdit)
                DataSource.SaveChanges()

                Return RedirectToAction("Details", New With {.id = model.PumpTemplateID})

            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' POST: /Pump/Template/Delete

        <HttpPost()>
        Public Function Delete(PumpTemplateID As Integer) As ActionResult
            If DataSource.Pumps.Any(Function(x) x.PumpTemplateID = PumpTemplateID) Then
                ModelState.AddModelError("PumpTemplateID", "Could Not delete this template because there Is at least one pump assigned To it.")
                Return Json(ModelState.ToDataSourceResult())
            End If
            Dim toDelete As PumpTemplate = DataSource.PumpTemplates.Find(PumpTemplateID)
            If toDelete IsNot Nothing Then
                Dim partDefsToRemove As List(Of TemplatePartDef) = toDelete.Parts.ToList()
                For Each part As TemplatePartDef In partDefsToRemove
                    DataSource.TemplatePartDefs.Remove(part)
                Next

                DataSource.PumpTemplates.Remove(toDelete)
                DataSource.SaveChanges()
            End If

            Return Json(Nothing)
        End Function

        '
        ' POST: /Pump/Template/List

        <HttpPost()> _
        Public Function List(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            req.SetDefaultSort(Function(x As PumpTemplateGridRowViewModel) x.VerboseSpec, ComponentModel.ListSortDirection.Descending)

            Return Json(DataSource.PumpTemplates.Select(Function(x) New PumpTemplateGridRowViewModel() With {
                                                            .PumpTemplateID = x.PumpTemplateID,
                                                            .ConciseSpecificationSummary = x.ConciseSpecificationSummary,
                                                            .VerboseSpec = x.VerboseSpecificationSummary
                                                        }) _
                                                .ToDataSourceResult(req)
                        )
        End Function

        '
        ' POST: /Pump/Template/IDList

        <HttpPost()> _
        Public Function IDList() As ActionResult
            Return Json(DataSource.PumpTemplates _
                                .Select(Function(p) New With {
                                            .PumpTemplateId = p.PumpTemplateID})
                                             )

        End Function

        '
        ' POST: /Pump/Template/GenerateName/id

        <HttpPost()> _
        Public Function GenerateName(id As Integer) As ActionResult
            Dim pumpTemplate As PumpTemplate = DataSource.PumpTemplates.FirstOrDefault(Function(x) x.PumpTemplateID = id)

            Dim name As String = GenerateName(pumpTemplate)
            Return Json(New With {.Name = name,
                                  .PumpTemplateID = id})
        End Function

        Public Function GenerateName(templateToEdit As PumpTemplate) As String
            Dim name As String
            If templateToEdit IsNot Nothing Then
                Dim generator As New PumpTemplateNameBuilder(templateToEdit)
                name = generator.GenerateName()
            Else
                name = "Could not find that template"
            End If
            Return name
        End Function

        '
        ' POST: /Pump/Template/PartList/id

        <HttpPost()>
        Public Function PartList(id As Integer, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return TemplatePartDefManager.List(id, req)
        End Function

        '
        ' POST: /Pump/Template/Duplicate/id

        <HttpPost()> _
        Public Function Duplicate(id As Integer) As ActionResult
            Dim template As PumpTemplate = DataSource _
                                            .PumpTemplates _
                                            .Include(Function(x) x.Parts) _
                                            .SingleOrDefault(Function(x) x.PumpTemplateID = id)

            If template Is Nothing Then
                TempData("error") = "Could not find any template to duplicate"

                Return RedirectToAction("Create")

            Else
                Dim duplicated As New PumpTemplate With {
                    .ConciseSpecificationSummary = template.ConciseSpecificationSummary,
                    .VerboseSpecificationSummary = If(template.VerboseSpecificationSummary IsNot Nothing, template.VerboseSpecificationSummary.Trim() & " Copy", Nothing),
                    .Markup = template.Markup,
                    .Discount = template.Discount,
                    .BallsAndSeats = template.BallsAndSeats,
                    .Barrel = template.Barrel,
                    .Collet = template.Collet,
                    .HoldDownType = template.HoldDownType,
                    .KnockOut = template.KnockOut,
                    .LowerExtension = template.LowerExtension,
                    .OnOffTool = template.OnOffTool,
                    .Plunger = template.Plunger,
                    .PonyRods = template.PonyRods,
                    .PumpBoreBasic = template.PumpBoreBasic,
                    .PumpType = template.PumpType,
                    .Seating = template.Seating,
                    .SpecialtyItems = template.SpecialtyItems,
                    .StandingValve = template.StandingValve,
                    .StandingValveCages = template.StandingValveCages,
                    .Strainers = template.Strainers,
                    .TopSeals = template.TopSeals,
                    .TravellingCages = template.TravellingCages,
                    .TubingSize = template.TubingSize,
                    .UpperExtension = template.UpperExtension,
                    .Parts = New List(Of TemplatePartDef)
                }

                For Each partDef As TemplatePartDef In template.Parts
                    duplicated.Parts.Add(New TemplatePartDef With {
                                            .PartTemplateID = partDef.PartTemplateID,
                                            .Quantity = partDef.Quantity,
                                            .SortOrder = partDef.SortOrder
                                        })
                Next

                DataSource.PumpTemplates.Add(duplicated)
                DataSource.SaveChanges()

                Return RedirectToAction("Edit", New With {.id = duplicated.PumpTemplateID})
            End If
        End Function

        '
        ' GET: /Pump/Template/Create

        <HttpGet()> _
        Public Function Create() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Pump/Template/Create

        <HttpPost()> _
        Public Function Create(model As PumpTemplateViewModel) As ActionResult
            Dim template As PumpTemplate = DataSource.PumpTemplates.LoadNew(model)
            template.Barrel = model.Barrel
            template.Seating = model.Seating
            template.Plunger = model.Plunger
            template.ConciseSpecificationSummary = GenerateName(template)

            DataSource.SaveChanges()

            Return RedirectToAction("Edit", New With {.id = template.PumpTemplateID})
        End Function

        '
        ' POST: /Pump/Template/AddPart

        <HttpPost()>
        Public Function AddPart(model As TemplatePartListRowViewModel, Optional partDefIDTarget As Integer = 0) As ActionResult
            Dim insertPosition As Integer

            If partDefIDTarget > 0 Then
                insertPosition = DataSource.TemplatePartDefs _
                                           .Where(Function(x) x.TemplatePartDefID = partDefIDTarget) _
                                           .Select(Function(x) x.SortOrder) _
                                           .DefaultIfEmpty(0) _
                                           .FirstOrDefault()
                insertPosition = insertPosition + 1
            Else
                insertPosition = DataSource.TemplatePartDefs _
                                           .Where(Function(x) x.PumpTemplateID = model.PumpTemplateID) _
                                           .Select(Function(x) x.SortOrder) _
                                           .DefaultIfEmpty(0) _
                                           .Min()
            End If

            MoveUpTemplatePartSort(model.PumpTemplateID, insertPosition)

            Dim partDef As TemplatePartDef = DataSource.TemplatePartDefs.LoadNew(model)
            partDef.SortOrder = insertPosition

            DataSource.SaveChanges()

            Return Json({TemplatePartDefManager.ModelMapper.Convert(partDef)}.ToDataSourceResult(New DataSourceRequest))
        End Function

        Private Sub MoveUpTemplatePartSort(pumpTemplateID As Integer, startFrom As Integer)
            For Each templatePartDef As TemplatePartDef In DataSource.TemplatePartDefs _
                                                .Where(Function(x) x.PumpTemplateID = pumpTemplateID) _
                                                .OrderBy(Function(x) x.SortOrder)
                If templatePartDef.SortOrder >= startFrom Then
                    templatePartDef.SortOrder = templatePartDef.SortOrder + 1
                End If
            Next
        End Sub

        '
        ' POST: /Pump/Template/UpdatePart

        <HttpPost()> _
        Public Function UpdatePart(model As TemplatePartListRowViewModel) As ActionResult
            Dim viewModel As TemplatePartListRowViewModel = Nothing
            If DataSource.TemplatePartDefs.LoadChanges(model) Then
                DataSource.SaveChanges()

                Dim partDef As TemplatePartDef = DataSource.TemplatePartDefs.Single(Function(x) x.TemplatePartDefID = model.TemplatePartDefID)
                viewModel = TemplatePartDefManager.ModelMapper.Convert(partDef)

            Else
                ViewData.ModelState.AddModelError("TemplatePartDefID", "Could not find that part on the pump template to update.")
            End If

            Return Json({viewModel}.ToDataSourceResult(New DataSourceRequest(), ModelState))
        End Function

        '
        ' POST: /Pump/Template/UpdatePartPosition

        <HttpPost()> _
        Public Function UpdatePartPosition(partDefIDToChange As Integer, partDefIDTarget As Integer) As ActionResult
            Dim toChange As TemplatePartDef = DataSource.TemplatePartDefs.Find(partDefIDToChange)
            Dim targetPosition As Integer

            If toChange Is Nothing Then
                Return Json(New With {.Success = False})
            End If

            If partDefIDTarget = 0 Then
                targetPosition = DataSource.TemplatePartDefs _
                                            .Where(Function(x) x.PumpTemplateID = toChange.PumpTemplateID) _
                                            .Select(Function(x) x.SortOrder) _
                                            .DefaultIfEmpty(0) _
                                            .Min()
            Else
                Dim target As TemplatePartDef = DataSource.TemplatePartDefs.Find(partDefIDTarget)

                If target Is Nothing Then
                    Return Json(New With {.Success = False})
                Else
                    targetPosition = target.SortOrder + 1
                End If
            End If

            MoveUpTemplatePartSort(toChange.PumpTemplateID, targetPosition)

            toChange.SortOrder = targetPosition

            DataSource.SaveChanges()

            Return Json(New With {.Success = True})
        End Function

        '
        ' POST: /Pump/Template/RemovePart

        <HttpPost()> _
        Public Function RemovePart(TemplatePartDefID As Integer) As ActionResult
            Dim def As TemplatePartDef = DataSource.TemplatePartDefs.Find(TemplatePartDefID)

            If def IsNot Nothing Then
                DataSource.TemplatePartDefs.Remove(def)
                DataSource.SaveChanges()
            End If

            Return Json(New With {.Success = (def IsNot Nothing)})
        End Function

        '
        ' POST: /Pump/Template/SwapParts

        <HttpPost()> _
        Public Function SwapParts(firstPartDefId As Integer, secondPartDefId As Integer) As ActionResult
            Dim firstPartDef As TemplatePartDef = DataSource.TemplatePartDefs.SingleOrDefault(Function(x) x.TemplatePartDefID = firstPartDefId)
            Dim secondPartDef As TemplatePartDef = DataSource.TemplatePartDefs.SingleOrDefault(Function(x) x.TemplatePartDefID = secondPartDefId)

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
        ' GET: /DeliveryTicket/Pdf/id

        Public Function Pdf(id As Integer) As ActionResult
            Return GetPdfReportAsActionResult(id, GetType(PumpTemplateReportDefinition), "Index")
        End Function

        '
        ' POST: /Pump/Template/Excel

        <HttpPost()> _
        Public Function ExcelExport(contentType As String, base64 As String, fileName As String) As ActionResult
            Dim fileContents = Convert.FromBase64String(base64)

            Return File(fileContents, contentType, fileName)
        End Function
    End Class
End Namespace