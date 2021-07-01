(function () {
    "use strict";
    describe("Tear Down Controller", function () {

        var $controller,
            ctrl,
            $httpBackend,
            $scope,
            $q,
            util,
            $window;

        var tearDownItems = [{ "PartInspectionID": 1, "Result": "Trashed", "ReasonRepaired": "Worn", "Quantity": 1, "CanBeRepresentedAsAssembly": false},
                             { "PartInspectionID": 2, "Result": "Inventory", "ReasonRepaired": "", "Quantity": 2, "CanBeRepresentedAsAssembly": true },
                             { "PartInspectionID": 3, "Result": "Trashed", "ReasonRepaired": "Gutted", "Quantity": 3, "CanBeRepresentedAsAssembly": false }];
        var reasonsRepaired = [{ "ItemTypeID": 3, "DisplayText": "BROKEN" },
                               { "ItemTypeID": 2, "DisplayText": "PACKED" },
                               { "ItemTypeID": 4, "DisplayText": "SCORED" },
                               { "ItemTypeID": 1, "DisplayText": "TORN" },
                               { "ItemTypeID": 5, "DisplayText": "WORN" }];
        var serverValues;

        beforeEach(function () {
            module("acePump");
            inject(function (_$controller_, $rootScope, _$httpBackend_, _$q_, _util_, _$window_) {
                $controller = _$controller_;
                $httpBackend = _$httpBackend_;
                $scope = $rootScope.$new();
                $q = _$q_;
                util = _util_;
                $window = _$window_;
            });

            init();
        });

        xdescribe("init", function () {     
            xit("should load tear down items from server", function () {
                var response = { Data: tearDownItems };
                $httpBackend.expectPOST(serverValues.readUrl).respond(response);

                ctrl.init(serverValues);

                $httpBackend.flush();
                expect(ctrl.tearDownItems.length).toBe(tearDownItems.length);
            });

            xit("should load reasons repaired from server", function () {
                $httpBackend.expectPOST(serverValues.reasonRepairedListUrl).respond(reasonsRepaired);

                ctrl.init(serverValues);

                $httpBackend.flush();
                expect(ctrl.reasonsRepaired).toEqual(reasonsRepaired);
            });

            it("set serverValues to what was received from server", function () {
                ctrl.init(serverValues);

                expect(ctrl.serverValues).toBe(serverValues);
            });

            //todo
            it("creates a new property on the tear down called onlyTrash with correct value");
        });

        describe("setResult", function () {
            var item;

            beforeEach(function () {
                spyOn(ctrl, "saveTearDownItem");

                item = {
                    Result: 'SomethingElse',
                    ReasonRepaired: 'SomeReason',
                    Quantity:1
                };
            });

            it("should set item.result to trashed if that was clicked", function () {
                ctrl.setResult(item, 'Trashed');

                expect(item.Result).toBe('Trashed');
            });

            it("should set item.result to inventory if inventory was clicked", function () {
                ctrl.setResult(item, 'Inventory');

                expect(item.Result).toBe('Inventory');
            });

            it("should clear item.reasonRepaired if inventory was clicked", function () {
                ctrl.setResult(item, 'Inventory');

                expect(item.ReasonRepaired).toBe('');
            });

            it("should clear not clear item.reasonRepaired if trashed was clicked", function () {
                ctrl.setResult(item, 'Trashed');

                expect(item.ReasonRepaired).toBe('SomeReason');
            });

            it("should call saveTearDownItem", function () {
                ctrl.setResult(item, 'Inventory');

                expect(ctrl.saveTearDownItem).toHaveBeenCalledWith(item);
            });

            it("should only set the result if quantity is 1", function () {
                item.Quantity = 3;
                ctrl.setResult(item, 'Trashed');

                expect(item.ReasonRepaired).toBe('SomeReason');
                expect(item.Result).toBe('SomethingElse');
                expect(ctrl.saveTearDownItem).not.toHaveBeenCalled();
            });
        });

        xdescribe("saveTearDownItem", function () {
            xit("should post the received item to the server", function () {                
                var item = tearDownItems[0];
                $httpBackend.expectPOST(serverValues.updateUrl, item).respond(200);

                ctrl.saveTearDownItem(item);

                $httpBackend.flush();
            });
        });

        xdescribe("switchRepairMode", function () {

            beforeEach(function () {
                spyOn(util, "confirm").and.returnValue($q.resolve());
                spyOn($window.location, "assign");
            });

            xit("should only switch if user confirmed he definitely wants to proceed.", function () {
                var response = { Success: true };
                $httpBackend.expectPOST(serverValues.switchRepairModeUrl).respond(response);

                ctrl.switchRepairMode();

                $httpBackend.flush();
            });

            xit("should not switch if user does not confirm he wants to proceed.", function () {
                spyOn($window, "alert");
                util.confirm.and.returnValue($q.reject());

                ctrl.switchRepairMode();

                $httpBackend.verifyNoOutstandingRequest();
            });

            xit("should POST to the received server value's url with the delivery ticket id.", function () {
                var response = { Success: true };
                $httpBackend.expectPOST(serverValues.switchRepairModeUrl, { id: serverValues.deliveryTicketID }).respond(response);

                ctrl.switchRepairMode();

                $httpBackend.flush();
            });

            xit("should redirect to the redirect url received from server on success.", function () {
                var response = { Success: true, RedirectUrl: "whatever" };
                $httpBackend.expectPOST(serverValues.switchRepairModeUrl, { id: serverValues.deliveryTicketID }).respond(response);

                ctrl.switchRepairMode();

                $httpBackend.flush();                
                expect($window.location.assign).toHaveBeenCalledWith(response.RedirectUrl);
            });

            xit("should alert the user of the errors if POST did not succeed.", function () {
                spyOn($window, 'alert');
                var response = { Success: false, Errors: "whatever" };
                $httpBackend.expectPOST(serverValues.switchRepairModeUrl, { id: serverValues.deliveryTicketID }).respond(response);

                ctrl.switchRepairMode();

                $httpBackend.flush();
                expect($window.alert).toHaveBeenCalledWith('whatever');
            });
        });

        xdescribe("completeTearDown", function () {
            xit("should post the DT id to the server", function () {
                spyOn($window.location, "assign");
                var response = { Success: true, RedirectUrl: "whatever" };
                $httpBackend.expectPOST(serverValues.completeTearDownUrl, { id: serverValues.deliveryTicketID }).respond(response);

                ctrl.completeTearDown();

                $httpBackend.flush();                
            });

            xit("should redirect to received url on success", function () {
                spyOn($window.location, "assign");
                var response = {Success: true, RedirectUrl: "whatever" };
                $httpBackend.expectPOST(serverValues.completeTearDownUrl, { id: serverValues.deliveryTicketID }).respond(response);

                ctrl.completeTearDown();

                $httpBackend.flush();
                expect($window.location.assign).toHaveBeenCalledWith(response.RedirectUrl);
            });

            xit("should alert user with errors on failure", function () {
                spyOn($window, "alert");
                var response = {Success: false, Errors: "whatever" };
                $httpBackend.expectPOST(serverValues.completeTearDownUrl, { id: serverValues.deliveryTicketID }).respond(response);

                ctrl.completeTearDown();

                $httpBackend.flush();
                expect($window.alert).toHaveBeenCalledWith(response.Errors);
            });
        });

        function init() {
            serverValues = {
                readUrl: "http://unit.test/TearDownItems/List",
                reasonRepairedListUrl: "http://unit.test/ReasonRepaired/List",
                deliveryTicketID: 14,
                updateUrl: "http://unit.test/UpdateTearDownItems",
                switchRepairModeUrl: "http://unit.test/SwtichRepairMode",
                completeTearDownUrl: "http://unit.test/CompleteTearDown"
            };

            ctrl = $controller("TearDownController", { $scope: $scope });
            ctrl.init(serverValues);
        }
    });
})();
