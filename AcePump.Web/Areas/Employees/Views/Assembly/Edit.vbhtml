@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.AssemblyModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
@Imports AcePump.Common

@Code
    ViewData("Title") = "Edit"
End Code

<h2>Edit</h2>

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

<script type="text/javascript">
    function swapRows(rowA, rowB) {
        if(rowA.length == 0 || rowB.length == 0) return;

        var grid = $("#parts").data("kendoGrid");
        var uidA = rowA.data("uid"); var uidB = rowB.data("uid");
        var defA = grid.dataSource.getByUid(uidA); var defB = grid.dataSource.getByUid(uidB);
        
        $.ajax({
            dataType: "json",
            url: "@Url.Action("SwapParts", "Assembly")",
            data: {firstPartDefId: defA.AssemblyPartDefID,
                   secondPartDefId: defB.AssemblyPartDefID},
            type: "POST",
            success: function() {
                var tmpOrder = defA.SortOrder;
                defA.SortOrder = defB.SortOrder;
                defB.SortOrder = tmpOrder;

                grid.dataSource.sort({ field: "SortOrder", dir: "asc" });

            }
        });
    }

    function moveUp_Click(e) {
        var currentRow = $(e.currentTarget).parents("tr");
        var rowAbove = currentRow.prev();

        swapRows(currentRow, rowAbove);

        e.preventDefault();
    }

    function moveDown_Click(e) {
        var currentRow = $(e.currentTarget).parents("tr");
        var rowBelow = currentRow.next();

        swapRows(currentRow, rowBelow);

        e.preventDefault();
    }

    function partsAdd_AdditionalData(e) {
        return { 
            assemblyId: @Model.AssemblyID,
            partDefIDTarget: $("#partDefIDTarget").val()
        };
    }

    $(document).ready(function () {
        $("#btnAddAtPosition").click(btnAddAtPosition_Click);
    });

    function btnAddAtPosition_Click(e) {
        var hasChanges = $("#parts").data("kendoGrid").dataSource.hasChanges();

        if (!hasChanges) {
            var lineNumber = $("#addPartLineNumber").val();
            $("#partDefIDTarget").val(0);
            $("#addPartLineNumber").data("kendoNumericTextBox").value("");

            var totalLines = $("#parts").data("kendoGrid").dataSource.total();

            if (lineNumber === "" || lineNumber === "0") {
                lineNumber = 0;
            } else if (lineNumber > totalLines) {
                lineNumber = totalLines;
            } 
        
            $("#partDefIDTarget").val(getSortOrderIdByLineNumber(lineNumber));        
        
            addPart(lineNumber);
        }
        e.preventDefault();
    }
    
    function getSortOrderIdByLineNumber(lineNumber) {
        if ( lineNumber === 0 ) {
            return 0;
        } else if ( lineNumber > 0 ) {
            var grid = $("#parts").data("kendoGrid");
            var row = grid.table.find("tr:nth(" + (lineNumber) + ")");
            var dataItem = grid.dataItem(row);

            if(dataItem === null) return null;

            return dataItem.AssemblyPartDefID;
        } else {
            return null;
        }
    }

    function addPart(lineNumber) {
        var grid = $("#parts").data("kendoGrid");
        var insertIndex;
        var editRowLineNumber;
        var totalLines= grid.dataSource.total()

        if (lineNumber >= totalLines) {
            insertIndex = getDataSourceIndexOfLineNumber(totalLines);
            insertIndex += 1
            editRowLineNumber = totalLines + 1;
        } else {
            var nextLineNumber= parseInt(lineNumber) + 1;
            insertIndex = getDataSourceIndexOfLineNumber( nextLineNumber );
            editRowLineNumber = nextLineNumber;
        }

        grid.dataSource.insert(insertIndex, { });
        grid.editRow($("#parts tr:eq(" + (editRowLineNumber) + ")"));
    }

    function getDataSourceIndexOfLineNumber(lineNumber) {
        var grid = $("#parts").data("kendoGrid");
        var data = grid.dataItem("tr:eq(" + (lineNumber) + ")");
        return grid.dataSource.indexOf(data);
     }

    function parts_onDataBound(e) {
        var grid = this;
        grid.tbody.find('.line-number-input').each(function (index) {
            var box = $(this).kendoNumericTextBox({
                format: "0.",
                decimals: "0",
                step: "1",
                min: "0",
                spinners: false,
                width: "50px",
                change: lineNumberChange              
            });
            box.data("kendoNumericTextBox").value(index + 1);
        })
    }

    function parts_onEdit(e) {
        row = e.container;
        row.find(".line-number-input").prop('disabled', true);
    }

    function parts_onCancel(e) {
        var grid = $("#parts").data("kendoGrid");
        grid.cancelRow();
        grid.refresh();
    }

    function lineNumberChange(e) {
        var newLineNumber = this.value();
        var currentDataItem = $('#parts').data('kendoGrid').dataItem($(this.element).closest('tr'));
        var partDefIDToChange = currentDataItem.AssemblyPartDefID;

        var partDefIDTarget = getSortOrderIdByLineNumber(newLineNumber);

        if (partDefIDToChange !== partDefIDTarget && partDefIDTarget !== null) {
            updatePartPosition(partDefIDToChange, partDefIDTarget);
        } else {
            e.preventDefault();
        }
    }

    function updatePartPosition(partDefIDToChange, partDefIDTarget) {
        if(partDefIDToChange.length == 0 || partDefIDTarget.length == 0) return;

        $.ajax({
            dataType: "json",
            url: "@Url.Action("UpdatePartPosition", "Assembly")",
            data: {partDefIDToChange: partDefIDToChange,
                   partDefIDTarget: partDefIDTarget},
            type: "POST",
            success: function() {
                if (arguments[0].Success === true ) {
                    var grid = $('#parts').data('kendoGrid');
                    grid.dataSource.read();
                }
            }
        });
     }

    function parts_Error(e) {
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
<style type="text/css">
    .line-number-input
    {
        width: 50px;
    }
</style>

<fieldset>
    <legend>Assembly</legend>
    @Using Html.BeginForm()
        @Html.ValidationSummary(True)
        @<ol>
        
        @Html.Hidden("partDefIDTarget")

        @Html.HiddenFor(Function(model) model.AssemblyID)

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Category)
            </div>
            <div class="editor-field">
                @Html.EditorFor(Function(model) model.Category)
                @Html.ValidationMessageFor(Function(model) model.Category)
            </div>
        </li>
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.AssemblyNumber)
            </div>
            <div class="editor-field">
                @Html.EditorFor(Function(model) model.AssemblyNumber)
                @Html.ValidationMessageFor(Function(model) model.AssemblyNumber)
            </div>
        </li>
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Description)
            </div>
            <div class="editor-field">
                @Html.EditorFor(Function(model) model.Description)
                @Html.ValidationMessageFor(Function(model) model.Description)
            </div>
        </li>

        @If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
            @<li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.TotalPartsCost)
                </div>
                <div class="editor-field">
                    @Html.DisplayFor(Function(model) model.TotalPartsCost)
                    @Html.ValidationMessageFor(Function(model) model.TotalPartsCost)
                </div>
            </li>
            
            @<li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.Markup)
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.Markup)
                    @Html.ValidationMessageFor(Function(model) model.Markup)
                </div>
            </li>
        End If

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Discount)
            </div>
            <div class="editor-field">
                @Html.EditorFor(Function(model) model.Discount)
                @Html.ValidationMessageFor(Function(model) model.Discount)
            </div>
        </li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.ResalePrice)
                @(Html.Kendo().Tooltip() _
                    .For("label[for=ResalePrice]").Content("Displays the resale price of the assembly it will appear in the parts table when the assembly is added.")
                )
            </div>
            <div class="editor-field">
                @Html.DisplayFor(Function(model) model.ResalePrice)
                @Html.ValidationMessageFor(Function(model) model.ResalePrice)
            </div>
        </li>
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.TotalPartsResalePrice)
                @(Html.Kendo().Tooltip() _
                    .For("label[for=TotalPartsResalePrice]").Content("Displays the sum of the resale price of all the individual parts in the assembly.")
                )
            </div>
            <div class="editor-field">
                @Html.DisplayFor(Function(model) model.TotalPartsResalePrice)
                @Html.ValidationMessageFor(Function(model) model.TotalPartsResalePrice)
            </div>
        </li>
    </ol>
    @<p>
        <input type="submit" value="Save" />
    </p>
    End Using

    <div style="clear: both; padding-top: 40px">
        <h2>Parts</h2>
        
        @(Html.Kendo().Grid(Of AssemblyPartListRowViewModel) _
            .Name("parts") _
            .Editable(Function(e) e.Mode(GridEditMode.InLine)) _
            .Events(Sub(e) e.DataBound("parts_onDataBound") _
                            .Cancel("parts_onCancel") _
                            .Edit("parts_onEdit")) _
            .ToolBar(Sub(toolbar)
                         toolbar.Template(@@<text>
                                           <div class="toolbar">
                                                    <label class="category-label" for="category">Add Part to assembly</label>
                                                    <label >at line number: </label>
                                                    <input id="addPartLineNumber" />
                                                    <script>
                                                        $("#addPartLineNumber").kendoNumericTextBox({
                                                            format: "0.",
                                                            decimals: 0,
                                                            step: 1,
                                                            min: 0,
                                                            placeholder: "first"
                                                        });
                                                    </script>
                                                    <a class="k-button k-button-icontext s-add-at-position" href="#" id="btnAddAtPosition">
                                                        <span class="k-icon k-add"></span>
                                                        Add
                                                    </a>                                                     
                                           </div>
                                        </text>)
                     End Sub) _
            .DataSource(Sub(config)
                            config _
                            .Ajax() _
                            .Events(Sub(events) events.Error("parts_Error")) _
                            .Sort(Sub(sort)
                                      sort.Add(Function(x) x.SortOrder)
                                  End Sub) _
                            .Model(Sub(model)
                                       model.Id(Function(x) x.AssemblyPartDefID)
                                       model.Field(Function(x) x.PartTemplateNumber)
                                       model.Field(Function(x) x.Description).Editable(False)
                                       model.Field(Function(x) x.Cost).Editable(False)
                                       model.Field(Function(x) x.ResaleValue).Editable(False)
                                       model.Field(Function(x) x.TotalResaleValue).Editable(False)
                                   End Sub) _
                            .Read("PartList", "Assembly", New With {.id = Model.AssemblyID}) _
                            .Destroy("RemovePart", "Assembly") _
                            .Create(Sub(create)
                                        create.Action("AddPart", "Assembly")
                                        create.Data("partsAdd_AdditionalData")
                                    End Sub) _
                            .Update("UpdatePart", "Assembly")
                        End Sub) _
            .Columns(Sub(config)
        config.Template(@@<text></text>).ClientTemplate("<input class=""line-number-input""/>").Title("Line #").Width(30)
                         config.Bound(Function(x) x.PartsQuantity).Width(20).Title("Quantity")
                         config.Bound(Function(x) x.PartTemplateID).ClientTemplate("#=PartTemplateNumber#").Title("Part Number")
                         config.Bound(Function(x) x.Description).Title("Part Description")

                         If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
                             config.Bound(Function(x) x.Cost).Format("{0:C}")
                         End If

                         config.Bound(Function(x) x.ResaleValue).Title("Resale").Format("{0:C}")
                         config.Bound(Function(x) x.TotalResaleValue).Title("Line Total").Format("{0:C}")
                         config.Command(Sub(command)
                                            command.Custom("moveUp").Text("/\\").Click("moveUp_Click")
                                            command.Custom("moveDown").Text("\\/").Click("moveDown_Click")
                                            command.Destroy().Text("delete")
                                            command.Edit()
                                        End Sub)
                     End Sub)
        )
    </div>
</fieldset>

<div>
    @Html.ActionKendoButton("Back to List", "Index")
</div>
