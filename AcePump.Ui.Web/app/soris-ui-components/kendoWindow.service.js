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