(function () {
    "use strict";

    angular
        .module("acePump.backOffice")
        .controller("MergeWellController", MergeWellController);

    MergeWellController.$inject = ["$http", "$scope"];
    function MergeWellController($http, $scope) {
        var vm = this;

        vm.getMergeInfo = getMergeInfo;
        vm.mergeAndClose = mergeAndClose;
        vm.mergeWells = mergeWells;
        vm.selectSourceWell = selectSourceWell;

        init();

        function getMergeInfo(wellId) {
            vm.loading = true;

            return $http({
                url: $scope._urls.getMergeInfo,
                method: "POST",
                data: { id: wellId }
            })
                .then(function (httpResult) {
                    vm.loading = false;

                    var info = httpResult.data;
                    info.installedPumpMergeOption = "OverwriteTarget";

                    return info;
                });
        }

        function init() {
            vm.getMergeInfo($scope.targetWellId)
                .then(function (info) {
                    vm.target = info;
                });

            vm.lists = {
                leases: {
                    serverFiltering: true,
                    transport: {
                        read: function (e) {
                            $http({
                                url: $scope._urls.leaseEndpoint,
                                method: "POST",
                                data: kendoFilterToTermsDict(e.data.filter, "LeaseName", { includeNonCustomerLeases: true })
                            })
                                .catch(function (httpError) {
                                    e.error(httpError.data);
                                })
                                .then(function (httpResult) {
                                    e.success(httpResult.data);
                                });
                        }
                    }
                },
                wells: {
                    serverFiltering: true,
                    transport: {
                        read: function (e) {
                            $http({
                                url: $scope._urls.wellEndpoint,
                                method: "POST",
                                data: kendoFilterToTermsDict(e.data.filter, "WellNumber", { includeNonCustomerWells: true })
                            })
                                .catch(function (httpError) {
                                    e.error(httpError.data);
                                })
                                .then(function (httpResult) {
                                    e.success(httpResult.data);
                                });
                        }
                    }
                }
            };

            function kendoFilterToTermsDict(kendoFilter, termFieldName, additionalData) {
                var termsDict = additionalData || {};
                for (var i = 0; i < kendoFilter.filters.length; i++) {
                    termsDict[kendoFilter.filters[i].field] = kendoFilter.filters[i].value;
                }

                termsDict.term = termsDict[termFieldName];

                return termsDict;
            }
        }

        function mergeAndClose() {
            vm.loading = true;

            return vm.mergeWells(vm.target.WellID, vm.target.installedPumpMergeOption, vm.source.WellID, vm.source.installedPumpMergeOption)
                .then(function () {
                    vm.loading = false;
                    $scope.window.complete();
                });
        }

        function mergeWells(targetWellId, targetWellMergeOption, otherWellId, otherWellMergeOption) {
            return $http({
                url: $scope._urls.mergeWells,
                method: "POST",
                data: { targetWellId: targetWellId, targetWellMergeOption: targetWellMergeOption, otherWellId: otherWellId, otherWellMergeOption: otherWellMergeOption }
            })
                .then(function (httpResult) {
                    return httpResult.data;
                });
        }

        function selectSourceWell() {
            return vm.getMergeInfo(vm.sourceWell.WellID)
                .then(function (info) {
                    vm.source = info;
                });
        }
    }
})();
