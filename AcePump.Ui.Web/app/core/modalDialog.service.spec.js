(function(){
    "use strict";

    describe("modalService", function() {
        beforeEach(module("acePump.core"));

        var modalService,
            $rootScope;

        beforeEach(inject(function(_$rootScope_, _modalService_){
            $rootScope = _$rootScope_;
            modalService = _modalService_;
        }));

        describe("getModal", function(){
            it("should not broadcast the modalService open event", function() {
                spyOn($rootScope, "$broadcast");

                modalService.getModal("test modal id");

                expect($rootScope.$broadcast).not.toHaveBeenCalled();
            });

            it("should return a promise-able object", function() {
                var r = modalService.getModal("test modal id");

                expect(r.then).not.toBeUndefined();
            });
        });

        describe("ModalDialog", function () {
            var modalId,
                dialog;

            beforeEach(function() {
                modalId = "test modal id";
                dialog = modalService.getModal(modalId);
            });

            describe("initial state", function() {
                it("should be closed", function(){
                    var modal = modalService.getModal("different mdoal id");

                    expect(modal.state).toEqual("MODAL_DIALOG_STATE_CLOSED");
                });
            });

            describe("then", function() {
                it("should pass the onFulfilled/onRejected to the promise", function() {
                    var promiseThenSpy = jasmine.createSpy("then");
                    dialog._deferred.promise.then = promiseThenSpy;
                    var onFulfilled = function() {};
                    var onRejected = function() {};

                    dialog.then(onFulfilled, onRejected);
                    $rootScope.$digest();

                    expect(promiseThenSpy).toHaveBeenCalledWith(onFulfilled, onRejected);
                });
            });

            describe("open", function() {
                it("should broadcast open event", function() {
                    spyOn($rootScope, "$broadcast");

                    dialog.open();

                    expect($rootScope.$broadcast).toHaveBeenCalledWith("modalService.open." + modalId);
                });

                it("should not broadcast open event if dialog already open", function(){
                    dialog.open();
                    spyOn($rootScope, "$broadcast");

                    dialog.open();

                    expect($rootScope.$broadcast).not.toHaveBeenCalledWith("modalService.open." + modalId);
                });
            });

            describe("close", function() {
                it("should reject the promise with closed", function() {
                    var rejectReason = null;
                    dialog.then(function(r){}, function(r){rejectReason=r;});

                    dialog.close();
                    $rootScope.$digest();

                    expect(rejectReason).toEqual("closed");
                });

                it("should broadcast the modalService.close.modalId event", function() {
                    spyOn($rootScope, "$broadcast");

                    dialog.close();

                    expect($rootScope.$broadcast).toHaveBeenCalledWith("modalService.close." + modalId);
                });
            });

            it("should resolve the promise with OK when ok event fires", function() {
                var resolveReason = null;
                dialog.then(function(r){resolveReason = r;}, function(r){});

                $rootScope.$broadcast("modalService.ok." + modalId);
                $rootScope.$digest();

                expect(resolveReason).toEqual("ok");
            });

            it("should resolve the promise with cancel when cancel event fires", function() {
                var resolveReason = null;
                dialog.then(function(r){resolveReason = r;}, function(r){});

                $rootScope.$broadcast("modalService.cancel." + modalId);
                $rootScope.$digest();

                expect(resolveReason).toEqual("cancel");
            });

            it("should recreate deferrable when deferred resolved", function() {
                var originalDeferred = dialog._deferred;

                dialog._deferred.resolve();
                $rootScope.$digest();

                expect(dialog._deferred).not.toBe(originalDeferred);
            });

            it("should recreate deferrable when deferred rejected", function() {
                var originalDeferred = dialog._deferred;

                dialog._deferred.reject();
                $rootScope.$digest();

                expect(dialog._deferred).not.toBe(originalDeferred);
            });
        });
    });
}());