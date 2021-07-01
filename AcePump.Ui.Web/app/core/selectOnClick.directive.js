(function () {
    "use strict";

    angular
        .module("acePump.core")
        .directive("selectOnClick", selectOnClick);

    selectOnClick.$inject = [];
    function selectOnClick() {
        var directive = {
            restrict: "A",
            link: link
        };

        function link(scope, el, attrs) {
            el.on("focus", function (e) {
                e.target.select();
            });
        }

        return directive;
    }
})();