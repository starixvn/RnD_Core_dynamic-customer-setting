/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface IBetSettingInputScope extends ng.IScope {
        settingItem: SettingType2Item;
        customerLevel: Level;
        uplineLevel: Level;
        fieldName: string;
        updateValue(): void;
    }

    class BetSettingInputDirective implements ng.IDirective {
        public static $inject: string[] = ["$compile", "$filter", "betSettingResourceService"];

        public replace: boolean = true;
        public templateUrl: string = "bet-setting-input.htm";

        public scope: any = {
            settingItem: "=",
            customerLevel: "=",
            uplineLevel: "=",
            fieldName: "@"
        };

        public controller: any = ["$scope", ($scope: IBetSettingInputScope) => {
            $scope.updateValue = () => {
                if ($scope.fieldName === "minBet") {
                    $scope.settingItem.validateMinBet();
                } else if ($scope.fieldName === "maxBet") {
                    $scope.settingItem.validateMaxBet();
                } else if ($scope.fieldName === "maxPerMatch") {
                    $scope.settingItem.validateMaxPerMatch();
                }
            };
        }];

        public link: ng.IDirectiveLinkFn = (scope: IBetSettingInputScope, wrapElement: ng.IAugmentedJQuery) => {
            var element: ng.IAugmentedJQuery = angular.element(wrapElement.children()[0]);
            var unlimitedUpline: boolean = true;
            var constraintElement: Node;
            var modelCtrl: any;

            element.attr("data-ng-model", "settingItem." + scope.fieldName);
            element.attr("data-ng-change", "updateValue()");
            element.attr("data-tooltip", "");
            element.attr(
                "data-ng-class",
                StringFormater.format("{ 'setting-field--invalid': (settingItem.validationState | highlightingField:'{0}') }", scope.fieldName));
            element.attr(
                "data-message",
                StringFormater.format("{{settingItem.validationState | tooltipMessage:'betSettingResourceService':'{0}'}}", scope.fieldName));

            if (scope.customerLevel !== Level.superAgent || (scope.fieldName === "minBet" && scope.settingItem.upline.minBet > 0)) {
                element.addClass("setting-field--has-constraint");
            }

            if (scope.settingItem.upline) {
                unlimitedUpline = scope.settingItem.upline.minBet === 0 && scope.settingItem.upline.maxBet === 0 && scope.settingItem.upline.maxPerMatch === 0;
            }

            if (!unlimitedUpline) {
                if (scope.fieldName === "minBet") {
                    wrapElement.append(StringFormater.format("<span> >={{settingItem.upline.{0} | number}}</span>", scope.fieldName));
                } else {
                    wrapElement.append(StringFormater.format("<span> <={{settingItem.upline.{0} | number}}</span>", scope.fieldName));
                }
            }

            this.$compile(element)(scope);
            constraintElement = element[0].nextSibling;

            if (constraintElement) {
                this.$compile(angular.element(constraintElement))(scope);
            }

            modelCtrl = element.controller("ngModel");

            // Chrome has a weird behavior with the mouseup event on textboxes.
            // The position of the cursor is set on this event so that once it is fired, the selection will be removed.
            // We add some lines of code to trace the focus status as a hack for this issue.
            element.on("focus", () => {
                element.data("focused", true);
            });

            element.on("focusout", () => {
                element.data("focused", false);
            });

            element.on("mouseup", () => {
                if (element.data("focused")) {
                    var input: HTMLInputElement = <HTMLInputElement>element[0];

                    input.select();
                }
            });

            element.on("keydown", function (event: JQueryEventObject) {
                var dotPistion = modelCtrl.$viewValue.indexOf(".");
                var numberParts: string[] = modelCtrl.$viewValue.split(".");
                var input: HTMLInputElement = <HTMLInputElement>element[0];

                if (modelCtrl.$modelValue === 0 && [46, 8].indexOf(event.keyCode) >= 0) {
                    event.preventDefault();

                    return;
                }

                if ([46, 8, 9, 27, 13, 110].indexOf(event.keyCode) >= 0 // Allow: backspace, delete, tab, escape, enter
                    || (event.keyCode === 65 && event.ctrlKey === true) // Allow: Ctrl+A
                    || (event.keyCode >= 35 && event.keyCode <= 40)) { // Allow: home, end, left, right, down, up
                    // let it happen, don't do anything
                    return;
                }

                if ((event.shiftKey || (event.keyCode < 48 || event.keyCode > 57))
                    && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();

                    return;
                }

                if (dotPistion >= 0 && dotPistion < getSelectionStart(input) && numberParts[1].length > 1) {
                    event.preventDefault();

                    return;
                }
            });

            modelCtrl.$formatters.unshift(() => {
                return this.$filter("number")(modelCtrl.$modelValue);
            });

            modelCtrl.$parsers.unshift((viewValue: string) => {
                var value: number = 0;
                var numberParts: string[] = viewValue ? viewValue.split(".") : [];

                if (numberParts.length > 1) {
                    var plainIntegerPart: string = numberParts[0].replace(/[\,]/g, "") || "0";
                    var decimalPart: string = numberParts[1].replace(/[\,\.]/g, "") || "";

                    decimalPart = decimalPart.substring(0, 2);
                    value = parseFloat(plainIntegerPart + "." + decimalPart);
                    element.val(this.$filter("number")(plainIntegerPart) + "." + decimalPart);

                    return value;
                } else if (numberParts.length > 0) {
                    var plainNumber: string = viewValue.replace(/[\,]/g, "");

                    value = parseInt(plainNumber, 10) || 0;
                    element.val(this.$filter("number")(value));

                    return value;
                }
            });

            function getSelectionStart(element: HTMLInputElement): number {
                if (element.createTextRange) {
                    var selectionRange = (<any>document).selection.createRange().duplicate();

                    selectionRange.moveEnd("character", element.value.length);

                    if (selectionRange.text === "") {
                        return element.value.length;
                    }

                    return element.value.lastIndexOf(selectionRange.text);
                } else {
                    return element.selectionStart;
                }
            }
        };

        private $compile: ng.ICompileService;
        private $filter: ng.IFilterService;

        constructor($compile: ng.ICompileService, $filter: ng.IFilterService) {
            this.$compile = $compile;
            this.$filter = $filter;
        }
    }

    ModuleRegister.directive("betSettingInput", BetSettingInputDirective);
}