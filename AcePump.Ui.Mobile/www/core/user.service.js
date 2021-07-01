(function () {
    "use strict";

    angular
        .module("app.core")
        .service("user", user);

    user.$inject = ["$cordovaNativeStorage", "$injector", "$q", "$rootScope"];
    function user($cordovaNativeStorage, $injector, $q, $rootScope) {
        var service = {
            login: login,
            logout: logout,

            loadCachedLogin: loadCachedLogin
        };

        init();

        return service;

        function init() {
            service.isLoggedIn = false;
            service.current = null;
        }

        function login(username, password) {
            if (!username) username = "";
            if (!password) password = "";

            var $http = $injector.get("$http");
            return $http.post(
                "/auth/token",
                "grant_type=password&username=" + username + "&password=" +password,
                {
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded"
                    }
                }
            )
                .then(function (httpResponse) {
                    service.current = httpResponse.data;
                    service.isLoggedIn = true;

                    onLogin();

                    return $cordovaNativeStorage.setItem("current-user", service.current);
                })
                .catch(function (httpError) {
                    return $q.reject(httpError.data.error_description);
                });
        }

        function logout() {
            service.isLoggedIn = false;
            service.current = null;

            var $ionicHistory = $injector.get("$ionicHistory");
            $ionicHistory.clearCache();
            $ionicHistory.nextViewOptions({ disableBack: true });

            onLogout();
            
            return $cordovaNativeStorage.remove("current-user");
        }

        function loadCachedLogin() {
            return $cordovaNativeStorage.getItem("current-user")
                .then(function (cachedUser) {
                    service.current = cachedUser;
                    service.isLoggedIn = true;

                    onLoadCached();
                })
                .catch(function () {
                    //swallow missing native storage item
                    return $q.resolve();
                });
        }

        function onLogin() {
            $rootScope.$broadcast("auth.login");
        }

        function onLogout() {
            $rootScope.$broadcast("auth.logout");
        }

        function onLoadCached() {
            $rootScope.$broadcast("auth.cacheload");
        }
    }
})();