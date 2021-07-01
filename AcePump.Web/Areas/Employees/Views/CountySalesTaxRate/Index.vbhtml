@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
@Imports AcePump.Common
@Imports Yesod.Ef.CustomColumns

@Code
    ViewData("Title") = "Index"
End Code

<script type="text/javascript" src="@Url.Content("~/Scripts/Soris.Kendo.min.js")"></script>

<script type="text/javascript">
    function countySalesTax_Error(e) {
        if (e.errors) {
            var message = "Errors:\n";
            $.each(e.errors, function (key, value) {
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n";
                    });
                }
            });
            alert(message);
        }
    }
</script>

<h2>County Sales Tax Rates</h2>

@(Html.Kendo().Grid(Of CountySalesTaxRateViewModel)() _
    .Name("countySalesTax") _
    .Filterable() _
    .Sortable() _
    .Pageable() _
    .Editable() _
    .ToolBar(Sub(toolbar)
                     toolbar.Create().Text("Create A New Sales Tax Rate")
             End Sub) _
    .Columns(Sub(c)
                     c.Bound(Function(x) x.CountySalesTaxRateID).Visible(False)
                     c.Bound(Function(x) x.CountyName)
                     c.Bound(Function(x) x.SalesTaxRate)
                     c.Command(Sub(command)
                                       command.Edit()
                               End Sub)
             End Sub) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Ajax() _
                        .Events(Sub(events) events.Error("countySalesTax_Error")) _
                        .Model(Sub(model)
                                       model.Id(Function(id) id.CountySalesTaxRateID)
                                       model.Field(Function(f) f.CountyName)
                                       model.Field(Function(f) f.SalesTaxRate)
                               End Sub) _
                        .Read("List", "CountySalesTaxRate") _
                        .Update("Update", "CountySalesTaxRate") _
                        .Create("Create", "CountySalesTaxRate")
                End Sub)
    )