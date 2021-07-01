(function () {
    "use strict";

    angular
        .module("app.core")
        .directive("acpSignaturePad", acpSignaturePad);

    acpSignaturePad.$inject = ["$", "SignaturePad"];
    function acpSignaturePad($, SignaturePad) {
        var directive = {
            link: link,
            restrict: "E",
            require: "ngModel",
            template: "<canvas class=\"signature-pad\"></canvas>"
        };

        function link(scope, element, attrs, ngModelCtrl) {
            var signaturePad = new SignaturePad(element.find("canvas")[0]);

            signaturePad.onEnd = function (e) {
                var data = signaturePad.toDataURL().replace("data:image/png;base64,", "");
                ngModelCtrl.$setViewValue(data);
            };

            ngModelCtrl.$render = function (viewValue) {
                if (!viewValue && !signaturePad.isEmpty()) {
                    signaturePad.clear();
                }
            };

            if (attrs.required || attrs.ngRequired) {
                ngModelCtrl.$validators.required(function (modelValue, viewValue) {
                    return !viewValue.length;
                });
            }
            var domElement = element[0];
            domElement.addEventListener("touchstart", function (e) {
                e.preventDefault();
            }, { passive: false });
            domElement.addEventListener("touchend", function (e) {
                e.preventDefault();
            }, { passive: false });
            domElement.addEventListener("touchmove", function (e) {
                e.preventDefault();
            }, { passive: false });
        }
    
        return directive;
    }
})();