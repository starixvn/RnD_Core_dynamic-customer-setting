/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface IPositionTakingSelectScope extends ng.IScope {
        settingItem: CopyWithCheckboxSettingItem;
        fieldName: string;
        min: number;
        max: number;
        disabled: string;
        divisor: number;
        range: Array<KeyValuePair<number, number>>;
        resetState: (fieldName: string) => void;
        isMinPt: boolean;
        isPartialMinPt: boolean;
        isFirst: boolean;
    }

    class PositionTakingSelectDirective implements ng.IDirective {
        public static $inject: string[] = ["$compile", "$timeout"];

        public replace: boolean = true;

        public get scope(): any {
            return {
                settingItem: "=",
                fieldName: "@",
                min: "@",
                max: "@",
                disabled: "@",
                isMinPt: "=?",
                isPartialMinPt: "=?",
                isFirst: "=?"
            };
        }

        public templateUrl: string = "position-taking-select.htm";

        public controller: any = ["$scope", "valueRangeService", ($scope: IPositionTakingSelectScope, valueRangeService: ValueRangeService) => {
            $scope.isMinPt = !!$scope.isMinPt;
            $scope.isPartialMinPt = !!$scope.isPartialMinPt;
            $scope.isFirst = !!$scope.isFirst;
            $scope.divisor = SettingType18Config.roundingFactor;

            $scope.$watchGroup(["min", "max"], (newData: number[], oldData: number[]) => {
                if ($scope.settingItem) {
                    if (!angular.equals(oldData, newData)) {
                        $scope.settingItem[$scope.fieldName] = Math.max($scope.min, Math.min($scope.max, $scope.settingItem[$scope.fieldName]));
                    }

                    $scope.range = valueRangeService.generateRanges(
                        $scope.settingItem[$scope.fieldName],
                        $scope.min,
                        $scope.max,
                        SettingType18Config.step,
                        SettingType18Config.roundingFactor);
                }
            });

            $scope.resetState = (fieldName: string) => {
                this.$timeout(() => {
                    if ($scope.isPartialMinPt && $scope.isFirst && $scope.settingItem.isCheckedUpdateRow) {
                        $scope.$emit("copySettingTypeWithCheckbox18");
                    }
                }, 100);

                var item: DefaultSettingItem = <DefaultSettingItem>$scope.settingItem;

                if (item.validationState && !item.validationState.isSuccessful) {
                    if (item.validationState.fieldName === fieldName) {
                        item.resetValidationState();
                    }
                } else {
                    item.resetHighlightState();
                }
            }
        }];

        public link: ng.IDirectiveLinkFn = (scope: IPositionTakingSelectScope, element: ng.IAugmentedJQuery) => {
            var select: ng.IAugmentedJQuery = element.find("select");
            select.attr("data-ng-change", "resetState(fieldName)");
            select.attr("data-ng-model", "settingItem." + scope.fieldName);
            select.attr("data-ng-options", "item.key as (item.value | ptFormat) for item in range");
            select.attr("data-field-highlighting", "");

            if (scope.disabled === "true") {
                select.attr("disabled", "disabled");
            }

            this.$compile(element)(scope);
        };

        private $compile: ng.ICompileService;

        private $timeout: ng.ITimeoutService;

        constructor($compile: ng.ICompileService, $timeout: ng.ITimeoutService) {
            this.$compile = $compile;
            this.$timeout = $timeout;
        }
    }

    ModuleRegister.directive("positionTakingSelect", PositionTakingSelectDirective);

    function PositionTakingFormatFilter() {
        return (ptValue: number) => {
            return ptValue / SettingType18Config.roundingFactor;
        };
    }

    ModuleRegister.filter("ptFormat", PositionTakingFormatFilter);

    function HideIsAutoColumnFilter() {
        return (settingType: SettingType) => {
            return settingType.extraData.some((item: KeyValuePair<string, any>) => {
                return item.key === "isDisableMasterAutoPT" && item.value;
            });
        };
    }

    ModuleRegister.filter("hideIsAutoColumn", HideIsAutoColumnFilter);
}