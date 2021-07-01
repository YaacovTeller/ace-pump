(function () {
    "use strict";

    angular
        .module("app.core")
        .controller("LoginController", LoginController);

    LoginController.$inject = ["environment", "$ionicLoading", "$ionicPopup", "$state", "user"];
    function LoginController(environment, $ionicLoading, $ionicPopup, $state, user) {
        var vm = this;
        vm.subtitle = environment.applicationSubtitle;

        vm.login = login;

        function login() {
            $ionicLoading.show({
                template: "Logging In..."
            });

            user
                .login(vm.username, vm.password)
                .then(function () {
                    $state.go("top.search");
                })
                .catch(function (err) {
                    $ionicPopup
                        .alert({
                            title: "Login Failed",
                            template: err
                        });
                })
                .finally(function () {
                    $ionicLoading.hide();
                });
        }
    }
})();