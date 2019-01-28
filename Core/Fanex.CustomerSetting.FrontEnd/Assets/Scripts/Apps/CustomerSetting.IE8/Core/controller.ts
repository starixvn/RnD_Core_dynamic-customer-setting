/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../resource/resource-translation.ts" />
/// <reference path="common.ts" />
/// <reference path="helper-services.ts" />
/// <reference path="models.ts" />
/// <reference path="module-registrater.ts" />
/// <reference path="parameters.ts" />
/// <reference path="requesting-server-service.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ISuccessfulUpdatingCallback {
        (isPartialUpdate: boolean, isMultipleUpdate: boolean): any;
    }

    class CustomerSettingController {
        public static $inject: string[] = [
            "$window",
            "$rootScope",
            "$timeout",
            "appConfigService",
            "parametersService",
            "resourceService",
            "requestService",
            "updatingSettingTypePromiseChain",
            "settingTypesSharedValuesService"];

        public systemId: number;
        public customerNames: string;
        public setting: Setting;
        public warningMessages: Result | UpdatingSettingResult;
        public requestMsgResult: Result | UpdatingSettingResult;

        private $window: ng.IWindowService;
        private $rootScope: ng.IRootScopeService;
        private $timeout: ng.ITimeoutService;
        private appConfigService: AppConfigService;
        private parametersService: ParametersService;
        private resourceService: ResourceService;
        private requestService: RequestService;
        private updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain;
        private settingTypesSharedValuesService: SettingTypesSharedValuesService;
        private successfulUpdatingCallback: ISuccessfulUpdatingCallback;

        constructor(
            $window: ng.IWindowService,
            $rootScope: ng.IRootScopeService,
            $timeout: ng.ITimeoutService,
            appConfigService: AppConfigService,
            parametersService: ParametersService,
            resourceService: ResourceService,
            requestService: RequestService,
            updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain,
            settingTypesSharedValuesService: SettingTypesSharedValuesService) {
            this.$window = $window;
            this.$rootScope = $rootScope;
            this.$timeout = $timeout;
            this.appConfigService = appConfigService;
            this.parametersService = parametersService;
            this.resourceService = resourceService;
            this.requestService = requestService;
            this.updatingSettingTypePromiseChain = updatingSettingTypePromiseChain;
            this.settingTypesSharedValuesService = settingTypesSharedValuesService;
            this.successfulUpdatingCallback = this.$window.parent["successfulUpdatingCallback"];

            this.systemId = this.parametersService.getSystemId();
            this.customerNames = this.parametersService.getCustomerNames();
            this.requestService.registerSharedScope(this);

            angular.element(this.$window.document).ready(() => {
                this.querySetting();
            });
        }

        public update(settingTypeId?: number): void {
            var isMultiple: boolean = !!this.customerNames;
            var path: string = isMultiple ? this.appConfigService.getUpdateMultiplePath() : this.appConfigService.getUpdatePath();
            var editableSettings: Setting = this.setting.getEditableSettingTypes(settingTypeId);
            var setting: Setting = isMultiple || !this.settingTypesSharedValuesService.normalMode ? editableSettings : this.setting.getChanges(settingTypeId);
            var validationResult: Result;
            var command: SettingCommand;

            this.$rootScope.$emit("updateSetting", settingTypeId);
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
            this.requestService.request(path, command.toJson(), (response: ng.IHttpPromiseCallbackArg<any>) => {
                this.requestMsgResult = new UpdatingSettingResult(response.data);
                this.querySetting();

                this.$timeout(() => {
                    var settingResults = (<UpdatingSettingResult>this.requestMsgResult);

                    if (settingResults && settingResults.removeSuccessResults && settingResults.removeSuccessResults().length === 0) {
                        this.requestMsgResult = undefined;

                        if (this.successfulUpdatingCallback) {
                            this.successfulUpdatingCallback(isPartialUpdate, isMultipleUpdate);
                        }
                    }
                }, Defaults.displayingMsgTime);
            });
        }

        private querySetting(): void {
            var query: SettingQuery = this.parametersService.getSettingQuery();

            if (query) {
                this.requestService.request(this.appConfigService.getQueryPath(), query.toJson(), (response: ng.IHttpPromiseCallbackArg<any>) => {
                    this.setting = new Setting(response.data);
                    this.warningMessages = this.setting.warningMessages;
                    this.settingTypesSharedValuesService.register(this.setting, this.parametersService.getUplineLevel());
                    this.$rootScope.$emit("queriedSetting");
                });
            }
        }
    }

    ModuleRegister.controller("customerSettingController", CustomerSettingController);
}