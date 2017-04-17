/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype15-resources.ts" />

module CustomerSetting {
    "use strict";

    class SettingType15Directive extends SettingTypeDirective {
        public static $inject: string[] = ["settingType15ResourceService"];

        public templateUrl: string = "settingtype15.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtype15-value.htm";
            scope.settingTypeName = this.settingType15ResourceService.getLabel("WinningShareCommission");
        };

        private settingType15ResourceService: SettingType15ResourceService;

        constructor(settingType15ResourceService: SettingType15ResourceService) {
            super((settingItem: any, backup?: boolean, customerLevel?: Level) => new SettingType15Item(settingItem, backup));

            this.settingType15ResourceService = settingType15ResourceService;
        }
    }

    ModuleRegister.directive("settingtype15", SettingType15Directive);
}