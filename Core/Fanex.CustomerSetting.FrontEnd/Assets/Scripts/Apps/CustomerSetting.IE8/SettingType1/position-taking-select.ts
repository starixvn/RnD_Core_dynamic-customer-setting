/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
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
        public static $inject: string[] = ["$compile", "valueRangeService"];

        public replace: boolean = true;

        public scope: any = {
            settingItem: "=",
            fieldName: "@",
            min: "@",
            max: "@",
            disabled: "@"
        };

        public templateUrl: string = "position-taking-select.htm";

        public link: ng.IDirectiveLinkFn = (scope: IPositionTakingSelectScope, element: ng.IAugmentedJQuery) => {
            var select: ng.IAugmentedJQuery = element.find("select");

            select.attr("data-ng-model", "settingItem." + scope.fieldName);
            select.attr("data-ng-class", StringFormater.format("{ 'setting-field--invalid': (settingItem.validationState | highlightingField:'{0}') }", scope.fieldName));
            select.attr("data-tooltip", "");
            select.attr(
                "data-message",
                StringFormater.format("{{settingItem.validationState | tooltipMessage:'positionTakingResourceService':'{0}'}}", scope.fieldName));

            if (scope.disabled === "true") {
                select.attr("disabled", "disabled");
            }

            select.on("mouseover", (event: JQueryEventObject) => {
                if (scope.range.length === 1) {
                    this.updatePositionTakingValues(scope);
                    scope.$apply();
                }
            });

            this.$compile(select)(scope);

            scope.divisor = SettingType1Config.roundingFactor;
            scope.$watch("min + max", (newValue: number, oldValue: number) => {
                if (!angular.equals(oldValue, newValue)) {
                    scope.settingItem[scope.fieldName] = Math.max(scope.min, Math.min(scope.max, scope.settingItem[scope.fieldName]));
                }

                var value: number = scope.settingItem[scope.fieldName];

                scope.range = [new KeyValuePair<number, number>(value, value)];
            });
            scope.$watch("settingItem." + scope.fieldName, (value: number) => {
                scope.range = [new KeyValuePair<number, number>(value, value)];
            });
        };

        private $compile: ng.ICompileService;
        private valueRangeService: ValueRangeService;

        constructor($compile: ng.ICompileService, valueRangeService: ValueRangeService) {
            this.$compile = $compile;
            this.valueRangeService = valueRangeService;
        }

        private updatePositionTakingValues($scope: IPositionTakingSelectScope): void {
            if ($scope.settingItem) {
                $scope.range = this.valueRangeService.generateRanges(
                    $scope.settingItem[$scope.fieldName],
                    $scope.min,
                    $scope.max,
                    SettingType1Config.step,
                    SettingType1Config.roundingFactor);
            }
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
            return _.some(settingType.extraData, (item: KeyValuePair<string, any>) => {
                return item.key === "isDisableMasterAutoPT" && item.value;
            });
        };
    }

    ModuleRegister.filter("hideIsAutoColumn", HideIsAutoColumnFilter);
}