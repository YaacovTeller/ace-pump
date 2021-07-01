(function () {
    "use strict";

    angular
        .module("app.core")
        .directive("acpApiHref", acpApiHref);

    acpApiHref.$inject = ["environment", "$parse", "user"];
    function acpApiHref(environment, $parse, user) {
        var directive = {
            link: link,
            restrict: "A"
        };

        function link(scope, element, attrs) {
            var href = getAcpUri();            
            element.attr("href", getAuthTokenHref(href));

            function getAcpUri() {
                var getAcpHref = $parse(attrs.acpApiHref);
                var uri = getAcpHref(scope);
                var environmentApiUrl = environment.apiUrl;
                 
                if (environmentApiUrl.slice(-1) === "/" && uri.charAt(0) === "/") {
                    environmentApiUrl = environmentApiUrl.slice(0,-1);
                }
                return environmentApiUrl + uri;
            }

            function getAuthTokenHref(href) {
                if (href && href.indexOf("/api/") >= 0) {
                    if (href.indexOf("?") === -1) href += "?"; 
                    else href += "&";

                    href += "oauth_qs_token=" + user.current.access_token;

                    return href;
                }
            }
        }
    
        return directive;
    }
})();