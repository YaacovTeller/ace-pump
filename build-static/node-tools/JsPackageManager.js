var extend      = require("./extend.js"),
    JsPackage   = require("./JsPackage.js");

module.exports = JsPackageManager;

function JsPackageManager(options) {
    this._options = extend({
        paths: {
            js: "build/Scripts/",
            css: "build/Content/",
            html: "build/Static/"
        },

        projectName: "App"
    }, options);

    this._packages = {};
    this._coreDependencies = [];
}

JsPackageManager.prototype._forEachPackage = function(callback, includeDependencyOnly) {
    for(var packageName in this._packages) {
        if (this._packages.hasOwnProperty(packageName) &&
            (includeDependencyOnly || !this._packages[packageName].isDependencyOnly())) {
            callback(this._packages[packageName]);
        }
    }
};

JsPackageManager.prototype.getMinifyFiles = function() {
    var fileNameBase = this._options.paths.js + this._options.projectName;
    
    var files = {};
    this._forEachPackage(function(package) {
        var minifiedFileName = fileNameBase + "." + package.getName() + ".min.js";
        var originalFileName = fileNameBase + "." + package.getName() + ".js";

        files[minifiedFileName] = [originalFileName];
    });

    return files;
};

JsPackageManager.prototype.get = function(packageName) {
    var package = this._packages[packageName];

    if(typeof package === "undefined") {
        throw new Error("Could not find a package named '" + packageName + "'!");
    } else {
        return package;
    }
};

JsPackageManager.prototype.add = function(options) {
    if(typeof this._packages[options.name] !== "undefined") {
        throw "There is already a package named '" + options.name + "'";
    }

    var resolvedDependencies = (typeof options.dependencies !== 'undefined') ?
                                    this._resolveDependencies(options.dependencies) :
                                    [];

    options.dependencies = resolvedDependencies;
    this._addCoreDependencies(options);
    this._packages[options.name] = new JsPackage(options);
    return this._packages[options.name];
};

JsPackageManager.prototype._addCoreDependencies = function(options) {
    for(var i=0; i<this._coreDependencies.length; i++) {
        options.dependencies.push(this._coreDependencies[i]);
    }
};

JsPackageManager.prototype.addCoreDependency = function(options) {
    options.dependencyOnly = true;
    var dependencyPackage = new JsPackage(options);
    this._coreDependencies.push(dependencyPackage);
};

JsPackageManager.prototype._resolveDependencies = function(dependencies) {
    var resolvedDependencies = [];
    for(var i=0; i<dependencies.length; i++) {
        var dependencyName = dependencies[i];
        var dependency = this._packages[dependencyName];
        if(typeof dependency === "undefined") {
            throw "Could not resolve '" + dependencyName + "'.  Did you define it?";
        }

        resolvedDependencies.push(dependency);
    }

    return resolvedDependencies;
};

JsPackageManager.merge = function () {
    var dst = {};

    for (var i = 0; i < arguments.length; i++) {
        var src = arguments[i];

        for (var prop in src) {
            if (src.hasOwnProperty(prop)) {
                dst[prop] = src[prop];
            }
        }
    }

    return dst;
};
