(function () {
    "use strict";

    var cheerio         = require("cheerio");
    var childProc       = require("child_process");
    var qify            = require("../node-tools/qify.js");
    var fs              = qify(require("fs-extra"));
    var q               = require("q");
    
    nugetUpdate()
        .then( ensureIisExpressConfig );

    function nugetUpdate() {
        console.log("Installing nuget packages...");
        var deferred = q.defer();

        var nuget = childProc.spawn("build-static/install/nuget.exe", ["restore", "AcePump.sln"]);
        nuget.stdout.on("data", function (data) {
            console.log(data.toString());
        });

        nuget.on("close", function () {
            deferred.resolve();
        });

        return deferred.promise;
    }

    function ensureIisExpressConfig() {
        console.log("Updating IIS express config file...");

        return fs.stat("build-static/iisexpress.config")
            .catch(function (err) {
                if(!err || err.code !== "ENOENT") return fs.deleteFile("build-static/iisexpress.config");
            })
            .then( createIisExpressConfig );
    }

    function createIisExpressConfig() {
        return readIisTemplate()
            .then(function (template) {
                var acePumpWebPath = process.cwd() + "\\build";

                var $ = cheerio.load(template.toString(), {
                    xmlMode: true
                });
                $("site virtualDirectory").attr("physicalPath", acePumpWebPath);

                return saveIisConfig($.xml());                
            });
    }

    function readIisTemplate() {
        return fs.readFile("build-static/install/iisexpress.config.template");
    }

    function saveIisConfig(config) {
        return fs.writeFile("build-static/iisexpress.config", config);
    }
})();
