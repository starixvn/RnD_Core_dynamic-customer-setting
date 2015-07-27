/// <reference path="../../../typings/angularjs/angular.d.ts" />

module CustomerSetting {
    var app: ng.IModule = angular.module("customerSetting", []);

    export class ModuleRegister {
        public static controller(name: string, target: Function) {
            app.controller(name, target);
        }

        public static service(name: string, target: Function) {
            app.service(name, target);
        }

        public static factory(name: string, target: Function) {
            app.factory(name, target);
        }

        public static filter(name: string, target: Function) {
            app.filter(name, target);
        }

        public static directive(name: string, target: any) {
            var args: any[] = [];

            args.push.apply(target.$inject || []);
            app.directive(name, () => new target(args));
        }

        public static value(name: string, target: Object) {
            app.value(name, target);
        }
    }
}