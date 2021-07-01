(function () {
    "use strict";

    angular
        .module("acePump.core")
        .config(config);

    config.$inject = ["$qProvider"];
    function config($qProvider) {
        if ($qProvider.errorOnUnhandledRejections) {
            $qProvider.errorOnUnhandledRejections(false);
        }
    }
})();