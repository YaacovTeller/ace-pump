@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.PartTemplateModel
@Imports AcePump.Common

@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>
<script type="text/javascript">
    function ConvertToAssemblyClick() {
        $.ajax({
            dataType: "json",
            url: "@Url.Action("ConvertToAssembly", "PartTemplate")",
            data: {id: @Model.PartTemplateID},
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

    $(document).ready(function () {
        @Code
            Dim actionString As String = If(Model.AssemblyID.HasValue,
                                            "$(""#btnConvertToAssembly"").remove();",
                                            "$(""#btnConvertToAssembly"").click(ConvertToAssemblyClick);")
        End Code
        @(New MvcHtmlString(actionString))
    });

    function displayAjaxModelError(modelState){
        var errorString = "";
        for(var property in modelState) {
            for(var i=0; i < modelState[property].length; i++) {
                errorString += property + ": " + modelState[property][i];
            }
        }

        alert(errorString);
    };
</script>


<fieldset>
    <legend>Part @Model.Number</legend>

    <ol>

        <li>
            <div class="display-label">Description</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Description)
            </div>
        </li>

        <li>
            <div class="display-label">Category</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Category)
            </div>
        </li>

        @If Model.AssemblyID.HasValue AndAlso Not String.IsNullOrEmpty(Model.AssemblyPartNumber) Then
        @<li>
            <div class="display-label">Part of Assembly</div>
            <div class="display-field">
                @Html.ActionKendoButton(Model.AssemblyPartNumber, "Details", "Assembly", New With {.id = Model.AssemblyID.Value})
            </div>
        </li>
        End If

        <li>
            <div class="display-label">Manufacturer</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Manufacturer)
            </div>
        </li>

        <li>
            <div class="display-label">Manufacturer Part Number</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.ManufacturerPartNumber)
            </div>
        </li>

        <li>
            <div class="display-label">Material</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Material)
            </div>
        </li>

        <li>
            <div class="display-label">Sold By</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.SoldByOption)
            </div>
        </li>

        <li>
            <div class="display-label">Active</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Active)
            </div>
        </li>

        <li>
            <div class="display-label">Taxable</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Taxable)
            </div>
        </li>

        @If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
        @<li>
            <div class="display-label">Cost</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Cost)
            </div>
        </li>

        @<li>
            <div class="display-label">Markup</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Markup)
            </div>
        </li>
        End If

        <li>
            <div class="display-label">List Price</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.ListPrice)
            </div>
        </li>

        <li>
            <div class="display-label">Discount</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Discount)
            </div>
        </li>

        <li>
            <div class="display-label">Resale Price</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.ResalePrice)
            </div>
        </li>
        <li>
            <div class="display-label">Price Last Updated</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.PriceLastUpdated)
            </div>
        </li>
    </ol>
</fieldset>
<p>
    @Html.ActionKendoButton("Back to List", "Index")
    <a class="k-button" href="#" id="btnConvertToAssembly">Convert To Assembly</a>
</p>
