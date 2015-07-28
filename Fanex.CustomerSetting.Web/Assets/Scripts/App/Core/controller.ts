/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../resource/resource-translation.ts" />
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
            "parametersService",
            "resourceService"];

        public isUIblocked: boolean;
        public customerNames: string;
        public setting: Setting;
        public requestMsgResult: Result;

        constructor(
            private $http: ng.IHttpService,
            private appConfigService: AppConfigService,
            private parametersService: ParametersService,
            private resourceService: ResourceService) {
            this.resourceService.setLanguage(this.parametersService.language);
            this.customerNames = this.parametersService.customerNames;
            this.querySetting();
        }

        public update(settingTypeId?: number): void {
            console.log(settingTypeId);
        }

        private querySetting(): void {
            var query = this.parametersService.getSettingQuery();

            this.isUIblocked = true;
            this.$http.get(this.appConfigService.resolveRootUrl("viewCustomerSetting/querySetting?" + query.toParamString())).success((data: any) => {
                //this.$http.get(this.appConfigService.resolveRootUrl("app/sample-data.json")).success((data: Setting) => {
                //console.log(JSON.stringify(data));
                this.setting = new Setting(data);
                console.log(this.setting);
                this.isUIblocked = false;
            }).error((error: Result) => {
                var error = Result.systemError;

                error.message = this.resourceService.getMessage(error.message);

                this.requestMsgResult = error;
                this.isUIblocked = false;
            });
        }
    }

    ModuleRegister.controller("customerSettingController", CustomerSettingController);
}