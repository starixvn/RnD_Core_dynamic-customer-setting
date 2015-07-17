/// <reference path="../../../typings/tsd.d.ts" />

module CustomerSetting {
    var app: ng.IModule = ng.module("customerSetting", ["ng", "oc.lazyLoad"]);

    export function controller(target: Function): any {
        var name: string = getComponentName(target);
        var dependencies: any[] = getComponentArgs(target);

        dependencies.push(target);
        app.controller(name, dependencies);
    }

    export function service(target: Function): any {
        var name: string = getComponentName(target);
        var dependencies: any[] = getComponentArgs(target);

        dependencies.push(target);
        app.service(name, dependencies);
    }

    export function provider(target: Function): any {
        var name: string = getComponentName(target);
        var dependencies: any[] = getComponentArgs(target);

        dependencies.push(target);
        app.provider(name, dependencies);
    }

    export function directive(target: Function): any {
        var name: string = getComponentName(target);
        var dependencies: any[] = getComponentArgs(target);

        dependencies.push(target);
        app.directive(name, dependencies);
    }

    export function value(target: Function): any {
        var name: string = getComponentName(target);

        app.value(name, target);
    }

    export function filter(target: Function) {
        var name: string = getComponentName(target);
        var dependencies: any[] = getComponentArgs(target);

        dependencies.push(target);
        app.filter(name, dependencies);
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