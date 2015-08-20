/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="helper-services.ts" />
/// <reference path="models.ts" />
/// <reference path="module-register.ts" />

module CustomerSettingMatrix {
    "use strict";

    class CustomerSettingMaxtrixController {
        public static $inject: string[] = [
            "$rootScope",
            "$http",
            "$timeout",
            "appConfigService"];

        public isUIblocked: boolean;
        public customerNames: string;
        public uplineLevel: Level;

        private get queryPath(): string {
            return "ViewCustomerSettingMatrix/QuerySettingMatrix?";
        }

        private $rootScope: ng.IRootScopeService;
        private $http: ng.IHttpService;
        private $timeout: ng.ITimeoutService;
        private appConfigService: AppConfigService;

        constructor(
            $rootScope: ng.IRootScopeService,
            $http: ng.IHttpService,
            $timeout: ng.ITimeoutService,
            appConfigService: AppConfigService) {
            this.$rootScope = $rootScope;
            this.$http = $http;
            this.$timeout = $timeout;
            this.appConfigService = appConfigService;

            this.querySettingMatrix();
        }

        private querySettingMatrix(): void {
            var queryParams: any;

            if (queryParams) {
                this.isUIblocked = true;

                this.$http
                    .post(this.appConfigService.resolveRootUrl(this.queryPath), queryParams.toJson())
                ////.get(this.appConfigService.resolveRootUrl("app/sample-data-4.json"))
                    .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                    var settingJsonObject = JSON.parse(response.data);
                    
                    })
                    .finally(() => {
                    this.isUIblocked = false;
                });
            }
        }
    }

    ModuleRegister.controller("customerSettingMatrixController", CustomerSettingMaxtrixController);
}