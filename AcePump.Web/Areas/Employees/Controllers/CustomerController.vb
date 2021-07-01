Imports AcePump.Domain.Models
Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports System.Data.Entity
Imports Yesod.Ef
Imports Kendo.Mvc.UI
Imports Kendo.Mvc.Extensions
Imports Yesod.Mvc

Namespace Areas.Employees.Controllers
    <Authorize(Roles:=AcePumpSecurityRoles.AcePumpEmployee)> _
    Public Class CustomerController
        Inherits AcePumpControllerBase

        Private Property CustomerMapper As New ModelMapper(Of Customer, CustomerViewModel)(
            Function(x As Customer) New CustomerViewModel With {
                .CustomerID = x.CustomerID,
                .CustomerName = x.CustomerName,
                .Address1 = x.Address1,
                .Address2 = x.Address2,
                .City = x.City,
                .State = x.State,
                .Zip = x.Zip,
                .Phone = x.Phone,
                .Website = x.Website,
                .APINumberRequired = x.APINumberRequired,
                .CountySalesTaxRateID = x.CountySalesTaxRateID,
                .CountyName = If(x.CountySalesTaxRate IsNot Nothing, x.CountySalesTaxRate.CountyName, ""),
                .UsesQuickbooksRunningInvoice = x.UsesQuickbooksRunningInvoice,
                .QbInvoiceClassID = If(x.QbInvoiceClass IsNot Nothing, x.QbInvoiceClass.QbInvoiceClassID, ""),
                .QbInvoiceClassName = If(x.QbInvoiceClass IsNot Nothing, x.QbInvoiceClass.FullName, ""),
                .UsesInventory = x.UsesInventory,
                .PayUpFront = x.PayUpFront.HasValue AndAlso x.PayUpFront.Value
                }
            )

        Private _CustomerGridManager As KendoGridRequestManager(Of Customer, CustomerGridRowModel)
        Private ReadOnly Property CustomerGridManager As KendoGridRequestManager(Of Customer, CustomerGridRowModel)
            Get
                If _CustomerGridManager Is Nothing Then
                    _CustomerGridManager = New KendoGridRequestManager(Of Customer, CustomerGridRowModel)(
                        DataSource,
                        Function(x As Customer) New CustomerGridRowModel With {
                            .CustomerID = x.CustomerID,
                            .CustomerName = x.CustomerName,
                            .Address1 = x.Address1,
                            .Address2 = x.Address2,
                            .City = x.City,
                            .State = x.State,
                            .Zip = x.Zip,
                            .Phone = x.Phone,
                            .Website = x.Website,
                            .APINumberRequired = x.APINumberRequired,
                            .CountyName = If(x.CountySalesTaxRate IsNot Nothing, x.CountySalesTaxRate.CountyName, ""),
                            .UsesInventory = x.UsesInventory,
                            .PayUpFront = x.PayUpFront.HasValue AndAlso x.PayUpFront.Value
                            },
                        Nothing,
                        Me)
                End If

                Return _CustomerGridManager
            End Get
        End Property

        Private Property CustomerPriceListGridRowMapper As New ModelMapper(Of CustomerPriceListGridRowModel, CustomerPriceListGridRowUpdateModel)(
            Function(x As CustomerPriceListGridRowModel) New CustomerPriceListGridRowUpdateModel With {
                .CustomerID = x.CustomerID,
                .PartTemplateID = x.partTemplateID,
                .Discount = x.Discount,
                .CustomerDiscount = x.CustomerDiscount
                }
            )

        '
        ' GET: /Customer/[Index]

        <HttpGet()> _
        Public Function Index() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Customer/List

        <HttpPost()> _
        Public Function List(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return CustomerGridManager.List(req)
        End Function

        '
        ' GET: /Customer/Create

        <HttpGet()> _
        Public Function Create() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Customer/Create

        <HttpPost()> _
        Public Function Create(model As CustomerViewModel) As ActionResult
            If Not ModelState.IsValid() Then
                Return View(model)
            End If

            Dim newCustomer As Customer = DataSource.Customers.LoadNew(model)
            DataSource.SaveChanges()

            Return RedirectToAction("Details", New With {.id = newCustomer.CustomerID})
        End Function

        '
        ' GET: /Customer/Edit

        <HttpGet()> _
        Public Function Edit(id As Integer) As ActionResult
            Dim customer As Customer = DataSource.Customers.SingleOrDefault(Function(c) c.CustomerID = id)

            If customer Is Nothing Then
                Return RedirectToAction("Index")
            Else

                Return View(CustomerMapper.Convert(customer))
            End If
        End Function

        '
        ' POST: /Customer/Edit
        <HttpPost()> _
        Public Function Edit(model As CustomerViewModel) As ActionResult
            If Not ModelState.IsValid Then
                Return View(model)
            End If

            Dim usedParts = From inspection In DataSource.PartInspections
                            Where inspection.ReplacedWithInventoryPartID.HasValue
                            Select inspection.ReplacedWithInventoryPart
            Dim anyAvailableParts As Boolean = DataSource.Parts.Where(Function(x) x.CustomerID = model.CustomerID) _
                                                               .Where(Function(x) Not usedParts.Contains(x)) _
                                                               .Any()
            If model.UsesInventory = False And anyAvailableParts Then
                ModelState.AddModelError("UsesInventory", "Customer still has parts in inventory. You can only uncheck this when all inventory parts for this customer have been used up.")
                Return View(model)
            End If

            If DataSource.Customers.LoadChanges(model) Then
                DataSource.SaveChanges()
                Return RedirectToAction("Details", New With {.id = model.CustomerID})
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' GET: /Customer/Details/[id]

        <HttpGet()> _
        Public Function Details(id As Integer) As ActionResult
            Dim customer As Customer = DataSource.Customers.SingleOrDefault(Function(c) c.CustomerID = id)

            If customer IsNot Nothing Then
                Return View(CustomerMapper.Convert(customer))
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' GET or POST: /Customer/StartsWith

        Public Function StartsWith(text As String, Optional filterNoCountySet As Boolean = False) As ActionResult
            If String.IsNullOrEmpty(text) Then
                text = Request("filter[filters][0][value]")
            End If

            Dim customersQuery = DataSource.Customers _
                                           .Where(Function(c) c.CustomerName.StartsWith(text))

            If filterNoCountySet Then
                customersQuery = customersQuery.Where(Function(x) x.CountySalesTaxRateID IsNot Nothing)
            End If

            Return Json(customersQuery.Select(Function(c) New With {
                                                            .Name = c.CustomerName,
                                                            .Id = c.CustomerID
                                                        }),
                        JsonRequestBehavior.AllowGet
                        )
        End Function

        '
        ' POST: /Customer/LookupAPINumberRequired

        <HttpPost()> _
        Public Function LookupAPINumberRequired(id As Integer) As ActionResult
            Dim customer As Customer = DataSource.Customers.Find(id)
            If customer Is Nothing Then
                Return Json(New With {.Success = False})
            Else
                Return Json(New With {.Success = True,
                                      .APINumberRequired = customer.APINumberRequired})
            End If
        End Function

        '
        ' POST: /Customer/LookupCountySalesTaxRate

        <HttpPost()>
        Public Function LookupCountySalesTaxRate(id As Integer) As ActionResult
            Dim customer As Customer = DataSource.Customers _
                                                 .Include(Function(x) x.CountySalesTaxRate) _
                                                 .FirstOrDefault(Function(x) x.CustomerID = id)
            If customer Is Nothing Then
                Return Json(New With {.Success = False})
            Else
                Return Json(New With {.Success = True,
                                      .CountySalesTaxRateID = customer.CountySalesTaxRate.CountySalesTaxRateID,
                                      .SalesTaxRate = customer.CountySalesTaxRate.SalesTaxRate,
                                      .CountyName = customer.CountySalesTaxRate.CountyName})
            End If
        End Function

        '
        ' POST: /Customer/LookupPayUpFront

        <HttpPost()>
        Public Function LookupPayUpFront(id As Integer) As ActionResult
            Dim customer As Customer = DataSource.Customers _
                                                .Include(Function(x) x.CountySalesTaxRate) _
                                                .FirstOrDefault(Function(x) x.CustomerID = id)
            If customer Is Nothing Then
                Return Json(New With {.Success = False})
            Else
                Return Json(New With {.Success = True,
                                      .PayUpFront = customer.PayUpFront.HasValue AndAlso customer.PayUpFront.Value})
            End If
        End Function

        '
        ' GET: /Customer/PriceList

        <HttpGet()> _
        Public Function PriceList(id As Integer) As ActionResult
            Dim customer As Customer = DataSource.Customers.Find(id)

            If customer Is Nothing Then
                Return RedirectToAction("Index")
            Else
                Return View(CustomerMapper.Convert(customer))
            End If
        End Function

        '
        ' POST: /Customer/PartPriceList

        <HttpPost()> _
        Public Function PartPriceList(id As Integer, hideGeneralDiscounts As Boolean?, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim parts As IQueryable(Of PartTemplate) = DataSource.PartTemplates

            If hideGeneralDiscounts Then
                parts = parts.Where(Function(x) x.CustomersWithSpecials.Any(Function(c) c.CustomerID = id))
            End If

            Dim result = parts.Select(Function(x) New CustomerPriceListGridRowModel With {
                            .PartTemplateID = x.PartTemplateID,
                            .CustomerID = id,
                            .PartTemplateNumber = x.Number,
                            .Description = x.Description,
                            .Category = x.PartCategory.CategoryName,
                            .Active = x.Active,
                            .Cost = x.Cost,
                            .Markup = x.Markup,
                            .Discount = x.Discount,
                            .CustomerDiscount = If(x.CustomersWithSpecials.Any(Function(c) c.CustomerID = id),
                                                   x.CustomersWithSpecials.FirstOrDefault(Function(c) c.CustomerID = id).Discount, New Nullable(Of Decimal)())
                        })

            Return Json(result.ToDataSourceResult(req))
        End Function

        '
        ' POST: /Customer/JsonEditSpecial

        <HttpPost()> _
        Public Function JsonEditSpecial(model As CustomerPriceListGridRowModel) As ActionResult
            If ModelState.IsValid Then
                UpdateDiscount(CustomerPriceListGridRowMapper.Convert(model))
                DataSource.SaveChanges()
            End If

            Return Json({model}.ToDataSourceResult(New DataSourceRequest(), ModelState))
        End Function

        Private Sub UpdateDiscount(model As CustomerPriceListGridRowUpdateModel, Optional preloaded As Boolean = False)
            Dim discountLoadedToContext As Boolean = DirectCast(DataSource, DbContext).ChangeTracker.Entries(Of CustomerPartSpecial).Any(Function(x) x.Entity.CustomerID = model.CustomerID And x.Entity.PartTemplateID = model.PartTemplateID)

            Dim customerPartSpecial As CustomerPartSpecial = Nothing
            If discountLoadedToContext Or Not preloaded Then
                customerPartSpecial = DataSource.CustomerPartSpecials.SingleOrDefault(Function(x) x.CustomerID = model.CustomerID And x.PartTemplateID = model.PartTemplateID)
                discountLoadedToContext = customerPartSpecial IsNot Nothing
            End If

            If model.CustomerDiscount.HasValue And discountLoadedToContext Then
                customerPartSpecial.Discount = model.CustomerDiscount

            ElseIf model.CustomerDiscount.HasValue Then
                DataSource.CustomerPartSpecials.Add(New CustomerPartSpecial With {
                    .PartTemplateID = model.PartTemplateID,
                    .CustomerID = model.CustomerID,
                    .Discount = model.CustomerDiscount
                })

            ElseIf discountLoadedToContext Then
                DataSource.CustomerPartSpecials.Remove(customerPartSpecial)
            End If
        End Sub

        '
        ' POST: /Customer/ResetSpecial

        <HttpPost()> _
        Public Function ResetSpecial(model As CustomerPriceListGridRowModel) As ActionResult
            If ModelState.IsValid Then
                Dim partTemplate As PartTemplate = DataSource.PartTemplates.Find(model.PartTemplateID)

                Dim customerPartSpecial As CustomerPartSpecial = partTemplate.CustomersWithSpecials.FirstOrDefault(Function(x) x.CustomerID = model.CustomerID)

                If customerPartSpecial IsNot Nothing Then
                    DataSource.CustomerPartSpecials.Remove(customerPartSpecial)
                    DataSource.SaveChanges()
                End If
            End If

            Return Json({model}.ToDataSourceResult(New DataSourceRequest(), ModelState))
        End Function

        '
        ' POST: /Customer/PartDiscountExcelExport

        <HttpPost()> _
        Public Function ExcelExport(contentType As String, base64 As String, fileName As String) As ActionResult
            Dim fileContents = Convert.FromBase64String(base64)

            Return File(fileContents, contentType, fileName)
        End Function

        '
        ' POST: /Customer/UpdateSpecials

        <HttpPost()> _
        Public Function UpdateSpecials(<DataSourceRequest()> req As DataSourceRequest, <Bind(Prefix:="models")> parts As IEnumerable(Of CustomerPriceListGridRowUpdateModel)) As ActionResult
            If parts Is Nothing Then
                ModelState.AddModelError("parts", "There are no parts to apply the discount to.")

            Else
                Dim customerIds As IQueryable(Of Integer) = parts.Select(Function(x) x.CustomerID).AsQueryable()
                Dim partTemplateIds As IQueryable(Of Integer) = parts.Select(Function(x) x.PartTemplateID).AsQueryable()
                DataSource.CustomerPartSpecials _
                    .Where(Function(x) customerIds.Contains(x.CustomerID) And partTemplateIds.Contains(x.PartTemplateID)) _
                    .Load()

                For Each viewModel As CustomerPriceListGridRowUpdateModel In parts
                    UpdateDiscount(viewModel, preloaded:=True)
                Next

                If ModelState.IsValid Then
                    DataSource.SaveChanges()
                End If
            End If

            Return Json({}, ModelState)
        End Function

        '
        ' POST: /Customer/OpenForQuickbooks

        <HttpPost()> _
        Public Function OpenForQuickbooks(id As Integer) As ActionResult
            Dim customer As Customer = DataSource.Customers.Find(id)
            If customer IsNot Nothing Then
                customer.QuickbooksID = Nothing

                DataSource.SaveChanges()
                Return Json(New With {.Success = True})
            End If
            Return Json(New With {.Success = False})
        End Function

        '
        'POST: /Customer/QbInvoinceClassList

        <HttpPost()> _
        Public Function QbInvoiceClassList() As ActionResult
            Return Json(DataSource.QbInvoiceClasses.Select(Function(x) New With {.Id = x.QbInvoiceClassID,
                                                                                 .QbInvoiceClassName = x.FullName}))
        End Function
    End Class
End Namespace