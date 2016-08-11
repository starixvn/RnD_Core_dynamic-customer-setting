/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="settingtype10-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType10ItemScope extends ng.IScope {
        dataSource: SettingType10Item | CommissionGroup;
        range: Array<KeyValuePair<number, number>>;
    }

    class SettingType10SelectDirective implements ng.IDirective {
        public static $inject: string[] = ["valueRangeService"];

        public replace: boolean = true;

        public get scope(): any {
            return {
                dataSource: "="
            };
        }

        public templateUrl: string = "settingtype10-select.htm";

        public controller: any = ["$scope", ($scope: ISettingType10ItemScope) => {
            $scope.$watch("dataSource", (dataSource: SettingType10Item | CommissionGroup) => {
                var maxRate: number;

                if (dataSource instanceof SettingType10Item) {
                    if (dataSource.upline) {
                        maxRate = dataSource.upline.rate;
                    }
                } else {
                    maxRate = dataSource.maxRate;
                }

                $scope.range = this.valueRangeService.generateRanges(
                    dataSource.rate,
                    0,
                    maxRate,
                    SettingType10Config.step,
                    SettingType10Config.roundingFactor);
            });
        }];

        private valueRangeService: ValueRangeService;

        constructor(valueRangeService: ValueRangeService) {
            this.valueRangeService = valueRangeService;
        }
    }

    ModuleRegister.directive("settingtype10Select", SettingType10SelectDirective);
}