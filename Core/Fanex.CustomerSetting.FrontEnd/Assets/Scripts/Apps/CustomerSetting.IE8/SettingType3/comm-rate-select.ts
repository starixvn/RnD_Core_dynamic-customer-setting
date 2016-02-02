/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ISettingType3ItemScope extends ng.IScope {
        settingItem: SettingType3Item;
        range: Array<KeyValuePair<number, number>>;
    }

    class CommRateSelectDirective implements ng.IDirective {
        public static $inject: string[] = ["valueRangeService"];

        public replace: boolean = true;

        public scope: any = {
            settingItem: "="
        };

        public templateUrl: string = "comm-rate-select.htm";

        public controller: any = ["$scope", ($scope: ISettingType3ItemScope) => {
            $scope.$watch("settingItem", () => {
                $scope.range = this.valueRangeService.generateRanges(
                    $scope.settingItem.commRate,
                    0,
                    $scope.settingItem.upline.commRate,
                    SettingType3Config.step,
                    SettingType3Config.roundingFactor);
            });
        }];

        private valueRangeService: ValueRangeService;

        constructor(valueRangeService: ValueRangeService) {
            this.valueRangeService = valueRangeService;
        }
    }

    ModuleRegister.directive("commRateSelect", CommRateSelectDirective);
}