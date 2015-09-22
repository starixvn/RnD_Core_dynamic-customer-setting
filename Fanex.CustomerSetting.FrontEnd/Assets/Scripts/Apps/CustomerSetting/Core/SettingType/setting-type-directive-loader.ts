/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../helper-services.ts" />
/// <reference path="../helpers.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingTypeLoaderScope extends ng.IScope {
        uplineLevel: Level;
        setting: Setting;
        update: (settingTypeId: { settingTypeId: number }) => void;
    }

    class SettingTypeDirectiveLoader implements ng.IDirective {
        public static $inject: string[] = ["$compile", "appConfigService"];
        public replace: boolean = true;

        public get scope(): any {
            return {
                uplineLevel: "=",
                setting: "=",
                update: "&"
            };
        }

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeLoaderScope, element: ng.IAugmentedJQuery) => {
            scope.$watch("setting.settingTypes", () => {
                this.generateNodes(scope, element);
            });
        };

        private $compile: ng.ICompileService;
        private appConfigService: AppConfigService;
        private loadedSettingTypes: Array<number>;

        constructor($compile: ng.ICompileService, appConfigService: AppConfigService) {
            this.$compile = $compile;
            this.appConfigService = appConfigService;
            this.loadedSettingTypes = new Array<number>();
        }

        private generateNodes(scope: ISettingTypeLoaderScope, element: ng.IAugmentedJQuery): void {
            if (angular.isDefined(scope.setting) && angular.isDefined(scope.setting.settingTypes) && angular.isArray(scope.setting.settingTypes)) {
                scope.setting.settingTypes.forEach((settingType: SettingType, index: number) => {
                    var loaded = this.loadedSettingTypes.some((item: number) => {
                        return item === settingType.id;
                    });

                    if (!loaded) {
                        var settingTypeName: string = StringFormater.format("settingtype{0}", settingType.id);
                        var newNodeParts = [
                            "<",
                            settingTypeName,
                            " data-upline-level=\"uplineLevel\"",
                            " data-customer-level=\"setting.customerLevel\"",
                            " data-product-id=\"setting.productId\"",
                            " data-product-name=\"setting.productName\"",
                            " data-setting-type=\"setting.settingTypes[",
                            index,
                            "]\"",
                            "data-update=\"update({ settingTypeId: settingTypeId})\"",
                            "></",
                            settingTypeName,
                            ">"
                        ];
                        var settingTypeElement = angular.element(newNodeParts.join(""));

                        element.append(settingTypeElement);
                        this.$compile(settingTypeElement)(scope);
                        this.loadedSettingTypes.push(settingType.id);
                    }
                });
            }
        }
    }

    ModuleRegister.directive("settingTypeLoader", SettingTypeDirectiveLoader);
}