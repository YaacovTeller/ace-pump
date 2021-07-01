(function () {
    "use strict";
    describe("Plunger Barrel Wear Controller", function () {

        var $controller,
            ctrl,
            $httpBackend,
            $q,
            $scope,
            serializationService,
            $window;

        beforeEach(function() {
            module("acePump");
            inject(function (_$controller_, _$httpBackend_, _plungerBarrelWearSerializationService_, _$q_, $rootScope, _$window_) {
                $controller = _$controller_;
                $httpBackend = _$httpBackend_;
                $q = _$q_;
                $scope = $rootScope.$new();
                serializationService = _plungerBarrelWearSerializationService_;
                $window = _$window_;
            });

            $scope.updateUrl = "http://www.api.DeliveryTicket.Update";
            $scope.deliveryTicketID = 14;

            init();
        });

        function init() {
            ctrl = $controller("PlungerBarrelWearController", { $scope: $scope});
        }

        function setupControllerArraysWithUnicodeValues() {
            var arrays = {
                PlungerOrig: new Array(15),
                PlungerWearRepaired: new Array(15),
                PlungerOut: new Array(15),
                BarrelOrig: new Array(36),
                BarrelWearRepaired: new Array(36),
                BarrelOut: new Array(36),
            };

            var charCodes = new Array(306);
            for (var i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;

            i = 0;
            var currentArray;
            for (var key in arrays) {
                if (arrays.hasOwnProperty(key)) {
                    currentArray = arrays[key];
                    for (var j = 0; j < currentArray.length; j++) {
                        currentArray[j] = String.fromCharCode(charCodes[i], charCodes[i + 1]);
                        i += 2;
                    }
                }
            }

            ctrl.PlungerOrig = arrays.PlungerOrig;
            ctrl.PlungerWearRepaired = arrays.PlungerWearRepaired;
            ctrl.PlungerOut = arrays.PlungerOut;
            ctrl.BarrelOrig = arrays.BarrelOrig;
            ctrl.BarrelWearRepaired = arrays.BarrelWearRepaired;
            ctrl.BarrelOut = arrays.BarrelOut;

            return arrays;
        }

        describe("init", function () {
            it("sets the vm properties of the received values from the server", function () {            
                var serverVars = {
                    originalPlungerBarrelWear: "" + new Array(307).join('i') + "",
                    readUrl: 'readUrl',
                    updateUrl: 'updateUrl',
                    deliveryTicketID: 'deliveryTicketID'
                };

                ctrl.init(serverVars);

                expect(ctrl.originalPlungerBarrelWear).toBe(serverVars.originalPlungerBarrelWear);
                expect(ctrl.readUrl).toBe(serverVars.readUrl);
                expect(ctrl.updateUrl).toBe(serverVars.updateUrl);
                expect(ctrl.deliveryTicketID).toBe(serverVars.deliveryTicketID);
            });

            it("initializes originalPlungerBarrelWear to empty string of 306 characters if empty", function () {
                var serverVars = {
                    originalPlungerBarrelWear: '',
                    readUrl: 'readUrl',
                    updateUrl: 'updateUrl',
                    deliveryTicketID: 'deliveryTicketID'
                };
                var empty306String = "" + new Array(306 + 1).join(' ') + "";

                ctrl.init(serverVars);

                expect(ctrl.originalPlungerBarrelWear).toEqual(empty306String);
            });

            it("initializes originalPlungerBarrelWear to empty string of 306 characters if null", function () {
                var serverVars = {
                    originalPlungerBarrelWear: null,
                    readUrl: 'readUrl',
                    updateUrl: 'updateUrl',
                    deliveryTicketID: 'deliveryTicketID'
                };
                var empty306String = "" + new Array(306 + 1).join(' ') + "";

                ctrl.init(serverVars);

                expect(ctrl.originalPlungerBarrelWear).toEqual(empty306String);
                expect(ctrl.originalPlungerBarrelWear.length).toBe(306);
            });

            it("calls deserialize with vm.originalPlungerBarrelWear", function () {
                var serverVars = {
                    originalPlungerBarrelWear: 'testString',
                    readUrl: 'readUrl',
                    updateUrl: 'updateUrl',
                    deliveryTicketID: 'deliveryTicketID',
                    plungerOrigFromPreviousOut: '',
                    barrelOrigFromPreviousOut:''
                };
                var arrays = {
                    PlungerOrig: ['po', 'po'],
                    PlungerWearRepaired: ['pw', 'pw'],
                    PlungerOut: ['pt', 'pt'],
                    BarrelOrig: ['bo', 'bo'],
                    BarrelWearRepaired: ['bw', 'bw'],
                    BarrelOut: ['bt', 'bt'],
                };
                spyOn(serializationService, "deserialize").and.returnValue(arrays);

                ctrl.init(serverVars);

                expect(serializationService.deserialize).toHaveBeenCalledWith(serverVars.originalPlungerBarrelWear, serverVars.plungerOrigFromPreviousOut, serverVars.barrelOrigFromPreviousOut);
            });

            it("sets the arrays received from deserialize to the controller's properties", function () {
                var firstArrayString = "" + new Array(30 + 1).join('1') + "";
                var secondArrayString = "" + new Array(30 + 1).join('2') + "";
                var thirdArrayString = "" + new Array(30 + 1).join('3') + "";
                var fourthArrayString = "" + new Array(72 + 1).join('4') + "";
                var fifthArrayString = "" + new Array(72 + 1).join('5') + "";
                var sixthArrayString = "" + new Array(72 + 1).join('6') + "";
                var veryLongWearString = firstArrayString + secondArrayString + thirdArrayString + fourthArrayString + fifthArrayString + sixthArrayString;

                var serverVars = {
                    originalPlungerBarrelWear: veryLongWearString,
                    readUrl: 'readUrl',
                    updateUrl: 'updateUrl',
                    deliveryTicketID: 'deliveryTicketID'
                };

                ctrl.init(serverVars);

                expect(ctrl.PlungerOrig.join('')).toBe(firstArrayString);
                expect(ctrl.PlungerWearRepaired.join('')).toBe(secondArrayString);
                expect(ctrl.PlungerOut.join('')).toBe(thirdArrayString);
                expect(ctrl.BarrelOrig.join('')).toBe(fourthArrayString);
                expect(ctrl.BarrelWearRepaired.join('')).toBe(fifthArrayString);
                expect(ctrl.BarrelOut.join('')).toBe(sixthArrayString);
            });
        });

        describe("save", function () {
            it("should call serialize on the service with the vm arrays", function () {
                var serverVars = {
                    originalPlungerBarrelWear: "" + new Array(307).join('a') + "",
                    readUrl: 'readUrl',
                    updateUrl: 'updateUrl',
                    deliveryTicketID: 'deliveryTicketID'
                };
                ctrl.init(serverVars);
                $httpBackend.expectPOST($scope.updateUrl).respond(200);

                spyOn(serializationService, "serialize");
                var arrays = setupControllerArraysWithUnicodeValues();

                ctrl.save();

                expect(serializationService.serialize).toHaveBeenCalledWith(arrays);
            });

            it("should POST the plunger barrel wear string if valid", function () {   
                var serverVars = {
                    originalPlungerBarrelWear: "" + new Array(307).join('a') + "",
                    readUrl: 'readUrl',
                    updateUrl: 'updateUrl',
                    deliveryTicketID: 'deliveryTicketID'
                };

                ctrl.init(serverVars);

                var data = { id: serverVars.deliveryTicketID, plungerBarrelWear: serverVars.originalPlungerBarrelWear };
                $httpBackend.expectPOST(serverVars.updateUrl, data).respond(200);

                ctrl.save(data);

                $httpBackend.flush();
            });

            xit("should alert the user if the POST didn't succeed", function () {
                spyOn($window, "alert");
                var serverVars = {
                    originalPlungerBarrelWear: "" + new Array(307).join('a') + "",
                    readUrl: 'readUrl',
                    updateUrl: 'updateUrl',
                    deliveryTicketID: 'deliveryTicketID'
                };

                ctrl.init(serverVars);

                var data = { id: serverVars.deliveryTicketID, plungerBarrelWear: serverVars.originalPlungerBarrelWear };
                var error = { status: "status", statusText : "status text"};
                $httpBackend.expectPOST(serverVars.updateUrl, data).respond(404, error);

                var promise = ctrl.save(data);
                var message = "";

                promise.then(function () {
                    message = "I'm here in the promise!";
                    expect($window.alert).toHaveBeenCalled();
                });
                $scope.$digest();
                $httpBackend.flush();

                expect(message).toBe("I'm here in the promise!");

                //expect($window.alert).toHaveBeenCalled();
                //upto here - this test doesn't work properly, it goes into the catch onthe ctrl side, but
                // I can't see it here in the spec, that means that it never got into the "then" of the promise a few lines up.
                
                //also need to write another test that makes sure the form gets set to pristine.

            });
        });
    });
})();
