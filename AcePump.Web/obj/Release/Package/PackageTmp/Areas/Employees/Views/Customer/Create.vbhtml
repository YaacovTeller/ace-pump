@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.CustomerViewModel

@Code
    ViewData("Title") = "Create"
End Code

<h2>Create</h2>

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

<script type="text/javascript">
    $.validator.setDefaults({
        ignore: ""
    });

    function CountySalesTaxRateID_AdditionalData() {
        return {
            term: $("#CountySalesTaxRateID").data("kendoComboBox").input.val()
        };
    }
</script>

@Using Html.BeginForm()
    @Html.ValidationSummary(True)
    @<fieldset>
        <legend>Customer</legend>

        <ol>

            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.CustomerName, "Customer Name")
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.CustomerName)
                    @Html.ValidationMessageFor(Function(model) model.CustomerName)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.Address1, "Address 1")
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.Address1)
                    @Html.ValidationMessageFor(Function(model) model.Address1)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.Address2, "Address 2")
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.Address2)
                    @Html.ValidationMessageFor(Function(model) model.Address2)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.City)
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.City)
                    @Html.ValidationMessageFor(Function(model) model.City)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.State)
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.State)
                    @Html.ValidationMessageFor(Function(model) model.State)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.Zip)
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.Zip)
                    @Html.ValidationMessageFor(Function(model) model.Zip)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.Phone)
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.Phone)
                    @Html.ValidationMessageFor(Function(model) model.Phone)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.Website)
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.Website)
                    @Html.ValidationMessageFor(Function(model) model.Website)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.APINumberRequired, "API Number Required")
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.APINumberRequired)
                    @Html.ValidationMessageFor(Function(model) model.APINumberRequired)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.CountySalesTaxRateID, "County Sales Tax Rate")
                </div>
                <div class="editor-field">
                    @(Html.Kendo().ComboBoxFor(Function(model) model.CountySalesTaxRateID) _
                         .DataTextField("CountyName") _
                         .DataValueField("Id") _
                         .MinLength(2) _
                         .Placeholder("Start typing ...") _
                         .Filter(FilterType.StartsWith) _
                         .AutoBind(False) _
                      .DataSource(Sub(dataSource)
                                      dataSource _
                                      .Read(Function(reader) reader.Action("StartsWith", "CountySalesTaxRate") _
                                                                     .Data("CountySalesTaxRateID_AdditionalData")
                                            ) _
                                      .ServerFiltering(True)
                                  End Sub)
                    )
                    @Html.ValidationMessageFor(Function(model) model.CountySalesTaxRateID)
                    <a href="@Url.Content("~/CountySalesTaxRate/Index")" class="k-button">View All County Sales Tax Rates</a>
                </div>
            </li>
            <li>
                <div class="editor-label" style="width:200px">
                    @Html.LabelFor(Function(model) model.UsesQuickbooksRunningInvoice, "Uses QuickBooks running invoice")
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.UsesQuickbooksRunningInvoice)
                    @Html.ValidationMessageFor(Function(model) model.UsesQuickbooksRunningInvoice)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.QbInvoiceClassID, "QuickBooks Class")
                </div>
                <div class="editor-field">
                    @(Html.Kendo().ComboBoxFor(Function(model) model.QbInvoiceClassID) _
                        .DataTextField("QbInvoiceClassName") _
                        .DataValueField("Id") _
                        .MinLength(2) _
                        .Placeholder("Start typing ...") _
                        .Filter(FilterType.StartsWith) _
                        .AutoBind(False) _
                     .DataSource(Sub(dataSource)
                                     dataSource _
                                     .Read(Function(reader) reader.Action("QbInvoiceClassList", "Customer") _
                                                                  .Type(HttpVerbs.Post)
                                           ) _
                                     .ServerFiltering(True)
                                 End Sub)
                    )
                    @Html.ValidationMessageFor(Function(model) model.QbInvoiceClassID)
                </div>
            </li>
            <li>
                <div class="editor-label">
                    @Html.LabelFor(Function(model) model.UsesInventory, "Uses Inventory")
                </div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.UsesInventory)
                    @Html.ValidationMessageFor(Function(model) model.UsesInventory)
                </div>
            </li>

            <li>
                @If User.IsInRole("AcePumpAdmin") Then
                    @<div Class="editor-label">
                        @Html.LabelFor(Function(model) model.PayUpFront, "Require Payment Up Front")
                    </div>
                    @<div Class="editor-field">
                        @Html.EditorFor(Function(model) model.PayUpFront)
                        @Html.ValidationMessageFor(Function(model) model.PayUpFront)
                    </div>
                End If
            </li>

        </ol>
        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
End Using

<div>
    @Html.ActionKendoButton("Back to List", "Index")
</div>
