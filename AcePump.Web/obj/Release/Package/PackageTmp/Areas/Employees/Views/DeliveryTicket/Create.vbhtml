@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.DeliveryTicketViewModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Create"
    ViewData("ContainsAngularApp") = True

End Code

<h2>Create New Delivery Ticket</h2>

<style type="text/css">
    .inaccessible {
        background-color: rgba(255, 0, 0, 0.5);
    }
</style>

<script type="text/javascript">
    var view = (function($, undefined) {
        "use strict";

        var HelperClass = function() {
        };

        HelperClass.prototype.Initialize = function() {
            this._WellID = $("#WellID").data("kendoComboBox");
            this._AttachHandlers();
        };

        HelperClass.prototype._AttachHandlers = function() {
            var h = this;
            this._WellID.input.on("keydown",function(e){
                var TAB = 9;
                if(e.keyCode === TAB && !h._WellTextMatchesSelection()) h._AutoSelectNonWhitespaceWellMatch();
            });

            this._WellID.bind("close", function(e) {
                if(!h._WellTextMatchesSelection()) {
                    h._AutoSelectNonWhitespaceWellMatch();
                }
            });

            $("form").on("submit", function(e) {
                if(h._PumpDispatchedID && h._PumpDispatchedID.value() !== "" && h._WellID.value() === "") {
                    var dispatchWithoutWell = confirm("WARNING: You selected a pump to dispatch without specifying a well.  Are you sure you want to continue?");
                    if(!dispatchWithoutWell) {
                        e.preventDefault();
                    }
                }
            });
        };

        HelperClass.prototype._WellTextMatchesSelection = function() {
            var currentSelectedText = (this._WellID.current() === null ? "" : this._WellID.current().text().trim());
            var currentTypedText = this._WellID.text();

            return currentSelectedText === currentTypedText;
        };

        HelperClass.prototype._AutoSelectNonWhitespaceWellMatch = function() {
            var h = this;
            var targetWellNumber = this._WellID.text().trim();

            $.each(this._WellID.dataSource.data(), function(ix, item){
                if(item.WellNumber !== null && item.WellNumber.trim() === targetWellNumber) {
                    h._WellID.value(item.WellId);
                }
            });
        };

        var h = new HelperClass();
        $(window).load(function(){h.Initialize();});
        return {
            Helper: h
        };
    })(jQuery);

    function CustomerID_AdditionalData() {
            var customerIDCombo = $("#CustomerID");

        return {
            text: $("#CustomerID").data("kendoComboBox").input.val(),
            filterNoCountySet: true
        };
    }

    function LeaseID_AdditionalData() {
        return {
            customerId: $("#CustomerID").val(),
            term: $("#LeaseID").data("kendoComboBox").input.val()
        };
    }

    function CustomerID_Select(e) {
        $("#LeaseID").data("kendoComboBox").value('');
    }

    function CategoryCombo_Select(e) {
        $("#partTemplateId").data("kendoComboBox").value('');
    }

    function CategoryCombo_AdditionalData() {
        return {
            term: $("#CategoryCombo").data("kendoComboBox").input.val()
        };
    }

    function PartsCombo_AdditionalData() {
        return {
            categoryId: $("#CategoryCombo").val(),
            term: $("#partTemplateId").data("kendoComboBox").input.val()
        };
    }

    function PumpDispatched_AdditionalData() {
        return {
            term: $("#PumpDispatchedID").data("kendoComboBox").input.val()
        };
    }

    function PumpFailed_AdditionalData() {
        return {
            term:  $("#PumpFailedID").data("kendoComboBox").input.val()
        };
    }

    function TypeManagerComboBoxForChange(e) {        
        ensureValueChosenIsInList(this);
    }

    function selectPumpInstalled(wellId) {
        //this is called from within _WellSelector.vbhtml
            $.ajax({
            dataType: "json",
            url: "@Url.Action("InstalledInWell", "Pump")",
            data: {wellId: wellId},
            type: "POST",
                success: function (result) {
                    if (result.Success) {
                        var failedPumpCombobox = $("#PumpFailedID").data("kendoComboBox");
                        var data = failedPumpCombobox.dataSource.data();
                        var found;
                        for (var i = 0; i < data.length; i++) {
                            if (data[0].PumpId === result.PumpId) {
                                found = true;
                                failedPumpCombobox.select(i);
                                break;
                            }
                        }
                        if (!found) {
                            failedPumpCombobox.dataSource.add({ "PumpId": result.PumpId, "PumpNumber": result.Prefix + result.PumpNumber });
                            failedPumpCombobox.select(data.length - 1);
                        }
                        failedPumpCombobox.trigger("change");
                    }
                },
            error: function(data) {
            }
        });
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

    function PumpDispatched_Change(e) {
        if (ensureValueChosenIsInList(this)) {
            var dispatchedPumpId = this.value();

            lookupPumpTemplate(dispatchedPumpId);
        }
    }

    function PumpFailed_Change(e) {
        if ($("#PumpFailedID").val() === "") {
            $("#LastPull").data("kendoDatePicker").readonly(false);
        } else {
            if (ensureValueChosenIsInList($("#PumpFailedID").data("kendoComboBox"))) {
                setLastPullDate($("#PumpFailedID").val());
            }
        }
    }

    function lookupPumpTemplate(pumpId) {
            $.ajax({
            dataType: "json",
            url: "@Url.Action("LookupPumpTemplate", "Pump")",
            data: {pumpId: pumpId},
            type: "POST",
            success: function(result) {
                var templateCombo = $("#PumpDispatchedTemplateID").data("kendoComboBox");
                templateCombo.value(result.pumpTemplateId);

                var specDisplay = $('.conciseTemplateSpec');
                specDisplay.html(result.conciseSpec);
            },
            error: function(data) {
            }
        });
    }

    $(document).ready(function () {
        $("#btnUpdateTemplate").click(btnUpdateTemplate_Click);
    });

    function btnUpdateTemplate_Click(e) {
        var selectedDispatchedPumpId = $("#PumpDispatchedID").val();
        var selectedTemplateId = $("#PumpDispatchedTemplateID").val();

        if (selectedDispatchedPumpId == "" || selectedDispatchedPumpId == null || selectedDispatchedPumpId == undefined) {
            alert("You must have a dispatched pump selected to change the pump template.");
            $("#PumpDispatchedTemplateID").val(0);
        } else {
            updateTemplate(selectedDispatchedPumpId, selectedTemplateId);
        }
        e.preventDefault();
    }

    function updateTemplate(pumpId, templateId){
            $.ajax({
                dataType: "json",
                url: "@Url.Action("UpdateTemplate", "Pump")",
                data: {pumpId: pumpId,
                       pumpTemplateId: templateId},
                type: "POST",
                success: function(result) {
                    var specDisplay = $('.conciseTemplateSpec');
                    specDisplay.html(result.conciseSpec);
                },
                error: function(data) {
                }
            });
    }

    $(window).load(function() {
        $("#CompletedBy").data("kendoComboBox").bind("dataBound", function(e) {
            var dataSource = e.sender.dataSource;

            var data = dataSource.data();
            for(var i=0; i < data.length; i++) {
                if(isBlank(data[i].DisplayText)) {
                    dataSource.remove(data[i]);
                }
            }
        });

        configurePumpRepairedLastPull();
     });

    function clearLastPull() {
        var lastPull = $("#LastPull").data("kendoDatePicker");
        lastPull.readonly(false);
        lastPull.value("");
    }

    $(document).ready(function () {
        $("#btnDeleteLastPull").click(clearLastPull);
    });

     function isBlank(str) {
        return (!str || /^\s*$/.test(str));
    }

    function setLastPullDate(pumpId) {
            $.ajax({
            dataType: "json",
            url: "@Url.Action("LastPull", "Pump")",
            data: {id: pumpId, currentTicketDate: $("#TicketDate").val()},
            type: "POST",
            success: function(data, status, xhr) {
                    var datepicker = $("#LastPull").data("kendoDatePicker");

                    if(!data.Success) {
                        alert(data.Errors);
                        datepicker.readonly(false);
                        datepicker.value("");
                    } else {
                        datepicker.value(new Date(data.ticketFound.DateFound));
                        datepicker.readonly();
                    }
            },
            error: function(data) {
            }
        });
    }

    function configurePumpRepairedLastPull() {
        if( $("#PumpFailedID").val() !== "" &&  $("#LastPull").val() !== "") {
             $("#LastPull").data("kendoDatePicker").readonly();
        }
    }

    function CustomerID_Change(e) {
        var taxRateUrl = "@Url.Action("LookupCountySalesTaxRate", "Customer")";
        var payUpUrl = "@Url.Action("LookupPayUpFront", "Customer")";
        var $q = migrate.getNgService("$q");
        var $http = migrate.getNgService("$http");

        var newCustomerId = this.value();

        $q.when()
            .then(function () {
                if (newCustomerId === 0 || newCustomerId === null || isNaN(newCustomerId) || newCustomerId === "") {
                    return { CountySalesTaxRateID: null, CountyName: "", SaleTaxRate: null, PayUpFront: false };

                } else {
                    return $q.all({
                        taxRate: $http({
                            method: "POST",
                            url: taxRateUrl,
                            responseType: "json",
                            data: { id: newCustomerId }
                        }),
                        payUp: $http({
                            method: "POST",
                            url: payUpUrl,
                            responseType: "json",
                            data: { id: newCustomerId }
                        })
                    })
                        .then(function (httpResponses) {
                            var result = { CountySalesTaxRateID: null, CountyName: "", SaleTaxRate: null, PayUpFront: false };

                            if (httpResponses.taxRate.data.Success) angular.extend(result, httpResponses.taxRate.data);
                            result.PayUpFront = httpResponses.payUp.data.Success && httpResponses.payUp.data.PayUpFront;

                            return result;
                        });
                }
            })
            .then(function (result) {
                updateCounty(result.CountySalesTaxRateID, result.CountyName, result.SalesTaxRate);

                if (result.PayUpFront) {
                    alert("Please note that this customer must pay up front!");
                }
            });
    }

    function updateCounty(countyID, countyName, currentSalesTaxRate) {
        var lblWarning = $("#WarningNoCountySelectedLabel");

        var combo = $("#CountySalesTaxRateID").data("kendoComboBox");

        $("#CountySalesTaxRateID").val(countyID);
        combo.input.val(countyName);

        if (countyID===0 || countyID === null || isNaN(countyID) || countyID===""){
            lblWarning.show();
        } else {
            lblWarning.hide();
        }
        updateSalesTaxRate(currentSalesTaxRate);
    }


    function updateSalesTaxRate(rate) {
        var hiddenSalesTaxRate = $("#SalesTaxRate");
        var lblSalesTaxRate = $("#SalesTaxRateLabel");

        if (rate === null || isNaN(rate) || rate===""){
            lblSalesTaxRate.hide();
            hiddenSalesTaxRate.val(null);
        } else {
            lblSalesTaxRate.show();
            lblSalesTaxRate.html("Sales tax rate in use: " + kendo.format('{0:p3}', rate));
            hiddenSalesTaxRate.val(rate);
        }
    }

    $(document).ready(function () {
        updateSalesTaxRate(parseFloat($("#SalesTaxRate").val()));
    });

    function CountySalesTaxRate_Change(e) {
        var newRate=this.value();
        if (newRate===0 || newRate === null || isNaN(newRate) || newRate===""){
            updateCounty(null, "", null);
        } else {
            lookupSingleCountySalesTaxRate(newRate);
        }
    }

    function lookupSingleCountySalesTaxRate(id) {
        $.ajax({
            dataType: "json",
            url: "@Url.Action("GetRate", "CountySalesTaxRate")",
            data: {id: id},
            type: "POST",
            success: function(result) {
                if(result.Success === true) {
                    updateCounty(result.CountySalesTaxRateID, result.CountyName, result.SalesTaxRate);
                } else {
                    updateCounty(null, "", null);
                }
            }
        });
    }

   function CountySalesTaxRateID_AdditionalData() {
        return {
            term: $("#CountySalesTaxRateID").data("kendoComboBox").input.val()
        };
    }

</script>

<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/Ticket.css")" />


@Using Html.BeginForm()
    @Html.ValidationSummary(True)

    If Model.RequiresPaymentUpFront Then
        @<div class="alert alert-danger">@Model.CustomerName requires payment up front.</div>
    End If
        @<fieldset>
        <div class="ticket-section-columns-edit">
                <div class="editor-label">@Html.LabelFor(Function(model) model.CustomerID, "Customer")</div>
                <div class="editor-field">
                 @(Html.Kendo().ComboBoxFor(Function(model) model.CustomerID) _
                                          .DataTextField("Name") _
                                          .DataValueField("Id") _
                                          .MinLength(2) _
                                          .Filter(FilterType.StartsWith) _
                                          .AutoBind(False) _
                                          .Placeholder("Start typing a customer name...") _
                                          .Events(Function(reader) reader.Select("CustomerID_Select") _
                                                                         .Change("CustomerID_Change")) _
                                          .DataSource(Sub(dataSource)
                                                          dataSource _
                                                          .Read(Function(reader) reader.Action("StartsWith", "Customer") _
                                                                                         .Data("CustomerID_AdditionalData")
                                                                ) _
                                                          .ServerFiltering(True)
                                                      End Sub)
                    )                                                                 
                </div>
                <div class="editor-label">Lease Location</div>
                <div class="editor-field">
                    @Html.Partial("_LeaseSelector")
                </div>
        
                <div class="editor-label">@Html.LabelFor(Function(model) model.WellID, "Well Number")</div>
                <div class="editor-field">
                    @Html.Partial("_WellSelector", Model, New ViewDataDictionary() From {
                                     {"IncludeOnlyActiveWells", True}
                                     })
                </div>
         </div>

         <div class="ticket-section-columns-edit">
                    <div class="ticket-section-column1-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.TicketDate, "Ticket Date")</div>                       
                        <div class="editor-field">@Html.EditorFor(Function(model) model.TicketDate)
                                                  @Html.ValidationMessageFor(Function(model) model.TicketDate)
                        </div>

                        <div class="editor-label">@Html.LabelFor(Function(model) model.OrderDate, "Order Date")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.OrderDate)
                                                  @Html.ValidationMessageFor(Function(model) model.OrderDate)
                        </div>
                        <div class="editor-label">@Html.LabelFor(Function(model) model.OrderTime, "Order Time")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.OrderTime)
                                                  @Html.ValidationMessageFor(Function(model) model.OrderTime)
                        </div>

                        <div class="editor-label">@Html.LabelFor(Function(model) model.PONumber, "PO Number")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.PONumber)
                                                  @Html.ValidationMessageFor(Function(model) model.PONumber)
                        </div>
                    </div>

                    <div class="ticket-section-column2-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.ShipVia, "Ship Via")</div>                        
                        <div class="editor-field">@Html.EditorFor(Function(model) model.ShipVia)
                                                  @Html.ValidationMessageFor(Function(model) model.ShipVia)
                        </div>

                        <div class="editor-label">@Html.LabelFor(Function(model) model.OrderedBy, "Ordered By")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.OrderedBy)
                                                  @Html.ValidationMessageFor(Function(model) model.OrderedBy)
                        </div>

                        <div class="editor-label">@Html.LabelFor(Function(model) model.ShipDate, "Ship Date")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.ShipDate)
                                                  @Html.ValidationMessageFor(Function(model) model.ShipDate)
                        </div>

                        <div class="editor-label">@Html.LabelFor(Function(model) model.ShipTime, "Ship Time")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.ShipTime)
                                                  @Html.ValidationMessageFor(Function(model) model.ShipTime)
                        </div>
                    </div>
           </div>

           <div class="ticket-section-pump-history-edit">
                    <div class="ticket-history-type-title-edit">Dispatched Pump</div>
                    <div class="ticket-history-pump-info-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.PumpDispatchedID, "Pump #")</div>
                        <div class="editor-field">@(Html.Kendo().ComboBoxFor(Function(model) model.PumpDispatchedID) _
.DataTextField("PumpNumber") _
.DataValueField("PumpId") _
.Filter(FilterType.Contains) _
.Events(Function(reader) reader.Change("PumpDispatched_Change")) _
.AutoBind(False) _
.HtmlAttributes(New With {.style = "width:110px"}) _
.DataSource(Sub(dataSource)
                dataSource _
                .Read(Function(reader) reader.Action("GetFiltered", "Pump") _
                .Data("PumpDispatched_AdditionalData") _
                .Type(HttpVerbs.Post)
                ) _
                .ServerFiltering(True)
            End Sub)
                                                            )
                                                        @Html.ValidationMessageFor(Function(model) model.PumpDispatchedID)
                         </div>
                    </div>
                     <div class="ticket-history-date-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.PumpDispatchedDate, "Date")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.PumpDispatchedDate, "Date")
                                                  @Html.ValidationMessageFor(Function(model) model.PumpDispatchedDate)
                        </div>
                     </div>

                    <div class="clear"></div>            
                    <div class="ticket-history-type-title-edit">Pump Repaired</div>
                    <div class="ticket-history-pump-info-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.PumpFailedID, "Pump #")</div>
                        <div class="editor-field">@(Html.Kendo().ComboBoxFor(Function(model) model.PumpFailedID) _
        .DataTextField("PumpNumber") _
        .DataValueField("PumpId") _
        .Filter(FilterType.Contains) _
        .Events(Function(reader) reader.Change("PumpFailed_Change")) _
        .AutoBind(False) _
        .HtmlAttributes(New With {.style = "width:110px"}) _
        .DataSource(Sub(dataSource)
                        dataSource _
                        .Read(Function(reader) reader.Action("GetFiltered", "Pump") _
                                                        .Data("PumpFailed_AdditionalData") _
                                                        .Type(HttpVerbs.Post)
                                ) _
                        .ServerFiltering(True)
                    End Sub)
                                                                )
                                                        @Html.ValidationMessageFor(Function(model) model.PumpFailedID)
                         </div>
                     </div>
                     <div class="ticket-history-date-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.PumpFailedDate, "Date")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.PumpFailedDate, "Date")
                                                  @Html.ValidationMessageFor(Function(model) model.PumpFailedDate)
                        </div>
                     </div>
                    <div class="clear"></div>
                    <div class="ticket-history-additional-data-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.LastPull, "Last Pull")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.LastPull)
                                                  @Html.ValidationMessageFor(Function(model) model.LastPull)
                        </div>
                        <div><a class="k-button" id="btnDeleteLastPull">Delete Last Pull Date</a></div>
                    </div>
            </div>

            <div class="ticket-section-additional-data-edit">
                    <div class="clear"></div>
                    <div class="ticket-additional-data-row-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.IsClosed, "Close Ticket:")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.IsClosed)
                                                  @Html.ValidationMessageFor(Function(model) model.IsClosed)
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="ticket-additional-data-row-edit">
                        <div class="editor-label" style="white-space: normal;">@Html.LabelFor(Function(model) model.IsSignificantDesignChange, "Significant Design Change:")</div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.IsSignificantDesignChange)
                                                  @Html.ValidationMessageFor(Function(model) model.IsSignificantDesignChange)
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="ticket-additional-data-row-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.HoldDown, "Hold Down Type:") </div>
                        <div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.HoldDown, "HoldDownType")
                                                  @Html.ValidationMessageFor(Function(model) model.HoldDown)
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="ticket-additional-data-row-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.Stroke, "Stroke:") </div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.Stroke)
                                                  @Html.ValidationMessageFor(Function(model) model.Stroke)
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="ticket-additional-data-row-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.CompletedBy, "Completed By:") </div>
                        <div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.CompletedBy, "TicketCompletedBy")
                                                  @Html.ValidationMessageFor(Function(model) model.CompletedBy)
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="ticket-additional-data-row-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.RepairedBy, "Repaired By:") </div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.RepairedBy)
                                                  @Html.ValidationMessageFor(Function(model) model.RepairedBy)
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="ticket-additional-data-row-edit">
                        <div class="editor-label">@Html.LabelFor(Function(model) model.Quote, "Quote?") </div>
                        <div class="editor-field">@Html.EditorFor(Function(model) model.Quote)
                                                  @Html.ValidationMessageFor(Function(model) model.Quote)
                        </div>
                    </div>
            </div>

            <div class="clear"></div>

                <table class="ticket-Invoice-table">
                    <tr>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvBarrel, "BARREL") </div></td>
                        <td colspan="3"><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvBarrel, "InvBarrelCondition")
                                                                  @Html.ValidationMessageFor(Function(model) model.InvBarrel)
                                        </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPlunger, "PLUNGER") </div></td>
                        <td  colspan="3"><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvPlunger, "InvPlungerCondition")
                                                                   @Html.ValidationMessageFor(Function(model) model.InvPlunger)
                                         </div>                            
                        </td>
                    </tr>
                    <tr>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvSVCages, "SV CAGES") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvSVCages, "InvCagesCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvSVCages)
                            </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvDVCages, "DV CAGES") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvDVCages, "InvCagesCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvDVCages)
                            </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPTVCages, "TV CAGES") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvPTVCages, "InvCagesCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvPTVCages)
                            </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPDVCages, "DV CAGES") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvPDVCages, "InvCagesCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvPDVCages)
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvSVSeats, "SV SEATS") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvSVSeats, "InvSeatsCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvSVSeats)
                            </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvDVSeats, "DV SEATS") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvDVSeats, "InvSeatsCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvDVSeats)
                             </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPTVSeats, "TV SEATS") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvPTVSeats, "InvSeatsCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvPTVSeats)
                                </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPDVSeats, "DV SEATS") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvPDVSeats, "InvSeatsCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvPDVSeats)
                            </div>
                        </td>
                    </tr>
                    <tr>                        
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvSVBalls, "SV BALLS") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvSVBalls, "InvBallsCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvSVBalls)
                            </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvDVBalls, "DV BALLS") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvDVBalls, "InvBallsCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvDVBalls)
                            </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPTVBalls, "TV BALLS") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvPTVBalls, "InvBallsCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvPTVBalls)
                            </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPDVBalls, "DV BALLS") </div></td>
                        <td><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvPDVBalls, "InvBallsCondition")
                                                      @Html.ValidationMessageFor(Function(model) model.InvPDVBalls)
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvHoldDown, "HOLD DOWN") </div></td>
                        <td colspan="3"><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvHoldDown, "InvHoldDownCondition")
                                                                  @Html.ValidationMessageFor(Function(model) model.InvHoldDown)
                                        </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvRodGuide, "ROD GUIDE") </div></td>
                        <td colspan="3"><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvRodGuide, "InvRodGuideCondition")
                                                                  @Html.ValidationMessageFor(Function(model) model.InvRodGuide)
                                        </div>
                        </td>
                    </tr>
                    <tr>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvValveRod, "VALVE ROD") </div></td>
                        <td colspan="3"><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvValveRod, "InvValveRodCondition")
                                                                  @Html.ValidationMessageFor(Function(model) model.InvValveRod)
                             </div>
                        </td>
                        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvTypeBallandSeat, "TYPE BALL & SEAT") </div>
                        </td>
                        <td colspan="3"><div class="editor-field">@Html.TypeManagerComboBoxFor(Function(model) model.InvTypeBallandSeat, "InvBallsAndSeatsCondition")
                                                                  @Html.ValidationMessageFor(Function(model) model.InvTypeBallandSeat)
                                        </div>
                        </td>
                    </tr>
                </table>

            <div class="clear"></div>
            <div class="pump-template">
                    <div class="editor-label">@Html.LabelFor(Function(model) model.PumpDispatchedTemplateID, "Template ID")</div>
                    <div class="editor-field">@(Html.Kendo().ComboBoxFor(Function(model) model.PumpDispatchedTemplateID) _
.DataValueField("PumpTemplateId") _
.DataTextField("PumpTemplateId") _
.HtmlAttributes(New With {.style = "width:80px"}) _
.AutoBind(False) _
.DataSource(Sub(dataSource)
                dataSource _
                .Read(Function(reader) reader.Action("IDList", "PumpTemplate") _
                  .Type(HttpVerbs.Post)
                ) _
                .ServerFiltering(True)
            End Sub)
                                                            )</div>
                    <button type="button" id="btnUpdateTemplate">Update PumpTemplate</button>
                    <div class="conciseTemplateSpec">@Html.DisplayFor(Function(model) model.PumpDispatchedConciseTemplate)</div>
            </div>
            <div class="float-left">
                        @Html.HiddenFor(Function(model) model.SalesTaxRate)

                         <div class="editor-label">
                            @Html.LabelFor(Function(model) model.CountySalesTaxRateID, "Chosen County Sales Tax Rate")
                        </div>
                        <div class="editor-field">
                             @(Html.Kendo().ComboBoxFor(Function(model) model.CountySalesTaxRateID) _
.DataTextField("CountyName") _
.Events(Function(e) e.Change("CountySalesTaxRate_Change")) _
.DataValueField("Id") _
.MinLength(2) _
.Placeholder("Start typing ...") _
.Filter(FilterType.StartsWith) _
.AutoBind(False) _
.DataSource(Sub(dataSource)
                dataSource _
                .Read(Function(reader) reader.Action("StartsWith", "CountySalesTaxRate") _
                                               .Data("CountySalesTaxRateID_AdditionalData")
                      ) _
                .ServerFiltering(True)
            End Sub)
                                )                                                                 
                            @Html.ValidationMessageFor(Function(model) model.CountySalesTaxRateID)
                        </div>
                        <span id="WarningNoCountySelectedLabel" style="color:Red">WARNING! No County is selected.</span>

                        <span id="SalesTaxRateLabel"></span>

                        <div class="editor-label-small">Notes </div>
                        <br />
                        <div class="multiline">
                            @Html.TextAreaFor(Function(model) model.Notes)
                            @Html.ValidationMessageFor(Function(model) model.Notes)                            
                        </div>
                                    
                        <div class="float-right ">
                            <input type="submit" value="Continue to Add Line Items" />
                        </div>
            </div>
</fieldset> End Using

<div>
    @Html.ActionKendoButton("Back to List", "Index")
</div>

<script type="text/javascript" src="@Url.Content("~/app/js/mvc.deliveryTicketApp.min.js")"></script>
