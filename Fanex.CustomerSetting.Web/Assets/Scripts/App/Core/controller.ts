/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../resource/resource-translation.ts" />
/// <reference path="common.ts" />
/// <reference path="helper-services.ts" />
/// <reference path="helpers.ts" />
/// <reference path="models.ts" />
/// <reference path="module-registrater.ts" />
/// <reference path="parameters.ts" />

module CustomerSetting {
    "use strict";

    export class CustomerSettingController {
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
        public requestMsgResult: Result | UpdatingSettingResult;

        private get queryPath(): string {
            return "viewCustomerSetting/querySetting?";
        }

        private get updatePath(): string {
            return "updateCustomerSetting/updateSetting";
        }

        private get updateMultiplePath(): string {
            return "updateCustomerSetting/updateMultipleSetting";
        }

        private $http: ng.IHttpService;
        private $timeout: ng.ITimeoutService;
        private appConfigService: AppConfigService;
        private parametersService: ParametersService;
        private resourceService: ResourceService;

        constructor(
            $http: ng.IHttpService,
            $timeout: ng.ITimeoutService,
            appConfigService: AppConfigService,
            parametersService: ParametersService,
            resourceService: ResourceService) {
            this.$http = $http;
            this.$timeout = $timeout;
            this.appConfigService = appConfigService;
            this.parametersService = parametersService;
            this.resourceService = resourceService;

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

                        this.$http
                            .post(this.appConfigService.resolveRootUrl(path), command)
                            .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                                this.requestMsgResult = new UpdatingSettingResult(response.data);

                                if (this.requestMsgResult.isSuccessful) {
                                    this.querySetting();
                                }

                                this.$timeout(() => {
                                    if ((<UpdatingSettingResult>this.requestMsgResult).removeSuccessResults().length === 0) {
                                        this.requestMsgResult = undefined;
                                    }
                                }, Defaults.displayingMsgTime);
                            }, () => {
                                var error: Result = Result.systemError;

                                error.message = this.resourceService.getMessage(error.message);
                                this.requestMsgResult = error;
                            })
                            .finally(() => {
                                this.isUIblocked = false;
                            });
                    } else {
                        this.requestMsgResult = validationResult;
                    }
                }
            } else {
                var error: Result = Result.noChangeToUpdate;

                error.message = this.resourceService.getMessage(error.message);
                this.requestMsgResult = error;
            }
        }

        private querySetting(): void {
            var query: SettingQuery = this.parametersService.getSettingQuery();

            if (query) {
                this.isUIblocked = true;

                this.$http
                    .post(this.appConfigService.resolveRootUrl(this.queryPath), query.toJson())
                    ////.get(this.appConfigService.resolveRootUrl("app/sample-data-1.json"))
                    .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                        this.setting = new Setting(response.data);
                    }, () => {
                        var error: Result = Result.systemError;

                        error.message = this.resourceService.getMessage(error.message);
                        this.requestMsgResult = error;
                    })
                    .finally(() => {
                        this.isUIblocked = false;
                    });
            }
        }
    }

    ModuleRegister.controller("customerSettingController", CustomerSettingController);
}