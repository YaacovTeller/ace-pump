@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.DeliveryTicketViewModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
@Imports Acepump.Common
   
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details of Delivery Ticket: @Model.DeliveryTicketID</h2>

<style>
    .inaccessible {
        background-color: inherit;
    }
</style>

<script type="text/javascript">
    function displayGridFooter() {

        var totals = calculateGridFooter();

        if (totals.hasData) {
            var footerHtml;

            footerHtml = "<p class='line-items-totals'>";
            footerHtml = footerHtml + "<span class='label'>Sales Total</span>";
            footerHtml = footerHtml + "<span class='total'> " + kendo.format('{0:c}', totals.total) + "</span>";
            footerHtml = footerHtml + "<span class='clear'/>";

            footerHtml = footerHtml + "<span class='label'>Sales Tax @@ ";
            footerHtml = footerHtml +  "<span style='background-color:yellow'>" + kendo.format('{0:p3}', @Model.SalesTaxRate) + "</span></span> "
            footerHtml = footerHtml + "<span class='total'>" + kendo.format('{0:c}', totals.salesTaxTotal)+ "</span>";
            footerHtml = footerHtml + "<span class='clear'/>";

            footerHtml = footerHtml + "<span class='label'>Invoice Total </span>";
            footerHtml = footerHtml + "<span class='total'>" + kendo.format('{0:c}', totals.invoiceTotal)+ "</span></p>";

            return footerHtml;
        }
        return "";
    }
    
    function calculateLineSalesTaxAmount(data) {
        var result = 0;

        var salesTax = @Model.SalesTaxRate;

        if (data.CollectSalesTax) {
            result = data.LineTotal * salesTax;
        }
        return result;
    }

    function calculateGridFooter(){
        var total = 0;
        var salesTaxTotal = 0;
        var invoiceTotal = 0;
        var hasData = false;

        var grid = $("#LineItemsGrid").data("kendoGrid");
        var dataSource = grid.dataSource;

        var data = dataSource.data();
        for(var i=0; i < data.length; i++) {
            hasData = true;
            total = total + data[i].LineTotal;
            salesTaxTotal = salesTaxTotal + calculateLineSalesTaxAmount(data[i]);
        }

        invoiceTotal = total + salesTaxTotal;
        return {
            invoiceTotal: invoiceTotal,
            total: total,
            salesTaxTotal: salesTaxTotal,
            hasData: hasData
        };
    }

    function displaySalesTaxCheckBox(data) {
        var returnHtml = "<input type='checkbox' disabled='disabled' value='" + data.CollectSalesTax + "'";
        if (data.CollectSalesTax) { 
            returnHtml = returnHtml + " checked='checked' ";
        }
        returnHtml = returnHtml + " />";
        return returnHtml;
    }

    function ComboboxNoFreeText_Change(e){
        ensureValueChosenIsInList(this);
    }

    function ensureValueChosenIsInList(comboboBox) {
    //only allow valid items to be entered
           if (comboboBox.value() && comboboBox.selectedIndex == -1) {    
                comboboBox.value('');
                comboboBox.select(0);
                return false;
            }
        return true;
    }


    function WellID_Change(e) {
        if (ensureValueChosenIsInList(this)) {
            var value = this.value();
        }
    }

    $(document).ready(function () {
        var lblSalesTaxRate = $("#SalesTaxRateLabel");
        var lblSalesTaxRateDisplay = $("#SalesTaxRateDisplay");        

        var rate = parseFloat($("#SalesTaxRate").val());
        if (rate === null || isNaN(rate) || rate===""){
            lblSalesTaxRate.hide();
        } else {
            lblSalesTaxRate.show();
            lblSalesTaxRate.html("Sales tax rate in use: ")
            lblSalesTaxRateDisplay.html(kendo.format('{0:p3}', rate));  
        }

        showWarningLabel($("#CountySalesTaxRateID").val());
    });

    function showWarningLabel(countyID) {
        var lblWarning = $("#WarningNoCountySelectedLabel");

        if (countyID===0 || countyID === null || isNaN(countyID) || countyID===""){
            lblWarning.show();
        } else {
            lblWarning.hide();
        }
    }

    function openForQuickbooks() {
        var deliveryticketID =  @Model.DeliveryTicketID;
            $.ajax({
            dataType: "json",
            url: "@Url.Action("OpenForQuickbooks", "DeliveryTicket")",
            data: {id:deliveryticketID},
            type: "POST",
            success: function(result) {           
                document.location = "@Url.Action("Edit")/" + deliveryticketID;
            },
            error: function(data) {
            }           
        });
    }

    function openCustomerForQuickbooks() {
        var customerID =  @Model.CustomerID;
            $.ajax({
            dataType: "json",
            url: "@Url.Action("OpenForQuickbooks", "Customer")",
            data: {id:customerID},
            type: "POST",
            success: function(result) {           
                alert('Customer is open!');
            },
            error: function(data) {
                alert('Customer not open.');
            }           
        });
    }

    function openSTRForQuickbooks() {
        var strId;
        @If Model.CountySalesTaxRateID IsNot Nothing Then
            @<text>strId = @Model.CountySalesTaxRateID   </text>
        End If
        if (strId !== undefined) {
            $.ajax({
                    dataType: "json",
                    url: "@Url.Action("OpenForQuickbooks", "CountySalesTaxRate")",
                    data: {id:strId},
                    type: "POST",
                    success: function(result) {           
                        alert('Sales Tax Rate is open!');
                    },
                    error: function(data) {
                        alert('Sales Tax Rate not open.');
                    }           
                });
        }
    }

    function markAsReadyForQuickbooks() {
        var deliveryticketID =  @Model.DeliveryTicketID;
            $.ajax({
                dataType: "json",
                url: "@Url.Action("MarkAsReadyForQuickbooks", "DeliveryTicket")",
                data: {id:deliveryticketID},
                type: "POST",
                success: function(result) {     
                    if(result.Success) {     
                        alert('Marked successfully for QuickBooks.');
                        $("#invoiceStatusText").html(result.StatusText);
                    } else {
                        alert('Could not mark the ticket as ready for QuickBooks. Please check that the ticket is closed and also not already in QuickBooks.');
                    }
                },
                error: function(data) {
                    alert('Could not mark the ticket as ready for QuickBooks. Please check that the ticket is closed and also not already in QuickBooks.');
                }           
            });
    }

    function hideUpdateQbInvoiceInputs() {
        $("#InvoiceNumberToUpdate").hide();
        $("#btnUpdateQbInvoiceNumber").hide();

    }

    function updateQbInvoiceNumber() {
        var yes = confirm('This action will set the invoice status to "In QuickBooks" and cannot be undone. Are you sure?');
        if (yes) {
            var deliveryticketID =  @Model.DeliveryTicketID;
            var existingInvoiceNumber = $("#QbInvoiceNumber").val();

            var invoiceNumberToUpdate = $("#InvoiceNumberToUpdate").val();

            if(existingInvoiceNumber === undefined || existingInvoiceNumber === null || existingInvoiceNumber === "" || isNaN(existingInvoiceNumber)) {    
                $.ajax({
                    dataType: "json",
                    url: "@Url.Action("UpdateQbInvoiceNumber", "DeliveryTicket")",
                    data: {id:deliveryticketID,
                           invoiceNumber: invoiceNumberToUpdate},
                    type: "POST",
                    success: function(result) {     
                        if(result.Success) {                             
                            $("#QbInvoiceNumber").html(result.InvoiceNumberToUpdate);
                            $("#invoiceStatusText").html(result.StatusText);
                            $("#InvoiceNumberToUpdate").val('');
                            hideUpdateQbInvoiceInputs();
                        } else {
                            alert('Could update the QuickBooks invoice number. Please check that the ticket is closed and also not already in QuickBooks, and that the invoice number does not already exist.');
                        }
                    },
                    error: function(data) {
                        alert('Could update the QuickBooks invoice number. Please check that the ticket is closed and also not already in QuickBooks, and that the invoice number does not already exist.');
                    }           
                });
            } else {
                alert('QuickBooks invoice number already exists. You cannot update the number.');  
            }
        }
    }


    $(document).ready(function () {
         $("#btnMarkReadyForQuickbooks").click(function () {
             markAsReadyForQuickbooks()
         });
     });

    $(document).ready(function () {
         $("#btnUpdateQbInvoiceNumber").click(function () {
             updateQbInvoiceNumber()
         });
     });

    $(document).ready(function () {
         $("#btnOpenForQuickbooks").click(function () {
             openForQuickbooks()
         });
         $("#btnOpenCustomerForQuickbooks").click(function () {
             openCustomerForQuickbooks()
         });
         $("#btnOpenSalesTaxRateForQuickbooks").click(function () {
             openSTRForQuickbooks()
         });
     });


