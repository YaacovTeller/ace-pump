(function(){
    "use strict";

    angular.module("acePump.core", ["kendo.directives", "soris.ui.components", "ngCookies"]);

    angular.module("acePump.core")
        .value("kendo", kendo);
}());