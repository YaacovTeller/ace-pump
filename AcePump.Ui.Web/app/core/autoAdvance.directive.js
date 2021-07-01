(function () {
    "use strict";

    angular
        .module("acePump.core")
        .directive("autoAdvance", autoAdvance);

    function autoAdvance() {
        var directive = {
            restrict: "A",
            link: link
        };

        link.$inject = ["$scope", "elem", "attrs"];
        function link($scope, elem, attrs) {
            var PRINTING_CHARACTER_MIN_CODE = 48;

            elem.on("keyup", function (e) {
                if (e.which < PRINTING_CHARACTER_MIN_CODE) return;

                var length = elem.val().length;
                var max = parseInt(attrs.maxlength);
                if (length === max) {
                    var textInputs = $("input[type=text]"),
                        ixCurrent = textInputs.index(e.target),
                        ixNext = ixCurrent + 1,
                        elNext = $("input[type=text]:nth(" + ixNext + ")");

                    elNext.focus();
                }
            });
        }
        return directive;
    }
})();