@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.RepairTicketModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Repair Ticket"
    ViewData("ContainsAngularApp") = True
End Code

<h2>Repair</h2>
<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/Ticket.css")" />
<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/Upload.css")" />
<link type="text/css" rel="Stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>

<style type="text/css">
    .btn-complete  {
        width: 100%;
        height: 50px;
        margin-left: auto;
        margin-right: auto;
        margin-top: 10px;
    }

    .server-operation {
        color: rgb(150,150,150);
    }

    .update-failed {
        background-color: rgba(255,0,0,0.2);
    }

    tr.server-operation .status-icon {
        background-image: url('@Url.Content("~/Content/kendo/Default/loading-image.gif")');
        background-repeat: no-repeat;
        background-size: contain;
        width: 25px;
        height: 25px;
        display: inline-block;
    }

    .has-parent-assembly {
        background-color: #FDC0CB;
    }

    .dialog-title {
        width: 225px;
    }

    .k-grid tbody button.k-button {
        min-width: auto;
    }

    .k-grid tbody .k-button {
        min-width: auto;
    }

    .grid-legend {
        text-align: right;
    }

    .grid-legend .k-button-iconcontext {
        border: gray solid 1px;
        padding: 3px 12px 3px 3px;
        margin: 5px;
        display: inline-block;
        background-color: #C6FFAF;
    }

    .s-modal-backdrop {
        position: fixed;
        top: 0;
        left: 0;
        background: rgba(0,0,0,0.6);
        width: 100%;
        height: 100%;
        vertical-align: middle;
    }

    .s-modal-container {
        background-color: white;
        border: 1px solid black;
        width: 300px;

        margin-left: auto;
        margin-right: auto;
    }

    .s-modal-title-bar {
        background-color: #3A487C;
        color: White;
        font-weight: bold;
        padding: 4px;
    }

    .s-modal-content {
        padding: 10px;
    }

    .s-modal-buttons button {
        margin-left: 5px;
        margin-right: 5px;
    }
    .s-modal-buttons {
        text-align: center;
        padding-bottom: 4px;
    }

    .wear-table{
        margin-top: 20px;
        border-collapse: collapse;
        empty-cells: show;
    }

    .title-cell {
        width:70px;
    }

    .wear-table th, td {
        border: 1px solid dimgray;
        padding: 3px;
        text-align: center;
    }

    .wear-table td input[type="text"]{
        width: 26px;
        border: 0px;
    }

   .originalPart-cust-icon{
        color: mediumvioletred;
    }

   .part-replaced-from-inventory {
        color: limegreen;
    }

    .part-replaced-not-inventory {
            color: gray;
    }

    .wear-table td input[type="text"].ng-invalid {
        border: red solid 1px;
    }
</style>

<script type="text/javascript">
    function TicketImageUploader_Success(e) {
        var imagesGrid = $("#ImagesGrid").data("kendoGrid");

        DisplayUploadErrors(e);
        imagesGrid.dataSource.read();
        imagesGrid.refresh();
    }

    function DisplayUploadErrors (e) {
        if (e.response) {
            var msg = "";

            for (var fileInfoIndex = 0; fileInfoIndex < e.files.length; fileInfoIndex++) {
                var fileInfo = e.files[fileInfoIndex];
                for (var i = 0; i < e.response[fileInfo.name].Errors.length; i++) {
                    msg += fileInfo.name + ": " + e.response[fileInfo.name].Errors[i].ErrorMessage + "\n";
                }
            }
            if(msg.length > 0) alert(msg);
        }
    }

    function TicketImageUploader_Error(e) {
        var err = e.XMLHttpRequest.responseText;
        alert(err);
    };

    function TicketImageUploader_Select(e) {
        var msg = "";
        $.each(e.files, function(index, value) {
            if(value.extension.toUpperCase() !== ".JPG") {
                e.preventDefault();
                msg = "Please upload only jpg image files";
            }
        });
        if(msg.length > 0) alert(msg);
    }

    function TicketImageUploader_Complete() {
        $("li.k-file.k-file-success").remove();
        $("li.k-file.k-file-error").remove();

        if($("li.k-file").length) {
            $(".k-upload-status").remove();
        }
    }

    function TicketImageUploader_Upload(e) {
        var files = e.files;

        $.each(files, function () {
            if (this.size > 4*1024*1024) {
                alert("The image you have chosen: " + this.name + " is too big! Max image size is 4 MB.");
                e.preventDefault();
            }
        });
    }
