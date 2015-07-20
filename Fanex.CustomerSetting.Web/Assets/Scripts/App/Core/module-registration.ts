/// <reference path="../../../typings/tsd.d.ts" />

module CustomerSetting {
    var app: ng.IModule = ng.module("customerSetting", ["ng", "oc.lazyLoad"]);

    export function controller(target: any): any {
        app.controller(target.$componentName, target);
    }

    export function service(target: any): any {
        app.service(target.$componentName, target);
    }

    export function provider(target: any): any {
        app.provider(target.$componentName, target);
    }

    export function directive(target: any): any {
        app.directive(target.$componentName, target);
    }

    export function value(target: any): any {
        app.value(target.$componentName, target);
    }

    export function filter(target: any) {
        app.filter(target.$componentName, target);
    }

    function getComponentArgs(component: Function): string[] {
        var argNames: string[] = [];

        if (component !== undefined) {
            var componentString = component.toString().replace(/((\/\/.*$)|(\/\*[\s\S]*?\*\/))/mg, "");
            var results = componentString.slice(componentString.indexOf('(') + 1, componentString.indexOf(')')).match(/([^\s,]+)/g);

            argNames = results;
        }

        return argNames;
    }

    function getComponentName(component: Function): string {
        var componentName = "";

        if (component !== undefined) {
            var results = (/function (.{1,})\(/).exec(component.toString());

            if (results !== undefined && results.length > 1) {
                componentName = results[1];
                componentName = componentName[0].toLowerCase() + componentName.substring(1);
            }
        }

        return componentName;
    }
}