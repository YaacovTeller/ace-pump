@Code
    ViewData("Title") = "Inventory Report"
    ViewData("ContainsAngularApp") = True
End Code

<h2>Inventory Report</h2>
<link type="text/css" rel="Stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"/>

<style type="text/css">
</style>
<fieldset>
    <div ng-app="acePump" ng-strict-di>
        <div ng-controller="InventoryController as invCtrl"
             ng-init="invCtrl.init({
                          readUrl: '@Url.Action("InventoryList", "Part")'
                    })">
            <ng-include src="'../html/deliveryTickets/inventoryGrid.html'"></ng-include>
        </div>
    </div>
</fieldset>

<script type="text/javascript" src="@Url.Content("~/app/js/mvc.inventoryApp.min.js")"></script>
<script src="//kendo.cdn.telerik.com/2015.3.930/js/jszip.min.js"></script>
