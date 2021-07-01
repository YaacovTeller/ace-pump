Imports AcePump.Domain.Models
Imports AcePump.Web.Controllers
Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
Imports Kendo.Mvc.Extensions
Imports Yesod.Mvc
Imports Yesod.Ef
Imports Kendo.Mvc.UI

Namespace Areas.Employees.Controllers

    <Authorize()> _
    Public Class CountySalesTaxRateController
        Inherits AcePumpControllerBase

        Private _CountySalesTaxRateManager As KendoGridRequestManager(Of CountySalesTaxRate, CountySalesTaxRateViewModel)
        Private ReadOnly Property CountySalesTaxRateManager As KendoGridRequestManager(Of CountySalesTaxRate, CountySalesTaxRateViewModel)
            Get
                If _CountySalesTaxRateManager Is Nothing Then
                    _CountySalesTaxRateManager = New KendoGridRequestManager(Of CountySalesTaxRate, CountySalesTaxRateViewModel)(
                        DataSource,
                        Function(x As CountySalesTaxRate) New CountySalesTaxRateViewModel With {
                            .CountySalesTaxRateID = x.CountySalesTaxRateID,
                            .CountyName = x.CountyName,
                            .SalesTaxRate = x.SalesTaxRate
                        },
                        Nothing,
                        Me)
                End If

                Return _CountySalesTaxRateManager
            End Get
        End Property

        '
        ' GET: /CountySalesTaxRate/Index

        Function Index() As ActionResult
            Return View()
        End Function

        '
        ' POST: /CountySalesTaxRate/List

        <HttpPost()> _
        Public Function List(<DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return CountySalesTaxRateManager.List(req)
        End Function

        '
        ' GET: /CountySalesTaxRate/Create

        Public Function Create() As ActionResult
            Return RedirectToAction("Index")
        End Function

        '
        ' POST: /CountySalesTaxRate/Create

        <HttpPost()> _
        Public Function Create(model As CountySalesTaxRateViewModel) As ActionResult
            If Not ModelState.IsValid Then
                Return View(model)
            End If

            Dim newTaxRate As CountySalesTaxRate = DataSource.CountySalesTaxRates.LoadNew(model)

            DataSource.SaveChanges()

            Return Json({CountySalesTaxRateManager.ModelMapper.Convert(newTaxRate)}.ToDataSourceResult(New DataSourceRequest))
        End Function

        '
        ' POST: /CountySalesTaxRate/Update

        <HttpPost()> _
        Public Function Update(model As CountySalesTaxRateViewModel) As ActionResult
            If Not ModelState.IsValid Then
                Return View(model)
            End If

            If DataSource.CountySalesTaxRates.LoadChanges(model) Then
                DataSource.SaveChanges()
            Else
                ModelState.AddModelError("CountySalesTaxRateID", "Could not find the county sales tax rate to update.")
            End If

            Return Json({model}.ToDataSourceResult(New DataSourceRequest))
        End Function

        '
        ' POST: /CountySalesTaxRate/GetRate

        <HttpPost()> _
        Public Function GetRate(id As Integer) As ActionResult
            Dim countySalesTaxRate As CountySalesTaxRate = DataSource.CountySalesTaxRates.Find(id)
            If countySalesTaxRate Is Nothing Then
                Return Json(New With {.Success = False})
            Else
                Return Json(New With {.Success = True,
                                      .CountySalesTaxRateID = countySalesTaxRate.CountySalesTaxRateID,
                                      .SalesTaxRate = countySalesTaxRate.SalesTaxRate,
                                      .CountyName = countySalesTaxRate.CountyName})
            End If
        End Function


        Public Function StartsWith(text As String) As ActionResult
            If String.IsNullOrEmpty(text) Then
                text = Request("filter[filters][0][value]")
            End If

            Return Json(DataSource.CountySalesTaxRates _
                            .Where(Function(c) c.CountyName.StartsWith(text)) _
                            .Select(Function(c) New With {
                                        .CountyName = c.CountyName,
                                        .SalseTaxRate = c.SalesTaxRate,
                                        .Id = c.CountySalesTaxRateID
                                    }),
                        JsonRequestBehavior.AllowGet
                        )
        End Function

        '
        ' POST: /CountySalesTaxRate/OpenForQuickbooks

        <HttpPost()> _
        Public Function OpenForQuickbooks(id As Integer) As ActionResult
            Dim rate As CountySalesTaxRate = DataSource.CountySalesTaxRates.Find(id)
            If rate IsNot Nothing Then
                rate.QuickbooksID = Nothing

                DataSource.SaveChanges()
                Return Json(New With {.Success = True})
            End If
            Return Json(New With {.Success = False})
        End Function
    End Class
End Namespace
