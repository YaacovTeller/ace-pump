@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Index"
End Code

<h2>Pump Template List</h2>

<p>
    @Html.ActionKendoButton("Create New", "Create")
</p>

<script type="text/javascript">
    function viewSpecs_Click(e) {
        document.location = " @Url.Action("Details")/" + this.dataItem($(e.currentTarget).closest("tr")).PumpTemplateID;
    }

    function template_Error(e) {
            if (e.errors) {
                var message = "Errors:\n";
                $.each(e.errors, function (key, value) {
                    if ('errors' in value) {
                        $.each(value.errors, function () {
                            message += this + "\n";
                        });
                    }
                });
                alert(message);
            }
    }

    function template_Error_RequestEnd(e) {
        if (e.type === "destroy" && e.response.Errors) {
            this.cancelChanges();
        }
    }
</script>

@(Html.Kendo().Grid(Of PumpTemplateGridRowViewModel)() _
                    .Name("pumpTemplates") _
                    .Filterable() _
                    .Sortable() _
                    .Pageable() _
                    .Columns(Sub(c)
                                 c.Bound(Function(x) x.PumpTemplateID).Title("ID").Width(70)
                                 c.Bound(Function(x) x.VerboseSpec).Title("Description")
                                 c.Bound(Function(x) x.ConciseSpecificationSummary).Title("Spec")
                                 c.Command(Sub(com)
                                               com.Destroy().Text("Delete")
                                               com.Custom("View Specs").Click("viewSpecs_Click")
                                           End Sub)
                             End Sub) _
                    .DataSource(Sub(dataSource)
                                    dataSource _
                                    .Ajax() _
                                    .Events(Sub(events) events.Error("template_Error").RequestEnd("template_Error_RequestEnd")) _
                                    .Model(Sub(model) model.Id(Function(id) id.PumpTemplateID)) _
                                    .Read("List", "PumpTemplate") _
                                    .Destroy("Delete", "PumpTemplate") _
                                    .Update("Edit", "PumpTemplate") _
                                    .PageSize(100)
                                End Sub)
)

<script>
    $(document).ready(function () {
        var grid = $("#pumpTemplates").data("kendoGrid");
        var wrapper = $('<div class="k-pager-wrap k-grid-pager pagerTop"/>').insertBefore(grid.element.children("table"));
        grid.pagerTop = new kendo.ui.Pager(wrapper, $.extend({}, grid.options.pageable, { dataSource: grid.dataSource }));
        grid.element.height("").find(".pagerTop").css("border-width", "0 0 1px 0");
    });
</script>