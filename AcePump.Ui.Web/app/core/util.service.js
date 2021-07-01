(function () {
    "use strict";

    angular
        .module("acePump.core")
        .service("util", util);

    util.$inject = ["$q", "$window"];
    function util($q, $window) {
        var service = {
            confirm: confirm
        };

        function confirm(text) {
            if ($window.confirm(text)) {
                return $q.resolve();
            } else {
                return $q.reject();
            }
        }

        return service;
    }
})();