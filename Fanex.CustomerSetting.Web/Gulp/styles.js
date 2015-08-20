"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var path = require("path");

    gulp.task("images:publish", function () {
        return gulp.src(path.join(process.cwd(), configs.root, configs.styles, "Images/*.*"))
                   .pipe(gulp.dest(path.join(process.cwd(), configs.root, configs.publish, configs.publishModules.styles, "Images")));
    });

    gulp.task("styles:publish", ["images:publish"], function () {
        return gulp.src(path.join(process.cwd(), configs.root, configs.styles, "customer-setting*.{map,css}"))
                   .pipe(gulp.dest(path.join(process.cwd(), configs.root, configs.publish, configs.publishModules.styles)));
    });
};