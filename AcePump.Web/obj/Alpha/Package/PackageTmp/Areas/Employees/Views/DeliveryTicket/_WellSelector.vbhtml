@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.DeliveryTicketViewModel

<script type="text/javascript">
    (function ($, window, undefined) {
        @If ViewData.ContainsKey("DoNotReassignWellsFromOtherCustomers") AndAlso ViewData("DoNotReassignWellsFromOtherCustomers") = True Then
            @<text>var doNotReassignWellsFromOtherCustomers = true;</text>
        Else
            @<text>var doNotReassignWellsFromOtherCustomers = false;</text>
        End If

        @If ViewData.ContainsKey("IncludeOnlyActiveWells") AndAlso ViewData("IncludeOnlyActiveWells") = True Then
            @<text>var includeOnlyActiveWells = true;</text>
        Else
            @<text>var includeOnlyActiveWells = false;</text>
        End If
        function WellSelector(kendoComboBox) {
            this._KendoComboBox = kendoComboBox;
        };

        WellSelector.prototype.Initialize = function () {
            this._AttachHandlers();
        };

        WellSelector.prototype._AttachHandlers = function() {
            var selector = this;

            this._KendoComboBox.bind("dataBound", function (e) {
                var wellComboBox = e.sender;
                var items = wellComboBox.items();

                for (var i = 0; i < items.length; i++) {
                    var dataItem = wellComboBox.dataItem(i);

                    if (!dataItem.CustomerOwnsWell) {
                        $(items[i]).addClass("inaccessible");
                    }
                }
            });

            this._KendoComboBox.bind("select", function (e) {
                selector.OnWellSelected(e);
            });

            this._KendoComboBox.bind("change", function (e) {
                var ticketLease = selector._GetTicketLease();
                var ticketCustomer = selector._GetTicketCustomer();
                var wellModel = {
                    WellNumber: this.value(),
                    LeaseID: ticketLease.LeaseID,
                    LeaseName: ticketLease.LeaseName,
                    CustomerID: ticketCustomer.Id,
                    CustomerName: ticketCustomer.Name
                }

                var enteredTextExistsInDataSource = (wellModel.WellNumber !== "" && this.selectedIndex >= 0);
                if (enteredTextExistsInDataSource) {
                    selectPumpInstalled(wellModel.WellNumber);
                } else {
                    selector.ConfirmDialog({
                        title: "Create Well",
                        model: wellModel,
                        dialogTemplateId: "__WellPartial_create-confirmation",
                        callback: function(result) {
                            if(result) {
                                selector._CreateWell(wellModel);
                            } else {
                                selector.UnselectWell();
                            }
                        }
                    });
                }
            });
        };

        WellSelector.prototype._CreateWell = function(model) {
            var selector = this;

            $.ajax({
                type: "POST",
                url: "@Url.Action("JsonCreate", "Well")",
                data: model,
                dataType: "json",
                success: function(data, status, xhr) {
                    if(!data.Success) {
                        selector.DisplayAjaxModelError(data.Errors);
                        selector.UnselectWell();
                    } else {
                        selector._KendoComboBox.dataSource.add(data.Model);
                        selector._KendoComboBox.select(0);
                    }
                }
            });
        };

        WellSelector.prototype._GetTicketCustomer = function() {
            var customerIdStore = $("#@Html.IdFor(Function(x) x.CustomerID)");
            var selectedCustomer;

            if("kendoComboBox" in customerIdStore.data()) {
                var combo = customerIdStore.data("kendoComboBox");
                selectedCustomer = combo.dataItem(combo.selectedIndex);

                if(selectedCustomer === undefined) {
                    selectedCustomer = {
                        Id: customerIdStore.val(),
                        Name: combo.text(),
                        IsEditModelCustomer: true
                    };
                }

            } else {
                selectedCustomer = {
                    Id: customerIdStore.val(),
                    Name: $("#CustomerName").val(),
                    IsEditModelCustomer: false
                };
            }

            return selectedCustomer;
        };

        WellSelector.prototype._GetTicketLease = function() {
            var leaseCombo = $("#@Html.IdFor(Function(x) x.LeaseID)").data("kendoComboBox");
            var selectedLease = leaseCombo.dataItem(leaseCombo.selectedIndex);

            if(selectedLease === undefined) {
                selectedLease = {
                    LeaseID: $("#@Html.IdFor(Function(x) x.LeaseID)").val(),
                    LeaseName: leaseCombo.text(),
                    IsEditModelLease: true
                };
            }

            return selectedLease;
        };

        WellSelector.prototype.OnWellSelected = function(e) {
            var listItem = e.item;
            var dataItem = this._KendoComboBox.dataItem(listItem.index());

            if (!dataItem.CustomerOwnsWell && !doNotReassignWellsFromOtherCustomers) {
                var selector = this;

                var transferModel = this.GetTransferModel(dataItem);
                this.AskUserForWellTransfer(transferModel, function (transferRequested) {
                    if (transferRequested) {
                        selector.TransferWellToTicketCustomer(transferModel);

                        listItem.removeClass("inaccessible");
                        dataItem.CustomerOwnsWell = true;
                    } else {
                        selector.UnselectWell();
                    }
                });
            }
        };

        WellSelector.prototype.UnselectWell = function() {
            this._KendoComboBox.value("");
        };

        WellSelector.prototype.GetSelectedWell = function() {
            return this._KendoComboBox.dataItem(this._KendoComboBox.selectedIndex);
        };

        WellSelector.prototype.ConfirmDialog = function(options) {
            this._ConfirmDialog_CallbackClosure = options.callback;
            var selector = this;

            if (this._ConfirmDialog === undefined) {
                this._ConfirmDialog = $("<div>").kendoWindow({
                    resizeable: false,
                    modal: true,
                    width: 350,
                    content: { template:
                                 "<div class=\"content-container\" />" +
                                 "<button class=\"confirm-confirm k-button\">Yes</button>" +
                                 "<a href=\"\\#\" class=\"confirm-cancel\">No</a>"
                             }
                }).data("kendoWindow");

                this._ConfirmDialog.element.find(".confirm-confirm,.confirm-cancel")
                                           .click(function (e) {
                                               e.preventDefault();

                                               var result = $(this).hasClass("confirm-confirm");
                                               selector._ConfirmDialog.close();

                                               selector._ConfirmDialog_CallbackClosure(result);
                                           });
            }

            var contentContainer = this._ConfirmDialog.element.find(".content-container");
            contentContainer.html($("#" + options.dialogTemplateId).html());

            kendo.bind(contentContainer, options.model);

            this._ConfirmDialog
                .title(options.title)
                .open()
                .center();
        };

        WellSelector.prototype.AskUserForWellTransfer = function(transfer, callback) {
            this.ConfirmDialog({
                title: "Transfer Well",
                model: transfer,
                dialogTemplateId: "__WellPartial_transfer-confirmation",
                callback: callback
            });
        };

        WellSelector.prototype.GetTransferModel = function(well) {
            var ticketCustomer = this._GetTicketCustomer();

            return {
                WellID: well.WellID,
                WellNumber: well.WellNumber,
                LeaseName: well.LeaseName,
                CurrentCustomer: well.CustomerName,
                TargetCustomerID: ticketCustomer.Id,
                TargetCustomer: ticketCustomer.Name
            }
        };

        WellSelector.prototype.TransferWellToTicketCustomer = function(transferModel) {
            var selector = this;

            $.ajax({
                type: "POST",
                url: "@Url.Action("Transfer", "Well")",
                data: {wellId: transferModel.WellID, targetCustomerId: transferModel.TargetCustomerID},
                dataType: "json",
                success: function(data, status, xhr) {
                    if(!data.Success) {
                        selector.DisplayAjaxModelError(data.Errors);
                        selector.UnselectWell();
                    }
                }
            });
        };

        WellSelector.prototype.DisplayAjaxModelError = function(modelState) {
            var errorString = "";
            for(var property in modelState) {
                for(var i=0; i < modelState[property].length; i++) {
                    errorString += property + ": " + modelState[property][i];
                }
            }

            alert(errorString);
        };

        $(window).load(function(){
            var selector = new WellSelector($("#@Html.IdFor(Function(x) x.WellID)").data("kendoComboBox"));
            selector.Initialize();
        });

        window.__WellSelectorData = function(e) {
            var typedText = $("#@Html.IdFor(Function(x) x.WellID)").data("kendoComboBox").input.val();

            return {
                term: typedText,
                customerId: $("#CustomerID").val(),
                leaseId: $("#LeaseID").val(),
                includeNonCustomerWells: doNotReassignWellsFromOtherCustomers || (typedText.length > 0),
                includeOnlyActiveWells: includeOnlyActiveWells
            };
        };
    })(jQuery, window);
