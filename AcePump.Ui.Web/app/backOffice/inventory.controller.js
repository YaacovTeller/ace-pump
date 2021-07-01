(function () {
    "use strict";

    angular
        .module("acePump.backOffice")
        .controller("InventoryController", InventoryController);

    InventoryController.$inject = ["$q", "$http", "kendo", "$scope"];
    function InventoryController($q, $http, kendo, $scope) {

        var vm = this;

        vm.init = init;
        vm.exportToExcel = exportToExcel;

        $scope.$watch(function () {
            return vm.loading;
        }, function (newValue, oldValue) {            
            if (newValue !== oldValue) {
                kendo.ui.progress(vm.grid.element, newValue);
            }
        });

        function init(serverValues) {
            vm.serverValues = serverValues;
            initGrid();
        }

        function exportToExcel() {
            vm.loading = true;
            vm.grid.saveAsExcel();            
        }

        function initGrid() {
            vm.gridOptions = {
                autoBind: true,
                filterable: true,
                pageable: true,
                reorderable: false,
                resizable: false,
                selectable: false,
                scrollable: false,
                sortable: true,

                columns: [
                    {
                        field: "CustomerName",
                        title: "Customer"
                    },
                    {
                        field: "Number",
                        title: "Part Number"
                    },
                    {
                        field: "Description",
                        title: "Description"
                    },
                    {
                        field: "QuantityAvailable",
                        title: "Quantity Available"
                    }
                ],

                excel: {
                    fileName: "Inventory.xlsx",
                    allPages: true,
                    filterable: true
                },

                excelExport: function (e) {
                    vm.loading = false;                    
                },

                dataSource: new kendo.data.DataSource({
                    pageSize: 20,
                    schema: {
                        data: "Data",
                        total: "Total"
                    },
                    serverFiltering: true,
                    serverPaging: true,
                    serverSorting: true,
                    serverAggregates: true,
                    serverGrouping: true,                    
                    transport: {
                        read: function (e) {
                            this.options = { prefix: "" };
                            $http({
                                method: "POST",
                                url: vm.serverValues.readUrl,
                                params: kendo.data.transports["aspnetmvc-ajax"].prototype.options.parameterMap.call(this, e.data, "read", false)
                            })
                                .then(function (httpResponse) {
                                    e.success(httpResponse.data);
                                });
                        }
                    }
                })
            };
        }
    }
})();
