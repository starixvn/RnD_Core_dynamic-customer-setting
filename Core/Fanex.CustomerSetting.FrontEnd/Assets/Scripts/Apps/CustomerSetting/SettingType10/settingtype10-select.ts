/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="settingtype10-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType10SelectDataSource {
        rate: number,
        upline?: { rate: number },
        maxRate?: number,
        validationState: ValidationResult;
    }

    interface ISettingType10SelectScope extends ng.IScope {
        dataSource: ISettingType10SelectDataSource;
        range: Array<KeyValuePair<number, number>>;
        resetValidationState: () => void;
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

        public controller: any = ["$scope", ($scope: ISettingType10SelectScope) => {
            $scope.$watch("dataSource", (dataSource: ISettingType10SelectDataSource) => {
                var maxRate: number = dataSource.rate;

                if (dataSource.upline) {
                    maxRate = dataSource.upline.rate;
                } else if (dataSource.maxRate) {
                    maxRate = dataSource.maxRate;
                }

                $scope.range = this.valueRangeService.generateRanges(
                    dataSource.rate,
                    0,
                    maxRate,
                    SettingType10Config.step,
                    SettingType10Config.roundingFactor);
            });

            $scope.resetValidationState = () => {
                $scope.dataSource.validationState = <ValidationResult>Result.normal;
            }
        }];

        private valueRangeService: ValueRangeService;

        constructor(valueRangeService: ValueRangeService) {
            this.valueRangeService = valueRangeService;
        }
    }

    ModuleRegister.directive("settingtype10Select", SettingType10SelectDirective);
}