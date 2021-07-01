(function () {
    "use strict";

    module.exports = extend;
    
    function extend(target, source) {
        if (!target) target = {};

        for (var prop in source) {
            if (source.hasOwnProperty(prop)) {
                if (typeof (source[prop]) === "object" && !Array.isArray(source[prop])) {
                    target[prop] = extend(target[prop], source[prop]);
                } else {
                    target[prop] = source[prop];
                }
            }
        }

        return target;
    }
})();
