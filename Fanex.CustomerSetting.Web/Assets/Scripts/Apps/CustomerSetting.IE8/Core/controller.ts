/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../resource/resource-translation.ts" />
/// <reference path="common.ts" />
/// <reference path="helper-services.ts" />
/// <reference path="helpers.ts" />
/// <reference path="models.ts" />
/// <reference path="module-registrater.ts" />
/// <reference path="parameters.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class CustomerSettingController {
        public static $inject: string[] = [
            "$rootScope",
            "$http",
            "$timeout",
            "appConfigService",
            "parametersService",
            "resourceService",
            "updatingSettingTypePromiseChain"];

        public isUIblocked: boolean;
        public customerNames: string;
        public uplineLevel: Level;
        public setting: Setting;
        public requestMsgResult: Result | UpdatingSettingResult;

        private queryPath: string = "viewCustomerSetting/querySetting?";
        private updatePath: string = "updateCustomerSetting/updateSetting";
        private updateMultiplePath: string = "updateCustomerSetting/updateMultipleSetting";

        private $rootScope: ng.IRootScopeService;
        private $http: ng.IHttpService;
        private $timeout: ng.ITimeoutService;
        private appConfigService: AppConfigService;
        private parametersService: ParametersService;
        private resourceService: ResourceService;
        private updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain;

        constructor(
            $rootScope: ng.IRootScopeService,
            $http: ng.IHttpService,
            $timeout: ng.ITimeoutService,
            appConfigService: AppConfigService,
            parametersService: ParametersService,
            resourceService: ResourceService,
            updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain) {
            this.$rootScope = $rootScope;
            this.$http = $http;
            this.$timeout = $timeout;
            this.appConfigService = appConfigService;
            this.parametersService = parametersService;
            this.resourceService = resourceService;
            this.updatingSettingTypePromiseChain = updatingSettingTypePromiseChain;

            this.customerNames = this.parametersService.getCustomerNames();
            this.uplineLevel = this.parametersService.getUplineLevel();
            this.querySetting();
        }

        public update(settingTypeId?: number): void {
            var path: string = this.customerNames ? this.updateMultiplePath : this.updatePath;
            var setting: Setting = this.setting.getChanges(settingTypeId);
            var validationResult: Result;
            var command: SettingCommand;

            this.$rootScope.$emit("updateSetting");
            this.requestMsgResult = undefined;

            if (setting) {
                if (setting.validate()) {
                    command = this.parametersService.createSettingCommand(setting);
                    validationResult = command.validate();

                    if (validationResult.isSuccessful()) {
                        this.updatingSettingTypePromiseChain.execute(command).then(() => {
                            this.updateSetting(path, command);
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

        private updateSetting(path: string, command: SettingCommand): void {
            this.isUIblocked = true;

            this.$http
                .post(this.appConfigService.resolveRootUrl(path), command.toJson())
                .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                    this.requestMsgResult = new UpdatingSettingResult(response.data);
                    this.querySetting();

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
                ["finally"](() => {
                    this.isUIblocked = false;
                });
        }

        private querySetting(): void {
            var query: SettingQuery = this.parametersService.getSettingQuery();

            if (query) {
                this.isUIblocked = true;

                this.$http
                    .post(this.appConfigService.resolveRootUrl(this.queryPath), query.toJson())
                    ////.get(this.appConfigService.resolveRootUrl("app/sample-data-4.json"))
                    .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                        this.setting = new Setting(response.data);
                    }, () => {
                        var error: Result = Result.systemError;

                        error.message = this.resourceService.getMessage(error.message);
                        this.requestMsgResult = error;
                    })
                    ["finally"](() => {
                        this.isUIblocked = false;
                    });
            }
        }
    }

    ModuleRegister.controller("customerSettingController", CustomerSettingController);
}