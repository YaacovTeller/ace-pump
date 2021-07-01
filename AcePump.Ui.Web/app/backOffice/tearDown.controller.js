(function () {
    "use strict";

    angular
        .module("acePump.backOffice")
        .controller("TearDownController", TearDownController);

    TearDownController.$inject = ["$q", "$http", "$timeout", "util", "$window"];
    function TearDownController($q, $http, $timeout, util, $window) {

        var vm = this;

        vm.tearDownItems = [];
        vm.reasonsRepaired = [];
        vm.init = init;
        vm.setResult = setResult;
        vm.saveTearDownItem = saveTearDownItem;
        vm.switchRepairMode = switchRepairMode;
        vm.verifyAllMarked = verifyAllMarked;
        vm.completeTearDown = completeTearDown;

        function init(serverValues) {
            vm.serverValues = serverValues;

            loadTearDownItems();
            if (vm.serverValues.reasonRepairedListUrl) {
                loadReasonsRepaired();
            }
        }

        function loadTearDownItems() {
            return $http({
                method: "POST",
                responseType: "json",
                url: vm.serverValues.readUrl
            })
                .then(function (httpResponse) {
                    vm.tearDownItems = httpResponse.data.Data;
                    for (var i = 0; i < vm.tearDownItems.length; i++) {
                        var item = vm.tearDownItems[i];
                        if (item.Quantity !== 1 || item.CanBeRepresentedAsAssembly || item.HasParentAssembly) {
                            item.onlyTrash = true;
                        }
                    }
                })
                .then(function () {
                    verifyAllMarked();
                });
        }

        function loadReasonsRepaired() {
            return $http({
                method: "POST",
                responseType: "json",
                url: vm.serverValues.reasonRepairedListUrl
            })
            .then(function (response) {
                vm.reasonsRepaired = response.data;
            });
        }

        function setResult(item, result) {
            if (item.Quantity === 1) {
                if (result === "Trashed") {
                    item.Result = result;
                } else if (result==="Inventory") {
                    item.Result = result;
                    item.ReasonRepaired = "";
                } else {
                    throw new Error("Unknown result: " + result);
                }
                vm.saveTearDownItem(item);
            }
        }

        function saveTearDownItem(item) {
            return $http({
                method: "POST",
                responseType: "json",
                url: vm.serverValues.updateUrl,
                data: item
            })
                .then(function (response) {
                    verifyAllMarked();
                });
        }

        function switchRepairMode() {
            return util.confirm("CAUTION: Switching back to regular repair will delete all inspections for this tear down. Are you sure you want to continue?")
                .then(function () {
                    return $http({
                        method: "POST",
                        responseType: "json",
                        url: vm.serverValues.switchRepairModeUrl,
                        data: { id: vm.serverValues.deliveryTicketID }
                    })
                        .then(function (httpResponse) {
                            if (httpResponse.data.Success) {
                                $window.location.assign(httpResponse.data.RedirectUrl);
                            } else {
                                return $q.reject(httpResponse.data.Errors);
                            }
                        });
                })
                .catch(function (errors) {
                    $window.alert(errors);
                });
        }

        function verifyAllMarked() {
            vm.allMarked = !vm.tearDownItems.some(function (item) {
                return !item.Result;
            });
            return vm.allMarked;
        }

        function completeTearDown() {
            return $http({
                method: "POST",
                responseType: "json",
                url: vm.serverValues.completeTearDownUrl,
                data: {id: vm.serverValues.deliveryTicketID}
            })
                .then(function (httpResponse) {
                    if (httpResponse.data.Success) {
                        $window.location.assign(httpResponse.data.RedirectUrl);
                    } else {
                        return $q.reject(httpResponse.data.Errors);
                    }
                })
                .catch(function (errors) {
                    $window.alert(errors);
                });
        }
    }
})();
