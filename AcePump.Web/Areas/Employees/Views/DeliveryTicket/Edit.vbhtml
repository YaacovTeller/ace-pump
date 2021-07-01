@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.DeliveryTicketViewModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
@Imports AcePump.Common

@Code
    ViewData("Title") = "Edit"
    ViewData("ContainsAngularApp") = True

End Code

<h2>Edit Delivery Ticket: @Model.DeliveryTicketID</h2>

<style type="text/css">
    .inaccessible {
        background-color: rgba(255, 0, 0, 0.5);
    }

    .line-number-input {
        width: 50px;
    }

    .new-rate {
        color: Green;
        font-weight: bold;
    }

    .old-rate {
        color: Blue;
        font-weight: bold;
    }

    .s-modal-backdrop {
        position: fixed;
        top: 0;
        left: 0;
        background: rgba(0,0,0,0.6);
        width: 100%;
        height: 100%;
        vertical-align: middle;
    }

    .s-modal-container {
        background-color: white;
        border: 1px solid black;
        width: 350px;
        margin-left: auto;
        margin-right: auto;
    }

    .s-modal-title-bar {
        background-color: #3A487C;
        color: White;
        font-weight: bold;
        padding: 4px;
    }

    .s-modal-content {
        padding: 10px;
    }

    .s-modal-buttons button {
        margin-left: 5px;
        margin-right: 5px;
    }

    .s-modal-buttons {
        text-align: center;
        padding-bottom: 4px;
    }
</style>

