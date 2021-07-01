var qify = require("./build-static/node-tools/qify.js");
var fs = qify(require("fs-extra"));
var IisExpress = require("./build-static/node-tools/IisExpress");
var AppManager = require("./build-static/node-tools/AppManager.js");
var MsBuild = require("./build-static/node-tools/MsBuild.js");
var path = require("path").win32;
var q = require("q");
var cheerio = require("cheerio");
var util = require("util");

module.exports = function (grunt) {
    "use strict";

    process.on("unhandledRejection", function (reason, p) {
        grunt.fail.fatal(reason);
    });

    var BASE_PATH = "build/";

    var appManager = new AppManager();

    var webapiApp = appManager.add("mobileUi", {
        paths: {
            root: BASE_PATH,
            bin: BASE_PATH + "bin/",
            app: BASE_PATH + "app/",
            js: BASE_PATH + "app/js/",
            css: BASE_PATH + "app/css/"
        },
        server: {
            projectFile: "AcePump.WebApi/AcePump.WebApi.csproj",
            deployable: [
                { source: "AcePump.WebApi/bin/", destPath: "bin" },
                { source: "AcePump.WebApi/Web.config", destPath: "root", dest: "Web.config" }
            ]
        },
        static: [
            { source: "AcePump.Ui.Mobile/www/", destPath: "app", filter: function (fileName) { return /(?:[\\\/][^\.]*$)|(?:\.html$)/.test(fileName); } },
            { source: "AcePump.Ui.Mobile/www/", destPath: "css", filter: function (fileName) { return /(?:[\\\/][^\.]*$)|(?:\.css$)|(?:\.png$)/.test(fileName); } },
            { source: "build-static/content-lib/webapi", destPath: "app" }
        ]
    });
    webapiApp.isMobileApp = true;

    webapiApp.pm.addCoreDependency({
        name: "core",
        filesBasePath: "AcePump.Ui.Mobile/www/core",
        files: [
            // Module
            "app.core.module.js",
            "app.core.httpIntercept.js",
            "app.core.preload.js",
            "app.core.router.js",
            "app.core.stateChangeIntercept.js",

            // Controllers
            "login.controller.js",

            // Services
            "environment.service.js",
            "user.service.js",

            // Directives
            "acpApiSrc.directive.js",
            "acpImgModalOnTap.directive.js",
            "acpSignaturePad.directive.js",
            "acpFocusNextOnEnter.js",
            "acpApiHref.directive.js",
            "acpApiHref.directive.js",

            // Filters
            "app.core.percent.filter.js"
        ]
    });

    webapiApp.pm.add({
        name: "tickets",
        filesBasePath: "AcePump.Ui.Mobile/www/tickets",
        files: [
            // Module
            "app.tickets.module.js",
            "app.tickets.router.js",

            //Services
            "ticketsRepository.service.js",

            // Controllers
            "details.controller.js",
            "search.controller.js",
            "viewPicture.controller.js",
            "takePicture.controller.js",
            "signature.controller.js"

        ],
        dependencyOnly: true
    });

    webapiApp.pm.add({
        name: "all",
        files: ["AcePump.Ui.Mobile/www/app.module.js"],
        dependencies: ["tickets"]
    });

    var mvcApp = appManager.add("mvc", {
        paths: {
            root: BASE_PATH,
            bin: BASE_PATH + "bin/",
            app: BASE_PATH + "app/",
            js: BASE_PATH + "app/js/",
            css: BASE_PATH + "app/css/"
        },
        server: {
            projectFile: "AcePump.Web/AcePump.Web.vbproj",
            deployable: [
                { source: "AcePump.Web/bin/", destPath: "bin" },
                { source: "AcePump.Web/Web.config", destPath: "root", dest: "Web.config" },
                { source: "AcePump.Web/Global.asax", destPath: "root", dest: "Global.asax" }
            ]
        },
        static: [
            { source: "AcePump.Ui.Web/app/", destPath: "app", filter: function (fileName) { return /(?:[\\\/][^\.]*$)|(?:\.html$)/.test(fileName); } },
            { source: "AcePump.Ui.Web/app/", destPath: "css", filter: function (fileName) { return /(?:[\\\/][^\.]*$)|(?:\.css$)|(?:\.png$)/.test(fileName); } },
            { source: "AcePump.Web/Views", destPath: "root", dest: "Views" },
            { source: "AcePump.Web/Areas/Customers/Views", destPath: "root", dest: "Areas/Customers/Views" },
            { source: "AcePump.Web/Areas/Employees/Views", destPath: "root", dest: "Areas/Employees/Views" },
            { source: "AcePump.Web/Content", destPath: "root", dest: "Content" },
            { source: "build-static/content-lib/mvc", destPath: "app" }
        ]
    });

    mvcApp.pm.addCoreDependency({
        name: "core",
        filesBasePath: "AcePump.Ui.Web/app",
        files: [
            //Soris UI
            "soris-ui-components/sorisui.module.js",
            "soris-ui-components/kendoWindow.service.js",
            "soris-ui-components/loading.directive.js",

            //Modules
            "core/app.core.module.js",
            "core/app.core.config.js",
            "core/app.core.httpIntercept.js",

            //Filters
            "core/percent.filter.js",

            //Directives
            "core/modalDialog.directive.js",
            "core/autoAdvance.directive.js",
            "core/padInput.directive.js",
            "core/selectOnClick.directive.js",

            //Services
            "core/modalDialog.service.js",
            "core/plungerBarrelWearSerialization.service.js",
            "core/inventory.service.js",
            "core/util.service.js"
        ]
    });

    mvcApp.pm.add({
        name: "repairTicketApp",
        filesBasePath: "AcePump.Ui.Web/app",
        files: [
            //Modules
            "backOffice/app.backOffice.module.js",
            "app.module.js",

            //Directives
            "core/__kendoComboDatasourceWorkaround.js",

            //Controllers
            "backOffice/plungerBarrelWear.controller.js",
            "backOffice/repairTicket.controller.js"
        ]
    });

    mvcApp.pm.add({
        name: "deliveryTicketApp",
        filesBasePath: "AcePump.Ui.Web/app",
        files: [
            //Modules
            "backOffice/app.backOffice.module.js",
            "app.module.js",

            //Directives
            "backOffice/deliveryTicketIntegration.directive.js",

            //Controllers
            "backOffice/deliveryTicket.controller.js"]
    });

    mvcApp.pm.add({
        name: "tearDownApp",
        filesBasePath: "AcePump.Ui.Web/app",
        files: [
            //Modules
            "backOffice/app.backOffice.module.js",
            "app.module.js",

            //Controllers
            "backOffice/tearDown.controller.js"]
    });

    mvcApp.pm.add({
        name: "inventoryApp",
        filesBasePath: "AcePump.Ui.Web/app",
        files: [
            //Modules
            "backOffice/app.backOffice.module.js",
            "app.module.js",

            //Controllers
            "backOffice/inventory.controller.js"]
    });

    mvcApp.pm.add({
        name: "inventoryCustomerApp",
        filesBasePath: "AcePump.Ui.Web/app",
        files: [
            //Modules
            "backOffice/app.backOffice.module.js",
            "app.module.js",

            //Controllers
            "backOffice/inventoryCustomer.controller.js"]
    });

    mvcApp.pm.add({
        name: "wellApp",
        filesBasePath: "AcePump.Ui.Web/app",
        files: [
            //Modules
            "backOffice/app.backOffice.module.js",
            "app.module.js",

            //Controllers
            "backOffice/mergeWell.controller.js"]
    });

    mvcApp.pm.add({
        name: "pumpPrefixApp",
        filesBasePath: "AcePump.Ui.Web/app",
        files: [
            //Modules
            "backOffice/app.backOffice.module.js",
            "app.module.js",

            //Controllers
            "backOffice/pumpPrefix.controller.js"]
    });

    grunt.initConfig({
        pkg: grunt.file.readJSON("package.json"),
        jshint: {
            files: ["Gruntfile.js", "AcePump.Ui.Web/**/*.js", "build-static/**/*.js", "!build-static/content-lib/**/*.js"],
            options: {
                globals: {
                    angular: true
                }
            }
        },
        karma: {
            options: {
                basePath: ".",
                frameworks: ["jasmine"],
                reporters: ["dots"],
                port: 9876,
                colors: true,
                logLevel: "INFO",
                browsers: ["PhantomJS"],
                concurrency: Infinity,
                browserConsoleLogOptions: {
                    level: 'log',
                    format: '%b %T: %m',
                    terminal: true
                }
            },
            mvc: {
                options: {
                    preprocessors: {
                        "AcePump.Ui.Web/app/**/*.html": ["ng-html2js"]
                    },
                    ngHtml2JsPreprocessor: {
                        stripPrefix: "AcePump.Ui.Web/app/",
                        moduleName: "app.templates"
                    },
                    files: [
                        // JS Libraries
                        "build-static/content-lib/mvc/js/jquery.min.js",
                        "node_modules/angular/angular.js",
                        "node_modules/angular-ui-router/release/angular-ui-router.min.js",
                        "build-static/content-lib/mvc/js/kendo.all.min.js",
                        "node_modules/angular-mocks/angular-mocks.js",
                        "AcePump.Ui.Web/app/soris-ui-components/sorisui.module.js",
                        "AcePump.Ui.Web/app/soris-ui-components/**/*.js",
                        "node_modules/angular-cookies/angular-cookies.min.js",


                        // Modules
                        "AcePump.Ui.Web/app/core/app.core.module.js",
                        "AcePump.Ui.Web/app/core/app.*.js",
                        "AcePump.Ui.Web/app/**/app.*.module.js",
                        "AcePump.Ui.Web/app/**/app.*.js",

                        // App
                        "AcePump.Ui.Web/app/**/*.js"
                    ]
                },
                singleRun: true,
                autoWatch: false
            },
            mobileUi: {
                options: {
                    preprocessors: {
                        "AcePump.Ui.Mobile/www/**/*.html": ["ng-html2js"]
                    },
                    ngHtml2JsPreprocessor: {
                        stripPrefix: "AcePump.Ui.Mobile/www/",
                        moduleName: "app.templates"
                    },
                    files: [
                        // JS Libraries
                        "AcePump.Ui.Mobile/www/ionic.bundle.min.js",
                        "node_modules/ng-file-upload/dist/ng-file-upload.min.js",
                        "https://ajax.googleapis.com/ajax/libs/angularjs/1.5.3/angular-mocks.js",
                        "AcePump.Ui.Mobile/www/ionic.native.min.js",
                        "AcePump.Ui.Mobile/www/test-lib/signature_pad.min.js",

                        // Modules
                        "AcePump.Ui.Mobile/www/core/app.core.module.js",
                        "AcePump.Ui.Mobile/www/core/app.*.js",
                        "AcePump.Ui.Mobile/www/**/app.*.module.js",
                        "AcePump.Ui.Mobile/www/**/app.*.js",

                        // App
                        "AcePump.Ui.Mobile/www/**/*.js"
                    ],
                    exclude: [
                        "AcePump.Ui.Mobile/www/lib/**/*.js"
                    ]
                },
                singleRun: true,
                autoWatch: false
            }
        },
        concat: appManager.getConcatTargets(),
        uglify: appManager.getUglifyTargets()
    });

    grunt.loadNpmTasks("grunt-contrib-jshint");
    grunt.loadNpmTasks("grunt-karma");
    grunt.loadNpmTasks("grunt-contrib-concat");
    grunt.loadNpmTasks("grunt-contrib-uglify");

    grunt.registerTask("validate", ["jshint"]);
    grunt.registerTask("test-unit", ["karma"]);
    grunt.registerTask("minify", ["uglify"]);
    grunt.registerTask("concatenate", ["concat"]);

    grunt.registerTask("init", function () {
        var completeTask = this.async();

        var app = appManager.get(grunt.option("app"));
        var ensurePromises = [];
        for (var key in app.options.paths) {
            if (app.options.paths.hasOwnProperty(key)) {
                ensurePromises.push(fs.ensureDir(app.options.paths[key]));
            }
        }

        for (var ixStatic = 0; ixStatic < app.options.static.length; ixStatic++) {
            var dest = getDest(app, app.options.static[ixStatic]);
            var staticPath = path.dirname(dest);

            ensurePromises.push(fs.ensureDir(staticPath));
        }

        q.all(ensurePromises)
            .then(completeTask);
    });

    grunt.registerTask("clean", function () {
        var completeTask = this.async();

        fs.remove(BASE_PATH)
            .then(completeTask);
    });

    grunt.registerTask("copy-server", function () {
        var completeTask = this.async();

        var app = appManager.get(grunt.option("app"));
        q.when()
            .then(function () {
                if (grunt.option("rebuild")) {
                    console.log("Compiling " + app.options.server.projectFile + " in " + grunt.option("rebuild") + " mode...");

                    var msbuild = new MsBuild();
                    return msbuild.run(app.options.server.projectFile, grunt.option("rebuild"));
                }
            })
            .then(function () {
                console.log("Copying " + app.options.server.projectFile + " deployables to output folder...");
                return q.all(app.options.server.deployable.map(function (def) {
                    return fs.copy(def.source, getDest(app, def));
                }));
            })
            .then(completeTask);
    });

    grunt.registerTask("copy-static-content", function () {
        var completeTask = this.async();

        var app = appManager.get(grunt.option("app"));
        q.all(app.options.static.map(function (def) {
            var src = def.source,
                dest = getDest(app, def),
                opts = def.filter || {};

            return fs.copy(src, dest, opts);
        }))
            .then(completeTask);
    });

    grunt.registerTask("run", function () {
        var completeTask = this.async();

        return IisExpress.get()
            .start({ hostConfig: "./build-static/iisexpress.config" })
            .then(completeTask);
    });

    grunt.registerTask("inject-environment-info", function () {
        var completeTask = this.async();

        var app = appManager.get(grunt.option("app"));
        return q.all([
            fs.readFile(app.options.paths.app + "index.html"),
            fs.readFile(app.options.paths.root + "Web.config")
        ])
            .then(function (files) {
                var index = files[0].toString();
                var webConfig = files[1].toString();

                var rgxBnPlaceholder = /\(\(\(BUILD\-NUMBER\)\)\)/g;
                var bn = grunt.option("build-number");
                index = index.replace(rgxBnPlaceholder, bn);

                var rgxEnvPlaceholder = /\(\(\(ENVIRONMENT\.?([a-zA-Z.]*)\)\)\)/g;
                var environment = parseEnvironment(webConfig);
                var strEnvSerialized = util.inspect(environment, { breakLength: Infinity, depth: null, maxArrayLength: null });
                index = index.replace(rgxEnvPlaceholder, function (match, p1) {
                    if (p1) {
                        var parts = p1.split(".");
                        var target = environment;
                        while (parts.length) {
                            target = target[parts.shift()];
                        }

                        if (typeof (target) === "string") return target;
                        else return util.inspect(target, { breakLength: Infinity, depth: null, maxArrayLength: null });

                    } else {
                        return strEnvSerialized;
                    }
                });

                return fs.writeFile(app.options.paths.app + "index.html", index);
            })
            .then(completeTask)
            .catch(function (err) {
                if (!err || err.code !== "ENOENT") return q.reject(err);
            });
    });

    function parseEnvironment(webConfig) {
        var $webConfig = cheerio.load(webConfig, {xmlMode: true});
        var mode = $webConfig.root().find("buildModes > mode[name=" + grunt.option("build-mode") + "]");        
        var environment = {
            buildNumber: grunt.option("build-number"),
            buildModeName: mode.attr("name"),
            apiUrl: mode.find("ptpApi").attr("uriV2"),
            applicationSubtitle: mode.attr("applicationsubtitle"),
            csp: {
                defaultSrc: mode.find("mobileApp").attr("csp-default-src")
            }
        };

        return environment;
    }

    grunt.registerTask("reverse-copy-minified-mobile-app", function () {
        var completeTask = this.async();

        return q.all([
            fs.copy("build/app/js/mobileUi.all.min.js", "AcePump.Ui.Mobile/www/lib/AcePump.mobileUi.all.min.js"),
            fs.copy("build/app/index.html", "AcePump.Ui.Mobile/www/index.compiled.html")
        ])
            .then(completeTask);
    });

    grunt.registerTask("build", function () {
        if (grunt.option("options")) {
            outputHelpText().then(this.async());
            return;
        }

        configureBuild()
            .then(function () {
                console.log("Building Ace Pump " + grunt.option("app") + " app.");

                if (grunt.option("clean")) {
                    grunt.task.run("clean");
                    grunt.task.run("init");
                }

                grunt.task.run("jshint");
                grunt.task.run("karma:" + grunt.option("app"));

                appManager.getConcatTargetNames(grunt.option("app")).forEach(function (targetName) {
                    grunt.task.run("concat:" + targetName);
                });

                appManager.getUglifyTargetNames(grunt.option("app")).forEach(function (targetName) {
                    grunt.task.run("uglify:" + targetName);
                });

                grunt.task.run("copy-static-content");
                grunt.task.run("copy-server");
                grunt.task.run("inject-environment-info");

                if (appManager.isMobileApp(grunt.option("app"))) {
                    grunt.task.run("reverse-copy-minified-mobile-app");
                }

                if (grunt.option("run")) {
                    grunt.task.run("run");
                }
            })
            .then(this.async());
    });

    function getDest(app, def) {
        var dest = "";

        if (def.destPath) dest += app.options.paths[def.destPath];
        if (def.dest) dest += def.dest;
        if (!dest) dest = BASE_PATH;

        return dest;
    }

    function configureBuild() {
        var BuildPersistedData = require("./build-static/node-tools/BuildPersistedData.js");
        return BuildPersistedData.get()
            .then(function (persistedData) {
                if (!grunt.option("app")) grunt.option("app", "mvc");
                var nextBuildNumber = persistedData.incrementBuildNumber(grunt.option("app"));

                grunt.option("build-number", nextBuildNumber + persistedData.raw.userCode);
                console.log("Build #" + nextBuildNumber + persistedData.raw.userCode);

                if (grunt.option("publish")) {
                    grunt.option("clean", true);

                    var modeParts = grunt.option("publish").split("/");
                    grunt.option("app", modeParts[0]);
                    grunt.option("build-mode", modeParts[1]);
                    grunt.option("rebuild", true);
                }

                if (!grunt.option("clean")) grunt.option("clean", false);
                if (!grunt.option("app")) grunt.option("app", "mvc");
                if (!grunt.option("build-mode")) grunt.option("build-mode", "Debug");
                if (!grunt.option("run")) grunt.option("run", false);

                if (grunt.option("app") !== persistedData.lastApp) grunt.option("clean", true);
                persistedData.lastApp = grunt.option("app");

                return persistedData;
            })
            .then(function (data) {
                return data.save();
            });
    }

    function outputHelpText() {
        var defer = q.defer();

        var fsRaw = require("fs");
        var s = fsRaw.createReadStream("./build-static/build-help.txt");
        s.pipe(process.stdout);
        s.on("close", defer.resolve);

        return defer.promise;
    }
};