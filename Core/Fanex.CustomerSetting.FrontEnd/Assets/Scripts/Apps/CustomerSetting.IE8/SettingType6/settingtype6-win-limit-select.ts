/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype6-resource.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ISettingType6WinLimitSelectDirectiveScope extends ng.IScope {
        settingItem: SettingType6Item;
        range: Array<KeyValuePair<number, string>>;
    }

    class SettingType6WinLimitSelectDirective implements ng.IDirective {
        public static $inject: string[] = ["settingType6ResourceService"];
        public static supportedWinLimits: number[] = [3, 5, 10, 0];

        public replace: boolean = true;

        public scope: any = {
            settingItem: "="
        };

        public templateUrl: string = "settingtype6-win-limit-select.htm";

        public controller: any = ["$scope", ($scope: ISettingType6WinLimitSelectDirectiveScope) => {
            $scope.$watch("settingItem", (settingItem: SettingType6Item) => {
                var availableWinLimits: number[] = [];

                if (settingItem.maxWinRange === 0) {
                    availableWinLimits = SettingType6WinLimitSelectDirective.supportedWinLimits;
                } else {
                    availableWinLimits = _.filter(SettingType6WinLimitSelectDirective.supportedWinLimits, (winLimit: number) => {
                        return winLimit > 0 && winLimit <= settingItem.maxWinRange;
                    });

                    if (_.filter(availableWinLimits, (range: number) => range === $scope.settingItem.winLimit).length === 0) {
                        availableWinLimits.unshift($scope.settingItem.winLimit);
                    }
                }

                $scope.range = _.map(availableWinLimits, (winLimit: number) => {
                    return new KeyValuePair<number, string>(winLimit, winLimit > 0 ? winLimit + "X" : this.settingType6ResourceService.getLabel("Unlimited"));
                });
            });
        }];

        private settingType6ResourceService: SettingType6ResourceService;

        constructor(settingType6ResourceService: SettingType6ResourceService) {
            this.settingType6ResourceService = settingType6ResourceService;
        }
    }

    ModuleRegister.directive("settingtype6WinLimitSelect", SettingType6WinLimitSelectDirective);
}