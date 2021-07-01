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

    // Case 1054
    //
    // Created by YY Kosbie 11/22/2015
    //
    // Angular directive which connects the dataSource of a kendo combo box to a simple scope
    // function in the controller.  Kendo's 2015 Q1 implementation of an angular combo box creates sever
    // speed issues on a page by loading and managing the DOM for many data sources simultaneously.  This causes
    // a delay of several seconds before the page becomes responsive.  Use this directive to wrap your kendo
    // angular combo box and get access to the text changed and data source.
    //
    // IMPLEMENTATION NOTES: See kendo source kendo.angular.js line 720.  The actual widget is instatiated in
    // the post-link phase of kendo directive.  This means any changes we make to the HTML in the compile
    // template or pre-link phases will be overwritten by Kendo's jQuery DOM manipulation.  Also, this means
    // our post link will run in reverse priority so a higher priority runs *later* than a lower.
    //
    // Additionally, the kendo wrapper does *not* store the kendoComboBox object in  data("kendoComboBox")
    // like a traditional kendo widget.  Instead it is in data("handler") of the HTML element which contained
    // the kendo-combo-box directive.
    //
    // USAGE:
    //      Add a <kendo-combo-datasource-workaround> wrapper element around your <ANY kendo-combo-box> element.  Add a
    //      searchTextChanged to your scope so the combo can update it's options.  In form:
    //
    //          function searchTextChanged(text) {
    //              return Promise([newOptions]);
    //          }
    //
    //      Be sure to set the k-data-source to [] to begin with.
    //

    angular
        .module("acePump.core")
        .directive("kendoComboDatasourceWorkaround", kendoComboDatasourceWorkaround);

    kendoComboDatasourceWorkaround.$inject = ["$timeout"];
    function kendoComboDatasourceWorkaround($timeout) {
        var directive = {
            priority: -100,
            restrict: "E",
            transclude: true,
            scope: {},
            template:
                "<span class=\"kendo-combo-workaround-container\" ng-keyup=\"searchTextChanged_Internal()\">" +
                    "<div ng-transclude>" +
                    "</div>" +
                "</span>",
            link: link,
            controller: ComboWorkaroundController
        };

        ComboWorkaroundController.$inject = ["$scope"];
        function ComboWorkaroundController($scope) {
            $scope.searchTextChanged_Internal = function () {
                if ($scope._searchTimeout !== null) $timeout.cancel($scope._searchTimeout);
                $scope._searchTimeout = $timeout($scope.runSearch, 100, false);
            };

            $scope.runSearch = function () {
                var searchText = $scope.getSearchText();
                if ($scope._kendoComboBox.options.minLength > searchText.length) {
                    $scope._kendoComboBox.dataSource.data([]);
                    $scope._kendoComboBox.close();
                    return;
                }

                if ($scope._reqInProgress) return;
                $scope._setReqInProgress();
                $scope.$parent
                    .searchTextChanged(searchText)
                    .then(function (newOptions) {
                        $scope._clearReqInProgress();
                        $scope._kendoComboBox.dataSource.data(newOptions);

                        if (newOptions.length > 0) $scope._kendoComboBox.open();
                    });
            };

            $scope._setReqInProgress = function () {
                $scope._reqInProgress = true;

                var arrow = $scope._wrapperElement.find(".k-select");
                kendo.ui.progress(arrow, true);
                arrow.find(".k-loading-image").css("background-size", "contain");
            };

            $scope._clearReqInProgress = function () {
                $scope._reqInProgress = false;

                var arrow = $scope._wrapperElement.find(".k-select");
                kendo.ui.progress(arrow, false);
            };

        var vm = this;
        
        vm.PlungerOrig = [];
        vm.PlungerWearRepaired = [];
        vm.PlungerOut = [];

        vm.BarrelOrig = [];
        vm.BarrelWearRepaired = [];
        vm.BarrelOut = [];

        vm.init = init;
        vm.save = save;

        function init(serverValues) {
            vm.originalPlungerBarrelWear = serverValues.originalPlungerBarrelWear;
            vm.readUrl = serverValues.readUrl;
            vm.updateUrl = serverValues.updateUrl;
            vm.deliveryTicketID = serverValues.deliveryTicketID;

            if (vm.originalPlungerBarrelWear === null || vm.originalPlungerBarrelWear === "") {
                vm.originalPlungerBarrelWear = "" + createFixedLengthEmptyString(306) + "";
            }

            var arrays = plungerBarrelWearSerializationService.deserialize(vm.originalPlungerBarrelWear, serverValues.plungerOrigFromPreviousOut, serverValues.barrelOrigFromPreviousOut);
            vm.PlungerOrig = arrays.PlungerOrig;
            vm.PlungerWearRepaired = arrays.PlungerWearRepaired;
            vm.PlungerOut = arrays.PlungerOut;
            vm.BarrelOrig = arrays.BarrelOrig;
            vm.BarrelWearRepaired = arrays.BarrelWearRepaired;
            vm.BarrelOut = arrays.BarrelOut;
        }

        function createFixedLengthEmptyString(length) {
            var wrapper = new Array(length + 1);
            return wrapper.join(" ");
        }

        vm.init = init;
        vm.save = save;

        function init(serverValues) {
            vm.originalPlungerBarrelWear = serverValues.originalPlungerBarrelWear;
            vm.readUrl = serverValues.readUrl;
            vm.updateUrl = serverValues.updateUrl;
            vm.deliveryTicketID = serverValues.deliveryTicketID;

            if (vm.originalPlungerBarrelWear === null || vm.originalPlungerBarrelWear === "") {
                vm.originalPlungerBarrelWear = "" + createFixedLengthEmptyString(306) + "";
            }

            var arrays = plungerBarrelWearSerializationService.deserialize(vm.originalPlungerBarrelWear, serverValues.plungerOrigFromPreviousOut, serverValues.barrelOrigFromPreviousOut);
            vm.PlungerOrig = arrays.PlungerOrig;
            vm.PlungerWearRepaired = arrays.PlungerWearRepaired;
            vm.PlungerOut = arrays.PlungerOut;
            vm.BarrelOrig = arrays.BarrelOrig;
            vm.BarrelWearRepaired = arrays.BarrelWearRepaired;
            vm.BarrelOut = arrays.BarrelOut;
        }

        function createFixedLengthEmptyString(length) {
            var wrapper = new Array(length + 1);
            return wrapper.join(" ");
        }


        function save() {
            var toSave = {
                PlungerOrig: vm.PlungerOrig,
                PlungerWearRepaired: vm.PlungerWearRepaired,
                PlungerOut: vm.PlungerOut,
                BarrelOrig: vm.BarrelOrig,
                BarrelWearRepaired: vm.BarrelWearRepaired,
                BarrelOut: vm.BarrelOut
            };

            var serialized = plungerBarrelWearSerializationService.serialize(toSave);

            return $http({
                method: "POST",
                url: vm.updateUrl,
                responseType: "json",
                data: {
                    id: vm.deliveryTicketID,
                    plungerBarrelWear: serialized
                }
            })
                .then(function (httpResponse) {
                    if ($scope.frmPlungerWear) $scope.frmPlungerWear.$setPristine();
                })
                .catch(function (httpError) {
                    $window.alert(
                            "Was not able to update the plunger barrel wear!  Please try again.\n" +
                            httpError.status + ": " + httpError.statusText
                        );                    
                });
        }
    }
})();

