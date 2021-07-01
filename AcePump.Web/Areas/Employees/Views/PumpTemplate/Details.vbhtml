@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.PumpTemplateViewModel
@Imports AcePump.Common
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Details"
End Code

@Section PreloadLibs
    <script src="@Url.Content("~/Scripts/kendo/jszip.min.js")" type="text/javascript"></script>
End Section

<h2>Details</h2>

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
</script>

<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/PumpTemplate.css")" />

<fieldset>
    <legend>Pump Template</legend>


    <div class="main-details">
        <div class="display-label">
            Pump Template ID and Number
        </div>

        <div class="display-field">
            <div class="template-number">
                @Html.DisplayFor(Function(model) model.ConciseSpecificationSummary)
            </div>  
          
            <div class="template-number">
                @Html.DisplayFor(Function(model) model.VerboseSpecificationSummary)
            </div>          

            <div class="template-id">
                @Html.DisplayFor(Function(model) model.PumpTemplateID)
            </div>
        </div>

        @Using Html.BeginForm("Duplicate", "PumpTemplate", New With {.id = Model.PumpTemplateID}, FormMethod.Post)
            @<input type="submit" value="Duplicate This Template" />
            @Html.ActionKendoButton("Print Template", "Pdf", New With {.id = Model.PumpTemplateID})
        End Using
    </div>

    <ol>

    @If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
        @<li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.TotalPartCost, "Total Part Cost")
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.TotalPartCost, "TotalPartCost")
            </div>
	    </li>

        @<li>
            <div class="display-label">
                @Html.LabelFor(Function(model) model.MarkupRate, "Markup Rate")
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.MarkupRate, "MarkupRate")
            </div>
	    </li>
    End If

    <li>
        <div class="display-label">
            @Html.LabelFor(Function(model) model.TotalPartResale, "Total Part Resale")
        </div>
        <div class="display-field">
            @Html.DisplayFor(Function(model) model.TotalPartResale, "TotalPartResale")
        </div>
	</li>

    <li>
        <div class="display-label">
            @Html.LabelFor(Function(model) model.ListPrice, "List Price")
        </div>
        <div class="display-field">
            @Html.DisplayFor(Function(model) model.ListPrice, "ListPrice")
        </div>
	</li>

    <li>
        <div class="display-label">
            @Html.LabelFor(Function(model) model.DiscountRate, "Discount Rate")
        </div>
        <div class="display-field">
            @Html.DisplayFor(Function(model) model.DiscountRate, "DiscountRate")
        </div>
	</li>

    <li>
        <div class="display-label">
            @Html.LabelFor(Function(model) model.ResalePrice, "Resale Price")
        </div>
        <div class="display-field">
            @Html.DisplayFor(Function(model) model.ResalePrice, "ResalePrice")
        </div>
	</li>
    
        </ol>

    <div style="clear: both; padding-top: 40px">
        <h2>Parts</h2>

        @(Html.Kendo().Grid(Of TemplatePartListRowViewModel) _
            .Name("parts") _
            .ToolBar(Sub(s) s.Excel()) _
            .Excel(Sub(excel) excel.FileName("Pump Template Export.xlsx") _
                                   .Filterable(False) _
                                   .ProxyURL(Url.Action("ExcelExport", "PumpTemplate"))
                  ) _
            .DataSource(Sub(config)
                            config _
                            .Ajax() _
                            .Events(Sub(events) events.Error("parts_Error")) _
                            .Sort(Sub(sort)
                                      sort.Add(Function(x) x.SortOrder)
                                  End Sub) _
                            .Model(Sub(model) model.Id(Function(x) x.TemplatePartDefID)) _
                            .Read("PartList", "PumpTemplate", New With {.id = Model.PumpTemplateID})
                        End Sub) _
            .Columns(Sub(config)
                         config.Bound(Function(x) x.Quantity)
                         config.Bound(Function(x) x.PartTemplateNumber).Title("Part Number")
                         config.Bound(Function(x) x.Description).Title("Part Description")

                         If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
                             config.Bound(Function(x) x.Cost)
                         End If

                         config.Bound(Function(x) x.ResaleValue).Title("Resale")
                         config.Bound(Function(x) x.TotalResaleValue).Title("Line Total")
                     End Sub)
        )
    </div>
</fieldset>

<div>
    @Html.ActionKendoButton("Edit", "Edit", New With {.id = Model.PumpTemplateID }) |
    @Html.ActionKendoButton("Back to List", "Index")
</div>

