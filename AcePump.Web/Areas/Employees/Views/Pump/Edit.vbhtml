﻿@Imports AcePump.Common
@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.PumpDisplayDto


@Code
    ViewData("Title") = "Edit"
    ViewData("ContainsAngularApp") = True
End Code

<h2>Edit</h2>

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

<script type="text/javascript">
    function LeaseID_AdditionalData() {
        return {
            customerId: $("#CustomerID").val()
        };
    }

    function WellID_AdditionalData() {
        return {
            term: $("#InstalledInWellID").data("kendoComboBox").input.val(),
            customerId: $("#CustomerID").val(),
            leaseId: $("#LeaseID").val(),
            includeOnlyActiveWells: true
        };
    }

    function CustomerID_Select(e) {
        $("#LeaseID").data("kendoComboBox").text("");
    }
</script>

@Using Html.BeginForm()
    @Html.ValidationSummary(True)

    @<div ng-app="acePump" ng-strict-di>
         <div ng-controller="PumpPrefixController as pumpPrefixCtrl"
              ng-init="pumpPrefixCtrl.init({
                    listShopLocationsUrl: '@String.Concat(AcePumpEnvironment.Environment.Configuration.PtpApi.UriV2, "api/pumps/shopLocations")',
                    shopLocationIDFromServer:  @(If(Model IsNot Nothing, Model.ShopLocationID, 0)),
                    pumpNumberFromServer: '@(If(Model IsNot Nothing, Model.PumpNumber, String.Empty))'})"
              ace-pump-loading="apiLoading">

             <fieldset>
                 <legend>Pump</legend>
                 @Html.HiddenFor(Function(model) model.PumpID)

                 <ol>
                     <li>
                         <div class="editor-label">
                             Pump Number
                         </div>
                         <fieldset class="editor-field" style="height:50px">
                             <legend>Shop</legend>
                             <input id="ShopLocationID" name="@Html.NameFor(Function(x) x.ShopLocationID)" type="hidden" ng-value="pumpPrefixCtrl.selectedShop.ShopLocationID">

                             <select ng-model="pumpPrefixCtrl.selectedShop"
                                     ng-options="shop as shop.Prefix for shop in pumpPrefixCtrl.shopLocations track by shop.ShopLocationID"
                                     ng-disabled="!pumpPrefixCtrl.shopLocations.length"></select>
                         </fieldset>

                         <fieldset class="editor-field" style="height:50px" ng-show="pumpPrefixCtrl.selectedShop">
                             <legend>Number</legend>
                             @Html.HiddenFor(Function(model) model.PumpNumber)
                             @Model.PumpNumber
                             @Html.ValidationMessageFor(Function(model) model.PumpNumber)
                         </fieldset>
                     </li>
                     <li>
                         <div class="editor-label">
                             Current Customer
                         </div>
                         <div class="editor-field">
                             @(Html.Kendo().ComboBox() _
                                                         .Name("CustomerID") _
                                                         .Text(Model.Customer) _
                                                         .DataTextField("Name") _
                                                         .DataValueField("Id") _
                                                         .MinLength(2) _
                                                         .Filter(FilterType.StartsWith) _
                                                         .Placeholder("Start typing a customer name...") _
                                                         .Events(Sub(events) events.Select("CustomerID_Select")) _
                                                         .DataSource(Sub(dataSource)
                                                                         dataSource _
                                                                         .Read(Sub(reader)
                                                                                   reader.Action("StartsWith", "Customer")
                                                                                   reader.Type(HttpVerbs.Post)
                                                                               End Sub) _
                                                                         .ServerFiltering(True)
                                                                     End Sub)
                             )
                         </div>
                         <a href="@Url.Action("Create", "Customer")" target="_blank" class="k-button k-button-icontext">Create New</a>
                     </li>
                     <li>
                         <div class="editor-label">
                             Lease Location
                         </div>
                         <div class="editor-field">
                             @(Html.Kendo().ComboBox() _
                                                         .Name("LeaseID") _
                                                         .Text(Model.Lease) _
                                                         .DataTextField("LeaseName") _
                                                         .DataValueField("LeaseID") _
                                                         .MinLength(2) _
                                                         .Filter(FilterType.Contains) _
                                                         .CascadeFrom("CustomerID") _
                                                         .Placeholder("Start typing a lease location name...") _
                                                         .AutoBind(False) _
                                                         .DataSource(Sub(dataSource)
                                                                         dataSource _
                                                                         .Read(Sub(reader)
                                                                                   reader.Action("GetFiltered", "Lease") _
                                                                                         .Data("LeaseID_AdditionalData") _
                                                                                         .Type(HttpVerbs.Post)
                                                                               End Sub) _
                                                                         .ServerFiltering(True)
                                                                     End Sub)
                             )

                         </div>
                     </li>
                     <li>
                         <div class="editor-label">
                             @Html.LabelFor(Function(model) model.InstalledInWellID, "Well Name")
                         </div>
                         <div class="editor-field">
                             @(Html.Kendo().ComboBoxFor(Function(model) model.InstalledInWellID) _
                                                     .DataTextField("WellNumber") _
                                                     .DataValueField("WellID") _
                                                     .AutoBind(False) _
                                                     .Text(Model.Well) _
                                                     .Filter(FilterType.Contains) _
                                                     .CascadeFrom("LeaseID") _
                                                     .DataSource(Sub(dataSource)
                                                                     dataSource _
                                                                     .Read(Sub(reader)
                                                                               reader.Action("GetFiltered", "Well") _
                                                                                     .Data("WellID_AdditionalData") _
                                                                                     .Type(HttpVerbs.Post)
                                                                           End Sub) _
                                                                     .ServerFiltering(True)
                                                                 End Sub)
                             )
                             @Html.ValidationMessageFor(Function(model) model.InstalledInWellID)
                         </div>
                     </li>
                     <li>
                         <div class="editor-label">
                             Pump Template
                         </div>
                         <div class="editor-field">
                             @(Html.Kendo().ComboBoxFor(Function(model) model.PumpTemplateID) _
                                                             .DataTextField("Display") _
                                                             .DataValueField("Id") _
                                                             .AutoBind(False) _
                                                             .MinLength(1) _
                                                             .Placeholder("Start typing a template ID...") _
                                                             .Filter(FilterType.StartsWith) _
                                                             .DataSource(Sub(dataSource)
                                                                             dataSource _
                                                                             .Read(Sub(reader)
                                                                                       reader.Action("StartsWith", "PumpTemplate") _
                                                                                             .Type(HttpVerbs.Post)
                                                                                   End Sub) _
                                                                             .ServerFiltering(True)
                                                                         End Sub)
                             )
                             @Html.ValidationMessageFor(Function(model) model.PumpTemplateID)
                         </div>
                     </li>
                 </ol>
                 <p>
                     <input type="submit" value="Save" />
                 </p>
             </fieldset>
         </div>
    </div>
End Using

<div>
    @Html.ActionKendoButton("Back to List", "Index")
</div>

<script type="text/javascript" src="@Url.Content("~/app/js/mvc.pumpPrefixApp.min.js")"></script>
