/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype22-resource.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType22WinLimitSelectDirectiveScope extends ng.IScope {
        settingItem: SettingType22Item;
        range: Array<KeyValuePair<number, string>>;
    }

    class SettingType22WinLimitSelectDirective implements ng.IDirective {
        public static $inject: string[] = ["settingType22ResourceService"];
        public static supportedWinLimits: number[] = [3, 5, 10, 0];

        public replace: boolean = true;

        public get scope(): any {
            return {
                settingItem: "="
            };
        }

        public templateUrl: string = "settingtype22-win-limit-select.htm";

        public controller: any = ["$scope", ($scope: ISettingType22WinLimitSelectDirectiveScope) => {
            $scope.$watch("settingItem", (settingItem: SettingType22Item) => {
                var availableWinLimits: number[] = [];

                if (settingItem.maxWinRange === 0) {
                    availableWinLimits = SettingType22WinLimitSelectDirective.supportedWinLimits;
                } else {
                    availableWinLimits = SettingType22WinLimitSelectDirective.supportedWinLimits.filter((winLimit: number) => {
                        return winLimit > 0 && winLimit <= settingItem.maxWinRange;
                    });

                    if (availableWinLimits.filter((range: number) => range === $scope.settingItem.winLimit).length === 0) {
                        availableWinLimits.unshift($scope.settingItem.winLimit);
                    }
                }

                $scope.range = availableWinLimits.map((winLimit: number) => {
                    return new KeyValuePair<number, string>(winLimit, winLimit > 0 ? winLimit + "X" : this.settingType22ResourceService.getLabel("Unlimited"));
                });
            });
        }];

        private settingType22ResourceService: SettingType22ResourceService;

        constructor(settingType22ResourceService: SettingType22ResourceService) {
            this.settingType22ResourceService = settingType22ResourceService;
        }
    }

    ModuleRegister.directive("settingtype22WinLimitSelect", SettingType22WinLimitSelectDirective);
}