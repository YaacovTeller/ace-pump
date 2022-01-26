@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.PumpDisplayDto
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<fieldset>
    <legend>Pump</legend>
    <ol>
        <li>
            <div class="display-label">Pump ID</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.PumpID)
            </div>
        </li>
        <li>
            <div class="display-label">Pump Number</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.ShopLocationPrefix)@Html.DisplayFor(Function(model) model.PumpNumber)
            </div>
        </li>
        <li>
            <div class="display-label">Lease Name</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Lease)
            </div>
        </li>
        <li>
            <div class="display-label">Well Number</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Well)
            </div>
        </li>
        <li>
            <div class="display-label">Current Customer</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Customer)
            </div>
        </li>
        <li>
            <div class="display-label">Pump Template</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.PumpTemplate)
            </div>
        </li>
    </ol>
</fieldset>

@(Html.Kendo().Grid(Of PumpHistoryViewModel) _
                                    .Name("grid") _
                                    .Sortable() _
                                    .Filterable() _
                                    .Pageable(Sub(pager) pager.Messages(Sub(messages) messages.Empty("This pump has no history"))) _
                                    .Columns(Sub(c)
                                                 c.Bound(Function(x) x.DeliveryTicketID).ClientTemplate("<a href=""/DeliveryTicket/Details/#= DeliveryTicketID #"">#= DeliveryTicketID #</a>")
                                                 c.Bound(Function(x) x.HistoryDate).Format("{0:d}")
                                                 c.Bound(Function(x) x.HistoryType)
                                             End Sub) _
                                    .DataSource(Sub(dataSource)
                                                    dataSource _
                                        .Ajax() _
                                        .Read("HistoryList", "Pump", New With {.PumpID = Model.PumpID})
                                                End Sub)
)

<p>
    @Html.ActionKendoButton("Back to List", "Index")
    @Html.ActionKendoButton("Edit Pump", "Edit", New With {.id = Model.PumpID})
</p>
