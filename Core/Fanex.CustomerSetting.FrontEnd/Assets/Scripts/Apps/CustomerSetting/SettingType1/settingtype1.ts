/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="position-taking-resources.ts" />

module CustomerSetting {
    "use strict";

    class SettingType1Directive extends SettingTypeDirective {
        public static $inject: string[] = ["positionTakingResourceService"];

        public templateUrl: string = "settingtype1.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.headerTemplateUrl = "settingtype1-header.htm";
            scope.valueTemplateUrl = "settingtype1-value.htm";
            scope.settingTypeName = this.positionTakingResourceService.getLabel("PositionTaking");
        };

        private positionTakingResourceService: PositionTakingResourceService;

        constructor(positionTakingResourceService: PositionTakingResourceService) {
            super(SettingType1Item.factory);

            this.positionTakingResourceService = positionTakingResourceService;
        }
    }

    ModuleRegister.directive("settingtype1", SettingType1Directive);
}