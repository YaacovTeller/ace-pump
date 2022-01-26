(function () {
    window.migrate = {
        getAceController: getAceController,
        getAceInjector: getAceInjector,
        getNgService: getNgService
    };

    function getAceController(ctrlName, scopeParams) {
        var aceInjector = angular.injector(["ng", "acePump.backOffice"]);
        var $controller = aceInjector.get("$controller");
        var $rootScope = aceInjector.get("$rootScope");
        var $scope = $rootScope.$new();
        angular.extend($scope, scopeParams || {});

        var ctrl = $controller(ctrlName, { $scope: $scope });

        return ctrl;
    }

    function getAceInjector() {
        var aceInjector = angular.injector(["ng", "acePump.backOffice"]);

        return aceInjector;
    }

    function getNgService(serviceName) {
        var ngInjector = angular.injector(["ng"]);
        var svc = ngInjector.get(serviceName);

        return svc;
    }
})();