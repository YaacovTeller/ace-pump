@Imports AcePump.Web.areas.customers.models
@Imports AcePump.Web

@Code
    ViewData("Title") = "Your Pumps"
End Code
    
<h2>Your Pumps</h2>

<script type="text/javascript">

     function LeaseCombo_AdditionalData() {
         return {
             term: $("#LeaseCombo").data("kendoComboBox").input.val()
         };
     }
     function WellCombo_AdditionalData() {
         return {
             leaseId: $("#LeaseCombo").val(),
             term: $("#WellCombo").data("kendoComboBox").input.val()
         };
     }

     function PumpsGrid_AdditionalData() {
         return {
             leaseId: $("#LeaseCombo").val(),
             wellId: $("#WellCombo").val(),
         };
     }

     function WellCombo_Change() {
         Search();
     }

     function Search() {
         $('#pumpsGrid').data('kendoGrid').dataSource.read();
         $('#pumpsGrid').data('kendoGrid').refresh();
     }


     function ClearSearchCriteria() {
        $("#LeaseCombo").data("kendoComboBox").input.val("");
        $('#WellCombo').data("kendoComboBox").input.val("");
        $("#LeaseCombo").val("");
        $('#WellCombo').val("");

     }

     
     $(document).ready(function () {
         $("#Search").click(function () {
             Search()
         });
     });

     $(document).ready(function () {
         $("#Clear").click(function () {
             ClearSearchCriteria();
             Search();
         });
     });

    function gridClick_PumpHistory(e) {
        document.location = "@Url.Action("PumpHistory", New With {.pumpID = "__replace__"})".replace("__replace__", + this.dataItem($(e.currentTarget).closest("tr")).PumpID);
    }

    function gridClick_DeliveryTickets(e) {
        document.location =  "@Url.Action("Index", "DeliveryTicket", New With {.pumpID = "__replace__"})".replace("__replace__", + this.dataItem($(e.currentTarget).closest("tr")).PumpID);
    }
</script>

<div>
    Lease:              
        @(Html.Kendo().ComboBox() _
            .Name("LeaseCombo") _
            .DataTextField("Name") _
            .DataValueField("Id") _
            .Placeholder("All Leases") _
            .Filter(FilterType.Contains) _
            .AutoBind(False) _
            .DataSource(Sub(dataSource)
                                dataSource _
                                .Read(Sub(reader)
                                              reader _
                                                  .Action("GetFiltered", "Lease") _
                                                  .Data("LeaseCombo_AdditionalData") _
                                                  .Type(HttpVerbs.Post)
                                      End Sub) _
                                .ServerFiltering(True)
                        End Sub)
        )
    
&nbsp;
&nbsp;
    Well:              
        @(Html.Kendo().ComboBox() _
            .Name("WellCombo") _
            .DataTextField("LeaseAndWell") _
            .DataValueField("WellId") _
            .Filter(FilterType.Contains) _
            .CascadeFrom("LeaseCombo") _
            .Placeholder("All Wells") _
            .Events(Function(ev) ev.Change("WellCombo_Change")) _
            .AutoBind(False) _
            .DataSource(Sub(dataSource)
                                dataSource _
                                .Read(Sub(reader)
                                              reader _
                                                  .Action("GetFiltered", "Well") _
                                                  .Data("WellCombo_AdditionalData") _
                                                  .Type(HttpVerbs.Post)
                                      End Sub) _
                                .ServerFiltering(True)
                        End Sub)
        )
    

&nbsp;
&nbsp;

    <input type="button"  id="Search" value="Search" />
    <input type="button"  id="Clear" value="Clear" />

    </div>
    

    <br />

@(Html.Kendo().Grid(Of PumpGridRowViewModel)() _
                            .Name("pumpsGrid") _
                            .Filterable() _
                            .Sortable() _
                            .Pageable() _
                            .Columns(Sub(c)
                                         c.Bound(Function(pump) pump.ShopLocationPrefix).Width(200)
                                         c.Bound(Function(pump) pump.PumpNumber)
                                         c.Bound(Function(pump) pump.Well)
                                         c.Bound(Function(pump) pump.PumpTemplate)
                                         c.Command(Sub(com)
                                                       com.Custom("ViewPumpHistory").Text("View Pump History").Click("gridClick_PumpHistory")
                                                       com.Custom("ViewDeliveryTickets").Text("View Delivery and Repair Tickets").Click("gridClick_DeliveryTickets")
                                                   End Sub)
                                     End Sub) _
                            .DataSource(Sub(dataSource)
                                            dataSource _
                                .Ajax() _
                                .Model(Sub(model) model.Id(Function(id) id.PumpID)) _
                                .Read(Sub(read)
                                          read.Action("GridSearch", "Pump")
                                          read.Data("PumpsGrid_AdditionalData")
                                          read.Type(HttpVerbs.Post)
                                      End Sub) _
                                .Group(Function(g) g.Add(Function(p) p.Lease))
                                        End Sub)
    )   