</script>

<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/Ticket.css")" />

<p>
    
    @If Model.RequiresPaymentUpFront Then
        @<div class="alert alert-danger">@Model.CustomerName requires payment up front.</div>
    End If

@Html.ActionKendoButton("View Repair", "Repair", New With {.id = Model.DeliveryTicketID})
@Html.ActionKendoButton("Download PDF", "Pdf", New With {.id = Model.DeliveryTicketID})
@Html.ActionKendoButton("Download Repair PDF", "RepairPdf", New With {.id = Model.DeliveryTicketID})
@Html.ActionKendoButton("Download PDF without Prices", "PdfUnpriced", New With {.id = Model.DeliveryTicketID})
@If Not Model.IsClosed Then
    @Html.ActionKendoButton("Edit", "Edit", New With {.id = Model.DeliveryTicketID})
Else
    @<a class="k-button" href="#" id="btnMarkReadyForQuickbooks">
        <span class="k-icon  k-i-seek-s"></span>
        Mark as Ready For Quickbooks
    </a> 
End If                                                    
@Html.ActionKendoButton("Back to List", "Index")

</p>

<p>
@Using Html.BeginForm("Copy", "DeliveryTicket")
    @Html.HiddenFor(Function(x) x.DeliveryTicketID)
    @Html.HiddenFor(Function(x) x.CustomerName)
    @Html.HiddenFor(Function(x) x.CustomerID)

    @<div class="editor-label">Lease Location</div>
    @<div class="editor-field">
        @Html.Partial("_LeaseSelector")
    </div>

    @<div class="editor-label">@Html.LabelFor(Function(model) model.WellID, "Well Number")</div>
    @<div class="editor-field">
        @Html.Partial("_WellSelector", Model, New ViewDataDictionary() From {
                                   {"DoNotReassignWellsFromOtherCustomers", True},
                                   {"IncludeOnlyActiveWells", False}
                                   })
    </div>

    @<input type="submit" value="Copy Delivery Ticket" />
