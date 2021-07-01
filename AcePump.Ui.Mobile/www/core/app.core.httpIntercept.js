(function () {
    "use strict";

    angular
        .module("app.core")
        .config(httpInterceptConfig);

    httpInterceptConfig.$inject = ["$httpProvider"];
    function httpInterceptConfig($httpProvider) {
        $httpProvider.interceptors.push(injectAuthToken);
        $httpProvider.interceptors.push(injectApiUrl);

        var rgxApiUrl = /^\/api\//i,
            rgxAuthUrl = /^\/auth\//i;

        injectApiUrl.$inject = ["environment"];
        function injectApiUrl(environment) {
            return {
                request: function (config) {
                    config.url = config.url.replace(rgxApiUrl, environment.apiUrl);
                    config.url = config.url.replace(rgxAuthUrl, environment.apiUrl + "auth/");

                    return config;
                }
            };
        }

        injectAuthToken.$inject = ["user"];
        function injectAuthToken(user) {
            return {
                request: function (config) {
                    if (user.isLoggedIn && rgxApiUrl.test(config.url)) {
                        config.headers = config.headers || {};
                        config.headers.Authorization = "Bearer " + user.current.access_token;
                    }

                    return config;
                }
            };
        }
    }
})();