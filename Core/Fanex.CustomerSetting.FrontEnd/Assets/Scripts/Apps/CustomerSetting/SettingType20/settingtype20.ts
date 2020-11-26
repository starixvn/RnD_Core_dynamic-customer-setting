/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype20-resources.ts" />

module CustomerSetting {
    "use strict";

    class SettingType20Directive extends SettingTypeDirective {
        public static $inject: string[] = ["settingType20ResourceService", "parametersService"];

        public templateUrl: string = "settingtype20.htm";

        public link: ng.IDirectiveLinkFn = (scope: IDefaultSettingTypeDirectiveScope) => {
            scope.headerTemplateUrl = "settingtype20-header.htm";
            scope.valueTemplateUrl = "settingtype20-value.htm";
            scope.settingTypeName = this.settingType20ResourceService.getLabel("BetSetting");
            scope.togglableSettingBlocks = true;
        };

        private settingType20ResourceService: SettingType20ResourceService;

        constructor(settingType20ResourceService: SettingType20ResourceService, parametersService: ParametersService) {
            super((settingItem: any, backup?: boolean) => new SettingType20Item(settingItem, backup, parametersService.systemId));

            this.settingType20ResourceService = settingType20ResourceService;
        }
    }

    ModuleRegister.directive("settingtype20", SettingType20Directive);
}