(function () {
    "use strict";

    angular
        .module("acePump.core")
        .service("inventoryService", inventoryService);

    inventoryService.$inject = ["$http", "$q"];

    function inventoryService($http, $q) {
        var service = {
            checkAvailability: checkAvailability,
            usePartFromInventory: usePartFromInventory,
            checkAvailabilityForInspections: checkAvailabilityForInspections,
            loadOriginalPartsCustomerOwned: loadOriginalPartsCustomerOwned
    };

        function checkAvailability(url, partReplacedId, customerId) {
            return $http({
                method: "POST",
                url: url,
                responseType: "json",
                data: { id: partReplacedId, customerID: customerId }
            })
                .then(function (httpResponse) {
                    return httpResponse.data.Available;
                });                
        }

        function usePartFromInventory(url, inspection) {
            return $http({
                method: "POST",
                responseType: "json",
                url: url,
                data: {
                    model: inspection,
                    useInventory: !inspection.ReplacedWithInventoryPartID
                }
            })
                .then(function (httpResponse) {
                    if (httpResponse.data.Success) {
                        return {
                            replacedWithInventoryPartID: httpResponse.data.ReplacedWithInventoryPartID,
                            availableInInventory: httpResponse.data.AvailableInInventory
                        };
                    } else {
                        return $q.reject(httpResponse.data.Errors);
                    }
                });                
        }

        function checkAvailabilityForInspections(url, customerId, inspections) {
            return $q.when()
                .then(function () {
                    var partsToCheck = [];
                    for (var i = 0; i < inspections.length; i++) {
                        if (inspections[i].Result === "Replace" || inspections[i].Result === "Convert") {
                            partsToCheck[i] = inspections[i].PartReplacedID;
                        }
                    }

                    if (partsToCheck.length > 0) {
                        return $http({
                            method: "POST",
                            responseType: "json",
                            url: url,
                            data: { partTemplateIDs: partsToCheck, customerId: customerId }
                        })
                            .then(function (response) {
                                return response.data;
                            });
                    } else {
                        return [];
                    }
                });
        }

        function loadOriginalPartsCustomerOwned(url, pumpId, customerId, currentTicketDate) {
            return $q.when()
                .then(function() {
                    return $http({
                        method: "POST",
                        responseType: "json",
                        url: url,
                        data: { pumpID: pumpId, customerID: customerId, currentTicketDate: currentTicketDate }
                    })
                    .then(function (httpResponse) {
                        return httpResponse;
                    });
                })
                .catch(function(errors) {
                    return [];
                });
        }
        return service;
    }
})();