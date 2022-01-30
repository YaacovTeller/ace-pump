﻿@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.RepairTicketModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Repair Ticket Details"
    ViewData("ContainsAngularApp") = True
End Code

<h2>Repair</h2>

<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/Ticket.css")" />

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
<link type="text/css" rel="Stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>

<style type="text/css">
    .has-parent-assembly {
        background-color: #FDC0CB;
    }

    .wear-table {
        margin-top: 20px;
        border-collapse: collapse;
        empty-cells: show;
    }

    .title-cell {
        width: 70px;
    }

    .wear-table th, td {
        border: 1px solid dimgray;
        padding: 3px;
        text-align: center;
    }

    .wear-details-cells {
        padding: 5px;
        min-width: 15px;
    }

    .originalPart-cust-icon{
        color: mediumvioletred;
    }

    .partReplacement-inventory-icon{
        color: limegreen;
    }

    .repair-mode-container {
        text-align: center;
    }

    .repair-mode-label {
        color: blue;
        font-size: larger;
        display: inline-block;
    }
</style>

<p>
    @Html.ActionKendoButton("View Delivery Ticket", "Details", New With {.id = Model.DeliveryTicketID})
    @Html.ActionKendoButton("Download PDF", "RepairPdf", New With {.id = Model.DeliveryTicketID})
</p>

