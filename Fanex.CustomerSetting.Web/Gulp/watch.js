"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var path = require("path");

    var sourceDir = path.join(process.cwd(), configs.root);

    gulp.task("watch", ["scripts:publish", "styles:publish"], function () {
        gulp.watch(path.join(sourceDir, configs.scripts, "**/*.{js,ts,htm}"), ["scripts:publish"]);
        gulp.watch(path.join(sourceDir, configs.styles, "**/*.{map,css,gif,png}"), ["styles:publish"]);
    });
};