(function () {
    "use strict";

    angular
        .module("app.tickets")
        .controller("SearchController", SearchController);

    SearchController.$inject = ["$ionicLoading", "$http", "$q", "ticketsRepository", "$window"];
    function SearchController($ionicLoading, $http, $q, ticketsRepository, $window) {
        var vm = this;
        var loading; 

        vm.runSearch = runSearch;

        init();

        function init() {
            updateTickets(null, 10);               
        }

        function runSearch() {
            updateTickets(parseInt(vm.searchID), 10);
        }

        function updateTickets(id, limit) {
            $ionicLoading.show();

            ticketsRepository.search(id, limit)
                .then(function (results) {
                    vm.tickets = results;
                })
                .catch(function (httpError) {
                    $window.alert(JSON.stringify(httpError));
                })
                .finally(function () {
                    $ionicLoading.hide();
                });
        }        
    }
})();