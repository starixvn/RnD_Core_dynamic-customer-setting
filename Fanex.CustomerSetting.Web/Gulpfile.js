/// <vs BeforeBuild='before-build' AfterBuild='publish' />
/*global require*/
(function (require) {
    'use strict';

    var gulp = require('gulp'),

        paths = {
            bower: "./bower_components/",
            publish: "./Publish/",
            libraries: "./Assets/Libraries/",
            app: "./Assets/Scripts/App/",
            style: "./Assets/Styles/"
        },

        newVersion;

    gulp.task('bump-version', function () {
        var prompt = require('gulp-prompt'),
            semver = require('semver'),
            pkg = require('./package.json');

        gulp.src('')
            .pipe(prompt.prompt({
                type: 'checkbox',
                name: 'increase version',
                message: 'What type of version bump would you like to do ? (current version is ' + pkg.version + ')',
                choices: [
                    'patch (' + pkg.version + ' --> ' + semver.inc(pkg.version, 'patch') + ')',
                    'minor (' + pkg.version + ' --> ' + semver.inc(pkg.version, 'minor') + ')',
                    'major (' + pkg.version + ' --> ' + semver.inc(pkg.version, 'major') + ')',
                    'none (exit)'
                ]
            }), function (result) {
                if (result.bump.match(/^patch/)) {
                    newVersion = semver.inc(pkg.version, 'patch');
                } else if (result.bump.match(/^minor/)) {
                    newVersion = semver.inc(pkg.version, 'minor');
                } else if (result.bump.match(/^major/)) {
                    newVersion = semver.inc(pkg.version, 'major');
                }
            });
    });

    gulp.task('update-version', function () {
        var jsonEditor = require('gulp-json-editor');

        gulp.src('./package.json')
            .pipe(jsonEditor({
                version: newVersion
            }))
            .pipe(gulp.dest('./'));

        gulp.src('./bower.json')
            .pipe(jsonEditor({
                version: newVersion
            }))
            .pipe(gulp.dest('./'));
    });

    gulp.task('install-tsd', function (callback) {
        var tsd = require('gulp-tsd');

        tsd({
            command: 'reinstall',
            latest: true,
            config: 'tsd.json'
        }, callback);
    });

    gulp.task('install-bower', function () {
        var bower = require('gulp-bower');

        bower();
    });

    gulp.task('install-compass', function () {
        gulp.src(paths.bower + 'compass-mixins/lib/**')
                .pipe(gulp.dest(paths.style + 'Compass'));
    });

    gulp.task('install-libs', ['install-bower', 'install-compass'], function () {
        var bower = {
            "AngularJs": "angularjs/angular*.{js,map}",
            "JQuery": "jquery/dist/jquery*.{js,map}",
            "OcLazyLoad": "oclazyload/dist/oclazyload.min.js"
        };

        for (var destinationDir in bower) {
            gulp.src(paths.bower + bower[destinationDir])
                .pipe(gulp.dest(paths.libraries + destinationDir));
        }
    });

    gulp.task('clean', function () {
        var clean = require('gulp-clean');

        gulp.src(paths.publish + '**/*.*', { read: false })
            .pipe(clean());
    });

    gulp.task('publish-lib', function () {
        gulp.src(paths.libraries + '**')
            .pipe(gulp.dest(paths.publish + 'Libraries'));
    });

    gulp.task('publish-style', function () {
        var header = require('gulp-header'),
            banner = [
                '/**',
                ' * <%= pkg.name %> - <%= pkg.description %>',
                ' * @version v' + (newVersion ? newVersion : '<%= pkg.version %>'),
                ' * @author <%= pkg.author %>',
                ' * @license <%= pkg.license %>',
                ' */',
                ''].join('\n'),

                pkg = require('./package.json');

        gulp.src(paths.style + 'Images/**')
            .pipe(gulp.dest(paths.publish + 'Styles/Images'));

        gulp.src(paths.style + '*.min.css')
            .pipe(header(banner, { pkg: pkg }))
            .pipe(gulp.dest(paths.publish + 'Styles'));
    });

    gulp.task('publish-script', function () {
        var concat = require('gulp-concat'),
            fs = require('fs'),
            gulpif = require('gulp-if'),
            uglify = require('gulp-uglify'),
            templateCache = require('gulp-angular-templatecache'),
            ts = require('gulp-typescript'),

            modules = [],
            moduleName = '';

        fs.readdirSync(paths.app).filter(function (item) {
            if (fs.statSync(paths.app + item).isDirectory()) {
                modules.push(item.toLowerCase());
            }
        });

        gulp.src(paths.app + '*.htm')
            .pipe(gulp.dest(paths.publish + 'App/'));

        for (var index in modules) {
            moduleName = modules[index];

            gulp.src(paths.app + moduleName + '/**')
                .pipe(gulpif(/[.]ts$/, ts({
                    noImplicitAny: true,
                    out: moduleName + '.js'
                })))
                .pipe(gulpif(/[.]htm$/, templateCache({
                    module: 'fanex-customer-setting'
                })))
                .pipe(concat(moduleName + '.js'))
                .pipe(gulp.dest(paths.publish + 'App/'))
                .pipe(uglify())
                .pipe(gulp.dest(paths.publish + 'App/' + moduleName + '.min.js'));
        }
    });

    gulp.task('publish', function (done) {
        var runSequence = require('run-sequence');

        runSequence(
            'clean',
            'publish-style',
            'publish-lib',
            'publish-script',
            done);
    });

    gulp.task('before-build', ['install-tsd', 'install-libs']);
}(require));