var extend  = require("./extend.js");

module.exports = JsPackage;

function JsPackage(options) {
    this._options = extend({
        files: [],
        filesBasePath: "",
        dependencies: [],
        dependencyOnly: false
    }, options);

    this._transformFilePaths();
    this._verifyFilesExist();
}

JsPackage.prototype._verifyFilesExist = function() {
    for (var i = 0; i < this._options.files.length; i++) {
        if(this._options.files[i].indexOf("*") >= 0) continue;

        if(!this._fileExists(this._options.files[i])) {
            throw "JsPackage: Could not find '" + this._options.files[i] + "' when creating package '" + this._options.name + "'.";
        }
    }
};

JsPackage.prototype._transformFilePaths = function () {
    if (this._options.filesBasePath) {
        for (var i = 0; i < this._options.files.length; i++) {
            this._options.files[i] = this._options.filesBasePath + "/" + this._options.files[i];
        }
    }
};

JsPackage.prototype._fileExists = function(filePath) {
    var fs = require("fs");

    try {
        fs.statSync(filePath);
    } catch(ex) {
        if(ex.code === "ENOENT") {
            return false;
        }
    }

    return true;
};

JsPackage.prototype.isDependencyOnly = function () {
    return this._options.dependencyOnly;
};

JsPackage.prototype.getName = function() {
    return this._options.name;
};

JsPackage.prototype.getFiles = function() {
    var build = new JsPackageBuild();
    build.addPackage(this);

    return build.getFiles();
};

JsPackage.prototype.getKarmaFiles = function() {
    var files = this.getFiles();

    var karmaFiles = [];
    for(var i=0; i<files.length; i++){
        karmaFiles.push({
            src: files[i],
            served: true,
            included: true
        });
    }

    return karmaFiles;
};

function JsPackageBuild() {
    this._files = [];
    this._addedPackages = [];
}

JsPackageBuild.prototype.addPackage = function(package) {
    if(typeof this._addedPackages[package._options.name] !== "undefined") return;

    for(var i=0; i<package._options.dependencies.length; i++) {
        this.addPackage(package._options.dependencies[i]);
    }

    Array.prototype.push.apply(this._files, package._options.files);
    this._addedPackages[package._options.name] = true;
};

JsPackageBuild.prototype.getFiles = function() {
    return this._files;
};
