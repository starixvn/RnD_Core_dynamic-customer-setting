/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="settingtype5-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class SettingType5Directive extends SettingTypeDirective {
        public static $inject: string[] = ["settingType5ResourceService"];

        public templateUrl: string = "settingtype5.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.headerTemplateUrl = "settingtype5-header.htm";
            scope.valueTemplateUrl = "settingtype5-value.htm";
            scope.settingTypeName = this.settingType5ResourceService.getLabel("ChargeCommission");
        };

        private settingType5ResourceService: SettingType5ResourceService;

        constructor(settingType5ResourceService: SettingType5ResourceService) {
            super((settingItem: any, backup?: boolean) => new SettingType5Item(settingItem, backup));

            this.settingType5ResourceService = settingType5ResourceService;
        }
    }

    ModuleRegister.directive("settingtype5", SettingType5Directive);
}