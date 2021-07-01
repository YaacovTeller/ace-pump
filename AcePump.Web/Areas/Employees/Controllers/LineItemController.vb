Imports System.Linq.Expressions
Imports AcePump.Common.Soris
Imports AcePump.Domain
Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models
Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Web.Controllers
Imports DelegateDecompiler
Imports Kendo.Mvc.Extensions
Imports Kendo.Mvc.UI
Imports Yesod.Ef
Imports Yesod.Mvc

Namespace Areas.Employees.Controllers

    <Authorize()>
    Public Class LineItemController
        Inherits AcePumpControllerBase

        Private ReadOnly Property CustomerID As Integer
            Get
                Return HttpContext.AcePumpUser.Profile.CustomerID
            End Get
        End Property

        Private _LineItemManager As KendoGridRequestManager(Of LineItem, LineItemsGridRowViewModel)
        Friend ReadOnly Property LineItemManager As KendoGridRequestManager(Of LineItem, LineItemsGridRowViewModel)
            Get
                Dim ex As Expression(Of Func(Of LineItem, LineItemsGridRowViewModel)) = Function(x As LineItem) New LineItemsGridRowViewModel With {
                    .LineItemID = x.LineItemID,
                    .DeliveryTicketID = x.DeliveryTicketID,
                    .AddedFromRepairTicket = x.PartInspectionID.HasValue,
                    .Quantity = x.Quantity,
                    .PartTemplateID = If(x.PartTemplateID.HasValue, x.PartTemplateID.Value, 0),
                    .PartTemplateNumber = If(x.PartTemplate IsNot Nothing, x.PartTemplate.Number, ""),
                    .Description = x.Description,
                    .CollectSalesTax = x.CollectSalesTax,
                    .UnitPrice = x.UnitPrice,
                    .UnitDiscount = x.UnitDiscount,
                    .CustomerDiscount = x.CustomerDiscount,
                    .UnitPriceAfterDiscount = Udf.ClrRound_10_4(x.UnitPriceAfterDiscount.Computed()),
                    .LineTotal = Udf.ClrRound_10_4(x.LineTotal.Computed()),
                    .SalesTaxAmount = Udf.ClrRound_10_4(x.SalesTaxAmount.Computed()),
                    .SortOrder = x.SortOrder
                }

                If _LineItemManager Is Nothing Then
                    _LineItemManager = New KendoGridRequestManager(Of LineItem, LineItemsGridRowViewModel)(
                        DataSource,
                        ex.Decompile(),
                        Function(x) x.DeliveryTicketID,
                        Me)
                End If

                Return _LineItemManager
            End Get
        End Property

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Public Sub New()
            Me.New(Nothing)
        End Sub

        '
        ' GET: /LineItem/Index

        Function Index() As ActionResult
            Return RedirectToAction("Index", "DeliveryTicket")
        End Function

        '
        ' POST: /LineItem/List

        <HttpPost()>
        Public Function List(id As Integer, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return LineItemManager.List(id, req)
        End Function

        '
        ' GET: /LineItem/Create

        Public Function Create() As ActionResult
            Return RedirectToAction("Index")
        End Function

        '
        ' POST: /LineItem/Create

        <HttpPost()>
        Public Function Create(model As LineItemsGridRowViewModel, Optional lineItemIDTarget As Integer = 0) As ActionResult
            If ModelState.IsValid Then
                Dim insertPosition As Integer

                If lineItemIDTarget > 0 Then
                    insertPosition = DataSource.LineItems _
                                               .Where(Function(x) x.LineItemID = lineItemIDTarget) _
                                               .Select(Function(x) x.SortOrder) _
                                               .DefaultIfEmpty(0) _
                                               .FirstOrDefault()
                    insertPosition = insertPosition + 1
                ElseIf lineItemIDTarget = -1 Then
                    insertPosition = DataSource.LineItems _
                                               .Where(Function(x) x.DeliveryTicketID = model.DeliveryTicketID) _
                                               .Select(Function(x) x.SortOrder) _
                                               .DefaultIfEmpty(0) _
                                               .Min()
                ElseIf lineItemIDTarget = 0 Then
                    insertPosition = DataSource.LineItems _
                                               .Where(Function(l) l.DeliveryTicketID = model.DeliveryTicketID) _
                                               .Select(Function(l) l.SortOrder) _
                                               .DefaultIfEmpty(-1) _
                                               .Max()
                    insertPosition = insertPosition + 1
                End If

                MoveUpLineItemSort(model.DeliveryTicketID, insertPosition)

                Dim created As LineItem = DataSource.LineItems.LoadNew(model)


                created.SortOrder = insertPosition

                Dim loadDefaultsFrom As PartTemplate = DataSource.PartTemplates.Find(created.PartTemplateID)
                created.Description = loadDefaultsFrom.Description
                created.UnitPrice = loadDefaultsFrom.ListPrice
                created.UnitDiscount = loadDefaultsFrom.Discount
                created.CustomerDiscount = If(loadDefaultsFrom.CustomersWithSpecials.Any(), loadDefaultsFrom.CustomersWithSpecials.FirstOrDefault.Discount, New Nullable(Of Decimal)())
                created.CollectSalesTax = loadDefaultsFrom.Taxable

                If model.Quantity > 0 Then
                    created.Quantity = model.Quantity
                Else
                    created.Quantity = 1
                End If

                DataSource.SaveChanges()
                Return Json({LineItemManager.ModelMapper.Convert(created)}.ToDataSourceResult(New DataSourceRequest))
            End If

            Return Json({model}.ToDataSourceResult(New DataSourceRequest), ModelState)
        End Function

        '
        ' POST: /LineItem/Update

        <HttpPost()>
        Public Function Update(model As LineItemsGridRowViewModel) As ActionResult
            Dim toUpdate As LineItem = DataSource.LineItems.Find(model.LineItemID)

            If toUpdate IsNot Nothing Then
                toUpdate.Quantity = model.Quantity
                toUpdate.CollectSalesTax = model.CollectSalesTax

                If model.PartTemplateID <> toUpdate.PartTemplateID AndAlso model.PartTemplateID <> 0 Then
                    Dim newPartTemplate As PartTemplate = DataSource.PartTemplates.Find(model.PartTemplateID)

                    toUpdate.PartTemplateID = newPartTemplate.PartTemplateID
                    toUpdate.Description = newPartTemplate.Description
                    toUpdate.UnitDiscount = newPartTemplate.Discount
                    toUpdate.CustomerDiscount = If(newPartTemplate.CustomersWithSpecials.Any(), newPartTemplate.CustomersWithSpecials.FirstOrDefault.Discount, New Nullable(Of Decimal)())
                    toUpdate.UnitPrice = newPartTemplate.ListPrice
                    toUpdate.CollectSalesTax = newPartTemplate.Taxable
                Else
                    toUpdate.Description = model.Description
                    toUpdate.UnitDiscount = model.UnitDiscount
                    toUpdate.UnitPrice = model.UnitPrice
                    If toUpdate.CustomerDiscount.HasValue Then
                        toUpdate.CustomerDiscount = Nothing
                    End If
                End If

                DataSource.SaveChanges()

            Else
                ViewData.ModelState.AddModelError("LineItemID", "Could not find that part on the pump template to update.")

            End If
            Return Json({LineItemManager.ModelMapper.Convert(toUpdate)}.ToDataSourceResult(New DataSourceRequest))
        End Function

        '
        ' POST: /LineItem/ZeroOutPrice

        <HttpPost()>
        Public Function ZeroOutPrice(id As Integer) As ActionResult
            Dim lineItem As LineItem = DataSource.LineItems.Find(id)
            If lineItem IsNot Nothing Then
                ZeroOutSingleLineItemPrice(lineItem)
                DataSource.SaveChanges()
            End If

            Return Json(New With {.Success = (lineItem IsNot Nothing)})
        End Function

        '
        ' POST: /LineItem/ZeroOutAllPrices

        <HttpPost()>
        Public Function ZeroOutAllPrices(deliveryticketId As Integer) As ActionResult
            Dim lineItems = DataSource.LineItems.Where(Function(x) x.DeliveryTicketID = deliveryticketId)
            For Each item In lineItems
                ZeroOutSingleLineItemPrice(item)
            Next
            DataSource.SaveChanges()

            Return Json(New With {.Success = True})
        End Function

        '
        ' POST: /LineItem/RemovePart

        <HttpPost()>
        Public Function Remove(LineItemID As Integer) As ActionResult
            Dim toDelete As LineItem = DataSource.LineItems.Find(LineItemID)

            If toDelete IsNot Nothing Then
                DataSource.LineItems.Remove(toDelete)
                DataSource.SaveChanges()
            End If

            Return Json(New With {.Success = (toDelete IsNot Nothing)})
        End Function

        '
        ' POST: /LineItem/SwapParts

        <HttpPost()>
        Public Function Swap(firstLineItemId As Integer, secondLineItemId As Integer) As ActionResult
            Dim firstLineItem As LineItem = DataSource.LineItems.SingleOrDefault(Function(x) x.LineItemID = firstLineItemId)
            Dim secondLineItem As LineItem = DataSource.LineItems.SingleOrDefault(Function(x) x.LineItemID = secondLineItemId)

            If firstLineItem IsNot Nothing And secondLineItem IsNot Nothing Then
                Dim firstPartDefOriginalOrder As Integer = firstLineItem.SortOrder
                firstLineItem.SortOrder = secondLineItem.SortOrder
                secondLineItem.SortOrder = firstPartDefOriginalOrder

                DataSource.SaveChanges()

                Return Json(New With {.Success = True})

            Else
                Return Json(New With {.Success = False})
            End If
        End Function

        '
        ' POST: /LineItem/UpdatePosition

        <HttpPost()>
        Public Function UpdatePosition(lineItemIDToChange As Integer, lineItemIDTarget As Integer) As ActionResult
            Dim toChange As LineItem = DataSource.LineItems.Find(lineItemIDToChange)
            Dim targetPosition As Integer

            If toChange Is Nothing Then
                Return Json(New With {.Success = False})
            End If

            If lineItemIDTarget = -1 Then
                targetPosition = DataSource.LineItems _
                                            .Where(Function(x) x.DeliveryTicketID = toChange.DeliveryTicketID) _
                                            .Select(Function(x) x.SortOrder) _
                                            .DefaultIfEmpty(0) _
                                            .Min()
            Else
                Dim target As LineItem = DataSource.LineItems.Find(lineItemIDTarget)

                If target Is Nothing Then
                    Return Json(New With {.Success = False})
                Else
                    targetPosition = target.SortOrder + 1
                End If
            End If

            MoveUpLineItemSort(toChange.DeliveryTicketID, targetPosition)

            toChange.SortOrder = targetPosition

            DataSource.SaveChanges()

            Return Json(New With {.Success = True})
        End Function

        Private Sub MoveUpLineItemSort(deliveryTicketID As Integer, startFrom As Integer)
            For Each lineItem As LineItem In DataSource.LineItems _
                                                .Where(Function(x) x.DeliveryTicketID = deliveryTicketID) _
                                                .OrderBy(Function(x) x.SortOrder)
                If lineItem.SortOrder >= startFrom Then
                    lineItem.SortOrder = lineItem.SortOrder + 1
                End If
            Next
        End Sub

        Private Sub ZeroOutSingleLineItemPrice(lineItem As LineItem)
            lineItem.UnitPrice = 0
            lineItem.UnitDiscount = 0
        End Sub
    End Class
End Namespace
