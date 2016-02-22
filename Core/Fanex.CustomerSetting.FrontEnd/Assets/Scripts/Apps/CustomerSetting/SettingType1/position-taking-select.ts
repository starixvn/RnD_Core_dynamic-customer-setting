/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface IPositionTakingSelectScope extends ng.IScope {
        settingItem: ISettingItem;
        fieldName: string;
        min: number;
        max: number;
        disabled: string;
        divisor: number;
        range: Array<KeyValuePair<number, number>>;
    }

    class PositionTakingSelectDirective implements ng.IDirective {
        public static $inject: string[] = ["$compile"];

        public replace: boolean = true;

        public get scope(): any {
            return {
                settingItem: "=",
                fieldName: "@",
                min: "@",
                max: "@",
                disabled: "@"
            };
        }

        public templateUrl: string = "position-taking-select.htm";

        public controller: any = ["$scope", "valueRangeService", ($scope: IPositionTakingSelectScope, valueRangeService: ValueRangeService) => {
            $scope.divisor = SettingType1Config.roundingFactor;

            $scope.$watchGroup(["min", "max"], (newData: number[], oldData: number[]) => {
                if ($scope.settingItem) {
                    if (!angular.equals(oldData, newData)) {
                        $scope.settingItem[$scope.fieldName] = Math.max($scope.min, Math.min($scope.max, $scope.settingItem[$scope.fieldName]));
                    }

                    $scope.range = valueRangeService.generateRanges(
                        $scope.settingItem[$scope.fieldName],
                        $scope.min,
                        $scope.max,
                        SettingType1Config.step,
                        SettingType1Config.roundingFactor);
                }
            });
        }];

        public link: ng.IDirectiveLinkFn = (scope: IPositionTakingSelectScope, element: ng.IAugmentedJQuery) => {
            var select: ng.IAugmentedJQuery = element.find("select");

            select.attr("data-ng-model", "settingItem." + scope.fieldName);
            select.attr("data-ng-options", "item.key as (item.value | ptFormat) for item in range");
            select.attr("data-ng-class", StringFormater.format("{ 'setting-field--invalid': (settingItem.validationState | highlightingField:'{0}') }", scope.fieldName));
            select.attr("data-tooltip", "");
            select.attr(
                "data-message",
                StringFormater.format("{{settingItem.validationState | tooltipMessage:'positionTakingResourceService':'{0}'}}", scope.fieldName));

            if (scope.disabled === "true") {
                select.attr("disabled", "disabled");
            }

            this.$compile(element)(scope);
        };

        private $compile: ng.ICompileService;

        constructor($compile: ng.ICompileService) {
            this.$compile = $compile;
        }
    }

    ModuleRegister.directive("positionTakingSelect", PositionTakingSelectDirective);

    function PositionTakingFormatFilter() {
        return (ptValue: number) => {
            return ptValue / SettingType1Config.roundingFactor;
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