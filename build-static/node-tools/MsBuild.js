(function () {
    "use strict";

    var runCmd      = require("./runCmd.js"),
        extend      = require("./extend.js"),
        qify        = require("./qify.js"),
        fs          = qify(require("fs-extra")),
        q           = require("q");

    module.exports = MsBuild;

    function MsBuild(options) {
        this.options = extend({
        }, options);
    }

    MsBuild.prototype.getLatestVersion = function () {
        return this.getInstalledVersions()
            .then(function (versions) {
                return versions[versions.length - 1];
            });
    };

    MsBuild.prototype.getCompatibleVersion = function (versionNumber) {
        return this.getInstalledVersions()
            .then(function (versions) {
                for (var i = 0; i < versions.length; i++) {
                    if (compareVersionNumbers(versions[i].versionNumber, versionNumber) >= 0) {
                        return versions[i];
                    }
                }

                return q.reject("No MSBuild version installed on this computer is compatible with `" + versionNumber + "`");
            });
    };

    MsBuild.prototype.getVersion = function (versionNumber) {
        return this.getInstalledVersions()
            .then(function (versions) {
                for (var i = 0; i < versionNumber.length; i++) {
                    if (compareVersionNumbers(versionNumber, versions[i].versionNumber) === 0) {
                        return versions[i];
                    }
                }

                return q.reject("MSBuild version `" + versionNumber + "` is not installed on this computer.");
            });
    };

    MsBuild.prototype.getInstalledVersions = function () {
        return q.all([
            this.getInstalledVersionsLess15(),
            this.getInstalledVersionsGreaterEq15()
        ])
            .then(function (results) {
                var installedVersions = results[0].concat(results[1]);

                if (installedVersions.length === 0) {
                    return q.reject("No versions of MSBuild installed");

                } else {
                    console.log("Installed MSBuild versions: [" + installedVersions.map(function (item) { return item.versionNumber; }).join(", ") + "]");

                    return installedVersions;
                }
            });
    };

    MsBuild.prototype.getInstalledVersionsGreaterEq15 = function () {
        return runCmd({
            command: "./build-static/node-tools/vswhere.exe",
            arguments: [
                "-requires",
                "Microsoft.Component.MSBuild",
                "-format",
                "json"
            ],
            stdout: "return"
        })
            .then(function (output) {
                var installedVersions = [];
                output = JSON.parse(output);
                for (var i = 0; i < output.length; i++) {
                    installedVersions.push({
                        versionNumber: output[i].installationVersion,
                        exePath: output[i].installationPath + "\\MSBuild\\15.0\\Bin\\msbuild.exe"
                    });
                }

                return installedVersions.sort(function (a, b) {
                    return compareVersionNumbers(a.versionNumber, b.versionNumber);
                });
            });
    };

    MsBuild.prototype.getInstalledVersionsLess15 = function () {
        return runCmd({
            command: "reg",
            arguments: [
                "query",
                "HKLM\\Software\\Microsoft\\MSBuild\\ToolsVersions",
                "/s"
            ],
            stdout: "return"
        })
            .then(function (output) {
                var lines = output.split("\n");
                var installedVersions = [],
                    currentVersion = null;

                for (var i = 0; i < lines.length; i++) {
                    var versionMatch = /^\s*HKEY_LOCAL_MACHINE\\Software\\Microsoft\\MSBuild\\ToolsVersions\\([.0-9]*)\s*$/.exec(lines[i]);
                    if (versionMatch !== null) {
                        currentVersion = {
                            versionNumber: versionMatch[1]
                        };

                        installedVersions.push(currentVersion);
                    }

                    var exePathMatch = /^\s*MSBuildToolsPath\s*REG_SZ\s*(.*)\s*$/.exec(lines[i]);
                    if (exePathMatch !== null && currentVersion !== null) {
                        currentVersion.exePath = exePathMatch[1] + "msbuild.exe";
                        currentVersion = null;
                    }
                }

                return installedVersions.sort(function (a, b) {
                    return compareVersionNumbers(a.versionNumber, b.versionNumber);
                });
            });
    };
    
    MsBuild.prototype.run = function (projectFile, configuration) {
        return this.getLatestVersion()
            .catch(function (err) {
                return q.reject("Failed to init MSBuild: " + err);
            })
            .then(function (version) {
                console.log("Using MSBuild version " + version.versionNumber);
                if (!configuration) configuration = "Release";
                
                return runCmd({
                    command: version.exePath,
                    arguments: [
                        projectFile,
                        "/nologo",
                        "/clp:ErrorsOnly",
                        "/target:Clean;Build",
                        "/p:configuration=" + configuration
                    ]
                });
            });
    };

    function compareVersionNumbers(a, b) {
        var rgxTrailingZero = /(\.0+)+$/;
        var aParts = a.replace(rgxTrailingZero, "").split(".");
        var bParts = b.replace(rgxTrailingZero, "").split(".");
        var commonLength = Math.min(aParts.length, bParts.length);

        for (var i = 0; i < commonLength; i++) {
            var diff = parseInt(aParts[i], 10) - parseInt(bParts[i], 10);
            if (diff) {
                return diff;
            }
        }
        return aParts.length - bParts.length;
    }
})();