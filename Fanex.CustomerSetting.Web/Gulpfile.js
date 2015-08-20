/// <vs BeforeBuild='publish' SolutionOpened='modules:compile' />
(function (require) {
    "use strict";

    var gulp = require("gulp");
    var path = require("path");
    var rimraf = require("rimraf");
    var util = require("gulp-util");
    var wrench = require("wrench");

    var configs = {
        gulpDir: "Gulp",
        bower: "bower_components",
        root: "Assets",
        scripts: "Scripts",
        styles: "Styles",
        publish: "Publish",
        apps: [
            "App",
            "App.IE8"
        ],
        specs: [
            "Specs",
            "Specs.IE8"
        ],
        configFiles: {
            bower: "bower.json",
            tslint: "tslint.json",
            tsd: "gulp-tsd.json"
        },
        publishModules: {
            ie8: "IE8",
            modernBrowsers: "ModernBrowsers",
            styles: "Styles"
        },
        errorHandler: function (title) {
            return function (error) {
                util.log(util.colors.red("[" + title + "]"), error.toString());
                this.emit("end");
            };
        }
    };

    wrench.readdirSyncRecursive(configs.gulpDir).filter(function (file) {
        return (/\.js$/i).test(file);
    }).map(function (file) {
        require(path.join(process.cwd(), configs.gulpDir, file))(configs);
    });

    gulp.task("clean", function (callback) {
        rimraf(path.join(process.cwd(), configs.root, configs.publish), callback);
    });

    gulp.task("install", ["libraries:install", "tsd:install"]);

    gulp.task("publish", ["clean"], function () {
        gulp.start("styles:publish");
        gulp.start("scripts:publish");
    });

    gulp.task("default", ["install"], function () {
        gulp.start("publish");
    });
})(require);