"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var path = require("path");

    var sourceDir = path.join(process.cwd(), configs.root);

    gulp.task("watch", function () {
        var scriptsDir = path.join(sourceDir, configs.scripts, configs.apps);
        var stylesDir = path.join(sourceDir, configs.styles);

        gulp.watch(path.join(scriptsDir, configs.packages.customerSetting, "**/*.{js,ts,htm}"), ["scripts:publish"]);
        gulp.watch(path.join(stylesDir, "customer-setting.{*.map,css,*.css}"), ["styles:publish"]);
        gulp.watch(path.join(scriptsDir, configs.packages.customerSettingMatrix, "**/*.{js,ts,htm}"), ["matrix:script:publish", "matrix:template:copy"]);
        gulp.watch(path.join(stylesDir, "customer-setting-matrix*.{map,css}"), ["matrix:styles:publish"]);
    });
};