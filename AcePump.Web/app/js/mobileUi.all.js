(function () {
    "use strict";

    angular.module("app.core", [
        "ionic",
        "ionic.native",
        "ngFileUpload",
        "ngCordova.plugins.nativeStorage"
    ]);

    angular
        .module("app.core")
        .value("$", angular.element)
        .value("SignaturePad", SignaturePad);
})();
(function () {
    "use strict";

    angular
        .module("app.core")
        .config(httpInterceptConfig);

    httpInterceptConfig.$inject = ["$httpProvider"];
    function httpInterceptConfig($httpProvider) {
        $httpProvider.interceptors.push(injectAuthToken);
        $httpProvider.interceptors.push(injectApiUrl);

        var rgxApiUrl = /^\/api\//i,
            rgxAuthUrl = /^\/auth\//i;

        injectApiUrl.$inject = ["environment"];
        function injectApiUrl(environment) {
            return {
                request: function (config) {
                    config.url = config.url.replace(rgxApiUrl, environment.apiUrl);
                    config.url = config.url.replace(rgxAuthUrl, environment.apiUrl + "auth/");

                    return config;
                }
            };
        }

        injectAuthToken.$inject = ["user"];
        function injectAuthToken(user) {
            return {
                request: function (config) {
                    if (user.isLoggedIn && rgxApiUrl.test(config.url)) {
                        config.headers = config.headers || {};
                        config.headers.Authorization = "Bearer " + user.current.access_token;
                    }

                    return config;
                }
            };
        }
    }
})();
(function () {
    "use strict";

    angular
        .module("app.core")
        .run(preload);

    preload.$inject = ["$ionicPlatform", "$rootScope", "user"];
    function preload($ionicPlatform, $rootScope, user) {
        $rootScope.preloadComplete = false;

        $rootScope.whenPreloadComplete = $ionicPlatform
            .ready()
            .then(function () {
                // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
                // for form inputs)
                if (window.cordova && window.cordova.plugins && window.cordova.plugins.Keyboard) {
                    cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
                    cordova.plugins.Keyboard.disableScroll(true);
                }
                if (window.StatusBar) {
                    // org.apache.cordova.statusbar required
                    StatusBar.styleDefault();
                }

                return user.loadCachedLogin();
            })
            .then(function () {
                $rootScope.preloadComplete = true;
            });
    }
})();

