/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/models.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="adjust-member-pt-modal.ts" />
/// <reference path="models.ts" />
/// <reference path="position-taking-resources.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class SettingType1Template1Directive extends SettingTypeDirective {
        public static $inject: string[] = ["$q", "positionTakingResourceService", "updatingSettingTypePromiseChain"];

        public templateUrl: string = "settingtype1-template1.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.headerTemplateUrl = "settingtyp1-template1-header.html";
            scope.valueTemplateUrl = "settingtyp1-template1-value.html";
            scope.settingTypeName = this.positionTakingResourceService.getLabel("PositionTaking");

            this.updatingSettingTypePromiseChain.register(scope.settingType.id, (settingPartialCommand: SettingPartialCommand) => {
                return this.updatePositionTakingOnMembers(settingPartialCommand);
            });
        };

        private $q: ng.IQService;

        private positionTakingResourceService: PositionTakingResourceService;
        private updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain;

        constructor(
            $q: ng.IQService,
            positionTakingResourceService: PositionTakingResourceService,
            updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain) {
            super(Settingtype1Template1Item.factory);

            this.$q = $q;
            this.positionTakingResourceService = positionTakingResourceService;
            this.updatingSettingTypePromiseChain = updatingSettingTypePromiseChain;
        }

        public updatePositionTakingOnMembers(settingPartialCommand: SettingPartialCommand): ng.IPromise<void> {
            var deferred: ng.IDeferred<void> = this.$q.defer<void>();
            var testBool: boolean = confirm("I'm testing");

            console.log(testBool);
            deferred.resolve();

            return deferred.promise;
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
        public static $inject: string[] = ["$compile", "valueRangeService"];

        public replace: boolean = true;

        public scope: any = {
            settingItem: "=",
            fieldName: "@",
            min: "@",
            max: "@"
        };

        public templateUrl: string = "settingtype1-template1-select.htm";

        public controller: any = ["$scope", ($scope: IPositionTakingSelectScope) => {
            $scope.divisor = Settingtype1Template1Config.roundingFactor;

            $scope.$watch("min + max", (newValue: number, oldValue: number) => {
                this.updatePositionTakingValues(newValue, oldValue, $scope);
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

            this.$compile(select)(scope);
        };

        private $compile: ng.ICompileService;
        private valueRangeService: ValueRangeService;

        constructor($compile: ng.ICompileService, valueRangeService: ValueRangeService) {
            this.$compile = $compile;
            this.valueRangeService = valueRangeService;
        }

        private updatePositionTakingValues(newValue: number, oldValue: number, $scope: IPositionTakingSelectScope): void {
            if ($scope.settingItem) {
                if (!angular.equals(oldValue, newValue)) {
                    $scope.settingItem[$scope.fieldName] = Math.max($scope.min, Math.min($scope.max, $scope.settingItem[$scope.fieldName]));
                }

                $scope.range = this.valueRangeService.generateRanges(
                    $scope.settingItem[$scope.fieldName],
                    $scope.min,
                    $scope.max,
                    Settingtype1Template1Config.step,
                    Settingtype1Template1Config.roundingFactor);
            }
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