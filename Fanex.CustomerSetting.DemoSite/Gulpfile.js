/// <vs />
(function (require) {
    "use strict";

    var concat = require("gulp-concat");
    var gulp = require("gulp");
    var gulpif = require("gulp-if");
    var path = require("path");
    var templateCache = require("gulp-angular-templatecache");
    var tsd = require("gulp-tsd");
    var ts = require("gulp-typescript");
    var tslint = require("gulp-tslint");
    var rimraf = require("rimraf");
    var runSequence = require("run-sequence");

    var paths = {
        source: "../Fanex.CustomerSetting.FrontEnd/Assets/Publish/**",
        app: "./App/",
        content: "./Content",
        scripts: "./Scripts"
    };

    gulp.task("clean", function (callback) {
        rimraf(path.join(paths.app), callback);
    });

    gulp.task("tsd:install", function (callback) {
        tsd({
            command: "reinstall",
            config: "tsd.json"
        }, callback);
    });

    gulp.task("customer-setting:copy", function () {
        return gulp.src(paths.source)
                   .pipe(gulp.dest(paths.app));
    });

    gulp.task("tslint:check", function () {
        return gulp.src(path.join(paths.scripts, "DemoForm*/*.ts"))
                   .pipe(tslint({
                       configuration: "tslint.json"
                   }))
                   .pipe(tslint.report("verbose"));
    });

    gulp.task("scripts:compile", function () {
        var dest = path.join(paths.scripts, "DemoForm");
        var sources = [
            path.join(dest, "demo-form.ts"),
            path.join(dest, "posting-to-iframe.htm")
        ];

        return gulp.src(sources)
                   .pipe(gulpif(/[.]ts$/, ts({
                       typescript: require("typescript"),
                       target: "ES5",
                       noImplicitAny: false,
                       removeComments: true
                   })))
                   .pipe(gulpif(/[.]htm$/, templateCache({
                       module: "actorForm"
                   })))
                   .pipe(concat("demo-form.js"))
                   .pipe(gulp.dest(dest));
    });

    gulp.task("watch", ["clean", "customer-setting:copy", "scripts:compile"], function () {
        gulp.watch(paths.source, function () {
            setTimeout(function () {
                runSequence("clean", "customer-setting:copy");
            }, 3000);
        });

        gulp.watch(path.join(paths.scripts + "DemoForm/*.{ts,htm}"), function () {
            runSequence("tslint:check", "scripts:compile");
        });
    });

    gulp.task("default", ["clean"], function () {
        runSequence(
            "tslint:check",
            "scripts:compile",
            "customer-setting:copy");
    });

    gulp.task("config:update:packages:publish", function () {
        runSequence("product:configs:query", "default");
    });
})(require);