<script type="text/javascript">
    var view = (function($, undefined) {
        "use strict";

        var HelperClass = function() {
        };

        HelperClass.prototype.Initialize = function() {
            this._WellID = $("#WellID").data("kendoComboBox");
            this._PumpDispatchedID = $("#PumpDispatchedID").data("kendoComboBox");

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
                if(h._PumpDispatchedID.value() !== "" && h._WellID.value() === "") {
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

var createAtPosition;

    function CustomerID_AdditionalData() {
            var customerIDCombo = $("#CustomerID");

        return {
            term: $("#CustomerID").data("kendoComboBox").input.val(),
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

    function LineItems_DataBound(e) {
        var lineItemsGrid = e.sender;
        var currentViewData = lineItemsGrid.dataSource.view();

        for(var i=0; i<currentViewData.length; i++) {
            if(currentViewData[i].AddedFromRepairTicket) {
                disableEditButtonsByUid(lineItemsGrid, currentViewData[i].uid);
            }
        }

        attachNumericTextBoxToLineNumberInput(e);

        //kendo.ui.progress($("#LineItemsGrid"), false); 
    }

    function disableEditButtonsByUid(grid, uid) {
        var editButton = grid.tbody.find("tr[data-uid='" + uid + "'] .k-grid-edit");

        editButton.addClass("k-state-disabled").removeClass("k-grid-edit");
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

    function selectPumpInstalled(wellId) {
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
                            failedPumpCombobox.dataSource.add({"PumpId": result.PumpId, "PumpNumber": result.Prefix + result.PumpNumber });
                            failedPumpCombobox.select(data.length - 1);
                        }
                        failedPumpCombobox.trigger("change");
                    }
            },
            error: function(data) {
            }
        });
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
                templateCombo.text(result.pumpTemplateId);

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

   function LineItemsGrid_Error(e) {
        if (e.errors) {
            var message = "Errors:\n";
            $.each(e.errors, function (key, value) {
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n";
                    });
                }
            });
            alert(message);
        }
    }

    function swapRows(rowA, rowB) {
        if(rowA.length == 0 || rowB.length == 0) return;

        var grid = $("#LineItemsGrid").data("kendoGrid");
        var uidA = rowA.data("uid"); var uidB = rowB.data("uid");
        var defA = grid.dataSource.getByUid(uidA); var defB = grid.dataSource.getByUid(uidB);

        
        $.ajax({
            dataType: "json",
            url: "@Url.Action("Swap", "LineItem")",
            data: {firstLineItemId: defA.LineItemID,
                   secondLineItemId: defB.LineItemID},
            type: "POST",
            success: function() {
                var tmpOrder = defA.SortOrder;
                defA.SortOrder = defB.SortOrder;
                defB.SortOrder = tmpOrder;

                grid.dataSource.sort({ field: "SortOrder", dir: "asc" });
            }
        });
    }

    function moveUp_Click(e) {
        var currentRow = $(e.currentTarget).parents("tr");
        var rowAbove = currentRow.prev();

        swapRows(currentRow, rowAbove);

        e.preventDefault();
    }

    function moveDown_Click(e) {
        var currentRow = $(e.currentTarget).parents("tr");
        var rowBelow = currentRow.next();

        swapRows(currentRow, rowBelow);

        e.preventDefault();
    }

    function zeroOutSinglePrice_Click(e) {
        kendo.ui.progress($("#LineItemsGrid"), true);   
        var currentRow = $(e.currentTarget).parents("tr");
        var grid = $("#LineItemsGrid").data("kendoGrid");


        var dataItem = grid.dataItem(currentRow);

        if (dataItem === null) return 0;

        var id = dataItem.LineItemID; 

            $.ajax({
                dataType: "json",
                url: "@Url.Action("ZeroOutPrice", "LineItem")",
                data: {id: id},
                type: "POST",
                success: function (result) {
                    dataItem.UnitPrice = 0;
                    dataItem.UnitDiscount = 0;
                    dataItem.UnitPriceAfterDiscount = 0;
                    dataItem.LineTotal = 0;                                        
                    grid.refresh();
                    kendo.ui.progress($("#LineItemsGrid"), false); 
                },
                error: function (data) {
                    
                }
            });           

        e.preventDefault();  
    }

    function displayGridFooter() {
        var totals = calculateGridFooter();

        if (totals.hasData) {
            var footerHtml;
            footerHtml = "<p class='line-items-totals'>";
            footerHtml = footerHtml + "<span class='label'>Sales Total</span>";
            footerHtml = footerHtml + "<span class='total'> " + kendo.format('{0:c}', totals.total) + "</span>";
            footerHtml = footerHtml + "<span class='clear'/>";

            footerHtml = footerHtml + "<span class='label'>Sales Tax @@ ";
            footerHtml = footerHtml + "<span style='background-color:yellow'>" + kendo.format('{0:p3}', parseFloat($("#SalesTaxRate").val())) + "</span></span> "
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

        var salesTax = $("#SalesTaxRate").val();

        if (data.CollectSalesTax) {
            result = data.LineTotal * salesTax;
        }
        return result;
    }

    function calculateGridFooter(total, salesTaxTotal, invoiceTotal){
        total = 0;
        salesTaxTotal = 0;
        invoiceTotal = 0;
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

    $(document).ready(function () {
        $("#btnDeleteSignature").click(btnDeleteSignature_Click);
    });

    function btnDeleteSignature_Click () {
        $.ajax({
            dataType: "json",
            url: "@Url.Action("DeleteSignature", "DeliveryTicket")",
            data: {id: @Model.DeliveryTicketID},
            type: "POST",
            success: function(result) {
                var displaySignature = $("#DisplaySignature");
                displaySignature.html("");
            },
            error: function(data) {
            }
        });
    }

    function LineItemsGrid_Edit(e) {
        popUp = e.container;

        if (e.model.isNew()) {

            popUp.find("#Description").prop('disabled', true);
            popUp.find("#UnitPrice").data("kendoNumericTextBox").enable(false);
            popUp.find("#UnitDiscount").data("kendoNumericTextBox").enable(false);
            popUp.find("#CollectSalesTax").hide();

            if (!createAtPosition) {
                var update = $(e.container).parent().find(".k-grid-update");
                var updateAndAdd = $("<a class=\"k-button k-button-iconcontext s-update-and-add\" href=\"#\"><span class=\"k-icon k-update\"></span>Update and Add</a>");
                updateAndAdd.click(function(e) { updateAndAddRow(e); });
                update.after(updateAndAdd);
            }

        } else {
            if(e.model.HasCustomerDiscount) {
                popUp.find("#UnitDiscount").data("kendoNumericTextBox").value(e.model.CustomerDiscount);
            }
        }

        popUp.find("#PartTemplateID").data("kendoComboBox").bind("change", function(f) {
            e.model.CollectSalesTax = true;

            popUp.find("#CollectSalesTax").prop("checked", e.model.CollectSalesTax)
        });

       disableLineNumberInput(e);
    }

   // Regular Add Part
    $(document).ready(function () {
         $("#btnAddPart").click(btnAddPart_Click);
    });

    function btnAddPart_Click(e) {
        var selectedPartTemplateId = $("#partTemplateId").val();
        var quantity = $("#addPartQuantity").val();
        if (!quantity) {
            quantity = 1;
        }
        addPartToBottomOfGrid(selectedPartTemplateId, quantity);
        e.preventDefault();
    }

    function addPartToBottomOfGrid(partTemplateId, quantity) {
        var grid = $("#LineItemsGrid").data("kendoGrid");
        createAtPosition = false;

        if(partTemplateId > 0) {
            $("#lineItemIDTarget").val(0);

            var newLineItem = createLineItemKendoModel();
            newLineItem.PartTemplateID = partTemplateId;
            newLineItem.PartTemplateNumber = partTemplateId;
            newLineItem.DeliveryTicketId = @Model.DeliveryTicketID;
            newLineItem.Quantity = quantity;

            grid.dataSource.add(newLineItem);
            grid.dataSource.sync();
        } else {
            addRowToBottomOfGrid();
        }
    }

    function createLineItemKendoModel() {
        return {
            PartTemplateID: 0,
            PartTemplateNumber: "",
            DeliveryTicketId: 0,
            HasCustomerDiscount: false,
            UnitDiscount: 0
        };
    }
    function updateAndAddRow(e) {
        var grid = $("#LineItemsGrid").data("kendoGrid");
        e.preventDefault();

        grid.dataSource.one("sync", function(e) { addRowToBottomOfGrid(); });
        grid.dataSource.sync();
    }

    function addRowToBottomOfGrid() {
            $("#lineItemIDTarget").val(0);
            var grid = $("#LineItemsGrid").data("kendoGrid");
            grid.addRow();
            $(".k-grid-edit-row").appendTo("#LineItemsGrid tbody");
    }

   function disableLineNumberInput(e) {
       row = e.container;
       row.find(".line-number-input").prop('disabled', true);
   }

   // Insert at Position
   $(document).ready(function () {
       $("#btnAddAtPosition").click(btnAddAtPosition_Click);

       $("#btnAddAtEndOfList").click(function(e) {
            $("#lineItemIDTarget").val(0);
            var totalLines = $("#LineItemsGrid").data("kendoGrid").dataSource.total();
            var ixInsert = totalLines - 1;
            createAtPosition = false;

            addPart(ixInsert);

            e.preventDefault();
       });

       $("#btnZeroAllPrices").click(function (e) {
           kendo.ui.progress($("#LineItemsGrid"), true);   
           var deliveryTicketID = @Model.DeliveryTicketID;
           $.ajax({
                dataType: "json",
                url: "@Url.Action("ZeroOutAllPrices", "LineItem")",
               data: { deliveryticketId: deliveryTicketID},
                type: "POST",
                success: function (result) {
                    var grid = $("#LineItemsGrid").data("kendoGrid");
                    grid.dataSource.read();
                    grid.refresh();

                    kendo.ui.progress($("#LineItemsGrid"), false);   
                },
                error: function (data) {
                    
                }
            });           

           e.preventDefault();
       });
   });

   function btnAddAtPosition_Click(e) {
       var hasChanges = $("#LineItemsGrid").data("kendoGrid").dataSource.hasChanges();

       if (!hasChanges) {
           var spnrLineNumber = $("#addLineItemLineNumber").data("kendoNumericTextBox");
           var lineNumberZeroBased = spnrLineNumber.value();
           spnrLineNumber.value(null);

           var totalLines = $("#LineItemsGrid").data("kendoGrid").dataSource.total();

           if (lineNumberZeroBased >= totalLines) {
               lineNumberZeroBased = totalLines;
               createAtPosition = false;

           } else {
               createAtPosition = true;
           }

           lineNumberZeroBased = lineNumberZeroBased - 1;

           $("#lineItemIDTarget").val(getSortOrderIdByLineNumber(lineNumberZeroBased));

           addPart(lineNumberZeroBased);
       }
       e.preventDefault();
   }

   function getSortOrderIdByLineNumber(lineNumber) {
           if(lineNumber === -1) {
                return -1;
           }
           var grid = $("#LineItemsGrid").data("kendoGrid");
           var row = grid.table.find("tbody tr:nth(" + (lineNumber) + ")");
           var dataItem = grid.dataItem(row);

           if(dataItem === null) return 0;

           return dataItem.LineItemID;
   }

   function addPart(lineNumberZeroBased) {
       var grid = $("#LineItemsGrid").data("kendoGrid");
       var insertIndex;
       var editRowLineNumber;
       var totalLines= grid.dataSource.total()

       if (lineNumberZeroBased >= totalLines) {
           insertIndex = getDataSourceIndexOfLineNumber(totalLines);
           insertIndex += 1;
           editRowLineNumber = totalLines + 1;
       } else if (lineNumberZeroBased === -1) {
            insertIndex = -1;
            editRowLineNumber = 0;
       } else {
           insertIndex = getDataSourceIndexOfLineNumber( parseInt(lineNumberZeroBased) );
           editRowLineNumber = parseInt(lineNumberZeroBased) + 1;
       }

       grid.dataSource.insert(insertIndex + 1, { });
       grid.editRow($("#LineItemsGrid tbody tr[data-uid]:eq(" + (editRowLineNumber) + ")"));
   }

   function getDataSourceIndexOfLineNumber(lineNumber) {
       var grid = $("#LineItemsGrid").data("kendoGrid");
       var data = grid.dataItem("tbody tr[data-uid]:eq(" + (lineNumber) + ")");
       return grid.dataSource.indexOf(data);
    }

   function attachNumericTextBoxToLineNumberInput(e) {
       var grid = $("#LineItemsGrid").data("kendoGrid");
       grid.tbody.find('.line-number-input').each(function (index) {
           var box = $(this).kendoNumericTextBox({
               format: "0.",
               decimals: "0",
               step: "1",
               min: "0",
               spinners: false,
               width: "50px",
               change: lineNumberChange
           });
           box.data("kendoNumericTextBox").value(index + 1);
       })
   }

   function lineNumberChange(e) {
       var newLineNumber = this.value();
       var currentDataItem = $('#LineItemsGrid').data('kendoGrid').dataItem($(this.element).closest('tr'));
       var lineItemIDToChange = currentDataItem.LineItemID;

       var newLineNumberZeroBased = newLineNumber - 1;
       var lineItemIDTarget = getSortOrderIdByLineNumber(newLineNumberZeroBased);

       if (lineItemIDToChange !== lineItemIDTarget && lineItemIDTarget !== null) {
           updatePartPosition(lineItemIDToChange, lineItemIDTarget);
       } else {
           e.preventDefault();
       }
   }

   function updatePartPosition(lineItemIDToChange, lineItemIDTarget) {
       if(lineItemIDToChange.length == 0 || lineItemIDTarget.length == 0) return;

       $.ajax({
           dataType: "json",
           url: "@Url.Action("UpdatePosition", "LineItem")",
           data: {lineItemIDToChange: lineItemIDToChange,
                  lineItemIDTarget: lineItemIDTarget},
           type: "POST",
           success: function() {
               if (arguments[0].Success === true ) {
                   var grid = $('#LineItemsGrid').data('kendoGrid');
                   grid.dataSource.read();
               }
           }
       });
    }

    function lineItemCreate_AdditionalData(e) {
        return {
            deliveryTicketID: @Model.DeliveryTicketID,
            lineItemIDTarget: $("#lineItemIDTarget").val()
        };
    }

   function LineItemsGrid_Cancel(e) {
       var grid = $("#LineItemsGrid").data("kendoGrid");
       grid.cancelRow();
       grid.refresh();
   }

    function lookupCountySalesTaxRateForCustomer(id) {
        $.ajax({
            dataType: "json",
            url: "@Url.Action("LookupCountySalesTaxRate", "Customer")",
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

    function showWarningLabel(countyID) {
        var lblWarning = $("#WarningNoCountySelectedLabel");

        if (countyID===0 || countyID === null || isNaN(countyID) || countyID===""){
            lblWarning.show();
        } else {
            lblWarning.hide();
        }
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

        var grid = $("#LineItemsGrid").data("kendoGrid");
        if(grid != undefined) {
            grid.refresh();
        }
    }

    $(document).ready(function () {
        showWarningLabel($("#CountySalesTaxRateID").val());
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
<p>
    @Html.ActionKendoButton("View Repair", "Repair", New With {.id = Model.DeliveryTicketID})
    @Html.ActionKendoButton("Download PDF", "Pdf", New With {.id = Model.DeliveryTicketID})
    @Html.ActionKendoButton("Download PDF without Prices", "PdfUnpriced", New With {.id = Model.DeliveryTicketID})
    </  p>

    @Using Html.BeginForm()
        @Html.ValidationSummary(True)

        @Html.Hidden("lineItemIDTarget")

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
                                   .Placeholder("Start typing ...") _
                                   .Filter(FilterType.StartsWith) _
                                   .Text(Model.CustomerName) _
                                   .AutoBind(False) _
                                   .Events(Sub(e) e.Select("CustomerID_Select") _
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
                    <div class="editor-field">
                        @Html.EditorFor(Function(model) model.TicketDate)
                    @Html.ValidationMessageFor(Function(model) model.TicketDate)
                </div>

                <div class="editor-label">@Html.LabelFor(Function(model) model.OrderDate, "Order Date")</div>
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.OrderDate)
                @Html.ValidationMessageFor(Function(model) model.OrderDate)
            </div>

            <div class="editor-label">@Html.LabelFor(Function(model) model.OrderTime, "Order Time")</div>
            <div class="editor-field">
                @Html.EditorFor(Function(model) model.OrderTime)
            @Html.ValidationMessageFor(Function(model) model.OrderTime)
        </div>

        <div class="editor-label">@Html.LabelFor(Function(model) model.PONumber, "PO Number")</div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.PONumber)
        @Html.ValidationMessageFor(Function(model) model.PONumber)
    </div>
</div>

<div class="ticket-section-column2-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.ShipVia, "Ship Via")</div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.ShipVia)
    @Html.ValidationMessageFor(Function(model) model.ShipVia)
</div>

<div class="editor-label">@Html.LabelFor(Function(model) model.OrderedBy, "Ordered By")</div>
<div class="editor-field">
    @Html.EditorFor(Function(model) model.OrderedBy)
@Html.ValidationMessageFor(Function(model) model.OrderedBy)
</div>

<div class="editor-label">@Html.LabelFor(Function(model) model.ShipDate, "Ship Date")</div>
<div class="editor-field">
    @Html.EditorFor(Function(model) model.ShipDate)
@Html.ValidationMessageFor(Function(model) model.ShipDate)
</div>

<div class="editor-label">@Html.LabelFor(Function(model) model.ShipTime, "Ship Time")</div>
<div class="editor-field">
    @Html.EditorFor(Function(model) model.ShipTime)
@Html.ValidationMessageFor(Function(model) model.ShipTime)
</div>
</div>
</div>

<div class="ticket-section-pump-history-edit">
    <div class="ticket-history-type-title-edit">Dispatched Pump (original ID: @Model.PumpDispatchedID)</div>
    <div class="ticket-history-pump-info-edit">
        <div class="editor-label">@Html.LabelFor(Function(model) model.PumpDispatchedNumber, "Pump #")</div>
        <div class="editor-field">
            @(Html.Kendo().ComboBoxFor(Function(model) model.PumpDispatchedID) _
            .DataTextField("PumpNumber") _
            .DataValueField("PumpId") _
            .Filter(FilterType.Contains) _
            .Text(Model.PumpDispatchedPrefix & Model.PumpDispatchedNumber) _
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
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.PumpDispatchedDate, "Date")
    @Html.ValidationMessageFor(Function(model) model.PumpDispatchedDate)
</div>
</div>

<div class="clear"></div>
<div class="ticket-history-type-title-edit">Pump Repaired</div>
<div class="ticket-history-pump-info-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.PumpFailedID, "Pump #")</div>
    <div class="editor-field">
        @(Html.Kendo().ComboBoxFor(Function(model) model.PumpFailedID) _
            .DataTextField("PumpNumber") _
            .DataValueField("PumpId") _
            .Filter(FilterType.Contains) _
            .Text(Model.PumpFailedPrefix & Model.PumpFailedNumber) _
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
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.PumpFailedDate, "Date")
    @Html.ValidationMessageFor(Function(model) model.PumpFailedDate)
</div>
</div>
<div class="clear"></div>
<div class="ticket-history-additional-data-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.LastPull, "Last Pull")</div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.LastPull)
    @Html.ValidationMessageFor(Function(model) model.LastPull)
</div>
<div><a class="k-button" id="btnDeleteLastPull">Delete Last Pull Date</a></div>
</div>
</div>

<div class="ticket-section-additional-data-edit">
    <div class="ticket-additional-data-row-edit">
        <div class="editor-label">@Html.LabelFor(Function(model) model.DeliveryTicketID, "Delivery Ticket:")</div>
        <div class="display-field">
            @Html.DisplayFor(Function(model) model.DeliveryTicketID)
        @Html.HiddenFor(Function(model) model.DeliveryTicketID)
    </div>
</div>
<div class="clear"></div>
<div class="ticket-additional-data-row-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.IsClosed, "Closed Ticket:")</div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.IsClosed)
    @Html.ValidationMessageFor(Function(model) model.IsClosed)
</div>
</div>
<div class="clear"></div>
<div class="ticket-additional-data-row-edit">
    <div class="editor-label" style="white-space: normal;">@Html.LabelFor(Function(model) model.IsSignificantDesignChange, "Significant Design Change:")</div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.IsSignificantDesignChange)
    @Html.ValidationMessageFor(Function(model) model.IsSignificantDesignChange)
</div>
</div>
<div class="clear"></div>
<div class="ticket-additional-data-row-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.ReasonStillOpen, "Reason Still Open:")</div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.ReasonStillOpen)
    @Html.ValidationMessageFor(Function(model) model.ReasonStillOpen)
</div>
</div>
<div class="clear"></div>
<div class="ticket-additional-data-row-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.HoldDown, "Hold Down Type:") </div>
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.HoldDown, "HoldDownType")
    @Html.ValidationMessageFor(Function(model) model.HoldDown)
</div>
</div>
<div class="clear"></div>
<div class="ticket-additional-data-row-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.Stroke, "Stroke:") </div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.Stroke)
    @Html.ValidationMessageFor(Function(model) model.Stroke)
</div>
</div>
<div class="clear"></div>
</div>

<div class="ticket-section-additional-data-edit">
    <div class="ticket-additional-data-row-edit">
        <div class="editor-label">@Html.LabelFor(Function(model) model.CompletedBy, "Completed By:") </div>
        <div class="editor-field">
            @Html.TypeManagerComboBoxFor(Function(model) model.CompletedBy, "TicketCompletedBy")
        @Html.ValidationMessageFor(Function(model) model.CompletedBy)
    </div>
</div>
<div class="clear"></div>
<div class="ticket-additional-data-row-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.RepairedBy, "Repaired By:") </div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.RepairedBy)
    @Html.ValidationMessageFor(Function(model) model.RepairedBy)
</div>
</div>
<div class="clear"></div>
<div class="ticket-additional-data-row-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.DisplaySignatureName, "Signed By:") </div>
    <div class="editor-field">
        <span id="DisplaySignature">@(If(Model.DisplaySignatureDate.HasValue, Model.DisplaySignatureName & " " & Model.DisplaySignatureDate.Value.ToString("d"), ""))</span>
    </div>
    <div><a class="k-button" id="btnDeleteSignature">Delete Signature</a></div>
</div>
<div class="clear"></div>
<div class="ticket-additional-data-row-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.Quote, "Quote?") </div>
    <div class="editor-field">
        @Html.EditorFor(Function(model) model.Quote)
    @Html.ValidationMessageFor(Function(model) model.Quote)
</div>
</div>
<div class="clear"></div>
<div class="ticket-additional-data-row-edit">
    <div class="editor-label">@Html.LabelFor(Function(model) model.InvoiceStatus, "Invoice Status")</div>

    @If Model.InvoiceStatus = AcePumpInvoiceStatuses.InQuickbooks Then
        @<div class="display-field" id="invoiceStatusText">@Html.DisplayFor(Function(model) model.InvoiceStatusText)</div>
        @Html.HiddenFor(Function(model) model.InvoiceStatus)
    Else
        @<div class="editor-field">
            @(Html.Kendo().ComboBoxFor(Function(model) model.InvoiceStatus) _
                                             .DataTextField("Text") _
                                             .DataValueField("ID") _
                                             .Text(Model.InvoiceStatusText) _
                                             .Events(Function(reader) reader.Change("ComboboxNoFreeText_Change")) _
                                             .AutoBind(True) _
                                             .DataSource(Sub(dataSource)
                                                             dataSource _
                                                             .Read(Function(reader) reader.Action("InvoiceStatusList", "DeliveryTicket") _
                                                                                             .Type(HttpVerbs.Post)
                                                                     ) _
                                                             .ServerFiltering(True)
                                                         End Sub)
        )
        @Html.ValidationMessageFor(Function(model) model.InvoiceStatus)
    </div>      End If
</div>
</div>

<div class="clear"></div>

<table class="ticket-Invoice-table">
    <tr>
        <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvBarrel, "BARREL") </div></td>
        <td colspan="3">
            <div class="editor-field">
                @Html.TypeManagerComboBoxFor(Function(model) model.InvBarrel, "InvBarrelCondition")
            @Html.ValidationMessageFor(Function(model) model.InvBarrel)
        </div>
    </td>
    <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPlunger, "PLUNGER") </div></td>
    <td colspan="3">
        <div class="editor-field">
            @Html.TypeManagerComboBoxFor(Function(model) model.InvPlunger, "InvPlungerCondition")
        @Html.ValidationMessageFor(Function(model) model.InvPlunger)
    </div>
</td>
</tr>
<tr>
    <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvSVCages, "SV CAGES") </div></td>
    <td>
        <div class="editor-field">
            @Html.TypeManagerComboBoxFor(Function(model) model.InvSVCages, "InvCagesCondition")
        @Html.ValidationMessageFor(Function(model) model.InvSVCages)
    </div>
</td>
<td><div class="editor-label">@Html.LabelFor(Function(model) model.InvDVCages, "DV CAGES") </div></td>
<td>
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvDVCages, "InvCagesCondition")
    @Html.ValidationMessageFor(Function(model) model.InvDVCages)
