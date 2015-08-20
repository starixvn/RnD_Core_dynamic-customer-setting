"use strict";

module.exports = function (configs) {
    var bower = require("gulp-bower");
    var gulp = require("gulp");
    var path = require("path");

    gulp.task("bower:install", function () {
        return bower();
    });

    gulp.task("compass:copy", function () {
        return gulp.src(path.join(process.cwd(), configs.bower, "compass-mixins/lib/**"))
                   .pipe(gulp.dest(path.join(process.cwd(), configs.root, configs.styles, "Compass")));
    });

    gulp.task("mocks:copy", function () {
        var source = gulp.src(path.join(process.cwd(), configs.bower, "angular-mocks/*.js"));

        configs.specs.forEach(function (spec) {
            source.pipe(gulp.dest(path.join(process.cwd(), configs.root, configs.scripts, spec, "Mocks")));
        });

        return source;
    });

    gulp.task("libraries:install", ["bower:install"], function () {
        gulp.start("compass:copy");
        gulp.start("mocks:copy");
    });
};