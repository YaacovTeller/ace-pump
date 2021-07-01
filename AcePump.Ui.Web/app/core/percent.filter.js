(function(){
    "use strict";

    angular
        .module("acePump.core")
        .filter("percent", ["$filter", function ($filter) {
            var numberFilter = $filter("number");

            return function(input) {
                return numberFilter(input * 100) + "%";
            };
        }]);
}());