</script>

<style type="text/css">
    .well-identifier, .lease-identifier, .current-customer-identifier {
        color: rgb(50, 150, 50);
        font-weight: bold;
    }
    
    .target-customer-identifier {
        color: rgb(30, 100, 130);
        font-weight: bold;
    }
</style>

<script id="__WellPartial_transfer-confirmation" type="text/x-kendo-template">
    You chose well <span data-bind="text: WellNumber" class="well-identifier" /> at the
    <span data-bind="text: LeaseName" class="lease-identifier" /> lease.  That well currently belongs to
    <span data-bind="text: CurrentCustomer" class="current-customer-identifier" />, not
    <span data-bind="text: TargetCustomer" class="target-customer-identifier" />.
    Would you like to move the well to  <span data-bind="text: TargetCustomer" class="target-customer-identifier" />?
</script>

<script id="__WellPartial_create-confirmation" type="text/x-kendo-template">
    There is no well number <span data-bind="text: WellNumber" class="well-identifier" /> at the
    <span data-bind="text: LeaseName" class="lease-identifier" /> lease.  Do you want 
    to automatically create well number <span data-bind="text: WellNumber" class="well-identifier" />
    and assign it to <span data-bind="text: CustomerName" class="current-customer-identifier" />?
</script>

@(Html.Kendo().ComboBoxFor(Function(x) x.WellID) _
    .DataTextField("WellNumber") _
    .DataValueField("WellID") _
    .Text(Model.WellNumber) _
    .Filter(FilterType.Contains) _
    .CascadeFrom("LeaseID") _
    .AutoBind(False) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Read(Function(reader) reader.Action("GetFiltered", "Well") _
                                                     .Type(HttpVerbs.Post) _
                                                     .Data("__WellSelectorData")
                                ) _
                        .ServerFiltering(True)
                End Sub)
)