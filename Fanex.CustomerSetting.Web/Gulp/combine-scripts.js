"use strict";

module.exports = function (configs) {
    //require("typescript-require");

    var concat = require("gulp-concat");
    var gulp = require("gulp");
    var gulpif = require("gulp-if");
    var merge = require("merge-stream");
    var minifyHTML = require("gulp-minify-html");
    var path = require("path");
    var rename = require("gulp-rename");
    var rimraf = require("rimraf");
    var runSequence = require("run-sequence");
    var templateCache = require("gulp-angular-templatecache");
    var ts = require("gulp-typescript");
    var uglify = require("gulp-uglify");

    var tsCompilingConfig = {
        typescript: require("typescript"),
        target: "ES5",
        noImplicitAny: false,
        removeComments: true
    };

    var sourceDir = path.join(process.cwd(), configs.root, configs.scripts, configs.apps, configs.packages.customerSetting);
    var destinationDir = path.join(process.cwd(), configs.root, configs.publish, configs.packages.customerSetting);
    var tempDir = path.join(sourceDir, "Temp");

    gulp.task("product:scripts:compile", function () {
        var productsConfigs = require(path.join(process.cwd(), configs.gulpDir, configs.configFiles.productsConfig));
        var streams = [];

        productsConfigs.forEach(function (prodConfig) {
            var sources = [
                path.join(sourceDir, "Core/**/*.{htm,ts}"),
                path.join(sourceDir, "Resource/*.{htm,ts}")
            ];

            if (prodConfig.settingTypes.length > 0) {
                prodConfig.settingTypes.forEach(function (item) {
                    sources.push(path.join(sourceDir, "SettingType" + item.settingType + ".Template" + item.settingTemplate, "*.{htm,ts}"));
                });
            }

            streams.push(gulp.src(sources)
                             .pipe(gulpif(/[.]ts$/, ts(tsCompilingConfig)))
                             .pipe(gulpif(/[.]htm$/, minifyHTML({ empty: true, quotes: true })))
                             .pipe(gulpif(/[.]htm$/, templateCache({
                                 module: "customerSetting"
                             })))
                             .pipe(concat("product-" + prodConfig.productId + ".js"))
                             .pipe(gulp.dest(tempDir)));
        });

        return merge(streams);
    });

    gulp.task("product:js:combine", ["product:scripts:compile"], function () {
        var productsConfigs = require(path.join(process.cwd(), configs.gulpDir, configs.configFiles.productsConfig));
        var streams = [];

        productsConfigs.forEach(function (prodConfig) {
            var sources = [
                path.join(process.cwd(), configs.bower, "angular/angular.js"),
                path.join(tempDir, "product-" + prodConfig.productId + ".js"),
                path.join(sourceDir, "Core/**/*.js"),
                path.join(sourceDir, "Resource/*.js")
            ];
            var destination = path.join(destinationDir, configs.scripts);

            if (prodConfig.settingTypes.length > 0) {
                prodConfig.settingTypes.forEach(function (item) {
                    sources.push(path.join(sourceDir, "SettingType" + item.settingType + ".Template" + item.settingTemplate, "*.js"));
                });
            }

            streams.push(gulp.src(sources)
                             .pipe(concat("product-" + prodConfig.productId + ".js"))
                             .pipe(gulp.dest(destination))
                             .pipe(uglify())
                             .pipe(rename({ suffix: ".min" }))
                             .pipe(gulp.dest(destination)));
        });

        return merge(streams);
    });

    gulp.task("product:temp:clean", function (callback) {
        rimraf(tempDir, callback);
    });

    gulp.task("template:copy", function () {
        return gulp.src(path.join(sourceDir, "*.htm"))
                   .pipe(minifyHTML({ empty: true, quotes: true }))
                   .pipe(gulp.dest(destinationDir));
    });

    gulp.task("scripts:publish", function () {
        runSequence("product:js:combine", "template:copy", "product:temp:clean");
    });

    gulp.task("images:publish", function () {
        return gulp.src(path.join(process.cwd(), configs.root, configs.styles, "Images/*.*"))
                   .pipe(gulp.dest(path.join(destinationDir, configs.styles, "Images")));
    });

    gulp.task("styles:publish", ["images:publish"], function () {
        return gulp.src(path.join(process.cwd(), configs.root, configs.styles, "customer-setting.{*.map,css,*.css}"))
                   .pipe(gulp.dest(path.join(destinationDir, configs.styles)));
    });
};