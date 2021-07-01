(function () {
    "use strict";

    angular
        .module("app.core")
        .config(configRoutes);

    configRoutes.$inject = ["$stateProvider", "$urlRouterProvider"];
    function configRoutes($stateProvider, $urlRouterProvider) {
        $urlRouterProvider
            .when("", "/home")
            .when("/", "/home")
            .otherwise("/home")
            .deferIntercept();

        $stateProvider
            .state("top", {
                abstract: true,
                template: "<ion-nav-view></ion-nav-view>",
                resolve: {
                    preload: waitForPreload
                }
            })

            .state("top.login", {
                url: "/login",
                templateUrl: "core/login.html",
                controller: "LoginController as loginCtrl",
                cache: false    
            })

            .state("top.logout", {
                url: "/logout",
                controller: ["$state", "user", function ($state, user) {
                    user
                        .logout()
                        .then(function () {
                            $state.go("top.login");
                        });
                }],
                cache: false
            });

        waitForPreload.$inject = ["$rootScope"];
        function waitForPreload($rootScope) {
            return $rootScope.whenPreloadComplete;
        }
    }
})();