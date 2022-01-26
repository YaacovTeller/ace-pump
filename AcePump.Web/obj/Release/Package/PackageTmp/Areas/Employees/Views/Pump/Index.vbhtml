@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Index"
End Code

<h2>Pump List</h2>

<p>
    @Html.ActionKendoButton("Create New", "Create")
</p>

<script type="text/javascript">
    function viewPump_Click(e) {
        document.location = "@Url.Action("Details")/" + this.dataItem($(e.currentTarget).closest("tr")).PumpID;
    }

    function editPump_Click(e) {
        document.location = "@Url.Action("Edit")/" + this.dataItem($(e.currentTarget).closest("tr")).PumpID;
    }

    function deletePump_Click(e) {
        var id = this.dataItem($(e.currentTarget).closest("tr")).PumpID;
        if (confirm("Are you sure you want to delete this pump?")) {
            $.ajax({ 
                url: '@Url.Action("Delete", "Pump")',
                type: 'POST',            
                dataType: "json",            
                data: {'id': id},
                success: function(result) {
                    if(!result.Success) {
                        displayAjaxModelError(result.Errors);
                    } else {
                        var grid = $("#pumps").data("kendoGrid");
                        grid.dataSource.read();
                        grid.refresh();
                    }
                },
                error: function(data) {  }
            });
        }
    }

    function displayAjaxModelError(modelState) {
        var errorString = "";
        for (var property in modelState) {
            for (var i = 0; i < modelState[property].length; i++) {
                errorString += property + ": " + modelState[property][i];
            }
        }

        alert(errorString);
    };

</script>

@(Html.Kendo().Grid(Of PumpDisplayDto)() _
                                        .Name("pumps") _
                                        .Filterable() _
                                        .Sortable() _
                                        .Pageable() _
                                        .Columns(Sub(c)
                                                     c.Bound(Function(pump) pump.ShopLocationPrefix).Title("Shop")
                                                     c.Bound(Function(pump) pump.PumpNumber)
                                                     c.Bound(Function(pump) pump.Lease)
                                                     c.Bound(Function(pump) pump.Well).Title("Well")
                                                     c.Bound(Function(pump) pump.Customer)
                                                     c.Bound(Function(pump) pump.PumpTemplateID).Title("Template Number")
                                                     c.Command(Sub(com)
                                                                   com.Custom("View Pump").Click("viewPump_Click")
                                                                   com.Custom("Edit").Click("editPump_Click")
                                                                   com.Custom("Delete").Click("deletePump_Click")
                                                               End Sub)
                                                 End Sub) _
                            .DataSource(Sub(dataSource)
                                            dataSource _
                                            .Ajax() _
                                            .Model(Sub(model)
                                                       model.Id(Function(id) id.PumpID)
                                                   End Sub) _
                                            .Read("List", "Pump")
                                        End Sub)
    )