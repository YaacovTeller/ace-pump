(function () {
    "use strict";

    angular
        .module("app.core")
        .directive("acpApiSrc", acpApiSrc);

    acpApiSrc.$inject = ["environment", "$parse", "user"];
    function acpApiSrc(environment, $parse, user) {
        var directive = {
            link: link,
            restrict: "A"
        };

        function link(scope, element, attrs) {
            var src = getAcpUri();            
            element.attr("src", getAuthTokenSrc(src));

            function getAcpUri() {
                var getAcpSrc = $parse(attrs.acpApiSrc);
                var uri = getAcpSrc(scope);
                var environmentApiUrl = environment.apiUrl;
                 
                if (environmentApiUrl.slice(-1) === "/" && uri.charAt(0) === "/") {
                    environmentApiUrl = environmentApiUrl.slice(0,-1);
                }
                return environmentApiUrl + uri;
            }

            function getAuthTokenSrc(src) {
                if (src && src.indexOf("/api/") >= 0) {
                    if (src.indexOf("?") === -1) src += "?"; 
                    else src += "&";

                    src += "oauth_qs_token=" + user.current.access_token;

                    return src;
                }
            }
        }
    
        return directive;
    }
})();