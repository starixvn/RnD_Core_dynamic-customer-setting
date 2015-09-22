/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/models.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="adjust-member-pt-modal.ts" />
/// <reference path="models.ts" />
/// <reference path="position-taking-resources.ts" />

module CustomerSetting {
    "use strict";

    class SettingType1Directive extends SettingTypeDirective {
        public static $inject: string[] = ["positionTakingResourceService"];

        public templateUrl: string = "settingtype1.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.headerTemplateUrl = "settingtype1-header.htm";
            scope.valueTemplateUrl = "settingtype1-value.htm";
            scope.settingTypeName = this.positionTakingResourceService.getLabel("PositionTaking");
        };

        private positionTakingResourceService: PositionTakingResourceService;

        constructor(positionTakingResourceService: PositionTakingResourceService) {
            super(SettingType1Item.factory);

            this.positionTakingResourceService = positionTakingResourceService;
        }
    }

    ModuleRegister.directive("settingtype1", SettingType1Directive);

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

        public templateUrl: string = "settingtype1-select.htm";

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
            select.attr("data-ng-class", StringFormater.format("{ 'invalid': (settingItem.validationState | highlightingField:'{0}') }", scope.fieldName));
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

    function HideIsAutoColumnFilter() {
        return (settingType: SettingType) => {
            return settingType.extraData.some((item: KeyValuePair<string, any>) => {
                return item.key === "isDisableMasterAutoPT" && item.value;
            });
        };
    }

    ModuleRegister.filter("hideIsAutoColumn", HideIsAutoColumnFilter);
}