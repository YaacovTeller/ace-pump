@ModelType AcePump.Web.Areas.Customers.Models.TearDownViewModel

@Imports AcePump.Web.areas.customers.models
@Code
    ViewData("Title") = "Details"
    ViewData("ContainsAngularApp") = True
End Code

<h2>Details of Repair Ticket: @ViewData("DeliveryTicketID")</h2>

<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/Ticket.css")" />
<link type="text/css" rel="Stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
<style type="text/css">
    .result-icon {
        margin-left: 5px;
        margin-right: 5px;
    }

    .result-icon-trash-selected {
        color: darkslategray;
    }

    .result-icon-recycle-selected {
        color: limegreen;
    }

    .result-icon-not-selected {
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

<div class="customer-ticket-box">    
        <div class="display-label">Delivery Ticket # </div>   <div class="display-field"> @Model.DeliveryTicketID</div>
        <br />
        <div class="display-label">Date </div>                <div class="display-field">@String.Format("{0:d}", Model.TicketDate) </div>
        <hr />
        <div class="display-label">Pump Repaired </div>       <div class="display-field">@Model.PumpRepairedNumber</div>
</div>

<div class="customer-ticket-link">
    @Html.ActionKendoButton("View Delivery Ticket", "Details", "DeliveryTicket", New With {.id = Model.DeliveryTicketID.ToString})
</div>

<div class="clear"></div>
<div class="customer-pump-template"> Template: </div>                
<div class="display-field"> @Model.PumpRepairedTemplateVerbose </div>

<div style="clear: both; padding-top: 40px" />

<div ng-app="acePump" ng-strict-di>
    <div ng-controller="TearDownController as tdCtrl"
         ng-init="tdCtrl.init({
                          readUrl: '@Url.Action("TearDownItemList", "DeliveryTicket", New With {.id = Model.DeliveryTicketID})',
                          deliveryTicketID: @Model.DeliveryTicketID,
                          isRepairComplete: @Html.Raw(Json.Encode(Model.IsRepairComplete))
                     })">

        <div>
            <i class="fa fa-scissors fa-2x"></i>
            <span ng-show="!tdCtrl.serverValues.isRepairComplete">This pump is being torn down.</span>
            <span ng-show="tdCtrl.serverValues.isRepairComplete">This pump was torn down.</span>
        </div>

        <h2>Parts</h2>

        <div class="k-grid k-widget">
            <table role="grid">
                <colgroup><col style="width:20px"><col><col><col style="width:100px"><col></colgroup>
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
                        <td>
                            <span ng-show="item.Quantity !== 1"
                                  class="result-icon result-icon-trash-selected">
                                <i class="fa fa-trash fa-2x"></i>
                            </span>
                            <span ng-show="item.Quantity === 1">
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
                        <td>
                            {{item.ReasonRepaired}}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <p>
            <div class="repair-mode-container">
                <div class="repair-mode-label" ng-show="tdCtrl.serverValues.isRepairComplete">Tear Down Complete</div>
            </div>
        </p>
    </div>
</div>

<p>
    <div class="display-label">Notes: </div>  <div class="display-field">@Model.Notes</div>
</p>
    @Html.ActionKendoButton("Back to List", "Index", "DeliveryTicket")

<script type="text/javascript" src="@Url.Content("~/app/js/mvc.tearDownApp.min.js")"></script>


