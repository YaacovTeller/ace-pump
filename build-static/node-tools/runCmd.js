(function () {
    "use strict";

    var childProc   = require("child_process"),
        extend      = require("./extend.js"),
        q           = require("q");

    module.exports = runCmd;
    function runCmd(options) {
        options = extend({
            stderr: "log",
            stdout: "log"
        }, options);
        
        var runCmdDeferred = q.defer();
        var outputBuffer = "";

        var runCmd = childProc.spawn(
            options.command,
            options.arguments
        );

        runCmd.stderr.on("data", function (buffer) {
            var msg = buffer.toString();

            if (options.stderr === "return") {
                runCmdDeferred.reject(msg);

            } else if (options.stderr === "log") {
                console.error(msg);
            }
        });

        runCmd.stdout.on("data", function (buffer) {
            var msg = buffer.toString();

            if (options.stdout === "return") {
                outputBuffer += msg;

            } else if (options.stdout === "log") {
                console.log(msg);
            }
        });

        runCmd.on("close", function (code) {
            if (code !== 0) {
                var cmdString = "> " + options.command + " " + options.arguments.join(" ");
                runCmdDeferred.reject(cmdString + " failed with status code `" + code + "`.  See errors above.");

            } else {
                runCmdDeferred.resolve(outputBuffer);
            }
        });

        return runCmdDeferred.promise;
    }
})();