@Imports AcePump.Common
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
@Imports Yesod.Kendo

@Code
    ViewData("Title") = "Index"
End Code

<script type="text/javascript">
    function assemblies_DetailsClick(e) {
        var grid = $("#assemblies").data("kendoGrid");
        var currentRow = $(e.currentTarget).closest("tr");
        var currentDataItem = grid.dataItem(currentRow);

        document.location = "@Url.Action("Details", "Assembly")/" + currentDataItem.AssemblyID;
    }

    function assemblies_DeleteClick(e) {
        var grid = $("#assemblies").data("kendoGrid");
        var currentRow = $(e.currentTarget).closest("tr");
        var currentDataItem = grid.dataItem(currentRow);
        var currentAssemblyID = currentDataItem.AssemblyID;

        $.ajax({ 
            url: '@Url.Action("EnsureCanDelete", "Assembly")',
            type: 'POST',            
            dataType: "json",            
            data: {'id': currentAssemblyID},
            success: function(result) {
                if(!result.Success) {
                    displayCannotDeleteAssemblyDialog(result.Errors);
                } else {
                    displayDeleteAssemblyDialog(currentAssemblyID);
                }
            },
            error: function(data) {  }
        });
    }
    function displayCannotDeleteAssemblyDialog(errors) {
        var viewModel = (function() {
            function translateErrorsToViewModelArray(errorName, urlPrefix) {
                if(errorName in errors) {
                    var rawIds = errors[errorName][0].split(",");
                    for(var i=0; i<rawIds.length; i++) {
                        rawIds[i] = {
                            text: rawIds[i],
                            url: urlPrefix + "/" + rawIds[i]
                        };
                    }
                    return {
                        data: rawIds,
                        hasNoData: false
                    };
                    
                } else {
                    return {
                        data: [],
                        hasNoData: true
                    };
                }
            }

            return {
                lineItems: translateErrorsToViewModelArray("lineItems", "@Url.Action("Details", "DeliveryTicket")"),
                partInspections: translateErrorsToViewModelArray("inspections", "@Url.Action("RepairDetails", "DeliveryTicket")"),
                templatePartDefs: translateErrorsToViewModelArray("templatePartDefs", "@Url.Action("Details", "PumpTemplate")"),
                customerSpecials: translateErrorsToViewModelArray("customerSpecials", "@Url.Action("PriceList", "Customer")")
            };
        }());

        var dialog = $("<div />").kendoWindow({
            title: "Cannot Delete Assembly",
            resizable: false,
            modal: true
        }).data("kendoWindow");

        var dialogHtml = $("#CannotDeleteAssemblyDialog").html().trim();
        
        dialog.content(dialogHtml);
        kendo.bind(dialog.element, viewModel);
        dialog.center().open();

        dialog.element
                .find(".dialog-close")
                    .click(function () {
                        dialog.close();
                    })
                    .end();  
    }

    function displayDeleteAssemblyDialog(id) {
        var dialog = $("<div />").kendoWindow({
            title: "Delete Assembly",
            resizable: false,
            modal: true
        });

        dialog.parent().find(".k-window-action").css("visibility", "hidden");

        dialog.data("kendoWindow")
                .content($("#DeleteAssemblyDialog").html())
                .center().open();

        dialog
                .find(".dialog-delete,.dialog-dont-delete")
                    .click(function () {
                        if ($(this).hasClass("dialog-delete")) {
                            deleteAssembly(id);
                        } 
                        dialog.data("kendoWindow").close();
                    })
                    .end();            
    }

    function displayAjaxModelError(modelState){
            var errorString = "";
            for(var property in modelState) {
                for(var i=0; i < modelState[property].length; i++) {
                    errorString += property + ": " + modelState[property][i];
                }
            }

            alert(errorString);
     }

     function deleteAssembly(id) {
        $.ajax({ 
            url: '@Url.Action("Delete", "Assembly")',
            type: 'POST',            
            dataType: "json",            
            data: {'id': id},
            success: function(result) {
                if(!result.Success) {
                    displayCannotDeleteAssemblyDialog(result.Errors);
                } else {
                    var grid = $("#assemblies").data("kendoGrid");
                    grid.dataSource.read();
                    grid.refresh();
                }
            },
            error: function(data) {  }
        });

     }

</script>

<script id="DeleteAssemblyDialog" type="text/x-kendo-template">
	<img class="delete-assembly-warning" src="@Url.Content("~/Content/RedX.jpg")" alt="Deleting Assembly" />
    <p class="dialog-title">WARNING: You are about to completely delete this assembly from your system.  
    </p>
    <p>
    Once an assembly has been deleted, it cannot be used in any pumps, templates, or tickets. 
    You will not be able to undo this action. 
    </p>
    <p>
    Are you sure you want to delete this assembly?
    </p>

    <button class="dialog-delete k-button">Yes</button>
    <button class="dialog-dont-delete k-button">No don't delete</button>
</script>

<script id="CannotDeleteAssemblyDialog" type="text/x-kendo-template">	
    <p class="dialog-title">Sorry, other records depend on this assembly and it cannot be deleted.  Please remove this assembly from those records and try again.
    </p>
    
    <h2 data-bind="invisible: lineItems.hasNoData">Delivery Tickets that use this assembly</h2>
    <ul data-bind="{source: lineItems.data, invisible: lineItems.hasNoData}" data-template="dialog-row-template">
    </ul>

    <h2 data-bind="invisible: partInspections.hasNoData">Repair ticket lines that use this assembly</h2>
    <ul data-bind="{source: partInspections.data, invisible: partInspections.hasNoData}" data-template="dialog-row-template">
    </ul>
    
    <h2 data-bind="invisible: templatePartDefs.hasNoData">Pump templates that use this assembly</h2>
    <ul data-bind="{source: templatePartDefs.data, invisible: templatePartDefs.hasNoData}" data-template="dialog-row-template">
    </ul>
    
    <h2 data-bind="invisible: customerSpecials.hasNoData">Customer's with specials on this assembly</h2>
    <ul data-bind="{source: customerSpecials.data, invisible: customerSpecials.hasNoData}" data-template="dialog-row-template">
    </ul>
    
    <p>
        <button class="dialog-close k-button">Close</button>
    </p>
</script>

<script id="dialog-row-template" type="text/x-kendo-template">	
    <li><a data-bind="attr: {href: url}">${ data.text }</a></li>
</script>

<h2>Asssemblies List</h2>

<p>
    @Html.ActionKendoButton("Create New Assembly", "Create")
</p>

@(Html.Kendo().Grid(Of AssemblyGridRowModel)() _
    .Name("assemblies") _
    .Filterable() _
    .Sortable() _
    .Pageable() _
    .Columns(Sub(c)
                     c.Bound(Function(x) x.AssemblyID).Filterable(FilterableType.NumericId)
                     c.Bound(Function(x) x.AssemblyNumber)
                     c.Bound(Function(x) x.Category)
                     
                     If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
                         c.Bound(Function(x) x.TotalPartsCost)
                         c.Bound(Function(x) x.Markup)
                     End If
                     
                     c.Bound(Function(x) x.Discount)
                     c.Bound(Function(x) x.TotalPartsResalePrice)
                     c.Bound(Function(x) x.ResalePrice)
                     c.Command(Sub(command)
                                       command.Custom("Details").Click("assemblies_DetailsClick")
                                       command.Custom("Delete").Click("assemblies_DeleteClick")
                               End Sub)
             End Sub) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Ajax() _
                        .Model(Sub(model) model.Id(Function(id) id.AssemblyID)) _
                        .Read("List", "Assembly")
                End Sub)
    )