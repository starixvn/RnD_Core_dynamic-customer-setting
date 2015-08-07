/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/oclazyload/oclazyload.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../helper-services.ts" />
/// <reference path="../helpers.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    interface SettingTypeLoaderScope extends ng.IScope {
        uplineLevel: Level;
        setting: Setting;
        update: (settingTypeId: { settingTypeId: number }) => void;
    }

    class SettingTypeDirectiveLoader implements ng.IDirective {
        private loadedSettingTypes: Array<KeyValuePair<number, number>>;

        public static $inject: string[] = ["$compile", "$ocLazyLoad", "appConfigService"];
        public replace: boolean = true;

        public get scope(): any {
            return {
                uplineLevel: "=",
                setting: "=",
                update: "&"
            }
        }

        public link: ng.IDirectiveLinkFn = (scope: SettingTypeLoaderScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            scope.$watch("setting.settingTypes", () => {
                this.generateNodes(scope, element);
            });
        }

        constructor(
            private $compile: ng.ICompileService,
            private $ocLazyLoad: oc.ILazyLoad,
            private appConfigService: AppConfigService) {
            this.loadedSettingTypes = new Array<KeyValuePair<number, number>>();
        }

        private generateNodes(scope: SettingTypeLoaderScope, element: ng.IAugmentedJQuery): void {
            if (angular.isDefined(scope.setting)) {
                angular.forEach(scope.setting.settingTypes, (settingType: SettingType, index: number) => {
                    var loaded = this.loadedSettingTypes.some((item: KeyValuePair<number, number>) => {
                        return item.key === settingType.id
                            && item.value === settingType.settingTemplate
                    });

                    if (!loaded) {
                        var settingTypeName: string = StringFormater.format("settingtype{0}-template{1}", settingType.id, settingType.settingTemplate);
                        var filePath: string = StringFormater.format(
                            "app/customerSetting/{0}/{1}{2}.js?v={3}",
                            settingTypeName.replace("-", "."),
                            settingTypeName,
                            this.appConfigService.debugMode() ? "" : ".min",
                            this.appConfigService.debugMode() ? "0.0.1" : this.appConfigService.scriptVersion());
                        var newNodeParts = [
                            "<div data-oc-lazy-load=\"{ files: ['",
                            this.appConfigService.resolveRootUrl(filePath),
                            "'], cache: true }\">",
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
                            "></div>"
                        ];
                        var settingTypeElement = angular.element(newNodeParts.join(""));

                        element.append(settingTypeElement);
                        this.$compile(settingTypeElement)(scope);
                        this.loadedSettingTypes.push(new KeyValuePair<number, number>(settingType.id, settingType.settingTemplate));
                    }
                });
            }
        }
    }

    ModuleRegister.directive("settingTypeLoader", SettingTypeDirectiveLoader);
}