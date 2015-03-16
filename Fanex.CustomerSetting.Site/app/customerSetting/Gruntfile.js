'use strict';

module.exports = function (grunt) {
    var jsFiles = ['customer-setting-app.js', '*/*.js'];
    var banner = '// <%= pkg.name %> - v<%= pkg.version %> - ' + '<%= grunt.template.today("yyyy-mm-dd HH:mm:ss") %>\n\n';
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        jshint: {
            files: jsFiles,
            options: {
                laxbreak: true,
                globalstrict: true,
                globals: {
                    module: true,
                    angular: true,
                    document: true,
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
                    compress: true
                },
                files: {
                    'customer-setting.min.css': 'customer-setting.less'
                }
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-less');

    grunt.registerTask('default', ['jshint', 'uglify', 'concat', 'less']);
    grunt.registerTask('js', ['jshint', 'uglify', 'concat']);
    grunt.registerTask('css', ['less']);
    grunt.registerTask('dev', ['concat']);
};