<fieldset>
    <legend>Repairs Related to Delivery Ticket #@Model.DeliveryTicketID</legend>

    @Html.ValidationSummary()

    <ol>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.PumpFailedNumber)
            </div>
            <div class="editor-field">
                @Html.DisplayFor(Function(model) model.PumpFailedPrefix)@Html.DisplayFor(Function(model) model.PumpFailedNumber)
            </div>
        </li>

        <li>
            <div class="editor-label">
                Template ID
            </div>

            <div class="editor-field">
                @Html.DisplayFor(Function(model) model.PumpFailedTemplateID)
            </div>
        </li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.PumpDispatchedNumber)
            </div>
            <div class="editor-field">
                @Html.DisplayFor(Function(model) model.PumpDispatchedPrefix)@Html.DisplayFor(Function(model) model.PumpDispatchedNumber)
                @Html.ValidationMessageFor(Function(model) model.PumpDispatchedNumber)
            </div>
        </li>

    </ol>

    <div class="repair-edit-images">
        <div class="repair-edit-label">Picture Manager</div>
        @(Html.Kendo().Grid(Of DeliveryTicketImageUploadGridRow)() _
                                    .Name("ImagesGrid") _
                                    .Columns(Sub(c)
                                                 c.Bound(Function(b) b.DeliveryTicketImageUploadID).Title("ID")
                                                 c.Bound(Function(b) b.SmallImagePath).Title("").ClientTemplate("<a href=""#=LargeImagePath#"" target=""_blank""><img src=""#=SmallImagePath#"" height=""150""/></a>").Width("10%")
                                                 c.Bound(Function(b) b.UploadedOn).Title("Date Added").Format("{0:d}")
                                                 c.Bound(Function(b) b.UploadedBy).Title("Uploaded By")
                                                 c.Bound(Function(b) b.Note).Title("Note")
                                             End Sub) _
                                    .DataSource(Sub(dataSource)
                                                    dataSource _
                                            .Ajax() _
                                            .Model(Sub(model)
                                                       model.Id(Function(x) x.DeliveryTicketImageUploadID)
                                                   End Sub) _
                                            .Read(Sub(read)
                                                      read.Action("ImageGridList", "DeliveryTicket")
                                                      read.Data("{id: " & Model.DeliveryTicketID & "}")
                                                  End Sub)
                                                End Sub)
        )
    </div>
    <div ng-app="acePump" ng-strict-di>
        <div ng-controller="PlungerBarrelWearController as pbwCtrl"
             ng-init="pbwCtrl.init({
                          originalPlungerBarrelWear: '@Model.PlungerBarrelWear'
             })">
            <table class="wear-table plunger-table">
                <thead>
                    <tr>
                        <th class="title-cell">FEET</th>
                        <th class="title-cell">TOP</th>
                        <th ng-repeat="num in pbwCtrl.PlungerOrig track by $index">{{$index + 1}}</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>PLUNGER</td>
                        <td>ORIG PLGR</td>
                        <td ng-repeat="plgOrig in pbwCtrl.PlungerOrig  track by $index" class="wear-details-cells">
                            {{pbwCtrl.PlungerOrig[$index]}}
                        </td>
                    </tr>
                    <tr>
                        <td>WEAR</td>
                        <td>REPAIRED</td>
                        <td ng-repeat="plgWear in pbwCtrl.PlungerWearRepaired  track by $index" class="wear-details-cells">
                            {{pbwCtrl.PlungerWearRepaired[$index]}}
                        </td>
                    </tr>
                    <tr>
                        <td>(0.001)</td>
                        <td>OUT</td>
                        <td ng-repeat="plgOut in pbwCtrl.PlungerOut  track by $index" class="wear-details-cells">
                            {{pbwCtrl.PlungerOut[$index]}}
                        </td>
                    </tr>
                </tbody>
            </table>

            <table class="wear-table barrel-table">
                <thead>
                    <tr>
                        <th class="title-cell">FEET</th>
                        <th class="title-cell">TOP</th>
                        <th ng-repeat="num in pbwCtrl.BarrelOrig track by $index">{{$index + 1}}</th>
                        <th>BTM</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>BARREL</td>
                        <td>ORIG BRL</td>
                        <td ng-repeat="brlOrig in pbwCtrl.BarrelOrig  track by $index" class="wear-details-cells">
                            {{pbwCtrl.BarrelOrig[$index]}}
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>WEAR</td>
                        <td>REPAIRED</td>
                        <td ng-repeat="brlWear in pbwCtrl.BarrelWearRepaired  track by $index" class="wear-details-cells">
                            {{pbwCtrl.BarrelWearRepaired[$index]}}
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>(0.001)</td>
                        <td>OUT</td>
                        <td ng-repeat="brlOut in pbwCtrl.BarrelOut  track by $index" class="wear-details-cells">
                            {{pbwCtrl.BarrelOut[$index]}}
                        </td>
                        <td></td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div style="clear: both; padding-top: 40px" />


            <div ng-controller="RepairTicketController"
                 ng-init="serverVars = {
                          readUrl: '@Url.Action("InspectionList", "DeliveryTicket")',
                          listPartsInUseForPumpUrl:'@Url.Action("ListPartsInUseForPump", "Part")',
                          pumpFailedID:  @IIf(Model.PumpFailedID.HasValue, Model.PumpFailedID, "null"),
                          currentTicketDate: '@Model.CurrentTicketDate',
                          customerID: @Model.CustomerID,
                          deliveryTicketID: @Model.DeliveryTicketID,
                          customerUsesInventory: @Model.CustomerUsesInventoryString,
                          canModifyInventory: false,
                          isRepairComplete: @Html.Raw(Json.Encode(Model.IsRepairComplete))}">
                <div>
                    <i class="fa fa-wrench fa-2x"></i>
                    <span ng-show="!serverVars.isRepairComplete">This pump is being repaired.</span>
                    <span ng-show="serverVars.isRepairComplete">This pump was repaired.</span>
                </div>

                <h2>Parts</h2>

                <div class="k-grid k-widget">
                    <table role="grid">
                        <colgroup><col style="width:20px"><col><col><col style="width:100px"><col><col><col></colgroup>
                        <thead role="rowgroup" class="k-grid-header">
                            <tr role="row">
                                <th role="columnheader" class="k-header">Qty</th>
                                <th role="columnheader" class="k-header">Part Number</th>
                                <th role="columnheader" class="k-header">Part Description</th>
                                <th role="columnheader" class="k-header">Status</th>
                                <th role="columnheader" class="k-header">Replacement Quantity</th>
                                <th role="columnheader" class="k-header">Replacement Part Number</th>
                                <th role="columnheader" class="k-header">Reason Repaired</th>                                
                            </tr>
                        </thead>
                        <tbody role="rowgroup" class="k-grid-content">
                            <tr role="row" ng-repeat="inspection in inspections | orderBy:'SortOrder'" ng-class="{'has-parent-assembly' : inspection.HasParentAssembly}">
                                <td>{{inspection.Quantity}}</td>
                                <td>{{inspection.OriginalPartTemplateNumber}} <i class="fa fa-history fa-lg originalPart-cust-icon" ng-show="inspection.OriginalCustomerOwnedPartID > 0"></i></td>
                                <td>{{inspection.PartDescription}}</td>
                                <td>{{inspection.Result}}</td>
                                <td>{{inspection.ReplacementQuantity}}</td>
                                <td>{{inspection.ReplacementPartTemplateNumber}}                                     
                                    <i class="fa fa-recycle fa-lg partReplacement-inventory-icon" ng-show="inspection.ReplacedWithInventoryPartID"></i>
                                </td>
                                <td>{{inspection.ReasonRepaired}}</td>                                                                
                            </tr>
                        </tbody>
                    </table>
                </div>
                <p>
                    <div class="repair-mode-container">
                        <div class="repair-mode-label" ng-show="serverVars.isRepairComplete">Repair Complete</div>
                    </div>
                    <a href="@Url.Action("Repair", "DeliveryTicket", New With {.id = Model.DeliveryTicketID})" ng-show="!serverVars.isRepairComplete" class="k-button">Edit Repair</a>
                </p>

            </div>
        </div>
</fieldset>

<div>
    @Html.ActionKendoButton("Back to Delivery Ticket List", "Index")
</div>
<script type="text/javascript" src="@Url.Content("~/app/js/mvc.repairTicketApp.min.js")"></script>

