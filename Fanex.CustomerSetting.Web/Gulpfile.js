/// <vs BeforeBuild='default' />
/*global require*/
(function (require) {
    "use strict";
    var gulp = require('gulp'),
        gulpif = require("gulp-if"),
        concat = require("gulp-concat"),
        tsd = require("gulp-tsd"),
        templateCache = require("gulp-angular-templatecache"),
        rimraf = require("rimraf"),
        karma = require("gulp-karma-runner"),
        ts = require("gulp-typescript"),
        fs = require("fs"),
        uglify = require("gulp-uglify"),

        paths = {
            bower: "./bower_components/",
            publish: "./Assets/Publish/",
            libraries: "./Assets/Libraries/",
            app: "./Assets/Scripts/App/",
            style: "./Assets/Styles/"
        };

    gulp.task("tsd", function (callback) {
        tsd({
            command: "reinstall",
            latest: true,
            config: "tsd.json"
        }, callback);
    });

    gulp.task("unit-test", function () {
        gulp.src([
            "Assets/Scripts/App/**/*.js",
            "Assets/Scripts/Specs/**/*.js"
        ], { "read": false }).pipe(
            karma.server({
                "singleRun": true,
                "frameworks": ['jasmine'],
                "browsers": ['Chrome']
            })
        );
    });

    gulp.task("clean", function (callback) {
        rimraf(paths.libraries, callback);
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
        var modules = [],
            moduleName = '';

        fs.readdirSync(paths.app).filter(function (item) {
            if (fs.statSync(paths.app + item).isDirectory()) {
                modules.push(item.toLowerCase());
            }
        });

        for (var index in modules) {
            moduleName = modules[index];

            gulp.src(paths.app + moduleName + "/**")
                .pipe(gulpif(/[.]ts$/, ts({
                    noImplicitAny: true,
                    out: moduleName + ".js"
                })))
                .pipe(gulpif(/[.]htm$/, templateCache({
                    module: "fanex-customer-setting"
                })))
                .pipe(concat(moduleName + ".js"))
                .pipe(uglify())
                .pipe(gulp.dest(paths.publish + "Scripts/App"));
        }
    });

    gulp.task("style-copy", function () {
        gulp.src(paths.style + "Images/**")
            .pipe(gulp.dest(paths.publish + "Styles/Images"));

        gulp.src(paths.style + "*.min.css")
            .pipe(gulp.dest(paths.publish + "Styles"));
    });

    gulp.task("default", ["style-copy", "ts-compile", "tsd", "lib-copy"]);
}(require));