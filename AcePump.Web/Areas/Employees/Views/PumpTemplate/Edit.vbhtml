@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.PumpTemplateViewModel
@Imports AcePump.Common
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Edit"
End Code

<h2>Edits</h2>

<script type="text/javascript">
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
    
    function swapRows(rowA, rowB) {
        if(rowA.length == 0 || rowB.length == 0) return;

        var grid = $("#parts").data("kendoGrid");
        var uidA = rowA.data("uid"); var uidB = rowB.data("uid");
        var defA = grid.dataSource.getByUid(uidA); var defB = grid.dataSource.getByUid(uidB);
        
        $.ajax({
            dataType: "json",
            url: "@Url.Action("SwapParts", "PumpTemplate")",
            data: {firstPartDefId: defA.TemplatePartDefID,
                   secondPartDefId: defB.TemplatePartDefID},
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
  
    function ddlAddPart_AdditionalData(e) {
        return { term: $("#ddlAddPart").val() };
    }

    function partsAdd_AdditionalData(e) {
        return { 
            pumpTemplateId: @Model.PumpTemplateID,
            partDefIDTarget: $("#partDefIDTarget").val()
        };
    }

    function templateNameEditButton_Click(e) {
        $("#templateNumberEditor").slideDown();
    }
</script>

<script type="text/javascript">
    var helper = (function ($, undefined) {
        function FormattedLabel(id, formatString) {
            this._Id = id;
            this._JqObj = null;

            this.FormatString = formatString || "{0}";
        }

        FormattedLabel.prototype._GetJqObj = function () {
            if (this._JqObj === null) this._JqObj = $("#" + this._Id);
            return this._JqObj;
        };

        FormattedLabel.prototype.SetText = function (value) {
            var formatted = kendo.format(this.FormatString, value);
            this._GetJqObj().text(formatted);
        };

        FormattedLabel.prototype.GetText = function () {
            return this._GetJqObj().text();
        };

        function HelperClass() {
            this._TotalPartCost = null;
            this._TotalPartResale = null;
            this._ListPrice = null;
            this._ResalePrice = null;

            this._PartsGrid = null;

            this._DiscountRate = null;
            this._MarkupRate = null;
        }

        HelperClass.prototype.Initialize = function () {
            this._TotalPartCost = new FormattedLabel("@Html.IdFor(Function(x) x.TotalPartCost)", "{0:c}");
            this._TotalPartResale = new FormattedLabel("@Html.IdFor(Function(x) x.TotalPartResale)", "{0:c}");
            this._ListPrice = new FormattedLabel("@Html.IdFor(Function(x) x.ListPrice)", "{0:c}");
            this._ResalePrice = new FormattedLabel("@Html.IdFor(Function(x) x.ResalePrice)", "{0:c}");

            this._PartsGrid = $("#parts").data("kendoGrid");

            this._DiscountRate = $("#@Html.IdFor(Function(x) x.DiscountRate)");
            this._MarkupRate = $("#@Html.IdFor(Function(x) x.MarkupRate)");

            this._AttachHandlers();
        };

        HelperClass.prototype._AttachHandlers = function () {
            var helper = this;
            var handler = function () { helper.UpdatePrices(); }

            this._DiscountRate.on("change", handler);
            this._MarkupRate.on("change", handler);
            this._PartsGrid.dataSource.bind("change", handler);
        };

        HelperClass.prototype.UpdatePrices = function () {
            var partsPriceInfo = this._GetPartsPriceInfo();

            var markup = parseFloat(this._MarkupRate.val());
            var resale = partsPriceInfo.Cost / (1 - markup);

            var discount = parseFloat(this._DiscountRate.val());
            var listPrice = resale / (1 - discount);

            this._TotalPartCost.SetText(partsPriceInfo.Cost);
            this._TotalPartResale.SetText(partsPriceInfo.Resale);
            this._ResalePrice.SetText(resale);
            this._ListPrice.SetText(listPrice);
        };

        HelperClass.prototype._GetPartsPriceInfo = function () {
            var data = this._PartsGrid.dataSource.data();

            var info = { Cost: 0.0, Resale: 0.0 };
            for (var i = 0; i < data.length; i++) {
                var item = data[i];

                info.Cost += (item.Cost * item.Quantity);
                info.Resale += (item.ResaleValue * item.Quantity);
            }

            return info;
        };

        return new HelperClass();
    })(jQuery);

    $(window).load(function () {
        helper.Initialize();
    });

    $(document).ready(function () {
        $("#btnAddAtPosition").click(btnAddAtPosition_Click);
    });

    function btnAddAtPosition_Click(e) {
        var hasChanges = $("#parts").data("kendoGrid").dataSource.hasChanges();

        if(!hasChanges) {
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
    // Insert at Position
    $(document).ready(function () {
        $("#btnAddAtPosition").click(btnAddAtPosition_Click);

        $("#btnAddAtEndOfList").click(function(e) {
            $("#partDefIDTarget").val(0);
            var lineNumber = $("#addPartLineNumber").val();
            var totalLines = $("#parts").data("kendoGrid").dataSource.total();
            lineNumber = totalLines;

            $("#partDefIDTarget").val(getSortOrderIdByLineNumber(lineNumber));        
        
            addPart(lineNumber);

            e.preventDefault();
        });
    });

    function getSortOrderIdByLineNumber(lineNumber) {
        if ( lineNumber === 0 ) {
            return 0;
        } else if ( lineNumber > 0 ) {
            var grid = $("#parts").data("kendoGrid");
            var row = grid.table.find("tr:nth(" + (lineNumber) + ")");
            var dataItem = grid.dataItem(row);
            
            if(dataItem === null) return null;

            return dataItem.TemplatePartDefID;
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
        var partDefIDToChange = currentDataItem.TemplatePartDefID;

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
            url: "@Url.Action("UpdatePartPosition", "PumpTemplate")",
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

</script>

<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/PumpTemplate.css")" />

<style type="text/css">
    #pnlAddPart {
        margin-bottom: 10px;
    }
    
    #templateNumberEditor {
        display: none;
        margin-top: 20px;
    }
    
    .template-number input {
        width: 100%;
    }
    
    .line-number-input
    {
        width: 50px;
    }
</style>

@Using Html.BeginForm()
@<fieldset>
    <legend>Pump Template</legend>
       
    <div class="main-details">
        
        @Html.HiddenFor(Function(model) model.PumpTemplateID)

        <div class="display-label">
            Pump Template ID and Number 
            @(Html.Kendo.Button() _
                        .Name("templateNameEditButton") _
                        .Content("edit") _
                        .HtmlAttributes(New With {.type = "button"}) _
                        .Events(Sub(events) events.Click("templateNameEditButton_Click"))
            )
                        .
        </div>

        <div class="display-field">
            <div class="template-number">
                @Html.DisplayFor(Function(model) model.ConciseSpecificationSummary)
            </div>  
          
            <div class="template-number">
                @Html.EditorFor(Function(model) model.VerboseSpecificationSummary)
            </div>          

            <div class="template-id">
                @Html.DisplayFor(Function(model) model.PumpTemplateID)
            </div>
        </div>                 
    </div>

    <div style="clear:both;"></div>

    @If Not User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
        @Html.HiddenFor(Function(model) model.MarkupRate)        
        End If

    <ol style="margin-top: 10px">

    @If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
        @<li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.TotalPartCost, "Total Part Cost")
            </div>
            <div class="display-field" id="@Html.IdFor(Function(x) x.TotalPartCost)">
                @Html.DisplayFor(Function(model) model.TotalPartCost)
            </div>
	    </li>

        @<li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.MarkupRate, "Markup Rate")
            </div>
            <div class="display-field">
                @(Html.Kendo().NumericTextBoxFor(Function(model) model.MarkupRate) _
                    .Format("{0:p}") _
                    .Decimals(2) _
                    .Step(0.01) _
                    .Min(0) _
                    .Max(0.999)
                )
            </div>
	    </li>
    End If

    <li>
        <div class="display-label">
            @Html.LabelFor(Function(model) model.TotalPartResale, "Total Part Resale")
        </div>
        <div class="display-field" id="@Html.IdFor(Function(x) x.TotalPartResale)">
            @Html.DisplayFor(Function(model) model.TotalPartResale)
        </div>
	</li>

    <li>
        <div class="display-label">
            @Html.LabelFor(Function(model) model.ListPrice, "List Price")
        </div>
        <div class="display-field" id="@Html.IdFor(Function(x) x.ListPrice)">
            @Html.DisplayFor(Function(model) model.ListPrice)
        </div>
	</li>

    <li>
        <div class="display-label">
            @Html.LabelFor(Function(model) model.DiscountRate, "Discount Rate")
        </div>
        <div class="display-field">
            @(Html.Kendo().NumericTextBoxFor(Function(model) model.DiscountRate) _
                    .Format("{0:p}") _
                    .Decimals(2) _
                    .Step(0.01) _
                    .Min(0) _
                    .Max(0.999)
                )
        </div>
	</li>

    <li>
        <div class="display-label">
            @Html.LabelFor(Function(model) model.ResalePrice, "Resale Price")
        </div>
        <div class="display-field" id="@Html.IdFor(Function(x) x.ResalePrice)">
            @Html.DisplayFor(Function(model) model.ResalePrice)
        </div>
	</li>
    
        </ol>

    <div style="clear:both;"></div>

    @Html.ValidationSummary()

    <div id="templateNumberEditor">
	    <ol>
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.TubingSize, "Tubing Size")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.TubingSize, "TubingSize")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.PumpBoreBasic, "Pump Bore Basic")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.PumpBoreBasic, "PumpBoreBasic")
            </div>
	    </li>

        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Barrel.Length, "Barrel Length")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Barrel.Length, "BarrelLength")
            </div>
	    </li>

        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.LowerExtension, "Lower Extension (ft)")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.LowerExtension, "LowerExtension")
            </div>
	    </li>

        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.UpperExtension, "Upper Extension (ft)")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.UpperExtension, "UpperExtension")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.PumpType, "Pump Type")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.PumpType, "PumpType")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Barrel.Type, "Barrel Type")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Barrel.Type, "BarrelType")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Barrel.Material, "Barrel Material")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Barrel.Material, "BarrelMaterial")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Seating.Location, "Seating Location")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Seating.Location, "SeatingLocation")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Seating.Type, "Seating Type")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Seating.Type, "SeatingType")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Plunger.Material, "Plunger Material")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Plunger.Material, "PlungerMaterial")
            </div>
	    </li>

        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Plunger.Length, "Plunger Length")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Plunger.Length, "PlungerLength")
            </div>
	    </li>

        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Plunger.Fit, "Plunger Fit")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Plunger.Fit, "PlungerFit")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.HoldDownType, "Hold Down Type")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.HoldDownType, "HoldDownType")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.TravellingCages, "Travelling Cages")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.TravellingCages, "TravellingCages")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.StandingValveCages, "Standing Valve Cages")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.StandingValveCages, "StandingValveCages")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.StandingValve, "Standing Valve")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.StandingValve, "StandingValve")
            </div>
	    </li>

        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.BallsAndSeats, "Balls And Seats")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.BallsAndSeats, "BallsAndSeats")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Barrel.Washer, "Barrel Washer")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Barrel.Washer, "BarrelWasher")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Collet)
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Collet, "Collet")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.TopSeals, "Top Seals")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.TopSeals, "TopSeals")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.OnOffTool, "On/Off Tool")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.OnOffTool, "OnOffTool")
            </div>
	    </li>
        
        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.SpecialtyItems, "Specialty Items")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.SpecialtyItems, "SpecialtyItems")
            </div>
	    </li>

        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.PonyRods, "Pony Rods")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.PonyRods, "PonyRods")
            </div>
	    </li>

        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.Strainers)
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.Strainers, "Strainers")
            </div>
	    </li>

        <li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.KnockOut, "Knock Out")
            </div>
            <div class="display-field">
                @Html.AcePump_TypeManagerDropDownFor(Function(model) model.KnockOut, "KnockOut")
            </div>
	    </li>
        </ol>
        
    </div>
    <p>
        <input type="submit" value="Update Spec Summaries and Pricing" />
    </p>

    <div style="clear: both; padding-top: 40px">
        <h2>Parts</h2>

        @Html.Hidden("partDefIDTarget")

        @(Html.Kendo().Grid(Of TemplatePartListRowViewModel) _
            .Name("parts") _
            .Editable(Function(e) e.Mode(GridEditMode.InLine)) _
            .Events(Sub(e) e.DataBound("parts_onDataBound") _
                            .Edit("parts_onEdit") _
                            .Cancel("parts_onCancel")) _
            .ToolBar(Sub(toolbar)
                         toolbar.Template(@@<text>
                                              <div class="toolbar">
                                                  <label class="category-label" for="category">Add Part to template</label>
                                                  <label>at line number: </label>
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
                                                  <a class="k-button k-button-icontext s-add-at-position" href="#" id="btnAddAtEndOfList">
                                                      <span class="k-icon k-add"></span>Add to End of List
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
                                       model.Id(Function(x) x.TemplatePartDefID)
                                       model.Field(Function(x) x.TemplatePartDefID).Editable(False)
                                       model.Field(Function(x) x.PartTemplateNumber)
                                       model.Field(Function(x) x.SortOrder).Editable(False)
                                       model.Field(Function(x) x.Description).Editable(False)
                                       model.Field(Function(x) x.Cost).Editable(False)
                                       model.Field(Function(x) x.ResaleValue).Editable(False)
                                       model.Field(Function(x) x.TotalResaleValue).Editable(False)
                                   End Sub) _
                            .Read("PartList", "PumpTemplate", New With {.id = Model.PumpTemplateID}) _
                            .Destroy("RemovePart", "PumpTemplate") _
                            .Create(Sub(create)
                                        create.Action("AddPart", "PumpTemplate")
                                        create.Data("partsAdd_AdditionalData")
                                    End Sub) _
                            .Update("UpdatePart", "PumpTemplate")
                        End Sub) _
            .Columns(Sub(config)
        config.Template(@@<text></text>).ClientTemplate("<input class=""line-number-input""/>").Title("Line #").Width(30)
                         config.Bound(Function(x) x.Quantity).Width(20)
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
                                            command.Destroy().Text("Delete")
                                            command.Edit()
                                        End Sub)
            End Sub)
        )
    </div>
</fieldset>
End Using

<div>
    @Html.ActionKendoButton("Back to List", "Index")
</div>
