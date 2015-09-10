"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var path = require("path");
    var ts = require("gulp-typescript");
    var tsCompilingConfig = {
        typescript: require("typescript"),
        target: "ES5",
        module: "commonjs"
    };

    var sourceDir = path.join(process.cwd(), configs.root, configs.scripts, configs.customNodeModules, "*.ts");
    var destinationDir = path.join(process.cwd(), configs.root, configs.scripts, configs.customNodeModules);

    gulp.task("modules:compile", function () {
        return gulp.src(sourceDir)
                   .pipe(ts(tsCompilingConfig))
                   .pipe(gulp.dest(destinationDir));
    });
};