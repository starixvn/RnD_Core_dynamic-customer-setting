"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var path = require("path");

    var sourceDir = path.join(process.cwd(), configs.root);

    gulp.task("watch", ["scripts:publish", "styles:publish", "ie8:scripts:publish", "ie8:styles:publish"], function () {
        gulp.watch(path.join(sourceDir, configs.scripts, "App/**/*.{js,ts,htm}"), ["scripts:publish"]);
        gulp.watch(path.join(sourceDir, configs.styles, "**/customer-setting.{*.map,css,*.css}"), ["styles:publish"]);
        gulp.watch(path.join(sourceDir, configs.scripts, "App.IE8/**/*.{js,ts,htm}"), ["ie8:scripts:publish"]);
        gulp.watch(path.join(sourceDir, configs.styles, "**/customer-setting-ie8*.{map,css}"), ["ie8:styles:publish"]);
    });
};