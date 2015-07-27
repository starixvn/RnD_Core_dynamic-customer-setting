/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="common.ts" />
/// <reference path="models.ts" />
/// <reference path="module-registrater.ts" />
/// <reference path="parameters.ts" />
/// <reference path="helpers.ts" />

module CustomerSetting {
    export class CustomerSettingController {
        private isMultipleUpdates: boolean;

        private get queryPath(): string {
            return "viewCustomerSetting/querySetting";
        }

        private get updatePath(): string {
            return "updateCustomerSetting/updateSetting";
        }

        private get updateMultiplePath(): string {
            return "updateCustomerSetting/updateMultipleSetting";
        }

        public static $inject = [
            "$http",
            "appConfigService",
            "parametersService"];

        public isUIblocked: boolean;
        public setting: Setting;

        constructor(
            private $http: ng.IHttpService,
            private appConfigService: AppConfigService,
            private parametersService: ParametersService) {
            this.querySetting();
        }

        private querySetting(): void {
            var query = this.parametersService.getSettingQuery();

            this.isUIblocked = true;
            console.log(query);
        }
    }

    ModuleRegister.controller("customerSettingController", CustomerSettingController);
}