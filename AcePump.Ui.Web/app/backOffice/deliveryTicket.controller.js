(function(){
    "use strict";

    angular
        .module("acePump.backOffice")
        .controller("DeliveryTicketController", DeliveryTicketController);

    DeliveryTicketController.$inject = ["$scope", "$http", "$q", "$window", "modalService"];
    function DeliveryTicketController($scope, $http, $q, $window, modalService) {
        /**
         * Queries the server for the default tax rate for this customer.
         * @param customerId
         * @returns (Promise) Resolves with rate when query completes, rejects if query errors.
         */
        $scope.queryCustomerDefaultTaxRate = function(customerId) {
            return $http({
                method: "POST",
                url: $scope.serverVars.queryCustomerTaxRateUrl,
                responseType: "json",
                data: { id: customerId }
            })
                .then(function(httpResponse){ return $q.resolve(httpResponse.data.DefaultSalesTaxRate); })
                .catch(function(httpResponse) { return $q.reject(httpResponse.data); });
        };

        /**
         * Copies the customer's default tax rate to be the rate on the ticket.
         */
        $scope.applyCustomerDefaultTaxRate = function() {
            if($scope.customerTaxRateAvailable) {
                $scope.taxRate = $scope.customerTaxRate;
                $scope.userEnteredTaxRate = false;
            }
        };

        /**
         * Sets the tax rate for the ticket based on these rules:
         *
         *      1. If no tax rate was ever typed in by the user, sets the rate to either
         *         the selected customer's default rate or the system default rate if no
         *         customer default is available (or if no customer is selected).
         *
         *      2. If the user typed in the rate and it is different than the default
         *         rate the system would have used, prompts the user to choose between the
         *         two of them.
         * @returns (Promise) Resolves when rate has been updated
         */
        $scope.autoUpdateTaxRateBySelectedCustomer = function() {
            var taxRateAvailable = $q.resolve(null);

            if("customerId" in $scope) {
                taxRateAvailable = $scope.queryCustomerDefaultTaxRate($scope.customerId);
            }

            taxRateAvailable
                .then(function(rate) {
                    var modalToUseId;
                    if(rate === null) { //no customerId or no default rate found on server
                        rate = $scope.serverVars.systemDefaultTaxRate;
                        $scope.customerTaxRateAvailable = false;
                        delete $scope.customerTaxRate;
                        modalToUseId = "choose-tax-rate-system";

                    } else {
                        $scope.customerTaxRate = rate;
                        $scope.customerTaxRateAvailable = true;
                        modalToUseId = "choose-tax-rate-customer";
                    }

                    if($scope.userEnteredTaxRate) {
                        if(rate !== $scope.taxRate) {
                            var modal = modalService.getModal(modalToUseId);
                            modal.open();
                            modal.then(function(reasonResolved) {
                                if(reasonResolved === "ok") { //customer default
                                    $scope.taxRate = rate;
                                    $scope.userEnteredTaxRate = false;
                                } // else do nothing
                            });
                        }

                    } else {
                        $scope.taxRate = rate;
                    }
                })
                .catch(function(reasonRejected) {
                    $window.alert("Sorry, the system could not automatically retrieve the default tax rate for this customer.  Please confirm what rate you want on the ticket.");
                });
        };
    }
})();