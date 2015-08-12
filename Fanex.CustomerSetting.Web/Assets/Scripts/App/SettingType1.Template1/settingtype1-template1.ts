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

    class SettingType1Template1Directive extends SettingTypeDirective {
        public static $inject: string[] = ["appConfigService", "positionTakingResourceService"];

        public templateUrl: Function = () => {
            return this.appConfigService.resolveRootUrl("app/customerSetting/settingtype1.template1/settingtype1-template1.htm");
        };

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.headerTemplateUrl = "settingtyp1-template1-header.html";
            scope.valueTemplateUrl = "settingtyp1-template1-value.html";
            scope.settingTypeName = this.positionTakingResourceService.getLabel("PositionTaking");
        };

        private appConfigService: AppConfigService;
        private positionTakingResourceService: PositionTakingResourceService;

        constructor(appConfigService: AppConfigService, positionTakingResourceService: PositionTakingResourceService) {
            super(Settingtype1Template1Item.factory);

            this.appConfigService = appConfigService;
            this.positionTakingResourceService = positionTakingResourceService;
        }
    }

    ModuleRegister.directive("settingtype1Template1", SettingType1Template1Directive);

    interface IPositionTakingSelectScope extends ng.IScope {
        settingItem: ISettingItem;
        fieldName: string;
        min: number;
        max: number;
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
                max: "@"
            };
        }

        public templateUrl: string = "settingtype1-template1-select.html";

        public controller: any = [
            "$scope",
            "valueRangeService",
            "positionTakingResourceService",
            ($scope: IPositionTakingSelectScope, valueRangeService: ValueRangeService, positionTakingResourceService: PositionTakingResourceService) => {
                $scope.divisor = Settingtype1Template1Config.roundingFactor;

                $scope.$watchGroup(["min", "max"], (newData: number[], oldData: number[]) => {
                    if ($scope.settingItem) {
                        if (!angular.equals(oldData, newData)) {
                            $scope.settingItem[$scope.fieldName] = Math.max($scope.min, Math.min($scope.max, $scope.settingItem[$scope.fieldName]));
                        }

                        $scope.range = valueRangeService.generateRanges(
                            $scope.settingItem[$scope.fieldName],
                            $scope.min,
                            $scope.max,
                            Settingtype1Template1Config.step,
                            Settingtype1Template1Config.roundingFactor);
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

            this.$compile(element)(scope);
        };

        private $compile: ng.ICompileService;

        constructor($compile: ng.ICompileService) {
            this.$compile = $compile;
        }
    }

    ModuleRegister.directive("positionTakingSelect", PositionTakingSelectDirective);

    interface IAutoPositionTakingScope extends ng.IScope {
        item: ISettingItem;
        confirmUpdatingMemberPt: (isShown: boolean, max: number, updatingMarker: string, ptOnMemberField: string) => void;
    }

    class AutoPositionTakingDirective implements ng.IDirective {
        public restrict: string = "A";

        public controller: any = [
            "$scope",
            "adjustMemberPtModalService",
            ($scope: IAutoPositionTakingScope, adjustMemberPtModalService: AdjustMemberPtModalService) => {
                $scope.confirmUpdatingMemberPt = (isShown: boolean, max: number, updatingMarker: string, ptOnMemberField: string) => {
                    if (isShown) {
                        adjustMemberPtModalService.show(max, (memberPt: number) => {
                            $scope.item[updatingMarker] = true;
                            $scope.item[ptOnMemberField] = memberPt;
                        });
                    }
                };
            }];
    }

    ModuleRegister.directive("autoPositionTaking", AutoPositionTakingDirective);
}