"use strict";

module.exports = function (configs) {
    var concat = require("gulp-concat");
    var embedTemplate = require("../Assets/Scripts/NodeModules/embeded-template.js");
    var fs = require("fs");
    var gulp = require("gulp");
    var gulpif = require("gulp-if");
    var minifyHTML = require("gulp-minify-html");
    var path = require("path");
    var rename = require("gulp-rename");
    var rimraf = require("rimraf");
    var templateCache = require("gulp-angular-templatecache");
    var ts = require("gulp-typescript");
    var uglify = require("gulp-uglify");

    var tsCompilingConfig = {
        typescript: require("typescript"),
        target: "ES3",
        noImplicitAny: false,
        removeComments: true
    };

    var sourceDir = path.join(process.cwd(), configs.root, configs.scripts, "App.IE8/");
    var destinationDir = path.join(process.cwd(), configs.root, configs.publish, "CustomerSetting.IE8");
    var tempDir = path.join(sourceDir, "Core/Temp");

    gulp.task("ie8:libraries:combine", function () {
        var libs = [
            path.join(process.cwd(), configs.bower, "jquery/jquery.min.js"),
            path.join(process.cwd(), configs.bower, "underscore/underscore-min.js"),
            path.join(process.cwd(), configs.bower, "angular-legacy/angular.min.js"),
            path.join(process.cwd(), configs.bower, "oclazyload/dist/ocLazyLoad.min.js")
        ];

        gulp.src(libs)
            .pipe(concat("libraries.js"))
            .pipe(gulp.dest(path.join(destinationDir, "Libraries")));
    });

    gulp.task("ie8:tricks:combine", function () {
        var libs = [
            path.join(process.cwd(), configs.bower, "es5-shim/es5-shim.min.js"),
            path.join(process.cwd(), configs.bower, "html5shiv/dist/html5shiv.min.js")
        ];

        gulp.src(libs)
            .pipe(concat("ie8tricks.js"))
            .pipe(gulp.dest(path.join(destinationDir, "Libraries")));
    });

    gulp.task("ie8:scripts:core:build", function () {
        var source = path.join(sourceDir, "Core/**/*.{htm,js,ts}");

        return gulp.src(source)
                   .pipe(gulpif(/[.]ts$/, ts(tsCompilingConfig)))
                   .pipe(gulpif(/[.]htm$/, minifyHTML({ empty: true, quotes: true })))
                   .pipe(gulpif(/[.]htm$/, templateCache({
                       module: "customerSetting"
                   })))
                   .pipe(concat("core.js"))
                   .pipe(gulp.dest(tempDir));
    });

    gulp.task("ie8:scripts:resource:build", function () {
        var source = path.join(sourceDir, "Resource/*.{js,ts}");

        return gulp.src(source)
                   .pipe(gulpif(/[.]ts$/, ts(tsCompilingConfig)))
                   .pipe(concat("resouce.js"))
                   .pipe(gulp.dest(tempDir));
    });

    gulp.task("ie8:scripts:core:publish", ["ie8:scripts:core:build", "ie8:scripts:resource:build"], function () {
        var destination = path.join(destinationDir, "Core");

        return gulp.src(path.join(tempDir, "*.js"))
                   .pipe(concat("core.js"))
                   .pipe(gulp.dest(destination))
                   .pipe(uglify())
                   .pipe(rename({ suffix: ".min" }))
                   .pipe(gulp.dest(destination));
    });

    gulp.task("ie8:scripts:modules:publish", function (callback) {
        var modules = [];
        var reservedDirs = ["Core", "Resource"];

        fs.readdirSync(sourceDir).filter(function (item) {
            if (fs.statSync(path.join(sourceDir, item)).isDirectory() && reservedDirs.indexOf(item) < 0) {
                modules.push(item);
            }
        });

        if (modules.length > 0) {
            modules.forEach(function (module) {
                var source = path.join(sourceDir, module, "*.{js,ts}");
                var destination = path.join(destinationDir, module);

                gulp.src(source)
                    .pipe(gulpif(/[.]ts$/, ts(tsCompilingConfig)))
                    .pipe(embedTemplate())
                    .pipe(concat(module.toLowerCase().replace(".", "-") + ".js"))
                    .pipe(gulp.dest(destination))
                    .pipe(uglify())
                    .pipe(rename({ suffix: ".min" }))
                    .pipe(gulp.dest(destination));
            });
        }

        callback.call();
    });

    gulp.task("ie8:template:publish", function () {
        return gulp.src(path.join(sourceDir, "*.htm"))
                   .pipe(minifyHTML({ empty: true, quotes: true }))
                   .pipe(gulp.dest(destinationDir));
    });

    gulp.task(
        "ie8:scripts:publish",
        [
            "ie8:libraries:combine",
            "ie8:tricks:combine",
            "ie8:scripts:core:publish",
            "ie8:scripts:modules:publish",
            "ie8:template:publish"
        ],
        function (callback) {
            rimraf(tempDir, callback);
        });
};