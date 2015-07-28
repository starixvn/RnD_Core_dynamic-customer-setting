/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../resource/resource-translation.ts" />
/// <reference path="../common.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    interface DefaultSettingBlockScope extends ng.IScope {
        productId: number;
        productName: string;
        settingType: SettingType;
        settingLevel: SettingLevel;
        update: (settingTypeId: { settingTypeId: number }) => void;
    }

    class DefaultSettingBlockDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                productId: "=",
                productName: "=",
                settingType: "=",
                update: "&"
            }
        }

        public templateUrl: string = "DefaultSettingBlock/default-setting-block.htm";

        public controller: any = ["$scope", "settingLevel", ($scope: DefaultSettingBlockScope, settingLevel: SettingLevel) => {
            $scope.settingLevel = settingLevel;
        }];
    }

    ModuleRegister.directive("defaultSettingBlock", DefaultSettingBlockDirective);
}