(function () {
    "use strict";

    angular
        .module("acePump.backOffice")
        .controller("RepairTicketController", RepairTicketController);

    RepairTicketController.$inject = ["$http", "inventoryService", "modalService", "$q", "$scope", "$timeout", "util", "$window"];
    function RepairTicketController($http, inventoryService, modalService, $q, $scope, $timeout, util, $window) {
        $scope.CONSTANTS = {
            ERROR_HANDLED: "$scope.ERROR_HANDLED"
        };

        $scope.grid_Error = function (e) {
            if (e.errors) {
                var message = "";

                for (var propertyName in e.errors) {
                    var property = e.errors[propertyName];
                    if ("errors" in property) {
                        for (var i = 0; i < property.errors.length; i++) {
                            message += propertyName + ": " + property.errors[i] + "\n";
                        }
                    }
                }

                $scope.errorMessage = message;
                $scope._revertPartInspectionValues(e.model);
            }
        };

        /**
         * Validates that all items on the repair have been marked.  This automatically
         * saves any open editors and verifies that the save succeeded.
         * @returns {Promise} Resolves if validation passes, rejects if it fails.
         */
        $scope.validateAllItemsMarked = function () {
            var allInspectionsMarked = true;
            for (var i = 0; i < $scope.inspections.length; i++) {
                if (!$scope.inspections[i].Result) {
                    allInspectionsMarked = false;
                    $scope.inspections[i].updateFailed = true;
                }
            }

            if (allInspectionsMarked) {
                return $scope.saveAll();

            } else {
                $scope.alert("You must mark all parts on the repair ticket before you can complete it.");

                return $q.reject();
            }
        };

        /**
         * Validates the repair ticket is filled out properly and POSTs the entire page
         * to the complete repair page.
         */
        $scope.validateAndPost = function () {
            $scope.validateAllItemsMarked()
                .then(function () {
                    var element = angular.element("form[name='inspectionForm']");
                    element.submit();
                });
        };

        /**
         * Tries to save any open editors (inspections where "Convert" or "Replace" was selected and POST
         * the update to the server.
         * @returns {Promise} Resolves when all the open editors succeed in posting their values.  Rejects
         * if any of the editors fail.
         */
        $scope.saveAll = function () {
            var markCompletedPromises = [];
            for (var i = 0; i < $scope.inspections.length; i++) {
                var inspection = $scope.inspections[i];
                if (inspection._markCompletedPromise) markCompletedPromises.push(inspection._markCompletedPromise);

                if ($scope.inspections[i].inEditMode) {
                    $scope._prepareForSave($scope.inspections[i]);

                    inspection._editDeferred.resolve();

                    delete inspection._editDeferred;
                }
            }

            return $q.all(markCompletedPromises);
        };

                    }
                })
                .then(function () {

                                } else {
                                    return $q.reject(err);
                                }
                            });

                    } else {
                        return $scope.postInspectionUpdate(partInspection)
                            .catch(function (err) {
                                $scope._revertPartInspectionValues(partInspection);
                                return $q.reject(err);
                            });
                    }
                })
                .then(function () {
                    return $scope.checkAvailabilityInInventory(partInspection);
                })
                .then(function () {
                    partInspection.updateFailed = false;

                    markDeferred.resolve();

                    delete partInspection._markCompletedPromise;
                })
                .catch(function (error) {
                    if (error !== $scope.CONSTANTS.ERROR_HANDLED) {
                        markDeferred.reject();
                        console.log("Unhandled error in markPart: " + error);
                    }
                });
        };

        $scope.checkAvailabilityInInventory = function (inspection) {
            return $q.when()
                .then(function () {
                    if ($scope.serverVars.customerUsesInventory) {
                        var url = $scope.serverVars.checkAvailabilityInInventoryUrl;
                        var partReplacedId = inspection.PartReplacedID;
                        var customerId = $scope.serverVars.customerID;

                        if ($scope.serverVars.canModifyInventory && inspection.PartReplacedID) {
                            return inventoryService.checkAvailability(url, inspection.PartReplacedID, customerId);

                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                })
                .then(function (available) {
                    inspection.AvailableInInventory = available;
                    inspection.showInventoryButton = $scope.showInventoryButton(inspection);
                });
        };

        /**
         * Starts a loop to allow edit / post inspection until the post succeeds or the edit is cancelled.
         * @param partInspection the inspection to edit
         * @returns A promise which resolves when the part is editing and posted and rejects when the edit is cancelled
         * @private
         */
        $scope._runAllowEditAndPostLoop = function (partInspection) {
            var whileClosure = { updateSucceeded: false };
            var i = 0;
            return qWhile(
                function () { return whileClosure.updateSucceeded; },
                function (r) {
                    return $scope.allowEditing(partInspection)
                        .catch(function (allowEditError) {
                            if (allowEditError !== $scope.CONSTANTS.ERROR_HANDLED) return $q.reject(allowEditError);
                            else return $q.resolve();
                        })
                        .then(function () {
                            return $scope.postInspectionUpdate(partInspection)
                                .then(function () {
                                    whileClosure.updateSucceeded = true;
                                    partInspection.updateFailed = false;    //POST success
                                })
                                .catch(function (postError) {
                                    partInspection.updateFailed = true;
                                    return $q.resolve();                    //swallow POST error and rerun the loop
                                });
                        });
                });
        };

        /**
         * Fill in for "missing" $q.while.  Creates a promise which continues to run the body until
         * the condition is satisfied. The promise resolves when the conidition is satisifed.
         *
         * This implementation is based on http://stackoverflow.com/a/17238793/794234
         *
         * @param A function which returns a boolean indicating if the loop is complete.  Gets the result of the previous iteration as an argument.
         * @param A function which returns a promise.  The q waits for this promise before checking the condiition and runs again if needed.  Gets the result of the previous iteration as an argument.
         */
        function qWhile(condition, body) {
            var whileDeferred = $q.defer();

            function runLoopIteration(previousIterationResult) {
                var conditionSatisfied = condition(previousIterationResult);
                if (conditionSatisfied) {
                    whileDeferred.resolve(previousIterationResult);
                } else {
                    $q.when(body(previousIterationResult))
                        .then(function (iterationResult) { runLoopIteration(iterationResult); })
                        .catch(function (error) { whileDeferred.reject(error); });
                }
            }
            runLoopIteration();

            return whileDeferred.promise;
        }

        $scope._shouldSuggestAssemblySplit = function (partInspection, markAs) {
            return markAs === "Replace" && partInspection.ReasonRepaired !== "ROUT/MAINT" && partInspection.CanBeRepresentedAsAssembly;
        };

        $scope._shouldAllowEditingBeforePostUpdate = function (partInspection) {
            return (partInspection.Result === "Replace" && partInspection.ReasonRepaired !== "ROUT/MAINT") ||
                        partInspection.Result === "Convert";
        };

        $scope._revertPartInspectionValues = function (partInspection) {
            if (typeof (partInspection._previousInspectionValues) === "undefined") return;

            $scope._copyPartInspectionValues(partInspection._previousInspectionValues, partInspection);

            delete partInspection._previousInspectionValues;
        };

        $scope._copyPartInspectionValues = function (from, to) {
            to.Result = from.Result;
            to.ReasonRepaired = from.ReasonRepaired;
            to.ReplacementQuantity = from.ReplacementQuantity;
            to.ReplacementPartTemplateNumber = from.ReplacementPartTemplateNumber;
            to.PartReplacedID = from.PartReplacedID;
        };

        $scope.postInspectionUpdate = function (partInspection) {
            partInspection.serverOperationInProgress = true;
            return $http({
                method: "POST",
                url: $scope.serverVars.updateUrl,
                responseType: "json",
                data: partInspection
            })
            .then(function (httpResponse) {
                partInspection.serverOperationInProgress = false;
                partInspection.ReplacedWithInventoryPartID = null;
                if (!$scope._handleInspectionPostErrors(httpResponse.data, partInspection)) {
                    return $q.reject($scope.CONSTANTS.ERROR_HANDLED);
                }
            });
        };

        /**
         * POSTs to /DT/RemoveInspection with the inspetion that was deleted.  Does not
         * update the $scope.inspections array.
         *
         * When the POST returns, removes all other inspections sent by the server in
         * the 'removed' array of the response.  This allows the server to remove other
         * related assemblies and still keep the grid up to date without requiring us
         * to dupilcate logic between the client and server.
         *
         * @param partInspection
         * @returns {*}
         */
        $scope.postInspectionDelete = function (partInspection) {
            partInspection.serverOperationInProgress = true;

            return $http({
                method: "POST",
                url: $scope.serverVars.removeUrl,
                responseType: "json",
                data: { PartInspectionID: partInspection.PartInspectionID }
            })
            .then(function (httpResponse) {
                partInspection.serverOperationInProgress = false;

                if (!$scope._handleInspectionPostErrors(httpResponse.data, partInspection)) {
                    $scope._revertPartInspectionValues(partInspection);
                    return $q.reject($scope.CONSTANTS.ERROR_HANDLED);
                } else if ("Changes" in httpResponse.data && "Removed" in httpResponse.data.Changes && httpResponse.data.Changes.Removed.length > 0) {
                    $scope._removeInspectionsFromDisplay(httpResponse.data.Changes.Removed);
                }
            });
        };

        $scope.alert = function (txt) {
            $window.alert(txt);
        };

        /**
         * Removes the specified inspections from the $scope.inspections array without POSTing
         * the removal to the server.  Should only be used in the removal a) came from the server
         * or b) was already POSTed somewhere else in the code.
         * @param inspection
         * @private
         */
        $scope._removeInspectionsFromDisplay = function (inspections) {
            for (var ixScopeInspections = 0; ixScopeInspections < $scope.inspections.length; ixScopeInspections++) {
                for (var ixRemovedInspections = 0; ixRemovedInspections < inspections.length; ixRemovedInspections++) {
                    if ($scope.inspections[ixScopeInspections].PartInspectionID === inspections[ixRemovedInspections].PartInspectionID) {
                        $scope.inspections.splice(ixScopeInspections, 1);
                        ixScopeInspections--;

                        inspections.splice(ixRemovedInspections, 1);
                        ixRemovedInspections = inspections.length; // continue the outer for loop
                    }
                }
            }
        };

        $scope._handleInspectionPostErrors = function (responseData, partInspection) {
            if ("Errors" in responseData && responseData.Errors !== null) {
                var msg = "Could not save " + partInspection.OriginalPartTemplateNumber + "\n";

                for (var propertyName in responseData.Errors) {
                    var propertyErrorList = responseData.Errors[propertyName].errors;

                    for (var i = 0; i < propertyErrorList.length; i++) {
                        msg += "\t- " + propertyName + ": " + propertyErrorList[i] + "\n";
                    }
                }

                $scope.alert(msg);

                return false;
            } else {
                return true;
            }
        };

        $scope._setInspectionValuesByMark = function (partInspection, markAs) {
            partInspection._previousInspectionValues = {
                Result: partInspection.Result,
                ReasonRepaired: partInspection.ReasonRepaired,
                ReplacementQuantity: partInspection.ReplacementQuantity,
                ReplacementPartTemplateNumber: partInspection.ReplacementPartTemplateNumber,
                PartReplacedID: partInspection.PartReplacedID
            };

            switch (markAs) {
                case "OK":
                    partInspection.Result = "OK";
                    partInspection.ReasonRepaired = null;
                    partInspection.ReplacementPartTemplateNumber = "";
                    partInspection.ReplacementQuantity = null;
                    partInspection.PartReplacedID = null;
                    partInspection.ReplacedWithInventoryPartID = null;
                    break;

                case "NA":
                    partInspection.Result = "N/A";
                    partInspection.ReasonRepaired = "Did not inspect";
                    partInspection.ReplacementPartTemplateNumber = "";
                    partInspection.ReplacementQuantity = null;
                    partInspection.PartReplacedID = null;
                    partInspection.ReplacedWithInventoryPartID = null;
                    break;

                case "Maintenance":
                    partInspection.Result = "Replace";
                    partInspection.ReasonRepaired = "ROUT/MAINT";
                    partInspection.ReplacementPartTemplateNumber = partInspection.OriginalPartTemplateNumber;
                    partInspection.ReplacementQuantity = partInspection.Quantity;
                    partInspection.PartReplacedID = partInspection.OriginalPartTemplateID;
                    break;

                case "Convert":
                    partInspection.Result = "Convert";
                    partInspection.ReplacementQuantity = partInspection.Quantity;
                    partInspection.ReplacementPartTemplateNumber = "";
                    partInspection.PartReplacedID = null;
                    break;

                case "Replace":
                    partInspection.Result = "Replace";
                    partInspection.ReasonRepaired = null;
                    partInspection.ReplacementPartTemplateNumber = partInspection.OriginalPartTemplateNumber;
                    partInspection.ReplacementQuantity = partInspection.Quantity;
                    partInspection.PartReplacedID = partInspection.OriginalPartTemplateID;
                    break;

                default:
                    throw new Error("Unknown mark: " + markAs);
            }
        };

        $scope.unsplitAssembly = function (partInspection) {
            partInspection.IsSplitAssembly = false;
            partInspection.serverOperationInProgress = true;

            var postsToResolve = [];
            for (var i = 0; i < $scope.inspections.length; i++) {
                var item = $scope.inspections[i];
                if (item.ParentAssemblyID === partInspection.PartInspectionID) {
                    var deletePromise = $scope.deleteInspection(item, i, true);
                    postsToResolve.push(deletePromise);
                }
            }

            var updatePromise = $scope.postInspectionUpdate(partInspection);
            postsToResolve.push(updatePromise);

            var usnplitPromise = $q
                .all(postsToResolve)
                .then(function () {
                    partInspection.serverOperationInProgress = false;
                });
            return usnplitPromise;
        };

        /**
         * Deletes specified inspection and posts the delete to the server.
         * @param partInspection to delete
         * @param ix index in the $scope.inspections array.  If index is supplied partInspection is ignored
         * @param doNotConfirm true skip the confirmation dialog
         * @returns {*}
         */
        $scope.deleteInspection = function (partInspection, ix, doNotConfirm) {
            if (doNotConfirm !== true) if (!$window.confirm("Are you sure you want to delete this record?")) return $q.reject("cancelled");
            if (typeof (ix) === "undefined" || ix === null) {
                ix = $scope._getInpsectionIndex(partInspection);
            }
            partInspection = $scope.inspections[ix];

            return $scope.postInspectionDelete(partInspection)
                .then(function () {
                    if ($scope.inspections[ix] !== partInspection) ix = $scope._getInpsectionIndex(partInspection);
                    $scope.inspections.splice(ix, 1);
                });
        };

        $scope._getInpsectionIndex = function (inspection) {
            for (var i = 0; i < $scope.inspections.length; i++) {
                if ($scope.inspections[i].PartInspectionID === inspection.PartInspectionID) {
                    return i;
                }
            }

            throw new Error("Could not find matching inspection");
        };

        $scope.suggestReplaceAssembly = function (partInspection) {
            var deferred = $q.defer();

            var modal = modalService.getModal("suggest-replace-assembly");
            modal.open();
            modal.then(function (reasonResolved) {
                if (reasonResolved === "ok") { //whole assembly
                    deferred.resolve();

                } else if (reasonResolved === "cancel") { //some parts
                    deferred.reject($scope.CONSTANTS.ERROR_HANDLED);
                    $scope._setInspectionValuesByMark(partInspection, "OK");
                    partInspection.IsSplitAssembly = true;

                    $scope.splitAssembly(partInspection);
                }
            }, function (reasonRejected) {
                deferred.reject(reasonRejected);
            });

            return deferred.promise;
        };

        $scope.splitAssembly = function (inspection) {
            inspection.serverOperationInProgress = true;

            var splitAssemblyHttpPromise = $http({
                method: "POST",
                responseType: "json",
                url: $scope.serverVars.splitAssmUrl,
                data: { partInspectionId: inspection.PartInspectionID }
            })
            .then(function (httpResponse) {
                $scope._updateInspectionSortOrdersAndAddNew(httpResponse.data.Data);
                inspection.serverOperationInProgress = false;
            });

            return splitAssemblyHttpPromise;
        };

        $scope._updateInspectionSortOrdersAndAddNew = function (updatedInspections) {
            var partInspectionIdComparer = function (a, b) { return (a.PartInspectionID > b.PartInspectionID) ? 1 : -1; };
            var inspectionsClone = $scope.inspections.slice();
            updatedInspections = updatedInspections.sort(partInspectionIdComparer);
            inspectionsClone = inspectionsClone.sort(partInspectionIdComparer);

            for (var localIx = 0, updateIx = 0; updateIx < updatedInspections.length; updateIx++) {
                var localInspection = inspectionsClone[localIx];
                var updatedInspection = updatedInspections[updateIx];

                if (localIx >= inspectionsClone.length || inspectionsClone[localIx].PartInspectionID !== updatedInspections[updateIx].PartInspectionID) {
                    $scope.inspections.push(updatedInspection);

                } else {
                    localInspection.SortOrder = updatedInspection.SortOrder;
                    localIx++;
                }
            }
        };

        $scope.syncInspectionOrder = function () {
            $http({
                url: $scope.serverVars.syncInspectionOrderUrl,
                method: "POST",
                data: { id: $scope.serverVars.deliveryTicketID }
            })
            .then(function (response) {
                $scope.inspections = response.data;
            });
        };

        $scope.allowEditing = function (partInspection) {
            var editDeferred = $q.defer();
            editDeferred.promise.finally(function () {
                partInspection.editReplacementPartTemplateNumber = false;
                partInspection.inEditMode = false;

                delete partInspection._editDeferred;
            });

            partInspection.inEditMode = true;
            if (partInspection.Result === "Convert") partInspection.editReplacementPartTemplateNumber = true;
            partInspection.showInventoryButton = $scope.showInventoryButton(partInspection);


            partInspection._editDeferred = editDeferred;
            return editDeferred.promise;
        };

        $scope.cancelEdit = function (partInspection) {
            $scope._verifyInEditMode(partInspection);

            partInspection._editDeferred.reject("cancel");

            $scope._revertPartInspectionValues(partInspection);
        };

        $scope.saveEdit = function (partInspection) {
            $scope._verifyInEditMode(partInspection);
            $scope._prepareForSave(partInspection);

            partInspection._editDeferred.resolve();
        };

        $scope._prepareForSave = function (partInspection) {
            if (partInspection.editReplacementPartTemplateNumber && "ReplacementPartInfo" in partInspection) {
                partInspection.ReplacementPartTemplateNumber = partInspection.ReplacementPartInfo.PartTemplateNumber;
                partInspection.PartReplacedID = partInspection.ReplacementPartInfo.PartTemplateID;
            }
        };

        $scope._verifyInEditMode = function (partInspection) {
            if (!partInspection.inEditMode) throw new Error("not currently in edit mode!");
            if (typeof (partInspection._editDeferred) === "undefined") throw new Error("missing deferred could not continue");
        };

        $scope.searchTextChanged = function (text) {
            var textChangedDeferred = $q.defer();

            $http({
                url: $scope.serverVars.partsByNumberReadUrl,
                method: "POST",
                data: { text: text }
            })
            .then(function (response) {
                textChangedDeferred.resolve(response.data);
            });

            return textChangedDeferred.promise;
        };

        $scope.switchRepairMode = function () {
            return util.confirm("CAUTION: Switching to tear down will delete all inspections for this ticket. Are you sure you want to continue?")
                .then(function () {
                    return $http({
                        method: "POST",
                        responseType: "json",
                        url: $scope.serverVars.switchRepairModeUrl,
                        data: { id: $scope.serverVars.deliveryTicketID }
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
                    var message = '';
                    for (var i = 0; i < errors.length; i++) {
                        message = errors[i].ErrorMessage + '/n';
                    }

                    $window.alert(message);
                });
        };

        $scope.usePartFromInventory = function (inspection) {
            var url = $scope.serverVars.updateUsingPartFromInventoryUrl;
            return inventoryService.usePartFromInventory(url, inspection)
                .then(function (response) {
                    inspection.ReplacedWithInventoryPartID = response.replacedWithInventoryPartID;
                    inspection.AvailableInInventory = response.availableInInventory;
                })
                .catch(function (errors) {
                    var message = '';
                    for (var i = 0; i < errors.length; i++) {
                        message = errors[i].ErrorMessage + '\n';
                    }
                    $window.alert(message);
                    return $scope.checkAvailabilityInInventory(inspection);
                });
        };

        $scope._shouldAllowUsingInventory = function (inspection) {
            return (inspection.Result === "Replace" || inspection.Result === "Convert");
        };

        $scope.showInventoryButton = function (inspection) {
            var show = $scope.serverVars.canModifyInventory && !inspection.CanBeRepresentedAsAssembly && !inspection.HasParentAssembly && !inspection.inEditMode && (!!inspection.ReplacedWithInventoryPartID || ($scope._shouldAllowUsingInventory(inspection) && inspection.AvailableInInventory && parseInt(inspection.ReplacementQuantity) === 1));
            return show;
        };

        $scope.checkInventoryList = function () {
            var url = $scope.serverVars.checkInventoryListUrl;
            var customerId = $scope.serverVars.customerID;
            return inventoryService.checkAvailabilityForInspections(url, customerId, $scope.inspections)
                .then(function (response) {
                    for (var i = 0; i < $scope.inspections.length; i++) {
                        var currentInspection = $scope.inspections[i];
                        for (var j = 0; j < response.length; j++) {
                            if (response[j].PartTemplateID === currentInspection.PartReplacedID) {
                                currentInspection.AvailableInInventory = true;
                                j = response.length;
                            }
                        }
                        currentInspection.showInventoryButton = $scope.showInventoryButton(currentInspection);
                    }
                });
        };

        $scope.loadInspections = function (id) {
            return $http({
                method: "POST",
                responseType: "json",
                url: $scope.serverVars.readUrl,
                data: { id: id }
            })
                .then(function (response) {
                    $scope.inspections = response.data.Data;
                });
        };

        $scope.loadReasonsRepaired = function () {
            return $http({
                method: "POST",
                responseType: "json",
                url: $scope.serverVars.reasonRepairedListUrl
            })
            .then(function (response) {
                $scope.reasonsRepaired = response.data;
            });
        };

        $scope.loadPumpFailedTemplates = function () {
            return $http({
                method: "POST",
                responseType: "json",
                url: $scope.serverVars.listTemplatesUrl
            })
            .then(function (response) {
                $scope.templates = response.data;

                if ($scope.serverVars.pumpTemplateID) {
                    for (var i = 0; i < $scope.templates.length; i++) {
                        if ($scope.serverVars.pumpTemplateID === $scope.templates[i].PumpTemplateId) {
                            $scope.selectedTemplate = $scope.templates[i];
                            i = $scope.templates.length;
                        }
                    }
                }
            });
        };

        $scope.loadTemplates = function () {
            if ($scope.templatesLoaded) return $q.resolve();
            return $http({
                method: "POST",
                responseType: "json",
                url: $scope.serverVars.listTemplatesUrl
            })
                .then(function (response) {
                    $scope.templates = response.data;

                    if ($scope.serverVars.pumpTemplateID) {
                        for (var i = 0; i < $scope.templates.length; i++) {
                            if ($scope.serverVars.pumpTemplateID === $scope.templates[i].PumpTemplateId) {
                                $scope.selectedTemplate = $scope.templates[i];
                                i = $scope.templates.length;
                            }
                        }
                    }
                    $scope.templatesLoaded = true;
                });
        };

        $scope.showUpdateTemplateSelect = function () {
            $scope.templatesLoading = true;
            $scope.loadTemplates()
                .then(function () {
                    $scope.updateTemplateSelectVisible = true;
                })
                .catch(function (errors) {
                    var message = '';
                    for (var i = 0; i < errors.length; i++) {
                        message = errors[i].ErrorMessage + '\n';
                    }
                    $window.alert(message);
                })
                .finally(function () {
                    $scope.templatesLoading = false;
                });
        };

        $scope.updateTemplate = function () {
            if ($scope.selectedTemplate.PumpTemplateId !== $scope.serverVars.pumpTemplateID) {
                return util.confirm("WARNING: Changing this template number will erase the parts shown below and replace everything with new parts.  Are you sure you want to change the template number?")
                    .then(function () {
                        return $http({
                            method: "POST",
                            responseType: "json",
                            url: $scope.serverVars.updateTemplateUrl,
                            data: {
                                deliveryTicketId: $scope.serverVars.deliveryTicketID,
                                newTemplateId: $scope.selectedTemplate.PumpTemplateId
                            }
                        })
                            .then(function (httpResponse) {
                                if (httpResponse.data.Success) {
                                    $scope.serverVars.pumpTemplateID = $scope.selectedTemplate.PumpTemplateId;
                                    $scope.serverVars.pumpFailedSpecSummary = httpResponse.data.PumpFailedConciseSpecSummary;
                                    $scope.updateTemplateSelectVisible = false;

                                    return $scope.refreshInspections();

                                } else {
                                    return $q.reject(httpResponse.data.Errors);
                                }
                            });
                    })
                    .catch(function (errors) {
                        var message = '';
                        for (var i = 0; i < errors.length; i++) {
                            message = errors[i].ErrorMessage + '\n';
                        }
                        $window.alert(message);
                    });
            }
        };

        $scope.loadOriginalPartCustomerOwned = function (pumpFailedId, customerId, currentTicketDate) {
            var url = $scope.serverVars.listPartsInUseForPumpUrl;
            return inventoryService.loadOriginalPartsCustomerOwned(url, pumpFailedId, customerId, currentTicketDate)
                .then(function (response) {
                    for (var i = 0; i < $scope.inspections.length; i++) {
                        var currentInspection = $scope.inspections[i];
                        for (var j = 0; j < response.data.length; j++) {
                            if (response.data[j].TemplatePartDefID === currentInspection.TemplatePartDefID) {
                                if (response.data[j].ReplacedWithInventoryPartID !== null) {
                                    currentInspection.OriginalCustomerOwnedPartID = response.data[j].ReplacedWithInventoryPartID;
                                }
                                j = response.data.length;
                            }
                        }
                    }
                });
        };

        $scope.refreshInspections = function () {
            return $scope.loadInspections($scope.serverVars.deliveryTicketID)
                 .then(function () {
                     if ($scope.serverVars.customerUsesInventory && $scope.serverVars.canModifyInventory) {
                         return $scope.checkInventoryList();
                     }
                 })
                 .then(function () {
                     if ($scope.serverVars.customerUsesInventory) {
                         return $scope.loadOriginalPartCustomerOwned(
                             $scope.serverVars.pumpFailedID,
                             $scope.serverVars.customerID,
                             $scope.serverVars.currentTicketDate);
                     }
                 });
        };

        (function init() {
            $timeout(function () {

                $scope.refreshInspections();

                if ($scope.serverVars.reasonRepairedListUrl) {
                    $scope.loadReasonsRepaired();
                }
            }, 0, false);

        }());
    }
})();