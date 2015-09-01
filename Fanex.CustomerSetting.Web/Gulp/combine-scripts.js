"use strict";

module.exports = function (configs) {
    //require("typescript-require");

    var concat = require("gulp-concat");
    var gulp = require("gulp");
    var gulpif = require("gulp-if");
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

    var sourceDir = path.join(process.cwd(), configs.root, configs.scripts, "App/");
    var destinationDir = path.join(process.cwd(), configs.root, configs.publish, "CustomerSetting");
    var tempDir = path.join(sourceDir, "Temp");

    gulp.task("product:compile", function (callback) {
        var productsConfigs = require(path.join(process.cwd(), configs.gulpDir, configs.configFiles.productsConfig));

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

            gulp.src(sources)
                .pipe(gulpif(/[.]ts$/, ts(tsCompilingConfig)))
                .pipe(gulpif(/[.]htm$/, minifyHTML({ empty: true, quotes: true })))
                .pipe(gulpif(/[.]htm$/, templateCache({
                    module: "customerSetting"
                })))
                .pipe(concat("product-" + prodConfig.productId + ".js"))
                .pipe(gulp.dest(tempDir));
        });

        //callback.call();
    });

    gulp.task("product:js:combine", function (callback) {
        var productsConfigs = require(path.join(process.cwd(), configs.gulpDir, configs.configFiles.productsConfig));

        productsConfigs.forEach(function (prodConfig) {
            var sources = [
                path.join(tempDir, "product-" + prodConfig.productId + ".js"),
                path.join(sourceDir, "Core/**/*.js"),
                path.join(sourceDir, "Resource/*.js")
            ];
            var destination = path.join(destinationDir, "Scripts");

            if (prodConfig.settingTypes.length > 0) {
                prodConfig.settingTypes.forEach(function (item) {
                    sources.push(path.join(sourceDir, "SettingType" + item.settingType + ".Template" + item.settingTemplate, "*.js"));
                });
            }

            gulp.src(sources)
                .pipe(concat("product-" + prodConfig.productId + ".js"))
                .pipe(gulp.dest(destination))
                .pipe(uglify())
                .pipe(rename({ suffix: ".min" }))
                .pipe(gulp.dest(destination));
        });

        //callback.call();
    });

    gulp.task("template:copy", function () {
        return gulp.src(path.join(sourceDir, "*.htm"))
                   .pipe(minifyHTML({ empty: true, quotes: true }))
                   .pipe(gulp.dest(destinationDir));
    });

    gulp.task("product:temp:clean", function (callback) {
        rimraf(tempDir, callback);
    });

    gulp.task("product:combine", function () {
        runSequence("product:compile", "product:js:combine", "template:copy", "product:temp:clean");
    });
};