/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="settingtype7-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class SettingType7Directive extends SettingTypeDirective {
        public static $inject: string[] = ["settingType7ResourceService"];

        public templateUrl: string = "settingtype7.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtype7-value.htm";
            scope.settingTypeName = this.settingType7ResourceService.getLabel("ChargedCommission");
        };

        private settingType7ResourceService: SettingType7ResourceService;

        constructor(settingType7ResourceService: SettingType7ResourceService) {
            super((settingItem: any, backup?: boolean) => new SettingType7Item(settingItem, backup));

            this.settingType7ResourceService = settingType7ResourceService;
        }
    }

    ModuleRegister.directive("settingtype7", SettingType7Directive);
}