(function (require) {
    "use strict";

    var gulp = require("gulp");
    var assetPaths = {
        bower: "./bower_components/",
        app: "./Assets/Scripts/App/",
        settingMatrixApp: "./Assets/Scripts/SettingMatrixApp/",
        style: "./Assets/Styles/",
        settingMatrixAppStyle: "./Assets/Styles/",
        image: "./Assets/Styles/Images/",
        compass: "./Assets/Styles/Compass/"
    };
    var publishPaths = {
        root: "./Asset/Publish/",
        customerSetting: "./Assets/Publish/App/CustomerSetting/",
        customerSettingMatrix: "./Assets/Publish/App/CustomerSettingMatrix/",
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

    gulp.task("install-bower", function () {
        var bower = require("gulp-bower");

        return bower();
    });

    gulp.task("install-compass", function () {
        return gulp.src(assetPaths.bower + "compass-mixins/lib/**")
                   .pipe(gulp.dest(assetPaths.compass));
    });

    gulp.task("publish-template", function () {
        var minifyHTML = require("gulp-minify-html");

        return gulp.src(assetPaths.app + "*.htm")
                   .pipe(minifyHTML({ empty: true }))
                   .pipe(gulp.dest(publishPaths.customerSetting));
    });

    gulp.task("publish-matrix-template", function () {
        var minifyHTML = require("gulp-minify-html");

        return gulp.src([
                            assetPaths.settingMatrixApp + "*.htm",
                            assetPaths.settingMatrixApp + "/**/*.htm"
                        ])
                   .pipe(minifyHTML({ empty: true }))
                   .pipe(gulp.dest(publishPaths.customerSettingMatrix));
    });

    // When using gulp-sass to compile scss, it will use libSasss which will make the compass' functions in wrong results.
    // We can fix it by gulp-ruby-sass, which uses Ruby as runtime compiler but it needs the Ruby gem be installed on the building machine.
    gulp.task("publish-styles2", function () {
        var rename = require("gulp-rename");
        var sass = require("gulp-sass");
        var sourcemaps = require("gulp-sourcemaps");
        var minify = require("gulp-minify-css");

        return gulp.src(assetPaths.style + "**")
                   .pipe(sourcemaps.init())
                   .pipe(sass())
                   .pipe(sourcemaps.write())
                   .pipe(gulp.dest(publishPaths.customerSetting))
                   .pipe(minify())
                   .pipe(rename({ suffix: ".min" }))
                   .pipe(gulp.dest(publishPaths.customerSetting));
    });

    gulp.task("publish-images", function () {
        return gulp.src(assetPaths.image + "*.*")
                   .pipe(gulp.dest(publishPaths.customerSetting + "Styles/Images"));
    });

    gulp.task("publish-styles", ["publish-images"], function () {

        return gulp.src(assetPaths.style + "customer-setting*.{map,css}")
                   .pipe(gulp.dest(publishPaths.customerSetting + "Styles"));
    });

    gulp.task("publish-core-script", function () {
        var concat = require("gulp-concat");
        var gulpif = require("gulp-if");
        var minifyHTML = require("gulp-minify-html");
        var rename = require("gulp-rename");
        var sourcemaps = require("gulp-sourcemaps");
        var templateCache = require("gulp-angular-templatecache");
        var ts = require("gulp-typescript");
        var uglify = require("gulp-uglify");

        return gulp.src(assetPaths.app + "Core/**")
            .pipe(gulpif(/[.]ts$/, ts({
                typescript: require("typescript"),
                target: "ES5",
                noImplicitAny: false,
                removeComments: true
            })))
            .pipe(gulpif(/[.]htm$/, minifyHTML({ empty: true })))
            .pipe(gulpif(/[.]htm$/, templateCache({
                module: "customerSetting"
            })))
            .pipe(concat("core.js"))
            .pipe(gulp.dest(publishPaths.customerSetting))
            .pipe(uglify())
            .pipe(rename({ suffix: ".min" }))
            .pipe(gulp.dest(publishPaths.customerSetting))
            .pipe(sourcemaps.write("."));
    });

    gulp.task("publish-modules-script", function (callback) {
        var concat = require("gulp-concat");
        var fs = require("fs");
        var gulpif = require("gulp-if");
        var minifyHTML = require("gulp-minify-html");
        var rename = require("gulp-rename");
        var sourcemaps = require("gulp-sourcemaps");
        var ts = require("gulp-typescript");
        var uglify = require("gulp-uglify");
        var modules = [];
        var moduleName = "";

        fs.readdirSync(assetPaths.app).filter(function (item) {
            if (fs.statSync(assetPaths.app + item).isDirectory() && item !== "Core") {
                modules.push(item.toLowerCase());
            }
        });

        for (var index in modules) {
            moduleName = modules[index];

            gulp.src(assetPaths.app + moduleName + "/*.{js,ts}")
                .pipe(gulpif(/[.]ts$/, ts({
                    typescript: require("typescript"),
                    target: "ES5",
                    noImplicitAny: false,
                    removeComments: true
                })))
                .pipe(concat(moduleName.replace(".", "-") + ".js"))
                .pipe(sourcemaps.write("."))
                .pipe(gulp.dest(publishPaths.customerSetting + moduleName))
                .pipe(uglify())
                .pipe(rename({ suffix: ".min" }))
                .pipe(gulp.dest(publishPaths.customerSetting + moduleName));

            gulp.src(assetPaths.app + moduleName + "/*.htm")
                .pipe(minifyHTML({ empty: true }))
                .pipe(gulp.dest(publishPaths.customerSetting + moduleName));
        }

        callback.call();
    });

    gulp.task("publish-matrix-script", function () {
        var concat = require("gulp-concat");
        var gulpif = require("gulp-if");
        var minifyHTML = require("gulp-minify-html");
        var rename = require("gulp-rename");
        var sourcemaps = require("gulp-sourcemaps");
        var templateCache = require("gulp-angular-templatecache");
        var ts = require("gulp-typescript");
        var uglify = require("gulp-uglify");

        gulp.src(
            [
                assetPaths.settingMatrixApp + "**"
            ])
            .pipe(gulpif(/[.]ts$/, ts({
                typescript: require("typescript"),
                target: "ES5",
                noImplicitAny: false,
                removeComments: true
            })))
            .pipe(gulpif(/[.]htm$/, minifyHTML({ empty: true })))
            .pipe(gulpif(/[.]htm$/, templateCache({
                module: "customerSettingMatrix"
            })))
            .pipe(concat("customer-setting-matrix.js"))
            .pipe(sourcemaps.write("."))
            .pipe(gulp.dest(publishPaths.customerSettingMatrix))
            .pipe(uglify())
            .pipe(rename({ suffix: ".min" }))
            .pipe(gulp.dest(publishPaths.customerSettingMatrix));
    });

    gulp.task("publish-script", ["publish-core-script", "publish-modules-script"]);

    gulp.task("publish-matrix", ["publish-matrix-template", "publish-matrix-script", "publish-matrix-styles"]);

    gulp.task("publish-matrix-styles", function () {
        gulp.src(assetPaths.image + "*.*")
                   .pipe(gulp.dest(publishPaths.customerSettingMatrix + "Styles/Images"));

        return gulp.src(assetPaths.settingMatrixAppStyle + "customer-setting-matrix*.{map,css}")
                   .pipe(gulp.dest(publishPaths.customerSettingMatrix + "Styles"));
    });

    gulp.task("prebuild", function (done) {
        var runSequence = require("run-sequence");

        runSequence(
            ["clean", "install-tsd", "install-bower"],
            "install-compass",
            ["publish-template", "publish-styles", "publish-script"],
            "publish-matrix",
            done);
    });

    gulp.task("watch", ["publish-script", "publish-template", "publish-styles", "publish-matrix"], function () {
        gulp.watch(assetPaths.app + "**/*.{ts,htm}", ["publish-script"]);
        gulp.watch(assetPaths.app + "*.htm", ["publish-template"]);
        gulp.watch(assetPaths.style + "customer-setting*.{map,css}", ["publish-styles"]);

        gulp.watch(assetPaths.settingMatrixApp + "**/*.{ts,htm}", ["publish-matrix"]);
        gulp.watch(assetPaths.settingMatrixApp + "*.htm", ["publish-matrix"]);
        gulp.watch(assetPaths.settingMatrixAppStyle + "customer-setting-matrix*.scss", ["publish-matrix"]);
    });

    gulp.task("tslint", function () {
        var tslint = require("gulp-tslint");

        return gulp.src([assetPaths.app + "Core/**/*.ts", assetPaths.app + "**/*.ts"])
                   .pipe(tslint({
                       configuration: "tslint.json"
                   }))
                   .pipe(tslint.report("prose"));
    });
})(require);