</div>
</td>
<td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPTVCages, "TV CAGES") </div></td>
<td>
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvPTVCages, "InvCagesCondition")
    @Html.ValidationMessageFor(Function(model) model.InvPTVCages)
</div>
</td>
<td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPDVCages, "DV CAGES") </div></td>
<td>
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvPDVCages, "InvCagesCondition")
    @Html.ValidationMessageFor(Function(model) model.InvPDVCages)
</div>
</td>
</tr>
<tr>
    <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvSVSeats, "SV SEATS") </div></td>
    <td>
        <div class="editor-field">
            @Html.TypeManagerComboBoxFor(Function(model) model.InvSVSeats, "InvSeatsCondition")
        @Html.ValidationMessageFor(Function(model) model.InvSVSeats)
    </div>
</td>
<td><div class="editor-label">@Html.LabelFor(Function(model) model.InvDVSeats, "DV SEATS") </div></td>
<td>
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvDVSeats, "InvSeatsCondition")
    @Html.ValidationMessageFor(Function(model) model.InvDVSeats)
</div>
</td>
<td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPTVSeats, "TV SEATS") </div></td>
<td>
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvPTVSeats, "InvSeatsCondition")
    @Html.ValidationMessageFor(Function(model) model.InvPTVSeats)
</div>
</td>
<td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPDVSeats, "DV SEATS") </div></td>
<td>
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvPDVSeats, "InvSeatsCondition")
    @Html.ValidationMessageFor(Function(model) model.InvPDVSeats)
