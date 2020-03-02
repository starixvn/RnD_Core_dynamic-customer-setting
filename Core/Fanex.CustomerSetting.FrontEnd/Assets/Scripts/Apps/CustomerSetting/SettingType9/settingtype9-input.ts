/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType9InputScope extends ng.IScope {
        settingItem: SettingType9Item;
        customerLevel: Level;
        uplineLevel: Level;
        fieldName: string;
        updateValue(): void;
    }

    class SettingType9InputDirective implements ng.IDirective {
        public static $inject: string[] = ["$compile"];

        public replace: boolean = true;
        public templateUrl: string = "settingtype9-input.htm";

        public get scope(): any {
            return {
                settingItem: "=",
                customerLevel: "=",
                uplineLevel: "=",
                fieldName: "@"
            };
        }

        public controller: any = ["$scope", ($scope: ISettingType9InputScope) => {
            $scope.updateValue = () => {
                if ($scope.fieldName === "minBet") {
                    $scope.settingItem.validateMinBet();
                } else if ($scope.fieldName === "maxBet") {
                    $scope.settingItem.validateMaxBet();
                } else if ($scope.fieldName === "maxPerNumber") {
                    $scope.settingItem.validateMaxPerNumber();
                }
            };
        }];

        public link: ng.IDirectiveLinkFn = (scope: ISettingType9InputScope, wrapElement: ng.IAugmentedJQuery) => {
            var element: ng.IAugmentedJQuery = angular.element(wrapElement.children()[0]);

            element.attr("data-ng-model", "settingItem." + scope.fieldName);
            element.attr("data-ng-change", "updateValue()");
            element.attr("data-numeric-input", "");

            if (scope.fieldName === "minBet") {
                wrapElement.append(
                    StringFormater.format("<span data-ng-if=\"settingItem.upline.{0} > 0\"> >={{settingItem.upline.{0} | formatNumber}}</span>", scope.fieldName));
            } else {
                wrapElement.append(
                    StringFormater.format("<span data-ng-if=\"settingItem.upline.{0} > 0\"> <={{settingItem.upline.{0} | formatNumber}}</span>", scope.fieldName));
            }

            this.$compile(wrapElement)(scope);
        };

        private $compile: ng.ICompileService;

        constructor($compile: ng.ICompileService) {
            this.$compile = $compile;
        }
    }

    ModuleRegister.directive("settingtype9Input", SettingType9InputDirective);
}