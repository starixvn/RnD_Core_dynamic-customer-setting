/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="module-register.ts" />

module CustomerSettingMatrix {
    "use strict";

    export class AppConfigService {
        public static $inject: string[] = ["$window"];

        private applicationPath: string;
        private antiForgeryToken: string;

        private $window: ng.IWindowService;

        constructor($window: ng.IWindowService) {
            var appConfig: any;

            this.$window = $window;
            /* tslint:disable:no-string-literal */
            appConfig = this.$window["appConfig"] || {};
            /* tslint:enable:no-string-literal */
            if (appConfig) {
                this.applicationPath = appConfig.applicationPath;
                this.antiForgeryToken = appConfig.antiForgeryToken;
            }
        }

        public resolveRootUrl(url: string): string {
            return this.applicationPath + url;
        }

        public requestVerificationToken(): string {
            return this.antiForgeryToken;
        }
    }

    ModuleRegister.service("appConfigService", AppConfigService);

    function PTValue() {
        return (numberValue: number) => {
            if (numberValue != null) {
                return (numberValue / 100).toFixed(2);
            }

            return "";
        };
    }

    ModuleRegister.filter("ptValue", PTValue);
}