(function () {
    "use strict";

    angular
        .module("app.core")
        .config(configRoutes);

    configRoutes.$inject = ["$stateProvider", "$urlRouterProvider"];
    function configRoutes($stateProvider, $urlRouterProvider) {
        $urlRouterProvider
            .when("", "/home")
            .when("/", "/home")
            .otherwise("/home")
            .deferIntercept();

        $stateProvider
            .state("top", {
                abstract: true,
                template: "<ion-nav-view></ion-nav-view>",
                resolve: {
                    preload: waitForPreload
                }
            })

            .state("top.login", {
                url: "/login",
                templateUrl: "core/login.html",
                controller: "LoginController as loginCtrl",
                cache: false    
            })

            .state("top.logout", {
                url: "/logout",
                controller: ["$state", "user", function ($state, user) {
                    user
                        .logout()
                        .then(function () {
                            $state.go("top.login");
                        });
                }],
                cache: false
            });

        waitForPreload.$inject = ["$rootScope"];
        function waitForPreload($rootScope) {
            return $rootScope.whenPreloadComplete;
        }
    }
})();
(function () {
    "use strict";

    angular
        .module("app.core")
        .run(stateChangeIntercept);

    stateChangeIntercept.$inject = ["$q", "$rootScope", "$state", "user", "$urlRouter"];
    function stateChangeIntercept($q, $rootScope, $state, user, $urlRouter) {
        $rootScope.$on("$locationChangeSuccess", function (evt, toUrl, fromUrl) {
            function noop() { }
            var action = {
                beforeSync: noop,
                afterSync: noop
            };

            if (!user.isLoggedIn) {
                action.beforeSync = function () { return $rootScope.whenPreloadComplete; };
            }

            if (action.beforeSync !== noop) {
                evt.preventDefault();

                $q.when()
                    .then(action.beforeSync)
                    .then(function () {
                        $urlRouter.sync();
                    })
                    .then(action.afterSync);
            }

        });
        $urlRouter.listen();

        $rootScope.$on("$stateChangeStart", function (e, toState, toParams, fromState) {
            if (!user.isLoggedIn && toState.name !== "top.login") {
                e.preventDefault();
                $state.go("top.login", {}, { location: true });
            }
        });
    }
})();   
(function () {
    "use strict";

    angular
        .module("app.core")
        .controller("LoginController", LoginController);

    LoginController.$inject = ["environment", "$ionicLoading", "$ionicPopup", "$state", "user"];
    function LoginController(environment, $ionicLoading, $ionicPopup, $state, user) {
        var vm = this;
        vm.subtitle = environment.applicationSubtitle;

        vm.login = login;

        function login() {
            $ionicLoading.show({
                template: "Logging In..."
            });

            user
                .login(vm.username, vm.password)
                .then(function () {
                    $state.go("top.search");
                })
                .catch(function (err) {
                    $ionicPopup
                        .alert({
                            title: "Login Failed",
                            template: err
                        });
                })
                .finally(function () {
                    $ionicLoading.hide();
                });
        }
    }
})();
(function () {
    "use strict";

    angular
        .module("app.core")
        .value("environment", window.environment);
})();
(function () {
    "use strict";

    angular
        .module("app.core")
        .service("user", user);

    user.$inject = ["$cordovaNativeStorage", "$injector", "$q", "$rootScope"];
    function user($cordovaNativeStorage, $injector, $q, $rootScope) {
        var service = {
            login: login,
            logout: logout,

            loadCachedLogin: loadCachedLogin
        };

        init();

        return service;

        function init() {
            service.isLoggedIn = false;
            service.current = null;
        }

        function login(username, password) {
            if (!username) username = "";
            if (!password) password = "";

            var $http = $injector.get("$http");
            return $http.post(
                "/auth/token",
                "grant_type=password&username=" + username + "&password=" +password,
                {
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded"
                    }
                }
            )
                .then(function (httpResponse) {
                    service.current = httpResponse.data;
                    service.isLoggedIn = true;

                    onLogin();

                    return $cordovaNativeStorage.setItem("current-user", service.current);
                })
                .catch(function (httpError) {
                    return $q.reject(httpError.data.error_description);
                });
        }

        function logout() {
            service.isLoggedIn = false;
            service.current = null;

            var $ionicHistory = $injector.get("$ionicHistory");
            $ionicHistory.clearCache();
            $ionicHistory.nextViewOptions({ disableBack: true });

            onLogout();
            
            return $cordovaNativeStorage.remove("current-user");
        }

        function loadCachedLogin() {
            return $cordovaNativeStorage.getItem("current-user")
                .then(function (cachedUser) {
                    service.current = cachedUser;
                    service.isLoggedIn = true;

                    onLoadCached();
                })
                .catch(function () {
                    //swallow missing native storage item
                    return $q.resolve();
                });
        }

        function onLogin() {
            $rootScope.$broadcast("auth.login");
        }

        function onLogout() {
            $rootScope.$broadcast("auth.logout");
        }

        function onLoadCached() {
            $rootScope.$broadcast("auth.cacheload");
        }
    }
})();
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
(function () {
    "use strict";

    angular
        .module("app.core")
        .directive("acpImgModalOnTap", acpImgModalOnTap);

    acpImgModalOnTap.$inject = ["$", "$ionicModal", "$parse"];
    function acpImgModalOnTap($, $ionicModal, $parse) {

        var directive = {
            link: link,
            restrict: "A",
            scope: true,
            priority: 100
        };

        function link(scope, element, attrs) {   
            var template =
                "<ion-modal-view ng-click=\"closeModal()\" >" +
                    "<ion-content>" +
                "<img style=\"display:block; margin:auto\" acp-api-src=" + "'" + attrs.acpImgModalOnTap + "'" + "  />" +
                    "</ion-content>" +
                "</ion-modal-view>";

            scope.gridModal = $ionicModal.fromTemplate(template,
                {
                    scope: scope,
                    animation: "slide-in-up"
                });

            scope.closeModal = function () {
                scope.gridModal.hide();
            }
        
            element.bind("click", function () {
                var overlayImg = $(scope.gridModal.el).find("img");

                if (overlayImg[0].clientWidth > overlayImg[0].clientHeight) {
                    overlayImg.css("width", "100vw");
                    overlayImg.css("height", "");

                } else {
                    overlayImg.css("width", "");
                    overlayImg.css("height", "100vh");
                }

                scope.gridModal.show();
            });
        }
    
        return directive;
    }
})();
(function () {
    "use strict";

    angular
        .module("app.core")
        .directive("acpSignaturePad", acpSignaturePad);

    acpSignaturePad.$inject = ["$", "SignaturePad"];
    function acpSignaturePad($, SignaturePad) {
        var directive = {
            link: link,
            restrict: "E",
            require: "ngModel",
            template: "<canvas class=\"signature-pad\"></canvas>"
        };

        function link(scope, element, attrs, ngModelCtrl) {
            var signaturePad = new SignaturePad(element.find("canvas")[0]);

            signaturePad.onEnd = function (e) {
                var data = signaturePad.toDataURL().replace("data:image/png;base64,", "");
                ngModelCtrl.$setViewValue(data);
            };

            ngModelCtrl.$render = function (viewValue) {
                if (!viewValue && !signaturePad.isEmpty()) {
                    signaturePad.clear();
                }
            };

            if (attrs.required || attrs.ngRequired) {
                ngModelCtrl.$validators.required(function (modelValue, viewValue) {
                    return !viewValue.length;
                });
            }
            var domElement = element[0];
            domElement.addEventListener("touchstart", function (e) {
                e.preventDefault();
            }, { passive: false });
            domElement.addEventListener("touchend", function (e) {
                e.preventDefault();
            }, { passive: false });
            domElement.addEventListener("touchmove", function (e) {
                e.preventDefault();
            }, { passive: false });
        }
    
        return directive;
    }
})();
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
(function () {
    "use strict";

    angular
        .module("app.core")
        .filter("percent", percentFilter);

    percentFilter.$inject = [];
    function percentFilter() {
        return function (value) {
            return (value * 100).toFixed(3) + "%";
        };
    }
})();

