/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="module-register.ts" />

module CustomerSettingMatrix {
    "use strict";

    export class AppConfigService {
        public static $inject: string[] = ["$window"];

        private rootUrl: string;
        private debug: boolean;
        private version: string;

        private $window: ng.IWindowService;

        constructor($window: ng.IWindowService) {
            var appConfig: any;

            this.$window = $window;

            appConfig = this.$window["appConfig"] || {};

            if (appConfig) {
                this.rootUrl = appConfig.rootUrl;
                this.debug = appConfig.isDebug;
                this.version = appConfig.scriptVersion;
            }
        }

        public resolveRootUrl(url: string): string {
            return this.rootUrl + url;
        }

        public debugMode(): boolean {
            return this.debug;
        }

        public scriptVersion(): string {
            return this.version;
        }
    }

    ModuleRegister.service("appConfigService", AppConfigService);
}