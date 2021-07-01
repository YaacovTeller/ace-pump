(function () {
    "use strict";

    angular
        .module("app.tickets")
        .config(configRouter);

    configRouter.$inject = ["$stateProvider", "$urlRouterProvider"];
    function configRouter($stateProvider, $urlRouterProvider) {
        $urlRouterProvider
            .when("", "/search")
            .when("/", "/search")
            .otherwise("/search");

        $stateProvider
            .state("top.search", {
                url: "/search",
                cache: false,
                templateUrl: "tickets/search.html",
                controller: "SearchController as searchCtrl"
            })

            .state("top.details", {
                url: "/details/{deliveryTicketId}",
                cache: false,
                templateUrl: "tickets/details.html",
                controller: "DetailsController as detailsCtrl"
            })

            .state("top.viewPicture", {
                url: "/viewPicture",
                templateUrl: "tickets/viewPicture.html",
                controller: "ViewPictureController as viewPictureCtrl"
            })

            .state("top.takePicture", {
                url: "/takePicture/{deliveryTicketId}",
                cache: false,
                templateUrl: "tickets/takePicture.html",
                controller: "TakePictureController as takePictureCtrl"            
            })

            .state("top.signature", {
                url: "/signature/{deliveryTicketId}",
                cache: false,
                templateUrl: "tickets/signature.html",
                controller: "SignatureController as signatureCtrl"
            });
    }
})();