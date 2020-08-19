/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="commission-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    const roundingFactor: number = 10000;

    interface ISettingType19ItemScope extends ng.IScope {
        settingItem: SettingType19Item;
        range: Array<KeyValuePair<number, number>>;
        resetValidationState: () => void;
        disabled: boolean;
        isCopyWithCheckbox: boolean;
    }

    class CommRateSelectDirective implements ng.IDirective {
        public static $inject: string[] = ["valueRangeService"];

        public replace: boolean = true;

        public get scope(): any {
            return {
                settingItem: "=",
                disabled: "=?",
                isCopyWithCheckbox: "=?"
            };
        }

        public templateUrl: string = "comm-rate-select.htm";

        public controller: any = ["$scope", "$timeout", ($scope: ISettingType19ItemScope, $timeout: ng.ITimeoutService) => {
            $scope.isCopyWithCheckbox = !!$scope.isCopyWithCheckbox;
            $scope.disabled = !!$scope.disabled;
            $scope.$watch("settingItem", () => {
                $scope.range = this.valueRangeService.generateRanges(
                    $scope.settingItem.commRate,
                    0,
                    $scope.settingItem.upline.commRate,
                    $scope.settingItem.intervalStep,
                    roundingFactor);
            });

            $scope.resetValidationState = () => {
                $scope.settingItem.resetValidationState();
                if ($scope.isCopyWithCheckbox && !$scope.disabled) {
                    $scope.$emit("copySettingTypeWithCheckbox19");
                }
            }
        }];

        private valueRangeService: ValueRangeService;

        private $timeout: ng.ITimeoutService;

        constructor(valueRangeService: ValueRangeService, $timeout: ng.ITimeoutService) {
            this.valueRangeService = valueRangeService;
            this.$timeout = $timeout;
        }
    }

    ModuleRegister.directive("commRateSelect", CommRateSelectDirective);
}