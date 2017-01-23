/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype13-resources.ts" />

module CustomerSetting {
    "use strict";

    class SettingType13Directive extends SettingTypeDirective {
        public static $inject: string[] = ["settingType13ResourceService", "parametersService"];

        public templateUrl: string = "settingtype13.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtype13-value.htm";
            scope.settingTypeName = this.settingType13ResourceService.getLabel("ProductStatus");
        };

        private settingType13ResourceService: SettingType13ResourceService;

        constructor(settingType13ResourceService: SettingType13ResourceService, parametersService: ParametersService) {
            super((settingItem: any, backup?: boolean, customerLevel?: Level) => new SettingType13Item(settingItem, backup, customerLevel, parametersService.systemId));

            this.settingType13ResourceService = settingType13ResourceService;
        }
    }

    ModuleRegister.directive("settingtype13", SettingType13Directive);
}