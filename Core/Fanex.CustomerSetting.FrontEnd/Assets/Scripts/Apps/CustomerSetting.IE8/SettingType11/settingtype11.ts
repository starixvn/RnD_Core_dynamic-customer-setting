/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype11-resources.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class SettingType11Directive extends SettingTypeDirective {
        public static $inject: string[] = ["settingType11ResourceService", "parametersService"];

        public templateUrl: string = "settingtype11.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtype11-value.htm";
            scope.settingTypeName = this.settingType11ResourceService.getLabel("ProductStatus");
        };

        private settingType11ResourceService: SettingType11ResourceService;

        constructor(settingType11ResourceService: SettingType11ResourceService, parametersService: ParametersService) {
            super((settingItem: any, backup?: boolean, customerLevel?: Level) => new SettingType11Item(settingItem, backup, customerLevel, parametersService.getSystemId()));

            this.settingType11ResourceService = settingType11ResourceService;
        }
    }

    ModuleRegister.directive("settingtype11", SettingType11Directive);
}