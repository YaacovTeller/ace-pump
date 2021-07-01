@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Index"
    ViewData("ContainsAngularApp") = True
End Code

<script type="text/javascript" src="@Url.Content("~/Scripts/Soris.Kendo.min.js")"></script>

<h2>Well Locations</h2>

<script type="text/javascript">
    WellController.$inject = ["_urls", "grid", "kendoWindowService"];
    function WellController(_urls, grid, kendoWindowService) {
        var vm = this;

        vm.showMergeWindow = showMergeWindow;

        function showMergeWindow(targetWellId) {
            var window = kendoWindowService.open({
                url: "@Url.Content("~/AngularTemplates/mergeWell.html")",
                controller: "MergeWellController as mwCtrl",
                scopeParams: {
                    targetWellId: targetWellId,
                    _urls: _urls
                },
                title: "Merge Wells"
            });

            window
                .whenCompleted()
                .then(function () {
                    grid.dataSource.read();
                });
        }
    }

    var urls = {
        getMergeInfo: "@Url.Action("GetMergeInfo", "Well")",
        leaseEndpoint: "@Url.Action("GetFiltered", "Lease")",
        mergeWells: "@Url.Action("Merge", "Well")",
        wellEndpoint: "@Url.Action("GetFiltered", "Well")"
    };

    function MergeWells_Click(e) {
        var row = $(e.target).closest("tr");
        var grid = row.closest(".k-grid").data("kendoGrid");
        var well = grid.dataItem(row);

        var injector = migrate.getAceInjector();
        var ctrl = injector.instantiate(WellController, { _urls: urls, grid: grid });

        ctrl.showMergeWindow(well.WellID);
    }

    function Well_Save(e) {
        if (e.model.APINumberRequired && isBlank(e.model.APINumber)) {
            e.preventDefault();

            var dialogHtml = $("#APINumberDialog").html().trim();
            var contentContainer = $(dialogHtml);
            kendo.bind(contentContainer, e.model);

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
                                e.model.set("IgnoreNoAPINumber", true);
                                $("#WellLocations").data("kendoGrid").saveChanges();
                            }

                            dialog.data("kendoWindow").close();
                        })
                        .end()
        }
    }

    function isBlank(str) {
        return (!str || /^\s*$/.test(str));
    }
</script>

<script id="APINumberDialog" type="text/x-kendo-template">
    <p class="dialog-title">Wells for <span data-bind="text: CustomerName" /> require an API number. Do you want to save without an API Number anyway?</p>

    <button class="dialog-dont-save k-button">Don't save</button>
    <button class="dialog-save-anyway k-button">Save without API Number</button>
</script>

<div ng-app="acePump.backOffice"></div>

<p>
    @Html.ActionKendoButton("Go to Leases", "Index", "Lease")

    @Html.ActionKendoButton("Add New Well", "Create")
</p>


@(Html.Kendo().Grid(Of WellGridRowModel)() _
                                                                    .Name("WellLocations") _
                                                                    .Events(Sub(e) e.Save("Well_Save")) _
                                                                    .Filterable() _
                                                                    .Sortable() _
                                                                    .Pageable() _
                                                                    .Columns(Sub(c)
                                                                                 c.Bound(Function(well) well.WellID).Width(40).Title("Well Location ID")
                                                                                 c.Bound(Function(well) well.LeaseID).Width(50).Title("Lease ID")
                                                                                 c.Bound(Function(well) well.Lease).Title("Lease Name")
                                                                                 c.Bound(Function(well) well.WellNumber).Title("Well #")
                                                                                 c.Bound(Function(well) well.APINumber).Title("API Number")
                                                                                 c.Bound(Function(well) well.IgnoreNoAPINumber).Hidden(True)
                                                                                 c.Bound(Function(well) well.APINumberRequired).Hidden(True)
                                                                                 c.Bound(Function(well) well.CustomerID).Width(50).Title("Current Customer ID")
                                                                                 c.Bound(Function(well) well.CustomerName).Title("Customer Name")
                                                                                 c.Bound(Function(well) well.Inactive) _
                         .ClientTemplate("<input type=""checkbox"" disabled=""disabled"" #= Inactive ? checked=""checked"" : """" # />") _
                         .Title("Inactive") _
                         .Filterable(Function(filterable) filterable.Messages(Function(m) m.IsFalse("active")) _
                                                                    .Messages(Function(m) m.IsTrue("inactive")) _
                                                                    .Messages(Function(m) m.Info("Show wells that are")))
                                                                                 c.Template(Function(well) "Shop").Title("Added By Shop")
                                                                                 c.Command(Sub(com)
                                                                                               com.Edit()
                                                                                               com.Custom("merge").Text("Merge").Click("MergeWells_Click")
                                                                                           End Sub)
                                                                             End Sub) _
                                                                    .DataSource(Sub(dataSource)
                                                                                    dataSource _
                                                                        .Ajax() _
                                                                        .Model(Sub(model)
                                                                                   model.Id(Function(id) id.WellID)
                                                                                   model.Field(Function(id) id.WellID).Editable(False)
                                                                                   model.Field(Function(id) id.Lease).Editable(False)
                                                                               End Sub) _
                                                                        .Read("List", "Well") _
                                                                        .Update("Edit", "Well")
                                                                                End Sub)
)

<script type="text/javascript" src="@Url.Content("~/app/js/mvc.wellApp.min.js")"></script>