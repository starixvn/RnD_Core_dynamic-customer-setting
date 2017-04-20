/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="settingtype14-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class SettingType14Directive extends SettingTypeDirective {
        public static $inject: string[] = ["settingType14ResourceService"];

        public templateUrl: string = "settingtype14.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.headerTemplateUrl = "settingtype14-header.htm";
            scope.valueTemplateUrl = "settingtype14-value.htm";
            scope.settingTypeName = this.settingType14ResourceService.getLabel("TicketCommission");

            if (scope.customerLevel === Level.member && scope.settingType.displayMode === DisplayMode.editable) {
                scope.settingType.displayMode = DisplayMode.readOnly;
            }
        };

        private settingType14ResourceService: SettingType14ResourceService;

        constructor(settingType14ResourceService: SettingType14ResourceService) {
            super((settingItem: any, backup?: boolean, customerLevel?: Level) => new SettingType14Item(settingItem, backup, customerLevel));

            this.settingType14ResourceService = settingType14ResourceService;
        }
    }

    ModuleRegister.directive("settingtype14", SettingType14Directive);
}