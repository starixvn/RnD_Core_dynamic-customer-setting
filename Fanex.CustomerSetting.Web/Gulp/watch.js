"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var path = require("path");

    var sourceDir = path.join(process.cwd(), configs.root);

    gulp.task("watch", ["scripts:publish", "ie8:scripts:publish", "styles:publish"], function () {
        gulp.watch(path.join(sourceDir, configs.scripts, "App/**/*.{js,ts,htm}"), ["scripts:publish"]);
        gulp.watch(path.join(sourceDir, configs.scripts, "App.IE8/**/*.{js,ts,htm}"), ["ie8:scripts:publish"]);
        gulp.watch(path.join(sourceDir, configs.styles, "**/*.{map,css,gif,png}"), ["styles:publish"]);
    });
};