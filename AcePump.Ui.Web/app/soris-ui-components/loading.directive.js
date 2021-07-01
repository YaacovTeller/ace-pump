(function () {
    "use strict";

    angular
        .module("soris.ui.components")
        .directive("srsLoading", srsLoading);

    srsLoading.$inject = ["kendo"];
    function srsLoading(kendo) {
        var directive = {
            link: link,
            restrict: "A"
        };

        function link(scope, element, attrs) {
            if (element.css("position") === "static") element.css("position", "relative");

            scope.$watch(attrs.srsLoading, function (value) {
                kendo.ui.progress(element, value);
            });
        }

        return directive;
    }
})();