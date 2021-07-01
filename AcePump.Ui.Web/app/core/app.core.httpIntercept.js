(function () {
    "use strict";

    angular
        .module("acePump.core")
        .config(httpInterceptConfig);

    httpInterceptConfig.$inject = ["$httpProvider"];
    function httpInterceptConfig($httpProvider) {
        $httpProvider.interceptors.push(injectAuthToken);

        injectAuthToken.$inject = ["$cookies"];
        function injectAuthToken($cookies) {
            return {
                request: function (config) {
                    var webapitoken = $cookies.get("webapitoken");
                    if (webapitoken !== undefined) {
                        config.headers = config.headers || {};
                        config.headers.Authorization = "Bearer " + webapitoken;
                    }

                    return config;
                }
            };
        }
    }
})();