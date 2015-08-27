/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="helper-services.ts" />

module CustomerSettingMatrix {
    "use strict";

    var app: ng.IModule = angular.module("customerSettingMatrix", []);

    export class ModuleRegister {
        public static controller(name: string, target: Function) {
            app.controller(name, target);
        }

        public static service(name: string, target: Function) {
            app.service(name, target);
        }

        public static filter(name: string, target: Function) {
            app.filter(name, target);
        }

        public static directive(name: string, target: any) {
            var directiveFactory: ng.IDirectiveFactory = function () {
                var args = Array.prototype.concat.apply([null], arguments);

                return new (Function.prototype.bind.apply(target, args));
            };

            directiveFactory.$inject = target.$inject;
            app.directive(name, directiveFactory);
        }

    }

    app.run(["$http", "appConfigService", ($http: ng.IHttpService, appConfigService: AppConfigService) => {
        /* tslint:disable:no-string-literal */
        $http.defaults.headers.common["__RequestVerificationToken"] = appConfigService.requestVerificationToken();
        /* tslint:enable:no-string-literal */
    }]);
}