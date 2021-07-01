@ModelType AcePump.Web.Areas.Customers.Models.DeliveryTicketViewModel
    
@Imports AcePump.Web.areas.customers.models
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details of Delivery Ticket: @Model.DeliveryTicketID</h2>

<script type="text/javascript">

    function LineItemsGrid_AdditionalData() {
         return {
             deliveryTicketId: @Model.DeliveryTicketID
         };
     }

</script>

<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/Ticket.css")" />

<div class="customer-ticket-box">    
        <div class="display-label">Delivery Ticket # </div>   <div class="display-field"> @Model.DeliveryTicketID</div>
        <br />
        <div class="display-label">Date </div>                <div class="display-field">@String.Format("{0:d}", Model.TicketDate) </div>
        <hr />
        <div class="display-label">Lease & Well </div>        <div class="display-field">@Model.LeaseAndWell</div>
        <br />
        <div class="display-label">Hold Down </div>           <div class="display-field">@Model.HoldDown</div>
</div>
<div class="customer-ticket-link">
@Html.ActionKendoButton("View Repair Ticket", "RepairDetails", "DeliveryTicket", New With {.Id = Model.DeliveryTicketID.ToString})
</div>


<div class="clear"></div>

<div class="display-label"> Template: </div>                
<div class="display-field"> @Model.PumpDispatchedTemplateVerbose </div>

                <table class="ticket-Invoice-table">
                    <tr>
                        <td>
                            <div class="display-label">BARREL</div>
                        </td>
                        <td colspan="3">
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvBarrel)</div>
                        </td>
                        <td>
                            <div class="display-label">PLUNGER</div>
                        </td>
                        <td  colspan="3">
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvPlunger)</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="display-label">SV CAGES</div>
                        </td>
                        <td>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvSVCages)</div>
                        </td>
                        <td>
                            <div class="display-label">DV CAGES</div>
                        </td>
                        <td>
                             <div class="display-field">@Html.DisplayFor(Function(model) model.InvDVCages)</div>
                        </td>
                        <td>
                            <div class="display-label">TV CAGES</div>
                        </td>
                        <td>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvPTVCages)</div>
                        </td>
                        <td>
                            <div class="display-label">DV CAGES</div>
                        </td>
                        <td>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvPDVCages)</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="display-label">SV SEATS</div>
                        </td>
                        <td>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvSVSeats)</div>
                        </td>
                        <td>
                            <div class="display-label">DV SEATS</div>
                        </td>
                        <td>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvDVSeats)</div>
                        </td>
                        <td>
                            <div class="display-label">TV SEATS</div>
                        </td>
                        <td>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvPTVSeats)</div>
                        </td>
                        <td> 
                            <div class="display-label">DV SEATS</div>
                        </td>
                        <td>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvPDVSeats)</div>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <div class="display-label">SV BALLS</div>
                        </td>
                        <td>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvSVBalls)</div>
                        </td>
                        <td>
                            <div class="display-label">DV BALLS</div>
                        </td>
                        <td>
                             <div class="display-field">@Html.DisplayFor(Function(model) model.InvDVBalls)</div>
                        </td>
                        <td>
                            <div class="display-label">TV BALLS</div>
                         </td>
                        <td>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvPTVBalls)</div>
                        </td>
                        <td>
                            <div class="display-label">DV BALLS</div>
                        </td>
                        <td>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvPDVBalls)</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="display-label">HOLD DOWN</div>
                        </td>
                        <td colspan="3">
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvHoldDown)</div>
                        </td>
                        <td>
                            <div class="display-label">ROD GUIDE</div>
                        </td>
                        <td colspan="3">
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvRodGuide)</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="display-label">VALVE ROD</div>
                        </td>
                        <td colspan="3">
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvValveRod)</div>
                        </td>
                        <td>
                            <div class="display-label">TYPE BALL & SEAT</div>
                        </td>
                        <td colspan="3">
                            <div class="display-field">@Html.DisplayFor(Function(model) model.InvTypeBallandSeat)</div>
                        </td>
                    </tr>
                </table>


<br />
<br />
<table class="customer-ticket-ship-table">
<tr>
    <th>SHIP VIA</th>
    <th>P.O NUMBER</th>
    <th>ORDER DATE</th>
    <th>SHIP DATE</th>
    <th>PUMP OUT</th>
    <th>REPAIRED</th>
    <th>ORDERED BY</th>
    <th>LAST PULL</th>
    <th>STROKE</th>
</tr>
<tr>
    <td>@Html.DisplayFor(Function(model) model.ShipVia)</td>
    <td>@Html.DisplayFor(Function(model) model.PONumber)</td>
    <td>@Html.DisplayFor(Function(model) model.OrderDate)</td>
    <td>@Html.DisplayFor(Function(model) model.ShipDate)</td>
    <td>@Html.DisplayFor(Function(model) model.PumpOutNumber)</td>
    <td>@Html.DisplayFor(Function(model) model.PumpRepairedNumber)</td>
    <td>@Html.DisplayFor(Function(model) model.OrderedBy)</td>
    <td>@Html.DisplayFor(Function(model) model.LastPull)</td>
    <td>@Html.DisplayFor(Function(model) model.Stroke)</td>
</tr>
</table>
<br />
<br />

@(Html.Kendo().Grid(Of LineItemsGridRowViewModel)() _
    .Name("LineItemsGrid") _
            .Columns(Sub(c)
                             c.Bound(Function(t) t.Quantity).Title("QTY")
                             c.Bound(Function(t) t.Sort).Visible(False)
                             c.Bound(Function(t) t.ItemNumber).Title("ITEM NUMBER")
                             c.Bound(Function(t) t.Description).Title("DESCRIPTION")
                     End Sub) _
                .DataSource(Sub(dataSource)
                                    dataSource _
                                    .Ajax() _
                                    .ServerOperation(False) _
                                    .Sort(Function(s) s.Add("Sort")) _
                                    .Model(Sub(model) model.Id(Function(id) id.LineItemID)) _
                                    .Read(Sub(read)
                                                  read.Action("List", "LineItem")
                                                  read.Data("LineItemsGrid_AdditionalData")
                                                  read.Type(HttpVerbs.Post)
                                          End Sub)
                            End Sub)
                        )


    <br />
    <div class="display-label">Notes: </div>  <div class="display-field">@Model.Notes</div>
    <br />
    <br />
    @Html.ActionKendoButton("Back to List", "Index", "DeliveryTicket")

