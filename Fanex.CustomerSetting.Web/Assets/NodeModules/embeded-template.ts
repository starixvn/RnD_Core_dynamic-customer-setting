/// <reference path="../typings/node/node.d.ts" />
/// <reference path="../typings/gulp-util/gulp-util.d.ts" />
/// <reference path="../typings/through2/through2.d.ts" />
/// <reference path="js-string-escape.d.ts" />
/// <reference path="minimize.d.ts" />

"use strict";

import fs = require("fs");
import gulpUtil = require("gulp-util");
import path = require("path");
import through = require("through2");
import jsStringEscape = require("js-string-escape");
import Minimize = require("minimize");

interface IEmbededTemplateOptions {
    minimizeOptions: IOptions;
    skipErrors: boolean;
    jsEncoding: string;
    templateEncoding: string
}

interface ITemplatePart {
    regexpMatches: RegExpExecArray;
    template: string;
}

export function embedTemplate(options: IEmbededTemplateOptions): NodeJS.ReadWriteStream {
    const pluginName: string = "embed-template";
    const templateUrlRegexp: RegExp = new RegExp("['\"]?templateUrl['\"]?[\\s]*[:=][\\s]*['\"]([^'\"]+)['\"]", "g");

    const exitCode: number = 1;
    const successCode: number = 0;
    const errorCode: number = -1;

    const templateBegin: Buffer = new Buffer("template = \"");
    const templateEnd: Buffer = new Buffer("\"");

    var defaultOptions = {
        minimizeOptions: {},
        skipErrors: false,
        jsEncoding: "utf-8",
        templateEncoding: "utf-8"
    };
    var minimizer: Minimize;
    var content: string;

    options = options || defaultOptions;
    options.minimizeOptions = options.minimizeOptions || defaultOptions.minimizeOptions;
    options.skipErrors = options.skipErrors || defaultOptions.skipErrors;
    options.jsEncoding = options.templateEncoding || defaultOptions.jsEncoding;
    options.templateEncoding = options.templateEncoding || defaultOptions.templateEncoding;

    minimizer = new Minimize(options.minimizeOptions);

    function replace(dirPath: string, callback: Function): void {
        var matches: RegExpExecArray = templateUrlRegexp.exec(content);
        var templatePath: string;
        var templateRelativePath: string;

        if (matches === null) {
            callback(exitCode);

            return;
        }

        templatePath = matches[1];
        templateRelativePath = path.join(dirPath, templatePath);

        fs.readFile(templateRelativePath, options.templateEncoding, (error: NodeJS.ErrnoException, templateConent: string) => {
            if (error) {
                callback(errorCode, "Can't read template file: \"" + templateRelativePath + "\". Error details: " + error);

                return;
            }

            minimizer.parse(templateConent, (error: Error, minifiedContent: string) => {
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

    function join(parts: ITemplatePart[]) {
        var templateParts: Array<Buffer> = new Array<Buffer>();
        var index: number = 0;

        parts.forEach((part: ITemplatePart) => {
            templateParts.push(new Buffer(content.substring(index, part.regexpMatches.index)));
            templateParts.push(templateBegin);
            templateParts.push(new Buffer(jsStringEscape(part.template)));
            templateParts.push(templateEnd);

            index = part.regexpMatches.index + part.regexpMatches[0].length;
        });

        templateParts.push(new Buffer(content.substring(index)));

        return Buffer.concat(templateParts);
    }

    function transform(chunk: any, enc: string, callback: Function): void {
        var pipe: any = this;
        var base: string = path.dirname(chunk.path);
        var parts: Array<ITemplatePart> = new Array<ITemplatePart>();

        if (chunk.isNull()) {
            callback(null, chunk);

            return;
        }

        if (chunk.isStream()) {
            throw new gulpUtil.PluginError(pluginName, "Streaming not supported. particular file: " + chunk.path);
        }

        content = chunk.contents.toString(options.jsEncoding);
        replace(base, replaceCallback);

        function replaceCallback(code: number, data: ITemplatePart | string): void {
            if (successCode === code) {
                parts.push(<ITemplatePart>data);
                replace(base, replaceCallback);
            } else if (errorCode === code) {
                var msg: string = <string>data;

                if (options.skipErrors) {
                    gulpUtil.log(pluginName, "[WARN]", gulpUtil.colors.magenta(msg));
                    replace(base, replaceCallback);
                } else {
                    pipe.emit("error", new gulpUtil.PluginError(pluginName, msg));
                }
            } else if (exitCode === code) {
                if (parts.length > 0) {
                    chunk.contents = join(parts);
                }

                callback(null, chunk);
            }
        }
    }

    return through.obj(transform);
}