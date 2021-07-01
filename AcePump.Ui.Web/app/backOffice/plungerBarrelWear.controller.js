(function () {
    "use strict";

    angular
        .module("acePump.backOffice")
        .controller("PlungerBarrelWearController", PlungerBarrelWearController);

    PlungerBarrelWearController.$inject = [ "$http", "plungerBarrelWearSerializationService", "$q", "$scope"];
    function PlungerBarrelWearController($http, plungerBarrelWearSerializationService, $q, $scope) {

        var vm = this;
        
        vm.PlungerOrig = [];
        vm.PlungerWearRepaired = [];
        vm.PlungerOut = [];

        vm.BarrelOrig = [];
        vm.BarrelWearRepaired = [];
        vm.BarrelOut = [];

        vm.init = init;
        vm.save = save;

        function init(serverValues) {
            vm.originalPlungerBarrelWear = serverValues.originalPlungerBarrelWear;
            vm.readUrl = serverValues.readUrl;
            vm.updateUrl = serverValues.updateUrl;
            vm.deliveryTicketID = serverValues.deliveryTicketID;

            if (vm.originalPlungerBarrelWear === null || vm.originalPlungerBarrelWear === "") {
                vm.originalPlungerBarrelWear = "" + createFixedLengthEmptyString(306) + "";
            }

            var arrays = plungerBarrelWearSerializationService.deserialize(vm.originalPlungerBarrelWear, serverValues.plungerOrigFromPreviousOut, serverValues.barrelOrigFromPreviousOut);
            vm.PlungerOrig = arrays.PlungerOrig;
            vm.PlungerWearRepaired = arrays.PlungerWearRepaired;
            vm.PlungerOut = arrays.PlungerOut;
            vm.BarrelOrig = arrays.BarrelOrig;
            vm.BarrelWearRepaired = arrays.BarrelWearRepaired;
            vm.BarrelOut = arrays.BarrelOut;
        }

        function createFixedLengthEmptyString(length) {
            var wrapper = new Array(length + 1);
            return wrapper.join(" ");
        }


        function save() {
            var toSave = {
                PlungerOrig: vm.PlungerOrig,
                PlungerWearRepaired: vm.PlungerWearRepaired,
                PlungerOut: vm.PlungerOut,
                BarrelOrig: vm.BarrelOrig,
                BarrelWearRepaired: vm.BarrelWearRepaired,
                BarrelOut: vm.BarrelOut
            };

            var serialized = plungerBarrelWearSerializationService.serialize(toSave);

            return $http({
                method: "POST",
                url: vm.updateUrl,
                responseType: "json",
                data: {
                    id: vm.deliveryTicketID,
                    plungerBarrelWear: serialized
                }
            })
                .then(function (httpResponse) {
                    if ($scope.frmPlungerWear) $scope.frmPlungerWear.$setPristine();
                })
                .catch(function (httpError) {
                    $window.alert(
                            "Was not able to update the plunger barrel wear!  Please try again.\n" +
                            httpError.status + ": " + httpError.statusText
                        );                    
                });
        }
    }
})();
