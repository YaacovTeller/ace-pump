(function () {
    "use strict";

    angular
        .module("app.core")
        .filter("percent", percentFilter);

    percentFilter.$inject = [];
    function percentFilter() {
        return function (value) {
            return (value * 100).toFixed(3) + "%";
        };
    }
})();
