var child_proc  = require("child_process"),
    q           = require("q");

module.exports = IisExpress;

function IisExpress(options) {
    this.options = _extend(options, {
        iisExpressPath: IisExpress.DEFAULTS.IISEXPRESS_PATH
    });
}

//Static
_extend(IisExpress, {
    DEFAULTS: {
        IISEXPRESS_PATH: process.env["ProgramFiles(x86)"] + "\\IIS Express\\iisexpress.exe",
        HOST_CONFIG: "./build-static/iisexpress.config"
    },

    get: static_get
});

//Instance
_extend(IisExpress.prototype, {
    start: start
});

function static_get() {
    if (!IisExpress._singleton) {
        IisExpress._singleton = new IisExpress();
    }

    return IisExpress._singleton;
}

/*
 * Start IIS Express with the specified config file.
 * Pipes all IO to originating process.
 *
 * Returns a promise that resolves when IIS Express closes
 */
function start(options) {
    _extend(options, {
        hostConfigPath: IisExpress.DEFAULTS.HOST_CONFIG
    });

    var completedDeferred = q.defer();
    var iisExpress = child_proc.spawn(this.options.iisExpressPath, ["/config:" + options.hostConfigPath]);
    iisExpress.on("close", function () {
        completedDeferred.resolve();
    });

    iisExpress.stdout.pipe(process.stdout);

    return completedDeferred.promise;
}

function _extend(target, source) {
    if (!target) target = {};

    for (var prop in source) {
        if (source.hasOwnProperty(prop) && !(prop in target)) {
            target[prop] = source[prop];
        }
    }

    return target;
}
