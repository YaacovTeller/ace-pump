@ModelType AcePump.Web.Areas.Customers.Models.PumpViewModel
@Imports AcePump.Web.Areas.Customers.Models
    
@Code
    ViewData("Title") = "Delivery Tickets"
    
    Dim pumpIdText As String = If(Model IsNot Nothing, Model.PumpID.ToString(), "null")
End Code

<script type="text/javascript">
     function deliveryTickets_AdditionalData() {
         return {
             pumpId: @pumpIdText
         };
     }

    function gridClick_DeliveryTicketDetails(e) {
        document.location = "@Url.Action("Details")/" + this.dataItem($(e.currentTarget).closest("tr")).DeliveryTicketID;
    }

    function gridClick_RepairTicketDetails(e) {
        document.location =  "@Url.Action("RepairDetails", "DeliveryTicket")/" + this.dataItem($(e.currentTarget).closest("tr")).DeliveryTicketID;
    }


         function deliveryTickets_dataBound() {
        //Selects all deliveryTicket buttons
        $('#deliveryTickets tbody tr .k-grid-ViewDeliveryTicket').each(function () {
            var currentDataItem = $('#deliveryTickets').data('kendoGrid').dataItem($(this).closest('tr'));                   
            //Check in the current dataItem if can show deliveryticket
            if (currentDataItem.ShowDeliveryTicket === false) {
                $(this).addClass("k-state-disabled");
                $(this).on("click", function () {
                    return false;
                });
            }                         
         });
    //Selects all RepairTickets
         $('#deliveryTickets tbody tr .k-grid-ViewRepairTicket').each(function () {
            var currentDataItem = $('#deliveryTickets').data('kendoGrid').dataItem($(this).closest('tr'));
                   
            //Check in the current dataItem if can show deliveryticket
            if (currentDataItem.ShowRepairTicket === false) {
                $(this).addClass("k-state-disabled");
                $(this).on("click", function () {
                    return false;
                });
            }                         
         });
    }


</script>

<h2>
    Delivery Tickets

    @If Model IsNot Nothing Then
        @<text>- For Pump @Model.ShopLocationPrefix@Model.PumpNumber</text>
    End If
</h2>

@(Html.Kendo().Grid(Of DeliveryTicketGridRowViewModel)() _
    .Name("deliveryTickets") _
    .Filterable() _
    .Sortable() _
    .Pageable() _
        .Events(Sub(ev) ev.DataBound("deliveryTickets_dataBound")) _
    .Columns(Sub(c)
                     c.Bound(Function(dt) dt.DeliveryTicketID).Title("Delivery Ticket ID")
                     c.Bound(Function(dt) dt.TicketDate).Format("{0:d}")
                     c.Bound(Function(dt) dt.IsClosed) _
                         .ClientTemplate("<input type=""checkbox"" disabled=""disabled"" #= IsClosed ? checked=""checked"" : """" # />") _
                         .Title("Closed") _
                         .Filterable(Function(filterable) filterable.Messages(Function(m) m.IsFalse("Open")) _
                                                                    .Messages(Function(m) m.IsTrue("Closed")) _
                                                                    .Messages(Function(m) m.Info("Show Delivery Tickets that are")))
                     c.Bound(Function(dt) dt.Lease)
                     c.Bound(Function(dt) dt.WellNumber).Title("Well")
                     c.Bound(Function(dt) dt.PumpFailedNumber).Title("Pump Failed #")
                     c.Bound(Function(dt) dt.PumpFailedDate).Format("{0:d}").Visible(False)
                     c.Bound(Function(dt) dt.PumpDispatchedNumber).Title("Pump Dispatched #")
                     c.Bound(Function(dt) dt.PumpDispatchedTemplateVerbose).Title("Pump Dispatched Template")
                     c.Command(Sub(com)
                                       com.Custom("ViewDeliveryTicket").Text("View Delivery Ticket Details").Click("gridClick_DeliveryTicketDetails")
                                       com.Custom("ViewRepairTicket").Text("View Repair Ticket Details").Click("gridClick_RepairTicketDetails")
                               End Sub)
             End Sub) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Ajax() _
                        .Model(Sub(model) model.Id(Function(id) id.DeliveryTicketID)) _
                        .Read(Sub(read)
                                      read.Action("List", "DeliveryTicket")
                                      read.Data("deliveryTickets_AdditionalData")
                                      read.Type(HttpVerbs.Post)
                              End Sub)
                End Sub)
    )
