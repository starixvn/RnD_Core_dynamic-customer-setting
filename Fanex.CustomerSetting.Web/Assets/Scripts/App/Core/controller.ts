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
            "$timeout",
            "appConfigService",
            "parametersService",
            "resourceService"];

        public isUIblocked: boolean;
        public customerNames: string;
        public uplineLevel: Level;
        public setting: Setting;
        public requestMsgResult: Result | Results;

        constructor(
            private $http: ng.IHttpService,
            private $timeout: ng.ITimeoutService,
            private appConfigService: AppConfigService,
            private parametersService: ParametersService,
            private resourceService: ResourceService) {
            this.resourceService.setLanguage(this.parametersService.language);
            this.customerNames = this.parametersService.customerNames;
            this.uplineLevel = this.parametersService.uplineLevel;
            this.querySetting();
        }

        public update(settingTypeId?: number): void {
            var path: string = this.customerNames ? this.updateMultiplePath : this.updatePath;
            var setting: Setting = this.setting.getChanges(settingTypeId);
            var validationResult: Result;
            var command: SettingCommand;

            this.requestMsgResult = undefined;

            if (setting) {
                if (setting.validate()) {
                    command = this.parametersService.createSettingCommand(setting);
                    validationResult = command.validate();

                    if (validationResult.isSuccessful) {
                        this.isUIblocked = true;
                        this.$http.post(this.appConfigService.resolveRootUrl(path), command).success((msg: any) => {
                            this.requestMsgResult = new Results(msg.code, msg.message, msg.msgType, msg.subResults);

                            if (this.requestMsgResult.isSuccessful) {
                                this.querySetting();
                            }

                            this.$timeout(() => {
                                if ((<Results>this.requestMsgResult).removeSuccessResults().length === 0) {
                                    this.requestMsgResult = undefined;
                                }
                            }, Defaults.displayingMsgTime);
                        }).error(() => {
                            var error: Result = Result.systemError;

                            error.message = this.resourceService.getMessage(error.message);
                            this.requestMsgResult = error;
                        }).finally(() => {
                            this.isUIblocked = false;
                        });
                    } else {
                        this.requestMsgResult = validationResult;
                    }
                }
            } else {
                var error: Result = Results.noChangeToUpdate;

                error.message = this.resourceService.getMessage(error.message);
                this.requestMsgResult = error;
            }
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