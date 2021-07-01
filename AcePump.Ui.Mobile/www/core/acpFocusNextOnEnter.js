(function () {
    "use strict";

    angular
        .module("app.core")
        .directive("acpFocusNextOnEnter", acpFocusNextOnEnter);

    acpFocusNextOnEnter.$inject = [];
    function acpFocusNextOnEnter() {
        var directive = {
            link: link,
            restrict: "A"
        };

        function link(scope, element, attrs) {
            element.bind('keydown', function (e) {
                var code = e.keyCode || e.which;
                if (code === 13) {
                    var pageElements = document.querySelectorAll('input, select, textarea, button');
                    var elem = e.srcElement || e.target;
                    var focusNext = false;
                    var len = pageElements.length;
                    for (var i = 0; i < len; i++) {
                        var nextElement = pageElements[i];
                        if (focusNext) {
                            if (nextElement.type === "submit") {                                
                                break;
                            }
                            else if (nextElement.style.display !== 'none') {
                                e.preventDefault();
                                nextElement.focus();
                                break;
                            }
                        } else if (nextElement === elem) {
                            focusNext = true;
                        }
                    }
                }
            });
        }
        return directive;
    }
})();