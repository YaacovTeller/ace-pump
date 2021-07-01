@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.CustomerViewModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
@Imports AcePump.Common


@Code
    ViewData("Title") = "Price List"
End Code

@Section PreloadLibs
    <script src="@Url.Content("~/Scripts/kendo/jszip.min.js")" type="text/javascript"></script>
End Section

<script type="text/javascript" src="@Url.Content("~/Scripts/Soris.Kendo.min.js")"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#chkHideGeneralDiscounts").click(function () {
            var grid = $("#parts").data("kendoGrid");

            grid.dataSource.read();
        });

        $("#btnSaveBulkDiscount").click(function() {
            var bulkDiscountAmount = parseFloat($("#BulkDiscount").val());
            if(bulkDiscountAmount === 0 || bulkDiscountAmount === null || bulkDiscountAmount === undefined || bulkDiscountAmount === "" || isNaN(bulkDiscountAmount)) {
                alert("Please enter a discount to apply.");
            } else {
                promptForSaveBulk(bulkDiscountAmount);
            }
        });

        $("#btnClearBulkDiscount").click(function() {
            promptForSaveBulk(null);
        });
    });

    function promptForSaveBulk(bulkDiscountAmount) {
        var totalRowsToChange = $("#parts").data("kendoGrid").dataSource.total();

        if (totalRowsToChange < 1) {
            alert("There are no parts in the grid to apply the discount to.");
            return;
        }

        var dialog = $("<div />").kendoWindow({
            title: "Confirm Bulk Operation",
            resizable: false,
            modal: true
        });

        dialog.parent().find(".k-window-action").css("visibility", "hidden");

        dialog.data("kendoWindow")
                .content($("#BulkOperationDialog").html())
                .center().open();

        kendo.bind(dialog, { 
            totalRows: totalRowsToChange, 
            operation: bulkDiscountAmount === null ? "remove" : "update"
        });
        
        dialog
                .find(".dialog-dont-save,.dialog-save")
                    .click(function () {
                        if ($(this).hasClass("dialog-dont-save")) {

                        } else {
                            saveDiscountForAllDataItems(bulkDiscountAmount);
                        }

                        dialog.data("kendoWindow").close();
                    })
                    .end();            
    }

    function saveDiscountForAllDataItems(bulkDiscountAmount) {
         var grid = $("#parts").data("kendoGrid");

         var data = grid.dataSource.data();
 
         for (var i = 0; i < data.length; i++) {
             data[i].CustomerDiscount = bulkDiscountAmount;
             data[i].CurrentDiscount = bulkDiscountAmount;
         }

         $.ajax({ url: '@Url.Action("UpdateSpecials", "Customer")',
                    type: 'POST',
                    cache: false,
                    traditional: true,
                    dataType: "json",
                    contentType: "application/json",                    
                    data: kendo.stringify({
                                models: $("#parts").data("kendoGrid").dataSource.data().toJSON()
                            }),
                    success: function(result) {
                        if(!result.Success) {
                            displayAjaxModelError(result.Errors);
                        } else {
                            grid.dataSource.read();
                        }
                    },
                    error: function(data) {  }
                });
    }

    function displayAjaxModelError(modelState){
            var errorString = "";
            for(var property in modelState) {
                for(var i=0; i < modelState[property].length; i++) {
                    errorString += property + ": " + modelState[property][i];
                }
            }

            alert(errorString);
        };


    function parts_AdditionalData(e) {
        var hideGeneral = $("#chkHideGeneralDiscounts").prop("checked");

        return {
            id: @Model.CustomerID,
            hideGeneralDiscounts: hideGeneral
        };
    }

    function resetDiscount_Click(e) {
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        if(dataItem.HasCustomerDiscount === true) {
            $.ajax({
                dataType: "json",
                url: "@Url.Action("ResetSpecial", "Customer")",
                data: JSON.parse(JSON.stringify(dataItem)),
                type: "POST",
                success: function() {
                        dataItem.CustomerDiscount = null;
                        $("#parts").data("kendoGrid").dataSource.read();
                        $("#parts").data("kendoGrid").refresh();
                }
            });
        }
        e.preventDefault();
    }

    $(document).ready(function () {
        $("#btnExportToExcel").click(btnExportToExcel_Click);
    });

    function btnExportToExcel_Click(e) {
        kendo.ui.progress($("#btnExportToExcel"), true) 
        var grid = $("#parts").data("kendoGrid");
        hideColumns(grid, true);
        grid.saveAsExcel();

        e.preventDefault();
    }

    function parts_ExcelExport(e) {     
        var sheet = e.workbook.sheets[0];

        for (var rowIndex = 1; rowIndex < sheet.rows.length; rowIndex++) {
            var row = sheet.rows[rowIndex];

            var colIndexResale = row.cells.length - 1;
            var colIndexDiscount  = row.cells.length - 2;
            var colIndexListPrice  = row.cells.length - 3;
            var colIndexMarkup  = row.cells.length - 4;
            var colIndexCost  = row.cells.length - 5;

            for (var cellIndex = 0; cellIndex < row.cells.length; cellIndex ++) {
                if(cellIndex === colIndexCost || cellIndex === colIndexResale || cellIndex === colIndexListPrice) {
                    row.cells[cellIndex].format = "$#####.00";
                }
                if(cellIndex === colIndexMarkup || cellIndex === colIndexDiscount) {
                    row.cells[cellIndex].format = "0.0##%";
                }
                if(e.data[rowIndex - 1].HasCustomerDiscount === true && cellIndex === colIndexDiscount) {
                    row.cells[cellIndex].color = "#0000ff";
                }
            }
        }

        hideColumns(e.sender, false);
        kendo.ui.progress($("#btnExportToExcel"), false);                
    }

    function hideColumns(grid, hide) {
        if (hide) {
            grid.hideColumn("Cost");
            grid.hideColumn("Markup");
        } else {
            grid.showColumn("Cost");
            grid.showColumn("Markup");
        }
    }

