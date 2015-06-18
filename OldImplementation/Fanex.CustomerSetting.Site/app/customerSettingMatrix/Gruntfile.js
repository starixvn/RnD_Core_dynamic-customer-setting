/*jshint -W101*/
'use strict';

module.exports = function (grunt) {
    var jsFiles = ['customer-setting-matrix-app.js', 'customer-setting-matrix-templates.js', '*/*.js'];
    var pkg = grunt.file.readJSON('package.json');
    var banner = '/*! <%= pkg.name %> v<%= pkg.version %> | (c) <%= grunt.template.today("yyyy") %> <%= pkg.license %> */\n\n';
    grunt.initConfig({
        pkg: pkg,
        jshint: {
            files: jsFiles,
            options: {
                laxbreak: true,
                globalstrict: true,
                globals: {
                    module: true,
                    angular: true,
                    document: true,
                    console: true,
                    window: true
                }
            }
        },
        concat: {
            options: {
                separator: '\n\n',
                banner: banner
            },
            dist: {
                src: jsFiles,
                dest: 'customer-setting-matrix.js'
            }
        },
        less: {
            development: {
                options: {
                    banner: banner
                },
                files: {
                    'customer-setting-matrix.css': 'customer-setting-matrix.less'
                }
            }
        },
        ngtemplates: {
            production: {
                src: 'templates/**/*.htm',
                dest: 'customer-setting-matrix-templates.js',
                options: {
                    htmlmin: {
                        collapseBooleanAttributes: true,
                        collapseWhitespace: true,
                        removeAttributeQuotes: true,
                        removeComments: true, // Only if you don't use comment directives!
                        removeEmptyAttributes: true,
                        removeRedundantAttributes: true,
                        removeScriptTypeAttributes: true,
                        removeStyleLinkTypeAttributes: true
                    },
                    bootstrap: function (module, script) {
                        return [
                            "/*jshint -W101*/\r\n",
                            "angular.module('customerSettingMatrixApp').run(['$templateCache', function($templateCache) {",
                            script,
                            "} ]);\r\n",
                            "/*jshint -W101*/"
                        ].join("\r\n");
                    }
                }
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-angular-templates');

    grunt.registerTask('default', ['ngtemplates', 'jshint', 'concat', 'less']); // development & production.
    grunt.registerTask('css', ['less:development']); // development - css only.
    grunt.registerTask('js', ['concat']); // development - js only.
    grunt.registerTask('htm', ['ngtemplates']); // transform htm to js.
};