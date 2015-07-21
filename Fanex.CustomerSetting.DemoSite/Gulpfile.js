(function (require) {
    "use strict";

    var gulp = require("gulp");
    var paths = {
        source: "../Fanex.CustomerSetting.Web/Assets/Publish/App/CustomerSetting/**",
        app: "./App/CustomerSetting",
        bower: "./bower_components/",
        content: "./Content/",
        compass: "./Content/Compass/",
        script: "./Scripts/",
        library: "./Scripts/Libraries/"
    };

    gulp.task("clean", function () {
        var clean = require("gulp-clean");

        return gulp.src(paths.app + "**", { read: false })
                   .pipe(clean());
    });

    gulp.task("install-tsd", function (callback) {
        var tsd = require("gulp-tsd");

        tsd({
            command: "reinstall",
            latest: true,
            config: "tsd.json"
        }, callback);
    });

    gulp.task("install-bower", function () {
        var bower = require("gulp-bower");

        return bower();
    });

    gulp.task("install-compass", function () {
        return gulp.src(paths.bower + "compass-mixins/lib/**")
                   .pipe(gulp.dest(paths.compass));
    });

    gulp.task("install-jquery", function () {
        return gulp.src(paths.bower + "jquery/dist/**")
                   .pipe(gulp.dest(paths.library + "JQuery"));
    });

    gulp.task("install-angular", function () {
        return gulp.src(paths.bower + "Angular/angular*")
                   .pipe(gulp.dest(paths.library + "AngularJs"));
    });

    gulp.task("install-jslibs", ["install-jquery", "install-angular"]);

    gulp.task("copy-customer-setting-asset", function () {
        return gulp.src(paths.source)
                   .pipe(gulp.dest(paths.app));
    });

    // When using gulp-sass to compile scss, it will use libSasss which will make the compass' functions in wrong results.
    // We can fix it by gulp-ruby-sass, which uses Ruby as runtime compiler but it needs the Ruby gem be installed on the building machine.
    gulp.task("compile-styles", function () {
        var sass = require("gulp-sass");
        var sourcemaps = require("gulp-sourcemaps");

        return gulp.src(paths.content + "*.scss")
                   .pipe(sass({ compass: true }))
                   .pipe(sourcemaps.init())
                   .pipe(sourcemaps.write())
                   .pipe(gulp.dest(paths.content));
    });

    gulp.task("compile-scripts", function () {
        var sourcemaps = require("gulp-sourcemaps");
        var ts = require("gulp-typescript");

        return gulp.src(paths.script + "*.ts")
                   .pipe(ts({
                       typescript: require("typescript"),
                       target: "ES5",
                       noImplicitAny: true,
                       removeComments: true
                   }))
                   .pipe(sourcemaps.write("."))
                   .pipe(gulp.dest(paths.script));
    });

    gulp.task("prebuild", function (done) {
        var runSequence = require("run-sequence");

        runSequence(
            ["clean", "install-tsd", "install-bower"],
            ["install-compass", "install-jslibs", "copy-customer-setting-asset"],
            ["compile-scripts"],
            done);
    });

    gulp.task("watch", ["compile-scripts", "copy-customer-setting-asset"], function () {
        gulp.watch(paths.source, ["copy-customer-setting-asset"]);
        gulp.watch(paths.script + "*.ts", ["compile-scripts"]);
    });
})(require);