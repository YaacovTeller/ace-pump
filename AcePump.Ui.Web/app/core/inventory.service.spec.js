(function () {
    "use strict";

    describe("inventoryService", function () {

        var $httpBackend,
            $scope,
            service;

        beforeEach(function () {
            module("acePump.core");
            inject(function (_$httpBackend_, _inventoryService_, $rootScope) {
                service = _inventoryService_;
                $httpBackend = _$httpBackend_;
                $scope = $rootScope;
            });
        });

        describe("checkAvailability", function () {
            it("POSTS to the supplied url with part ReplacedID and customerID", function () {
                var url = "www.inventory.service.js/checkAvailability";
                $httpBackend.expectPOST(url, { id: 14, customerID: 22 }).respond(200);

                service.checkAvailability(url, 14, 22);

                $httpBackend.flush();
            });

            it("returns the received available flag when POST succeeds", function () {
                var url = "www.inventory.service.js/checkAvailability";
                var response = { Available: true };
                $httpBackend.expectPOST(url, { id: 14, customerID: 22 }).respond(response);

                var promise = service.checkAvailability(url, 14, 22);

                $httpBackend.flush();

                promise.then(function (result) {
                    expect(result).toBe(true);
                });

                $scope.$digest();
            });
        });

        describe("usePartFromInventory", function () {
            it("POSTS to the supplied url with the inspection and usesInventoryFlag", function () {
                var url = "www.inventory.service.js/usePartFromInventory";
                var inspection = { ID: 24, ReplacedWithInventoryPartID: true };

                $httpBackend.expectPOST(url, { model: inspection, useInventory: !inspection.ReplacedWithInventoryPartID }).respond(200);

                service.usePartFromInventory(url, inspection);

                $httpBackend.flush();
            });

            it("returns the server received InventoryPartID and Available flagged when POST succeeds.", function () {
                var url = "www.inventory.service.js/usePartFromInventory";
                var inspection = { ID: 24, ReplacedWithInventoryPartID: true };
                var response = { Success: true, ReplacedWithInventoryPartID: 22, AvailableInInventory: false };

                $httpBackend.expectPOST(url, { model: inspection, useInventory: !inspection.ReplacedWithInventoryPartID }).respond(response);

                var promise = service.usePartFromInventory(url, inspection);

                $httpBackend.flush();

                promise.then(function (result) {
                    expect(result).toEqual({ replacedWithInventoryPartID: 22, availableInInventory: false });
                });

                $scope.$digest();
            });

            it("returns a rejection with the server errors when POST is not successful.", function () {
                var url = "www.inventory.service.js/usePartFromInventory";
                var inspection = { ID: 24, ReplacedWithInventoryPartID: true };
                var response = { Success: false, Errors: "Some Really Bad Errors" };

                $httpBackend.expectPOST(url, { model: inspection, useInventory: !inspection.ReplacedWithInventoryPartID }).respond(response);

                var promise = service.usePartFromInventory(url, inspection);

                $httpBackend.flush();

                promise.catch(function (result) {
                    expect(result).toEqual(response.Errors);
                });

                $scope.$digest();
            });
        });

        describe("checkAvailabilityForInspections", function () {
            it("POSTs to the received url with the inspections that were Replace or Convert and the customer Id", function () {
                var url = "www.inventory.service.js/checkAvailabilityForInspections";
                var inspections = [
                    { ID: 1, Result: "Trashed" },
                    { ID: 2, Result: "Replace" },
                    { ID: 1, Result: "Convert" },
                    { ID: 1, Result: "Convert" }];

                var partsToCheck = [];
                for (var i = 0; i < inspections.length; i++) {
                    if (inspections[i].Result === "Replace" || inspections[i].Result === "Convert") {
                        partsToCheck[i] = inspections[i].PartReplacedID;
                    }
                }

                $httpBackend.expectPOST(url, { partTemplateIDs: partsToCheck, customerId: 33 }).respond(200);

                service.checkAvailabilityForInspections(url, 33, inspections);
                $scope.$digest();
                $scope.$digest();

                $httpBackend.flush();
            });

            it("Returns the received data when POST succeeds", function () {
                var url = "www.inventory.service.js/checkAvailabilityForInspections";
                var inspections = [
                    { ID: 1, Result: "Trashed" },
                    { ID: 2, Result: "Replace" },
                    { ID: 1, Result: "Convert" },
                    { ID: 1, Result: "Convert" }];

                $httpBackend.expectPOST(url).respond(inspections);

                var promise = service.checkAvailabilityForInspections(url, 33, inspections);
                $scope.$digest();
                $scope.$digest();

                $httpBackend.flush();

                promise.then(function (response) {
                    expect(response).toEqual(inspections);
                });
                $scope.$digest();
            });

            it("Returns an empty array if no parts resulted in Replace or Convert", function () {
                var url = "www.inventory.service.js/checkAvailabilityForInspections";
                var inspections = [
                    { ID: 1, Result: "Trashed" },
                    { ID: 2, Result: "Trashed" },
                    { ID: 1, Result: "Trashed" },
                    { ID: 1, Result: "Trashed" }];

                var promise = service.checkAvailabilityForInspections(url, 33, inspections);
                $scope.$digest();
                $scope.$digest();

                promise.then(function (response) {
                    expect(response).toEqual([]);
                });

                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();

                $scope.$digest();
            });
        });

        describe("loadOriginalPartsCustomerOwned", function () {
            it("POSTs the received parameters.", function () {
                var url = "www.inventory.service.js/loadOriginalPartsCustomerOwned";
                var parameters = { pumpID: 1, customerID: 2, currentTicketDate: 3 };

                $httpBackend.expectPOST(url, parameters).respond(200);

                service.loadOriginalPartsCustomerOwned(url, 1,2,3);

                $scope.$digest();
                $httpBackend.flush();
            });

            it("Returns the response from the server on success.", function () {
                var url = "www.inventory.service.js/loadOriginalPartsCustomerOwned";
                var parameters = { pumpID: 1, customerID: 2, currentTicketDate: 3 };
                var response = { SomeParameter: "SomeValue" };

                $httpBackend.expectPOST(url, parameters).respond(response);

                var promise = service.loadOriginalPartsCustomerOwned(url, 1, 2, 3);

                $scope.$digest();
                $httpBackend.flush();

                promise.then(function (value) {
                    expect(valure).toEqual(response);
                });
                $scope.$digest();
            });
            it("Returns an empty array on failure.", function () {
                var url = "www.inventory.service.js/loadOriginalPartsCustomerOwned";
                var parameters = { pumpID: 1, customerID: 2, currentTicketDate: 3 };

                $httpBackend.expectPOST(url, parameters).respond(404);

                var promise = service.loadOriginalPartsCustomerOwned(url, 1, 2, 3);

                $scope.$digest();
                $httpBackend.flush();

                promise.then(function (response) {
                    expect(response).toEqual([]);
                });
                $scope.$digest();
            });
        });
    });

})();