"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var path = require("path");

    var destinations = configs.packages.map(function (app) {
        return path.join(process.cwd(), configs.root, configs.publish, app, configs.styles);
    });

    gulp.task("images:publish", function () {
        var src = gulp.src(path.join(process.cwd(), configs.root, configs.styles, "Images/*.*"));

        destinations.forEach(function (dest) {
            src.pipe(gulp.dest(path.join(dest, "Images")));
        });

        return src;
    });

    gulp.task("styles:publish", ["images:publish"], function () {
        var src = gulp.src(path.join(process.cwd(), configs.root, configs.styles, "customer-setting*.{map,css}"));

        destinations.forEach(function (dest) {
            src.pipe(gulp.dest(dest));
        });

        return src;
    });
};