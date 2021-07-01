(function(){
    "use strict";

    describe("percent filter", function(){
        beforeEach(module("acePump.core"));

        var filter;

        beforeEach(inject(function($filter) {
            filter = $filter("percent");
        }));

        it("multiplies value by 100 and adds a % symbol", function() {
            var ORIGINAL = 0.1234;

            var result = filter(ORIGINAL);

            expect(result).toBe("12.34%");
        });
    });
}());