</div>
</td>
</tr>
<tr>
    <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvSVBalls, "SV BALLS") </div></td>
    <td>
        <div class="editor-field">
            @Html.TypeManagerComboBoxFor(Function(model) model.InvSVBalls, "InvBallsCondition")
        @Html.ValidationMessageFor(Function(model) model.InvSVBalls)
    </div>
</td>
<td><div class="editor-label">@Html.LabelFor(Function(model) model.InvDVBalls, "DV BALLS") </div></td>
<td>
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvDVBalls, "InvBallsCondition")
    @Html.ValidationMessageFor(Function(model) model.InvDVBalls)
</div>
</td>
<td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPTVBalls, "TV BALLS") </div></td>
<td>
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvPTVBalls, "InvBallsCondition")
    @Html.ValidationMessageFor(Function(model) model.InvPTVBalls)
</div>
</td>
<td><div class="editor-label">@Html.LabelFor(Function(model) model.InvPDVBalls, "DV BALLS") </div></td>
<td>
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvPDVBalls, "InvBallsCondition")
    @Html.ValidationMessageFor(Function(model) model.InvPDVBalls)
</div>
</td>
</tr>
<tr>
    <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvHoldDown, "HOLD DOWN") </div></td>
    <td colspan="3">
        <div class="editor-field">
            @Html.TypeManagerComboBoxFor(Function(model) model.InvHoldDown, "InvHoldDownCondition")
        @Html.ValidationMessageFor(Function(model) model.InvHoldDown)
    </div>
