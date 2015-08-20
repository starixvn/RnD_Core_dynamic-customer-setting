(function (require) {
    "use strict";

    var bower = require("gulp-bower");
    var gulp = require("gulp");
    var path = require("path");
    var rimraf = require("rimraf");

    var paths = {
        source: "../Fanex.CustomerSetting.Web/Assets/Publish/**",
        app: "./App/CustomerSetting",
        bower: "./bower_components/",
        content: "./Content/",
        compass: "./Content/Compass/",
        demoForm: "./Scripts/DemoForm/",
        library: "./Scripts/Libraries/"
    };

    gulp.task("clean", function (callback) {
        rỉmraf(path.join(paths.app, "**"), callback);
    });

    gulp.task("tsd:install", function (callback) {
        tsd({
            command: "reinstall",
            latest: true,
            config: "tsd.json"
        }, callback);
    });

    gulp.task("bower:install", function () {
        return bower();
    });
})(require);