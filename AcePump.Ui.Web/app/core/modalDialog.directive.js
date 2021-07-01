(function () {
    "use strict";

    angular
        .module("acePump.core")
        .directive("modalDialog", modalDialog);

    modalDialog.$inject = ["$window"];
    function modalDialog($window) {
        var directive = {
            restrict: "E",
            scope: {
                title: "@",
                okText: "@",
                cancelText: "@",
                modalId: "@"
            },
            template: "<div class=\"s-modal-backdrop\" ng-show=\"visible\">" +
                        "<div class=\"s-modal-container\" ng-show=\"visible\">" +
                            "<div class=\"s-modal-title-bar\">{{title}}</div>" +
                            "<div class=\"s-modal-content\"></div>" +
                            "<div class=\"s-modal-buttons\">" +
                                "<button ng-click=\"ok()\" type=\"button\">{{okText}}</button>" +
                                "<button ng-click=\"cancel()\" type=\"button\">{{cancelText}}</button>" +
                            "</div>" +
                        "</div>" +
                      "</div>",
            transclude: true,
            link: link,
            controller: ModalDialogController
        };

        ModalDialogController.$inject = ["$scope"];
        function ModalDialogController($scope) {
            $scope.ok = function () {
                $scope._closeDialog();
                $scope._broadcastModalSpecificEvent("ok");
            };

            $scope.cancel = function () {
                $scope._closeDialog();
                $scope._broadcastModalSpecificEvent("cancel");
            };

            $scope._closeDialog = function () {
                $scope.visible = false;
            };

            $scope._broadcastModalSpecificEvent = function (event) {
                var fullEventName = "modalService." + event + "." + $scope.modalId;
                $scope.$emit(fullEventName);
            };

            $scope.visible = false;
        }

        link.$inject = ["$scope", "elem", "attrs", "controller", "$transclude"];
        function link($scope, elem, attrs, controller, $transclude) {
            elem.find(".s-modal-content").append($transclude());

            var eventName = "modalService.open." + $scope.modalId;
            $scope.$on(eventName, function (e, params) {
                $scope.visible = true;
                _centerVertically();
            });

            angular.element($window).on("resize", _centerVertically);
            _centerVertically();

            function _centerVertically() {
                var contentDiv = elem.find(".s-modal-container");
                var newPageHeight = angular.element($window).height();
                var modalHeight = contentDiv.height();
                var verticalOffset = (newPageHeight - modalHeight) / 2;

                contentDiv.css("margin-top", verticalOffset);
            }
        }

        return directive;
    }
})();