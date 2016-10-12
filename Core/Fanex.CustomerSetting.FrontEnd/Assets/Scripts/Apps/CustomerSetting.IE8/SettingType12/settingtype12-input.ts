/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype12-resource.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ISettingType12InputScope extends ng.IScope {
        item: SettingType12Item;
        updateValue(): void;
    }

    class SettingType12LossLimitInputDirective implements ng.IDirective {
        public static $inject: string[] = ["$compile", "$filter", "settingType12ResourceService"];

        public restrict: string = "A";
        public require: string = "ngModel";

        public controller: any = ["$scope", ($scope: ISettingType12InputScope) => {
            $scope.updateValue = () => {
                $scope.item.validate();
            };
        }];

        public link: ng.IDirectiveLinkFn = (
            scope: ISettingType12InputScope,
            element: ng.IAugmentedJQuery,
            attributes: ng.IAttributes,
            modelCtrl: ng.INgModelController) => {
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
                var value: string = element.val();
                var dotPistion = value.indexOf(".");
                var numberParts: string[] = value.split(".");
                var input: HTMLInputElement = <HTMLInputElement>element[0];

                if (value.length === 0 && event.keyCode === 96) {
                    event.preventDefault();

                    return;
                }

                if ([110, 190].indexOf(event.keyCode) >= 0) {
                    if (numberParts.length > 1) {
                        event.preventDefault();

                        return;
                    } else {
                        return;
                    }
                }

                if ([46, 8, 9, 27, 13].indexOf(event.keyCode) >= 0 // Allow: backspace, delete, tab, escape, enter
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
                if (modelCtrl.$modelValue === 0) {
                    return this.settingType12ResourceService.getLabel("Unlimited");
                } else {
                    return this.$filter("number")(modelCtrl.$modelValue);
                }
            });

            modelCtrl.$parsers.unshift((viewValue: string) => {
                var value: number = 0;
                var formattedValue: string = viewValue;
                var numberParts: string[] = viewValue ? viewValue.split(".") : [];

                if (numberParts.length > 1) {
                    var plainIntegerPart: string = numberParts[0].replace(/[\,]/g, "") || "0";
                    var decimalPart: string = numberParts[1].replace(/[\,\.]/g, "") || "";

                    decimalPart = decimalPart.substring(0, 2);
                    value = parseFloat(plainIntegerPart + "." + decimalPart);
                    formattedValue = this.$filter("number")(plainIntegerPart) + "." + decimalPart;
                } else if (numberParts.length > 0) {
                    var plainNumber: string = viewValue.replace(/[\,]/g, "");

                    value = parseInt(plainNumber, 10) || 0;
                    formattedValue = this.$filter("number")(value);
                }

                if (value === 0) {
                    formattedValue = "";
                    value = null;
                }

                element.val(formattedValue);

                return value;
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
        private settingType12ResourceService: SettingType12ResourceService;

        constructor($compile: ng.ICompileService, $filter: ng.IFilterService, settingType12ResourceService: SettingType12ResourceService) {
            this.$compile = $compile;
            this.$filter = $filter;
            this.settingType12ResourceService = settingType12ResourceService;
        }
    }

    ModuleRegister.directive("settingtype12LossLimitInput", SettingType12LossLimitInputDirective);

    interface ISettingType12UnlimitedCheckboxDirectiveScope extends ng.IScope {
        item: SettingType12Item;
        isUnlimited: boolean;
        check(): void;
    }

    class SettingType12UnlimitedCheckboxDirective implements ng.IDirective {
        public restrict: string = "A";

        public controller: any = ["$scope", ($scope: ISettingType12UnlimitedCheckboxDirectiveScope) => {
            $scope.isUnlimited = $scope.item.lossLimit === 0;

            $scope.check = () => {
                $scope.item.lossLimit = $scope.isUnlimited ? 0 : null;

                if ($scope.isUnlimited && $scope.item.validationState.fieldName === "lossLimit") {
                    $scope.item.resetValidationState();
                }
            };
        }];
    }

    ModuleRegister.directive("settingtype12UnlimitedCheckbox", SettingType12UnlimitedCheckboxDirective);
}