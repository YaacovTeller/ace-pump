@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.TearDownViewModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Tear Down"
    ViewData("ContainsAngularApp") = True
End Code

<h2>Tear Down</h2>
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

    .k-grid tbody button.k-button {
        min-width: auto;
    }

    .k-grid tbody .k-button {
        min-width: auto;
    }

    .result-icon {
        margin-left: 5px;
        margin-right: 5px;
    }

    .result-icon-trash-selected{
        color: darkslategray;
    }

    .result-icon-recycle-selected{
        color: limegreen;
    }

    .result-icon-not-selected{
        color: gainsboro;
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
    @Html.ActionKendoButton("Download PDF", "TearDownPdf", New With {.id = Model.DeliveryTicketID})
</p>

<fieldset>
    <legend>Tear Down Related to Delivery Ticket #@Model.DeliveryTicketID</legend>

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

                <span id="PumpFailedTemplatSpecSummary">@Html.DisplayFor(Function(model) model.PumpFailedTemplatSpecSummary)</span>
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

    <div style="clear: both; padding-top: 40px" />
    <div ng-app="acePump" ng-strict-di>
        <div ng-controller="TearDownController as tdCtrl"
             ng-init="tdCtrl.init({
                          readUrl: '@Url.Action("TearDownItemList", "DeliveryTicket", New With {.id = Model.DeliveryTicketID})',
                          reasonRepairedListUrl: '@Url.TypeManagerListUrl("ReasonRepaired")',
                          updateUrl: '@Url.Action("UpdateTearDownItem", "DeliveryTicket")',
                          switchRepairModeUrl: '@Url.Action("SwitchRepairMode", "DeliveryTicket")',
                          completeTearDownUrl: '@Url.Action("CompleteTearDown", "DeliveryTicket")',
                          deliveryTicketID: @Model.DeliveryTicketID
                     })">

      <div><i class="fa fa-scissors fa-2x"></i> This pump is being torn down.</div>

      <p>
          <button class="k-button" ng-click="tdCtrl.switchRepairMode()"><i class="fa fa-wrench fa-2x"></i> Switch Back To Regular Repair</button>
      </p>

      <h2>Parts</h2>
                <div class="k-grid k-widget">
                    <table role="grid">
                        <colgroup><col style="width:20px"><col style="width:100px"><col style="width:500px"><col style="width:100px"><col></colgroup>
                        <thead role="rowgroup" class="k-grid-header">
                            <tr role="row">
                                <th role="columnheader" class="k-header">Qty</th>
                                <th role="columnheader" class="k-header">Part Number</th>
                                <th role="columnheader" class="k-header">Part Description</th>
                                <th role="columnheader" class="k-header">Result</th>
                                <th role="columnheader" class="k-header">Reason Trashed</th>
                            </tr>
                        </thead>
                        <tbody role="rowgroup" class="k-grid-content">
                            <tr role="row" ng-repeat="item in tdCtrl.tearDownItems | orderBy: 'SortOrder'" ng-class="{'server-operation': item.serverOperationInProgress, 'update-failed': item.updateFailed}">
                                <td>{{item.Quantity}}</td>
                                <td>{{item.OriginalPartTemplateNumber}}</td>
                                <td>{{item.PartDescription}}</td>
                                <td><span ng-show="item.onlyTrash"
                                          class="result-icon result-icon-trash-selected">
                                        <i class="fa fa-trash fa-2x"></i>
                                    </span>   
                                    <span ng-show="!item.onlyTrash">
                                        <span class="result-icon"
                                              ng-class="item.Result==='Inventory' ? 'result-icon-recycle-selected' : 'result-icon-not-selected'"
                                              ng-click="tdCtrl.setResult(item,'Inventory')">
                                            <i class="fa fa-recycle fa-2x"></i>
                                        </span>
                                        <span class="result-icon"
                                              ng-class="item.Result==='Trashed' ? 'result-icon-trash-selected' : 'result-icon-not-selected'"
                                              ng-click="tdCtrl.setResult(item,'Trashed')">
                                            <i class="fa fa-trash fa-2x"></i>
                                        </span>
                                    </span>
                                </td>
                                <td>
                                    <span ng-show="item.Result === 'Trashed'" >
                                        <select kendo-drop-down-list
                                                k-ng-model="item.ReasonRepaired"
                                                k-data-text-field="'DisplayText'"
                                                k-data-value-field="'DisplayText'"
                                                k-value-primitive="true"
                                                k-data-source="tdCtrl.reasonsRepaired" />

                                        <input type="button" style="margin-left: 10px" class="k-button" ng-click="tdCtrl.saveTearDownItem(item)" value="Save Reason Repaired" />
                                    </span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <button class="k-button btn-complete" 
                        ng-disabled="!tdCtrl.allMarked" 
                        ng-click="tdCtrl.completeTearDown()">Complete Tear Down</button>

                <form action="@Url.Action("CompleteRepair", "DeliveryTicket")" method="post">
                    <input type="hidden" name="id" value="@Model.DeliveryTicketID" />
                </form>
            </div>
    </div>
</fieldset>

<div>
    @Html.ActionKendoButton("Back to Delivery Ticket List", "Index")
</div>

<script type="text/javascript" src="@Url.Content("~/app/js/mvc.tearDownApp.min.js")"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>