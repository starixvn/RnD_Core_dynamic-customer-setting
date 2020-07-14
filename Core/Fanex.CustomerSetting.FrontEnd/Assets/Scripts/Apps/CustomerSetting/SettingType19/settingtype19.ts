/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="commission-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    class SettingType19Directive extends SettingTypeDirective {
        public static $inject: string[] = ["commissionResourceService"];

        public templateUrl: string = "settingtype19.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            if (scope.customerLevel === Level.member && scope.uplineLevel !== Level.agent) {
                scope.headerTemplateUrl = "settingtype19-header.htm";
            }

            scope.valueTemplateUrl = "settingtype19-value.htm";
            scope.settingTypeName = this.commissionResourceService.getLabel("Commission");
        };

        private commissionResourceService: CommissionResourceService;

        constructor(commissionResourceService: CommissionResourceService) {
            super((settingItem: any, backup?: boolean) => new SettingType19Item(settingItem, backup));

            this.commissionResourceService = commissionResourceService;
        }
    }

    ModuleRegister.directive("settingtype19", SettingType19Directive);
}