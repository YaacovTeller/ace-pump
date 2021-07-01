(function () {
    "use strict";

    angular
        .module("acePump.core")
        .directive("padInput", padInput);

    padInput.$inject = [];
    function padInput() {
        var directive = {
            restrict: "A",
            link: link,
            require: "ngModel"
        };

        var PAD_CHAR = " ";
        function link(scope, el, attrs, ngModelCtrl) {
            el.on("change", function (e) {
                var requiredLength = attrs.maxlength;
                var text = el.val();

                while (text.length < requiredLength) {
                    text += PAD_CHAR;
                }

                ngModelCtrl.$setViewValue(text);
                ngModelCtrl.$render();
            });
        }

        return directive;
    }
})();