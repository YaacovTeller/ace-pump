(function () {
    "use strict";

    angular
        .module("acePump.core")
        .directive("acePumpLoading", acePumpLoading);

    acePumpLoading.$inject = ["kendo"];
    function acePumpLoading(kendo) {
        var directive = {
            link: link,
            restrict: "A"
        };

        function link(scope, element, attrs) {
            if (element.css("position") === "static") element.css("position", "relative");

            scope.$watch(attrs.acePumpLoading, function (value) {
                kendo.ui.progress(element, value);
            });
        }

        return directive;
    }
})();