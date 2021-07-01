(function () {
    "use strict";

    describe("RepairTicketController", function () {
        beforeEach(module("acePump"));

        var $controller,
            inventoryService,
            $httpBackend,
            $scope,
            $q,
            util,
            $window;

        beforeEach(inject(function (_$controller_, _inventoryService_, _$q_, $http, _$httpBackend_, $rootScope, _util_, _$window_) {
            $scope = $rootScope.$new();
            $scope.serverVars = {
                readUrl: "",
                updateUrl: "",
                removeUrl: "",
                syncInspectionOrderUrl: "",
                checkAvailabilityInInventoryUrl: "",
                switchRepairModeUrl: "",
                updateUsingPartFromInventoryUrl: "",
                listPartsInUseForPumpUrl: "",
                checkInventoryListUrl: "",
                customerID: 3,
                deliveryTicketID: 0,
            };

            $q = _$q_;
            util = _util_;
            $window = _$window_;
            $httpBackend = _$httpBackend_;
            inventoryService = _inventoryService_;

            $controller = _$controller_;
            $controller("RepairTicketController", { $scope: $scope, $q: _$q_, $http: $http });

            $scope.inspections = [];

            jasmine.addMatchers({
                toBeResolved: function (util, customerEqualityTesters) {
                    return {
                        compare: function (expected, actual) {
                            var result = {
                                message: "Expected " + expected + " to be resolved but it was not",
                                pass: false
                            };

                            if (!expected || !expected.then) {
                                result.message = "Expected " + expected + " to be a promise";

                            } else {
                                expected.then(function (reason) {
                                    if (actual && actual !== resolved) {
                                        result.message = "Expected " + expected + " to be resolved with '" + actual + "' message, but the actual message was '" + reason + "'";
                                    } else {
                                        result.pass = true;
                                    }
                                });
                            }

                            $scope.$digest();
                            return result;
                        }
                    };
                },
                toBeRejected: function (util, customerEqualityTesters) {
                    return {
                        compare: function (expected, actual) {
                            var result = {
                                message: "Expected " + expected + " to be rejected but it was not",
                                pass: false
                            };

                            if (!expected || !expected.catch) {
                                result.message = "Expected " + expected + " to be a promise";

                            } else {
                                expected.catch(function (reason) {
                                    if (actual && actual !== resolved) {
                                        result.message = "Expected " + expected + " to be rejected with '" + actual + "' message, but the actual message was '" + reason + "'";
                                    } else {
                                        result.pass = true;
                                    }
                                });
                            }

                            $scope.$digest();
                            return result;
                        }
                    };
                }
            });
        }));

        describe("alert", function() {
            var $window;

            beforeEach(inject(function(_$window_){
                $window = _$window_;
            }));

            it("calls $window.alert", function() {
                spyOn($window, "alert");
                var txt = {};

                $scope.alert(txt);

                expect($window.alert).toHaveBeenCalledWith(txt);
            });
        });

        describe("$scope.allowEditing", function () {
            it("sets partInspection.inEditMode to true if undefined", function () {
                var inspection = {};

                $scope.allowEditing(inspection);

                expect(inspection.inEditMode).toBe(true);
            });

            it("sets partInspection.inEditMode to true if false", function () {
                var inspection = { inEditMode: false };

                $scope.allowEditing(inspection);

                expect(inspection.inEditMode).toBe(true);
            });

            it("stores the deferred in the inspection._editDeferred", function () {
                var inspection = {};

                var promise = $scope.allowEditing(inspection);

                expect(inspection._editDeferred.promise).toBe(promise);
            });

            it("sets editReplacementPartTemplateNumber to true if Result===Convert", function () {
                var inspection = { Result: "Convert" };

                $scope.allowEditing(inspection);
                $scope.$digest();

                expect(inspection.editReplacementPartTemplateNumber).toBe(true);
            });

            it("cleans up edit flags when _editDeferred resolves", function() {
                var inspection = { Result: "Convert" };

                $scope.allowEditing(inspection);
                inspection._editDeferred.resolve();
                $scope.$digest();

                expect(inspection.editReplacementPartTemplateNumber).toBe(false);
                expect(inspection.inEditMode).toBe(false);
            });

            it("deletes the _editDeferred object when _editDeferred resolves", function () {
                var inspection = { Result: "Convert" };

                $scope.allowEditing(inspection);
                inspection._editDeferred.resolve();
                $scope.$digest();

                expect("_editDeferred" in inspection).toBe(false);
            });

            it("deletes the _editDeferred object when _editDeferred rejects", function () {
                var inspection = { Result: "Convert" };

                $scope.allowEditing(inspection);
                inspection._editDeferred.reject();
                $scope.$digest();

                expect("_editDeferred" in inspection).toBe(false);
            });

            it("cleans up edit flags when _editDeferred rejects", function() {
                var inspection = { Result: "Convert" };

                $scope.allowEditing(inspection);
                inspection._editDeferred.reject();
                $scope.$digest();

                expect(inspection.editReplacementPartTemplateNumber).toBe(false);
                expect(inspection.inEditMode).toBe(false);
            });
        });

        describe("$scope.cancelEdit", function () {
            it("rejects the _editDeferred deferred", function () {
                var rejected = false;
                var inspection = { _editDeferred: $q.defer(), inEditMode: true };
                inspection._editDeferred.promise.then(function () { }, function () { rejected = true; });

                $scope.cancelEdit(inspection);
                $scope.$digest();

                expect(rejected).toBe(true);
            });

            it("throws if not in edit mode", function () {
                var inspection = { inEditMode: false, _editDeferred: $q.defer() };

                expect(function () { $scope.cancelEdit(inspection); }).toThrow();
            });

            it("throws if no _editDeferred", function () {
                var inspection = { inEditMode: true };

                expect(function () { $scope.cancelEdit(inspection); }).toThrow();
            });

            it("reverts the inspection values to pre edit values", function() {
                var originalValues = {
                    Result: "pre edit r",
                    ReasonRepaired: "pre edit rr",
                    ReplacementQuantity: "pre edit rq",
                    ReplacementPartTemplateNumber: "pre edit rpn",
                    PartReplacedID: "pre edit pri"
                };
                var inspection = {
                    Result: originalValues.Result,
                    ReasonRepaired: originalValues.ReasonRepaired,
                    ReplacementQuantity: originalValues.ReplacementQuantity,
                    ReplacementPartTemplateNumber: originalValues.ReplacementPartTemplateNumber,
                    PartReplacedID: originalValues.PartReplacedID
                };
                $scope.markPart(inspection, "Replace");
                $scope.$digest();
                expect(inspection.Result).toBe("Replace");

                $scope.cancelEdit(inspection);
                $scope.$digest();

                expect(inspection.ReasonRepaired).toBe(originalValues.ReasonRepaired);
                expect(inspection.ReplacementQuantity).toBe(originalValues.ReplacementQuantity);
                expect(inspection.Result).toBe(originalValues.Result);
                expect(inspection.ReplacementPartTemplateNumber).toBe(originalValues.ReplacementPartTemplateNumber);
                expect(inspection.PartReplacedID).toBe(originalValues.PartReplacedID);
            });
        });

        describe("validateAllItemsMarked", function() {
            beforeEach(function() {
                spyOn($scope, "alert");
            });

            it("returns false if an item has no mark", function() {
                $scope.inspections = [
                    {Result: "", inEditMode: false}
                ];

                var validatePromise = $scope.validateAllItemsMarked();

                expect(validatePromise).toBeRejected();
            });

            it("returns false if an item does not complete properly", function() {
                $scope.inspections = [
                    {Result: "OK", _markCompletedPromise: $q.reject()}
                ];

                var validatePromise = $scope.validateAllItemsMarked();

                expect(validatePromise).toBeRejected();
            });

            it("shows error message if fails", function() {
                $scope.inspections = [ {Result: "", inEditMode: false} ];

                $scope.validateAllItemsMarked();

                expect($scope.alert).toHaveBeenCalled();
            });

            it("returns true if all items saves properly", function() {
                $scope.inspections = [
                    {Result: "OK", _markCompletedPromise: $q.resolve()}
                ];

                var validatePromise = $scope.validateAllItemsMarked();

                expect(validatePromise).toBeResolved();
            });

            it("returns true if all items marked not inEditMode", function(){
                $scope.inspections = [
                    {Result: "OK", inEditMode: false}
                ];

                var validatePromise = $scope.validateAllItemsMarked();

                expect(validatePromise).toBeResolved();
            });
        });

        describe("$scope.deleteInspection", function() {
            var inspections;

            beforeEach(function(){
                $scope.inspections  = inspections = [];
                spyOn($scope, "postInspectionDelete").and.returnValue($q.resolve());
            });

            it("does not confirm if doNotConfirm is true", function() {
                var $window; inject(function(_$window_){$window = _$window_;});
                spyOn($window, "confirm");
                var deleted = {PartInspectionID: 5};
                inspections.push(deleted);

                $scope.deleteInspection(deleted, null, true);

                expect($window.confirm).not.toHaveBeenCalled();
            });

            it("confirms if doNotConfirm is false", function() {
                var $window; inject(function(_$window_){$window = _$window_;});
                spyOn($window, "confirm");
                var deleted = {PartInspectionID: 5};
                inspections.push(deleted);

                $scope.deleteInspection(deleted, null, false);

                expect($window.confirm).toHaveBeenCalled();
            });

            it("confirms if doNotConfirm is undefined", function() {
                var $window; inject(function(_$window_){$window = _$window_;});
                spyOn($window, "confirm");
                var deleted = {PartInspectionID: 5};
                inspections.push(deleted);

                $scope.deleteInspection(deleted);

                expect($window.confirm).toHaveBeenCalled();
            });

            it("does not call postInspectionDelete if confirm is cancelled", function() {
                var $window; inject(function(_$window_){$window = _$window_;});
                spyOn($window, "confirm").and.returnValue(false);
                var deleted = {PartInspectionID: 5};
                inspections.push(deleted);

                $scope.deleteInspection(deleted);

                expect($scope.postInspectionDelete).not.toHaveBeenCalled();
            });

            it("rejects promise w/ 'cancelled' if confirm is cancelled", function() {
                var $window; inject(function(_$window_){$window = _$window_;});
                spyOn($window, "confirm").and.returnValue(false);
                var deleted = {PartInspectionID: 5};
                inspections.push(deleted);

                var promise = $scope.deleteInspection(deleted);

                var reasonRejected = null;
                promise.then(function(){},function(r){reasonRejected=r;});
                $scope.$digest();
                expect(reasonRejected).toBe("cancelled");
            });

            describe("if confirm OK'ed", function() {
                beforeEach(inject(function(_$window_) {
                    spyOn(_$window_, "confirm").and.returnValue(true);
                }));

                it("calls postInspectionDelete w/ insp", function() {
                    var deleted = {PartInspectionID: 5};
                    inspections.push(deleted);

                    $scope.deleteInspection(deleted);

                    expect($scope.postInspectionDelete).toHaveBeenCalledWith(deleted);
                });

                it("does not delete from $scope.inspections before postInspectionDelete resolves", function() {
                    $scope.postInspectionDelete.and.returnValue($q.reject());
                    var deleted = {PartInspectionID: 5};
                    var notDeleted = {PartInspectionID: 10};
                    inspections.push(deleted, notDeleted);

                    $scope.deleteInspection(deleted, null);

                    expect(inspections).toContain(deleted);
                    expect(inspections).toContain(notDeleted);
                });

                describe("when postInspectionDelete resolves", function() {
                    it("promise returned resolves", function() {
                        var inspection = {PartInspectionID: 12};
                        inspections.push(inspection);
                        var postDefer = $q.defer();
                        $scope.postInspectionDelete.and.returnValue(postDefer.promise);

                        var promise = $scope.deleteInspection(inspection, null, true);

                        var resolved = false;
                        promise.then(function(){resolved=true;});
                        expect(resolved).toBe(false);
                        $scope.$digest();

                        postDefer.resolve();
                        $scope.$digest();
                        expect(resolved).toBe(true);
                    });

                    it("deletes from $scope.inspections that matches the partInspection if no ix supplied", function() {
                        var deleted = {PartInspectionID: 5};
                        var notDeleted = {PartInspectionID: 10};
                        inspections.push(deleted, notDeleted);

                        $scope.deleteInspection(deleted, null);

                        $scope.$digest();
                        expect(inspections).not.toContain(deleted);
                        expect(inspections).toContain(notDeleted);
                    });

                    it("deletes from $scope.inspections at ix instead of argument if ix supplied", function() {
                        var deleted = {PartInspectionID: 5};
                        var notDeleted = {PartInspectionID: 10};
                        inspections.push(deleted, notDeleted);

                        $scope.deleteInspection(notDeleted, 0);

                        $scope.$digest();
                        expect(inspections).not.toContain(deleted);
                        expect(inspections).toContain(notDeleted);
                    });

                    it("deletes from $scope.inspections at ix if null and ix supplied", function() {
                        var deleted = {PartInspectionID: 5};
                        inspections.push(deleted);

                        $scope.deleteInspection(null, 0);

                        $scope.$digest();
                        expect(inspections).not.toContain(deleted);
                    });

                    it("deletes correct inspection when ix supplied but inspection at ix changes between calling deleteInspection and POST completing (like when two deleteInspections are called in a row).", function() {
                        var deleted = {PartInspectionID: 5};
                        var notDeleted = {PartInspectionID: 10};
                        inspections.push(deleted, notDeleted);

                        $scope.deleteInspection(deleted, 0);
                        inspections[0] = notDeleted;
                        inspections[1] = deleted;

                        $scope.$digest();
                        expect(inspections).not.toContain(deleted);
                        expect(inspections).toContain(notDeleted);
                    });
                });
            });
        });

        describe("$scope.markPart", function () {
            var inspection;

            beforeEach(function () {
                inspection = {
                    IsSplitAssembly: false,
                    CanBeRepresentedAsAssembly: false
                };

                spyOn($scope, "postInspectionUpdate").and.returnValue($q.resolve());
                spyOn($scope, "checkAvailabilityInInventory").and.returnValue($q.resolve());
            });

            describe("for OK mark", function () {
                var mark = "OK";

                it("should create _markCompletedPromise and resolve immediately", function() {
                    var spy = jasmine.createSpy("spy");

                    $scope.markPart(inspection, mark);
                    inspection._markCompletedPromise.then(spy);
                    $scope.$digest();

                    expect(spy).toHaveBeenCalled();
                });

                it("should set result to OK", function () {
                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.Result).toEqual("OK");
                });

                it("should clear replacement info", function () {
                    inspection.ReasonRepaired = "reason";
                    inspection.ReplacementPartTemplateNumber = "number";
                    inspection.ReplacementQuantity = 5;
                    inspection.PartReplacedID = 10;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.ReasonRepaired).toBeNull();
                    expect(inspection.ReplacementPartTemplateNumber).toEqual("");
                    expect(inspection.ReplacementQuantity).toBeNull();
                    expect(inspection.PartReplacedID).toBeNull();
                });

                it("should not call allowEditing", function () {
                    spyOn($scope, "allowEditing");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.allowEditing).not.toHaveBeenCalled();
                });

                it("should not call suggestAssembly even if is assm", function () {
                    spyOn($scope, "suggestReplaceAssembly");
                    inspection.CanBeRepresentedAsAssembly = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.suggestReplaceAssembly).not.toHaveBeenCalled();
                });

                it("should postInspectionUpdate immediately", function () {
                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).toHaveBeenCalled();
                });

                it("should call unsplitAssembly with inspection if is split assm", function () {
                    spyOn($scope, "unsplitAssembly");
                    inspection.IsSplitAssembly = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.unsplitAssembly.calls.argsFor(0)[0]).toBe(inspection);
                });

                it("should not call unsplitAssembly with inspection if is not split assm", function () {
                    spyOn($scope, "unsplitAssembly");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.unsplitAssembly).not.toHaveBeenCalled();
                });

                it("should log unhandled errors onto console", function() {
                    var errText = "unhandled error";
                    $scope.postInspectionUpdate.and.callFake(createRejectOnce(errText));
                    spyOn(console, "log");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(console.log).toHaveBeenCalledWith("Unhandled error in markPart: " + errText);
                });

                it("should not log handled errors (where reject reason === $scope.CONSTANTS.ERROR_HANDLED) onto console" ,function() {
                    $scope.postInspectionUpdate.and.returnValue($q.reject($scope.CONSTANTS.ERROR_HANDLED));
                    spyOn(console, "log");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(console.log).not.toHaveBeenCalled();
                });

                it("reverts the values if POST fails", function() {
                    var $httpBackend;
                    inject(function(_$httpBackend_, $window) {
                        $httpBackend = _$httpBackend_;
                        spyOn($window, "alert");
                    });

                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());
                    $scope.postInspectionUpdate.and.callThrough();
                    $httpBackend.expectPOST($scope.serverVars.updateUrl)
                        .respond({Errors: {prop1: {errors: ["error for prop 1"]}}});

                    inspection.Result = "12345";

                    $scope.markPart(inspection, mark);
                    $scope.$digest();
                    $httpBackend.flush();

                    expect(inspection.Result).toBe("12345");
                    expect(inspection._previousInspectionValues).toBeUndefined();
                });

                it("clears updateFailed if was prev set", function() {
                    inspection.updateFailed = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.updateFailed).toBe(false);
                });
            });

            describe("for N/A mark", function () {
                var mark = "NA";

                it("should create _markCompletedPromise and resolve immediately", function() {
                    var spy = jasmine.createSpy("spy");

                    $scope.markPart(inspection, mark);
                    inspection._markCompletedPromise.then(spy);
                    $scope.$digest();

                    expect(spy).toHaveBeenCalled();
                });

                it("should set result to N/A and reason to no inspect", function () {
                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.Result).toEqual("N/A");
                    expect(inspection.ReasonRepaired).toEqual("Did not inspect");
                });

                it("should clear replacement info", function () {
                    inspection.ReplacementPartTemplateNumber = "number";
                    inspection.ReplacementQuantity = 5;
                    inspection.PartReplacedID = 10;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.ReplacementPartTemplateNumber).toEqual("");
                    expect(inspection.ReplacementQuantity).toBeNull();
                    expect(inspection.PartReplacedID).toBeNull();
                });

                it("should not call allowEditing", function () {
                    spyOn($scope, "allowEditing");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.allowEditing).not.toHaveBeenCalled();
                });

                it("should not call suggestAssembly even if is assm", function () {
                    spyOn($scope, "suggestReplaceAssembly");
                    inspection.CanBeRepresentedAsAssembly = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.suggestReplaceAssembly).not.toHaveBeenCalled();
                });

                it("should postInspectionUpdate immediately", function () {
                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).toHaveBeenCalled();
                });

                it("should call unsplitAssembly with inspection if is split assm", function () {
                    spyOn($scope, "unsplitAssembly");
                    inspection.IsSplitAssembly = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.unsplitAssembly.calls.argsFor(0)[0]).toBe(inspection);
                });

                it("should not call unsplitAssembly with inspection if is not split assm", function () {
                    spyOn($scope, "unsplitAssembly");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.unsplitAssembly).not.toHaveBeenCalled();
                });

                it("should log unhandled errors onto console", function() {
                    var errText = "unhandled error";
                    $scope.postInspectionUpdate.and.callFake(createRejectOnce(errText));
                    spyOn(console, "log");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(console.log).toHaveBeenCalledWith("Unhandled error in markPart: " + errText);
                });

                it("should not log handled errors (where reject reason === $scope.CONSTANTS.ERROR_HANDLED) onto console" ,function() {
                    $scope.postInspectionUpdate.and.returnValue($q.reject($scope.CONSTANTS.ERROR_HANDLED));
                    spyOn(console, "log");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(console.log).not.toHaveBeenCalled();
                });

                it("reverts the values if POST fails", function() {
                    var $httpBackend;
                    inject(function(_$httpBackend_, $window) {
                        $httpBackend = _$httpBackend_;
                        spyOn($window, "alert");
                    });

                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());
                    $scope.postInspectionUpdate.and.callThrough();
                    $httpBackend.expectPOST($scope.serverVars.updateUrl)
                        .respond({Errors: {prop1: {errors: ["error for prop 1"]}}});

                    inspection.Result = "OK";

                    $scope.markPart(inspection, mark);
                    $scope.$digest();
                    $httpBackend.flush();

                    expect(inspection.Result).toBe("OK");
                    expect(inspection._previousInspectionValues).toBeUndefined();
                });

                it("clears updateFailed if was prev set", function() {
                    inspection.updateFailed = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.updateFailed).toBe(false);
                });
            });

            describe("for Maintenance mark", function () {
                var mark = "Maintenance";

                it("should create _markCompletedPromise and resolve immediately", function() {
                    var spy = jasmine.createSpy("spy");

                    $scope.markPart(inspection, mark);
                    inspection._markCompletedPromise.then(spy);
                    $scope.$digest();

                    expect(spy).toHaveBeenCalled();
                });

                it("should set result to repl and reason to rout/maint", function () {
                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.Result).toEqual("Replace");
                    expect(inspection.ReasonRepaired).toEqual("ROUT/MAINT");
                });

                it("should set replacement info from original info", function () {
                    inspection.OriginalPartTemplateNumber = "number";
                    inspection.Quantity = 5;
                    inspection.OriginalPartTemplateID = 10;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.ReplacementPartTemplateNumber).toEqual(inspection.OriginalPartTemplateNumber);
                    expect(inspection.ReplacementQuantity).toEqual(inspection.Quantity);
                    expect(inspection.PartReplacedID).toEqual(inspection.OriginalPartTemplateID);
                });

                it("should not call allowEditing", function () {
                    spyOn($scope, "allowEditing");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.allowEditing).not.toHaveBeenCalled();
                });

                it("should not call suggestAssembly even if is assm", function () {
                    spyOn($scope, "suggestReplaceAssembly");
                    inspection.CanBeRepresentedAsAssembly = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.suggestReplaceAssembly).not.toHaveBeenCalled();
                });

                it("should postInspectionUpdate immediately", function () {
                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).toHaveBeenCalled();
                });

                it("should call unsplitAssembly with inspection if is split assm", function () {
                    spyOn($scope, "unsplitAssembly");
                    inspection.IsSplitAssembly = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.unsplitAssembly.calls.argsFor(0)[0]).toBe(inspection);
                });

                it("should not call unsplitAssembly with inspection if is not split assm", function () {
                    spyOn($scope, "unsplitAssembly");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.unsplitAssembly).not.toHaveBeenCalled();
                });

                it("should log unhandled errors onto console", function() {
                    var errText = "unhandled error";
                    $scope.postInspectionUpdate.and.callFake(createRejectOnce(errText));
                    spyOn(console, "log");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(console.log).toHaveBeenCalledWith("Unhandled error in markPart: " + errText);
                });

                it("should not log handled errors (where reject reason === $scope.CONSTANTS.ERROR_HANDLED) onto console" ,function() {
                    $scope.postInspectionUpdate.and.returnValue($q.reject($scope.CONSTANTS.ERROR_HANDLED));
                    spyOn(console, "log");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(console.log).not.toHaveBeenCalled();
                });

                it("reverts the values if POST fails", function() {
                    var $httpBackend;
                    inject(function(_$httpBackend_, $window) {
                        $httpBackend = _$httpBackend_;
                        spyOn($window, "alert");
                    });

                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());
                    $scope.postInspectionUpdate.and.callThrough();
                    $httpBackend.expectPOST($scope.serverVars.updateUrl)
                        .respond({Errors: {prop1: {errors: ["error for prop 1"]}}});

                    inspection.Result = "OK";

                    $scope.markPart(inspection, mark);
                    $scope.$digest();
                    $httpBackend.flush();

                    expect(inspection.Result).toBe("OK");
                    expect(inspection._previousInspectionValues).toBeUndefined();
                });

                it("clears updateFailed if was prev set", function() {
                    inspection.updateFailed = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.updateFailed).toBe(false);
                });
            });

            describe("for Convert mark", function () {
                var mark = "Convert";

                it("should create _markCompletedPromise and resolve after edit ends", function() {
                    var spy = jasmine.createSpy("spy");

                    $scope.markPart(inspection, mark);
                    inspection._markCompletedPromise.then(spy);
                    $scope.$digest();

                    inspection._editDeferred.resolve();
                    $scope.$digest();
                    expect(spy).toHaveBeenCalled();
                });

                it("should set result to repl", function () {
                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.Result).toEqual("Convert");
                });

                it("should set replacement qty from orig info", function () {
                    inspection.Quantity = 5;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.ReplacementQuantity).toEqual(inspection.Quantity);
                });

                it("should clear other replacement info", function () {
                    inspection.ReplacementPartTemplateNumber = "number";
                    inspection.PartReplacedID = 1;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.ReplacementPartTemplateNumber).toEqual("");
                    expect(inspection.PartReplacedID).toBeNull();
                });

                it("should call allowEditing with inspection", function () {
                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.allowEditing).toHaveBeenCalled();
                });

                it("should not call suggestAssembly even if is assm", function () {
                    spyOn($scope, "suggestReplaceAssembly");
                    inspection.CanBeRepresentedAsAssembly = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.suggestReplaceAssembly).not.toHaveBeenCalled();
                });

                it("should postInspectionUpdate if edit resolved", function () {
                    var editDeferred = $q.defer();
                    spyOn($scope, "allowEditing").and.returnValue(editDeferred.promise);

                    $scope.markPart(inspection, mark);
                    editDeferred.resolve();
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).toHaveBeenCalled();
                });

                it("should call unsplitAssembly with inspection if is split assm", function () {
                    spyOn($scope, "unsplitAssembly");
                    inspection.IsSplitAssembly = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.unsplitAssembly.calls.argsFor(0)[0]).toBe(inspection);
                });

                it("should not call unsplitAssembly with inspection if is not split assm", function () {
                    spyOn($scope, "unsplitAssembly");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.unsplitAssembly).not.toHaveBeenCalled();
                });

                it("should log unhandled errors onto console", function() {
                    var errText = "unhandled error";
                    spyOn($scope, "allowEditing").and.callFake(createRejectOnce(errText));
                    spyOn(console, "log");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(console.log).toHaveBeenCalledWith("Unhandled error in markPart: " + errText);
                });

                it("should not log handled errors (where reject reason === $scope.CONSTANTS.ERROR_HANDLED) onto console" ,function() {
                    spyOn($scope, "allowEditing").and.returnValue($q.reject($scope.CONSTANTS.ERROR_HANDLED));
                    spyOn(console, "log").and.callThrough();

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(console.log).not.toHaveBeenCalled();
                });

                it("calls postInspectionUpdate when allowEdit resolved", function() {
                    var allowDeferred = $q.defer();
                    spyOn($scope, "allowEditing").and.returnValue(allowDeferred.promise);

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).not.toHaveBeenCalled();
                    allowDeferred.resolve();
                    $scope.$digest();
                    expect($scope.postInspectionUpdate).toHaveBeenCalledWith(inspection);
                });

                it("calls postInspectionUpdate if allowEdit rejected w/ $scope.CONSTANTS.ERROR_HANDLED", function() {
                    spyOn($scope, "allowEditing").and.returnValue($q.reject($scope.CONSTANTS.ERROR_HANDLED));

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).toHaveBeenCalledWith(inspection);
                });

                it("does not loop if allowEdit rejected w/ $scope.CONSTANTS.ERROR_HANDLED", function() {
                    spyOn($scope, "allowEditing").and.returnValue($q.reject($scope.CONSTANTS.ERROR_HANDLED));

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.allowEditing.calls.count()).toBe(1);
                });

                it("does not call postInspectionUpdate if allowEdit rejected w/ 'cancel'", function() {
                    spyOn($scope, "allowEditing").and.callFake(createRejectOnce("cancel"));

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).not.toHaveBeenCalled();
                });

                it("loops to allowEdit a second time if first allowEdit resolved but postInspection rejects", function() {
                    $scope.postInspectionUpdate.and.callFake(createRejectOnce());
                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.allowEditing.calls.count()).toBe(2);
                });

                it("does not revert the values if POST fails", function() {
                    var $httpBackend,
                        firstPost = true;

                    inject(function(_$httpBackend_, $window) {
                        $httpBackend = _$httpBackend_;
                        spyOn($window, "alert");
                    });

                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());
                    $scope.postInspectionUpdate.and.callThrough();
                    $httpBackend.whenPOST($scope.serverVars.updateUrl).respond(function() {
                        expect(inspection.Result).toBe(mark);

                        if (firstPost) {
                            firstPost = false;
                            return [200, {Errors: {prop1: {errors: ["error for prop 1"]}}}, {}, "OK"];
                        } else {
                            return [200, {}, {}, "OK"];
                        }
                    });

                    $httpBackend.expectPOST($scope.serverVars.updateUrl);
                    $httpBackend.expectPOST($scope.serverVars.updateUrl);
                    inspection._previousInspectionValues = { ReasonRepaired: "Not repaired" };
                    inspection.Result = "OK";

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    $httpBackend.flush();
                });

                it("sets the updateFailed flag if POST fails and clears when POST passes", function() {
                    var $httpBackend,
                        firstPost = true;

                    inject(function(_$httpBackend_, $window) {
                        $httpBackend = _$httpBackend_;
                        spyOn($window, "alert");
                    });

                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());
                    $scope.postInspectionUpdate.and.callThrough();
                    $httpBackend.whenPOST($scope.serverVars.updateUrl).respond(function() {
                        if (firstPost) {
                            firstPost = false;
                            return [200, {Errors: {prop1: {errors: ["error for prop 1"]}}}, {}, "OK"];
                        } else {
                            expect(inspection.updateFailed).toBe(true);

                            return [200, {}, {}, "OK"];
                        }
                    });

                    $httpBackend.expectPOST($scope.serverVars.updateUrl);
                    $httpBackend.expectPOST($scope.serverVars.updateUrl);
                    inspection._previousInspectionValues = { ReasonRepaired: "Not repaired" };
                    inspection.Result = "OK";

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    $httpBackend.flush();
                    expect(inspection.updateFailed).toBe(false);
                });

                it("clears updateFailed if was prev set", function() {
                    inspection.updateFailed = true;
                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.updateFailed).toBe(false);
                });
            });

            describe("for Replace mark", function () {
                var mark = "Replace";

                it("should create _markCompletedPromise and resolve after edit ends", function() {
                    var spy = jasmine.createSpy("spy");

                    $scope.markPart(inspection, mark);
                    inspection._markCompletedPromise.then(spy);
                    $scope.$digest();

                    inspection._editDeferred.resolve();
                    $scope.$digest();
                    expect(spy).toHaveBeenCalled();
                });

                it("should set result to repl and reason to  null", function () {
                    inspection.ReasonRepaired = "reason";

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.Result).toEqual("Replace");
                    expect(inspection.ReasonRepaired).toBeNull();
                });

                it("should set replacement info from orig info", function () {
                    inspection.OriginalPartTemplateNumber = "number";
                    inspection.Quantity = 5;
                    inspection.OriginalPartTemplateID = 10;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.ReplacementPartTemplateNumber).toEqual(inspection.OriginalPartTemplateNumber);
                    expect(inspection.ReplacementQuantity).toEqual(inspection.Quantity);
                    expect(inspection.PartReplacedID).toEqual(inspection.OriginalPartTemplateID);
                });

                it("should not call allowEditing before suggest replace promise is resolved if assembly", function () {
                    var suggestDefer = $q.defer();
                    inspection.CanBeRepresentedAsAssembly = true;
                    spyOn($scope, "allowEditing");
                    spyOn($scope, "suggestReplaceAssembly").and.returnValue(suggestDefer.promise);

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.allowEditing).not.toHaveBeenCalled();
                });

                it("should call allowEditing with inspection after suggest replace promise is resolved", function () {
                    inspection.CanBeRepresentedAsAssembly = true;
                    var suggestDefer = $q.defer();
                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());
                    spyOn($scope, "suggestReplaceAssembly").and.returnValue(suggestDefer.promise);

                    $scope.markPart(inspection, mark);
                    suggestDefer.resolve();
                    $scope.$digest();

                    expect($scope.allowEditing).toHaveBeenCalledWith(inspection);
                });

                it("should call allowEditing with inspection immediately if not assembly", function () {
                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.allowEditing).toHaveBeenCalledWith(inspection);
                });

                it("should call suggestAssembly if is assm", function () {
                    spyOn($scope, "suggestReplaceAssembly");
                    inspection.CanBeRepresentedAsAssembly = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.suggestReplaceAssembly).toHaveBeenCalled();
                });

                it("should not call suggestAssembly if not assm", function () {
                    spyOn($scope, "suggestReplaceAssembly");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.suggestReplaceAssembly).not.toHaveBeenCalled();
                });

                it("should not postInspectionUpdate if assm and suggest cancelled", function () {
                    var suggestDeferred = $q.defer();
                    inspection.CanBeRepresentedAsAssembly = true;
                    spyOn($scope, "suggestReplaceAssembly").and.returnValue(suggestDeferred.promise);

                    $scope.markPart(inspection, mark);
                    suggestDeferred.reject($scope.CONSTANTS.ERROR_HANDLED);
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).not.toHaveBeenCalled();
                });

                it("should not postInspectionUpdate if edit cancelled", function () {
                    var editDeferred = $q.defer();
                    spyOn($scope, "allowEditing").and.returnValue(editDeferred.promise);

                    $scope.markPart(inspection, mark);
                    editDeferred.reject("cancel");
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).not.toHaveBeenCalled();
                });

                it("should postInspectionUpdate if edit resolved", function () {
                    var editDeferred = $q.defer();
                    spyOn($scope, "allowEditing").and.returnValue(editDeferred.promise);

                    $scope.markPart(inspection, mark);
                    editDeferred.resolve();
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).toHaveBeenCalled();
                });

                it("should call unsplitAssembly with inspection if is split assm", function () {
                    spyOn($scope, "unsplitAssembly");
                    inspection.IsSplitAssembly = true;

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.unsplitAssembly.calls.argsFor(0)[0]).toBe(inspection);
                });

                it("should not call unsplitAssembly with inspection if is not split assm", function () {
                    spyOn($scope, "unsplitAssembly");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.unsplitAssembly).not.toHaveBeenCalled();
                });

                it("should log unhandled errors onto console", function() {
                    var errText = "unhandled error";
                    spyOn($scope, "allowEditing").and.callFake(createRejectOnce(errText));
                    spyOn(console, "log");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(console.log).toHaveBeenCalledWith("Unhandled error in markPart: " + errText);
                });

                it("should not log handled errors (where reject reason === $scope.CONSTANTS.ERROR_HANDLED) onto console" ,function() {
                    spyOn($scope, "allowEditing").and.callFake(createRejectOnce($scope.CONSTANTS.ERROR_HANDLED));
                    spyOn(console, "log");

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(console.log).not.toHaveBeenCalled();
                });

                it("calls postInspectionUpdate when allowEdit resolved", function() {
                    var allowDeferred = $q.defer();
                    spyOn($scope, "allowEditing").and.returnValue(allowDeferred.promise);

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).not.toHaveBeenCalled();
                    allowDeferred.resolve();
                    $scope.$digest();
                    expect($scope.postInspectionUpdate).toHaveBeenCalledWith(inspection);
                });

                it("calls postInspectionUpdate if allowEdit rejected w/ $scope.CONSTANTS.ERROR_HANDLED", function() {
                    spyOn($scope, "allowEditing").and.returnValue($q.reject($scope.CONSTANTS.ERROR_HANDLED));

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).toHaveBeenCalledWith(inspection);
                });

                it("does not loop if allowEdit rejected w/ $scope.CONSTANTS.ERROR_HANDLED", function() {
                    spyOn($scope, "allowEditing").and.returnValue($q.reject($scope.CONSTANTS.ERROR_HANDLED));

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.allowEditing.calls.count()).toBe(1);
                });

                it("does not call postInspectionUpdate if allowEdit rejected w/ 'cancel'", function() {
                    spyOn($scope, "allowEditing").and.callFake(createRejectOnce("cancel"));

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.postInspectionUpdate).not.toHaveBeenCalled();
                });

                it("loops to allowEdit a second time if first allowEdit resolved but postInspection rejects", function() {
                    $scope.postInspectionUpdate.and.callFake(createRejectOnce());
                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect($scope.allowEditing.calls.count()).toBe(2);
                });

                it("does not revert the values if POST fails", function() {
                    var $httpBackend,
                        firstPost = true;

                    inject(function(_$httpBackend_, $window) {
                        $httpBackend = _$httpBackend_;
                        spyOn($window, "alert");
                    });

                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());
                    $scope.postInspectionUpdate.and.callThrough();
                    $httpBackend.whenPOST($scope.serverVars.updateUrl).respond(function() {
                        expect(inspection.Result).toBe(mark);

                        if (firstPost) {
                            firstPost = false;
                            return [200, {Errors: {prop1: {errors: ["error for prop 1"]}}}, {}, "OK"];
                        } else {
                            return [200, {}, {}, "OK"];
                        }
                    });

                    $httpBackend.expectPOST($scope.serverVars.updateUrl);
                    $httpBackend.expectPOST($scope.serverVars.updateUrl);
                    inspection.Result = "OK";

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    $httpBackend.flush();
                });

                it("sets the updateFailed flag if POST fails and clears when POST passes", function() {
                    var $httpBackend,
                        firstPost = true;

                    inject(function(_$httpBackend_, $window) {
                        $httpBackend = _$httpBackend_;
                        spyOn($window, "alert");
                    });

                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());
                    $scope.postInspectionUpdate.and.callThrough();
                    $httpBackend.whenPOST($scope.serverVars.updateUrl).respond(function() {
                        if (firstPost) {
                            firstPost = false;
                            return [200, {Errors: {prop1: {errors: ["error for prop 1"]}}}, {}, "OK"];
                        } else {
                            expect(inspection.updateFailed).toBe(true);

                            return [200, {}, {}, "OK"];
                        }
                    });

                    $httpBackend.expectPOST($scope.serverVars.updateUrl);
                    $httpBackend.expectPOST($scope.serverVars.updateUrl);
                    inspection._previousInspectionValues = { ReasonRepaired: "Not repaired" };
                    inspection.Result = "OK";

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    $httpBackend.flush();
                    expect(inspection.updateFailed).toBe(false);
                });

                it("clears updateFailed if was prev set", function() {
                    inspection.updateFailed = true;
                    spyOn($scope, "allowEditing").and.returnValue($q.resolve());

                    $scope.markPart(inspection, mark);
                    $scope.$digest();

                    expect(inspection.updateFailed).toBe(false);
                });
            });

            function createRejectOnce(reason) {
                var firstCall = true;
                return function() {
                    if(firstCall) {
                        firstCall = false;
                        return $q.reject(reason);
                    }

                    return $q.resolve();
                };
            }
        });

        describe("$scope.postInspectionDelete", function() {
            var $httpBackend,
                $window;

            beforeEach(inject(function(_$httpBackend_, _$window_){
                $httpBackend = _$httpBackend_;
                $window = _$window_;

                spyOn($window, "alert");
            }));

            afterEach(function() {
                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();
            });

            it("posts to removeUrl w/ inspection ID", function() {
                var inspection = {PartInspectionID: 5};
                $httpBackend.expectPOST($scope.serverVars.removeUrl, {PartInspectionID: inspection.PartInspectionID})
                            .respond(200, {});

                $scope.postInspectionDelete(inspection);

                $httpBackend.flush();
            });

            it("returns http promise", function() {
                var httpResolved = null;
                var inspection = {PartInspectionID: 5};
                $httpBackend.expectPOST($scope.serverVars.removeUrl, {PartInspectionID: inspection.PartInspectionID})
                            .respond(200, {});

                $scope.postInspectionDelete(inspection)
                      .then(function() { httpResolved = true;});

                $httpBackend.flush();
            });

            it("sets serverOperationInProgress on inspection when split starts", function() {
                var inspection = {PartInspectionID: 454};
                $httpBackend.expectPOST($scope.serverVars.removeUrl, {PartInspectionID: inspection.PartInspectionID})
                    .respond(function() {
                        expect(inspection.serverOperationInProgress).toBe(true);
                        return 200;
                    });

                $scope.postInspectionDelete(inspection);

                $httpBackend.flush();
            });

            it("clears serverOperationInProgress on inspection when http ends", function() {
                var inspection = {PartInspectionID: 454};
                $httpBackend.expectPOST($scope.serverVars.removeUrl, {PartInspectionID: inspection.PartInspectionID})
                            .respond(200, {});

                $scope.postInspectionDelete(inspection);

                $httpBackend.flush();
                expect(inspection.serverOperationInProgress).toBe(false);
            });

            it("reverts changes if server responds with errors", function() {
                var prevValues = { Result: "OK", ReasonRepaired: "Not repaired"};
                var inspection = {PartInspectionID: 454, _previousInspectionValues: prevValues};
                $httpBackend.expectPOST($scope.serverVars.removeUrl)
                    .respond({Errors: {PropertyName: {errors: ["error message"]}}});

                $scope.postInspectionDelete(inspection);

                $httpBackend.flush();
                expect(inspection.ReasonRepaired).toBe(prevValues.ReasonRepaired);
                expect(inspection.Result).toBe(prevValues.Result);
            });

            it("alerts an error message w/ the part number and server error text if the POST fails", function() {
                var inspection = { PartInspectionID: 454, OriginalPartTemplateNumber: "33XR12" };
                $httpBackend.expectPOST($scope.serverVars.removeUrl)
                            .respond({Errors: {PropertyName: {errors: ["error message"]}}});

                $scope.postInspectionDelete(inspection);

                $httpBackend.flush();
                expect($window.alert).toHaveBeenCalled();
                var msg = $window.alert.calls.argsFor(0)[0];
                expect(msg).toContain(inspection.OriginalPartTemplateNumber);
                expect(msg).toContain("error message");
            });

            it("rejects downstream promise if errors with reason 'POST error'", function() {
                var rejected = false;
                var inspection = {PartInspectionID: 454};
                $httpBackend.expectPOST($scope.serverVars.removeUrl)
                    .respond({Errors: {PropertyName: {errors: ["error message"]}}});

                var promise = $scope.postInspectionDelete(inspection);
                promise.then(function(){},function(){rejected=true;});

                $httpBackend.flush();
                expect(rejected).toBe(true);
            });

            describe("when http returns", function() {
                describe("if success", function() {
                    it("removes inspections sent back in the 'Changes.Removed' list by the server", function() {
                        var inspection = { PartInspectionID: 23 },
                            child1 = { ParentAssemblyID: inspection.PartInspectionID },
                            child2 = { ParentAssemblyID: inspection.PartInspectionID },
                            child3 = { ParentAssemblyID: inspection.PartInspectionID };
                        $scope.inspections.push(child1, child2, child3);
                        $httpBackend.expectPOST($scope.serverVars.removeUrl)
                            .respond({Changes: { Removed: [child1, child2, child3]} });

                        $scope.postInspectionDelete(inspection);
                        $httpBackend.flush();

                        expect($scope.inspections).not.toContain(child1);
                        expect($scope.inspections).not.toContain(child2);
                        expect($scope.inspections).not.toContain(child3);
                    });
                });
            });
        });

        describe("$scope.postInspectionUpdate", function() {
            var $httpBackend,
                $window;

            beforeEach(inject(function(_$httpBackend_, _$window_){
                $httpBackend = _$httpBackend_;
                $window = _$window_;

                spyOn($window, "alert");
            }));

            it("posts to updateUrl w/ inspection values", function() {
                var inspection = {Id: 5};
                $httpBackend.expectPOST($scope.serverVars.updateUrl, inspection)
                            .respond(200, {});

                $scope.postInspectionUpdate(inspection);

                $httpBackend.flush();
            });

            it("returns http promise", function() {
                var httpResolved = null;
                var inspection = {Id: 5};
                $httpBackend.expectPOST($scope.serverVars.updateUrl, inspection)
                    .respond(200, {});

                $scope.postInspectionUpdate(inspection)
                      .then(function() { httpResolved = true;});

                $httpBackend.flush();
            });

            it("sets serverOperationInProgress on inspection when split starts", function() {
                var inspection = {PartInspectionID: 454};

                $scope.postInspectionUpdate(inspection);

                expect(inspection.serverOperationInProgress).toBe(true);
            });

            it("clears serverOperationInProgress on inspection when http ends", function() {
                var inspection = {PartInspectionID: 454};
                $httpBackend.expectPOST($scope.serverVars.updateUrl, inspection)
                            .respond(200, {});

                $scope.postInspectionUpdate(inspection);

                $httpBackend.flush();
                expect(inspection.serverOperationInProgress).toBe(false);
            });

            it("does not revert changes if server responds with errors (revert is handled by the markPart workflow)", function() {
                var prevVals = { Result: "OK", ReasonRepaired: "Not repaired"};
                var inspection = {PartInspectionID: 454, Result: prevVals.Result, ReasonRepaired: prevVals.ReasonRepaired, _previousInspectionValues: {Result:"Convert", ReasonRepaired:"MARKED"}};
                $httpBackend.expectPOST($scope.serverVars.updateUrl, inspection)
                            .respond({Errors: {PropertyName: {errors: ["error message"]}}});

                $scope.postInspectionUpdate(inspection);

                $httpBackend.flush();
                expect(inspection.ReasonRepaired).toBe(prevVals.ReasonRepaired);
                expect(inspection.Result).toBe(prevVals.Result);
            });

            it("alerts an error message w/ the part number and server error text if the POST fails", function() {
                var inspection = { PartInspectionID: 454, OriginalPartTemplateNumber: "33XR12" };
                $httpBackend.expectPOST($scope.serverVars.updateUrl, inspection)
                            .respond({Errors: {PropertyName: {errors: ["error message"]}}});

                $scope.postInspectionUpdate(inspection);

                $httpBackend.flush();
                expect($window.alert).toHaveBeenCalled();
                var msg = $window.alert.calls.argsFor(0)[0];
                expect(msg).toContain(inspection.OriginalPartTemplateNumber);
                expect(msg).toContain("error message");
            });

            it("rejects downstream promise if errors with reason 'POST error'", function() {
                var rejected = false;
                var inspection = {PartInspectionID: 454};
                $httpBackend.expectPOST($scope.serverVars.updateUrl, inspection)
                    .respond({Errors: {PropertyName: {errors: ["error message"]}}});

                var promise = $scope.postInspectionUpdate(inspection);
                promise.then(function(){},function(){rejected=true;});

                $httpBackend.flush();
                $scope.$digest();
                expect(rejected).toBe(true);
            });
        });

        describe("initial state (post auto init)", function () {
            var $timeout,
                $httpBackend,
                reasonRepairedRequest,
                inspectionsRequest;

            beforeEach(inject(function (_$timeout_, _$httpBackend_) {
                $timeout = _$timeout_;
                $httpBackend = _$httpBackend_;

                $scope.serverVars.reasonRepairedListUrl = "http://unit.test/ReasonRepaired/List";
                $scope.serverVars.readUrl = "http://unit.test/Inspection/List";
                (function configureHttpExpectationsForInit(){
                    inspectionsRequest = $httpBackend.expectPOST($scope.serverVars.readUrl);
                    inspectionsRequest.respond({resp: "response from beforeEach"});

                    reasonRepairedRequest = $httpBackend.expectPOST($scope.serverVars.reasonRepairedListUrl);
                    reasonRepairedRequest.respond({resp: "response from beforeEach"});
                }());
            }));

            afterEach(function () {
                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();
            });

            it("contains reasons repaired from remote server", function() {
                var response = {resp: "test response"};
                reasonRepairedRequest.respond(response);

                $timeout.flush();
                $httpBackend.flush();

                expect($scope.reasonsRepaired).toEqual(response);
            });

            it("contains reasons repaired from remote server", function() {
                var response = {resp: "test response"};
                reasonRepairedRequest.respond(response);

                $timeout.flush();
                $httpBackend.flush();

                expect($scope.reasonsRepaired).toEqual(response);
            });

            it("contains inspections in Data property from remote server", function() {
                var response = {Data:{resp: "inspections from server"}};
                inspectionsRequest.respond(response);

                $timeout.flush();
                $httpBackend.flush();

                expect($scope.inspections).toEqual(response.Data);
            });
        });

        describe("$scope.saveAll", function() {
            it("resolves promises for all inspections in edit mode (each edit posts its on save)", function() {
                spyOn($scope, "postInspectionUpdate").and.returnValue($q.when());
                var inspectionInEditMode = { inEditMode: true, _editDeferred: $q.defer(), __resolved: null };
                inspectionInEditMode._editDeferred.promise.then(function(){ inspectionInEditMode.__resolved = true; });
                var inspectionNotInEditMode = { inEditMode: false };
                $scope.inspections = [ inspectionInEditMode, inspectionNotInEditMode ];

                $scope.saveAll();
                $scope.$digest();

                expect(inspectionInEditMode.__resolved).toBe(true);
            });

            it("does not call postInspctionUpdate for not pending edits", function() {
                spyOn($scope, "postInspectionUpdate");
                var notPendingEdit = { inEditMode: false };
                $scope.inspections = [ notPendingEdit ];

                $scope.saveAll();

                expect($scope.postInspectionUpdate).not.toHaveBeenCalled();
            });

            it("returns a promise that resolves after all the pending edits resolve", function() {
                spyOn($scope, "postInspectionUpdate").and.returnValue($q.when());
                var pendingEdit1 = { inEditMode: true, _editDeferred: $q.defer(), __resolved: false};
                pendingEdit1._editDeferred.promise.then(function(){pendingEdit1.__resolved=true;});
                var pendingEdit2 = { inEditMode: true, _editDeferred: $q.defer(), __resolved: false};
                pendingEdit2._editDeferred.promise.then(function(){pendingEdit2.__resolved=true;});
                $scope.inspections = [ pendingEdit1, pendingEdit2 ];

                var promise = $scope.saveAll();

                var resolvedAfterEdits = false;
                promise.then(function(){resolvedAfterEdits=(pendingEdit1.__resolved && pendingEdit2.__resolved);});
                $scope.$digest();
                expect(resolvedAfterEdits).toBe(true);
            });

            it("rejects if one of the mark parts does not complete", function() {
                $scope.inspections = [
                    {Result: "OK", _markCompletedPromise: $q.reject()}
                ];

                var saveAllPromise = $scope.saveAll();

                expect(saveAllPromise).toBeRejected();
            });

            it("rejects if one of the mark parts is cancelled", function() {
                $scope.inspections = [
                    {Result: "OK", _markCompletedPromise: $q.reject("cancel")}
                ];

                var saveAllPromise = $scope.saveAll();

                expect(saveAllPromise).toBeRejected();
            });
        });

        describe("$scope.saveEdit", function () {
            it("does not call postInspectionUpdate (postInspectionUpdate is finaly promise in markPart)", function() {
                var inspection = { inEditMode: true, _editDeferred: $q.defer(), ReplacementPartInfo: {} };
                spyOn($scope, "postInspectionUpdate");

                $scope.saveEdit(inspection);

                expect($scope.postInspectionUpdate).not.toHaveBeenCalled();
            });

            it("resolves the _editDeferred deferred when sync completes", function () {
                var resolved = false;
                var inspection = { _editDeferred: $q.defer(), inEditMode: true, ReplacementPartInfo: {} };
                inspection._editDeferred.promise.then(function () { resolved = true; });

                $scope.saveEdit(inspection);
                $scope.$digest();

                expect(resolved).toBe(true);
            });
            
            it("copies replacementpartinfo to appropriate fields if editReplacementPartTemplateNumber true", function () {
                var inspection = {
                    inEditMode: true, _editDeferred: $q.defer(), editReplacementPartTemplateNumber: true,
                    ReplacementPartInfo: {
                        PartTemplateID: 1,
                        PartTemplateNumber: "number"
                    }};

                $scope.saveEdit(inspection);

                expect(inspection.PartReplacedID).toBe(inspection.ReplacementPartInfo.PartTemplateID);
                expect(inspection.ReplacementPartTemplateNumber).toBe(inspection.ReplacementPartInfo.PartTemplateNumber);
            });

            it("does not copy replacementpartinfo to appropriate fields if editReplacementPartTemplateNumber false", function () {
                var inspection = {
                    inEditMode: true, _editDeferred: $q.defer(), editReplacementPartTemplateNumber: false,
                    ReplacementPartInfo: {
                        PartTemplateID: 1,
                        PartTemplateNumber: "number"
                    }};

                $scope.saveEdit(inspection);

                expect(inspection.PartReplacedID).toBeUndefined();
                expect(inspection.ReplacementPartTemplateNumber).toBeUndefined();
            });

            it("does not throw if no replacementPartInfo and in editReplacementPartTemplateNumber (this happens in kendo before a part is chosen)", function () {
                var inspection = { inEditMode: true, _editDeferred: $q.defer(), editReplacementPartTemplateNumber: true };

                expect(function() { $scope.saveEdit(inspection); }).not.toThrow();
            });

            it("throws if not in edit mode", function () {
                var inspection = { inEditMode: false, _editDeferred: $q.defer() };

                expect(function () { $scope.saveEdit(inspection); }).toThrow();
            });

            it("throws if no _editDeferred", function () {
                var inspection = { inEditMode: true };

                expect(function () { $scope.saveEdit(inspection); }).toThrow();
            });
        });

        describe("$scope.splitAssembly", function() {
            var $httpBackend;

            beforeEach(inject(function (_$httpBackend_) {
                $httpBackend = _$httpBackend_;
                $scope.inspections = [];

                $scope.serverVars.splitAssmUrl = "http://test.unit/split/assembly";
            }));

            it("posts to splitAssmUrl w/ inspection ID", function() {
                var inspection = {PartInspectionID: 454};
                $httpBackend.expectPOST($scope.serverVars.splitAssmUrl, inspection.PartInspectionId)
                    .respond({Data:[]});

                $scope.splitAssembly(inspection);

                $httpBackend.flush();
            });

            it("sets serverOperationInProgress on inspection when split starts", function() {
                var inspection = {PartInspectionID: 454};

                $scope.splitAssembly(inspection);

                expect(inspection.serverOperationInProgress).toBe(true);
            });

            it("clears serverOperationInProgress on inspection when http ends", function() {
                var inspection = {PartInspectionID: 454};
                $httpBackend.expectPOST($scope.serverVars.splitAssmUrl, inspection.PartInspectionId)
                    .respond({Data:[]});

                $scope.splitAssembly(inspection);

                $httpBackend.flush();
                expect(inspection.serverOperationInProgress).toBe(false);
            });

            it("adds all inspections to $scope.inspections when HTTP completes", function() {
                var asm = {PartInspectionID: 341};
                var part1 = {PartInspectionID: 2};
                var part2 = {PartInspectionID: 3};
                var part3 = {PartInspectionID: 4};
                $scope.inspections.push(asm);
                $httpBackend.expectPOST($scope.serverVars.splitAssmUrl, asm.PartInspectionId)
                    .respond({Data: [part1,part2,part3]});

                $scope.splitAssembly(asm);

                $httpBackend.flush();
                expect($scope.inspections.length).toBe(4);
                expect($scope.inspections).toContain(asm);
                expect($scope.inspections).toContain(part1);
                expect($scope.inspections).toContain(part2);
                expect($scope.inspections).toContain(part3);
            });

            it("reloads sort order of $scope.inspections when HTTP completes", function() {
                var asm = {PartInspectionID: 341, SortOrder: 5};
                var part1 = {PartInspectionID: 2, SortOrder: 6};
                var part2 = {PartInspectionID: 3, SortOrder: 7};
                var part3 = {PartInspectionID: 4, SortOrder: 8};
                var updatedPart1 = {PartInspectionID: 2, SortOrder: 10};
                var updatedPart2 = {PartInspectionID: 3, SortOrder: 11};
                var updatedPart3 = {PartInspectionID: 4, SortOrder: 12};
                $scope.inspections.push(asm, part1, part2, part3);
                $httpBackend.expectPOST($scope.serverVars.splitAssmUrl, asm.PartInspectionId)
                    .respond({Data: [asm,updatedPart1,updatedPart2,updatedPart3]});

                $scope.splitAssembly(asm);

                $httpBackend.flush();
                expect(part1.SortOrder).toBe(updatedPart1.SortOrder);
                expect(part2.SortOrder).toBe(updatedPart2.SortOrder);
                expect(part3.SortOrder).toBe(updatedPart3.SortOrder);
            });

            it("returns promise that resolves when http resolves but after inspections are added", function() {
                var asm = {PartInspectionID: 25};
                var added = {PartInspectionID: 23};
                $scope.inspections.push(asm);
                $httpBackend.expectPOST($scope.serverVars.splitAssmUrl, asm.PartInspectionId)
                    .respond({Data: [added]});

                var splitPromise = $scope.splitAssembly(asm);

                splitPromise.then(function() {
                    expect($scope.inspections).toContain(added);
                });
                $httpBackend.flush();
            });
        });

        describe("$scope.suggestReplaceAssembly", function () {
            var modalService,
                $httpBackend;

            beforeEach(inject(function (_modalService_, _$httpBackend_) {
                modalService = _modalService_;
                $httpBackend = _$httpBackend_;
            }));

            afterEach(function () {
                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();
            });

            it("should call modalService open for suggest-replace-assembly dialog", function () {
                var modal = modalService.getModal("suggest-replace-assembly");
                spyOn(modal, "open");

                $scope.suggestReplaceAssembly({});

                expect(modal.open).toHaveBeenCalled();
            });

            it("should return then-able object", function () {
                var promise = $scope.suggestReplaceAssembly({});

                expect(promise.then).not.toBeUndefined();
            });

            it("should call splitAssembly with inspection when modal is cancel'ed ('some parts')", function () {
                spyOn($scope, "splitAssembly").and.returnValue({ then: function () { } });
                var inspection = { ins: "ins" };

                $scope.suggestReplaceAssembly(inspection);
                var modal = modalService.getModal("suggest-replace-assembly");
                modal.cancel();
                $scope.$digest();

                expect($scope.splitAssembly).toHaveBeenCalledWith(inspection);
            });

            it("should set inspection values to OK when modal is cancel'ed ('some parts')", function () {
                spyOn($scope, "splitAssembly").and.returnValue({ then: function () { } });
                var inspection = { CanBeRepresentedAsAssembly: true, Result: "OK", ReasonRepaired: "reason", ReplacementPartTemplateNumber: "replacement", ReplacementQuantity: 5, PartReplacedID: 12 };

                $scope.markPart(inspection, "Replace");
                $scope.$digest();
                var modal = modalService.getModal("suggest-replace-assembly");
                modal.cancel();
                $scope.$digest();

                expect(inspection.Result).toBe("OK");
                expect(inspection.ReplacementPartTemplateNumber).toBe("");
                expect(inspection.ReplacementQuantity).toBeNull();
                expect(inspection.PartReplacedID).toBeNull();
                expect(inspection.ReasonRepaired).toBeNull();
            });

            it("should mark inspection as IsSplitAssembly when modal is cancel'ed ('some parts')", function () {
                spyOn($scope, "splitAssembly").and.returnValue({ then: function () { } });
                var inspection = { CanBeRepresentedAsAssembly: true, IsSplitAssembly: false, Result: "OK", ReasonRepaired: "reason" };

                $scope.markPart(inspection, "Replace");
                $scope.$digest();
                var modal = modalService.getModal("suggest-replace-assembly");
                modal.cancel();
                $scope.$digest();

                expect(inspection.IsSplitAssembly).toBe(true);
            });

            describe("promise returned", function () {
                var modal;

                beforeEach(function () {
                    modal = modalService.getModal("suggest-replace-assembly");
                });

                it("should be rejected when modal is cancel'ed ('some parts') and http returns", function () {
                    var rejected = false;
                    $scope.serverVars.splitAssmUrl = "test split assm url";
                    $httpBackend.expectPOST($scope.serverVars.splitAssmUrl)
                                .respond({Data:[]});

                    modal.cancel();
                    var promise = $scope.suggestReplaceAssembly({});
                    promise.then(function(){},function () { rejected = true; });

                    $httpBackend.flush();
                    expect(rejected).toBe(true);
                });

                it("should be resolved when modal is closed", function () {
                    var rejected = false;
                    var promise = $scope.suggestReplaceAssembly({});
                    promise.then(function () { }, function () { rejected = true; });

                    modal.close();
                    $scope.$digest();

                    expect(rejected).toBe(true);
                });

                it("should be resolved when modal ok ('whole assembly') is clicked", function () {
                    var resolved = false;

                    modal.ok();
                    var promise = $scope.suggestReplaceAssembly({});
                    promise.then(function () { resolved = true; }, function () { });

                    $scope.$digest();
                    expect(resolved).toBe(true);
                });
            });
        });

        describe("$scope.syncInspectionOrder", function() {
            var $httpBackend;

            beforeEach(inject(function(_$httpBackend_){
                $httpBackend = _$httpBackend_;
                $scope.serverVars.syncInspectionOrderUrl = "http://test.unit/sync/order";
                $scope.serverVars.deliveryTicketID = 1;
            }));

            it("posts to $scope.serverVars.syncInspectionOrderUrl w/ $scope.serverVars.deliveryTicketID", function() {
                $httpBackend.expectPOST($scope.serverVars.syncInspectionOrderUrl, {id: $scope.serverVars.deliveryTicketID})
                            .respond(200);

                $scope.syncInspectionOrder();

                $httpBackend.flush();
            });

            it("reloads $scope.inspections w/ http response", function() {
                $scope.inspections = [{a:'a'}, {b:'b'}, {c:'c'}];
                var newInspections = [{d:"d"},{e:"e"},{f:"f"}];
                $httpBackend.expectPOST($scope.serverVars.syncInspectionOrderUrl)
                            .respond(newInspections);

                $scope.syncInspectionOrder();

                $httpBackend.flush();
                expect($scope.inspections).toEqual(newInspections);
            });
        });

        describe("$scope.unsplitAssembly", function () {
            var inspections;

            beforeEach(function() {
                $scope.inspections = inspections = [];
            });

            it("sets serverOperationInProgress on inspection when split starts", function() {
                var inspection = {PartInspectionID: 454};

                $scope.unsplitAssembly(inspection);

                expect(inspection.serverOperationInProgress).toBe(true);
            });

            it("clears serverOperationInProgress on inspection when all posts end", function() {
                spyOn($scope, "postInspectionUpdate").and.returnValue($q.when());
                var inspection = {PartInspectionID: 454};

                $scope.unsplitAssembly(inspection);

                $scope.$digest();
                expect(inspection.serverOperationInProgress).toBe(false);
            });

            it("sets IsSplitAssembly on the inspection to false", function() {
                var inspection = {IsSplitAssembly: true};

                $scope.unsplitAssembly(inspection);

                expect(inspection.IsSplitAssembly).toBe(false);
            });

            it("should call deleteInspection on items in $scope.inspections that matches parent ID", function () {
                spyOn($scope, "deleteInspection");
                var inspection = { PartInspectionID: 23 },
                    match1 = { ParentAssemblyID: inspection.PartInspectionID },
                    match2 = { ParentAssemblyID: inspection.PartInspectionID },
                    match3 = { ParentAssemblyID: inspection.PartInspectionID };
                inspections.push(inspection, match1, match2, match3);

                $scope.unsplitAssembly(inspection);

                expect($scope.deleteInspection).toHaveBeenCalledWith(match1, 1, true);
                expect($scope.deleteInspection).toHaveBeenCalledWith(match2, 2, true);
                expect($scope.deleteInspection).toHaveBeenCalledWith(match3, 3, true);
                expect($scope.deleteInspection).not.toHaveBeenCalledWith(inspection);
            });

            it("should not remove items that do not match parent ID", function () {
                var inspection = { PartInspectionID: 23 },
                    nonmatch1 = { ParentAssemblyID: 0 },
                    nonmatch2 = { ParentAssemblyID: 0 },
                    nonmatch3 = { ParentAssemblyID: 0 };
                inspections.push(inspection, nonmatch1, nonmatch2, nonmatch3);

                $scope.unsplitAssembly(inspection);

                expect(inspections).toContain(nonmatch1);
                expect(inspections).toContain(nonmatch2);
                expect(inspections).toContain(nonmatch3);
                expect(inspections).toContain(inspection);
            });

            it("returns promise which resolves when all posts resolve", function () {
                var inspection = { PartInspectionID: 23 },
                    match1 = { ParentAssemblyID: inspection.PartInspectionID },
                    match2 = { ParentAssemblyID: inspection.PartInspectionID };
                inspections.push(inspection, match1, match2);
                var resolved = false;
                var updatePostDeferred = $q.defer();
                var deletePostDeferred1 = $q.defer();
                var deletePostDeferred2 = $q.defer();
                spyOn($scope, "postInspectionUpdate").and.returnValue(updatePostDeferred.promise);
                spyOn($scope, "postInspectionDelete").and.callFake(function(){
                    if($scope.postInspectionDelete.calls.count() === 1) return deletePostDeferred1.promise;
                    else return deletePostDeferred2.promise;
                });

                var promise = $scope.unsplitAssembly(inspection);
                promise.then(function(){ resolved = true; });
                $scope.$digest();

                expect(resolved).toBe(false);
                updatePostDeferred.resolve();
                $scope.$digest();

                expect(resolved).toBe(false);
                deletePostDeferred1.resolve();
                $scope.$digest();

                expect(resolved).toBe(false);
                deletePostDeferred2.resolve();
                $scope.$digest();

                expect(resolved).toBe(true);
            });

            it("should call postInpsectionUpdate w/ assembly", function () {
                spyOn($scope, "postInspectionUpdate").and.callThrough();
                var inspection = {};

                $scope.unsplitAssembly(inspection);

                expect($scope.postInspectionUpdate).toHaveBeenCalledWith(inspection);
            });
        });

        describe("validateAndPost", function() {
            var validateAllItemsMarkedDeferred,
                form;

            beforeEach(function() {
                form = {
                    submit: jasmine.createSpy("submit")
                };

                validateAllItemsMarkedDeferred = $q.defer();
                spyOn($scope, "validateAllItemsMarked").and.returnValue(validateAllItemsMarkedDeferred.promise);
                spyOn(angular, "element").and.callFake(function (selector) { return (selector === "form[name='inspectionForm']") ? form : null; });
            });

            afterEach(function() {
                angular.element.and.callThrough();
            });

            it("should call validateAllItemsMarked", function() {
                $scope.validateAndPost();

                expect($scope.validateAllItemsMarked).toHaveBeenCalled();
            });

            describe("if validateAllItemsMarked resolves", function() {
                it("should post completeForm", function() {
                    validateAllItemsMarkedDeferred.resolve();

                    $scope.validateAndPost();
                    $scope.$digest();

                    expect(form.submit).toHaveBeenCalled();
                });
            });

            describe("if validateAllItemsMarked rejects", function() {
                it("should not post completeForm", function() {
                    validateAllItemsMarkedDeferred.reject();

                    $scope.validateAndPost();
                    $scope.$digest();

                    expect(form.submit).not.toHaveBeenCalled();
                });
            });
        });

        describe("test helper tools - getHandler", function(){
            it("should use bind as default registerMethodName", function(){
                var obj = { bind: jasmine.createSpy("bind") };
                var eventName = "my custom event";
                var handlerCalled = false;
                function handler() { handlerCalled = true; }

                obj.bind(eventName, handler);

                getHandler(obj, eventName).call();
                expect(handlerCalled).toBe(true);
            });

            it("should use specified registerMethodName", function(){
                var obj = { on: jasmine.createSpy("on") };
                var eventName = "my custom event";
                var handlerCalled = false;
                function handler() { handlerCalled = true; }

                obj.on(eventName, handler);

                getHandler(obj, eventName, "on").call();
                expect(handlerCalled).toBe(true);
            });

            it("should return a call-able object which invokes handler", function() {
                var obj = { bind: jasmine.createSpy("bind") };
                var eventName = "my custom event";
                var handlerCalled = false;
                function handler() { handlerCalled = true; }

                obj.bind(eventName, handler, "bind");

                var foundHandler = getHandler(obj, eventName);
                expect("call" in foundHandler).toBe(true);
                expect(handlerCalled).toBe(false);
                foundHandler.call();
                expect(handlerCalled).toBe(true);
            });

            it("should reutrn handler match by name", function() {
                var obj = { bind: jasmine.createSpy("bind") };
                var eventOne = {
                    name: "custome evt 1",
                    handlerCalled: false,
                    handler: function() { eventOne.handlerCalled = true; }
                };
                var eventTwo = {
                    name: "custom evt 2",
                    handlerCalled: false,
                    handler: function() { eventTwo.handlerCalled = true; }
                };

                obj.bind(eventOne.name, eventOne.handler);
                obj.bind(eventTwo.name, eventTwo.handler);

                getHandler(obj, eventOne.name).call();
                expect(eventOne.handlerCalled).toBe(true);
                expect(eventTwo.handlerCalled).toBe(false);
            });

            it("should return null on no match", function() {
                var obj = { bind: jasmine.createSpy("bind") };

                var handler = getHandler(obj, "event");

                expect(handler).toBe(null);
            });
        });

        describe("switchRepairMode", function () {

            beforeEach(function () {
                spyOn(util, "confirm").and.returnValue($q.resolve());
                spyOn($window.location, "assign");
            });

            it("should only switch if user confirmed he definitely wants to proceed.", function () {
                var response = { Success: true };
                $httpBackend.expectPOST($scope.serverVars.switchRepairModeUrl).respond(response);

                $scope.switchRepairMode();

                $httpBackend.flush();
            });

            it("should not switch if user does not confirm he wants to proceed.", function () {
                spyOn($window, "alert");
                util.confirm.and.returnValue($q.reject());

                $scope.switchRepairMode();

                $httpBackend.verifyNoOutstandingRequest();
            });

            it("should POST to the received server value's url with the delivery ticket id.", function () {
                var response = { Success: true };
                $httpBackend.expectPOST($scope.serverVars.switchRepairModeUrl, { id: $scope.serverVars.deliveryTicketID }).respond(response);

                $scope.switchRepairMode();

                $httpBackend.flush();
            });

            it("should redirect to the redirect url received from server on success.", function () {
                var response = { Success: true, RedirectUrl: "whatever" };
                $httpBackend.expectPOST($scope.serverVars.switchRepairModeUrl, { id: $scope.serverVars.deliveryTicketID }).respond(response);

                $scope.switchRepairMode();

                $httpBackend.flush();
                expect($window.location.assign).toHaveBeenCalledWith(response.RedirectUrl);
            });

            it("should alert the user of the errors if POST did not succeed.", function () {
                spyOn($window, 'alert');
                var response = { Success: false, Errors: [{ ErrorMessage: "whatever" }]};
                $httpBackend.expectPOST($scope.serverVars.switchRepairModeUrl, { id: $scope.serverVars.deliveryTicketID }).respond(response);

                $scope.switchRepairMode();

                $httpBackend.flush();
                expect($window.alert).toHaveBeenCalledWith('whatever/n');
            });
        });

        describe("checkAvailabilityInInventory", function () {
            it("returns false if customer doesn't use inventory", function () {
                $scope.serverVars.customerUsesInventory = false;
                var inspection = { PartReplacedID: 2 };

                var promise = $scope.checkAvailabilityInInventory(inspection);
                $scope.$digest();

                promise.then(function (available) {
                    expect(availabe).toBe(false);
                });
                $scope.$digest();
            });

            it("returns false if can't modify inventory", function () {
                $scope.serverVars.canModifyInventory = false;
                var inspection = { PartReplacedID: 2 };

                var promise = $scope.checkAvailabilityInInventory(inspection);
                $scope.$digest();

                promise.then(function (available) {
                    expect(availabe).toBe(false);
                });
                $scope.$digest();
            });

            it("returns inventoryService checkAvailability can modify inventory and there's a valid PartReplacedID", function () {                
                spyOn(inventoryService, "checkAvailability").and.returnValue($q.resolve(true));
                $scope.serverVars.canModifyInventory = true;
                var inspection = { PartReplacedID : 2};

                var promise = $scope.checkAvailabilityInInventory(inspection);
                $scope.$digest();

                promise.then(function (available) {
                    expect(availabe).toBe(true);
                });
                $scope.$digest();
            });

            it("return false if can modify inventory but there's no valid PartReplacedID", function () {
                $scope.serverVars.canModifyInventory = true;
                var inspection = {};

                var promise = $scope.checkAvailabilityInInventory(inspection);
                $scope.$digest();

                promise.then(function (available) {
                    expect(availabe).toBe(false);
                });
                $scope.$digest();
            });
        });

        describe("usePartFromInventory", function () {
            it("calls inventory service with url and inspection", function () {
                spyOn(inventoryService, "usePartFromInventory").and.returnValue($q.resolve());
                var inspection = {};

                $scope.usePartFromInventory(inspection);
                $scope.$digest();

                expect(inventoryService.usePartFromInventory).toHaveBeenCalledWith($scope.serverVars.updateUsingPartFromInventoryUrl, inspection);
            });

            it("sets replacedPartID and available from service response if call succeeds", function () {
                var qResponse = { replacedWithInventoryPartID: 1, availableInInventory : false};
                spyOn(inventoryService, "usePartFromInventory").and.returnValue($q.resolve(qResponse));
                var inspection = {};

                var promise = $scope.usePartFromInventory(inspection);
                $scope.$digest();

                promise.then(function (response) {                    
                    expect(inspection.ReplacedWithInventoryPartID).toBe(1);
                    expect(inspection.AvailableInInventory).toBe(false);
                });
                $scope.$digest();
            });

            it("sets alerts with received errors if service call fails", function () {
                var qResponse = {Errors: [{ ErrorMessage: "whatever" }] };
                spyOn(inventoryService, "usePartFromInventory").and.returnValue($q.reject(qResponse));
                spyOn($window, "alert");
                var inspection = {};

                var promise = $scope.usePartFromInventory(inspection);
                $scope.$digest();

                promise.then(function (response) {
                    expect($window.alert).toHaveBeenCalled();
                });
                $scope.$digest();
            });

            it("calls and returns checkAvailabilityInInventory when service call fails", function () {
                var qResponse = { Errors: [{ ErrorMessage: "whatever" }] };
                spyOn(inventoryService, "usePartFromInventory").and.returnValue($q.reject(qResponse));
                spyOn($window, "alert");
                spyOn($scope, "checkAvailabilityInInventory");

                var inspection = {};

                var promise = $scope.usePartFromInventory(inspection);
                $scope.$digest();

                promise.then(function (response) {
                    expect($scope.checkAvailabilityInInventory).toHaveBeenCalledWith(inspection);
                });
                $scope.$digest();

            });
        }); 

        describe("loadOriginalPartCustomerOwned", function () {
            it("calls inventory service with parameters.", function () {
                spyOn(inventoryService, "loadOriginalPartsCustomerOwned").and.returnValue($q.resolve());

                $scope.loadOriginalPartCustomerOwned(1, 2, 3);
                $scope.$digest();

                expect(inventoryService.loadOriginalPartsCustomerOwned).toHaveBeenCalledWith($scope.serverVars.listPartsInUseForPumpUrl, 1, 2, 3);
            });

            it("loops through response on success and sets inspection.OriginalCustomerOwnedPartID when found.", function () {
                var serviceResponse = {
                    data: [
                        { TemplatePartDefID: 1, ReplacedWithInventoryPartID: 3 },
                        { TemplatePartDefID: 2, ReplacedWithInventoryPartID: null }
                    ]};
                spyOn(inventoryService, "loadOriginalPartsCustomerOwned").and.returnValue($q.resolve(serviceResponse));
                $scope.inspections = [
                    { TemplatePartDefID: 1 },
                    { TemplatePartDefID: 2 }
                ];

                var promise = $scope.loadOriginalPartCustomerOwned(1, 2, 3);

                $scope.$digest();
                promise.then(function () {
                    expect($scope.inspections[0].OriginalCustomerOwnedPartID).toBe(3);
                    expect($scope.inspections[1].OriginalCustomerOwnedPartID).toBe(undefined);
                });
                $scope.$digest();
            });
        });

        describe("checkInventoryList", function () {
            it("calls inventoryService with parameters", function () {
                spyOn(inventoryService, "checkAvailabilityForInspections").and.returnValue($q.resolve());
                $scope.inspections = {};

                $scope.checkInventoryList();
                $scope.$digest();

                expect(inventoryService.checkAvailabilityForInspections).toHaveBeenCalledWith($scope.serverVars.checkInventoryListUrl, $scope.serverVars.customerID, $scope.inspections);
            });

            it("loops through response on success return from service and sets AvailableInInventory for those inspections that where found.", function () {
                var response = [{ PartTemplateID: 2 }];
                spyOn(inventoryService, "checkAvailabilityForInspections").and.returnValue($q.resolve(response));
                $scope.inspections = [
                    { PartReplacedID: 4 },
                    { PartReplacedID: 2 }
                ];

                var promise = $scope.checkInventoryList();

                $scope.$digest();
                promise.then(function () {                    
                    expect($scope.inspections[0].AvailableInInventory).toBe(undefined);
                    expect($scope.inspections[1].AvailableInInventory).toBe(true);
                });
                $scope.$digest();
            });
        });

        describe("showInventoryButton", function () {
            it("returns false if can't modify inventory", function () {
                $scope.serverVars.canModifyInventory = false;
                var inspection = {
                    CanBeRepresentedAsAssembly: false,
                    HasParentAssembly: false,
                    inEditMode: false,
                    ReplacedWithInventoryPartID: 1,
                    Result: "Convert",
                    AvailableInInventory: true,
                    ReplacementQuantity: 1
                };

                var canShow = $scope.showInventoryButton(inspection);

                expect(canShow).toBe(false);
            });

            it("returns false if it's an assembly", function () {
                $scope.serverVars.canModifyInventory = true;
                var inspection = {
                    CanBeRepresentedAsAssembly: true,
                    HasParentAssembly: false,
                    inEditMode: false,
                    ReplacedWithInventoryPartID: 1,
                    Result: "Convert",
                    AvailableInInventory: true,
                    ReplacementQuantity: 1
                };

                var canShow = $scope.showInventoryButton(inspection);

                expect(canShow).toBe(false);
            });

            it("returns false if parent is an assembly", function () {
                $scope.serverVars.canModifyInventory = false;
                var inspection = {
                    CanBeRepresentedAsAssembly: false,
                    HasParentAssembly: true,
                    inEditMode: false,
                    ReplacedWithInventoryPartID: 1,
                    Result: "Convert",
                    AvailableInInventory: true,
                    ReplacementQuantity: 1
                };

                var canShow = $scope.showInventoryButton(inspection);

                expect(canShow).toBe(false);
            });

            it("returns false if it's in edit mode", function () {
                $scope.serverVars.canModifyInventory = false;
                var inspection = {
                    CanBeRepresentedAsAssembly: false,
                    HasParentAssembly: false,
                    inEditMode: true,
                    ReplacedWithInventoryPartID: 1,
                    Result: "Convert",
                    AvailableInInventory: true,
                    ReplacementQuantity: 1
                };

                var canShow = $scope.showInventoryButton(inspection);

                expect(canShow).toBe(false);
            });

            it("returns true if there's a ReplacedWithInventoryPartID", function () {
                $scope.serverVars.canModifyInventory = true;
                var inspection = {
                    CanBeRepresentedAsAssembly: false,
                    HasParentAssembly: false,
                    inEditMode: false,
                    ReplacedWithInventoryPartID: 6,
                    Result: "",
                    AvailableInInventory: false,
                    ReplacementQuantity: 0
                };

                var canShow = $scope.showInventoryButton(inspection);

                expect(canShow).toBe(true);
            });

            it("returns false if there's no ReplacedWithInventoryPartID", function () {
                $scope.serverVars.canModifyInventory = true;
                var inspection = {
                    CanBeRepresentedAsAssembly: false,
                    HasParentAssembly: false,
                    inEditMode: false,
                    ReplacedWithInventoryPartID: undefined,
                    Result: "",
                    AvailableInInventory: false,
                    ReplacementQuantity: 0
                };

                var canShow = $scope.showInventoryButton(inspection);

                expect(canShow).toBe(false);
            });

            it("returns true if it's a replace or convert and it's available and replacement quantity is 1", function () {
                $scope.serverVars.canModifyInventory = true;
                var inspection = {
                    CanBeRepresentedAsAssembly: false,
                    HasParentAssembly: false,
                    inEditMode: false,
                    ReplacedWithInventoryPartID: undefined,
                    Result: "Convert",
                    AvailableInInventory: true,
                    ReplacementQuantity: 1
                };

                var canShow = $scope.showInventoryButton(inspection);

                expect(canShow).toBe(true);
            });
            it("returns false if it's a replace or convert but it's not available and replacement quantity is 1", function () {
                $scope.serverVars.canModifyInventory = true;
                var inspection = {
                    CanBeRepresentedAsAssembly: false,
                    HasParentAssembly: false,
                    inEditMode: false,
                    ReplacedWithInventoryPartID: undefined,
                    Result: "Replace",
                    AvailableInInventory: false,
                    ReplacementQuantity: 1
                };

                var canShow = $scope.showInventoryButton(inspection);

                expect(canShow).toBe(false);
            });

            it("returns false if it's not replace or convert", function () {
                $scope.serverVars.canModifyInventory = true;
                var inspection = {
                    CanBeRepresentedAsAssembly: false,
                    HasParentAssembly: false,
                    inEditMode: false,
                    ReplacedWithInventoryPartID: undefined,
                    Result: "Trashed",
                    AvailableInInventory: true,
                    ReplacementQuantity: 1
                };

                var canShow = $scope.showInventoryButton(inspection);

                expect(canShow).toBe(false);
            });
            it("returns false if it's a replace or convert and it's available and replacement quantity is not 1", function () {
                $scope.serverVars.canModifyInventory = true;
                var inspection = {
                    CanBeRepresentedAsAssembly: false,
                    HasParentAssembly: false,
                    inEditMode: false,
                    ReplacedWithInventoryPartID: undefined,
                    Result: "Convert",
                    AvailableInInventory: true,
                    ReplacementQuantity: 4
                };

                var canShow = $scope.showInventoryButton(inspection);

                expect(canShow).toBe(false);
            });
        });
        
        function getHandler(from, handlerName, registerMethodName) {
            if(typeof(registerMethodName) === "undefined") registerMethodName = "bind";
            var handlerCalls = from[registerMethodName].calls.all();

            for (var i = 0; i < handlerCalls.length; i++) {
                if (handlerCalls[i].args[0] === handlerName) {
                    return {
                        call: handlerCalls[i].args[1]
                    };
                }
            }

            return null;
        }
    });
} ());