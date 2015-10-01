/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="bet-setting-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class SettingType2Directive extends SettingTypeDirective {
        public static $inject: string[] = ["betSettingResourceService"];

        public templateUrl: string = "settingtype2.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.headerTemplateUrl = "settingtype2-header.htm";
            scope.valueTemplateUrl = "settingtype2-value.htm";
            scope.settingTypeName = this.betSettingResourceService.getLabel("BetSetting");
        };

        private betSettingResourceService: BetSettingResourceService;

        constructor(betSettingResourceService: BetSettingResourceService) {
            super((settingItem: any, backup?: boolean) => new SettingType2Item(settingItem, backup));

            this.betSettingResourceService = betSettingResourceService;
        }
    }

    ModuleRegister.directive("settingtype2", SettingType2Directive);
}