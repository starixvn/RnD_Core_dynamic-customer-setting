/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="settingtype9-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    class SettingType9Directive extends SettingTypeDirective {
        public static $inject: string[] = ["settingType9ResourceService"];

        public templateUrl: string = "settingtype9.htm";

        public link: ng.IDirectiveLinkFn = (scope: IDefaultSettingTypeDirectiveScope) => {
            scope.headerTemplateUrl = "settingtype9-header.htm";
            scope.valueTemplateUrl = "settingtype9-value.htm";
            scope.settingTypeName = this.settingType9ResourceService.getLabel("BetSetting");
            scope.togglableSettingBlocks = true;
        };

        private settingType9ResourceService: SettingType9ResourceService;

        constructor(settingType9ResourceService: SettingType9ResourceService) {
            super((settingItem: any, backup?: boolean) => new SettingType9Item(settingItem, backup));

            this.settingType9ResourceService = settingType9ResourceService;
        }
    }

    ModuleRegister.directive("settingtype9", SettingType9Directive);
}