</td>
<td><div class="editor-label">@Html.LabelFor(Function(model) model.InvRodGuide, "ROD GUIDE") </div></td>
<td colspan="3">
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvRodGuide, "InvRodGuideCondition")
    @Html.ValidationMessageFor(Function(model) model.InvRodGuide)
</div>
</td>
</tr>
<tr>
    <td><div class="editor-label">@Html.LabelFor(Function(model) model.InvValveRod, "VALVE ROD") </div></td>
    <td colspan="3">
        <div class="editor-field">
            @Html.TypeManagerComboBoxFor(Function(model) model.InvValveRod, "InvValveRodCondition")
        @Html.ValidationMessageFor(Function(model) model.InvValveRod)
    </div>
</td>
<td>
    <div class="editor-label">@Html.LabelFor(Function(model) model.InvTypeBallandSeat, "TYPE BALL & SEAT") </div>
</td>
<td colspan="3">
    <div class="editor-field">
        @Html.TypeManagerComboBoxFor(Function(model) model.InvTypeBallandSeat, "InvBallsAndSeatsCondition")
    @Html.ValidationMessageFor(Function(model) model.InvTypeBallandSeat)
</div>
</td>
</tr>
</table>

<div class="clear"></div>
<div class="pump-template">
    <div class="editor-label">@Html.LabelFor(Function(model) model.PumpDispatchedTemplateID, "Template ID")</div>
    <div class="editor-field">
        @(Html.Kendo().ComboBoxFor(Function(model) model.PumpDispatchedTemplateID) _
                                              .DataTextField("PumpTemplateId") _
                                              .DataValueField("PumpTemplateId") _
                                              .Events(Function(e) e.Change("ComboboxNoFreeText_Change")) _
                                              .HtmlAttributes(New With {.style = "width:80px"}) _
                                              .AutoBind(False) _
                                              .DataSource(Sub(dataSource)
                                                              dataSource _
                                                              .Read(Function(reader) reader.Action("IDList", "PumpTemplate") _
                                                                                              .Type(HttpVerbs.Post)
                                                                      ) _
                                                              .ServerFiltering(True)
                                                          End Sub)
    )
