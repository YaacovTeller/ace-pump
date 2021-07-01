(function () {
    "use strict";

    angular
        .module("app.core")
        .directive("acpImgModalOnTap", acpImgModalOnTap);

    acpImgModalOnTap.$inject = ["$", "$ionicModal", "$parse"];
    function acpImgModalOnTap($, $ionicModal, $parse) {

        var directive = {
            link: link,
            restrict: "A",
            scope: true,
            priority: 100
        };

        function link(scope, element, attrs) {   
            var template =
                "<ion-modal-view ng-click=\"closeModal()\" >" +
                    "<ion-content>" +
                "<img style=\"display:block; margin:auto\" acp-api-src=" + "'" + attrs.acpImgModalOnTap + "'" + "  />" +
                    "</ion-content>" +
                "</ion-modal-view>";

            scope.gridModal = $ionicModal.fromTemplate(template,
                {
                    scope: scope,
                    animation: "slide-in-up"
                });

            scope.closeModal = function () {
                scope.gridModal.hide();
            }
        
            element.bind("click", function () {
                var overlayImg = $(scope.gridModal.el).find("img");

                if (overlayImg[0].clientWidth > overlayImg[0].clientHeight) {
                    overlayImg.css("width", "100vw");
                    overlayImg.css("height", "");

                } else {
                    overlayImg.css("width", "");
                    overlayImg.css("height", "100vh");
                }

                scope.gridModal.show();
            });
        }
    
        return directive;
    }
})();