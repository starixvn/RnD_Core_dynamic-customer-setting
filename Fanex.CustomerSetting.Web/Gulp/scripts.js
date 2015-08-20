"use strict";

module.exports = function (configs) {
    //require("typescript-require");

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
        target: "ES5",
        noImplicitAny: false,
        removeComments: true
    };

    var sourceDir = path.join(process.cwd(), configs.root, configs.scripts, "App/");
    var destinationDir = path.join(process.cwd(), configs.root, configs.publish);
    var tempDir = path.join(destinationDir, "Core/Temp");

    gulp.task("scripts:core:build", function (callback) {
        var source = path.join(sourceDir, "Core/**/*.{htm,js,ts}");

        return gulp.src(source)
                   .pipe(gulpif(/[.]ts$/, ts(tsCompilingConfig)))
                   .pipe(gulpif(/[.]htm$/, minifyHTML({ empty: true })))
                   .pipe(gulpif(/[.]htm$/, templateCache({
                       module: "customerSetting"
                   })))
                   .pipe(concat("core.js"))
                   .pipe(gulp.dest(tempDir));
    });

    gulp.task("scripts:resource:build", function (callback) {
        var source = path.join(sourceDir, "Resource/*.{js,ts}");

        return gulp.src(source)
                   .pipe(gulpif(/[.]ts$/, ts(tsCompilingConfig)))
                   .pipe(concat("resouce.js"))
                   .pipe(gulp.dest(tempDir));
    });

    gulp.task("scripts:core:publish", ["scripts:core:build", "scripts:resource:build"], function () {
        var destination = path.join(destinationDir, "Core");

        return gulp.src(path.join(tempDir, "*.js"))
                   .pipe(concat("core.js"))
                   .pipe(gulp.dest(destination))
                   .pipe(uglify())
                   .pipe(rename({ suffix: ".min" }))
                   .pipe(gulp.dest(destination));
    });

    gulp.task("scripts:modules:publish", function (callback) {
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

    gulp.task("template:publish", function () {
        return gulp.src(path.join(sourceDir, "*.htm"))
                   .pipe(minifyHTML({ empty: true }))
                   .pipe(gulp.dest(destinationDir));
    });

    gulp.task("scripts:publish", ["scripts:core:publish", "scripts:modules:publish", "template:publish"], function (callback) {
        rimraf(tempDir, callback);
    });
};