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
        resetState: (fieldName: string) => void;
    }

    class PositionTakingSelectDirective implements ng.IDirective {
        public static $inject: string[] = ["$compile", "settingTypesSharedValuesService"];

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
            var isSupportPT3DecimalSites = this.settingTypesSharedValuesService.isSupportPT3DecimalSites;
            $scope.divisor = SettingType1Config.roundingFactor(isSupportPT3DecimalSites);

            $scope.$watchGroup(["min", "max"], (newData: number[], oldData: number[]) => {
                if ($scope.settingItem) {
                    if (!angular.equals(oldData, newData)) {
                        $scope.settingItem[$scope.fieldName] = Math.max($scope.min, Math.min($scope.max, $scope.settingItem[$scope.fieldName]));
                    }

                    $scope.range = valueRangeService.generateRanges(
                        $scope.settingItem[$scope.fieldName],
                        $scope.min,
                        $scope.max,
                        SettingType1Config.step(isSupportPT3DecimalSites),
                        SettingType1Config.roundingFactor(isSupportPT3DecimalSites));
                }
            });

            $scope.resetState = (fieldName: string) => {
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
            var isSupportPT3DecimalSites = this.settingTypesSharedValuesService.isSupportPT3DecimalSites;
            select.attr("data-ng-change", "resetState(fieldName)");
            select.attr("data-ng-model", "settingItem." + scope.fieldName);
            select.attr("data-ng-options", "item.key as (item.value | ptFormat:" + isSupportPT3DecimalSites +") for item in range");
            select.attr("data-field-highlighting", "");

            if (scope.disabled === "true") {
                select.attr("disabled", "disabled");
            }

            this.$compile(element)(scope);
        };

        private $compile: ng.ICompileService;
        private settingTypesSharedValuesService: SettingTypesSharedValuesService;

        constructor($compile: ng.ICompileService, settingTypesSharedValuesService: SettingTypesSharedValuesService) {
            this.$compile = $compile;
            this.settingTypesSharedValuesService = settingTypesSharedValuesService;
        }
    }

    ModuleRegister.directive("positionTakingSelect", PositionTakingSelectDirective);

    function PositionTakingFormatFilter() {
        return (ptValue: number, isSupportPT3DecimalSites: boolean) => {
            return ptValue / SettingType1Config.roundingFactor(isSupportPT3DecimalSites);
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