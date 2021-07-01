(function(){
    "use strict";

    describe("autoAdvanceDirective", function() {
        beforeEach(module("acePump.core"));

        var $compile,
            $rootScope;

        beforeEach(inject(function(_$compile_, _$rootScope_) {
            $compile = _$compile_;
            $rootScope = _$rootScope_;
        }));

        describe("link", function () {
            it("advances to the next input field if user clicked a non special character");
            it("does not advance to the next input field if user clicked a special character");
            it("advances to the next input only if length is exactly max length");
            it("focuses on the next input when advances to it");
        });

    });
})();