"use strict";

module.exports = function (config) {
    config.set({
        files: [],
        browsers: ["chrome"],
        logLevel: config.LOG_WARN,
        basePath: "../",
        port: 7676,
        frameworks: ["jasmine"],
        reporters: ["progress"],
        color: true
    });
};