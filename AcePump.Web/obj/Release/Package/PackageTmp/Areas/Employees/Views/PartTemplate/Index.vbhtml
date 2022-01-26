@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
@Imports AcePump.Common
@Imports Yesod.Ef.CustomColumns

@Code
    ViewData("Title") = "Index"
End Code

<script type="text/javascript" src="@Url.Content("~/Scripts/Soris.Kendo.min.js")"></script>

<script type="text/javascript">

    function parts_DetailsClick(e) {
        var grid = $("#parts").data("kendoGrid");
        var currentRow = $(e.currentTarget).closest("tr");
        var currentDataItem = grid.dataItem(currentRow);

        document.location = "@Url.Action("Details", "PartTemplate")/" + currentDataItem.PartTemplateID;
    }

    function parts_ConvertToAssemblyClick(e) {
        var grid = $("#parts").data("kendoGrid");
        var currentRow = $(e.currentTarget).closest("tr");
        var part = grid.dataItem(currentRow);

        $.ajax({
            dataType: "json",
            url: "@Url.Action("ConvertToAssembly", "PartTemplate")",
            data: { id: part.PartTemplateID },
            type: "POST",
            success: function(data, status, xhr) {
                    if(!data.Success) {
                        displayAjaxModelError(data.Errors);
                    } else {
                        document.location = "@Url.Action("Edit", "Assembly")/" + data.Model.newAssemblyID;
                    }
            }
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

    function parts_DataBound(e) {
        $(".convertToAssembly").click(parts_ConvertToAssemblyClick);
    }

    function parts_Edit(e) {
        var editor = e.container;

        if (e.model.isNew()) {
            editor.find(".convertToAssembly").hide();
        }
    }

    function parts_Save(e) {
        if ((e.model.Cost / (1 - e.model.Markup) / (1 - e.model.Discount) > 1000000)) {
            alert("The resale price is above one million, which is not possible. Please change one of the values.")
            e.preventDefault();
        }
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
            //the last column, "IsAssembly" is being deleted from the columns, so resale is the last in this 0 based array.
            var colIndexResale = row.cells.length - 1;
            var colIndexDiscount  = row.cells.length - 2;
            var colIndexMarkup  = row.cells.length - 3;
            var colIndexCost  = row.cells.length - 4;

            for (var cellIndex = 0; cellIndex < row.cells.length; cellIndex ++) {
                if(cellIndex === colIndexCost || cellIndex === colIndexResale) {
                    row.cells[cellIndex].format = "$#####.00"
                }
                if(cellIndex === colIndexMarkup || cellIndex === colIndexDiscount) {
                    row.cells[cellIndex].format = "0.0##%"
                }
            }
        }

        hideColumns(e.sender, false);
        kendo.ui.progress($("#btnExportToExcel"), false);
    }

    function hideColumns(grid, hide) {
        if (hide) {
            grid.hideColumn("SoldByOptionID");
            grid.hideColumn("Active");
            grid.hideColumn("Taxable");
            grid.hideColumn("ListPrice");
            grid.hideColumn("IsAssembly");
        } else {
            grid.showColumn("SoldByOptionID");
            grid.showColumn("Active");
            grid.showColumn("Taxable");
            grid.showColumn("ListPrice");
            grid.showColumn("IsAssembly");
        }
    }
         //# sourceURL=indexEmbeddedScript.js
</script>

@Section PreloadLibs
    <script src="@Url.Content("~/Scripts/kendo/jszip.min.js")" type="text/javascript"></script>
End Section

<h2>Part List</h2>

<style type="text/css">
    .k-grid {
        font-size: 11px;
    }

    .k-grid-header th.k-header {
        vertical-align: top;
    }

        .k-grid-header th.k-header > .k-link {
            max-height: 65px;
            white-space: normal;
            vertical-align: text-top;
            text-align: center;
            text-overflow: ellipsis;
        }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        //$("#chkHideGeneralDiscounts").click(function () {
        //    var grid = $("#parts").data("kendoGrid");

        //    grid.dataSource.read();
        //});

        $("#btnSaveBulkCost").click(function() {
            var bulkCostAmount = parseFloat($("#BulkCost").val());
            if(bulkCostAmount === 0 || bulkCostAmount === null || bulkCostAmount === undefined || bulkCostAmount === "" || isNaN(bulkCostAmount)) {
                alert("Please enter a percentage to edit costs.");
            } else {
                promptForSaveBulk(bulkCostAmount);
            }
        });
    });

    function promptForSaveBulk(bulkCostAmount) {
        var totalRowsToChange = $("#parts").data("kendoGrid").dataSource.total();

        if (totalRowsToChange < 1) {
            alert("There are no parts in the grid to change.");
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
            operation: bulkCostAmount === null ? "remove" : "update"
        });

        dialog
            .find(".dialog-dont-save,.dialog-save")
            .click(function () {
                if ($(this).hasClass("dialog-dont-save")) {

                } else {
                    loopForAllData(bulkCostAmount);
                }

                dialog.data("kendoWindow").close();
            })
            .end();
    }

    function loopForAllData(amount) {
        var grid = $("#parts").data("kendoGrid");
        let dataSource = grid.dataSource
        let origdataLength = dataSource.data().length < 10 ? 10 : dataSource.data().length;
        let totalLength = dataSource.total()

        if (totalLength > origdataLength) {
            dataSource.pageSize(totalLength);
            dataSource.read().then(function () {
                saveCostForAllDataItems(amount, dataSource)
                dataSource.pageSize(origdataLength);
            })
        }
        else {
            saveCostForAllDataItems(amount, dataSource)
        }
    }

    function saveCostForAllDataItems(amount, dataSource) {
        var data = dataSource.data();
        //if (data.length > 1000) {
        //    warn?
        //    alert('Bulk change currently capped at 1000, in case of error. Requested change was for ' + data.length + '. This operation was cancelled.')
        //    return
        //}
         for (var i = 0; i < data.length; i++) {
             data[i].Cost += data[i].Cost * amount;
         }

         $.ajax({ url: '@Url.Action("BulkEditCost", "PartTemplate")',
                    type: 'POST',
                    cache: false,
                    traditional: true,
                    dataType: "json",
                    contentType: "application/json",
                    data: kendo.stringify({
                                models: data.toJSON()
                            }),
                    success: function(result) {
                        if(!result.Success) {
                            displayAjaxModelError(result.Errors);
                        } else {
                            dataSource.read();
                        }
                    },
                    error: function(data) {  }
                });
    }
    function displayAjaxModelError(modelState) {
        var errorString = "";
        for (var property in modelState) {
            for (var i = 0; i < modelState[property].length; i++) {
                errorString += property + ": " + modelState[property][i];
            }
        }

        alert(errorString);
    };
             //# sourceURL=indexEmbeddedScript2.js
