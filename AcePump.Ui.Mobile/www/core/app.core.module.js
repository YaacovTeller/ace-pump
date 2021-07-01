(function () {
    "use strict";

    angular.module("app.core", [
        "ionic",
        "ionic.native",
        "ngFileUpload",
        "ngCordova.plugins.nativeStorage"
    ]);

    angular
        .module("app.core")
        .value("$", angular.element)
        .value("SignaturePad", SignaturePad);
})();