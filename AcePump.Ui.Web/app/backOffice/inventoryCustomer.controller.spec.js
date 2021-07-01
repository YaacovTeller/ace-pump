(function () {
    "use strict";
    describe("Inventory Controller Customer", function () {
        var $controller,
            ctrl,
            $httpBackend,
            $scope;

        beforeEach(function () {
            module("acePump");
            inject(function (_$controller_, _$httpBackend_, $rootScope) {
                $controller = _$controller_;
                $httpBackend = _$httpBackend_;
                $scope = $rootScope.$new();
            });

            init();
        });

        function init() {
            ctrl = $controller("InventoryCustomerController", { $scope: $scope });
        }

        describe("init", function () {
            var serverValues;

            beforeEach(function () {
                serverValues = { readUrl: "http://www.readInventory.com" };
            });

            it("should set controller serverValues to received values from server", function () {
                ctrl.init(serverValues);

                expect(ctrl.serverValues).toEqual(serverValues);
            });

            it("should set gridOptions to the provided options.", function () {
                ctrl.init(serverValues);

                expect(ctrl.gridOptions).not.toBe(undefined);
            });
        });

        describe("exportToExcel", function () {
            beforeEach(function () {
                ctrl.loading = false;
                ctrl.grid = { saveAsExcel: function () { } };
                spyOn(ctrl.grid, "saveAsExcel");
            });
           
            it("should set loading to true", function () {
                ctrl.exportToExcel();

                expect(ctrl.loading).toBe(true);
            });

            it("should call grid's saveToExcel method.", function () {
                ctrl.exportToExcel();

                expect(ctrl.grid.saveAsExcel).toHaveBeenCalled();
            });
        });
    });
})();