</script>

<script id="BulkOperationDialog" type="text/x-kendo-template">
    <p class="dialog-title">Do you really want to <span data-bind="{text: operation}" /> the cost for all <span data-bind="{text: totalRows}" /> parts that match the current filter?</p>

    <button class="dialog-save k-button">Yes</button>
    <button class="dialog-dont-save k-button">No</button>
</script>

<p style="float:left; display:contents">
    <label for="BulkCost">Set percentage cost change for all matching items:</label>
    @(Html.Kendo().NumericTextBox(Of Decimal).Name("BulkCost") _
                                                            .Format("p0") _
                                                            .Min(-1) _
                                                            .Max(3) _
                                                            .Step(0.01) _
                                                             .Value(0.01))
    <a class="k-button" id="btnSaveBulkCost">Save</a>
</p>

@Html.ActionKendoButton("Edit Categories", "Index", "PartCategory", Nothing)

@(Html.Kendo().Grid(Of PartTemplateGridRowModel)() _
                                .Name("parts") _
                                .Events(Sub(events)
                                            events.DataBound("parts_DataBound")
                                            events.ExcelExport("parts_ExcelExport")
                                            events.Edit("parts_Edit")
                                            events.Save("parts_Save")
                                        End Sub) _
                                .Excel(Sub(excel) excel.FileName("Price Book.xlsx") _
                                                        .Filterable(True) _
                                                        .AllPages(True) _
                                                        .ProxyURL(Url.Action("ExcelExport", "PartTemplate"))
                                        ) _
                                .Filterable() _
                                .Sortable() _
                                .Pageable() _
                                .Editable() _
                                .ToolBar(Sub(toolbar)
                                             toolbar.Create().Text("Create A New Part")
                                             toolbar.Custom().Text("Export to Excel").HtmlAttributes(New With {.id = "btnExportToExcel"})
                                         End Sub) _
                                .Columns(Sub(c)
                                             c.Bound(Function(x) x.Number).Title("Part#").Width(40)
                                             c.Bound(Function(x) x.Description)
                                             c.Bound(Function(x) x.SoldByOptionID).ClientTemplate("#=SoldByOption#").Title("Sold By")
                                             c.Bound(Function(x) x.Category)
                                             c.Bound(Function(x) x.Active).ClientTemplate("# if(Active) { # Yes # } else { # No # } #")
                                             c.Bound(Function(x) x.Taxable).ClientTemplate("# if(Taxable) { # Yes # } else { # No # } #")
                                             c.Bound(Function(x) x.Manufacturer).Title("Manuf.")
                                             c.Bound(Function(x) x.ManufacturerPartNumber).Title("Manufacturer Part#").Width(120)

                                             If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
                                                 c.Bound(Function(x) x.Cost)
                                                 c.Bound(Function(x) x.Markup)
                                             End If

                                             c.Bound(Function(x) x.ListPrice)
                                             c.Bound(Function(x) x.Discount)
                                             c.Bound(Function(x) x.ResalePrice).Width(85)
                                             c.Bound(Function(x) x.PriceLastUpdated).Format("{0:d}").Width(95)
                                             c.Bound(Function(x) x.IsAssembly).Title("").Filterable(False).ClientTemplate(
                                     "# if(!IsAssembly) { #" &
                                     "<a class=""k-button convertToAssembly"">Convert To <br> Assembly</a>" &
                                     "# } #")
                                             c.Command(Sub(command)
                                                           command.Custom("Details").Click("parts_DetailsClick")
                                                           command.Edit()
                                                       End Sub)
                                             c.ContainsCustomColumns()
                                         End Sub) _
                                             .DataSource(Sub(dataSource)
                                                             dataSource _
                                                 .Ajax() _
                                                 .Model(Sub(model)
                                                            model.Id(Function(id) id.PartTemplateID)
                                                            model.Field(Function(f) f.Active).DefaultValue(True)
                                                            model.Field(Function(f) f.Taxable).DefaultValue(True)
                                                            model.Field(Function(f) f.IsAssembly).DefaultValue(False).Editable(False)
                                                            model.Field("ResalePrice", GetType(Decimal))
                                                            model.Field("ListPrice", GetType(Decimal))
                                                            model.Field(Function(f) f.PriceLastUpdated).Editable(False)
                                                        End Sub) _
                                                 .Read("List", "PartTemplate") _
                                                 .Update("JsonEdit", "PartTemplate") _
                                                 .Create("Create", "PartTemplate")
                                                         End Sub)
    )