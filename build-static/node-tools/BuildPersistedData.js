(function () {
    "use strict";

    var childProc   = require("child_process"),
        qify        = require("./qify.js"),
        fs          = qify(require("fs-extra")),
        q           = require("q"),
        readline    = require("readline");

    module.exports = BuildPersistedData;
    function BuildPersistedData(raw) {
        this.raw = raw;
    }

    BuildPersistedData.get = function () {
        return BuildPersistedData.unpersist()
            .then(function (raw) {
                return new BuildPersistedData(raw);
            });
    };

    BuildPersistedData.unpersist = function () {
        return fs.readFile("./build-static/build-persisted-data.json")
            .catch(function (err) {
                if (err.code === "ENOENT") {
                    return q.resolve("{}");

                } else {
                    return q.reject();
                }
            })
            .then(function (raw) {
                if (raw.length > 0) {
                    return JSON.parse(raw);
                } else {
                    return {};
                }
            })
            .then(function(raw) {
                if (!raw.buildNumbers) raw.buildNumbers = {};

                if (!raw.userCode) {
                    return BuildPersistedData.getUserCode()
                        .then(function (code) {
                            raw.userCode = code;
                            return raw;
                        });
                }

                return raw;
            });
    };

    BuildPersistedData.persist = function (raw) {
        raw = JSON.stringify(raw);

        return fs.writeFile("./build-static/build-persisted-data.json", raw);
    };
    
    BuildPersistedData.getUserCode= function () {
        var defer = q.defer();

        var cmdCfg = childProc.spawn("hg", ["config"], { stdio: "pipe" });
        var rlCfg = readline.createInterface({ input: cmdCfg.stdout, output: null });

        rlCfg.on("line", function (line) {
            if (line.indexOf("ui.username") === 0) {
                var parts = line.split("=");
                var hgUsername = parts[1];
                var initials = hgUsername.substring(0, 2);
                var shifted = BuildPersistedData.CaesarShift(initials, 10);

                defer.resolve(shifted);
            }
        });

        cmdCfg.stderr.on("data", rejectOnError);

        return defer.promise;

        function rejectOnError(err) {
            defer.reject(err);
        }
    };

    BuildPersistedData.CaesarShift = function (string, shiftCnt) {
        var arrChars = string.split("");

        for (var i = 0; i < arrChars.length; i++) {
            var charCode = arrChars[i].charCodeAt(0);

            var asciiOffset = 0;
            if (charCode >= 65 && charCode <= 90) asciiOffset = 65;
            else if (charCode >= 97 && charCode <= 122) asciiOffset = 97;

            var newCode = ((charCode - asciiOffset + shiftCnt) % 26) + asciiOffset;

            arrChars[i] = String.fromCharCode(newCode);
        }

        return arrChars.join("");
    };

    /**
     * Saves current build data to drive.
     */
    BuildPersistedData.prototype.save = function () {
        return BuildPersistedData.persist(this.raw);
    };
    
    /**
     * Increments build number for specified app
     */
    BuildPersistedData.prototype.incrementBuildNumber = function (app) {
        if (typeof (app) !== "string") throw new Error("app was not a string, got " + typeof (mode));

        if (typeof (this.raw.buildNumbers[app]) !== "number") {
            this.raw.buildNumbers[app] = 0;
        }

        this.raw.buildNumbers[app]++;
        return this.raw.buildNumbers[app];
    };
})();
