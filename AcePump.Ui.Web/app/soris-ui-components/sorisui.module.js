(function(){
    "use strict";

    angular
        .module("soris.ui.components", ["kendo.directives"])
        .value("kendo", kendo)
        .value("$", jQuery);
}());