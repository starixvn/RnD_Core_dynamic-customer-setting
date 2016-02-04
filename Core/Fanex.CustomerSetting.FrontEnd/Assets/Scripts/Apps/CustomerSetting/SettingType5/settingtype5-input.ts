/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType5InputScope extends ng.IScope {
        item: SettingType5Item;
        updateValue(): void;
    }

    class SettingType5InputDirective implements ng.IDirective {
        public static $inject: string[] = ["$compile", "$filter"];

        public replace: boolean = true;
        public require: string = "ngModel";

        public controller: any = ["$scope", ($scope: ISettingType5InputScope) => {
            $scope.updateValue = () => {
                $scope.item.validate();
            };
        }];

        public link: ng.IDirectiveLinkFn = (scope: ISettingType5InputScope, element: ng.IAugmentedJQuery, attributes: ng.IAttributes, modelCtrl: ng.INgModelController) => {
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

                if (modelCtrl.$modelValue === 0 && [46, 8].indexOf(event.keyCode) >= 0) {
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

                if (dotPistion >= 0 && dotPistion < input.selectionStart && numberParts[1].length > 1) {
                    event.preventDefault();

                    return;
                }
            });

            modelCtrl.$formatters.unshift(() => {
                return this.$filter("number")(modelCtrl.$modelValue);
            });

            modelCtrl.$parsers.unshift((viewValue: string) => {
                var input: HTMLInputElement = <HTMLInputElement>element[0];
                var value: number = 0;
                var formattedValue: string = viewValue;
                var caretPosition: number = input.selectionStart;
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

                element.val(formattedValue);

                if (viewValue.length !== caretPosition) {
                    var offset: number = viewValue.length - formattedValue.length;

                    caretPosition -= offset;

                    if (input.selectionStart === input.selectionEnd) {
                        input.selectionEnd = caretPosition;
                    }

                    input.selectionStart = caretPosition;
                }

                return value;
            });
        };

        private $compile: ng.ICompileService;
        private $filter: ng.IFilterService;

        constructor($compile: ng.ICompileService, $filter: ng.IFilterService) {
            this.$compile = $compile;
            this.$filter = $filter;
        }
    }

    ModuleRegister.directive("settingtype5Input", SettingType5InputDirective);
}