"use strict";

module.exports = function (configs) {
    var concat = require("gulp-concat");
    var gulp = require("gulp");
    var gulpif = require("gulp-if");
    var minifyHTML = require("gulp-minify-html");
    var path = require("path");
    var rename = require("gulp-rename");
    var templateCache = require("gulp-angular-templatecache");
    var ts = require("gulp-typescript");
    var uglify = require("gulp-uglify");

    var sourceDir = path.join(process.cwd(), configs.root, configs.scripts, configs.apps, configs.packages.customerSettingMatrix);
    var destinationDir = path.join(process.cwd(), configs.root, configs.publish, configs.packages.customerSettingMatrix);
    var destTemplateDir = "../Fanex.CustomerSetting.Matrix.WebAPI/App_Data/Templates";

    gulp.task("matrix:script:publish", function () {
        var sources = [
            path.join(process.cwd(), configs.bower, "angular/angular.js"),
            path.join(sourceDir, "**/*.{htm,js,ts}")
        ];

        gulp.src(sources)
            .pipe(gulpif(/[.]ts$/, ts({
                typescript: require("typescript"),
                target: "ES5",
                noImplicitAny: false,
                removeComments: true
            })))
            .pipe(gulpif(/[.]htm$/, minifyHTML({ empty: true, quotes: true })))
            .pipe(gulpif(/[.]htm$/, templateCache({
                module: "customerSettingMatrix"
            })))
            .pipe(concat("customer-setting-matrix.js"))
            .pipe(gulp.dest(destinationDir))
            .pipe(uglify())
            .pipe(rename({ suffix: ".min" }))
            .pipe(gulp.dest(destinationDir));
    });

    gulp.task("matrix:template:copy", function () {
        return gulp.src(path.join(sourceDir, "*.htm"))
                   .pipe(minifyHTML({ empty: true, quotes: true }))
                   .pipe(gulp.dest(path.join(process.cwd(), destTemplateDir)));
    });

    gulp.task("matrix:images:publish", function () {
        return gulp.src(path.join(process.cwd(), configs.root, configs.styles, "Images/*.*"))
                   .pipe(gulp.dest(path.join(destinationDir, configs.styles, "Images")));
    });

    gulp.task("matrix:styles:publish", ["matrix:images:publish"], function () {
        return gulp.src(path.join(process.cwd(), configs.root, configs.styles, "customer-setting-matrix*.{map,css}"))
                   .pipe(gulp.dest(path.join(destinationDir, configs.styles)));
    });

    gulp.task("matrix:publish", ["matrix:script:publish", "matrix:template:copy", "matrix:styles:publish"]);
};