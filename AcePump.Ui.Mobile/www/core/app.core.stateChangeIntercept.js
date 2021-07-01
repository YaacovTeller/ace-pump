(function () {
    "use strict";

    angular
        .module("app.core")
        .run(stateChangeIntercept);

    stateChangeIntercept.$inject = ["$q", "$rootScope", "$state", "user", "$urlRouter"];
    function stateChangeIntercept($q, $rootScope, $state, user, $urlRouter) {
        $rootScope.$on("$locationChangeSuccess", function (evt, toUrl, fromUrl) {
            function noop() { }
            var action = {
                beforeSync: noop,
                afterSync: noop
            };

            if (!user.isLoggedIn) {
                action.beforeSync = function () { return $rootScope.whenPreloadComplete; };
            }

            if (action.beforeSync !== noop) {
                evt.preventDefault();

                $q.when()
                    .then(action.beforeSync)
                    .then(function () {
                        $urlRouter.sync();
                    })
                    .then(action.afterSync);
            }

        });
        $urlRouter.listen();

        $rootScope.$on("$stateChangeStart", function (e, toState, toParams, fromState) {
            if (!user.isLoggedIn && toState.name !== "top.login") {
                e.preventDefault();
                $state.go("top.login", {}, { location: true });
            }
        });
    }
})();   