(function(){
    "use strict";

    describe("Delivery Ticket Controller", function() {
        beforeEach(module("acePump"));

        var $controller,
            $scope,
            $q,
            $httpBackend,
            $window,
            modalService;

        beforeEach(inject(function(_$controller_, _$httpBackend_, _$q_, _modalService_, _$window_, $http, $rootScope) {
            $scope = $rootScope.$new();
            $scope.serverVars = {
                queryCustomerTaxRateUrl: "http://www.query.customer.rate",
                systemDefaultTaxRate: 0.12345,
                _serverVarsLoaded: true
            };

            $controller = _$controller_;
            $controller("DeliveryTicketController", { $scope: $scope, $http: $http, $q: _$q_, $window: _$window_, modalService: _modalService_ });

            $httpBackend = _$httpBackend_;
            $q = _$q_;
            modalService = _modalService_;
            $window = _$window_;
        }));

        describe("applyCustomerDefaultTaxRate", function() {
            var ORIGINAL_TAX_RATE = 0.12345;
            var CUSTOMER_TAX_RATE = 0.54321;

            it("sets $scope.taxRate to $scope.customerTaxRate and clears userEntered flag if $scope.customerTaxRateAvailable is true", function() {
                $scope.taxRate = ORIGINAL_TAX_RATE;
                $scope.customerTaxRate = CUSTOMER_TAX_RATE;
                $scope.customerTaxRateAvailable = true;
                $scope.userEnteredTaxRate = true;

                $scope.applyCustomerDefaultTaxRate();

                expect($scope.taxRate).toBe(CUSTOMER_TAX_RATE);
                expect($scope.userEnteredTaxRate).toBe(false);
            });

            it("doesn't change $scope.taxRate or userEntered flag if no customer rate available", function() {
                $scope.taxRate = ORIGINAL_TAX_RATE;
                delete $scope.customerTaxRate;
                $scope.customerTaxRateAvailable = false;
                $scope.userEnteredTaxRate = true;

                $scope.applyCustomerDefaultTaxRate();

                expect($scope.taxRate).toBe(ORIGINAL_TAX_RATE);
                expect($scope.userEnteredTaxRate).toBe(true);
            });
        });

        describe("autoUpdateTaxRateBySelectedCustomer", function() {
            var CHOOSE_TAX_RATE_BY_SYSTEM_MODAL_ID = "choose-tax-rate-system";
            var CHOOSE_TAX_RATE_BY_CUSTOMER_MODAL_ID = "choose-tax-rate-customer";

            it("queries customer default with selected customer if customerId set", function() {
                spyOn($scope, "queryCustomerDefaultTaxRate").and.returnValue($q.resolve());
                $scope.customerId = 225;

                $scope.autoUpdateTaxRateBySelectedCustomer();

                expect($scope.queryCustomerDefaultTaxRate).toHaveBeenCalledWith($scope.customerId);
            });

            it("does not query if no customer is selected", function() {
                spyOn($scope, "queryCustomerDefaultTaxRate");
                delete($scope.customerId);

                $scope.autoUpdateTaxRateBySelectedCustomer();

                expect($scope.queryCustomerDefaultTaxRate).not.toHaveBeenCalled();
            });

            describe("when query completes", function() {
                describe("if rate found", function() {
                    var CUSTOMER_DEFAULT_TAX_RATE = 0.015;

                    beforeEach(function() {
                        spyOn($scope, "queryCustomerDefaultTaxRate").and.returnValue($q.resolve(CUSTOMER_DEFAULT_TAX_RATE));
                        $scope.customerId = 253;
                    });

                    describe("if tax rate was user input", function() {// - i.e. they typed it instead of it loading from either 1. System default, 2. Customer default
                                                                       //   tracked by $scope.userInputRate

                        var TYPED_TAX_RATE = 0.0007;

                        beforeEach(function() {
                            $scope.userEnteredTaxRate = true;
                            $scope.taxRate = TYPED_TAX_RATE;
                        });

                        it("dialogs if user wants new customer default or typed rate", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_CUSTOMER_MODAL_ID);
                            spyOn(modal, "open");

                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect(modal.open).toHaveBeenCalled();
                        });

                        it("sets ticket tax rate to customer default and clears typed flag if choose customer default (modal ok'ed)", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_CUSTOMER_MODAL_ID);

                            $scope.autoUpdateTaxRateBySelectedCustomer();
                            modal.ok();

                            $scope.$digest();
                            expect($scope.taxRate).toBe(CUSTOMER_DEFAULT_TAX_RATE);
                            expect($scope.userEnteredTaxRate).toBe(false);
                        });

                        it("does not change typed rate if choose typed and leaves flag true (modal cancel'ed)", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_CUSTOMER_MODAL_ID);

                            $scope.autoUpdateTaxRateBySelectedCustomer();
                            modal.cancel();

                            $scope.$digest();
                            expect($scope.taxRate).toBe(TYPED_TAX_RATE);
                            expect($scope.userEnteredTaxRate).toBe(true);
                        });

                        it("stores rate to $scope.customerTaxRate and sets $scope.customerTaxRateAvailalable flag", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_CUSTOMER_MODAL_ID);

                            $scope.autoUpdateTaxRateBySelectedCustomer();
                            modal.ok();

                            $scope.$digest();
                            expect($scope.customerTaxRate).toBe(CUSTOMER_DEFAULT_TAX_RATE);
                            expect($scope.customerTaxRateAvailable).toBe(true);
                        });
                    });

                    describe("if tax rate was user input and matches the default loaded from server", function() {
                        var TYPED_TAX_RATE = CUSTOMER_DEFAULT_TAX_RATE;

                        beforeEach(function() {
                            $scope.userEnteredTaxRate = true;
                            $scope.taxRate = TYPED_TAX_RATE;
                        });

                        it("does not dialog", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_CUSTOMER_MODAL_ID);
                            spyOn(modal, "open");

                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect(modal.open).not.toHaveBeenCalled();
                        });

                        it("does not change the tax rate", function() {
                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect($scope.taxRate).toBe(TYPED_TAX_RATE);
                        });

                        it("stores rate to $scope.customerTaxRate and sets $scope.customerTaxRateAvailalable flag", function() {
                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect($scope.customerTaxRate).toBe(CUSTOMER_DEFAULT_TAX_RATE);
                            expect($scope.customerTaxRateAvailable).toBe(true);
                        });
                    });

                    describe("if tax rate was not user input", function() {// - see above
                        beforeEach(function() {
                            $scope.userEnteredTaxRate = false;
                        });

                        it("loads the customer default even if it is not the system default", function() { // even though it's not the sys def, it was not user input
                            $scope.taxRate = $scope.serverVars.systemDefaultTaxRate + 25;

                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect($scope.taxRate).toBe(CUSTOMER_DEFAULT_TAX_RATE);
                        });

                        it("does not dialog", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_CUSTOMER_MODAL_ID);
                            spyOn(modal, "open");

                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect(modal.open).not.toHaveBeenCalled();
                        });

                        it("stores rate to $scope.customerTaxRate and sets $scope.customerTaxRateAvailalable flag", function() {
                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect($scope.customerTaxRate).toBe(CUSTOMER_DEFAULT_TAX_RATE);
                            expect($scope.customerTaxRateAvailable).toBe(true);
                        });
                    });
                });

                describe("if no rate found", function() {
                    beforeEach(function() {
                        spyOn($scope, "queryCustomerDefaultTaxRate").and.returnValue($q.resolve(null));

                        $scope.customerId = 253;
                        $scope.customerTaxRate = 0.00123;
                        $scope.customerTaxRateAvailable = true;
                    });

                    describe("if tax rate was user input", function() {// - i.e. they typed it instead of it loading from either 1. System default, 2. Customer default
                        var TYPED_TAX_RATE = 0.0007;

                        beforeEach(function() {
                            $scope.userEnteredTaxRate = true;
                            $scope.taxRate = TYPED_TAX_RATE;
                        });

                        it("dialogs if user wants system default or typed rate", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_SYSTEM_MODAL_ID);
                            spyOn(modal, "open");

                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect(modal.open).toHaveBeenCalled();
                        });

                        it("sets ticket tax rate to customer default and clears typed flag if choose system default (modal ok'ed)", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_SYSTEM_MODAL_ID);

                            $scope.autoUpdateTaxRateBySelectedCustomer();
                            modal.ok();

                            $scope.$digest();
                            expect($scope.taxRate).toBe($scope.serverVars.systemDefaultTaxRate);
                            expect($scope.userEnteredTaxRate).toBe(false);
                        });

                        it("does not change typed rate and does not clear flag if choose typed (modal cancel'ed)", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_SYSTEM_MODAL_ID);

                            $scope.autoUpdateTaxRateBySelectedCustomer();
                            modal.cancel();

                            $scope.$digest();
                            expect($scope.taxRate).toBe(TYPED_TAX_RATE);
                            expect($scope.userEnteredTaxRate).toBe(true);
                        });

                        it("clears the $scope.customerTaxRateAvailable flag and $scope.customerTaxRate", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_SYSTEM_MODAL_ID);

                            $scope.autoUpdateTaxRateBySelectedCustomer();
                            modal.ok();

                            $scope.$digest();
                            expect($scope.customerTaxRate).toBeUndefined();
                            expect($scope.customerTaxRateAvailable).toBe(false);
                        });
                    });

                    describe("if tax rate was user input and matches the system default", function() {
                        var TYPED_TAX_RATE;

                        beforeEach(function() {
                            $scope.userEnteredTaxRate = true;
                            $scope.taxRate = TYPED_TAX_RATE = $scope.serverVars.systemDefaultTaxRate;
                        });

                        it("does not change the rate", function() {
                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect($scope.taxRate).toBe(TYPED_TAX_RATE);
                        });

                        it("does not dialog", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_SYSTEM_MODAL_ID);
                            spyOn(modal, "open");

                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            expect(modal.open).not.toHaveBeenCalled();
                        });

                        it("clears the $scope.customerTaxRateAvailable flag and $scope.customerTaxRate", function() {
                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect($scope.customerTaxRate).toBeUndefined();
                            expect($scope.customerTaxRateAvailable).toBe(false);
                        });
                    });

                    describe("if tax rate was not user input", function() {// - see above
                        beforeEach(function() {
                            $scope.userEnteredTaxRate = false;
                        });

                        it("loads the system default", function() {
                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect($scope.taxRate).toBe($scope.serverVars.systemDefaultTaxRate);
                        });

                        it("does not dialog", function() {
                            var modal = modalService.getModal(CHOOSE_TAX_RATE_BY_SYSTEM_MODAL_ID);
                            spyOn(modal, "open");

                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            expect(modal.open).not.toHaveBeenCalled();
                        });

                        it("clears the $scope.customerTaxRateAvailable flag and $scope.customerTaxRate", function() {
                            $scope.autoUpdateTaxRateBySelectedCustomer();

                            $scope.$digest();
                            expect($scope.customerTaxRate).toBeUndefined();
                            expect($scope.customerTaxRateAvailable).toBe(false);
                        });
                    });
                });

                describe("if error", function() { // i.e. promise rejected
                    it("shows a warning error that we couldn't retrieve the default and user must manually confirm", function() {
                        $scope.customerId = 12345;
                        spyOn($scope, "queryCustomerDefaultTaxRate").and.returnValue($q.reject());
                        spyOn($window, "alert");

                        $scope.autoUpdateTaxRateBySelectedCustomer();

                        $scope.$digest();
                        expect($window.alert).toHaveBeenCalledWith("Sorry, the system could not automatically retrieve the default tax rate for this customer.  Please confirm what rate you want on the ticket.");
                    });
                });
            });
        });

        describe("queryCustomerDefaultTaxRate", function() {
            var CUSTOMER_ID = 342453;

            afterEach(function() {
                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();
            });

            it("returns a promise", function() {
                $httpBackend.expectPOST($scope.serverVars.queryCustomerTaxRateUrl)
                            .respond(200, {DefaultSalesTaxRate: 0});

                var queryPromise = $scope.queryCustomerDefaultTaxRate(CUSTOMER_ID);

                expect(queryPromise.then).not.toBeUndefined();
                $httpBackend.flush();
            });

            it("POSTs to lookup rate on server", function() {
                $httpBackend.expectPOST($scope.serverVars.queryCustomerTaxRateUrl, {id: CUSTOMER_ID})
                            .respond(200, {DefaultSalesTaxRate: 0});

                $scope.queryCustomerDefaultTaxRate(CUSTOMER_ID);

                $httpBackend.flush();
            });

            it("resolves when POST resolves with tax rate as reason", function() {
                var DEFAULT_RATE = 0.004234;
                $httpBackend.expectPOST($scope.serverVars.queryCustomerTaxRateUrl, {id: CUSTOMER_ID})
                    .respond(200, {DefaultSalesTaxRate: DEFAULT_RATE } );

                var queryPromise = $scope.queryCustomerDefaultTaxRate(CUSTOMER_ID);
                $httpBackend.flush();

                var rate = null;
                queryPromise.then(function(resolvedRate) { rate = resolvedRate;  });
                $scope.$digest();
                expect(rate).toBe(DEFAULT_RATE);
            });

            it("rejects when POST fails with error as reason", function() {
                var ERR_MSG = "error and failure";
                $httpBackend.expectPOST($scope.serverVars.queryCustomerTaxRateUrl, {id: CUSTOMER_ID})
                    .respond(500, ERR_MSG );

                var msg = null;
                $scope.queryCustomerDefaultTaxRate(CUSTOMER_ID)
                    .catch(function (err) { msg = err; });

                $httpBackend.flush();
                expect(msg).toBe(ERR_MSG);
            });
        });
    });
}());