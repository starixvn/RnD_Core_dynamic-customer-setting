/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/oclazyload/oclazyload.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../helpers.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    interface SettingTypeLoaderScope extends ng.IScope {
        setting: Setting;
    }

    class SettingTypeDirectiveLoader implements ng.IDirective {
        public static $inject = ["$compile", "$ocLazyLoad", "appConfigService"];

        public replace: boolean = true;
        public restrict: string = "A";

        public get scope(): any {
            return {
                setting: "="
            }
        }

        public link: ng.IDirectiveLinkFn = (scope: SettingTypeLoaderScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            scope.$watch("setting", () => {
                this.generateNodes(scope, element);
            });
        }

        constructor(
            private $compile: ng.ICompileService,
            private $ocLazyLoad: oc.ILazyLoad,
            private appConfigService: AppConfigService) { }

        private generateNodes(scope: SettingTypeLoaderScope, element: ng.IAugmentedJQuery): void {
            if (angular.isDefined(scope.setting)) {
                angular.forEach(scope.setting.settingTypes, (settingType, index) => {
                    var scriptUrl = "app/customerSetting/settingType" + settingType.id + ".template" + settingType.settingTemplate + ".js";
                    this.$ocLazyLoad.load(this.appConfigService.resolveRootUrl(scriptUrl)).then(() => {
                        var settingTypeEleName = "settingtype" + settingType.id + "-template" + settingType.settingTemplate;
                        var settingTypeEleParts = [
                            "<",
                            settingTypeEleName,
                            " data-productId=\"setting.productId\"",
                            " data-productName=\"setting.productName\"",
                            " data-settingType=\"setting.settingTypes[",
                            index,
                            "]\"></",
                            settingTypeEleName,
                            ">"
                        ];
                        var settingTypeElement = angular.element(settingTypeEleParts.join(""));

                        element.append(settingTypeElement);
                        this.$compile(settingTypeElement)(scope);
                    });
                });
            }
        }
    }

    ModuleRegister.directive("settingTypeLoader", SettingTypeDirectiveLoader);
}