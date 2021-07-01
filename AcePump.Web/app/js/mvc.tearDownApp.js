(function(){
    "use strict";

    angular
        .module("soris.ui.components", ["kendo.directives"])
        .value("kendo", kendo)
        .value("$", jQuery);
}());
(function () {
    "use strict";

    angular
        .module("soris.ui.components")
        .factory("kendoWindowService", kendoWindowService);

    kendoWindowService.$inject = ["$", "$compile", "$controller", "$rootScope", "$templateRequest", "$q", "$window"];
    function kendoWindowService($, $compile, $controller, $rootScope, $templateRequest, $q, $window) {
        var MINIMIZE = "srs-minimize";

        var service = {
            CLOSE_REASON: {
                CANCEL: 1,
                COMPLETE: 2
            },

            KendoWindowWrapper: KendoWindowWrapper,
            modalInput: modalInput,
            multiCenter: multiCenter,
            open: open,
            setCreateWindowFactory: setCreateWindowFactory
        };

        var createWindowFactory = {
            create: function (options) {
                options = angular.extend(options || {}, { visible: false });
                return $("<div>")
                    .appendTo($("body"))
                    .kendoWindow(options)
                    .data("kendoWindow");
            }
        };

        function autowireSorisUiInteractions(window) {
            if (isSrsDesktopPresent()) {
                window.wrapper.find(".k-i-" + MINIMIZE)
                    .addClass("k-si-minus")
                    .click(function (e) {
                        var desktopScope = $("[srs-desktop]").scope();
                        desktopScope.ctrl.hideElement(window.element.closest(".k-window"));
                        desktopScope.$digest();

                        e.preventDefault();
                    });
            }
        }

        /**
         * Checks for existing other Soris UI components and auto wires integrations.
         */
        function autowireSorisUiInteractionsPrepOptions(options) {
            if (isSrsDesktopPresent() && !options.modal) {
                if (!options.actions) options.actions = ["Close"];
                options.actions.unshift(MINIMIZE);
            }
        }

        function isSrsDesktopPresent() {
            return $("[srs-desktop]").length > 0;
        }

        /**
         * Opens a modal dialog for user input and returns a promise which resolves with the input results.
         * options - { title:, message: "message to display inside the modal above the input", fields: { field1: "initial value" } }
         */
        function modalInput(options) {
            var templateBuilder = $("<div>");

            templateBuilder
                .append(
                $("<div>")
                    .addClass("container-fluid")
                    .append(
                    $("<p>").text(options.message)
                    )
                    .append(
                    $("<form>")
                        .addClass("form-horizontal")
                        .attr("name", "frm")
                        .attr("srs-use-bootstrap-validation-classes", "true")
                        .attr("ng-submit", "frm.$valid && window.complete()")
                    )
                );

            var fields = {};
            var form = templateBuilder.find("form");
            for (var kField in options.fields) {
                form.append(
                    $("<div>")
                        .addClass("form-group")
                        .append(
                        $("<label>")
                            .addClass("col-md-4 control-label")
                            .text(options.fields[kField].label)
                        )
                        .append(
                        $("<div>")
                            .addClass("col-md-8")
                            .append(
                            $("<input>")
                                .addClass("form-control")
                                .attr("type", "text")
                                .attr("ng-model", "fields." + kField)
                            )
                        )
                );

                fields[kField] = options.fields[kField].value;
            }

            form
                .append($(
                    "<div class=\"form-group\">" +
                    "<div class=\"col-md-offset-4 col-md-8\">" +
                    "<button class=\"btn btn-primary\"><span class=\"fa fa-check\"></span> {{ ::localize.Lbl_OK }}</button> " +
                    "<button class=\"btn btn-default\" type=\"button\" ng-click=\"window.close()\"><span class=\"fa fa-ban\"></span> {{ ::localize.Lbl_Cancel }}</button>" +
                    "</div>" +
                    "</div>"
                ));

            var modal = service.open({
                modal: true,
                scopeParams: { fields: fields },
                template: templateBuilder.html(),
                title: options.title
            });

            return modal
                .whenCompleted()
                .then(function () {
                    return fields;
                });
        }

        function multiCenter(margin) {
            if (arguments.length !== 3) throw new Error("multiCenter only supports two windows");
            var wrapper1 = arguments[1];
            var wrapper2 = arguments[2];

            var halfMargin = margin / 2;

            positionWindow(wrapper1.kendoWindow, "up");
            positionWindow(wrapper2.kendoWindow, "down");

            function positionWindow(window, offsetDir) {
                var options = {
                    width: window.options.width,
                    height: window.options.height,
                    position: {
                        top: window.options.position.top,
                        left: window.options.position.left
                    }
                };
                if (offsetDir === "up") {
                    options.height = ($window.innerHeight / 2) - margin;
                    options.position.top = halfMargin;
                } else {
                    options.height = ($window.innerHeight / 2) - margin;
                    options.position.top = ($window.innerHeight / 2) + halfMargin;
                }

                if (!options.width) {
                    options.width = $window.innerWidth - margin;
                    options.position.left = halfMargin;
                } else {
                    options.position.left = ($window.innerWidth / 2) - (options.width / 2) + (margin / 2);
                }

                window.setOptions(options);
                autowireSorisUiInteractions(window);
            }
        }

        /**
         * Opens a window overlay above the currently showing UI.  The window is creating with a new $scope.
         * @param {KendoWindowOpenOptions} options - { url: "template.html", title: "title", cssClass: "class", scopeParams: {}, modal: false, template: "<html>" }
         * @returns {KendoWindowWrapper}
         */
        function open(options) {
            options = angular.extend({}, options);
            if (!options.title) options.title = "Message";
            var nonKendoOptions = _extractNonKendoOptions(options);

            autowireSorisUiInteractionsPrepOptions(options);

            var kendoWindow = createWindowFactory.create(options);
            nonKendoOptions.scope.window = new KendoWindowWrapper(kendoWindow, nonKendoOptions.scope);

            $q.when()
                .then(function () {
                    if (options.template) {
                        return options.template;

                    } else {
                        return $templateRequest(nonKendoOptions.url);
                    }
                })
                .then(function (template) {
                    kendoWindow.content(template);
                    if (nonKendoOptions.cssClass) kendoWindow.element.closest(".k-window").addClass(nonKendoOptions.cssClass);
                    if (nonKendoOptions.controller) {
                        $controller(nonKendoOptions.controller, { $scope: nonKendoOptions.scope });
                    }

                    $compile(kendoWindow.element)(nonKendoOptions.scope);

                    autowireSorisUiInteractions(kendoWindow);

                    kendoWindow
                        .open()
                        .center();
                });

            return nonKendoOptions.scope.window;
        }

        function _extractNonKendoOptions(options) {
            var nonKendoOptions = {};

            nonKendoOptions.scope = $rootScope.$new();
            angular.extend(nonKendoOptions.scope, options.scopeParams);
            delete options.scopeParams;

            nonKendoOptions.url = options.url;
            delete options.url;

            nonKendoOptions.cssClass = options.cssClass;
            delete options.cssClass;

            nonKendoOptions.controller = options.controller;
            delete options.controller;

            return nonKendoOptions;
        }

        function setCreateWindowFactory(factory) {
            createWindowFactory = factory;
        }

        function KendoWindowWrapper(kendoWindow, scope) {
            this.kendoWindow = kendoWindow;
            this.scope = scope;

            this.init();
        }

        KendoWindowWrapper.prototype.center = function () {
            this.kendoWindow.center();
        };

        KendoWindowWrapper.prototype.cancel = function () {
            this.close(service.CLOSE_REASON.CANCEL);
        };

        KendoWindowWrapper.prototype.close = function (REASON, result) {
            if (!this._isRunFlags._finalizePromises) this._finalizePromises(REASON, result);
            if (!this._isRunFlags._closeKendoWindow) this._closeKendoWindow();
            if (!this._isRunFlags._cleanup) this._cleanup();
        };

        KendoWindowWrapper.prototype._closeKendoWindow = function () {
            this._isRunFlags._closeKendoWindow = true;
            this.kendoWindow.close();
        };

        KendoWindowWrapper.prototype._cleanup = function () {
            this._isRunFlags._cleanup = true;

            this.scope.$destroy();
            this.kendoWindow.element.remove();
            this.kendoWindow.destroy();
        };

        KendoWindowWrapper.prototype.complete = function (result) {
            this.close(service.CLOSE_REASON.COMPLETE, result);
        };

        KendoWindowWrapper.prototype._finalizePromises = function (REASON, result) {
            this._isRunFlags._finalizePromises = true;
            this._deferreds.whenClosed.resolve(result);

            if (REASON === service.CLOSE_REASON.CANCEL) {
                this._deferreds.whenCompleted.reject();

            } else if (REASON === service.CLOSE_REASON.COMPLETE) {
                this._deferreds.whenCompleted.resolve(result);

            } else {
                this._deferreds.whenCompleted.reject("UNKNOWN CLOSE REASON: " + REASON);
            }
        };

        KendoWindowWrapper.prototype.init = function () {
            var kwp = this;
            kwp._deferreds = {
                whenClosed: $q.defer(),
                whenCompleted: $q.defer(),
                whenOpened: $q.defer()
            };

            kwp._isRunFlags = {
                _cleanup: false,
                _closeKendoWindow: false,
                _finalizePromises: false
            };

            kwp.kendoWindow.bind("close", function () {
                kwp._closeKendoWindow._isRun = true;
                kwp.close(service.CLOSE_REASON.CANCEL);
            });

            kwp.kendoWindow.bind("open", function () {
                kwp._deferreds.whenOpened.resolve();
            });
        };

        KendoWindowWrapper.prototype.whenClosed = function () {
            return this._deferreds.whenClosed.promise;
        };

        KendoWindowWrapper.prototype.whenCompleted = function () {
            return this._deferreds.whenCompleted.promise;
        };

        KendoWindowWrapper.prototype.whenOpened = function () {
            return this._deferreds.whenOpened.promise;
        };

        return service;
    }
})();
(function () {
    "use strict";

    angular
        .module("soris.ui.components")
        .directive("srsLoading", srsLoading);

    srsLoading.$inject = ["kendo"];
    function srsLoading(kendo) {
        var directive = {
            link: link,
            restrict: "A"
        };

        function link(scope, element, attrs) {
            if (element.css("position") === "static") element.css("position", "relative");

            scope.$watch(attrs.srsLoading, function (value) {
                kendo.ui.progress(element, value);
            });
        }

        return directive;
    }
})();
(function(){
    "use strict";

    angular.module("acePump.core", ["kendo.directives", "soris.ui.components", "ngCookies"]);

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
        if ($qProvider.errorOnUnhandledRejections) {
            $qProvider.errorOnUnhandledRejections(false);
        }
    }
})();
(function () {
    "use strict";

    angular
        .module("acePump.core")
        .config(httpInterceptConfig);

    httpInterceptConfig.$inject = ["$httpProvider"];
    function httpInterceptConfig($httpProvider) {
        $httpProvider.interceptors.push(injectAuthToken);

        injectAuthToken.$inject = ["$cookies"];
        function injectAuthToken($cookies) {
            return {
                request: function (config) {
                    var webapitoken = $cookies.get("webapitoken");
                    if (webapitoken !== undefined) {
                        config.headers = config.headers || {};
                        config.headers.Authorization = "Bearer " + webapitoken;
                    }

                    return config;
                }
            };
        }
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
(function () {
    "use strict";

    angular
        .module("acePump.backOffice")
        .controller("TearDownController", TearDownController);

    TearDownController.$inject = ["$q", "$http", "$timeout", "util", "$window"];
    function TearDownController($q, $http, $timeout, util, $window) {

        var vm = this;

        vm.tearDownItems = [];
        vm.reasonsRepaired = [];
        vm.init = init;
        vm.setResult = setResult;
        vm.saveTearDownItem = saveTearDownItem;
        vm.switchRepairMode = switchRepairMode;
        vm.verifyAllMarked = verifyAllMarked;
        vm.completeTearDown = completeTearDown;

        function init(serverValues) {
            vm.serverValues = serverValues;

            loadTearDownItems();
            if (vm.serverValues.reasonRepairedListUrl) {
                loadReasonsRepaired();
            }
        }

        function loadTearDownItems() {
            return $http({
                method: "POST",
                responseType: "json",
                url: vm.serverValues.readUrl
            })
                .then(function (httpResponse) {
                    vm.tearDownItems = httpResponse.data.Data;
                    for (var i = 0; i < vm.tearDownItems.length; i++) {
                        var item = vm.tearDownItems[i];
                        if (item.Quantity !== 1 || item.CanBeRepresentedAsAssembly || item.HasParentAssembly) {
                            item.onlyTrash = true;
                        }
                    }
                })
                .then(function () {
                    verifyAllMarked();
                });
        }

        function loadReasonsRepaired() {
            return $http({
                method: "POST",
                responseType: "json",
                url: vm.serverValues.reasonRepairedListUrl
            })
            .then(function (response) {
                vm.reasonsRepaired = response.data;
            });
        }

        function setResult(item, result) {
            if (item.Quantity === 1) {
                if (result === "Trashed") {
                    item.Result = result;
                } else if (result==="Inventory") {
                    item.Result = result;
                    item.ReasonRepaired = "";
                } else {
                    throw new Error("Unknown result: " + result);
                }
                vm.saveTearDownItem(item);
            }
        }

        function saveTearDownItem(item) {
            return $http({
                method: "POST",
                responseType: "json",
                url: vm.serverValues.updateUrl,
                data: item
            })
                .then(function (response) {
                    verifyAllMarked();
                });
        }

        function switchRepairMode() {
            return util.confirm("CAUTION: Switching back to regular repair will delete all inspections for this tear down. Are you sure you want to continue?")
                .then(function () {
                    return $http({
                        method: "POST",
                        responseType: "json",
                        url: vm.serverValues.switchRepairModeUrl,
                        data: { id: vm.serverValues.deliveryTicketID }
                    })
                        .then(function (httpResponse) {
                            if (httpResponse.data.Success) {
                                $window.location.assign(httpResponse.data.RedirectUrl);
                            } else {
                                return $q.reject(httpResponse.data.Errors);
                            }
                        });
                })
                .catch(function (errors) {
                    $window.alert(errors);
                });
        }

        function verifyAllMarked() {
            vm.allMarked = !vm.tearDownItems.some(function (item) {
                return !item.Result;
            });
            return vm.allMarked;
        }

        function completeTearDown() {
            return $http({
                method: "POST",
                responseType: "json",
                url: vm.serverValues.completeTearDownUrl,
                data: {id: vm.serverValues.deliveryTicketID}
            })
                .then(function (httpResponse) {
                    if (httpResponse.data.Success) {
                        $window.location.assign(httpResponse.data.RedirectUrl);
                    } else {
                        return $q.reject(httpResponse.data.Errors);
                    }
                })
                .catch(function (errors) {
                    $window.alert(errors);
                });
        }
    }
})();
