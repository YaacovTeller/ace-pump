(function () {
    "use strict";

    describe("plungerBarrelWearSerializationService", function () {

        var service;

        beforeEach(function () {
            module("acePump.core");
            inject(function (_plungerBarrelWearSerializationService_) {
                service = _plungerBarrelWearSerializationService_;
            });
        });

        describe("deserialize", function () {
            it("deserializes characters from string into matching index in arrays", function () {
                var charCodes = new Array(306);
                for (var i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;
                var serialized = String.fromCharCode.apply(null, charCodes);

                var arrays = service.deserialize(serialized);

                expect(arrays.PlungerOrig.length).toBe(15);
                expect(arrays.PlungerWearRepaired.length).toBe(15);
                expect(arrays.PlungerOut.length).toBe(15);
                expect(arrays.BarrelOrig.length).toBe(36);
                expect(arrays.BarrelWearRepaired.length).toBe(36);
                expect(arrays.BarrelOut.length).toBe(36);

                var charCodesCounter = 0;
                var currentArray;
                for (var key in arrays) {
                    if (arrays.hasOwnProperty(key)) {
                        currentArray = arrays[key];
                        for (var j = 0; j < currentArray.length; j++) {
                            expect(currentArray[j].length).toBe(2);
                            expect(currentArray[j].charCodeAt(0)).toBe(charCodes[charCodesCounter]);
                            expect(currentArray[j].charCodeAt(1)).toBe(charCodes[charCodesCounter + 1]);
                            charCodesCounter += 2;
                        }
                    }
                }                
            });

            it("should not replace the first 30 characters if plungerOrig is empty", function () {
                var toDeserialize = new Array(307).join('a');
                var plungerOrigFromPrevious = "";
                var barrelOrigFromPrevious = "";
                                
                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.PlungerOrig.join('')).toEqual(toDeserialize.substring(0,30));
            });

            it("should not replace the first 30 characters if plungerOrig is undefined", function () {
                var toDeserialize = new Array(307).join('a');
                var barrelOrigFromPrevious = "";

                var arrays = service.deserialize(toDeserialize, undefined, barrelOrigFromPrevious);

                expect(arrays.PlungerOrig.join('')).toEqual(toDeserialize.substring(0, 30));
            });

            it("should not replace the first 72 characters of barrel if barrelOrig is empty", function () {
                var toDeserialize = new Array(307).join('a');
                var plungerOrigFromPrevious = "";
                var barrelOrigFromPrevious = "";

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.BarrelOrig.join('')).toEqual(toDeserialize.substring(90,162));
            });

            it("should not replace the first 72 characters of barrel if barrelOrig is undefined", function () {
                var toDeserialize = new Array(307).join('a');
                var plungerOrigFromPrevious = "";

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, undefined);

                expect(arrays.BarrelOrig.join('')).toEqual(toDeserialize.substring(90, 162));

            });

            it("should replace the first 30 characters of wear if plungerOrig is not empty", function () {
                var charCodes = new Array(306);
                for (var i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;
                var toDeserialize = String.fromCharCode.apply(null, charCodes);
                var plungerOrigFromPrevious = new Array(31).join('a');
                var barrelOrigFromPrevious = "";

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.PlungerOrig.join('')).toEqual(plungerOrigFromPrevious);
                expect(arrays.PlungerOrig.join('')).not.toEqual(toDeserialize.substring(0, 30));
            });

            it("should replace the middle 72 characters of wear if barrelOrig is not empty", function () {
                var charCodes = new Array(306);
                for (var i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;
                var toDeserialize = String.fromCharCode.apply(null, charCodes);
                var plungerOrigFromPrevious = "";
                var barrelOrigFromPrevious = new Array(73).join('a');

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.BarrelOrig.join('')).toEqual(barrelOrigFromPrevious);
                expect(arrays.BarrelOrig.join('')).not.toEqual(toDeserialize.substring(90, 162));
            });

            it("should not replace plungerOrig if it's longer than 30 characters.", function () {
                var charCodes = new Array(306);
                for (var i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;
                var toDeserialize = String.fromCharCode.apply(null, charCodes);
                var plungerOrigFromPrevious = new Array(32).join('a');
                var barrelOrigFromPrevious = "";

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.PlungerOrig.join('')).not.toEqual(plungerOrigFromPrevious);
                expect(arrays.PlungerOrig.join('')).toEqual(toDeserialize.substring(0, 30));
            });

            it("should not replace plungerOrig if it's shorter than 30 characters.", function () {
                var charCodes = new Array(306);
                for (var i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;
                var toDeserialize = String.fromCharCode.apply(null, charCodes);
                var plungerOrigFromPrevious = new Array(30).join('a');
                var barrelOrigFromPrevious = "";

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.PlungerOrig.join('')).not.toEqual(plungerOrigFromPrevious);
                expect(arrays.PlungerOrig.join('')).toEqual(toDeserialize.substring(0, 30));
            });

            it("should not replace barrelOrig if it's longer than 72 characters.", function () {
                var charCodes = new Array(306);
                for (var i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;
                var toDeserialize = String.fromCharCode.apply(null, charCodes);
                var plungerOrigFromPrevious = "";
                var barrelOrigFromPrevious = new Array(74).join('a');

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.BarrelOrig.join('')).not.toEqual(barrelOrigFromPrevious);
                expect(arrays.BarrelOrig.join('')).toEqual(toDeserialize.substring(90, 162));
            });

            it("should not replace barrelOrig if it's shorter than 72 characters.", function () {
                var charCodes = new Array(306);
                for (var i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;
                var toDeserialize = String.fromCharCode.apply(null, charCodes);
                var plungerOrigFromPrevious = "";
                var barrelOrigFromPrevious = new Array(72).join('a');

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.BarrelOrig.join('')).not.toEqual(barrelOrigFromPrevious);
                expect(arrays.BarrelOrig.join('')).toEqual(toDeserialize.substring(90, 162));
            });
        });

        describe("makeDeserializeIterator next", function () {
            it("returns null if string is empty", function () {
                var returnedObj = service.makeDeserializeIterator("");

                expect(returnedObj.next()).toBe(null);
            });

            it("returns null if string is iterated passed length of string", function () {
                var returnedObj = service.makeDeserializeIterator("12345");
                returnedObj.next();
                returnedObj.next();
                returnedObj.next();

                expect(returnedObj.next()).toBe(null);
            });

            it("returns an object with value of the last charachter of the string if only one is left", function () {
                var returnedObj = service.makeDeserializeIterator("12345");
                returnedObj.next();
                returnedObj.next();

                expect(returnedObj.next().value).toBe("5");
            });

            it("returns an object with value of next 2 characters in string", function () {
                var returnedObj = service.makeDeserializeIterator("12345");

                expect(returnedObj.next().value).toBe("12");
            });

            it("returns an object with value of next 2 characters in string, continuing from where it was left off", function () {
                var returnedObj = service.makeDeserializeIterator("12345");
                returnedObj.next();

                expect(returnedObj.next().value).toBe("34");
            });
        });

        describe("serialize", function () {
            it("serialze characters from arrays into matching index in string", function () {
                var arrays = {
                    PlungerOrig: new Array(15),
                    PlungerWearRepaired: new Array(15),
                    PlungerOut: new Array(15),
                    BarrelOrig: new Array(36),
                    BarrelWearRepaired: new Array(36),
                    BarrelOut: new Array(36),
                };

                var charCodes = new Array(306);
                var i, j;
                for (i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;

                i = 0;
                var currentArray;
                for (var key in arrays) {
                    if (arrays.hasOwnProperty(key)) {
                        currentArray = arrays[key];
                        for (j = 0; j < currentArray.length; j++) {
                            currentArray[j] = String.fromCharCode(charCodes[i], charCodes[i + 1]);
                            i += 2;
                        }
                    }
                }
                
                var serialized = service.serialize(arrays);

                var len = serialized.length;
                expect(len).toBe(306);

                for (i = 0; i < len; i++) {
                    for (key in arrays) {
                        if (arrays.hasOwnProperty(key)) {
                            currentArray = arrays[key];                            
                            for (j = 0; j < currentArray.length; j++) {
                                expect(currentArray[j].charCodeAt(0)).toBe(serialized.charCodeAt(i));
                                expect(currentArray[j].charCodeAt(1)).toBe(serialized.charCodeAt(i + 1));
                                i += 2;
                            }
                        }
                    }
                }
            });

            it("does the opposite of deserialize", function () {
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

                var serialized = service.serialize(arrays);
                var arrays2 = service.deserialize(serialized);

                expect(arrays).toEqual(arrays2);                
            });

            it("serializes empty arrays into empty string of length 306.", function () {
                var arrays = {
                    PlungerOrig: new Array(15),
                    PlungerWearRepaired: new Array(15),
                    PlungerOut: new Array(15),
                    BarrelOrig: new Array(36),
                    BarrelWearRepaired: new Array(36),
                    BarrelOut: new Array(36),
                };
                var i = 0;
                var currentArray;
                for (var key in arrays) {
                    if (arrays.hasOwnProperty(key)) {
                        currentArray = arrays[key];
                        for (var j = 0; j < currentArray.length; j++) {
                            currentArray[j] = String.fromCharCode(160, 160);
                            i += 2;
                        }
                    }
                }

                var emptyString = service.serialize(arrays);

                expect(emptyString.length).toBe(306);
            });

            it("serializes when there's an empty element at first position of arrays.", function () {
                var charCodes = new Array(305);
                for (var i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;
                var toDeserialize = " " + String.fromCharCode.apply(null, charCodes);
                var plungerOrigFromPrevious = "";
                var barrelOrigFromPrevious = "";

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.PlungerOrig[0].charAt(0)).toBe(" ");                
            });

            it("serializes when there's an empty element at last position of arrays.", function () {
                var charCodes = new Array(305);
                for (var i = 0; i < charCodes.length; i++) charCodes[i] = i + 200;
                var toDeserialize = String.fromCharCode.apply(null, charCodes) + " ";
                var plungerOrigFromPrevious = "";
                var barrelOrigFromPrevious = "";

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.BarrelOut[35].charAt(1)).toBe(" ");
            });

            it("serializes when there's an empty element in a middle position of arrays.", function () {
                var charCodesFirstPart = new Array(150);
                var charCodesSecondPart = new Array(155);

                for (var i = 0; i < charCodesFirstPart.length; i++) charCodesFirstPart[i] = i + 200;
                for (i = 0; i < charCodesSecondPart.length; i++) charCodesSecondPart[i] = i + 200;

                var toDeserialize = String.fromCharCode.apply(null, charCodesFirstPart) + " " + String.fromCharCode.apply(null, charCodesSecondPart);
                var plungerOrigFromPrevious = "";
                var barrelOrigFromPrevious = "";

                var arrays = service.deserialize(toDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

                expect(arrays.BarrelOrig[30].charAt(0)).toBe(" ");
            });
        }); 
    });
})();