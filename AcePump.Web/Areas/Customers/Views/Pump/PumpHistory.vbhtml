@ModelType AcePump.Web.Areas.Customers.Models.PumpViewModel
@Imports AcePump.Web.Areas.Customers.Models

@Imports AcePump.Web

@Code
    ViewData("Title") = "Pump History"
    Dim pumpIdText As String = If(Model IsNot Nothing, Model.PumpID.ToString(), "null")
End Code

    
<h2>Pump History
 
    @If Model IsNot Nothing Then
        @<text>- For Pump @Model.ShopLocationPrefix@Model.PumpNumber</text>
    End If

</h2>

<script type="text/javascript">
    function gridClick_DeliveryTicket(e) {
        document.location =  "@Url.Action("Details", "DeliveryTicket")/" + this.dataItem($(e.currentTarget).closest("tr")).DeliveryTicketID;
    }

    function gridClick_RepairTicket(e) {
        document.location =  "@Url.Action("RepairDetails", "DeliveryTicket")/" + this.dataItem($(e.currentTarget).closest("tr")).DeliveryTicketID;
    }

    function HistoryGrid_additionalData() {
        return {
            pumpId: @pumpIdText
            };
    }

    function HistoryGrid_dataBound() {
        //Selects all deliveryTicket buttons
        $('#HistoryGrid tbody tr .k-grid-ViewDeliveryTicket').each(function () {
            var currentDataItem = $('#HistoryGrid').data('kendoGrid').dataItem($(this).closest('tr'));                   
            //Check in the current dataItem if can show deliveryticket
            if (currentDataItem.ShowDeliveryTicket === false) {
                $(this).addClass("k-state-disabled");
                $(this).on("click", function () {
                    return false;
                });
            }                         
         });
    }
</script>

@(Html.Kendo().Grid(Of PumpHistoryGridRowViewModel) _
        .Name("HistoryGrid") _
        .Sortable() _
        .Filterable() _
        .AutoBind(True) _
        .Pageable(Sub(pager) pager.Messages(Sub(messages) messages.Empty("This pump has no history"))) _
        .Events(Sub(ev) ev.DataBound("HistoryGrid_dataBound")) _
        .Columns(Sub(c)
                         c.Bound(Function(x) x.DeliveryTicketID)
                         c.Bound(Function(x) x.HistoryDate).Format("{0:d}")
                         c.Bound(Function(x) x.HistoryType)
                         c.Bound(Function(x) x.ShowDeliveryTicket).Visible("False")
                         c.Command(Sub(com)
                                           com.Custom("ViewDeliveryTicket").Text("View Delivery Ticket").Click("gridClick_DeliveryTicket")
                                           com.Custom("ViewRepairTicket").Text("View Repair Ticket").Click("gridClick_RepairTicket")
                                   End Sub)

                 End Sub) _
        .DataSource(Sub(dataSource)
                            dataSource _
                            .Ajax() _
                            .Read(Sub(reader)
                                          reader.Action("HistoryList", "Pump")
                                          reader.Data("HistoryGrid_additionalData")
                                          reader.Type(HttpVerbs.Post)
                                  End Sub)
                    End Sub)
)
