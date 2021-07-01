var q       = require("q");

module.exports = qify;
function qify(object) {
    var qified = {};
    for (var propName in object) {
        if (object.hasOwnProperty(propName) && typeof (object[propName]) === "function") {
            qified[propName] = createWrapper(object, propName);
        }
    }

    return qified;
}

function createWrapper(object, methodName) {
    return function () {
        var d = q.defer();

        var methodArgs = [];
        for (var i = 0; i < arguments.length; i++) methodArgs.push(arguments[i]);
        methodArgs.push(function (err, data) {
            if (err) d.reject(err);
            else d.resolve(data);
        });

        object[methodName].apply(null, methodArgs);

        return d.promise;
    };
}
