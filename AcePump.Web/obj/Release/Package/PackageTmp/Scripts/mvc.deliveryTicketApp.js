(function(){
    "use strict";

    angular.module("acePump.core", ["kendo.directives"]);

    angular.module("acePump.core")
        .value("kendo", kendo);
}());
(function () {
    "use strict";

    angular
        .module("acePump.core")
        .config(config);

    config.$inject = ["$qProvider"];
    function config($qProvider) {
        $qProvider.errorOnUnhandledRejections(false);
    }
})();
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
(function () {
    "use strict";

    angular
        .module("acePump.core")
        .directive("modalDialog", modalDialog);

    modalDialog.$inject = ["$window"];
    function modalDialog($window) {
        var directive = {
            restrict: "E",
            scope: {
                title: "@",
                okText: "@",
                cancelText: "@",
                modalId: "@"
            },
            template: "<div class=\"s-modal-backdrop\" ng-show=\"visible\">" +
                        "<div class=\"s-modal-container\" ng-show=\"visible\">" +
                            "<div class=\"s-modal-title-bar\">{{title}}</div>" +
                            "<div class=\"s-modal-content\"></div>" +
                            "<div class=\"s-modal-buttons\">" +
                                "<button ng-click=\"ok()\" type=\"button\">{{okText}}</button>" +
                                "<button ng-click=\"cancel()\" type=\"button\">{{cancelText}}</button>" +
                            "</div>" +
                        "</div>" +
                      "</div>",
            transclude: true,
            link: link,
            controller: ModalDialogController
        };

        ModalDialogController.$inject = ["$scope"];
        function ModalDialogController($scope) {
            $scope.ok = function () {
                $scope._closeDialog();
                $scope._broadcastModalSpecificEvent("ok");
            };

            $scope.cancel = function () {
                $scope._closeDialog();
                $scope._broadcastModalSpecificEvent("cancel");
            };

            $scope._closeDialog = function () {
                $scope.visible = false;
            };

            $scope._broadcastModalSpecificEvent = function (event) {
                var fullEventName = "modalService." + event + "." + $scope.modalId;
                $scope.$emit(fullEventName);
            };

            $scope.visible = false;
        }

        link.$inject = ["$scope", "elem", "attrs", "controller", "$transclude"];
        function link($scope, elem, attrs, controller, $transclude) {
            elem.find(".s-modal-content").append($transclude());

            var eventName = "modalService.open." + $scope.modalId;
            $scope.$on(eventName, function (e, params) {
                $scope.visible = true;
                _centerVertically();
            });

            angular.element($window).on("resize", _centerVertically);
            _centerVertically();

            function _centerVertically() {
                var contentDiv = elem.find(".s-modal-container");
                var newPageHeight = angular.element($window).height();
                var modalHeight = contentDiv.height();
                var verticalOffset = (newPageHeight - modalHeight) / 2;

                contentDiv.css("margin-top", verticalOffset);
            }
        }

        return directive;
    }
})();
(function () {
    "use strict";

    angular
        .module("acePump.core")
        .directive("autoAdvance", autoAdvance);

    function autoAdvance() {
        var directive = {
            restrict: "A",
            link: link
        };

        link.$inject = ["$scope", "elem", "attrs"];
        function link($scope, elem, attrs) {
            var PRINTING_CHARACTER_MIN_CODE = 48;

            elem.on("keyup", function (e) {
                if (e.which < PRINTING_CHARACTER_MIN_CODE) return;

                var length = elem.val().length;
                var max = parseInt(attrs.maxlength);
                if (length === max) {
                    var textInputs = $("input[type=text]"),
                        ixCurrent = textInputs.index(e.target),
                        ixNext = ixCurrent + 1,
                        elNext = $("input[type=text]:nth(" + ixNext + ")");

                    elNext.focus();
                }
            });
        }
        return directive;
    }
})();
(function () {
    "use strict";

    angular
        .module("acePump.core")
        .directive("padInput", padInput);

    padInput.$inject = [];
    function padInput() {
        var directive = {
            restrict: "A",
            link: link,
            require: "ngModel"
        };

        var PAD_CHAR = " ";
        function link(scope, el, attrs, ngModelCtrl) {
            el.on("change", function (e) {
                var requiredLength = attrs.maxlength;
                var text = el.val();

                while (text.length < requiredLength) {
                    text += PAD_CHAR;
                }

                ngModelCtrl.$setViewValue(text);
                ngModelCtrl.$render();
            });
        }

        return directive;
    }
})();
(function () {
    "use strict";

    angular
        .module("acePump.core")
        .directive("selectOnClick", selectOnClick);

    selectOnClick.$inject = [];
    function selectOnClick() {
        var directive = {
            restrict: "A",
            link: link
        };

        function link(scope, el, attrs) {
            el.on("focus", function (e) {
                e.target.select();
            });
        }

        return directive;
    }
})();
(function () {
    "use strict";

    angular
        .module("acePump.core")
        .directive("acePumpLoading", acePumpLoading);

    acePumpLoading.$inject = ["kendo"];
    function acePumpLoading(kendo) {
        var directive = {
            link: link,
            restrict: "A"
        };

        function link(scope, element, attrs) {
            if (element.css("position") === "static") element.css("position", "relative");

            scope.$watch(attrs.acePumpLoading, function (value) {
                kendo.ui.progress(element, value);
            });
        }

        return directive;
    }
})();
(function(){
    "use strict";

    angular
        .module("acePump.core")
        .service("modalService", ["$rootScope", "$q", function($rootScope, $q) {
            var MODAL_STATE = {
                OPEN: "MODAL_DIALOG_STATE_OPEN",
                CLOSED: "MODAL_DIALOG_STATE_CLOSED"
            };

            function ModalDialog(id) {
                this.id = id;

                this.init();
            }

            ModalDialog.prototype.init = function() {
                this._initDeferred();

                var dialog = this;
                this._listenForModalSpecificEvent("ok", function() {
                    dialog.ok();
                });

                this._listenForModalSpecificEvent("cancel", function() {
                    dialog.cancel();
                });

                this.state = MODAL_STATE.CLOSED;
            };

            ModalDialog.prototype._initDeferred = function() {
                createReinitializeDefer(this, "_deferred");

                function createReinitializeDefer(initializeOnObject, propertyName) {
                    var reinitializingDefer = $q.defer();
                    reinitializingDefer.promise.finally(function(){
                        createReinitializeDefer(initializeOnObject, propertyName);
                    });

                    initializeOnObject[propertyName] = reinitializingDefer;
                }
            };

            ModalDialog.prototype.then = function(onFulfilled, onRejected) {
                this._deferred.promise.then(onFulfilled, onRejected);
            };

            ModalDialog.prototype.open = function(params) {
                if(this.state === MODAL_STATE.OPEN) return;
                this.state = MODAL_STATE.OPEN;

                this._broadcastModalSpecificEvent("open", params);
            };

            ModalDialog.prototype.close = function() {
                this.state = MODAL_STATE.CLOSED;

                this._deferred.reject("closed");

                this._broadcastModalSpecificEvent("close");
            };

            ModalDialog.prototype.ok = function() {
                this.state = MODAL_STATE.CLOSED;

                this._deferred.resolve("ok");
            };

            ModalDialog.prototype.cancel = function() {
                this.state = MODAL_STATE.CLOSED;

                this._deferred.resolve("cancel");
            };

            ModalDialog.prototype._broadcastModalSpecificEvent = function(eventName, eventParams) {
                var fullEventName = this._getModalSpecificEventName(eventName);

                if(typeof(eventParams) === "undefined"){
                    $rootScope.$broadcast(fullEventName);
                } else {
                    $rootScope.$broadcast(fullEventName, eventParams);
                }
            };

            ModalDialog.prototype._listenForModalSpecificEvent = function(eventName, listener) {
                var fullEventName = this._getModalSpecificEventName(eventName);
                $rootScope.$on(fullEventName, listener);
            };

            ModalDialog.prototype._getModalSpecificEventName = function(eventName) {
                return "modalService." + eventName + "." + this.id;
            };

            var _modalCache = {};
            function _getOrCreateModal(id) {
                if(!(id in _modalCache)) {
                    _modalCache[id] = new ModalDialog(id);
                }

                return _modalCache[id];
            }

            return {
                getModal: function(id) {
                    return _getOrCreateModal(id);
                }
            };
        }]);
}());
(function () {
    "use strict";

    angular
        .module("acePump.core")
        .service("plungerBarrelWearSerializationService", plungerBarrelWearSerializationService);

    function plungerBarrelWearSerializationService() {
        var service = {
            serialize: serialize,
            deserialize: deserialize,
            makeDeserializeIterator: makeDeserializeIterator
        };

        function serialize(arraysToDeserialize) {
            var serialized = "";
            for (var key in arraysToDeserialize) {
                if (arraysToDeserialize.hasOwnProperty(key)) {                                        
                    serialized += arraysToDeserialize[key].join('');
                }
            }
            return serialized;
        }

        function useOutFromPreviousPump(plungerBarrelWearToDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious) {

            var start, until, plungerCharsLength, firstPart;
            if (!(plungerOrigFromPrevious === undefined || plungerOrigFromPrevious === '') && plungerOrigFromPrevious.length === 30) {
                start = plungerOrigFromPrevious.length;                
                plungerBarrelWearToDeserialize = plungerOrigFromPrevious + plungerBarrelWearToDeserialize.substring(start, plungerBarrelWearToDeserialize.length);
            }

            if (!(barrelOrigFromPrevious === undefined || barrelOrigFromPrevious === '') && barrelOrigFromPrevious.length === 72) {
                plungerCharsLength = (2 * 3 * 15);
                start = plungerCharsLength + barrelOrigFromPrevious.length;                
                firstPart = plungerBarrelWearToDeserialize.substring(0, plungerCharsLength);
                plungerBarrelWearToDeserialize = firstPart + barrelOrigFromPrevious + plungerBarrelWearToDeserialize.substring(start, plungerBarrelWearToDeserialize.length);
            }
            return plungerBarrelWearToDeserialize;
        }
    
        function deserialize(plungerBarrelWearToDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious) {
            plungerBarrelWearToDeserialize = useOutFromPreviousPump(plungerBarrelWearToDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

            var it = new makeDeserializeIterator(plungerBarrelWearToDeserialize);
            var returnObj = {};
            var i = 0;
            returnObj.PlungerOrig = [];
            for (i = 0; i < 15; i++) {
                returnObj.PlungerOrig[i] = it.next().value;
            }
            returnObj.PlungerWearRepaired = [];
            for (i = 0; i < 15; i++) {
                returnObj.PlungerWearRepaired[i] = it.next().value;
            }

            returnObj.PlungerOut = [];
            for (i = 0; i < 15; i++) {
                returnObj.PlungerOut[i] = it.next().value;
            }

            returnObj.BarrelOrig = [];
            for (i = 0; i < 36; i++) {
                returnObj.BarrelOrig[i] = it.next().value;
            }

            returnObj.BarrelWearRepaired = [];
            for (i = 0; i < 36; i++) {
                returnObj.BarrelWearRepaired[i] = it.next().value;
            }

            returnObj.BarrelOut = [];
            for (i = 0; i < 36; i++) {
                returnObj.BarrelOut[i] = it.next().value;
            }

            return returnObj;
        }

        function makeDeserializeIterator(string) {
            var nextIndex = 0;

            return {
                next: function () {
                    if (nextIndex < string.length) {
                        var obj = {
                            value: string.substring(nextIndex, nextIndex + 2)
                        };
                        nextIndex += 2;
                        return obj;
                    }
                    else {
                        return null;
                    }
                }
            };
        }

        return service;
    }
})();
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
(function () {
    "use strict";

    angular
        .module("acePump.core")
        .service("util", util);

    util.$inject = ["$q", "$window"];
    function util($q, $window) {
        var service = {
            confirm: confirm
        };

        function confirm(text) {
            if ($window.confirm(text)) {
                return $q.resolve();
            } else {
                return $q.reject();
            }
        }

        return service;
    }
})();
(function(){
    "use strict";

    angular.module("acePump.backOffice", ["acePump.core"]);
}());
(function(){
    "use strict";

    angular.module("acePump", ["acePump.backOffice"]);
}());
(function(){
    "use strict";

    angular
        .module("acePump.backOffice")
        .directive("deliveryTicketIntegration", deliveryTicketIntegration);
    
    function deliveryTicketIntegration() {
        return {
            restrict: "E",
            link: function(scope, element, attrs, controller) {
                var cmbCustomerId = $("#CustomerID").data("kendoComboBox");
                var nmSalesTaxRate = $("#SalesTaxRate").data("kendoNumericTextBox");
                var gridLineItems = $("#LineItemsGrid").data("kendoGrid");

                cmbCustomerId.bind("change", function(e) {
                    var custId = e.sender.value();
                    var custName = e.sender.text();
                    scope.$apply(function(scope){
                        scope.customerId = custId;
                        scope.customerName = custName;
                        scope.autoUpdateTaxRateBySelectedCustomer();
                    });
                });

                nmSalesTaxRate.bind("change", function(e) {
                    var rate = e.sender.value();
                    scope.$apply(function(scope){
                        scope.taxRate = rate;
                        scope.userEnteredTaxRate = true;
                    });
                });

                scope.$watch("taxRate", function(newVal, oldVal, scope){
                    nmSalesTaxRate.value(newVal);
                    gridLineItems.refresh();
                });
            }
        };
    }
})();
(function(){
    "use strict";

    angular
        .module("acePump.backOffice")
        .controller("DeliveryTicketController", DeliveryTicketController);

    DeliveryTicketController.$inject = ["$scope", "$http", "$q", "$window", "modalService"];
    function DeliveryTicketController($scope, $http, $q, $window, modalService) {
        /**
         * Queries the server for the default tax rate for this customer.
         * @param customerId
         * @returns (Promise) Resolves with rate when query completes, rejects if query errors.
         */
        $scope.queryCustomerDefaultTaxRate = function(customerId) {
            return $http({
                method: "POST",
                url: $scope.serverVars.queryCustomerTaxRateUrl,
                responseType: "json",
                data: { id: customerId }
            })
                .then(function(httpResponse){ return $q.resolve(httpResponse.data.DefaultSalesTaxRate); })
                .catch(function(httpResponse) { return $q.reject(httpResponse.data); });
        };

        /**
         * Copies the customer's default tax rate to be the rate on the ticket.
         */
        $scope.applyCustomerDefaultTaxRate = function() {
            if($scope.customerTaxRateAvailable) {
                $scope.taxRate = $scope.customerTaxRate;
                $scope.userEnteredTaxRate = false;
            }
        };

        /**
         * Sets the tax rate for the ticket based on these rules:
         *
         *      1. If no tax rate was ever typed in by the user, sets the rate to either
         *         the selected customer's default rate or the system default rate if no
         *         customer default is available (or if no customer is selected).
         *
         *      2. If the user typed in the rate and it is different than the default
         *         rate the system would have used, prompts the user to choose between the
         *         two of them.
         * @returns (Promise) Resolves when rate has been updated
         */
        $scope.autoUpdateTaxRateBySelectedCustomer = function() {
            var taxRateAvailable = $q.resolve(null);

            if("customerId" in $scope) {
                taxRateAvailable = $scope.queryCustomerDefaultTaxRate($scope.customerId);
            }

            taxRateAvailable
                .then(function(rate) {
                    var modalToUseId;
                    if(rate === null) { //no customerId or no default rate found on server
                        rate = $scope.serverVars.systemDefaultTaxRate;
                        $scope.customerTaxRateAvailable = false;
                        delete $scope.customerTaxRate;
                        modalToUseId = "choose-tax-rate-system";

                    } else {
                        $scope.customerTaxRate = rate;
                        $scope.customerTaxRateAvailable = true;
                        modalToUseId = "choose-tax-rate-customer";
                    }

                    if($scope.userEnteredTaxRate) {
                        if(rate !== $scope.taxRate) {
                            var modal = modalService.getModal(modalToUseId);
                            modal.open();
                            modal.then(function(reasonResolved) {
                                if(reasonResolved === "ok") { //customer default
                                    $scope.taxRate = rate;
                                    $scope.userEnteredTaxRate = false;
                                } // else do nothing
                            });
                        }

                    } else {
                        $scope.taxRate = rate;
                    }
                })
                .catch(function(reasonRejected) {
                    $window.alert("Sorry, the system could not automatically retrieve the default tax rate for this customer.  Please confirm what rate you want on the ticket.");
                });
        };
    }
})();