</script>

<script id="BulkOperationDialog" type="text/x-kendo-template">
    <p class="dialog-title">Do you really want to <span data-bind="{text: operation}" /> the discount for all <span data-bind="{text: totalRows}" /> parts that match the current filter?</p>

    <button class="dialog-save k-button">Yes</button>
    <button class="dialog-dont-save k-button">No</button>
</script>

<h2>Discounts for @Model.CustomerName</h2>

<p>
    <label for="chkHideGeneralDiscounts">Hide General Discounts?</label>
    <input type="checkbox" id="chkHideGeneralDiscounts" checked="checked" />
</p>

<p style="float:left">
    <label for="BulkDiscount">Set discount for all matching items:</label>
    @(Html.Kendo().NumericTextBox(Of Decimal).Name("BulkDiscount") _
                                             .Format("p2") _
                                             .Min(0) _
                                             .Max(100) _
                                             .Step(0.0005) _
                                             .Decimals(4))  
    <a class="k-button" id="btnSaveBulkDiscount">Save</a>
    <a class="k-button" id="btnClearBulkDiscount">Remove Discount</a>
</p>

<div style="clear:both"></div>

@(Html.Kendo().Grid(Of CustomerPriceListGridRowModel)() _
                                        .Name("parts") _
                                        .Filterable() _
                                        .Excel(Sub(excel) excel.FileName("Customer Price List.xlsx") _
                                                                .Filterable(True) _
                                                                .AllPages(True)) _
                                        .ToolBar(Sub(s) s.Custom().Text("Export to Excel").HtmlAttributes(New With {.id = "btnExportToExcel"})) _
                                        .Events(Sub(events)
                                                    events.ExcelExport("parts_ExcelExport")
                                                End Sub) _
                                        .Sortable() _
                                        .Pageable() _
                                        .Editable() _
                                        .Columns(Sub(c)
                                                     c.Bound(Function(x) x.PartTemplateNumber).HeaderHtmlAttributes(New With {.title = "PartTemplateNumber"})
                                                     c.Bound(Function(x) x.Description)
                                                     c.Bound(Function(x) x.Category)
                                                     c.Bound(Function(x) x.Active).ClientTemplate("# if(Active) { # Yes # } else { # No # } #")

                                                     If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
                                                         c.Bound(Function(x) x.Cost)
                                                         c.Bound(Function(x) x.Markup)
                                                     End If

                                                     c.Bound(Function(x) x.ListPrice)
                                                     c.Bound(Function(x) x.CurrentDiscount).ClientTemplate("<span #= HasCustomerDiscount ? ""style=color:blue"": """" # > #=  kendo.format('{0:p}', CurrentDiscount) #</span>").Title("Discount")
                                                     c.Bound(Function(x) x.ResalePrice)
                                                     c.Bound(Function(x) x.CustomerDiscount).Hidden(True)
                                                     c.Bound(Function(x) x.HasCustomerDiscount).Hidden(True)
                                                     c.Command(Sub(command)
                                                                   command.Edit()
                                                                   command.Custom("Reset To Default Discount").Click("resetDiscount_Click")
                                                               End Sub)
                                                 End Sub) _
                            .DataSource(Sub(dataSource)
                                            dataSource _
                                            .Ajax() _
                                            .Model(Sub(model)
                                                       model.Id(Function(id) id.PartTemplateID)
                                                       model.Field(Function(f) f.PartTemplateNumber).Editable(False)
                                                       model.Field(Function(f) f.Active).Editable(False)
                                                       model.Field(Function(f) f.Description).Editable(False)
                                                       model.Field(Function(f) f.Category).Editable(False)
                                                       model.Field(Function(f) f.Cost).Editable(False)
                                                       model.Field(Function(f) f.Markup).Editable(False)
                                                       model.Field(Function(f) f.ListPrice).Editable(False)
                                                       model.Field(Function(f) f.ResalePrice).Editable(False)
                                                       model.Field(Function(f) f.CustomerDiscount).Editable(False)
                                                       model.Field(Function(f) f.HasCustomerDiscount).Editable(False)
                                                   End Sub) _
                                            .Read(Sub(read) read.Action("PartPriceList", "Customer") _
                                                                .Data("parts_AdditionalData") _
                                                                .Type(HttpVerbs.Post)) _
                                            .Update("JsonEditSpecial", "Customer")
                                        End Sub)
    )