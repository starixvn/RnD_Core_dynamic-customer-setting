/// <vs BeforeBuild='default' />
/*global require*/
(function (require) {
    "use strict";
    var gulp = require('gulp'),
        tsd = require("gulp-tsd"),
        ts = require("gulp-typescript"),

        paths = {
            bower: "./bower_components/",
            libraries: "./Scripts/Libraries/",
            app: "./Scripts/App/"
        };

    gulp.task("tsd", function (callback) {
        tsd({
            command: "reinstall",
            latest: true,
            config: "tsd.json"
        }, callback);
    });

    gulp.task("lib-copy", function () {
        var bower = {
            "AngularJs": "angularjs/angular*.{js,map}",
            "JQuery": "jquery/dist/jquery*.{js,map}",
            "Compass": "compass-mixins/lib/**"
        };

        for (var destinationDir in bower) {
            gulp.src(paths.bower + bower[destinationDir])
                .pipe(gulp.dest(paths.libraries + destinationDir));
        }
    });

    gulp.task("ts-compile", function () {
        gulp.src(paths.app)
            .pipe(ts({
                noImplicitAny: true
            }))
            .pipe(gulp.dest(paths.app));
    });

    gulp.task("default", ["ts-compile", "tsd", "lib-copy"]);
}(require));