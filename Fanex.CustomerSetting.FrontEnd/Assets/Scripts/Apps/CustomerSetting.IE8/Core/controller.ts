/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../resource/resource-translation.ts" />
/// <reference path="common.ts" />
/// <reference path="helper-services.ts" />
/// <reference path="models.ts" />
/// <reference path="module-registrater.ts" />
/// <reference path="parameters.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ISuccessfulUpdatingCallback {
        (isPartialUpdate: boolean, isMultipleUpdate: boolean): any;
    }

    class CustomerSettingController {
        public static $inject: string[] = [
            "$window",
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

        private $window: ng.IWindowService;
        private $rootScope: ng.IRootScopeService;
        private $http: ng.IHttpService;
        private $timeout: ng.ITimeoutService;
        private appConfigService: AppConfigService;
        private parametersService: ParametersService;
        private resourceService: ResourceService;
        private updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain;
        private successfulUpdatingCallback: ISuccessfulUpdatingCallback;

        constructor(
            $window: ng.IWindowService,
            $rootScope: ng.IRootScopeService,
            $http: ng.IHttpService,
            $timeout: ng.ITimeoutService,
            appConfigService: AppConfigService,
            parametersService: ParametersService,
            resourceService: ResourceService,
            updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain) {
            this.$window = $window;
            this.$rootScope = $rootScope;
            this.$http = $http;
            this.$timeout = $timeout;
            this.appConfigService = appConfigService;
            this.parametersService = parametersService;
            this.resourceService = resourceService;
            this.updatingSettingTypePromiseChain = updatingSettingTypePromiseChain;
            this.successfulUpdatingCallback = this.$window.parent["successfulUpdatingCallback"];

            this.customerNames = this.parametersService.getCustomerNames();
            this.uplineLevel = this.parametersService.getUplineLevel();
            this.querySetting();
        }

        public update(settingTypeId?: number): void {
            var isMultiple: boolean = !!this.customerNames;
            var path: string = isMultiple ? this.updateMultiplePath : this.updatePath;
            var setting: Setting = isMultiple ? this.setting.getEditableSettingTypes(settingTypeId) : this.setting.getChanges(settingTypeId);
            var validationResult: Result;
            var command: SettingCommand;

            this.$rootScope.$emit("updateSetting");
            this.requestMsgResult = undefined;

            if (setting) {
                if (this.setting.validate()) {
                    command = this.parametersService.createSettingCommand(setting);
                    validationResult = command.validate();

                    if (validationResult.isSuccessful()) {
                        this.updatingSettingTypePromiseChain.execute(command).then(() => {
                            this.updateSetting(path, command, !!settingTypeId, isMultiple);
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

        private updateSetting(path: string, command: SettingCommand, isPartialUpdate?: boolean, isMultipleUpdate?: boolean): void {
            this.isUIblocked = true;

            this.$http
                .post(this.appConfigService.resolveRootUrl(path), command.toJson())
                .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                    this.requestMsgResult = new UpdatingSettingResult(response.data);
                    this.querySetting();

                    this.$timeout(() => {
                        if ((<UpdatingSettingResult>this.requestMsgResult).removeSuccessResults().length === 0) {
                            this.requestMsgResult = undefined;

                            if (this.successfulUpdatingCallback) {
                                this.successfulUpdatingCallback(isPartialUpdate, isMultipleUpdate);
                            }
                        }
                    }, Defaults.displayingMsgTime);
                }, (response: ng.IHttpPromiseCallbackArg<any>) => {
                    var error: Result;

                    if (response) {
                        if (response.status === 401) {
                            error = Result.notAuthorizedError;
                            error.message = this.resourceService.getMessage(error.message);
                        } else if (response.data) {
                            error = new Result(response.status, response.data.message, MessageType.error);
                        } else {
                            error = Result.systemError;
                            error.message = this.resourceService.getMessage(error.message);
                        }
                    }

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
                    .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                        this.setting = new Setting(response.data);
                    }, (response: ng.IHttpPromiseCallbackArg<any>) => {
                        var error: Result;

                        if (response) {
                            if (response.status === 401) {
                                error = Result.notAuthorizedError;
                                error.message = this.resourceService.getMessage(error.message);
                            } else if (response.data) {
                                error = new Result(response.status, response.data.message, MessageType.error);
                            } else {
                                error = Result.systemError;
                                error.message = this.resourceService.getMessage(error.message);
                            }
                        }

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