End Using
</p>


        <fieldset>
            <div class="ticket-section-columns-details">
                    <div class="display-label">Customer</div>
                    <div class="display-field">@Model.CustomerName</div>

                    <div class="display-label">Lease Location</div>
                    <div class="display-field"> @Model.LeaseLocation</div>

                    <div class="display-label">Well Number</div>
                    <div class="display-field">@Model.WellNumber</div>
             </div>

            <div class="ticket-section-columns-details">
                    <div class="ticket-section-column1-details">
                            <div class="display-label">Ticket Date</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.TicketDate)</div>
                            <div class="display-label">Order Date</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.OrderDate)</div>
                            <div class="display-label">Order Time</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.OrderTime)</div>
                            <div class="display-label">PO Number</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.PONumber)</div>
                    </div>
                    <div class="ticket-section-column2-details">
                            <div class="display-label">Ship Via</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.ShipVia)</div>
                            <div class="display-label">Ordered By</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.OrderedBy)</div>
                            <div class="display-label">Ship Date</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.ShipDate)</div>
                            <div class="display-label">Ship Time</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.ShipTime)</div>
                    </div>
               </div>

            <div class="ticket-section-pump-history-details">
                        <div class="ticket-history-type-title-details">Dispatched Pump</div>
                        <div class="ticket-history-pump-info-details">
                            <div class="display-label">Pump #</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.PumpDispatchedPrefix)@Html.DisplayFor(Function(model) model.PumpDispatchedNumber)</div>
                        </div>
                        <div class="ticket-history-date-details">
                            <div class="display-label">Date</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.PumpDispatchedDate)</div>
                        </div>

                        <div class="clear"></div>            

                        <div class="ticket-history-type-title-details">Pump Repaired</div>
                        <div class="ticket-history-pump-info-details">
                            <div class="display-label">Pump #</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.PumpFailedPrefix)@Html.DisplayFor(Function(model) model.PumpFailedNumber)</div>
                         </div>
                         <div class="ticket-history-date-details">
                            <div class="display-label">Date</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.PumpFailedDate)</div>
                         </div>
                         <div class="clear"></div>
                         <div class="ticket-history-additional-data-details">
                            <div class="display-label">Last Pull</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.LastPull)</div>
                        </div>
                </div>

            <div class="ticket-section-additional-data-details">
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Delivery Ticket:</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.DeliveryTicketID)</div>
                    </div>
                    <div class="clear"></div>                    
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Closed Ticket:</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.IsClosed)</div>
                    </div>
                    <div class="clear"></div>
                    <div class="ticket-additional-data-row-details">
                        <div class="display-label" style="white-space: normal;">@Html.LabelFor(Function(model) model.IsSignificantDesignChange, "Significant Design Change:")</div>
                        <div class="display-field">@Html.DisplayFor(Function(model) model.IsSignificantDesignChange)</div>
                    </div>
                    <div class="clear"></div>                    
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Reason Still Open:</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.ReasonStillOpen)</div>
                    </div>
                    <div class="clear"></div>                    
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Hold Down Type:</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.HoldDown)</div>
                    </div>
                    <div class="clear"></div>                    
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Stroke:</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.Stroke)</div>
                    </div>
                    <div class="clear"></div>                    
           </div>

           <div class="ticket-section-additional-data-details">
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Completed By:</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.CompletedBy)</div>
                    </div>
                    <div class="clear"></div>                    
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Repaired By:</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.RepairedBy)</div>
                    </div>
                    <div class="clear"></div>     
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Signed By:</div>
                            <div class="display-field">@(If(Model.DisplaySignatureDate.HasValue, Model.DisplaySignatureName & " " & Model.DisplaySignatureDate.Value.ToString("d"), ""))</div>
                    </div>
                    <div class="clear"></div>                                                       
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Quote?</div>
                            <div class="display-field">@Html.DisplayFor(Function(model) model.Quote)</div>
                    </div>
                    <div class="clear"></div>                                                       
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Quickbooks Invoice #</div>
                            <div class="display-field" id="QbInvoiceNumber">@Html.DisplayFor(Function(model) model.QuickbooksInvoiceNumber)</div>
                    </div>
                    <div class="clear"></div>                                                       
                    <div class="ticket-additional-data-row-details">
                            <div class="display-label">Invoice Status</div>
                            <div class="display-field" id="invoiceStatusText">@Html.DisplayFor(Function(model) model.InvoiceStatusText)</div>
                    </div>
                    <div class="clear"></div>
                        @If Model.IsClosed And Not Model.InvoiceStatus = AcePumpInvoiceStatuses.InQuickbooks Then
                            @<input type="text" class="k-textbox width" style="width: 110px" id="InvoiceNumberToUpdate" />
                    
                            @<a class="k-button" href="#" id="btnUpdateQbInvoiceNumber">
                                <span class="k-icon  k-i-edit-s"></span>
                                    Update QuickBooks Invoice Number
                            </a> 
                        End If                    
            </div>                    

           <div class="clear"></div>

           <table class="ticket-Invoice-table">
                        <tr>
                            <td><div class="display-label">BARREL </div></td>
                            <td colspan="3"><div class="display-field">@Html.DisplayFor(Function(model) model.InvBarrel)</div></td>
                            <td><div class="display-label">PLUNGER</div></td>
                            <td  colspan="3"><div class="display-field">@Html.DisplayFor(Function(model) model.InvPlunger)</div></td>
                        </tr>
                        <tr>
                            <td><div class="display-label">SV CAGES</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvSVCages)</div></td>
                            <td><div class="display-label">DV CAGES</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvDVCages)</div></td>
                            <td><div class="display-label">TV CAGES</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvPTVCages)</div></td>
                            <td><div class="display-label">DV CAGES</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvPDVCages)</div></td>
                        </tr>
                        <tr>
                            <td><div class="display-label">SV SEATS</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvSVSeats)</div></td>
                            <td><div class="display-label">DV SEATS</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvDVSeats)</div></td>
                            <td><div class="display-label">TV SEATS</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvPTVSeats)</div></td>
                            <td><div class="display-label">DV SEATS</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvPDVSeats)</div></td>
                        </tr>
                        <tr>                        
                            <td><div class="display-label">SV BALLS</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvSVBalls)</div></td>
                            <td><div class="display-label">DV BALLS</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvDVBalls)</div></td>
                            <td><div class="display-label">TV BALLS</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvPTVBalls)</div></td>
                            <td><div class="display-label">DV BALLS</div></td>
                            <td><div class="display-field">@Html.DisplayFor(Function(model) model.InvPDVBalls)</div></td>
                        </tr>
                        <tr>
                            <td><div class="display-label">HOLD DOWN</div></td>
                            <td colspan="3"><div class="display-field">@Html.DisplayFor(Function(model) model.InvHoldDown)</div></td>
                            <td><div class="display-label">ROD GUIDE</div></td>
                            <td colspan="3"><div class="display-field">@Html.DisplayFor(Function(model) model.InvRodGuide)</div></td>
                        </tr>
                        <tr>
                            <td><div class="display-label">VALVE ROD</div></td>
                            <td colspan="3"><div class="display-field">@Html.DisplayFor(Function(model) model.InvValveRod)</div></td>
                            <td><div class="display-label">TYPE BALL & SEAT</div></td>
                            <td colspan="3"><div class="display-field">@Html.DisplayFor(Function(model) model.InvTypeBallandSeat)</div></td>
                        </tr>
                    </table>

                <div class="clear"></div>
                <br />
                    <div class="display-label">Template ID</div>

                    <div class="display-field">@Model.PumpDispatchedTemplateID</div>
                    &nbsp; &nbsp; &nbsp;
                    <div class="display-field">@Model.PumpDispatchedConciseTemplate</div>
                <hr />

    @(Html.Kendo().Grid(Of LineItemsGridRowViewModel)() _
                    .Name("LineItemsGrid") _
                            .Columns(Sub(c)
                                         c.Bound(Function(t) t.Quantity).Title("Quantity")
                                         c.Bound(Function(t) t.SortOrder).Visible(False)
                                         c.Bound(Function(t) t.PartTemplateNumber)
                                         c.Bound(Function(t) t.Description).Title("Description")
                                         c.Bound(Function(t) t.CollectSalesTax).Title("Tax").ClientTemplate("#= displaySalesTaxCheckBox(data) #")
                                         c.Bound(Function(t) t.UnitPrice).Title("List Price").Format("{0:C}")
                                         c.Bound(Function(t) t.UnitDiscount).ClientTemplate(
                                            "# if(HasCustomerDiscount) { #" &
                                            "<span style=""color:blue"" > #= kendo.format('{0:p}', CustomerDiscount) #</span>" &
                                            "# } else { #" &
                                            "<span> #= kendo.format('{0:p}', UnitDiscount) #</span>" &
                                            "# } #")
                                         c.Bound(Function(t) t.UnitPriceAfterDiscount).Title("Unit Price").Format("{0:C}")
                                         c.Bound(Function(t) t.LineTotal).Title("Line Total").Format("{0:C}") _
                                                                        .ClientFooterTemplate("#= displayGridFooter() #") _
                                                                        .HtmlAttributes(New With {.style = "text-align:right"}) _
                                                                        .HeaderHtmlAttributes(New With {.style = "text-align:right"})
                                     End Sub) _
                                .DataSource(Sub(dataSource)
                                                dataSource _
                                                .Ajax() _
                                                .ServerOperation(False) _
                                                .Sort(Function(s) s.Add("SortOrder")) _
                                                .Aggregates(Function(a) a.Add(Function(t) t.LineTotal).Sum) _
                                                .Model(Sub(model) model.Id(Function(id) id.LineItemID)) _
                                                .Read("List", "LineItem", New With {.id = Model.DeliveryTicketID})
                                            End Sub)
                            )

          <div class="editor-label">
            @Html.LabelFor(Function(model) model.CountySalesTaxRateID, "Chosen County Sales Tax Rate:")
          </div>
          <div class="display-field">@Html.DisplayFor(Function(model) model.CountySalesTaxRateName)</div>
          <span id="WarningNoCountySelectedLabel" style="color:Red">WARNING! No County is selected.</span>

         <br />
         @Html.HiddenFor(Function(model) model.SalesTaxRate)
         @Html.HiddenFor(Function(model) model.CountySalesTaxRateID)
          <span id="SalesTaxRateLabel"></span>
          <div class="display-field" id="SalesTaxRateDisplay"></div>
          <br />
                <div class="display-label">Notes </div>
                <div class="display-field multiline">@Html.DisplayFor(Function(model) model.Notes)</div>


     </fieldset>
            <br />
            <br />

    @If Not Model.IsClosed Then
        @Html.ActionKendoButton("Edit", "Edit", New With {.id = Model.DeliveryTicketID})
    End If

    @Html.ActionKendoButton("Back to List", "Index")
