/// <reference path="../typings/node/node.d.ts" />
/// <reference path="../typings/gulp-util/gulp-util.d.ts" />
/// <reference path="../typings/through2/through2.d.ts" />
/// <reference path="js-string-escape.d.ts" />
/// <reference path="minimize.d.ts" />
"use strict";
var fs = require("fs");
var gulpUtil = require("gulp-util");
var path = require("path");
var through = require("through2");
var jsStringEscape = require("js-string-escape");
var Minimize = require("minimize");
function embedTemplate(options) {
    var pluginName = "embeded-template";
    var templateUrlRegexp = new RegExp("['\"]?templateUrl['\"]?[\\s]*[:=][\\s]*['\"]([^'\"]+)['\"]", "g");
    var exitCode = 1;
    var successCode = 0;
    var errorCode = -1;
    var templateBegin = new Buffer("template = \"");
    var templateEnd = new Buffer("\"");
    var defaultOptions = {
        minimizeOptions: {
            empty: true,
            quotes: true
        },
        skipErrors: false,
        jsEncoding: "utf-8",
        templateEncoding: "utf-8"
    };
    var minimizer;
    var content;
    options = options || defaultOptions;
    options.minimizeOptions = options.minimizeOptions || defaultOptions.minimizeOptions;
    options.skipErrors = options.skipErrors || defaultOptions.skipErrors;
    options.jsEncoding = options.templateEncoding || defaultOptions.jsEncoding;
    options.templateEncoding = options.templateEncoding || defaultOptions.templateEncoding;
    minimizer = new Minimize(options.minimizeOptions);
    function replace(dirPath, callback) {
        var matches = templateUrlRegexp.exec(content);
        var templatePath;
        var templateRelativePath;
        if (matches === null) {
            callback(exitCode);
            return;
        }
        templatePath = matches[1];
        templateRelativePath = path.join(dirPath, templatePath);
        fs.readFile(templateRelativePath, options.templateEncoding, function (error, templateConent) {
            if (error) {
                callback(errorCode, "Can't read template file: \"" + templateRelativePath + "\". Error details: " + error);
                return;
            }
            minimizer.parse(templateConent, function (error, minifiedContent) {
                if (error) {
                    callback(errorCode, "Error while minifying template: \"" + templateRelativePath + "\". Error from \"minimize\" module: " + error);
                    return;
                }
                callback(successCode, {
                    regexpMatches: matches,
                    template: minifiedContent
                });
            });
        });
    }
    function join(parts) {
        var templateParts = new Array();
        var index = 0;
        parts.forEach(function (part) {
            templateParts.push(new Buffer(content.substring(index, part.regexpMatches.index)));
            templateParts.push(templateBegin);
            templateParts.push(new Buffer(jsStringEscape(part.template)));
            templateParts.push(templateEnd);
            index = part.regexpMatches.index + part.regexpMatches[0].length;
        });
        templateParts.push(new Buffer(content.substring(index)));
        return Buffer.concat(templateParts);
    }
    function transform(chunk, enc, callback) {
        var pipe = this;
        var base = path.dirname(chunk.path);
        var parts = new Array();
        if (chunk.isNull()) {
            callback(null, chunk);
            return;
        }
        if (chunk.isStream()) {
            throw new gulpUtil.PluginError(pluginName, "Streaming not supported. Particular file: " + chunk.path);
        }
        content = chunk.contents.toString(options.jsEncoding);
        replace(base, replaceCallback);
        function replaceCallback(code, data) {
            if (successCode === code) {
                parts.push(data);
                replace(base, replaceCallback);
            }
            else if (errorCode === code) {
                var msg = data;
                if (options.skipErrors) {
                    gulpUtil.log(pluginName, "[WARN]", gulpUtil.colors.magenta(msg));
                    replace(base, replaceCallback);
                }
                else {
                    pipe.emit("error", new gulpUtil.PluginError(pluginName, msg));
                }
            }
            else if (exitCode === code) {
                if (parts.length > 0) {
                    chunk.contents = join(parts);
                }
                callback(null, chunk);
            }
        }
    }
    return through.obj(transform);
}
module.exports = embedTemplate;
