/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/underscore/underscore.d.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ISettingTypeLoaderScope extends ng.IScope {
        setting: Setting;
        update: (settingTypeId: { settingTypeId: number }) => void;
    }

    class SettingTypeDirectiveLoader implements ng.IDirective {
        public static $inject: string[] = ["$compile"];
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
        private loadedSettingTypes: Array<number>;

        constructor($compile: ng.ICompileService) {
            this.$compile = $compile;
            this.loadedSettingTypes = new Array<number>();
        }

        private generateNodes(scope: ISettingTypeLoaderScope, element: ng.IAugmentedJQuery): void {
            if (scope.setting && scope.setting.settingTypes && angular.isArray(scope.setting.settingTypes)) {
                _.forEach(scope.setting.settingTypes, (settingType: SettingType, index: number) => {
                    var loaded = _.some(this.loadedSettingTypes, (item: number) => {
                        return item === settingType.id;
                    });

                    if (!loaded) {
                        var settingTypeName: string = StringFormater.format("settingtype{0}", settingType.id);
                        var newNodeParts = [
                            "<div data-",
                            settingTypeName,
                            " data-setting-type=\"setting.settingTypes[",
                            index,
                            "]\"",
                            " data-update=\"update({ settingTypeId: settingTypeId})\"",
                            "></div>"
                        ];
                        var settingTypeElement = angular.element(newNodeParts.join(""));

                        if (index === scope.setting.settingTypes.length - 1) {
                            settingTypeElement.addClass("last-setting-type");
                        }

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