(function(){
    "use strict";

    describe("modalDialog", function() {
        beforeEach(module("acePump.core"));

        var $compile,
            $rootScope;

        beforeEach(inject(function(_$compile_, _$rootScope_) {
            $compile = _$compile_;
            $rootScope = _$rootScope_;
        }));

        describe("genereated HTML", function(){
            it("should contain a backdrop bound to visible", function() {
                var element = createModal();
                var html = element.html();

                expect(html).toContain("<div class=\"s-modal-backdrop ng-hide\" ng-show=\"visible\">");
            });

            it("should contain a container bound to visible", function() {
                var element = createModal();
                var html = element.html();

                expect(html).toContain("<div class=\"s-modal-container ng-hide\" ng-show=\"visible\"");
            });

            it("should bind title passed in", function() {
                var title = "test title";
                var element = createModal("id", title);
                var html = element.html();

                expect(html).toContain("<div class=\"s-modal-title-bar ng-binding\">" + title + "</div>");
            });

            it("should bind message passed in", function() {
                var message = "test message";
                var element = createModal("id", "title", message);
                var html = element.html();

                expect(html).toContain("<div class=\"s-modal-content\">" + message + "</div>");
            });

            it("should bind ok text passed in", function() {
                var okText = "test ok text";
                var element = createModal("id", "title", "message", okText);
                var html = element.html();

                expect(html).toContain("<button ng-click=\"ok()\" type=\"button\" class=\"ng-binding\">" + okText + "</button>");
            });

            it("should bind cancel text passed in", function() {
                var cancelText = "test ok text";
                var element = createModal("id", "title", "message", "ok text", cancelText);
                var html = element.html();

                expect(html).toContain("<button ng-click=\"cancel()\" type=\"button\" class=\"ng-binding\">" + cancelText + "</button>");
            });
        });

        describe("controller", function() {
            var modalId = "controller modal id",
                element,
                compiled,
                $scope;

            beforeEach(function() {
                var html = createModalHtml(modalId);
                element = angular.element(html);
                compiled = $compile(element)($rootScope);
                $rootScope.$digest();
                $scope = element.isolateScope();
            });

            describe("ok", function() {
                it("should set visible to false", function() {
                    $scope.visible = true;

                    $scope.ok();

                    expect($scope.visible).toBe(false);
                });

                it("should emit modalService.ok.modalId", function() {
                    spyOn($scope, "$emit");

                    $scope.ok();

                    expect($scope.$emit).toHaveBeenCalledWith("modalService.ok." + modalId);
                });
            });

            describe("cancel", function() {
                it("should set visible to false", function() {
                    $scope.visible = true;

                    $scope.cancel();

                    expect($scope.visible).toBe(false);
                });

                it("should emit modalService.cancel.modalId", function() {
                    spyOn($scope, "$emit");

                    $scope.cancel();

                    expect($scope.$emit).toHaveBeenCalledWith("modalService.cancel." + modalId);
                });
            });
        });

        it("should set its $scope.visible to true when modalService.open.modalId fires", function() {
            var modalId = "myModalId";

            var element = createModal(modalId);
            var modalScope = element.isolateScope();
            $rootScope.$digest();

            modalScope.$emit("modalService.open." + modalId);
            modalScope.$digest();

            expect(modalScope.visible).toBe(true);
        });

        it("should have $scope.visible false to start", function() {
            var element = createModal();
            var modalScope = element.isolateScope();
            $rootScope.$digest();

            expect(modalScope.visible).toBe(false);
        });

        function createModalHtml(id, title, message, okText, cancelText) {
            if (typeof(id) === "undefined") id = "id";

            return "<modal-dialog " +
                        "modal-id=\"" + id + "\"" +
                        "title=\"" + title + "\"" +
                        "ok-text=\"" + okText + "\"" +
                        "cancel-text=\"" + cancelText + "\"" +
                   " >" + message + "</modal-dialog>";
        }

        function createModal(id, title, message, okText, cancelText) {
            var html = createModalHtml(id, title, message, okText, cancelText);

            var compiled = $compile(html)($rootScope);
            $rootScope.$digest();

            return compiled;
        }
    });
}());