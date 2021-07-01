(function () {
    "use strict";

    angular
        .module("app.tickets")
        .factory("ticketsRepository", ticketsRepository);

    ticketsRepository.$inject = ["$cacheFactory", "$http", "$q"];
    function ticketsRepository($cacheFactory, $http, $q) {
        var service = {
            get: get,
            search: search
        };

        var _cache = $cacheFactory("ticketsRepository");
        function getOrCreateInCache(ticketId) {
            var entry = _cache.get(ticketId);
            if (typeof entry === "undefined") {
                //this shouldn't happen because the id came from the list that they searched for
            }

            return entry;
        }

        function get(ticketId) {
            return getOrCreateInCache(ticketId);
        }

        function search(id, limit) {
            _cache.removeAll();

            var filter = "";
            if (id) {
                filter = "DeliveryTicketID eq " + id;
            } else {
                filter += "CloseTicket ne true";
            }

            return $http({
                url: "/api/deliverytickets",
                method: "GET",
                params: {
                    $top: limit,
                    $filter: filter,
                    $orderby: "DeliveryTicketID desc"
                }
            })
                .then(function (httpResponse) {
                    for (var i = 0; i < httpResponse.data.length; i++) {
                        _cache.put(httpResponse.data[i].DeliveryTicketID, httpResponse.data[i]);
                    }
                    return httpResponse.data;
                });
        }

        return service;
    }
})();