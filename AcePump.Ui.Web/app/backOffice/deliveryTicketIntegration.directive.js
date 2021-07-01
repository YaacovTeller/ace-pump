(function(){
    "use strict";

    angular
        .module("acePump.backOffice")
        .directive("deliveryTicketIntegration", deliveryTicketIntegration);
    
    function deliveryTicketIntegration() {
        return {
            restrict: "E",
            link: function(scope, element, attrs, controller) {
                var cmbCustomerId = $("#CustomerID").data("kendoComboBox");
                var nmSalesTaxRate = $("#SalesTaxRate").data("kendoNumericTextBox");
                var gridLineItems = $("#LineItemsGrid").data("kendoGrid");

                cmbCustomerId.bind("change", function(e) {
                    var custId = e.sender.value();
                    var custName = e.sender.text();
                    scope.$apply(function(scope){
                        scope.customerId = custId;
                        scope.customerName = custName;
                        scope.autoUpdateTaxRateBySelectedCustomer();
                    });
                });

                nmSalesTaxRate.bind("change", function(e) {
                    var rate = e.sender.value();
                    scope.$apply(function(scope){
                        scope.taxRate = rate;
                        scope.userEnteredTaxRate = true;
                    });
                });

                scope.$watch("taxRate", function(newVal, oldVal, scope){
                    nmSalesTaxRate.value(newVal);
                    gridLineItems.refresh();
                });
            }
        };
    }
})();