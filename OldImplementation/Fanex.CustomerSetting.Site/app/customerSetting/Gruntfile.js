/*jshint -W101*/
'use strict';

module.exports = function (grunt) {
    var jsFiles = ['customer-setting-app.js', 'customer-setting-templates.js', '*/*.js'];
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
        uglify: {
            options: {
                banner: banner
            },
            dist: {
                src: jsFiles,
                dest: 'customer-setting.min.js'
            }
        },
        concat: {
            options: {
                separator: '\n\n',
                banner: banner
            },
            dist: {
                src: jsFiles,
                dest: 'customer-setting.js'
            }
        },
        less: {
            development: {
                options: {
                    banner: banner
                },
                files: {
                    'customer-setting.css': 'customer-setting.less'
                }
            },
            production: {
                options: {
                    plugins: [
                        new (require('less-plugin-clean-css'))({
                            compatibility: 'ie8',
                            advanced: true
                        })
                    ],
                    banner: banner,
                    compress: true
                },
                files: {
                    'customer-setting.min.css': 'customer-setting.less'
                }
            }
        },
        ngtemplates: {
            production: {
                src: 'templates/**/*.htm',
                dest: 'customer-setting-templates.js',
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
                            "angular.module('customerSettingApp').run(['$templateCache', function($templateCache) {",
                            script,
                            "} ]);\r\n",
                            "/*jshint -W101*/"
                        ].join("\r\n");
                    }
                }
            }
        },
        replace: {
            production: {
                src: 'customer-setting-page-template.htm',
                dest: 'customer-setting-page.htm',
                replacements: [{
                    from: '{{version}}',
                    to: '<%= pkg.version %>'
                }]
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-angular-templates');
    grunt.loadNpmTasks('grunt-text-replace');

    grunt.registerTask('default', ['ngtemplates', 'jshint', 'uglify', 'concat', 'less', 'page']); // development & production.
    grunt.registerTask('verify', ['ngtemplates', 'jshint']); // verify codes before build.
    grunt.registerTask('css', ['less:development']); // development - css only.
    grunt.registerTask('js', ['concat']); // development - js only.
    grunt.registerTask('htm', ['ngtemplates']); // transform htm to js.
    grunt.registerTask('page', ['replace']); // transform page.
};