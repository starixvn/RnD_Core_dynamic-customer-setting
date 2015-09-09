"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var path = require("path");
    var tslint = require("gulp-tslint");

    gulp.task("tslint:check", function () {
        var sourceDir = path.join(process.cwd(), configs.root, configs.scripts);
        var sources = [];

        if (configs.apps) {
            Object.keys(configs.packages).forEach(function (app) {
                sources.push(path.join(sourceDir, configs.packages[app], "**/*.ts"));
            });
        }

        return gulp.src(sources)
                   .pipe(tslint({
                       configuration: path.join(process.cwd(), configs.gulpDir, configs.configFiles.tslint)
                   }))
                   .pipe(tslint.report("prose"));
    });
};