/// <vs BeforeBuild='tsd:refresh' />
module.exports = function (grunt) {
    var pgk = grunt.file.readJSON('package.json');

    grunt.initConfig({
        tsd: {
            refresh: {
                options: {
                    command: 'reinstall',
                    latest: true,
                    config: 'tsd.json'
                }
            }
        }
    });

    grunt.loadNpmTasks('grunt-tsd');
};