(function () {
    "use strict";

    angular
        .module("app.tickets", [
            "app.core"
        ]);
})();
(function () {
    "use strict";

    angular
        .module("app.tickets")
        .config(configRouter);

    configRouter.$inject = ["$stateProvider", "$urlRouterProvider"];
    function configRouter($stateProvider, $urlRouterProvider) {
        $urlRouterProvider
            .when("", "/search")
            .when("/", "/search")
            .otherwise("/search");

        $stateProvider
            .state("top.search", {
                url: "/search",
                cache: false,
                templateUrl: "tickets/search.html",
                controller: "SearchController as searchCtrl"
            })

            .state("top.details", {
                url: "/details/{deliveryTicketId}",
                cache: false,
                templateUrl: "tickets/details.html",
                controller: "DetailsController as detailsCtrl"
            })

            .state("top.viewPicture", {
                url: "/viewPicture",
                templateUrl: "tickets/viewPicture.html",
                controller: "ViewPictureController as viewPictureCtrl"
            })

            .state("top.takePicture", {
                url: "/takePicture/{deliveryTicketId}",
                cache: false,
                templateUrl: "tickets/takePicture.html",
                controller: "TakePictureController as takePictureCtrl"            
            })

            .state("top.signature", {
                url: "/signature/{deliveryTicketId}",
                cache: false,
                templateUrl: "tickets/signature.html",
                controller: "SignatureController as signatureCtrl"
            });
    }
})();
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
(function () {
    "use strict";

    angular
        .module("app.tickets")
        .controller("DetailsController", DetailsController);

    DetailsController.$inject = ["$http", "$ionicLoading", "$ionicPopup", "$q", "$scope", "$stateParams", "ticketsRepository"];
    function DetailsController($http, $ionicLoading, $ionicPopup, $q, $scope, $stateParams, ticketsRepository) {
        var vm = this;

        vm.currentTicket = ticketsRepository.get($stateParams.deliveryTicketId);

        vm.deleteImage = deleteImage;
        vm.editNote = editNote;
        vm.getSmallImageSrc = getSmallImageSrc;
        vm.getLargeImageSrc = getLargeImageSrc;
        vm.getPdfHref = getPdfHref;

        init();

        function init() {
            getTicketPictures();
        }

        function getTicketPictures() {
            $ionicLoading.show();
            return $http({
                url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/images",
                method: "GET"
            })
                .then(function (httpResponse) {
                    vm.images = httpResponse.data;
                    $ionicLoading.hide();
                });

        }

        function getSmallImageSrc(image) {
            return "/api/deliverytickets/" + image.DeliveryTicketID + "/smallImages/" + image.DeliveryTicketImageUploadID;
        }

        function getLargeImageSrc(image) {
            return "/api/deliverytickets/" + image.DeliveryTicketID + "/largeImages/" + image.DeliveryTicketImageUploadID;
        }

        function deleteImage(image) {
            return $ionicPopup.confirm({
                title: "Delete Image",
                template: "Are you sure you want to delete this image?"
            })
                .then(function (result) {
                    $ionicLoading.show();
                    if (result) {
                        $http({
                            url: "/api/deliverytickets/" + image.DeliveryTicketID + "/images/" + image.DeliveryTicketImageUploadID,
                            method: "DELETE"
                        })
                            .then(function (httpResponse) {
                                var ix = vm.images.indexOf(image);
                                vm.images.splice(ix, 1);
                                $ionicLoading.hide();
                            });
                    } else {
                        $ionicLoading.hide();
                    }
                })
                .catch(function (err) {
                    $ionicLoading.hide();
                    $ionicPopup
                        .alert({
                            title: "Delete Failed",
                            template: err
                        });
                });
        }

        function editNote(image) {
            $scope.currentImage = image;
            $scope.data = {};
            

            return $ionicPopup.show({
                title: "Please enter a note for this image",
                template: "<input type='text' ng-model='data.newNote' ng-value='currentImage.Note'></input>",
                scope: $scope,
                buttons: [
                    { text: 'Cancel', onTap: function (e) { return false; } },
                    {
                        text: '<b>Save</b>',
                        type: 'button-positive',
                        onTap: function (e) {
                            return $scope.data.newNote || true;
                        }
                    }
                ]
            })
                .then(function (result) {
                    if (result) {
                        if (image.Note !== $scope.data.newNote) {
                            return updateImageNote(image, $scope.data.newNote);
                        }
                    }
                })
                .catch(function (err) {
                    $ionicPopup.alert("Could not change note for this image. " + err);
                });
        }

        function updateImageNote(image, newNote) {
            $ionicLoading.show();
            return $http({
                url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/images/" + image.DeliveryTicketImageUploadID + "/Note",
                method: "PATCH",
                params: {
                    note: newNote
                }
            })
                .then(function (httpResponse) {
                    $ionicLoading.hide();
                    image.Note = httpResponse.data.note;
                });
        }

       function getPdfHref() {
            return "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/pdf";
        }
    }
})();
(function () {
    "use strict";

    angular
        .module("app.tickets")
        .controller("SearchController", SearchController);

    SearchController.$inject = ["$ionicLoading", "$http", "$q", "ticketsRepository", "$window"];
    function SearchController($ionicLoading, $http, $q, ticketsRepository, $window) {
        var vm = this;
        var loading; 

        vm.runSearch = runSearch;

        init();

        function init() {
            updateTickets(null, 10);               
        }

        function runSearch() {
            updateTickets(parseInt(vm.searchID), 10);
        }

        function updateTickets(id, limit) {
            $ionicLoading.show();

            ticketsRepository.search(id, limit)
                .then(function (results) {
                    vm.tickets = results;
                })
                .catch(function (httpError) {
                    $window.alert(JSON.stringify(httpError));
                })
                .finally(function () {
                    $ionicLoading.hide();
                });
        }        
    }
})();
(function () {
    "use strict";

    angular
        .module("app.tickets")
        .controller("ViewPictureController", ViewPictureController);

    ViewPictureController.$inject = [];
    function ViewPictureController() {
        var vm = this;
    }
})();
(function () {
    "use strict";

    angular
        .module("app.tickets")
        .controller("TakePictureController", TakePictureController);

    TakePictureController.$inject = ["$", "$cordovaCamera", "$cordovaTransfer", "$http", "$ionicHistory", "$ionicLoading", "$ionicPopup", "$q", "$scope", "$state", "$stateParams", "Upload", "ticketsRepository"];
    function TakePictureController($, $cordovaCamera, $cordovaTransfer, $http, $ionicHistory, $ionicLoading, $ionicPopup, $q, $scope, $state, $stateParams, Upload, ticketsRepository) {
        var vm = this;

        vm.currentTicket = ticketsRepository.get($stateParams.deliveryTicketId);

        vm.takePicture = takePicture;
        vm.savePicture = savePicture;

        init();

        function init() {
            takePicture();
        }

        function takePicture() {
            var options = {
                quality: 50,
                destinationType: Camera.DestinationType.DATA_URL,
                sourceType: Camera.PictureSourceType.CAMERA,
                encodingType: Camera.EncodingType.JPEG
            };

            $cordovaCamera.getPicture(options).then(function (imageData) {
                vm.imgData = imageData;
                vm.imgURI = "data:image/jpeg;base64," + imageData;                
                var dimensions = getDimensions(vm.imgURI);
            }, function (err) {
                $ionicPopup
                    .alert({
                        title: "Camera Failure",
                        template: err
                    })
                    .then(function () {
                        $ionicHistory.goBack();
                    });
            });
        }

        function getDimensions(imgUri) {
            var imgContainer = $("<div>")
                .css({
                    visibility: "hidden",
                    position: "absolute"
                })
                .append($("<img>").attr("src", imgUri));

            document.body.append(imgContainer);
            var img = imgContainer[0].querySelector("img");


            var dimensions = {
                width: img.clientWidth,
                height: img.clientHeight
            };

            imgContainer.remove();

            return dimensions;
        }

        function savePicture() {
            $ionicLoading.show({
                template: "Uploading image <progress max=\"100\" value=\"0\" id=\"progress_bar_con\"> </progress >"
            })
                .then(function () {
                    return Upload.upload({
                        url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/images",
                        method: "PUT",
                        data: {
                            file: vm.imgData
                        }
                    })
                        .then(
                            function uploadSuccess(e) {
                                $ionicLoading.hide();
                                $ionicPopup
                                    .alert({
                                        title: "Upload Picture",
                                        template: "Upload was successful."
                                    })
                                    .then(function () {
                                        $ionicHistory.goBack();
                                    });
                            },
                            function uploadFail() {
                                $ionicLoading.hide();
                                $ionicPopup
                                    .alert({
                                        title: "Upload Picture",
                                        template: "Picture upload failed, please try again."
                                    })
                                    .then(function () {
                                        $ionicHistory.goBack();
                                    });
                            },
                            function updateProgress(e) {
                                var percentComplete = parseInt(100.0 * e.loaded / e.total);
                                document.getElementById("progress_bar_con").value = percentComplete;
                            });
                });
        }
    }
})();
(function () {
    "use strict";

    angular
        .module("app.tickets")
        .controller("SignatureController", SignatureController);

    SignatureController.$inject = ["$http", "$ionicLoading", "$ionicHistory", "$q", "$rootScope", "$scope", "$stateParams", "ticketsRepository"];
    function SignatureController($http, $ionicLoading, $ionicHistory, $q, $rootScope, $scope, $stateParams, ticketsRepository) {
        var vm = this;

        vm.currentTicket = ticketsRepository.get($stateParams.deliveryTicketId);
        
        vm.clearForm = clearForm;
        vm.saveSignature = saveSignature;
        vm.resign = resign;
        vm.goBack = goBack;

        vm.signatureForm = {};
        vm.signaturePad = {};
        vm.lineItems = {};
        vm.signatureData = {};

        init();


        function init() {
            loadSignatureData();
            loadLineItems();
        }

        function clearForm() {
            vm.signatureForm.$setPristine();
            vm.signatureForm.$setUntouched();
        }

        function saveSignature() {
            $ionicLoading.show();

            return $http({
                url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/sign",
                method: "PATCH",
                data: vm.signatureData
            })
                .then(function (httpResponse) {
                    vm.currentTicket.HasSignature = true;
                    vm.clearForm();
                })
                .finally(function () {
                    $ionicLoading.hide();
                });

        }

        function resign() {
            vm.signatureData.Signature = null;
            vm.signatureData.SignatureName = "";
            vm.signatureData.SignatureCompanyName = "";
            vm.signatureData.SignatureDate = new Date();
            vm.currentTicket.HasSignature = false;
        }

        function loadSignatureData() {
            $ionicLoading.show();
            vm.signatureData = {};

            return $http({
                url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/signatureData",
                method: "GET"
            })
                .then(function (httpResponse) {
                    vm.signatureData = httpResponse.data;    
                    vm.currentTicket.HasSignature = !!vm.signatureData.Signature;  
                    if (!vm.currentTicket.HasSignature) {
                        vm.signatureData.SignatureDate = new Date();
                    }
                })
                .finally(function () {
                    $ionicLoading.hide();
                });
        }    

        function loadLineItems() {
            $ionicLoading.show();
            vm.lineItems = {};

            return $http({
                url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/lineItems",                
                method: "GET"
            })
                .then(function (httpResponse) {
                    vm.lineItems = httpResponse.data;

                    vm.signatureData.salesTotal = 0;
                    vm.signatureData.taxOwed = 0;

                    for (var i = 0; i < vm.lineItems.length; i++) {
                        vm.signatureData.salesTotal += vm.lineItems[i].LineTotal;
                        vm.signatureData.taxOwed += vm.lineItems[i].LineTotal * (vm.lineItems[i].LineIsTaxable ? vm.signatureData.SalesTaxRate : 0);                     
                    }

                    vm.signatureData.grandTotal = vm.signatureData.salesTotal + vm.signatureData.taxOwed;
                })
                .finally(function () {
                    $ionicLoading.hide();
                });
        }     

        function goBack() {
            $ionicHistory.backView().stateParams = { deliveryTicketId: vm.currentTicket.DeliveryTicketID };
            $rootScope.$ionicGoBack();
        }        
    }
})();
(function () {
    "use strict";

    angular.module("app", [
        "app.tickets"
    ]);
})();