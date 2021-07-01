(function () {
    "use strict";

    describe("searchController", function () {
        var $controller,
            $httpBackend,
            $scope,
            ctrl;

        beforeEach(function () {
            module("app.tickets");
            inject(function (_$controller_, _$httpBackend_) {
                $httpBackend = _$httpBackend_;
                $controller = _$controller_;
            });       

            ctrl = $controller("SearchController", { $scope: $scope });
        });

        describe("init", function () {
            it("calls getTickets with limit of 10 records.", function () {
                //spyOn(ctrl, "getTickets");

                //ctrl.init();

                //expect(ctrl.getTickets).toHaveBeenCalledWith(null, 10);
            });
        });

        describe("getTickets", function () {
            it("$http GET to tickets");            
            it("$http applies $top if specified");
            it("$http applies filter for ID if specified");
        });
    });
})();