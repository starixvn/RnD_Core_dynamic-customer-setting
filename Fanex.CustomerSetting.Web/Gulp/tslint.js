"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var path = require("path");
    var tslint = require("gulp-tslint");

    gulp.task("tslint:check", function () {
        var sources = [];

        if (configs.apps) {
            configs.apps.forEach(function (app) {
                sources.push(path.join(process.cwd(), configs.root, configs.scripts, app, "**/*.ts"));
            });
        }

        return gulp.src(sources)
                   .pipe(tslint({
                       configuration: path.join(process.cwd(), configs.gulpDir, configs.configFiles.tslint)
                   }))
                   .pipe(tslint.report("prose"));
    });
};