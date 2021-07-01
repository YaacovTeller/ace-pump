@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.CustomerViewModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Details"
End Code
<script type="text/javascript">
    function grid_Select(e) {
        document.location = "@Url.Action("Details", "DeliveryTicket")/" + this.dataItem($(e.currentTarget).closest("tr")).DeliveryTicketID;
    }

</script>
<h2>Details</h2>

<fieldset>
    <legend>Customer</legend>
    <ol>
        <li>
            <div class="display-label">Customer Name</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.CustomerName)
            </div>
        </li>
        <li>
            <div class="display-label">Address 1</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Address1)
            </div>
        </li>
        <li>
            <div class="display-label">Address 2</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Address2)
            </div>
        </li>
        <li>
            <div class="display-label">City</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.City)
            </div>
        </li>
        <li>
            <div class="display-label">State & Zip</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.State), @Html.DisplayFor(Function(model) model.Zip)
            </div>
        </li>
        <li>
            <div class="display-label">Phone</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Phone)
            </div>
        </li>
        <li>
            <div class="display-label">Website</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.Website)
            </div>
        </li>
        <li>
            <div class="display-label">APINumber Required</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.APINumberRequired)
            </div>
        </li>
        <li>
            <div class="display-label">County Sales Tax Rate</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.CountyName)
            </div>
        </li>
        <li>
            <div class="display-label" style="width:200px">Uses QuickBooks running invoice</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.UsesQuickbooksRunningInvoice)
            </div>
        </li>
        <li>
            <div class="display-label">Qb Invoice Class</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.QbInvoiceClassName)
            </div>
        </li>
        <li>
            <div class="display-label">Uses Inventory</div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.UsesInventory)
            </div>
        </li>

        @If User.IsInRole("AcePumpAdmin") Then
            @<li>
                <div Class="display-label">Require Payment Up Front</div>
                <div Class="display-field">
                    @Html.DisplayFor(Function(model) model.PayUpFront)
                </div>
            </li>
        End If
        
        <li>
            @Html.ActionKendoButton("Edit", "Edit", New With {.id = Model.CustomerID})
            @Html.ActionKendoButton("View Price List", "PriceList", New With {.id = Model.CustomerID})
            @Html.ActionKendoButton("Back to List", "Index")
            <a href="@Url.Content("~/CountySalesTaxRate/Index")" class="k-button">View All County Sales Tax Rates</a>
        </li>
    </ol>
</fieldset>

@(Html.Kendo().TabStrip() _
                    .Name("tabstrip") _
                    .Animation(False) _
                    .Items(Sub(tabstrip)
                               tabstrip.Add().Text("Delivery Tickets") _
                       .Selected(True) _
                       .Content(Html.Kendo().Grid(Of DeliveryTicketGridRowViewModel) _
                                                      .Name("Grid") _
                                                      .Filterable() _
                                                      .Sortable() _
                                                      .Pageable() _
                   .Columns(Sub(c)
                                c.Bound(Function(x) x.DeliveryTicketID)
                                c.Bound(Function(x) x.TicketDate).Format("{0:d}")
                                c.Bound(Function(x) x.WellNumber)
                                c.Bound(Function(x) x.LocationName)
                                c.Command(Sub(com)
                                              com.Custom("Details").Click("grid_Select")
                                          End Sub)

                            End Sub) _
                   .DataSource(Sub(dataSource)
                                   dataSource _
                       .Ajax() _
                       .Read("List", "DeliveryTicket", New With {.CustomerID = Model.CustomerID})
                               End Sub) _
                   .ToHtmlString()
                                )

                               tabstrip.Add().Text("Contacts") _
                        .Content(Html.Kendo().Grid(Of ContactDisplayDto) _
                            .Name("contactsGrid") _
                            .Columns(Sub(c)
                                         c.Bound(Function(x) x.FirstName)
                                         c.Bound(Function(x) x.LastName)
                                         c.Bound(Function(x) x.Email)
                                         c.Bound(Function(x) x.WorkPhone)
                                         c.Bound(Function(x) x.CellPhone)
                                     End Sub) _
                            .DataSource(Sub(dataSource)
                                            dataSource _
                                .Ajax() _
                                .Read(Function(reader) reader.Action("List", "Contact", New With {.CustomerID = Model.CustomerID}))
                                        End Sub) _
                            .ToHtmlString()
                        )

                               tabstrip.Add().Text("Wells") _
                        .Content(Html.Kendo().Grid(Of WellGridRowModel) _
                            .Name("wellsGrid") _
                            .Filterable() _
                            .Sortable() _
                            .Pageable() _
                            .Groupable() _
                            .Columns(Sub(c)
                                         c.Bound(Function(x) x.Lease)
                                         c.Bound(Function(x) x.WellNumber)
                                         c.Bound(Function(x) x.LeaseID)
                                     End Sub) _
                            .DataSource(Sub(dataSource)
                                            dataSource _
                                .Ajax() _
                                .Read(Function(reader) reader.Action("List", "Well", New With {.CustomerID = Model.CustomerID}))
                                        End Sub) _
                            .ToHtmlString()
                                )
                           End Sub)
)