</div>
<button type="button" id="btnUpdateTemplate">Update PumpTemplate</button>
<div class="conciseTemplateSpec">@Html.DisplayFor(Function(model) model.PumpDispatchedConciseTemplate)</div>
</div>
<hr />

<br />
<div class="part-selection-box">
    @(Html.Kendo().NumericTextBox() _
                                .Name("addPartQuantity") _
                                .Format("0.") _
                                .Decimals(0) _
                                .Step(1) _
                                .Min(0) _
                                .Placeholder("Quantity"))

    @(Html.Kendo().ComboBox() _
                                .Name("CategoryCombo") _
                                .Placeholder("Choose a Category") _
                                .DataTextField("CategoryName") _
                                .DataValueField("CategoryID") _
                                .AutoBind(False) _
                                .Filter(FilterType.StartsWith) _
                                .Events(Function(reader) reader.Select("CategoryCombo_Select") _
                                                                .Change("ComboboxNoFreeText_Change")) _
                                .DataSource(Sub(dataSource)
                                                dataSource _
                                                .Read(Function(reader) reader.Action("StartsWith", "PartCategory") _
                                                                                .Data("CategoryCombo_AdditionalData") _
                                                                                .Type(HttpVerbs.Post)
                                                        ) _
                                                .ServerFiltering(True)
                                            End Sub)
    )

    @(Html.Kendo().ComboBox() _
                                            .Name("partTemplateId") _
                                            .DataTextField("PartTemplateDescription") _
                                            .DataValueField("PartTemplateID") _
                                            .Placeholder("Choose a Part") _
                                            .Filter(FilterType.Contains) _
                                            .CascadeFrom("CategoryCombo") _
                                            .Events(Function(reader) reader.Change("ComboboxNoFreeText_Change")) _
                                            .AutoBind(False) _
                                            .DataSource(Sub(dataSource)
                                                            dataSource _
                                                            .Read(Sub(reader) reader.Action("Contains", "PartTemplate") _
                                                                                        .Data("PartsCombo_AdditionalData") _
                                                                                        .Type(HttpVerbs.Post)
                                                                    ) _
                                                            .ServerFiltering(True)
                                                        End Sub)
    )

    <a class="k-button k-button-icontext" href="#" id="btnAddPart">
        <span class="k-icon k-add"></span>Add Part
    </a>

    <script type="text/javascript">
        $(function () {
            var combobox = $('#partTemplateId').data('kendoComboBox');
            combobox.list.width(350);
        });
    </script>
