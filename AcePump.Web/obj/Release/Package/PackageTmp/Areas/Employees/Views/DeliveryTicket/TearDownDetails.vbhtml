@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.TearDownViewModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Tear Down"
    ViewData("ContainsAngularApp") = True
End Code

<h2>Tear Down</h2>
<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/Ticket.css")" />
<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/Upload.css")" />
<link type="text/css" rel="Stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"/>

<style type="text/css">
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

    <div style="clear: both; padding-top: 40px" />

    <div ng-app="acePump" ng-strict-di>
        <div ng-controller="TearDownController as tdCtrl"
             ng-init="tdCtrl.init({
                          readUrl: '@Url.Action("TearDownItemList", "DeliveryTicket", New With {.id = Model.DeliveryTicketID})',
                          deliveryTicketID: @Model.DeliveryTicketID,
                          isRepairComplete: @Html.Raw(Json.Encode(Model.RepairComplete))
                     })">

        <div><i class="fa fa-scissors fa-2x"></i>
            <span ng-show="!tdCtrl.serverValues.isRepairComplete">This pump is being torn down.</span>
            <span ng-show="tdCtrl.serverValues.isRepairComplete">This pump was torn down.</span>
        </div>

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
                        <tr role="row" ng-repeat="item in tdCtrl.tearDownItems | orderBy: 'SortOrder'">
                            <td>{{item.Quantity}}</td>
                            <td>{{item.OriginalPartTemplateNumber}}</td>
                            <td>{{item.PartDescription}}</td>
                            <td><span ng-show="item.onlyTrash"
                                      class="result-icon result-icon-trash-selected">
                                        <i class="fa fa-trash fa-2x"></i>
                                </span>                                
                                <span ng-show="!item.onlyTrash">
                                    <span class="result-icon"
                                          ng-class="item.Result==='Inventory' ? 'result-icon-recycle-selected' : 'result-icon-not-selected'">
                                        <i class="fa fa-recycle fa-2x"></i>
                                    </span>
                                    <span class="result-icon"
                                          ng-class="item.Result==='Trashed' ? 'result-icon-trash-selected' : 'result-icon-not-selected'">
                                        <i class="fa fa-trash fa-2x"></i>
                                    </span>
                                </span>
                            </td>
                            <td>{{item.ReasonRepaired}}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <p>
                <div class="repair-mode-container">
                    <div class="repair-mode-label" ng-show="tdCtrl.serverValues.isRepairComplete">Tear Down Complete</div>
                </div>
                <a href="@Url.Action("TearDown", "DeliveryTicket", New With {.id = Model.DeliveryTicketID})" ng-show="!tdCtrl.serverValues.isRepairComplete" class="k-button">Edit Tear Down</a>
            </p>
        </div>
    </div>

</fieldset>

<div>
    @Html.ActionKendoButton("Back to Delivery Ticket List", "Index")
</div>

<script type="text/javascript" src="@Url.Content("~/app/js/mvc.tearDownApp.min.js")"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>