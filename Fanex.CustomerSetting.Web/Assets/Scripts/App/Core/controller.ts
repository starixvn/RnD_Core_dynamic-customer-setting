/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../resource/resource-translation.ts" />
/// <reference path="common.ts" />
/// <reference path="helpers.ts" />
/// <reference path="models.ts" />
/// <reference path="module-registrater.ts" />
/// <reference path="parameters.ts" />

module CustomerSetting {
    export class CustomerSettingController {
        private isMultipleUpdates: boolean;

        private get queryPath(): string {
            return "viewCustomerSetting/querySetting?";
        }

        private get updatePath(): string {
            return "updateCustomerSetting/updateSetting";
        }

        private get updateMultiplePath(): string {
            return "updateCustomerSetting/updateMultipleSetting";
        }

        public static $inject: string[] = [
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
            var setting = settingTypeId ? this.setting.getSettingType(settingTypeId) : this.setting;
            var path = this.customerNames ? this.updateMultiplePath : this.updatePath;

            this.isUIblocked = true;
            this.$http.post(this.appConfigService.resolveRootUrl(path), this.parametersService.createSettingCommand(setting)).success((msg: SettingResult) => {
                this.requestMsgResult = msg;
            }).error(() => {
                var error: Result = Result.systemError;

                error.message = this.resourceService.getMessage(error.message);

                this.requestMsgResult = error;
            }).finally(() => {
                this.isUIblocked = false;
            });
        }

        private querySetting(): void {
            var query: SettingQuery = this.parametersService.getSettingQuery();

            this.isUIblocked = true;
            this.$http.get(this.appConfigService.resolveRootUrl(this.queryPath + query.toParamString())).success((data: any) => {
                //this.$http.get(this.appConfigService.resolveRootUrl("app/sample-data.json")).success((data: any) => {
                this.setting = new Setting(data);
            }).error(() => {
                var error: Result = Result.systemError;

                error.message = this.resourceService.getMessage(error.message);

                this.requestMsgResult = error;
            }).finally(() => {
                this.isUIblocked = false;
            });
        }
    }

    ModuleRegister.controller("customerSettingController", CustomerSettingController);
}