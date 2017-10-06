/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="settingtype16-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class SettingType16Directive extends SettingTypeDirective {
        public static $inject: string[] = ["settingType16ResourceService"];

        public templateUrl: string = "settingtype16.htm";

        public link: ng.IDirectiveLinkFn = (scope: IDefaultSettingTypeDirectiveScope) => {
            scope.headerTemplateUrl = "settingtype16-header.htm";
            scope.valueTemplateUrl = "settingtype16-value.htm";
            scope.settingTypeName = this.settingType16ResourceService.getLabel("BetSetting");
        };

        private settingType16ResourceService: SettingType16ResourceService;

        constructor(settingType16ResourceService: SettingType16ResourceService) {
            super((settingItem: any, backup?: boolean) => new SettingType16Item(settingItem, backup));

            this.settingType16ResourceService = settingType16ResourceService;
        }
    }

    ModuleRegister.directive("settingtype16", SettingType16Directive);
}