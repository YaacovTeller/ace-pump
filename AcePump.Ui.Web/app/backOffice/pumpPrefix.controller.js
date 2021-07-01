(function () {
    "use strict";

    angular
        .module("acePump.backOffice")
        .controller("PumpPrefixController", PumpPrefixController);

    PumpPrefixController.$inject = ["$q", "$http", "$scope", "$window"];
    function PumpPrefixController($q, $http, $scope, $window) {

        var vm = this;       

        vm.init = init;
        vm.shopLocationChange = shopLocationChange;
        vm.changeInputMode = changeInputMode;

        function init(serverValues) {
            vm.serverValues = serverValues;
            getShopLocations()
                .finally(function () {
                    $scope.apiLoading = false;
                });  
            vm.pumpNumber = parseInt(vm.serverValues.pumpNumberFromServer) || null;
            vm.pumpNumberInputMode = vm.serverValues.pumpNumberInputMode;
            if (!vm.pumpNumberInputMode) {
                vm.pumpNumberInputMode = 'auto';
                vm.pumpNumberInputModeLegend = 'Auto Number';
            }
        }

        function changeInputMode() {
            vm.pumpNumberInputMode = 'custom';
            vm.pumpNumberInputModeLegend = 'Number';
        }

        function shopLocationChange() {
            $scope.apiLoading = true;
            getNextPumpNumber()
                .finally(function () {
                    $scope.apiLoading = false;
                });
        }

        function getShopLocations() {
            $scope.apiLoading = true;
            return $http({
                method: "GET",
                url: vm.serverValues.listShopLocationsUrl,
                responseType: "json"
            })
                .then(function (httpResponse) {
                    vm.shopLocations = httpResponse.data;

                    if (vm.serverValues.shopLocationIDFromServer) {
                        for (var i = 0; i < vm.shopLocations.length; i++) {
                            if (vm.serverValues.shopLocationIDFromServer === vm.shopLocations[i].ShopLocationID) {
                                vm.selectedShop = vm.shopLocations[i];                                
                                i = vm.shopLocations.length;
                            }
                        }
                    }

                })
                .catch(function (httpError) {
                    $window.alert(
                        "Was not able to find shop locations!\n"
                    );
                })
                .finally(function () {
                    $scope.apiLoading = true;
                });
        }

        function getNextPumpNumber() {
            return $http({
                method: "GET",
                url: vm.serverValues.getNextPumpNumberUrl,
                responseType: "json",
                params: { shopLocationId: vm.selectedShop.ShopLocationID}
            })
                .then(function (httpResponse) {
                    vm.pumpNumber = httpResponse.data;
                })
                .catch(function (httpError) {
                    $window.alert(
                        "Was not able to find next pump number!\n"
                    );
                });
        }
    }
})();
