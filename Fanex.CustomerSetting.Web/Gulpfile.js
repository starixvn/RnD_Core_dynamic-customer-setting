(function (require) {
    "use strict";

    var gulp = require("gulp");
    var banner = [
                "/**",
                " * <%= pkg.name %> - <%= pkg.description %>",
                " * @version v<%= pkg.version %>",
                " * @author <%= pkg.author %>",
                " * @license <%= pkg.license %>",
                " */",
                ""].join("\n");
    var assetPaths = {
        bower: "./bower_components/",
        app: "./Assets/Scripts/App/",
        style: "./Assets/Styles/*.scss",
        image: "./Assets/Styles/Images/",
        compass: "./Assets/Styles/Compass/"
    };
    var publishPaths = {
        root: "./Asset/Publish/",
        customerSetting: "./Assets/Publish/App/CustomerSetting/",
        image: "./Assets/Publish/App/CustomerSetting/Images/",
        library: "./Assets/Publish/Libraries/"
    };
    var newVersion;

    gulp.task("bump-version", function () {
        var prompt = require("gulp-prompt");
        var semver = require("semver");
        var pkg = require("./package.json");

        return gulp.src("")
                   .pipe(prompt.prompt({
                       type: "list",
                       name: "increase version",
                       message: "What type of version bump would you like to do ? (current version is " + pkg.version + ")",
                       choices: [
                           "patch (" + pkg.version + " --> " + semver.inc(pkg.version, "patch") + ")",
                           "minor (" + pkg.version + " --> " + semver.inc(pkg.version, "minor") + ")",
                           "major (" + pkg.version + " --> " + semver.inc(pkg.version, "major") + ")",
                           "none (exit)"
                       ]
                   }), function (result) {
                       if (result.bump.match(/^patch/)) {
                           newVersion = semver.inc(pkg.version, "patch");
                       } else if (result.bump.match(/^minor/)) {
                           newVersion = semver.inc(pkg.version, "minor");
                       } else if (result.bump.match(/^major/)) {
                           newVersion = semver.inc(pkg.version, "major");
                       }
                   });
    });

    gulp.task("update-version", function () {
        var jsonEditor = require("gulp-json-editor");

        gulp.src("./package.json")
            .pipe(jsonEditor({
                version: newVersion
            }))
            .pipe(gulp.dest("./"));

        gulp.src("./bower.json")
            .pipe(jsonEditor({
                version: newVersion
            }))
            .pipe(gulp.dest("./"));
    });

    gulp.task("clean", function () {
        var clean = require("gulp-clean");

        return gulp.src(publishPaths.root + "**", { read: false })
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

    gulp.task("install-bower2", function () {
        var bower = require("gulp-bower");
        var filter = require("gulp-filter");

        return bower().pipe(filter(assetPaths.bower + "compass-mixins/lib/**"))
                      .pipe(gulp.dest(assetPaths.compass));
    });

    gulp.task("install-bower", function () {
        var bower = require("gulp-bower");

        return bower();
    });

    gulp.task("install-compass", function () {
        console.log(assetPaths.bower + "compass-mixins/lib/**");
        return gulp.src(assetPaths.bower + "compass-mixins/lib/**")
            .pipe(gulp.dest(assetPaths.compass));
    });

    gulp.task("publish-template", function () {
        return gulp.src(assetPaths.app + "*.htm")
                   .pipe(gulp.dest(publishPaths.customerSetting));
    });

    gulp.task("publish-styles", function () {
        var header = require("gulp-header");
        var rename = require("gulp-rename");
        var sass = require("gulp-sass");
        var sourcemaps = require("gulp-sourcemaps");
        var minify = require("gulp-minify-css");
        var pkg = require("./package.json");

        return gulp.src(assetPaths.style)
                   .pipe(sourcemaps.init())
                   .pipe(sass())
                   .pipe(sourcemaps.write())
                   .pipe(header(banner, { pkg: pkg }))
                   .pipe(gulp.dest(publishPaths.customerSetting))
                   .pipe(minify())
                   .pipe(rename({ suffix: ".min" }))
                   .pipe(gulp.dest(publishPaths.customerSetting));
    });

    gulp.task("publish-script", function (callback) {
        var concat = require("gulp-concat");
        var fs = require("fs");
        var gulpif = require("gulp-if");
        var gulpignore = require("gulp-ignore");
        var header = require("gulp-header");
        var rename = require("gulp-rename");
        var sourcemaps = require("gulp-sourcemaps");
        var templateCache = require("gulp-angular-templatecache");
        var ts = require("gulp-typescript");
        var uglify = require("gulp-uglify");
        var pkg = require("./package.json");
        var modules = [];
        var moduleName = "";

        fs.readdirSync(assetPaths.app).filter(function (item) {
            if (fs.statSync(assetPaths.app + item).isDirectory()) {
                modules.push(item.toLowerCase());
            }
        });

        for (var index in modules) {
            moduleName = modules[index];

            gulp.src(assetPaths.app + moduleName + "/**")
                .pipe(gulpif(/[.]ts$/, ts({
                    typescript: require("typescript"),
                    target: "ES5",
                    noImplicitAny: true,
                    out: moduleName + ".js",
                    removeComments: true
                })))
                .pipe(gulpif(/[.]htm$/, templateCache({
                    module: "customerSetting"
                })))
                .pipe(gulpignore.exclude(/[.]tt$/))
                .pipe(concat(moduleName + ".js"))
                .pipe(header(banner, { pkg: pkg }))
                .pipe(gulp.dest(publishPaths.customerSetting))
                .pipe(uglify())
                .pipe(rename({ suffix: ".min" }))
                .pipe(sourcemaps.write("."))
                .pipe(gulp.dest(publishPaths.customerSetting));
        }

        callback.call();
    });

    gulp.task("prebuild", function (done) {
        var runSequence = require("run-sequence");

        runSequence(
            ["clean", "install-tsd", "install-bower"],
            "install-compass",
            ["publish-template", "publish-styles", "publish-script"],
            done);
    });
})(require);