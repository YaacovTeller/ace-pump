(function(){
    "use strict";

    describe("padInput", function() {
        beforeEach(module("acePump.core"));

        var $compile,
            $rootScope;

        beforeEach(inject(function(_$compile_, _$rootScope_) {
            $compile = _$compile_;
            $rootScope = _$rootScope_;
        }));

        describe("link", function () {
            it("adds spaces to the maxlength property of the input");
        });
    });
})();