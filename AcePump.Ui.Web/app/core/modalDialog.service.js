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