var extend = require("./extend.js"),
    JsPackageManager = require("./JsPackageManager.js");

module.exports = AppManager;

function AppManager(options) {
    this.apps = {};
}

AppManager.prototype.add = function (name, options) {
    if (typeof this.apps[name] !== "undefined") {
        throw "There is already an app named '" + name + "'";
    }

    this.apps[name] = {
        options: options,
        pm: new JsPackageManager({
            projectName: name,
            paths: options.paths
        })
    };

    return this.apps[name];
};

AppManager.prototype.forEachApp = function (callback) {
    for (var appName in this.apps) {
        if (this.apps.hasOwnProperty(appName)) {
            callback(appName, this.apps[appName]);
        }
    }
};

AppManager.prototype.getConcatTargets = function () {
    var targets = {};

    this.forEachApp(function (appName, app) {
        var fileNameBase = app.options.paths.js + appName;
        app.pm._forEachPackage(function (package) {
            var concatedFileName = fileNameBase + "." + package.getName() + ".js";
            var files = package.getFiles();

            targets[package.getName()] = {
                src: files,
                dest: concatedFileName
            };
        });
    });

    return targets;
};

AppManager.prototype.getConcatTargetNames = function (appName) {
    var targetNames = [];

    this.apps[appName].pm._forEachPackage(function (package) {
        targetNames.push(package.getName());
    });
    
    return targetNames;
};

AppManager.prototype.getUglifyTargets = function () {
    var targets = {};

    this.forEachApp(function (appName, app) {
        var fileNameBase = app.options.paths.js + appName;
        targets[appName] = {
            banner: "/*! <%= pkg.name %> <%= grunt.template.today(\"dd-mm-yyyy\") %> */\n",
            sourceMap: true,
            files: app.pm.getMinifyFiles()
        };
    });

    return targets;
};

AppManager.prototype.getUglifyTargetNames = function (appName) {
    return [appName];
};

AppManager.prototype.get = function (appName) {
    var app = this.apps[appName];

    if (typeof app === "undefined") {
        throw new Error("Could not find an app named '" + appName + "'!");
    } else {
        return app;
    }
};

AppManager.prototype.isMobileApp = function (appName) {
    return this.apps[appName].isMobileApp;
};