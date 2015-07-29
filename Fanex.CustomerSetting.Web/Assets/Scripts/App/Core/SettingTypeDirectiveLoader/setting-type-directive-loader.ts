/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/oclazyload/oclazyload.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../helpers.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    interface SettingTypeLoaderScope extends ng.IScope {
        setting: Setting;
        update: (settingTypeId: { settingTypeId: number }) => void;
    }

    class SettingTypeDirectiveLoader implements ng.IDirective {
        public static $inject: string[] = ["$compile", "$ocLazyLoad", "appConfigService"];

        public replace: boolean = true;
        public restrict: string = "A";

        public get scope(): any {
            return {
                setting: "=",
                update: "&"
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
                    var settingTypeName = "settingtype" + settingType.id + "-template" + settingType.settingTemplate;
                    var filePath = "app/customerSetting/" + settingTypeName.replace("-", ".") + "/" + settingTypeName + ".js";
                    var newNodeParts = [
                        "<div data-oc-lazy-load=\"'",
                        this.appConfigService.resolveRootUrl(filePath),
                        "'\">",
                        "<",
                        settingTypeName,
                        " data-product-id=\"setting.productId\"",
                        " data-product-name=\"setting.productName\"",
                        " data-setting-type=\"setting.settingTypes[",
                        index,
                        "]\"",
                        "data-update=\"update({ settingTypeId: settingTypeId})\"",
                        "></",
                        settingTypeName,
                        "></div>"
                    ];
                    var settingTypeElement = angular.element(newNodeParts.join(""));

                    element.append(settingTypeElement);
                    this.$compile(settingTypeElement)(scope);
                });
            }
        }
    }

    ModuleRegister.directive("settingTypeLoader", SettingTypeDirectiveLoader);
}