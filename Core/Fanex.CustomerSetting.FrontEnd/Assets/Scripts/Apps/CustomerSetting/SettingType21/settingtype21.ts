/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype21-resources.ts" />


module CustomerSetting {
    "use strict";

    class SettingType21Directive extends SettingTypeDirective {
        public static $inject: string[] = ["settingType21ResourceService", "parametersService", "$rootScope"];

        public templateUrl: string = "settingtype21.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtype21-value.htm";
            scope.settingTypeName = this.settingType21ResourceService.getLabel("ProductStatus");
            
            const parentGame = scope.settingType.settingItems.filter(e => e["parentBetTypeId"] === 0 && e["parentSportTypeId"] === 0)[0];
            const childGames = scope.settingType.settingItems.filter(e => e["parentBetTypeId"] === parentGame["betTypeId"] && e["parentSportTypeId"] === parentGame["sportTypeId"]);
            if (parentGame["isDisabled"]) {
                childGames.forEach(e => {
                    e["isDisabled"] = true;
                    e["readonly"] = true;
                })
            }
        };

        private settingType21ResourceService: SettingType21ResourceService;

        constructor(settingType21ResourceService: SettingType21ResourceService, parametersService: ParametersService, $rootScope: ng.IRootScopeService) {
            super((settingItem: any, backup?: boolean, customerLevel?: Level) => new SettingType21Item(settingItem, backup, customerLevel, parametersService.systemId, $rootScope));
            this.settingType21ResourceService = settingType21ResourceService;
        }
        
    }

    ModuleRegister.directive("settingtype21", SettingType21Directive);
}