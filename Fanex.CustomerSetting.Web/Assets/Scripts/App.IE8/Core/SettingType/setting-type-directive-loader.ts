/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../helper-services.ts" />
/// <reference path="../helpers.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ISettingTypeLoaderScope extends ng.IScope {
        uplineLevel: Level;
        setting: Setting;
        update: (settingTypeId: { settingTypeId: number }) => void;
    }

    class SettingTypeDirectiveLoader implements ng.IDirective {
        public static $inject: string[] = ["$compile", "appConfigService"];
        public replace: boolean = true;

        public scope: any = {
            uplineLevel: "=",
            setting: "=",
            update: "&"
        };

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeLoaderScope, element: ng.IAugmentedJQuery) => {
            scope.$watch("setting.settingTypes", () => {
                this.generateNodes(scope, element);
            });
        };

        private $compile: ng.ICompileService;
        private appConfigService: AppConfigService;
        private loadedSettingTypes: Array<KeyValuePair<number, number>>;

        constructor($compile: ng.ICompileService, appConfigService: AppConfigService) {
            this.$compile = $compile;
            this.appConfigService = appConfigService;
            this.loadedSettingTypes = new Array<KeyValuePair<number, number>>();
        }

        private generateNodes(scope: ISettingTypeLoaderScope, element: ng.IAugmentedJQuery): void {
            if (angular.isDefined(scope.setting)) {
                _.forEach(scope.setting.settingTypes, (settingType: SettingType, index: number) => {
                    var loaded = _.some(this.loadedSettingTypes, (item: KeyValuePair<number, number>) => {
                        return item.key === settingType.id
                            && item.value === settingType.settingTemplate;
                    });

                    if (!loaded) {
                        var settingTypeName: string = StringFormater.format("settingtype{0}-template{1}", settingType.id, settingType.settingTemplate);
                        var newNodeParts = [
                            "<div data-",
                            settingTypeName,
                            " data-upline-level=\"uplineLevel\"",
                            " data-customer-level=\"setting.customerLevel\"",
                            " data-product-id=\"setting.productId\"",
                            " data-product-name=\"setting.productName\"",
                            " data-setting-type=\"setting.settingTypes[",
                            index,
                            "]\"",
                            "data-update=\"update({ settingTypeId: settingTypeId})\"",
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