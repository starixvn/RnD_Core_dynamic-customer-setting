/// <vs />
(function (require) {
    "use strict";

    var bower = require("gulp-bower");
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
        source: "../Fanex.CustomerSetting.Web/Assets/Publish/**",
        app: "./App/",
        bower: "./bower_components",
        content: "./Content",
        scripts: "./Scripts"
    };

    gulp.task("clean", function (callback) {
        rimraf(path.join(paths.app), callback);
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

    gulp.task("compass:install", function () {
        return gulp.src(path.join(paths.bower, "compass-mixins/lib/**"))
                   .pipe(gulp.dest(path.join(paths.content, "Compass")));
    });

    gulp.task("libraries:copy", function () {
        var bower = {
            "Angular": "angular/angular*.{js,map,css}",
            "Angular-Legacy": "angular-legacy/angular*.{js,map}",
            "Es5-Shim": "es5-shim/es5-shim*.{js,map}",
            "Html5shiv": "html5shiv/dist/html5shiv*.js",
            "JQuery": "jquery/dist/jquery*.{js,map}",
            "Underscore": "Underscore/underscore*.{js,map}"
        };

        for (var destinationDir in bower) {
            gulp.src(path.join(paths.bower, bower[destinationDir]))
                .pipe(gulp.dest(path.join(paths.scripts, "Libraries", destinationDir)));
        }
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

    gulp.task("scripts:ie8:compile", function () {
        var dest = path.join(paths.scripts, "DemoForm.IE8");
        var sources = [
            path.join(dest, "demo-form.ts"),
            path.join(dest, "posting-to-iframe.htm")
        ];

        return gulp.src(sources)
                   .pipe(gulpif(/[.]ts$/, ts({
                       typescript: require("typescript"),
                       target: "ES3",
                       noImplicitAny: false,
                       removeComments: true
                   })))
                   .pipe(gulpif(/[.]htm$/, templateCache({
                       module: "actorForm"
                   })))
                   .pipe(concat("demo-form.js"))
                   .pipe(gulp.dest(dest));
    });

    gulp.task("watch", ["clean", "customer-setting:copy", "scripts:compile", "scripts:ie8:compile"], function () {
        gulp.watch(paths.source, function () {
            setTimeout(function () {
                runSequence("clean", "customer-setting:copy");
            }, 3000);
        });

        gulp.watch(path.join(paths.scripts + "DemoForm/*.{ts,htm}"), function () {
            runSequence("tslint:check", "scripts:compile");
        });

        gulp.watch(path.join(paths.scripts + "DemoForm.IE8/*.{ts,htm}"), function () {
            runSequence("tslint:check", "scripts:ie8:compile");
        });
    });

    gulp.task("default", ["clean"], function () {
        runSequence(
            ["tsd:install", "bower:install"],
            ["compass:install", "libraries:copy"],
            "tslint:check",
            "scripts:compile",
            "scripts:ie8:compile",
            "customer-setting:copy");
    });
})(require);