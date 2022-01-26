@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.AssemblyModel
@Imports AcePump.Common

@Code
    ViewData("Title") = "Create"
End Code

<h2>Create</h2>

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

@Using Html.BeginForm()
    @Html.ValidationSummary(True)
    @<fieldset>
    <legend>Assembly</legend>

    <ol>
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Description)
            </div>
            <div class="editor-field">
                @Html.EditorFor(Function(model) model.Description)
                @Html.ValidationMessageFor(Function(model) model.Description)
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
                @Html.LabelFor(Function(model) model.Category)
            </div>
            <div class="editor-field">
                @Html.EditorFor(Function(model) model.Category)
                @Html.ValidationMessageFor(Function(model) model.Category)
            </div>
        </li>

        @If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
            @<li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.TotalPartsCost)
                </div>
                <div class="editor-field">
                    Calculated automatically when you click save.
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
            </div>
            <div class="editor-field">
                Calculated automatically when you click save.
            </div>
        </li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.TotalPartsResalePrice)
            </div>
            <div class="editor-field">
                Calculated automatically when you click save.
            </div>
        </li>
    </ol>

    <p>
        <input type="submit" value="Save and Add Parts" />
    </p>
</fieldset>
End Using

<div>
    @Html.ActionKendoButton("Back to List", "Index")
</div>
