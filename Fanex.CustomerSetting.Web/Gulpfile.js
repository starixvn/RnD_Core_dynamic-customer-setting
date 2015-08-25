/// <vs SolutionOpened='modules:compile, clean' />
(function (require) {
    "use strict";

    var gulp = require("gulp");
    var path = require("path");
    var rimraf = require("rimraf");
    var runSequence = require("run-sequence");
    var util = require("gulp-util");
    var wrench = require("wrench");

    var configs = {
        gulpDir: "Gulp",
        bower: "bower_components",
        root: "Assets",
        scripts: "Scripts",
        styles: "Styles",
        publish: "Publish",
        packages: [
            "CustomerSetting",
            "CustomerSetting.IE8"
        ],
        apps: [
            "App",
            "App.IE8",
            "SettingMatrixApp"
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
        runSequence(["styles:publish", "scripts:publish", "ie8:scripts:publish"], "matrix:publish");
    });

    gulp.task("default", ["install"], function () {
        gulp.start("publish");
    });
})(require);