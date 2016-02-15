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
            "blockUiService",
            "updatingSettingTypePromiseChain"];

        public customerNames: string;
        public uplineLevel: Level;
        public setting: Setting;
        public warningMessages: Result | UpdatingSettingResult;
        public requestMsgResult: Result | UpdatingSettingResult;

        private $window: ng.IWindowService;
        private $rootScope: ng.IRootScopeService;
        private $http: ng.IHttpService;
        private $timeout: ng.ITimeoutService;
        private appConfigService: AppConfigService;
        private parametersService: ParametersService;
        private resourceService: ResourceService;
        private blockUiService: BlockUiService;
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
            blockUiService: BlockUiService,
            updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain) {
            this.$window = $window;
            this.$rootScope = $rootScope;
            this.$http = $http;
            this.$timeout = $timeout;
            this.appConfigService = appConfigService;
            this.parametersService = parametersService;
            this.resourceService = resourceService;
            this.blockUiService = blockUiService;
            this.updatingSettingTypePromiseChain = updatingSettingTypePromiseChain;
            this.successfulUpdatingCallback = this.$window.parent["successfulUpdatingCallback"];

            this.customerNames = this.parametersService.getCustomerNames();
            this.uplineLevel = this.parametersService.getUplineLevel();

            angular.element(this.$window.document).ready(() => {
                this.querySetting();
            });
        }

        public update(settingTypeId?: number): void {
            var isMultiple: boolean = !!this.customerNames;
            var path: string = isMultiple ? this.appConfigService.getUpdateMultiplePath() : this.appConfigService.getUpdatePath();
            var editableSettings: Setting = this.setting.getEditableSettingTypes(settingTypeId);
            var setting: Setting = isMultiple || !this.setting.isNormalMode ? editableSettings : this.setting.getChanges(settingTypeId);
            var validationResult: Result;
            var command: SettingCommand;

            this.$rootScope.$emit("updateSetting");
            this.requestMsgResult = undefined;

            if (setting) {
                if (editableSettings.validate()) {
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
            this.blockUiService.blockUi();

            this.$http
                .post(path, command.toJson())
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
                this.blockUiService.unblockUi();
            });
        }

        private querySetting(): void {
            var query: SettingQuery = this.parametersService.getSettingQuery();

            if (query) {
                this.blockUiService.blockUi();

                this.$http
                    .post(this.appConfigService.getQueryPath(), query.toJson())
                    .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                        this.setting = new Setting(response.data);
                        this.warningMessages = this.setting.warningMessage;
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
                    this.blockUiService.unblockUi();
                });
            }
        }
    }

    ModuleRegister.controller("customerSettingController", CustomerSettingController);
}