</div>
<div class="clear"></div>
<br />
<br />
@(Html.Kendo().Grid(Of LineItemsGridRowViewModel)() _
                            .Name("LineItemsGrid") _
                                   .Editable(Function(e) e.CreateAt(GridInsertRowPosition.Bottom)) _
                                   .ToolBar(Sub(toolbar)
            toolbar.Template(@@<text>
        <div class="toolbar">
            <label for="addLineItemLineNumber">Add Part at line number: </label>
            <input id="addLineItemLineNumber" />
            <script>
                $("#addLineItemLineNumber").kendoNumericTextBox({
                    format: "0.",
                    decimals: 0,
                    step: 1,
                    min: 0,
                    placeholder: "first"
                });
            </script>
            <a class="k-button k-button-icontext s-add-at-position" href="#" id="btnAddAtPosition">
                <span class="k-icon k-add"></span>Add at Line
            </a>

            <a class="k-button k-button-icontext s-add-at-position" href="#" id="btnAddAtEndOfList">
                <span class="k-icon k-add"></span>Add to End of List
            </a>
            <a class="k-button k-button-icontext s-zero-all-prices" href="#" id="btnZeroAllPrices">
                <span class="k-icon k-i-cancel"></span>Zero out all prices
            </a>
        </div>
                                </text>)
                                            End Sub) _
                .Events(Sub(events)
                            events.DataBound("LineItems_DataBound")
                            events.Edit("LineItemsGrid_Edit")
                            events.Cancel("LineItemsGrid_Cancel")
                        End Sub) _
                .Columns(Sub(c)
            c.Template(@@<text></text>).ClientTemplate("<input class=""line-number-input""/>").Title("Line #").Width(30)
                             c.Bound(Function(t) t.Quantity).Title("Quantity").Width(20)
                             c.Bound(Function(t) t.PartTemplateID).ClientTemplate("#=PartTemplateNumber#")
                             c.Bound(Function(t) t.Description).Title("Description")
                             c.Bound(Function(t) t.CollectSalesTax).Title("Tax").ClientTemplate("#= displaySalesTaxCheckBox(data) #")
                             c.Bound(Function(t) t.UnitPrice).Title("List Price").Format("{0:C}").Width(120)
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
                             c.Bound(Function(t) t.SortOrder).Visible(False)
                             c.Command(Sub(command)
                                           command.Custom("moveUp").Text("/\\").Click("moveUp_Click")
                                           command.Custom("moveDown").Text("\\/").Click("moveDown_Click")
                                           command.Destroy().Text("Delete")
                                               command.Edit()
                                               command.Custom("zeroSinglePrice").Text("<span class='k-icon k-i-cancel'></span>Zero Price").Click("zeroOutSinglePrice_Click")
                                       End Sub)

                             End Sub) _
                .DataSource(Sub(dataSource)
                                dataSource _
                .Ajax() _
                .ServerOperation(True) _
                .Events(Sub(events) events.Error("LineItemsGrid_Error")
                ) _
                .Sort(Function(s) s.Add(Function(x) x.SortOrder)) _
                .Aggregates(Sub(a)
                                a.Add(Function(t) t.LineTotal).Sum()
                                a.Add(Function(t) t.SalesTaxAmount).Sum()
                            End Sub) _
                .Model(Sub(model)
                           model.Id(Function(x) x.LineItemID)
                           model.Field(Function(x) x.UnitPriceAfterDiscount).Editable(False)
                           model.Field(Function(x) x.LineTotal).Editable(False)
                           model.Field(Function(x) x.PartTemplateNumber)
                           model.Field(Function(x) x.SalesTaxAmount).Editable(False)
                       End Sub) _
                .Read("List", "LineItem", New With {.id = Model.DeliveryTicketID}) _
                .Create(Sub(create)
                            create.Action("Create", "LineItem")
                            create.Data("lineItemCreate_AdditionalData")
                        End Sub) _
                .Update("Update", "LineItem") _
                .Destroy("Remove", "LineItem")
                            End Sub))
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
                                   .Text(Model.CountySalesTaxRateName) _
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
</div>
<div class="float-left">
    <div class="editor-label-small">Notes </div>
    <br />
    <div class="multiline">
        @Html.TextAreaFor(Function(model) model.Notes)
        @Html.ValidationMessageFor(Function(model) model.Notes)

    </div>


    <div class="float-right ">
        <input type="submit" value="Save" />
    </div>
</div>


</fieldset> End Using

    <div>
        @Html.ActionKendoButton("Back to List", "Index")
    </div>

    <script type="text/javascript" src="@Url.Content(" ~/app/js/mvc.deliveryTicketApp.min.js")"></script>
