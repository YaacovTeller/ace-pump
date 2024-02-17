Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports System.Data.Entity
Imports AcePump.Domain.BL
Imports System.IO
Imports Yesod.Mvc
Imports Yesod.Ef
Imports Kendo.Mvc.Extensions
Imports Kendo.Mvc.UI
Imports AcePump.Domain.Models
Imports AcePump.Domain.ReportDefinitions
Imports AcePump.Common.ImageProcessing
Imports AcePump.Common.Tools.Files
Imports AcePump.Common.Storage
Imports System.Net

Namespace Areas.Employees.Controllers
    <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)>
    Public Class DeliveryTicketController
        Inherits AcePumpControllerBase

        Private Const SignatureDataFormat = "data:image/png;base64,"


        Private Property RepairTicketMapper As New ModelMapper(Of DeliveryTicket, RepairTicketModel)(
            Function(x As DeliveryTicket) New RepairTicketModel With {
                .DeliveryTicketID = x.DeliveryTicketID,
                .CustomerID = x.CustomerID,
                .IsRepairComplete = x.RepairComplete,
                .CurrentTicketDate = x.TicketDate,
                .PumpDispatchedNumber = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.PumpNumber, ""),
                .PumpDispatchedPrefix = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.ShopLocation.Prefix, ""),
                .PumpFailedID = x.PumpFailedID,
                .PumpFailedNumber = If(x.PumpFailed IsNot Nothing, x.PumpFailed.PumpNumber, ""),
                .PumpFailedPrefix = If(x.PumpFailed IsNot Nothing, x.PumpFailed.ShopLocation.Prefix, ""),
                .PumpFailedTemplateID = If(x.PumpFailed IsNot Nothing, x.PumpFailed.PumpTemplateID, 0),
                .PumpFailedTemplatSpecSummary = If(x.PumpFailed IsNot Nothing, x.PumpFailed.PumpTemplate.ConciseSpecificationSummary, ""),
                .PlungerBarrelWear = x.PlungerBarrelWear,
                .CustomerUsesInventory = x.Customer.UsesInventory
            }
        )

        Private Property TearDownMapper As New ModelMapper(Of DeliveryTicket, TearDownViewModel)(
            Function(x As DeliveryTicket) New TearDownViewModel With {
                .DeliveryTicketID = x.DeliveryTicketID,
                .PumpDispatchedNumber = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.PumpNumber, ""),
                .PumpDispatchedPrefix = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.ShopLocation.Prefix, ""),
                .PumpFailedID = x.PumpFailedID,
                .PumpFailedNumber = If(x.PumpFailed IsNot Nothing, x.PumpFailed.PumpNumber, ""),
                .PumpFailedPrefix = If(x.PumpFailed IsNot Nothing, x.PumpFailed.ShopLocation.Prefix, ""),
                .PumpFailedTemplateID = If(x.PumpFailed IsNot Nothing, x.PumpFailed.PumpTemplateID, 0),
                .PumpFailedTemplatSpecSummary = If(x.PumpFailed IsNot Nothing, x.PumpFailed.PumpTemplate.ConciseSpecificationSummary, ""),
                .RepairComplete = x.RepairComplete
            }
        )

        Private _LineItemMapper As ModelMapper(Of LineItem, LineItemsGridRowViewModel)
        Private ReadOnly Property LineItemMapper As ModelMapper(Of LineItem, LineItemsGridRowViewModel)
            Get
                If _LineItemMapper Is Nothing Then
                    Using ctrl As New LineItemController()
                        _LineItemMapper = ctrl.LineItemManager.ModelMapper
                    End Using
                End If

                Return _LineItemMapper
            End Get
        End Property


        Private _PartInspectionManager As KendoGridRequestManager(Of PartInspection, PartInspectionGridRowModel)
        Private ReadOnly Property PartInspectionManager As KendoGridRequestManager(Of PartInspection, PartInspectionGridRowModel)
            Get
                If _PartInspectionManager Is Nothing Then
                    _PartInspectionManager = New KendoGridRequestManager(Of PartInspection, PartInspectionGridRowModel)(
                        DataSource,
                        Function(x As PartInspection) New PartInspectionGridRowModel With {
                            .PartInspectionID = x.PartInspectionID,
                            .OriginalPartTemplateNumber = If(x.PartFailed IsNot Nothing, x.PartFailed.Number, ""),
                            .OriginalPartTemplateID = If(x.PartFailedID.HasValue, x.PartFailedID.Value, 0),
                            .TemplatePartDefID = x.TemplatePartDefID,
                            .Quantity = If(x.Quantity.HasValue, x.Quantity.Value, 0),
                            .CanBeRepresentedAsAssembly = If(x.PartFailed IsNot Nothing, (x.PartFailed.RelatedAssemblyID.HasValue AndAlso x.PartFailed.RelatedAssemblyID.Value > 0), False),
                            .PartDescription = If(x.PartFailed IsNot Nothing, x.PartFailed.Description, ""),
                            .Result = x.Result,
                            .ReasonRepaired = x.ReasonRepaired,
                            .PartReplacedID = If(x.PartReplacedID.HasValue, x.PartReplacedID.Value, 0),
                            .ReplacementPartTemplateNumber = If(x.PartReplaced IsNot Nothing, x.PartReplaced.Number, ""),
                            .ReplacementQuantity = If(x.ReplacementQuantity.HasValue, x.ReplacementQuantity.Value, 0),
                            .SortOrder = If(x.Sort.HasValue, x.Sort.Value, 0),
                            .HasParentAssembly = x.ParentAssemblyID.HasValue,
                            .ParentAssemblyID = x.ParentAssemblyID,
                            .IsSplitAssembly = x.IsSplitAssembly,
                            .IsConvertible = Not x.IsConvertible.HasValue OrElse x.IsConvertible.Value,
                            .DeliveryTicketID = x.DeliveryTicketID,
                            .ReplacedWithInventoryPartID = x.ReplacedWithInventoryPartID
                        },
                        Function(x) x.DeliveryTicketID,
                        Me
                    )
                End If

                Return _PartInspectionManager
            End Get
        End Property

        Private _TearDownItemManager As KendoGridRequestManager(Of PartInspection, TearDownItemGridRowModel)
        Private ReadOnly Property TearDownItemManager As KendoGridRequestManager(Of PartInspection, TearDownItemGridRowModel)
            Get
                If _TearDownItemManager Is Nothing Then
                    _TearDownItemManager = New KendoGridRequestManager(Of PartInspection, TearDownItemGridRowModel)(
                        DataSource,
                        Function(x As PartInspection) New TearDownItemGridRowModel With {
                            .PartInspectionID = x.PartInspectionID,
                            .DeliveryTicketID = x.DeliveryTicketID,
                            .SortOrder = If(x.Sort.HasValue, x.Sort.Value, 0),
                            .Result = x.Result,
                            .ReasonRepaired = x.ReasonRepaired,
                            .OriginalPartTemplateNumber = If(x.PartFailed IsNot Nothing, x.PartFailed.Number, ""),
                            .OriginalPartTemplateID = If(x.PartFailedID.HasValue, x.PartFailedID.Value, 0),
                            .PartDescription = If(x.PartFailed IsNot Nothing, x.PartFailed.Description, ""),
                            .Quantity = If(x.Quantity.HasValue, x.Quantity.Value, 0),
                            .CanBeRepresentedAsAssembly = If(x.PartFailed IsNot Nothing, (x.PartFailed.RelatedAssemblyID.HasValue AndAlso x.PartFailed.RelatedAssemblyID.Value > 0), False),
                            .HasParentAssembly = x.ParentAssemblyID.HasValue
                        },
                        Function(x) x.DeliveryTicketID,
                        Me
                    )
                End If

                Return _TearDownItemManager
            End Get
        End Property

        Private _DeliveryTicketSignatureViewModelMapper As ModelMapper(Of DeliveryTicket, DeliveryTicketSignatureViewModel)
        Private ReadOnly Property DeliveryTicketSignatureViewModelMapper As ModelMapper(Of DeliveryTicket, DeliveryTicketSignatureViewModel)
            Get
                If _DeliveryTicketSignatureViewModelMapper Is Nothing Then
                    _DeliveryTicketSignatureViewModelMapper = New ModelMapper(Of DeliveryTicket, DeliveryTicketSignatureViewModel)(
                        Function(x) New DeliveryTicketSignatureViewModel With {
                            .DeliveryTicketID = x.DeliveryTicketID,
                            .SignatureDate = x.SignatureDate,
                            .SignatureName = x.SignatureName,
                            .Signature = x.Signature,
                            .SignatureCompanyName = x.SignatureCompanyName,
                            .CustomerName = x.Customer.CustomerName,
                            .Lease = x.Well.Lease.LocationName,
                            .Well = x.Well.WellNumber,
                            .TicketDate = x.TicketDate,
                            .CloseTicket = x.CloseTicket.HasValue AndAlso x.CloseTicket.Value,
                            .SalesTaxRate = x.SalesTaxRate,
                            .LineItems = x.LineItems.OrderBy(Function(y) y.SortOrder) _
                                                    .Select(Function(y) New DeliveryTicketSignatureLineItemGridRowModel With {
                                                                .Item = y.Description,
                                                                .Quantity = y.Quantity,
                                                                .UnitPrice = (y.UnitPrice * (1 - y.UnitDiscount)),
                                                                .LineIsTaxable = y.CollectSalesTax.HasValue AndAlso y.CollectSalesTax.Value
                                                            })
                        })
                End If

                Return _DeliveryTicketSignatureViewModelMapper
            End Get
        End Property

        Private Property FileManager As New KendoGridRequestManager(Of DeliveryTicketImageUpload, DeliveryTicketImageUploadGridRow)(
            DataSource,
            Function(x) New DeliveryTicketImageUploadGridRow() With {
                .DeliveryTicketImageUploadID = x.DeliveryTicketImageUploadID,
                .LargeImagePath = "/DeliveryTicket/GetLargeImage/" & x.DeliveryTicketImageUploadID,
                .SmallImagePath = "/DeliveryTicket/GetSmallImage/" & x.DeliveryTicketImageUploadID,
                .UploadedBy = x.UploadedBy,
                .UploadedOn = x.UploadedOn,
                .Note = x.Note
            },
            Function(x) x.DeliveryTicketID,
            Me
        )

        '
        ' POST: /DeliveryTicket/UpdateTemplate

        <HttpPost()>
        Public Function UpdateTemplate(deliveryTicketId As Integer, newTemplateId As Integer) As ActionResult
            Dim newTemplate As PumpTemplate = DataSource.PumpTemplates.Find(newTemplateId)

            If newTemplate Is Nothing Then
                ModelState.AddModelError("newTemplateId", "Could not find that template.")
            Else

                Dim ticket As DeliveryTicket = DataSource.DeliveryTickets _
                                                    .Include(Function(x) x.PumpFailed) _
                                                    .SingleOrDefault(Function(x) x.DeliveryTicketID = deliveryTicketId)

                If ticket IsNot Nothing Then
                    If ticket.PumpFailed IsNot Nothing Then
                        If ticket.LineItems.Any(Function(x) x.PartInspectionID IsNot Nothing) Then
                            ModelState.AddModelError("deliverytTicketId", "The template cannot be changed because there are line items that were created from part inspections.")
                        Else
                            ticket.PumpFailed.PumpTemplateID = newTemplateId

                            UpdatePartInspections(ticket, newTemplate)
                            DataSource.SaveChanges()
                        End If
                    Else
                        ModelState.AddModelError("deliveryTicketId", "The delivery ticket does not have a failed pump selected.  Could not update the template.")
                    End If

                Else
                    ModelState.AddModelError("deliveryTicketId", "Could not find that delivery ticket")
                End If
            End If

            If ModelState.IsValid() Then
                Return Json(New With {
                                .PumpFailedConciseSpecSummary = newTemplate.ConciseSpecificationSummary,
                                .Success = ModelState.IsValid
                            })
            Else
                Return Json(New With {.Success = ModelState.IsValid,
                                      .Errors = ModelState.SelectMany(Function(x) x.Value.Errors)})
            End If
        End Function

        '
        ' POST: /DeliveryTicket/SynchronizeRepairOrder

        <HttpPost()>
        Public Function SynchronizeRepairOrder(id As Integer) As ActionResult
            Dim orderer As New PartInspectionOrderer(DataSource, id)
            orderer.Order()

            DataSource.SaveChanges()

            Return Json(DataSource.PartInspections _
                            .Where(Function(x) x.DeliveryTicketID = id) _
                            .Select(PartInspectionManager.ModelMapper.Selector)
                        )
        End Function

        '
        ' GET: /DeliveryTicket/TearDown

        <HttpGet()>
        Public Function TearDown(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets _
                                            .Include(Function(x) x.Inspections) _
                                            .Include(Function(x) x.PumpFailed.PumpTemplate.Parts) _
                                            .SingleOrDefault(Function(x) x.DeliveryTicketID = id)
            If ticket IsNot Nothing Then
                If ticket.RepairMode = AcePumpRepairModes.Repair Then
                    Return RedirectToAction("Repair", New With {.id = id})
                Else
                    If ticket.RepairComplete Or (ticket.CloseTicket.HasValue AndAlso ticket.CloseTicket = True) Then
                        Return RedirectToAction("TearDownDetails", New With {.id = id})
                    Else
                        EnsureInspectionRecordsExist(ticket)
                        DataSource.SaveChanges()

                        Return View(TearDownMapper.Convert(ticket))
                    End If
                End If
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' GET: /DeliveryTicket/TearDownDetails

        <HttpGet()>
        Public Function TearDownDetails(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets _
                                .Include(Function(x) x.Inspections) _
                                .Include(Function(x) x.PumpFailed.PumpTemplate.Parts) _
                                .SingleOrDefault(Function(x) x.DeliveryTicketID = id)

            If ticket IsNot Nothing Then
                If ticket.RepairMode = AcePumpRepairModes.Repair Then
                    Return RedirectToAction("RepairDetails", New With {.id = id})
                Else
                    Return View(TearDownMapper.Convert(ticket))
                End If
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' POST: /DeliveryTicket/CompleteTearDown

        <HttpPost()>
        Public Function CompleteTearDown(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets _
                                            .Include(Function(x) x.Inspections) _
                                            .SingleOrDefault(Function(x) x.DeliveryTicketID = id)

            If ticket IsNot Nothing AndAlso (Not ticket.RepairComplete) Then
                If ticket.Inspections.Any(Function(x) x.Result Is Nothing OrElse x.Result = "") Then
                    ModelState.AddModelError("Inspections", "You must mark all parts on the tear down before you can complete it.")
                Else
                    Dim intoInventory As List(Of PartInspection) = ticket.Inspections.Where(Function(x) x.Result = "Inventory").ToList()
                    If intoInventory.Count > 0 Then
                        For Each inspection In intoInventory
                            Dim newInventoryPart As New Part With {.PartTemplateID = inspection.PartFailedID,
                                                                   .CustomerID = ticket.CustomerID}

                            DataSource.Parts.Add(newInventoryPart)
                        Next
                    End If

                    MarkRepairCompleted(ticket)
                    DataSource.SaveChanges()
                    Return Json(New With {.Success = ModelState.IsValid,
                                          .RedirectUrl = Url.Action("TearDownDetails", "DeliveryTicket", New With {.id = id})})
                End If
            End If

            Return Json(New With {.Success = ModelState.IsValid,
                                  .Errors = ModelState.SelectMany(Function(x) x.Value.Errors)})
        End Function


        '
        ' POST: /DeliveryTicket/UpdateTearDownItem

        <HttpPost()>
        Public Function UpdateTearDownItem(model As TearDownItemGridRowModel) As ActionResult
            Return TearDownItemManager.Update(model, Function(tearDown)
                                                         If ModelState.IsValid Then
                                                             tearDown.Result = model.Result
                                                             tearDown.Sort = model.SortOrder
                                                             tearDown.ReasonRepaired = model.ReasonRepaired
                                                         End If

                                                         Return Nothing
                                                     End Function)
        End Function

        '
        ' GET: /DeliveryTicket/Repair

        <HttpGet()>
        Public Function Repair(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets _
                                            .Include(Function(x) x.Inspections) _
                                            .Include(Function(x) x.PumpFailed.PumpTemplate.Parts) _
                                            .SingleOrDefault(Function(x) x.DeliveryTicketID = id)

            If ticket IsNot Nothing Then
                If ticket.RepairMode = AcePumpRepairModes.TearDown Then
                    Return RedirectToAction("TearDown", New With {.id = id})
                Else
                    If (ticket.CloseTicket.HasValue AndAlso ticket.CloseTicket = True) Then
                        Return RedirectToAction("RepairDetails", New With {.id = id})
                    Else
                        EnsureInspectionRecordsExist(ticket)
                        Dim repairModel As RepairTicketModel = RepairTicketMapper.Convert(ticket)
                        SetPlungerBarrelWearFromPreviousOut(repairModel)
                        DataSource.SaveChanges()

                        Return View(repairModel)
                    End If
                End If
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        Sub SetPlungerBarrelWearFromPreviousOut(repairModel As RepairTicketModel)
            If repairModel.PlungerBarrelWear = "" Then
                Dim previousOut = DataSource.DeliveryTickets _
                                        .Where(Function(x) x.DeliveryTicketID <> repairModel.DeliveryTicketID) _
                                        .Where(Function(x) x.PumpFailedID = repairModel.PumpFailedID) _
                                        .OrderByDescending(Function(x) x.TicketDate) _
                                        .FirstOrDefault()

                If previousOut IsNot Nothing AndAlso Not String.IsNullOrEmpty(previousOut.PlungerBarrelWear) Then
                    Dim plungerOutPosition As Integer = 15 * 2 * 2
                    Dim barrelOutPosition As Integer = (15 * 2 * 3) + (36 * 2 * 2)

                    repairModel.PlungerOrig = previousOut.PlungerBarrelWear.Substring(plungerOutPosition, 2 * 15)
                    repairModel.BarrelOrig = previousOut.PlungerBarrelWear.Substring(barrelOutPosition, 2 * 36)
                End If
            End If
        End Sub

        '
        ' GET: /DeliveryTicket/RepairDetails

        <HttpGet()>
        Public Function RepairDetails(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets _
                                .Include(Function(x) x.Inspections) _
                                .Include(Function(x) x.PumpFailed.PumpTemplate.Parts) _
                                .SingleOrDefault(Function(x) x.DeliveryTicketID = id)

            If ticket IsNot Nothing Then
                If ticket.RepairMode = AcePumpRepairModes.TearDown Then
                    Return RedirectToAction("TearDownDetails", New With {.id = id})
                Else
                    Return View(RepairTicketMapper.Convert(ticket))
                End If
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        Private Sub EnsureInspectionRecordsExist(ticket As DeliveryTicket)
            If Not ticket.Inspections.Any() Then
                If ticket.PumpFailed IsNot Nothing Then

                    CopyPartInspectionsFromPumpTemplate(ticket, ticket.PumpFailed.PumpTemplate)
                    ModifyNewInspectionsForRepairMode(ticket)
                Else
                    ModelState.AddModelError("PumpFailedID", "There was no failed pump associated with this delivery ticket.  Unable to create the repair ticket!")
                End If
            End If
        End Sub

        Private Sub UpdatePartInspections(ticket As DeliveryTicket, createInspectionsFrom As PumpTemplate)
            ClearPartInspections(ticket)

            CopyPartInspectionsFromPumpTemplate(ticket, createInspectionsFrom)
            ModifyNewInspectionsForRepairMode(ticket)
        End Sub

        Private Sub CopyPartInspectionsFromPumpTemplate(ticket As DeliveryTicket, createInspectionsFrom As PumpTemplate)
            For Each partDef As TemplatePartDef In createInspectionsFrom.Parts.OrderBy(Function(x) x.SortOrder)
                ticket.Inspections.Add(New PartInspection() With {
                                          .PartFailedID = partDef.PartTemplateID,
                                          .Quantity = partDef.Quantity,
                                          .Sort = partDef.SortOrder,
                                          .TemplatePartDefID = partDef.TemplatePartDefID
                                       })
            Next
        End Sub

        Private Sub ClearPartInspections(ticket As DeliveryTicket)
            For Each inspection In ticket.Inspections.ToList()
                DataSource.PartInspections.Remove(inspection)
            Next
        End Sub

        '
        ' POST: /DeliveryTicket/SwitchRepairMode

        <HttpPost()>
        Public Function SwitchRepairMode(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets _
                                            .Include(Function(x) x.Inspections) _
                                            .Include(Function(x) x.Inspections.Select(Function(p) p.PartReplaced.CustomersWithSpecials)) _
                                            .SingleOrDefault(Function(x) x.DeliveryTicketID = id)

            If ticket IsNot Nothing AndAlso (Not ticket.CloseTicket.HasValue OrElse Not ticket.CloseTicket.Value) AndAlso Not (ticket.RepairMode = AcePumpRepairModes.TearDown AndAlso ticket.RepairComplete) Then
                Dim redirect As String = ""
                If ticket.RepairMode = AcePumpRepairModes.Repair Then
                    ticket.RepairMode = AcePumpRepairModes.TearDown
                    UndoRepairCompleted(ticket)
                    redirect = "TearDown"
                ElseIf ticket.RepairMode = AcePumpRepairModes.TearDown Then
                    ticket.RepairMode = AcePumpRepairModes.Repair
                    redirect = "Repair"
                End If

                DeleteRepairTicketGeneratedLineItems(ticket)
                ClearPartInspections(ticket)
                EnsureInspectionRecordsExist(ticket)

                DataSource.SaveChanges()

                Return Json(New With {.Success = True,
                                      .RedirectUrl = Url.Action(redirect, "DeliveryTicket", New With {.id = id})
                            })
            Else
                Return Json(New With {.Success = False,
                                      .Errors = "Could not switch repair mode. Please make sure that the repair or tear down isn't already completed and that the ticket isn't closed."})
            End If
        End Function

        Private Sub UndoRepairCompleted(ticket As DeliveryTicket)
            ticket.RepairComplete = False
        End Sub

        Private Sub ModifyNewInspectionsForRepairMode(ticket As DeliveryTicket)
            For Each inspection As PartInspection In ticket.Inspections
                inspection.Result = ""
                If ticket.RepairMode = AcePumpRepairModes.TearDown Then
                    inspection.ReplacementQuantity = Nothing
                    If inspection.Quantity <> 1 Then
                        inspection.Result = "Trashed"
                    End If
                    If CanBeRepresentedAsAssembly(inspection) Then
                        inspection.Result = "Trashed"
                    End If
                ElseIf ticket.RepairMode = AcePumpRepairModes.Repair Then
                    inspection.ReplacementQuantity = inspection.Quantity
                End If
            Next
        End Sub

        Private Function CanBeRepresentedAsAssembly(inspection As PartInspection) As Boolean
            If inspection.PartFailedID IsNot Nothing Then
                Dim partFailed As PartTemplate = DataSource.PartTemplates.Find(inspection.PartFailedID.Value)
                If partFailed IsNot Nothing Then
                    If partFailed.RelatedAssemblyID.HasValue AndAlso partFailed.RelatedAssemblyID.Value > 0 Then
                        Return True
                    End If
                End If
            End If
            Return False
        End Function

        '
        ' POST: /DeliveryTicket/CompleteRepair

        <HttpPost()>
        Public Function CompleteRepair(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets _
                                            .Include(Function(x) x.Inspections) _
                                            .Include(Function(x) x.Inspections.Select(Function(p) p.PartReplaced.CustomersWithSpecials)) _
                                            .SingleOrDefault(Function(x) x.DeliveryTicketID = id)

            If ticket IsNot Nothing AndAlso (Not ticket.CloseTicket.HasValue OrElse Not ticket.CloseTicket.Value) Then
                If ticket.Inspections.Any(Function(x) x.Result Is Nothing OrElse x.Result = "") Then
                    ModelState.AddModelError("Inspections", "You must mark all parts on the repair ticket before you can complete it.")
                    Return View("Repair", RepairTicketMapper.Convert(ticket))
                ElseIf ticket.Inspections.Any(Function(x) (x.Result = "Convert" Or x.Result = "Replace") And Not x.PartReplacedID.HasValue) Then
                    ModelState.AddModelError("Inspections", "You must specify a replacement part for all parts that were converted or replaced.")
                    Return View("Repair", RepairTicketMapper.Convert(ticket))
                Else
                    DeleteRepairTicketGeneratedLineItems(ticket)
                    GenerateLineItemsFromRepair(ticket)
                    MarkRepairCompleted(ticket)

                    DataSource.SaveChanges()

                    Return RedirectToAction("Details", New With {.id = ticket.DeliveryTicketID})
                End If
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        ''' <summary>
        ''' Deletes all line items from the ticket which were originall created by
        ''' a repair ticket.
        ''' </summary>
        Private Sub DeleteRepairTicketGeneratedLineItems(ticket As DeliveryTicket)
            Dim lineItemsToRemove As List(Of LineItem) = ticket.LineItems.Where(Function(x) x.PartInspectionID.HasValue).ToList()
            For Each lineItem As LineItem In lineItemsToRemove
                DataSource.LineItems.Remove(lineItem)
            Next
        End Sub

        ''' <summary>
        ''' Creates line items based on failing repairs.  Moves any existing manually added line items
        ''' to the end of the list of line items.
        ''' </summary>
        Private Function GenerateLineItemsFromRepair(ticket As DeliveryTicket) As Integer
            Dim currentMaxSortOrder As Integer = 0
            For Each inspection As PartInspection In ticket.Inspections.OrderBy(Function(x) x.Sort)
                If (inspection.Result = "Convert" Or inspection.Result = "Replace") AndAlso inspection.ReplacedWithInventoryPartID Is Nothing Then
                    ticket.LineItems.Add(New LineItem With {
                                            .PartTemplateID = inspection.PartReplacedID,
                                            .Quantity = If(inspection.ReplacementQuantity.HasValue, inspection.ReplacementQuantity, 0D),
                                            .PartInspectionID = inspection.PartInspectionID,
                                            .SortOrder = currentMaxSortOrder,
                                            .Description = If(inspection.PartReplaced IsNot Nothing, inspection.PartReplaced.Description, Nothing),
                                            .UnitPrice = If(inspection.PartReplaced IsNot Nothing, inspection.PartReplaced.ListPrice, 0D),
                                            .UnitDiscount = If(inspection.PartReplaced IsNot Nothing, inspection.PartReplaced.Discount, 0D),
                                            .CustomerDiscount = If(inspection.PartReplaced IsNot Nothing _
                                                                   AndAlso inspection.PartReplaced.CustomersWithSpecials.Any(Function(c) c.CustomerID = ticket.CustomerID),
                                                                   inspection.PartReplaced.CustomersWithSpecials.FirstOrDefault(Function(c) c.CustomerID = ticket.CustomerID).Discount,
                                                                   New Nullable(Of Decimal)()),
                                            .CollectSalesTax = True
                                         })
                    currentMaxSortOrder += 1
                End If
            Next

            For Each manualLineItem As LineItem In ticket.LineItems.Where(Function(x) Not x.PartInspectionID.HasValue).OrderBy(Function(x) x.SortOrder)
                manualLineItem.SortOrder = currentMaxSortOrder

                currentMaxSortOrder += 1
            Next
        End Function

        Private Sub MarkRepairCompleted(ticket As DeliveryTicket)
            ticket.RepairComplete = True
        End Sub

        '
        ' POST: /DeliveryTicket/SplitAssemblyInspection

        <HttpPost()>
        Public Function SplitAssemblyInspection(partInspectionId As Integer) As ActionResult
            Dim inspectionToSplit As PartInspection = DataSource.PartInspections _
                                                    .Include(Function(x) x.PartFailed.RelatedAssembly.Parts) _
                                                    .Include(Function(x) x.DeliveryTicket.Inspections) _
                                                    .SingleOrDefault(Function(x) x.PartInspectionID = partInspectionId)

            If inspectionToSplit IsNot Nothing Then
                Dim assemblyDbInfo As AssemblyWithRelatedPart = DataSource.AssembliesWithRelatedParts.SingleOrDefault(Function(x) x.PartTemplate.PartTemplateID = inspectionToSplit.PartFailedID.Value)

                If assemblyDbInfo IsNot Nothing Then
                    Dim ticket As DeliveryTicket = inspectionToSplit.DeliveryTicket
                    Dim positionOfAssemblyToSplit As Integer = If(inspectionToSplit.Sort.HasValue, inspectionToSplit.Sort.Value, 0)
                    Dim positionToInsertNewInspection As Integer = positionOfAssemblyToSplit + 1

                    For Each partDef As AssemblyPartDef In assemblyDbInfo.Assembly.Parts
                        MoveUpInspectionSort(ticket, positionToInsertNewInspection)
                        ticket.Inspections.Add(New PartInspection With {
                                                   .PartFailedID = partDef.PartTemplateID,
                                                   .Quantity = partDef.PartsQuantity,
                                                   .ParentAssemblyID = inspectionToSplit.PartInspectionID,
                                                   .ReplacementQuantity = partDef.PartsQuantity,
                                                   .Sort = positionToInsertNewInspection,
                                                   .TemplatePartDefID = inspectionToSplit.TemplatePartDefID,
                                                   .IsConvertible = False
                                               })

                        positionToInsertNewInspection += 1
                    Next

                    SetInspectionValuesByMark(inspectionToSplit, "OK")
                    inspectionToSplit.IsSplitAssembly = True

                    DataSource.SaveChanges()

                Else
                    ModelState.AddModelError("partInspectionId", "That part is not an assembly, could not break it down into its component parts.")
                End If
            Else
                ModelState.AddModelError("partInspectionId", "Could not find that record!")
            End If

            Return Json(DataSource.PartInspections _
                            .Where(Function(x) x.DeliveryTicketID = inspectionToSplit.DeliveryTicketID) _
                            .Select(PartInspectionManager.ModelMapper.Selector) _
                            .ToDataSourceResult(New DataSourceRequest(), ModelState)
                    )
        End Function

        Private Sub SetInspectionValuesByMark(inpsection As PartInspection, markAs As String)
            Select Case markAs
                Case "OK"
                    inpsection.Result = "OK"
                    inpsection.ReasonRepaired = Nothing
                    inpsection.ReplacementQuantity = Nothing
                    inpsection.PartReplacedID = Nothing

                Case "NA"
                    inpsection.Result = "N/A"
                    inpsection.ReasonRepaired = "Did not inspect"
                    inpsection.ReplacementQuantity = Nothing
                    inpsection.PartReplacedID = Nothing

                Case "Maintenance"
                    inpsection.Result = "Replace"
                    inpsection.ReasonRepaired = "ROUT/MAINT"
                    inpsection.ReplacementQuantity = inpsection.Quantity
                    inpsection.PartReplacedID = inpsection.PartFailedID

                Case "Convert"
                    inpsection.Result = "Convert"
                    inpsection.ReplacementQuantity = inpsection.Quantity
                    inpsection.PartReplacedID = Nothing

                Case "Replace"
                    inpsection.Result = "Replace"
                    inpsection.ReasonRepaired = Nothing
                    inpsection.ReplacementQuantity = inpsection.Quantity
                    inpsection.PartReplacedID = inpsection.PartFailedID

                Case Else
                    Throw New ArgumentException("Unknown mark: " & markAs)
            End Select
        End Sub

        Private Sub MoveUpInspectionSort(ticket As DeliveryTicket, startFrom As Integer)
            For Each inspection As PartInspection In ticket.Inspections
                If inspection.Sort >= startFrom Then
                    inspection.Sort = inspection.Sort + 1
                End If
            Next
        End Sub

        '
        ' POST: /DeliveryTicket/UpdateInspection

        <HttpPost()>
        Public Function UpdateInspection(model As PartInspectionGridRowModel) As ActionResult
            Dim result = PartInspectionManager.Update(model, Function(inspection)
                                                                 ValidateInspectionResult(model, inspection)

                                                                 If ModelState.IsValid Then
                                                                     inspection.Result = model.Result
                                                                     inspection.Sort = model.SortOrder

                                                                     inspection.ReplacedWithInventoryPartID = Nothing

                                                                     If model.PartReplacedID = 0 Then
                                                                         model.PartReplacedID = Nothing
                                                                     End If

                                                                     Dim newDate As Date? = DataSource.DeliveryTickets _
                                                                                              .Where(Function(x) x.DeliveryTicketID = model.DeliveryTicketID) _
                                                                                              .Select(Function(x) If(x.PumpFailedDate.HasValue, x.PumpFailedDate.Value, x.TicketDate)) _
                                                                                              .FirstOrDefault()
                                                                 End If

                                                                 Return Nothing
                                                             End Function)
            Return result
        End Function

        '
        ' POST: /DeliveryTicket/UpdateInspectionUsingInventory

        <HttpPost()>
        Public Function UpdateInspectionUsingInventory(model As PartInspectionGridRowModel, useInventory As Boolean) As ActionResult
            ValidateUsingInventory(model)
            If ModelState.IsValid Then
                Dim inspection As PartInspection = DataSource.PartInspections.Include(Function(x) x.DeliveryTicket) _
                                                                             .Include(Function(x) x.ReplacedWithInventoryPart) _
                                                                             .FirstOrDefault(Function(x) x.PartInspectionID = model.PartInspectionID)
                If inspection IsNot Nothing Then

                    If useInventory Then
                        Dim usedParts = From usedInspection In DataSource.PartInspections
                                        Where usedInspection.ReplacedWithInventoryPartID.HasValue
                                        Select usedInspection.ReplacedWithInventoryPart

                        Dim availableInventoryPart As Part = DataSource.Parts.FirstOrDefault(Function(x) x.CustomerID = inspection.DeliveryTicket.CustomerID _
                                                                                                And x.PartTemplateID = inspection.PartReplacedID _
                                                                                                And Not usedParts.Contains(x))
                        If availableInventoryPart IsNot Nothing Then
                            inspection.ReplacedWithInventoryPart = availableInventoryPart

                            DataSource.SaveChanges()
                            Return Json(New With {.Success = True,
                                                  .ReplacedWithInventoryPartID = availableInventoryPart.PartID,
                                                  .AvailableInInventory = Nothing
                                        })
                        Else
                            ModelState.AddModelError("PartTemplateID", "This part is not available in inventory!")
                        End If

                    ElseIf inspection.ReplacedWithInventoryPartID.HasValue Then 'And Not useInventory

                        inspection.ReplacedWithInventoryPartID = Nothing

                        DataSource.SaveChanges()
                        Return Json(New With {.Success = True,
                                                  .ReplacedWithInventoryPartID = Nothing,
                                                  .AvailableInInventory = True})
                    End If
                Else
                    ModelState.AddModelError("InspectionID", "Could not find inspection to update.")
                End If
            End If

            Return Json(New With {.Success = False,
                                  .Errors = ModelState.SelectMany(Function(x) x.Value.Errors)})
        End Function

        Private Sub ValidateUsingInventory(model As PartInspectionGridRowModel)
            If model.ReplacementQuantity <> 1 And (model.Result = "Replace" Or model.Result = "Convert") Then
                ModelState.AddModelError("Use from Inventory", "You cannot use inventory if the replacement quantity is not 1.")
            End If
        End Sub

        ''' <summary>
        ''' Business rules for different result types.  For example, a replace or convert needs a replacement quantity.
        ''' </summary>
        Private Sub ValidateInspectionResult(model As PartInspectionGridRowModel, inspection As PartInspection)
            If inspection.IsConvertible.HasValue AndAlso Not inspection.IsConvertible.Value AndAlso model.Result = "Convert" Then
                ModelState.AddModelError("Cannot convert", "This part cannot be converted or removed because it is part of an assembly.")
            End If

            If model.Result = "Convert" Or model.Result = "Replace" Then
                If Not model.ReplacementQuantity.HasValue Then
                    ModelState.AddModelError("Replacement Quantity", "You must enter a replacement quantity.")
                End If
            End If
        End Sub

        '
        ' POST: /DeliveryTicket/AddInspection

        <HttpPost()>
        Public Function AddInspection(model As PartInspectionGridRowModel) As ActionResult
            Return PartInspectionManager.Add(model, Function(inspection)
                                                        Dim currentMaxSortOrder As Integer = DataSource.PartInspections _
                                                            .Where(Function(l) l.DeliveryTicketID = model.DeliveryTicketID) _
                                                            .Select(Function(l) l.Sort) _
                                                            .DefaultIfEmpty(-1) _
                                                            .Max()

                                                        inspection.Sort = currentMaxSortOrder + 1

                                                        Return Nothing
                                                    End Function)
        End Function

        '
        ' POST: /DeliveryTicket/RemoveInspection

        <HttpPost()>
        Public Function RemoveInspection(PartInspectionID As Integer) As ActionResult
            Return PartInspectionManager.Remove(PartInspectionID, Function(inspection)
                                                                      Dim inspectionsToRemove As List(Of PartInspection) = Nothing
                                                                      If inspection IsNot Nothing Then
                                                                          Dim childInspections As IQueryable(Of PartInspection) = DataSource.PartInspections.Where(Function(x) x.ParentAssemblyID.HasValue AndAlso x.ParentAssemblyID.Value = inspection.PartInspectionID)
                                                                          Dim childInspectionIds As IQueryable(Of Integer) = childInspections.Select(Function(x) x.PartInspectionID)
                                                                          Dim lineItemsToRemove As List(Of LineItem) = (From lineItem In DataSource.LineItems
                                                                                                                        Where lineItem.PartInspectionID.HasValue AndAlso lineItem.PartInspectionID.Value = inspection.PartInspectionID Or childInspectionIds.Contains(lineItem.PartInspectionID.Value)) _
                                                                                                                        .ToList()

                                                                          If lineItemsToRemove.Count > 0 Then
                                                                              For Each lineItemToRemove As LineItem In lineItemsToRemove
                                                                                  DataSource.LineItems.Remove(lineItemToRemove)
                                                                              Next
                                                                          End If

                                                                          inspectionsToRemove = childInspections.ToList()
                                                                          If inspectionsToRemove.Count > 0 Then
                                                                              For Each inspectionToRemove As PartInspection In inspectionsToRemove
                                                                                  DataSource.PartInspections.Remove(inspectionToRemove)
                                                                              Next
                                                                          End If
                                                                      End If

                                                                      Return New DataSourceChanges(Of PartInspection)() With {
                                                                          .Removed = inspectionsToRemove
                                                                      }
                                                                  End Function)
        End Function

        '
        ' POST: /DeliveryTicket/InspectionList

        <HttpPost()>
        Public Function InspectionList(id As Integer, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return PartInspectionManager.List(id, req)
        End Function

        '
        ' POST: /DeliveryTicket/TearDownItemList

        <HttpPost()>
        Public Function TearDownItemList(id As Integer, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return TearDownItemManager.List(id, req)
        End Function

        '
        ' POST: /DeliveryTicket/List

        <HttpPost()>
        Public Function List(customerId As Integer?, hideClosed As Boolean?, hideQuotes As Boolean?, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim tickets As IQueryable(Of DeliveryTicket) = DataSource.DeliveryTickets

            If customerId.HasValue Then
                tickets = tickets.Include("CustomerID").Where(Function(x) x.CustomerID = customerId.Value)
            End If

            If hideClosed.HasValue AndAlso hideClosed.Value Then
                tickets = tickets.Where(Function(x) Not x.CloseTicket.HasValue OrElse x.CloseTicket.Value = False)
            End If

            If hideQuotes.HasValue AndAlso hideQuotes.Value Then
                tickets = tickets.Where(Function(x) Not x.Quote.HasValue OrElse x.Quote.Value = False)
            End If

            Return Json(tickets.Select(Function(t) New DeliveryTicketGridRowViewModel With {
                                          .DeliveryTicketID = t.DeliveryTicketID,
                                          .CustomerID = t.CustomerID,
                                          .CustomerName = If(t.Customer IsNot Nothing, t.Customer.CustomerName, ""),
                                          .TicketDate = t.TicketDate,
                                          .WellNumber = If(t.Well IsNot Nothing, t.Well.WellNumber, ""),
                                          .LocationName = If(t.Well IsNot Nothing AndAlso t.Well.Lease IsNot Nothing, t.Well.Lease.LocationName, ""),
                                          .IsClosed = t.CloseTicket.HasValue AndAlso t.CloseTicket.Value,
                                          .IsSigned = t.Signature IsNot Nothing,
                                          .ReasonStillOpen = t.ReasonStillOpen,
                                          .Quote = t.Quote.HasValue AndAlso t.Quote.Value,
                                          .IsDownloadedToQb = t.InvoiceStatus = AcePumpInvoiceStatuses.InQuickbooks,
                                          .IsReadyForQb = t.InvoiceStatus = AcePumpInvoiceStatuses.ReadyForQuickbooks And t.CloseTicket = True,
                                          .InvoiceStatus = t.InvoiceStatus,
                                          .IsSignificantDesignChange = t.IsSignificantDesignChange.HasValue And t.IsSignificantDesignChange = True
                                }) _
                                .ToDataSourceResult(req)
                        )
        End Function

        '
        ' GET: /DeliveryTicket/[Index]

        Public Function Index() As ActionResult
            Return View()
        End Function

        '
        ' GET: /DeliveryTicket/Pdf/id

        Public Function Pdf(id As Integer) As ActionResult
            Return GetPdfReportAsActionResult(id, GetType(DtReportDefinition), "Index")
        End Function

        '
        ' GET: /DeliveryTicket/PdfUnpriced/id

        Public Function PdfUnpriced(id As Integer) As ActionResult
            Return GetPdfReportAsActionResult(id, GetType(DtUnpricedReportDefinition), "Index")
        End Function

        '
        ' GET: /DeliveryTicket/RepairPdf/id

        Public Function RepairPdf(id As Integer) As ActionResult
            Return GetPdfReportAsActionResult(id, GetType(RepairReportDefinition), "Index")
        End Function

        '
        ' GET: /DeliveryTicket/TearDownPdf/id

        Public Function TearDownPdf(id As Integer) As ActionResult
            Return GetPdfReportAsActionResult(id, GetType(TearDownReportDefinition), "Index")
        End Function

        '
        ' GET: /DeliveryTicket/Reopen/id

        <HttpGet()>
        Public Function Reopen(id As Integer) As ActionResult
            Dim deliveryTicket As DeliveryTicket = DataSource.DeliveryTickets.FirstOrDefault(Function(dt) dt.DeliveryTicketID = id)

            If deliveryTicket IsNot Nothing Then
                deliveryTicket.CloseTicket = False
                DataSource.SaveChanges()

                Return RedirectToAction("Edit", New With {.id = id})

            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' POST: /DeliveryTicket/Delete/deliveryTicketId

        <HttpPost()>
        Public Function Delete(deliveryTicketId As Integer) As ActionResult
            Dim deliveryTicket As DeliveryTicket = DataSource.DeliveryTickets.FirstOrDefault(Function(dt) dt.DeliveryTicketID = deliveryTicketId)

            If deliveryTicket IsNot Nothing AndAlso (Not deliveryTicket.CloseTicket.HasValue OrElse Not deliveryTicket.CloseTicket.Value) Then
                '-- TEMPORARY PART HISTORY/LIVE TRUNK PATCH --'
                Dim ctx As DbContext = DirectCast(DataSource, DbContext)
                ctx.Database.ExecuteSqlCommand("delete from PartRuntimeSegments where SegmentStartedByTicketID={0} or SegmentEndedByTicketID={0}", deliveryTicket.DeliveryTicketID)
                ctx.Database.ExecuteSqlCommand("delete from PartRuntimeSegments where RuntimeID in (select PartRuntimeID from PartRuntimes where RuntimeStartedByTicketID={0})", deliveryTicket.DeliveryTicketID)
                ctx.Database.ExecuteSqlCommand("delete from PartRuntimes where RuntimeStartedByTicketID={0}", deliveryTicket.DeliveryTicketID)
                ctx.Database.ExecuteSqlCommand("delete from PumpRuntimes where RuntimeStartedByTicketID={0} or RuntimeEndedByTicketID={0}", deliveryTicket.DeliveryTicketID)
                '-- END TEMPORARY PART HISTORY/LIVE TRUNK PATCH --'

                DataSource.DeliveryTickets.Remove(deliveryTicket)
                DataSource.SaveChanges()

            End If

            Return Json(Nothing)
        End Function

        '
        ' GET: /DeliveryTicket/Close/id

        <HttpGet()>
        Public Function Close(id As Integer) As ActionResult
            Dim deliveryTicket As DeliveryTicket = DataSource.DeliveryTickets.FirstOrDefault(Function(dt) dt.DeliveryTicketID = id)

            If deliveryTicket IsNot Nothing Then
                deliveryTicket.CloseTicket = True

                DataSource.SaveChanges()
            End If

            Return RedirectToAction("Index")
        End Function

        '
        ' POST: /DeliveryTicket/CloseAjax/id

        <HttpPost()>
        Public Function CloseAjax(id As Integer) As ActionResult
            Dim deliveryTicket As DeliveryTicket = DataSource.DeliveryTickets.FirstOrDefault(Function(dt) dt.DeliveryTicketID = id)

            If deliveryTicket IsNot Nothing Then
                deliveryTicket.CloseTicket = True

                DataSource.SaveChanges()

                Return Json(New With {.Success = True})
            End If

            Return Json(New With {.Success = False,
                        .Errors = ModelState.SelectMany(Function(x) x.Value.Errors)})
        End Function

        '
        ' GET: /DeliveryTicket/Create

        <HttpGet()>
        Public Function Create() As ActionResult
            Return View(New DeliveryTicketViewModel)
        End Function

        '
        ' POST: /DeliveryTicket/Create

        <HttpPost()>
        Public Function Create(model As DeliveryTicketViewModel) As ActionResult
            UpdateWellInstalledIn(model)
            If Not ModelState.IsValid Then
                Return View(model)
            End If

            Dim newDeliveryTicket As DeliveryTicket = DataSource.DeliveryTickets.LoadNew(model)

            newDeliveryTicket.LastPull = If(model.LastPull.HasValue, model.LastPull.Value.ToShortDateString, "")
            newDeliveryTicket.SalesTaxRate = model.SalesTaxRate

            DataSource.SaveChanges()

            Return RedirectToAction("Edit", New With {.id = newDeliveryTicket.DeliveryTicketID})
        End Function

        '
        ' GET: /DeliveryTicket/Details/id

        <HttpGet()>
        Public Function Details(id As Integer) As ActionResult
            Dim dt As DeliveryTicketViewModel = GenerateDeliveryTicketViewModelFromPersisted(id)

            If dt IsNot Nothing Then
                Return View(dt)

            Else
                Return RedirectToAction("Index")
            End If
        End Function

        Public Function GenerateDeliveryTicketViewModelFromPersisted(id As Integer) As DeliveryTicketViewModel
            Dim domainDeliveryTicket As DeliveryTicket = DataSource.DeliveryTickets _
                                                             .Include(Function(h) h.PumpFailed) _
                                                             .Include(Function(h) h.PumpDispatched) _
                                                             .Include(Function(h) h.Well) _
                                                             .SingleOrDefault(Function(x) x.DeliveryTicketID = id)
            If domainDeliveryTicket Is Nothing Then
                Return Nothing
            End If

            Return New DeliveryTicketViewModel With {
                        .DeliveryTicketID = domainDeliveryTicket.DeliveryTicketID,
                        .CustomerID = domainDeliveryTicket.CustomerID,
                        .LeaseID = If(domainDeliveryTicket.Well IsNot Nothing AndAlso domainDeliveryTicket.Well.Lease IsNot Nothing,
                                        domainDeliveryTicket.Well.Lease.LeaseID, New Integer?()),
                        .WellID = If(domainDeliveryTicket.Well IsNot Nothing, domainDeliveryTicket.WellID, Nothing),
                        .CustomerName = If(domainDeliveryTicket.Customer IsNot Nothing, domainDeliveryTicket.Customer.CustomerName, Nothing),
                        .PumpFailedID = domainDeliveryTicket.PumpFailedID,
                        .PumpFailedNumber = If(domainDeliveryTicket.PumpFailed IsNot Nothing, domainDeliveryTicket.PumpFailed.PumpNumber, Nothing),
                        .PumpFailedPrefix = If(domainDeliveryTicket.PumpFailed IsNot Nothing, domainDeliveryTicket.PumpFailed.ShopLocation.Prefix, Nothing),
                        .PumpFailedDate = domainDeliveryTicket.PumpFailedDate,
                        .PumpDispatchedID = domainDeliveryTicket.PumpDispatchedID,
                        .PumpDispatchedNumber = If(domainDeliveryTicket.PumpDispatched IsNot Nothing, domainDeliveryTicket.PumpDispatched.PumpNumber, Nothing),
                        .PumpDispatchedPrefix = If(domainDeliveryTicket.PumpDispatched IsNot Nothing, domainDeliveryTicket.PumpDispatched.ShopLocation.Prefix, Nothing),
                        .PumpDispatchedDate = domainDeliveryTicket.PumpDispatchedDate,
                        .PumpDispatchedTemplateID = If(domainDeliveryTicket.PumpDispatched IsNot Nothing, domainDeliveryTicket.PumpDispatched.PumpTemplateID, Nothing),
                        .PumpDispatchedConciseTemplate = If(domainDeliveryTicket.PumpDispatched IsNot Nothing, domainDeliveryTicket.PumpDispatched.PumpTemplate.ConciseSpecificationSummary, Nothing),
                        .LeaseLocation = If(domainDeliveryTicket.Well IsNot Nothing AndAlso domainDeliveryTicket.Well.Lease IsNot Nothing,
                                            domainDeliveryTicket.Well.Lease.LocationName, Nothing),
                        .WellNumber = If(domainDeliveryTicket.Well IsNot Nothing, domainDeliveryTicket.Well.WellNumber, Nothing),
                        .TicketDate = domainDeliveryTicket.TicketDate,
                        .ShipVia = domainDeliveryTicket.ShipVia,
                        .IsSignificantDesignChange = domainDeliveryTicket.IsSignificantDesignChange.HasValue AndAlso domainDeliveryTicket.IsSignificantDesignChange.Value,
                        .LastPull = TryParseDate(domainDeliveryTicket.LastPull),
                        .OrderDate = domainDeliveryTicket.OrderDate,
                        .OrderTime = domainDeliveryTicket.OrderTime,
                        .OrderedBy = domainDeliveryTicket.OrderedBy,
                        .PONumber = domainDeliveryTicket.PONumber,
                        .ShipDate = domainDeliveryTicket.ShipDate,
                        .ShipTime = domainDeliveryTicket.ShipTime,
                        .IsClosed = (domainDeliveryTicket.CloseTicket.HasValue AndAlso domainDeliveryTicket.CloseTicket.Value),
                        .HoldDown = domainDeliveryTicket.HoldDown,
                        .Stroke = domainDeliveryTicket.Stroke,
                        .CompletedBy = domainDeliveryTicket.CompletedBy,
                        .ReasonStillOpen = domainDeliveryTicket.ReasonStillOpen,
                        .RepairedBy = domainDeliveryTicket.RepairedBy,
                        .SalesTaxRate = domainDeliveryTicket.SalesTaxRate,
                        .CountySalesTaxRateID = domainDeliveryTicket.CountySalesTaxRateID,
                        .CountySalesTaxRateName = If(domainDeliveryTicket.CountySalesTaxRate IsNot Nothing, domainDeliveryTicket.CountySalesTaxRate.CountyName, "None Chosen"),
                        .Notes = domainDeliveryTicket.Notes,
                        .InvBarrel = domainDeliveryTicket.InvBarrel,
                        .InvSVCages = domainDeliveryTicket.InvSVCages,
                        .InvDVCages = domainDeliveryTicket.InvDVCages,
                        .InvSVSeats = domainDeliveryTicket.InvSVSeats,
                        .InvDVSeats = domainDeliveryTicket.InvDVSeats,
                        .InvSVBalls = domainDeliveryTicket.InvSVBalls,
                        .InvDVBalls = domainDeliveryTicket.InvDVBalls,
                        .InvHoldDown = domainDeliveryTicket.InvHoldDown,
                        .InvValveRod = domainDeliveryTicket.InvValveRod,
                        .InvPlunger = domainDeliveryTicket.InvPlunger,
                        .InvPTVCages = domainDeliveryTicket.InvPTVCages,
                        .InvPDVCages = domainDeliveryTicket.InvPDVCages,
                        .InvPTVSeats = domainDeliveryTicket.InvPTVSeats,
                        .InvPDVSeats = domainDeliveryTicket.InvPDVSeats,
                        .InvPTVBalls = domainDeliveryTicket.InvPTVBalls,
                        .InvPDVBalls = domainDeliveryTicket.InvPDVBalls,
                        .InvRodGuide = domainDeliveryTicket.InvRodGuide,
                        .InvTypeBallandSeat = domainDeliveryTicket.InvTypeBallandSeat,
                        .Quote = (domainDeliveryTicket.Quote.HasValue AndAlso domainDeliveryTicket.Quote.Value),
                        .DisplaySignatureDate = domainDeliveryTicket.SignatureDate,
                        .DisplaySignatureName = domainDeliveryTicket.SignatureName,
                        .QuickbooksInvoiceNumber = domainDeliveryTicket.QuickbooksInvoiceNumber,
                        .InvoiceStatus = domainDeliveryTicket.InvoiceStatus,
                        .InvoiceStatusText = DirectCast(domainDeliveryTicket.InvoiceStatus, AcePumpInvoiceStatuses).GetDescription(),
                        .RequiresPaymentUpFront = domainDeliveryTicket.Customer.PayUpFront.HasValue AndAlso domainDeliveryTicket.Customer.PayUpFront.Value
                    }
        End Function

        Private Function TryParseDate(from As String) As Date?
            Dim buffer As Date

            If [from] IsNot Nothing AndAlso Date.TryParse([from], buffer) Then
                Return buffer
            Else
                Return Nothing
            End If
        End Function

        '
        'GET: /DeliveryTicket/Edit/id

        <HttpGet()>
        Public Function Edit(id As Integer) As ActionResult
            Dim dt As DeliveryTicketViewModel = GenerateDeliveryTicketViewModelFromPersisted(id)

            If dt IsNot Nothing AndAlso Not dt.IsClosed Then
                Return View(dt)
            ElseIf dt IsNot Nothing AndAlso dt.IsClosed Then
                Return View("Details", dt)
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        'POST: /DeliveryTicket/Edit

        <HttpPost()>
        Public Function Edit(model As DeliveryTicketViewModel) As ActionResult
            UpdateWellInstalledIn(model)

            If Not ModelState.IsValid Then
                Return View(model)
            Else
                Dim toEdit As DeliveryTicket = DataSource.DeliveryTickets _
                                       .FirstOrDefault(Function(m) m.DeliveryTicketID = model.DeliveryTicketID)

                If toEdit IsNot Nothing Then
                    toEdit.LastPull = If(model.LastPull.HasValue, model.LastPull.Value.ToShortDateString, Nothing)
                    toEdit.SalesTaxRate = model.SalesTaxRate
                    toEdit.CloseTicket = model.IsClosed
                    DataSource.DeliveryTickets.LoadChanges(model)

                    DataSource.SaveChanges()

                    Return RedirectToAction("Details", New With {.id = model.DeliveryTicketID})
                End If
            End If

            Return RedirectToAction("Index")
        End Function

        '
        ' POST: /DeliveryTicket/Copy

        <HttpPost()>
        Public Function Copy(deliveryTicketId As Integer, wellId As Integer?) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets _
                                                .Include(Function(x) x.LineItems) _
                                                .SingleOrDefault(Function(x) x.DeliveryTicketID = deliveryTicketId)

            Dim well As Well = DataSource.WellLocations.Find(wellId)

            If ticket IsNot Nothing And well IsNot Nothing Then
                Dim newTicket As New DeliveryTicket() With {
                    .CloseTicket = ticket.CloseTicket,
                    .SalesTaxRate = ticket.SalesTaxRate,
                    .CompletedBy = ticket.CompletedBy,
                    .CustomerID = well.CustomerID,
                    .HoldDown = ticket.HoldDown,
                    .InvBarrel = ticket.InvBarrel,
                    .InvDVBalls = ticket.InvDVBalls,
                    .InvDVCages = ticket.InvDVCages,
                    .InvDVSeats = ticket.InvDVSeats,
                    .InvHoldDown = ticket.InvHoldDown,
                    .InvPDVBalls = ticket.InvPDVBalls,
                    .InvPDVCages = ticket.InvPDVCages,
                    .InvPDVSeats = ticket.InvPDVSeats,
                    .InvPlunger = ticket.InvPlunger,
                    .InvPTVBalls = ticket.InvPTVBalls,
                    .InvPTVCages = ticket.InvPTVCages,
                    .InvPTVSeats = ticket.InvPTVSeats,
                    .InvRodGuide = ticket.InvRodGuide,
                    .InvSVBalls = ticket.InvSVBalls,
                    .InvSVCages = ticket.InvSVCages,
                    .InvSVSeats = ticket.InvSVSeats,
                    .InvTypeBallandSeat = ticket.InvTypeBallandSeat,
                    .InvValveRod = ticket.InvValveRod,
                    .LastPull = ticket.LastPull,
                    .Notes = ticket.Notes,
                    .OrderDate = ticket.OrderDate,
                    .OrderedBy = ticket.OrderedBy,
                    .PONumber = ticket.PONumber,
                    .ShipDate = ticket.ShipDate,
                    .ShipVia = ticket.ShipVia,
                    .Stroke = ticket.Stroke,
                    .TicketDate = ticket.TicketDate,
                    .WellID = wellId,
                    .CountySalesTaxRateID = ticket.CountySalesTaxRateID
                }

                newTicket.LineItems = New List(Of LineItem)

                For Each lineItem As LineItem In ticket.LineItems
                    newTicket.LineItems.Add(New LineItem() With {
                                                .CollectSalesTax = lineItem.CollectSalesTax,
                                                .Description = lineItem.Description,
                                                .PartTemplateID = lineItem.PartTemplateID,
                                                .Quantity = lineItem.Quantity,
                                                .SortOrder = lineItem.SortOrder,
                                                .UnitDiscount = lineItem.UnitDiscount,
                                                .UnitPrice = lineItem.UnitPrice
                                            })
                Next

                DataSource.DeliveryTickets.Add(newTicket)
                DataSource.SaveChanges()

                Return RedirectToAction("Details", New With {.id = newTicket.DeliveryTicketID})

            Else
                Return RedirectToAction("Details", New With {.id = deliveryTicketId})
            End If
        End Function

        Private Sub UpdateWellInstalledIn(model As DeliveryTicketViewModel)
            If model.PumpDispatchedID.HasValue Then
                Dim pump As Pump = DataSource.Pumps.Find(model.PumpDispatchedID.Value)

                If pump IsNot Nothing Then
                    Dim pumpsWithThisWell As IQueryable(Of Pump) = DataSource.Pumps.Where(Function(x) x.InstalledInWellID = model.WellID)
                    For Each pumpInWell As Pump In pumpsWithThisWell
                        pumpInWell.InstalledInWellID = Nothing
                    Next
                    pump.InstalledInWellID = model.WellID
                End If
            End If
        End Sub

        '
        ' GET: /DeliveryTicket/GetSmallImage/id
        <HttpGet()>
        Public Function GetSmallImage(id As Integer) As ActionResult
            Using dtImageRepo As New DeliveryTicketImageUploadRepository(DataSource, ImageProcessingFactory.GetImageProcessingLibrary(), StorageFactory.GetStorageProvider(VirtualPathMapper.Instance))
                Dim smImage = dtImageRepo.GetSmallImage(id)

                If smImage IsNot Nothing Then
                    Return New FileStreamResult(smImage.ReadStream, If(smImage.MimeType, "image/jpeg"))

                Else
                    Return New HttpStatusCodeResult(HttpStatusCode.NotFound)
                End If
            End Using
        End Function

        '
        ' GET: /DeliveryTicket/GetLargeImage/id
        <HttpGet()>
        Public Function GetLargeImage(id As Integer) As ActionResult
            Using dtImageRepo As New DeliveryTicketImageUploadRepository(DataSource, ImageProcessingFactory.GetImageProcessingLibrary(), StorageFactory.GetStorageProvider(VirtualPathMapper.Instance))
                Dim lgImage = dtImageRepo.GetLargeImage(id)

                If lgImage IsNot Nothing Then
                    Return New FileStreamResult(lgImage.ReadStream, If(lgImage.MimeType, "image/jpeg"))

                Else
                    Return New HttpStatusCodeResult(HttpStatusCode.NotFound)
                End If
            End Using
        End Function

        '
        ' POST: /DeliveryTicket/ImageGridList

        <HttpPost()>
        Public Function ImageGridList(id As Integer, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return FileManager.List(id, req)
        End Function

        '
        ' POST: /DeliveryTicket/UploadImage

        <HttpPost()>
        Public Function UploadImage(id As Integer) As ActionResult
            Dim deliveryTicket As DeliveryTicket = DataSource.DeliveryTickets.Find(id)

            If deliveryTicket Is Nothing Then
                ModelState.AddModelError("Upload failed", "Could not find that delivery ticket.")
            ElseIf Request.Files.Count = 0 Then
                ModelState.AddModelError("Upload failed", "No files were selected for upload.")
            Else
                SaveImages(deliveryTicket)
            End If

            If ModelState.IsValid() Then
                Return Json(String.Empty)
            Else
                Return Json(ModelState)
            End If
        End Function

        Private Sub SaveImages(deliveryTicket As DeliveryTicket)
            For Each fileName As String In Request.Files
                Dim upload As HttpPostedFileBase = Request.Files(fileName)

                Dim imageBytes(upload.ContentLength) As Byte
                upload.InputStream.Read(imageBytes, 0, upload.ContentLength)
                If FileUtil.IsJPG(imageBytes) Then
                    Using dtImageRepo As New DeliveryTicketImageUploadRepository(DataSource, ImageProcessingFactory.GetImageProcessingLibrary(), StorageFactory.GetStorageProvider(VirtualPathMapper.Instance))
                        Dim contentType As String = upload.ContentType
                        Dim uploadedBy As String = User.Identity.Name

                        dtImageRepo.AddUpload(imageBytes, contentType, uploadedBy, deliveryTicket)
                    End Using
                Else
                    ModelState.AddModelError(upload.FileName, "Could not upload this image. Only JPG images are allowed.")
                End If
            Next

            DataSource.SaveChanges()
        End Sub

        '
        ' POST: /DeliveryTicket/DeleteImage

        <HttpPost()>
        Public Function DeleteImage(model As DeliveryTicketImageUploadGridRow) As ActionResult
            Using dtImageRepo As New DeliveryTicketImageUploadRepository(DataSource, ImageProcessingFactory.GetImageProcessingLibrary(), StorageFactory.GetStorageProvider(VirtualPathMapper.Instance))
                dtImageRepo.DeleteUpload(model.DeliveryTicketImageUploadID)

                Return New HttpStatusCodeResult(HttpStatusCode.NoContent)
            End Using
        End Function

        '
        ' POST: /DeliveryTicket/EditImageInfo

        <HttpPost()>
        Public Function EditImageInfo(model As DeliveryTicketImageUploadGridRow) As ActionResult
            Return FileManager.Update(model)
        End Function

        '
        ' GET: /DeliveryTicket/Choose

        <HttpGet()>
        Public Function Choose() As ActionResult
            If TempData("Error") IsNot Nothing Then
                ModelState.AddModelError("", TempData("Error"))
            End If

            Return View(ModelState)
        End Function

        '
        ' POST: /DeliveryTicket/Choose

        <HttpPost()>
        Public Function Choose(id As Integer?) As ActionResult
            If id.HasValue Then
                Return RedirectToAction("Sign", New With {.id = id})
            Else
                ModelState.AddModelError("", "No valid deliveryticket number was entered.")
                Return View()
            End If
        End Function

        '
        ' GET: /DeliveryTicket/Sign

        <HttpGet()>
        Public Function Sign(id As Integer?) As ActionResult
            If id.HasValue Then
                Dim deliveryTicket As DeliveryTicketSignatureViewModel = DataSource.DeliveryTickets _
                                                                                   .Where(Function(x) x.DeliveryTicketID = id.Value) _
                                                                                   .Select(DeliveryTicketSignatureViewModelMapper.Selector) _
                                                                                   .SingleOrDefault()


                If deliveryTicket Is Nothing Then
                    TempData("Error") = "Could not find that delivery ticket. Please try again."
                    Return RedirectToAction("Choose")
                End If

                Return View(deliveryTicket)
            Else
                Return RedirectToAction("Choose")
            End If
        End Function

        '
        ' POST: /DeliveryTicket/Sign

        <HttpPost()>
        Public Function Sign(model As DeliveryTicketSignatureViewModel) As ActionResult
            If Not ModelState.IsValid() Then
                Return View(model)
            End If

            Dim deliveryTicket As DeliveryTicket = DataSource.DeliveryTickets _
                                                                .Include(Function(x) x.LineItems) _
                                                                .SingleOrDefault(Function(x) x.DeliveryTicketID = model.DeliveryTicketID)

            If deliveryTicket Is Nothing Then
                TempData("Error") = "There was a problem signing that delivery ticket. Please try again."
                Return RedirectToAction("Choose")
            End If

            Dim signatureByteArray As Byte() = ConvertSignatureBase64ToByteArray(model.SignatureBase64)
            deliveryTicket.SignatureDate = Today()
            deliveryTicket.SignatureName = model.SignatureName
            deliveryTicket.SignatureCompanyName = model.SignatureCompanyName
            deliveryTicket.Signature = signatureByteArray

            model.SignatureDate = deliveryTicket.SignatureDate
            model.Signature = signatureByteArray

            DataSource.SaveChanges()

            Return View(DeliveryTicketSignatureViewModelMapper.Convert(deliveryTicket))
        End Function

        Private Function ConvertSignatureBase64ToByteArray(signature As String) As Byte()
            signature = signature.Replace(SignatureDataFormat, "")
            Return Convert.FromBase64String(signature)
        End Function

        '
        ' POST: /DeliveryTicket/Resign

        <HttpPost()>
        Public Function Resign(model As DeliveryTicketSignatureViewModel) As ActionResult
            Dim deliveryTicket As DeliveryTicket = DataSource.DeliveryTickets.SingleOrDefault(Function(x) x.DeliveryTicketID = model.DeliveryTicketID)

            If deliveryTicket Is Nothing Then
                TempData("Error") = "There was a problem resigning that delivery ticket. Please try again."
                Return RedirectToAction("Choose", New With {.id = model.DeliveryTicketID})
            End If

            deliveryTicket.SignatureName = ""
            deliveryTicket.SignatureDate = Nothing
            deliveryTicket.SignatureCompanyName = ""

            DataSource.SaveChanges()

            Return RedirectToAction("Sign", New With {.id = model.DeliveryTicketID})
        End Function

        '
        ' POST: /DeliveryTicket/DeleteSignature

        <HttpPost()>
        Public Function DeleteSignature(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets.Find(id)

            If ticket Is Nothing Then
                ModelState.AddModelError("deliveryticketid", "Could not find that ticket")
                Return Json(New With {.Success = False,
                                      .Errors = ModelState.SelectMany(Function(x) x.Value.Errors)})
            End If

            ticket.SignatureDate = Nothing
            ticket.Signature = Nothing
            ticket.SignatureName = ""
            ticket.SignatureCompanyName = ""

            DataSource.SaveChanges()

            Return Json(New With {.Success = True})
        End Function

        '
        ' POST: /DeliveryTicket/OpenForQuickbooks

        <HttpPost()>
        Public Function OpenForQuickbooks(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets.Find(id)
            If ticket IsNot Nothing Then
                ticket.QuickbooksID = Nothing
                ticket.QuickbooksInvoiceNumber = Nothing
                ticket.CloseTicket = False
                ticket.InvoiceStatus = AcePumpInvoiceStatuses.ReadyForQuickbooks

                DataSource.SaveChanges()
                Return Json(New With {.Success = True})
            End If
            Return Json(New With {.Success = False})
        End Function

        '
        ' POST: /DeliveryTicket/MarkAsReadyForQuickbooks

        <HttpPost()>
        Public Function MarkAsReadyForQuickbooks(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets.Find(id)
            If ticket IsNot Nothing AndAlso ticket.CloseTicket = True AndAlso Not ticket.InvoiceStatus = AcePumpInvoiceStatuses.InQuickbooks Then
                ticket.InvoiceStatus = AcePumpInvoiceStatuses.ReadyForQuickbooks

                DataSource.SaveChanges()
                Return Json(New With {.Success = True, .StatusText = AcePumpInvoiceStatuses.ReadyForQuickbooks.GetDescription()})
            End If
            Return Json(New With {.Success = False})
        End Function

        '
        ' POST: /DeliveryTicket/UpdateQbInvoiceNumber

        <HttpPost()>
        Public Function UpdateQbInvoiceNumber(id As Integer, invoiceNumber As String) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets.Find(id)
            If ticket IsNot Nothing AndAlso ticket.CloseTicket = True AndAlso Not ticket.InvoiceStatus = AcePumpInvoiceStatuses.InQuickbooks AndAlso String.IsNullOrEmpty(ticket.QuickbooksInvoiceNumber) Then
                ticket.InvoiceStatus = AcePumpInvoiceStatuses.InQuickbooks
                ticket.QuickbooksInvoiceNumber = invoiceNumber

                DataSource.SaveChanges()
                Return Json(New With {.Success = True,
                                      .StatusText = AcePumpInvoiceStatuses.InQuickbooks.GetDescription(),
                                      .InvoiceNumberToUpdate = invoiceNumber})
            End If
            Return Json(New With {.Success = False})
        End Function

        '
        ' POST: /DeliveryTicket/InvoiceStatusesList

        <HttpPost()>
        Public Function InvoiceStatusList() As ActionResult
            Return Json(GetType(AcePumpInvoiceStatuses).GetFields() _
                                                  .Where(Function(field) _
                                                         field.GetCustomAttributes(True) _
                                                         .Where(Function(f) f.GetType() = GetType(InvoiceStatusAccessAttribute)) _
                                                         .Cast(Of InvoiceStatusAccessAttribute) _
                                                         .Any(Function(attribute) attribute.UserAccess = True)) _
                                                  .Select(Function(filtered) _
                                                              New With {.ID = CInt(filtered.GetValue(Nothing)),
                                                                        .Text = CType(filtered.GetValue(Nothing), AcePumpInvoiceStatuses).GetDescription()
                                                                       }
                                                          )
                        )
        End Function

        '
        ' POST: /DeliveryTicket/UpdatePlungerBarrelWear

        <HttpPost()>
        Public Function UpdatePlungerBarrelWear(id As Integer, plungerBarrelWear As String) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets.Find(id)
            If ticket IsNot Nothing Then
                ticket.PlungerBarrelWear = plungerBarrelWear

                DataSource.SaveChanges()
                Return Json(New With {.Success = True})
            End If
            Return Json(New With {.Success = False})
        End Function

        '
        ' POST: /DeliveryTicket/GetPlungerBarrelWear

        <HttpPost()>
        Public Function GetPlungerBarrelWear(id As Integer) As ActionResult
            Dim ticket As DeliveryTicket = DataSource.DeliveryTickets.Find(id)
            If ticket IsNot Nothing Then
                Return Json(New With {.Success = True,
                                      .plungerBarrelWear = ticket.PlungerBarrelWear})
            Else
                Return Json(New With {.Success = False})
            End If
        End Function
    End Class
End Namespace