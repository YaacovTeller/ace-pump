@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.DeliveryTicketViewModel

<script type="text/javascript">
    (function ($, window, undefined) {
        function LeaseSelector(kendoComboBox) {
            this._KendoComboBox = kendoComboBox;
        };

        LeaseSelector.prototype.Initialize = function () {
            this._AttachHandlers();
        };

        LeaseSelector.prototype._AttachHandlers = function () {
            var selector = this;

            this._KendoComboBox.bind("dataBound", function (e) {
                var leaseCombo = e.sender;
                var items = leaseCombo.items();

                for (var i = 0; i < items.length; i++) {
                    var dataItem = leaseCombo.dataItem(i);

                    if (!dataItem.CustomerHasWellsAtLease) {
                        $(items[i]).addClass("inaccessible");
                    }
                }
            });

            this._KendoComboBox.bind("change", function (e) {
                var ticketCustomer = selector._GetTicketCustomer();
                var leaseModel = {
                    LocationName: this.value(),
                    CustomerID: ticketCustomer.Id,
                    CustomerName: ticketCustomer.Name
                }

                var enteredTextExistsInDataSource = (leaseModel.LeaseName !== "" && this.selectedIndex >= 0);
                if (!enteredTextExistsInDataSource) {
                    selector.ConfirmDialog({
                        title: "Create Lease",
                        model: leaseModel,
                        dialogTemplateId: "__LeasePartial_create-confirmation",
                        callback: function (result) {
                            if (result) {
                                selector._CreateLease(leaseModel);
                            } else {
                                selector.UnselectLease();
                            }
                        }
                    });
                }
            });
        };
        
        LeaseSelector.prototype._CreateLease = function(model) {
            var selector = this;

            $.ajax({
                type: "POST",
                url: "@Url.Action("JsonCreate", "Lease")",
                data: model,
                dataType: "json",
                success: function(data, status, xhr) {
                    if(!data.Success) {
                        selector.DisplayAjaxModelError(data.Errors);
                        selector.UnselectLease();
                    } else {
                        data.Model.LeaseName = data.Model.LocationName;
                        selector._KendoComboBox.dataSource.add(data.Model);

                        selector._KendoComboBox.select(0);
                    }
                }
            });
        };
        
        LeaseSelector.prototype._GetTicketCustomer = function() {
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

        LeaseSelector.prototype.ConfirmDialog = function (options) {
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

        LeaseSelector.prototype.UnselectLease = function() {
            this._KendoComboBox.value("");
        };

        LeaseSelector.prototype.DisplayAjaxModelError = function(modelState) {
            var errorString = "";
            for(var property in modelState) {
                for(var i=0; i < modelState[property].length; i++) {
                    errorString += property + ": " + modelState[property][i];
                }
            }

            alert(errorString);
        };
        
        $(window).load(function () {
            var selector = new LeaseSelector($("#@Html.IdFor(Function(x) x.LeaseID)").data("kendoComboBox"));
            selector.Initialize();
        });        

        window.__LeaseSelectorData = function(e) {
            var typedText = $("#@Html.IdFor(Function(x) x.LeaseID)").data("kendoComboBox").input.val();

            return {
                term: typedText,
                customerId: $("#CustomerID").val(),
                includeNonCustomerLeases: typedText.length > 0
            };
        };
    })(jQuery, window);
</script>

<script id="__LeasePartial_create-confirmation" type="text/x-kendo-template">
    There is no lease called <span data-bind="text: LocationName" class="lease-identifier" />.
    Do you want to automatically create a lease called <span data-bind="text: LocationName" class="lease-identifier" />?
</script>

@(Html.Kendo().ComboBoxFor(Function(x) x.LeaseID) _
    .DataTextField("LeaseName") _
    .DataValueField("LeaseID") _
    .Text(Model.LeaseLocation) _
    .Filter(FilterType.Contains) _
    .CascadeFrom("CustomerID") _
    .AutoBind(False) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Read(Function(reader) reader.Action("GetFiltered", "Lease") _
                                                     .Data("__LeaseSelectorData") _
                                                     .Type(HttpVerbs.Post)
                                ) _
                        .ServerFiltering(True)
                End Sub)
)