</script>

<p>
    @Html.ActionKendoButton("View Delivery Ticket", "Details", New With {.id = Model.DeliveryTicketID})
    @Html.ActionKendoButton("Download PDF", "RepairPdf", New With {.id = Model.DeliveryTicketID})
</p>

<fieldset>
    <legend>Repairs Related to Delivery Ticket #@Model.DeliveryTicketID</legend>

    @Html.ValidationSummary()
    <div ng-app="acePump" ng-strict-di>
        <div ng-controller="RepairTicketController"
             ng-init="serverVars = {
                          currentTicketDate: '@Model.CurrentTicketDate',
                          deliveryTicketID: @Model.DeliveryTicketID,
                          customerID: @Model.CustomerID,
                          readUrl: '@Url.Action("InspectionList", "DeliveryTicket")',
                          updateUrl: '@Url.Action("UpdateInspection", "DeliveryTicket")',
                          removeUrl: '@Url.Action("RemoveInspection", "DeliveryTicket")',
                          splitAssmUrl: '@Url.Action("SplitAssemblyInspection", "DeliveryTicket")',
                          reasonRepairedListUrl: '@Url.TypeManagerListUrl("ReasonRepaired")',
                          partsByNumberReadUrl: '@Url.Action("StartsWith", "PartTemplate")',
                          syncInspectionOrderUrl: '@Url.Action("SynchronizeRepairOrder", "DeliveryTicket")',
                          switchRepairModeUrl: '@Url.Action("SwitchRepairMode", "DeliveryTicket")',
                          updateUsingPartFromInventoryUrl: '@Url.Action("UpdateInspectionUsingInventory", "DeliveryTicket")',
                          checkAvailabilityInInventoryUrl: '@Url.Action("IsAvailableInInventory", "Part")',
                          checkInventoryListUrl:'@Url.Action("ListPartsAvailableInInventory", "Part")',
                          listPartsInUseForPumpUrl:'@Url.Action("ListPartsInUseForPump", "Part")',
                          listTemplatesUrl:'@Url.Action("IDList", "PumpTemplate")',
                          updateTemplateUrl:'@Url.Action("UpdateTemplate", "DeliveryTicket")',
                          customerUsesInventory: @Model.CustomerUsesInventoryString,
                          canModifyInventory: true,
                          pumpTemplateID: @Model.PumpFailedTemplateID,
                          pumpFailedID:  @IIf(Model.PumpFailedID.HasValue, Model.PumpFailedID, "null"),
                          pumpFailedSpecSummary: '@HttpUtility.JavaScriptStringEncode(Model.PumpFailedTemplatSpecSummary)'
                     }">

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
                        <span>{{serverVars.pumpTemplateID}}</span>
                    </div>
                    <a href ng-click="showUpdateTemplateSelect()" ace-pump-loading="templatesLoading" ng-show="!updateTemplateSelectVisible" style="padding-left:10px; display: inline-block; padding-top: 30px;">Update Pump Failed Template</a>

                    <div id="updateTemplateContainer" ng-show="updateTemplateSelectVisible" style="padding-left:10px; display: inline-block; padding-top: 30px;">
                        <select id="updateTemplate" ng-model="selectedTemplate" style="width: 80px" ng-options='template.PumpTemplateId for template in templates'></select>
                        <button class="k-button" ng-disabled="!selectedTemplate" ng-click="updateTemplate()">Update Pump Template</button>
                    </div>

                    <div style="clear:both"></div>

                    <div style="padding-top:10px;">{{serverVars.pumpFailedSpecSummary}}</div>
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
                <div class="repair-uploader">
                    @(Html.Kendo.Upload() _
                    .Name("TicketImageUploader") _
                    .Async(Sub(async)
                               async.Save("UploadImage", "DeliveryTicket", New With {.id = Model.DeliveryTicketID})
                               async.AutoUpload(True)
                           End Sub) _
                    .Events(Sub(events)
                                events.Success("TicketImageUploader_Success")
                                events.Error("TicketImageUploader_Error")
                                events.Select("TicketImageUploader_Select")
                                events.Complete("TicketImageUploader_Complete")
                                events.Upload("TicketImageUploader_Upload")
                            End Sub) _
                    .HtmlAttributes(New With {.accept = "image/jpeg"})
                    )
                </div>
                @(Html.Kendo().Grid(Of DeliveryTicketImageUploadGridRow)() _
                                .Name("ImagesGrid") _
                                .Editable() _
                                .Columns(Sub(c)
                                             c.Bound(Function(b) b.SmallImagePath).Title("").ClientTemplate("<a href=""#=LargeImagePath#"" target=""_blank""><img src=""#=SmallImagePath#"" height=""150""/></a>").Width("10%")
                                             c.Bound(Function(b) b.UploadedOn).Title("Date Added").Format("{0:d}")
                                             c.Bound(Function(b) b.UploadedBy).Title("Uploaded By")
                                             c.Bound(Function(b) b.Note).Title("Note")
                                             c.Command(Sub(com)
                                                           com.Edit().Text("Edit")
                                                           com.Destroy()
                                                       End Sub)
                                         End Sub) _
                                .DataSource(Sub(dataSource)
                                                dataSource _
                                        .Ajax() _
                                        .Model(Sub(model)
                                                   model.Id(Function(x) x.DeliveryTicketImageUploadID)
                                                   model.Field(Function(x) x.DeliveryTicketImageUploadID).Editable(False)
                                                   model.Field(Function(x) x.SmallImagePath).Editable(False)
                                                   model.Field(Function(x) x.LargeImagePath).Editable(False)
                                                   model.Field(Function(x) x.UploadedOn).Editable(False)
                                                   model.Field(Function(x) x.UploadedBy).Editable(False)
                                               End Sub) _
                                        .Read(Sub(read)
                                                  read.Action("ImageGridList", "DeliveryTicket")
                                                  read.Data("{id: " & Model.DeliveryTicketID & "}")
                                              End Sub) _
                                        .Update("EditImageInfo", "DeliveryTicket") _
                                        .Destroy("DeleteImage", "DeliveryTicket")
                                            End Sub)
                )
            </div>

            <div ng-controller="PlungerBarrelWearController as pbwCtrl"
                 ng-init="pbwCtrl.init({
                          originalPlungerBarrelWear: '@Model.PlungerBarrelWear',
                          updateUrl: '@Url.Action("UpdatePlungerBarrelWear", "DeliveryTicket")',
                          plungerOrigFromPreviousOut: '@Model.PlungerOrig',
                          barrelOrigFromPreviousOut: '@Model.BarrelOrig',
                          deliveryTicketID: @Model.DeliveryTicketID
                          })">
                <form name="frmPlungerWear" ng-submit="frmPlungerWear.$valid && pbwCtrl.save()" novalidate>
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
                                <td ng-repeat="plgOrig in pbwCtrl.PlungerOrig  track by $index">
                                    <input type="text" class="wear-input" ng-trim="false" ng-model="pbwCtrl.PlungerOrig[$index]" maxlength="2" auto-advance select-on-click pattern="[- 0-9][ 0-9]" pad-input />
                                </td>
                            </tr>
                            <tr>
                                <td>WEAR</td>
                                <td>REPAIRED</td>
                                <td ng-repeat="plgWear in pbwCtrl.PlungerWearRepaired  track by $index">
                                    <input type="text" class="wear-input" ng-trim="false" maxlength="2" ng-model="pbwCtrl.PlungerWearRepaired[$index]" auto-advance select-on-click pattern="[- 0-9][ 0-9]" pad-input />
                                </td>
                            </tr>
                            <tr>
                                <td>(0.001)</td>
                                <td>OUT</td>
                                <td ng-repeat="plgOut in pbwCtrl.PlungerOut  track by $index">
                                    <input type="text" class="wear-input" ng-trim="false" maxlength="2" ng-model="pbwCtrl.PlungerOut[$index]" auto-advance select-on-click pattern="[- 0-9][ 0-9]" pad-input />
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
                                <td ng-repeat="brlOrig in pbwCtrl.BarrelOrig  track by $index">
                                    <input type="text" class="wear-input" ng-trim="false" maxlength="2" ng-model="pbwCtrl.BarrelOrig[$index]" auto-advance select-on-click pattern="[- 0-9][ 0-9]" pad-input />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>WEAR</td>
                                <td>REPAIRED</td>
                                <td ng-repeat="brlWear in pbwCtrl.BarrelWearRepaired  track by $index">
                                    <input type="text" class="wear-input" ng-trim="false" maxlength="2" ng-model="pbwCtrl.BarrelWearRepaired[$index]" auto-advance select-on-click pattern="[- 0-9][ 0-9]" pad-input />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>(0.001)</td>
                                <td>OUT</td>
                                <td ng-repeat="brlOut in pbwCtrl.BarrelOut  track by $index">
                                    <input type="text" class="wear-input" ng-trim="false" maxlength="2" ng-model="pbwCtrl.BarrelOut[$index]" auto-advance select-on-click pattern="[- 0-9][ 0-9]" pad-input />
                                </td>
                                <td></td>
                            </tr>
                        </tbody>
                    </table>
                    <p>
                        <button class="k-button" ng-disabled="!frmPlungerWear.$valid || frmPlungerWear.$pristine">Save Plunger / Barrel Wear</button>
                        <span ng-show="frmPlungerWear.$dirty">You have unsaved changes.</span>
                        <span ng-show="!frmPlungerWear.$dirty">All changes saved.</span>
                    </p>
                </form>
            </div>

            <div style="clear: both; padding-top: 5px" />
            <p>
                <button class="k-button"
                        ng-show="serverVars.customerUsesInventory"
                        ng-click="switchRepairMode()">
                    <i class="fa fa-wrench fa-2x"></i> Scrap Pump / Tear Down
                </button>
            </p>

            <h2>Parts</h2>

            <button class="k-button" ng-click="syncInspectionOrder()">Synchronize Order</button>

            <div class="grid-legend">
                <span class="k-button-iconcontext"><span class="k-icon k-update"></span>OK</span>
                <span class="k-button-iconcontext"><span class="k-icon k-cancel"></span>N/A</span>
                <span class="k-button-iconcontext"><span class="k-icon k-i-custom"></span>Maintenance</span>
                <span class="k-button-iconcontext"><span class="k-icon k-i-redo"></span>Convert</span>
                <span class="k-button-iconcontext"><span class="k-icon k-i-refresh"></span>Replace</span>
                <span class="k-button-iconcontext"><span class="k-icon k-delete"></span>Delete</span>
            </div>

            <div class="k-grid k-widget">
                <table role="grid">
                    <colgroup><col style="width:20px"><col><col><col style="width:100px"><col><col><col><col></colgroup>
                    <thead role="rowgroup" class="k-grid-header">
                        <tr role="row">
                            <th role="columnheader" class="k-header">Qty</th>
                            <th role="columnheader" class="k-header">Part Number</th>
                            <th role="columnheader" class="k-header">Part Description</th>
                            <th role="columnheader" class="k-header">Status</th>
                            <th role="columnheader" class="k-header">Replacement Quantity</th>
                            <th role="columnheader" class="k-header">Replacement Part Number</th>
                            <th role="columnheader" class="k-header">Reason Repaired</th>
                            <th role="columnheader" class="k-header"></th>
                        </tr>
                    </thead>
                    <tbody role="rowgroup" class="k-grid-content">
                        <tr role="row" ng-repeat="inspection in inspections | orderBy:'SortOrder'" ng-class="{'has-parent-assembly' : inspection.HasParentAssembly, 'server-operation': inspection.serverOperationInProgress, 'update-failed': inspection.updateFailed}">
                            <td>{{inspection.Quantity}}</td>
                            <td>{{inspection.OriginalPartTemplateNumber}} <i class="fa fa-history fa-lg originalPart-cust-icon" ng-show="inspection.OriginalCustomerOwnedPartID > 0"></i></td>
                            <td>{{inspection.PartDescription}}</td>
                            <td>{{inspection.Result}}</td>
                            <td>
                                <span ng-show="!inspection.inEditMode">{{inspection.ReplacementQuantity}}</span>
                                <span ng-show="inspection.inEditMode"><input kendo-numeric-text-box ng-model="inspection.ReplacementQuantity" k-min="0" k-format="'n0'" /></span>
                            </td>
                            <td>
                                <span ng-show="!inspection.editReplacementPartTemplateNumber">
                                    {{inspection.ReplacementPartTemplateNumber}}
                                </span>
                                <span ng-show="inspection.editReplacementPartTemplateNumber">
                                    <kendo-combo-datasource-workaround>
                                        <select kendo-combo-box
                                                name="PartReplacedID"
                                                k-ng-model="inspection.ReplacementPartInfo"
                                                k-data-text-field="'PartTemplateNumber'"
                                                k-data-value-field="'PartTemplateID'"
                                                k-filter="'startswith'"
                                                k-min-length="2"
                                                k-placeholder="'Start typing a part number...'"
                                                k-data-source="[]"></select>
                                    </kendo-combo-datasource-workaround>
                                </span>
                                <button class="k-button" ng-click="usePartFromInventory(inspection)" ng-show="inspection.showInventoryButton">
                                    <i class="fa fa-recycle fa-lg" ng-class="{'part-replaced-from-inventory' : inspection.ReplacedWithInventoryPartID, 'part-replaced-not-inventory': !inspection.ReplacedWithInventoryPartID}"></i>
                                </button>
                            </td>
                            <td>
                                <span ng-show="!inspection.inEditMode">{{inspection.ReasonRepaired}}</span>
                                <span ng-show="inspection.inEditMode">
                                    <select kendo-drop-down-list
                                            k-ng-model="inspection.ReasonRepaired"
                                            k-data-text-field="'DisplayText'"
                                            k-data-value-field="'DisplayText'"
                                            k-value-primitive="true"
                                            k-auto-bind="false"
                                            k-data-source="reasonsRepaired" />
                                </span>
                            </td>
                            <td>
                                <button class="k-button" ng-disabled="inspection.serverOperationInProgress" ng-class="{'k-state-disabled' : inspection.serverOperationInProgress}" ng-show="!inspection.inEditMode" ng-click="markPart(inspection, 'OK')" title="OK"><span class="k-icon k-update"></span></button>
                                <button class="k-button" ng-disabled="inspection.serverOperationInProgress" ng-class="{'k-state-disabled' : inspection.serverOperationInProgress}" ng-show="!inspection.inEditMode" ng-click="markPart(inspection, 'NA')" title="N/A"><span class="k-icon k-cancel"></span></button>
                                <button class="k-button" ng-disabled="inspection.serverOperationInProgress" ng-class="{'k-state-disabled' : inspection.serverOperationInProgress}" ng-show="!inspection.inEditMode" ng-click="markPart(inspection, 'Maintenance')" title="Maintenance"><span class="k-icon k-i-custom"></span></button>
                                <button class="k-button" ng-disabled="inspection.serverOperationInProgress" ng-class="{'k-state-disabled' : inspection.serverOperationInProgress}" ng-show="!inspection.inEditMode && inspection.IsConvertible" ng-click="markPart(inspection, 'Convert')" title="Convert"><span class="k-icon k-i-redo"></span></button>
                                <button class="k-button" ng-disabled="inspection.serverOperationInProgress" ng-class="{'k-state-disabled' : inspection.serverOperationInProgress}" ng-show="!inspection.inEditMode" ng-click="markPart(inspection, 'Replace')" title="Replace"><span class="k-icon k-i-refresh"></span></button>
                                <button class="k-button" ng-disabled="inspection.serverOperationInProgress" ng-class="{'k-state-disabled' : inspection.serverOperationInProgress}" ng-show="!inspection.inEditMode && inspection.IsConvertible" ng-click="deleteInspection(inspection)" title="Delete"><span class="k-icon k-delete" title="Delete"></span></button>
                                <button class="k-button" ng-show="inspection.inEditMode" ng-click="saveEdit(inspection)" title="Save"><span class="k-icon k-update"></span>Save</button>
                                <button class="k-button" ng-show="inspection.inEditMode" ng-click="cancelEdit(inspection)" title="Cancel"><span class="k-icon k-cancel"></span>Cancel</button>
                                <span class="status-icon"></span>
                            </td>
                        </tr>
                    </tbody>
                    <tfoot class="k-grid-footer">
                        <tr class="k-footer-template">
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td><button class="k-button k-button-icontext" ng-click="saveAll()">Save All</button></td>
                        </tr>
                    </tfoot>
                </table>
            </div>

            <modal-dialog modal-id="suggest-replace-assembly"
                          title="This is an assembly"
                          ok-text="Whole Assembly"
                          cancel-text="Only Some Parts">

                This part is an assembly.  What are you replacing?
            </modal-dialog>

            <button class="k-button btn-complete" ng-click="validateAndPost()">Complete Repair Ticket</button>

            <form action="@Url.Action("CompleteRepair", "DeliveryTicket")" method="post" name="inspectionForm">
                <input type="hidden" name="id" value="@Model.DeliveryTicketID" />
            </form>
        </div>
    </div>
</fieldset>

<div>
    @Html.ActionKendoButton("Back to Delivery Ticket List", "Index")
</div>

<script type="text/javascript" src="@Url.Content("~/app/js/mvc.repairTicketApp.min.js")"></script>