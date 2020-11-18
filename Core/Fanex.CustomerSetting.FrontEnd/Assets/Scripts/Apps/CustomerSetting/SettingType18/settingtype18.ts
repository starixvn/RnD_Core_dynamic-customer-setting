/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="position-taking-resources.ts" />

module CustomerSetting {
    "use strict";

    class SettingType18Directive extends SettingTypeDirective {
        public static $inject: string[] = ["parametersService", "positionTakingResourceService"];

        public templateUrl: string = "settingtype18.htm";

        public link: ng.IDirectiveLinkFn = (scope: IDefaultSettingTypeDirectiveScope) => {
            /* tslint:disable:no-string-literal */
            scope.isMultiple = this.parametersService.multipleUpdates;
            /* tslint:enable:no-string-literal */
            scope.headerTemplateUrl = "settingtype18-header.htm";
            scope.valueTemplateUrl = "settingtype18" + (scope.isMultiple ? "" : "-default") + "-value.htm";
            scope.settingTypeName = this.positionTakingResourceService.getLabel("PositionTaking");
        };

        private positionTakingResourceService: PositionTakingResourceService;
        private parametersService: ParametersService;

        constructor(parametersService: ParametersService, positionTakingResourceService: PositionTakingResourceService) {
            super(SettingType18Item.factory);

            this.positionTakingResourceService = positionTakingResourceService;
            this.parametersService = parametersService;
        }
    }

    ModuleRegister.directive("settingtype18", SettingType18Directive);
}