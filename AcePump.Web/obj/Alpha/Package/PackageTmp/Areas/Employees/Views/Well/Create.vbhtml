@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.WellModel

@Code
    ViewData("Title") = "Create"
End Code

<h2>Create</h2>

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

<script type="text/javascript">
    function CustomerID_AdditionalData() {
        return {
            term: $("#CustomerID").data("kendoComboBox").input.val()
        };
    }

    var APINumberRequired;
    $(document).ready(function () {
        $("form").on("submit", function (event) {
            if (APINumberRequired === true && isBlank($("#APINumber").val())) {
                event.preventDefault();

                var customerName = $("#CustomerID").data("kendoComboBox").input.val();
                var dialogHtml = $("#APINumberDialog").html().trim();
                var contentContainer = $(dialogHtml);
                kendo.bind(contentContainer, {CustomerName: customerName});
                
                var save = false;
                var dialog = $("<div />").kendoWindow({
                    title: "API Number required.",
                    resizable: false,
                    modal: true
                });

                dialog.parent().find(".k-window-action").hide();

                dialog.data("kendoWindow")
                        .content(contentContainer)
                        .center().open();

                dialog
                        .find(".dialog-dont-save,.dialog-save-anyway")
                            .click(function () {
                                if ($(this).hasClass("dialog-dont-save")) {
                                    
                                } else {
                                    $("#IgnoreNoAPINumber").val(true);
                                    $("form").unbind("submit"); 
                                    $("form").submit();                                                                                                
                                }

                                dialog.data("kendoWindow").close();
                            })
                            .end()
            }   
            return save;          
        });
    });
  

     function isBlank(str) {
        return (!str || /^\s*$/.test(str));
    }

    function lookupAPINumberRequiredForCustomer(id) {    
        $.ajax({
            dataType: "json",
            url: "@Url.Action("LookupAPINumberRequired", "Customer")",
            data: {id: id},
            type: "POST",
            success: function(result) {
                if(result.Success === true) {
                    APINumberRequired = result.APINumberRequired;
                } else {
                    alert("Could not find out whether APINumber is required for this customer.");
                }
            }
        });
    }

    function Customer_Select(e) {
        lookupAPINumberRequiredForCustomer(this.value());
    }
 </script>

<script id="APINumberDialog" type="text/x-kendo-template">
    <p class="dialog-title">Wells for <span data-bind="text: CustomerName" /> require an API number. Do you want to save without an API Number anyway?</p>

    <button class="dialog-dont-save k-button">Don't save</button>
    <button class="dialog-save-anyway k-button">Save without API Number</button>
</script>

@Using Html.BeginForm()
    @Html.ValidationSummary(True)
    @<fieldset>
    <legend>Well Location</legend>

    @Html.HiddenFor(Function(x) x.IgnoreNoAPINumber)
    <div class="editor-label">
        Customer
    </div>
    <div class="editor-field">
        @(Html.Kendo().ComboBoxFor(Function(x) x.CustomerID) _
                        .DataTextField("Name") _
                        .DataValueField("Id") _
                        .MinLength(2) _
                        .Placeholder("Start typing a customer name...") _
                        .Filter(FilterType.StartsWith) _
                        .Events(Sub(e) e.Change("Customer_Select")) _
                        .AutoBind(False) _
                        .DataSource(Sub(dataSource)
                                        dataSource _
                                        .Read(Function(reader) reader.Action("StartsWith", "Customer") _
                                                                     .Data("CustomerID_AdditionalData") _
                                                                     ) _
                                        .ServerFiltering(True)
                                    End Sub)
                    )
    </div>
    <br />
    <div class="editor-label">
        @Html.LabelFor(Function(model) model.LeaseID, "Lease Name")
    </div>
    <div class="editor-field">
        @(Html.Kendo().ComboBoxFor(Function(x) x.LeaseID) _
                        .DataTextField("LeaseName") _
                        .DataValueField("LeaseId") _
                        .Filter(FilterType.StartsWith) _
                        .AutoBind(False) _
                        .Placeholder("Start typing a lease name...") _
                        .DataSource(Sub(dataSource)
                                        dataSource _
                                        .Read(Function(reader) reader.Action("StartsWith", "Lease") _
                                                                    .Type(HttpVerbs.Post)
                                                                        ) _
                                        .ServerFiltering(True)
                                    End Sub)
                    )
        @Html.ValidationMessageFor(Function(model) model.LeaseID)
    </div>
    <br />
    <div class="editor-label">
        @Html.LabelFor(Function(model) model.WellNumber, "Well Number")
    </div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.WellNumber)
        @Html.ValidationMessageFor(Function(model) model.WellNumber)
    </div>
    <br />
    <div class="editor-label">
        @Html.LabelFor(Function(model) model.APINumber, "API Number")
    </div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.APINumber)
        @Html.ValidationMessageFor(Function(model) model.APINumber)
    </div>
    <br />
    <div class="editor-label">
        @Html.LabelFor(Function(model) model.Inactive, "Inactive")
    </div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.Inactive)
        @Html.ValidationMessageFor(Function(model) model.Inactive)
    </div>

    <p>
        <input type="submit" value="Create" id="save" />
    </p>

</fieldset>
End Using

<div>
    @Html.ActionKendoButton("Back to List", "Index")
</div>
