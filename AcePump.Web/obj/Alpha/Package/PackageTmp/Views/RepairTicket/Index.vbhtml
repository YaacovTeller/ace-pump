@Imports AcePump.Web.Models.DisplayDtos

@Code
    ViewData("Title") = "Index"
End Code

<h2>Repair Ticket List</h2>

<p>
    @Html.ActionKendoButton("Start A New Repair Ticket", "Create")
</p>

<script type="text/javascript"">
    function grid_Select(e) {
        document.location = "@Url.Action("Details")/" + this.dataItem($(e.currentTarget).closest("tr")).RepairTicketID;
    }
    function edit_Click(e){
        document.location="@Url.Action("Edit")/" + this.dataItem($(e.currentTarget).closest("tr")).RepairTicketID;
    }
</script>

@(Html.Kendo().Grid(Of RepairTicketDisplayDto)() _
    .Name("repairTickets") _
    .Filterable() _
    .Sortable() _
    .Pageable() _
    .Columns(Sub(c)
                     c.Bound(Function(rt) rt.DeliveryTicketID)
                     c.Bound(Function(rt) rt.FailDate).Format("{0:d}")
                     c.Bound(Function(rt) rt.CustomerName)
                     c.Bound(Function(rt) rt.FailedPump)
                     c.Bound(Function(rt) rt.PumpTemplate)
                     c.Command(Sub(com)
                                       com.Custom("Details").Click("grid_Select")
                                       com.Custom("Edit").Click("edit_Click")
                                       com.Destroy().Text("Delete")
                               End Sub)
             End Sub) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Ajax() _
                        .Model(Sub(model) model.Id(Function(id) id.RepairTicketID)) _
                        .Read("List", "RepairTicket") _
                        .Destroy("Delete", "RepairTicket")
                End Sub)
    )