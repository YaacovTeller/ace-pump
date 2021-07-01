(function () {
    "use strict";

    angular
        .module("acePump.core")
        .service("plungerBarrelWearSerializationService", plungerBarrelWearSerializationService);

    function plungerBarrelWearSerializationService() {
        var service = {
            serialize: serialize,
            deserialize: deserialize,
            makeDeserializeIterator: makeDeserializeIterator
        };

        function serialize(arraysToDeserialize) {
            var serialized = "";
            for (var key in arraysToDeserialize) {
                if (arraysToDeserialize.hasOwnProperty(key)) {                                        
                    serialized += arraysToDeserialize[key].join('');
                }
            }
            return serialized;
        }

        function useOutFromPreviousPump(plungerBarrelWearToDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious) {

            var start, until, plungerCharsLength, firstPart;
            if (!(plungerOrigFromPrevious === undefined || plungerOrigFromPrevious === '') && plungerOrigFromPrevious.length === 30) {
                start = plungerOrigFromPrevious.length;                
                plungerBarrelWearToDeserialize = plungerOrigFromPrevious + plungerBarrelWearToDeserialize.substring(start, plungerBarrelWearToDeserialize.length);
            }

            if (!(barrelOrigFromPrevious === undefined || barrelOrigFromPrevious === '') && barrelOrigFromPrevious.length === 72) {
                plungerCharsLength = (2 * 3 * 15);
                start = plungerCharsLength + barrelOrigFromPrevious.length;                
                firstPart = plungerBarrelWearToDeserialize.substring(0, plungerCharsLength);
                plungerBarrelWearToDeserialize = firstPart + barrelOrigFromPrevious + plungerBarrelWearToDeserialize.substring(start, plungerBarrelWearToDeserialize.length);
            }
            return plungerBarrelWearToDeserialize;
        }
    
        function deserialize(plungerBarrelWearToDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious) {
            plungerBarrelWearToDeserialize = useOutFromPreviousPump(plungerBarrelWearToDeserialize, plungerOrigFromPrevious, barrelOrigFromPrevious);

            var it = new makeDeserializeIterator(plungerBarrelWearToDeserialize);
            var returnObj = {};
            var i = 0;
            returnObj.PlungerOrig = [];
            for (i = 0; i < 15; i++) {
                returnObj.PlungerOrig[i] = it.next().value;
            }
            returnObj.PlungerWearRepaired = [];
            for (i = 0; i < 15; i++) {
                returnObj.PlungerWearRepaired[i] = it.next().value;
            }

            returnObj.PlungerOut = [];
            for (i = 0; i < 15; i++) {
                returnObj.PlungerOut[i] = it.next().value;
            }

            returnObj.BarrelOrig = [];
            for (i = 0; i < 36; i++) {
                returnObj.BarrelOrig[i] = it.next().value;
            }

            returnObj.BarrelWearRepaired = [];
            for (i = 0; i < 36; i++) {
                returnObj.BarrelWearRepaired[i] = it.next().value;
            }

            returnObj.BarrelOut = [];
            for (i = 0; i < 36; i++) {
                returnObj.BarrelOut[i] = it.next().value;
            }

            return returnObj;
        }

        function makeDeserializeIterator(string) {
            var nextIndex = 0;

            return {
                next: function () {
                    if (nextIndex < string.length) {
                        var obj = {
                            value: string.substring(nextIndex, nextIndex + 2)
                        };
                        nextIndex += 2;
                        return obj;
                    }
                    else {
                        return null;
                    }
                }
            };
        }

        return service;
    }
})();