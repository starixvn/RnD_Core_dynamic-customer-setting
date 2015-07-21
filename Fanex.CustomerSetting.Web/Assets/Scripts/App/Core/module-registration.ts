/// <reference path="../../../typings/angularjs/angular.d.ts" />

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
}