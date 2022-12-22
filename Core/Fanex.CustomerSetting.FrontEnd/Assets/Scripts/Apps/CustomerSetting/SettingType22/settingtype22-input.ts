/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/numericinput/numeric-input.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype22-resource.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType22InputScope extends ng.IScope {
        item: SettingType22Item;
        updateValue(): void;
    }

    class SettingType22LossLimitInputDirective extends NumericInputDirective {
        public static $inject: string[] = ["$filter", "appConfigService", "settingType22ResourceService"];

        public controller: any = ["$scope", ($scope: ISettingType22InputScope) => {
            $scope.updateValue = () => {
                $scope.item.validate();
            };
        }];

        private settingType22ResourceService: SettingType22ResourceService;

        constructor($filter: ng.IFilterService, appConfigService: AppConfigService, settingType22ResourceService: SettingType22ResourceService) {
            super($filter, appConfigService);
            this.settingType22ResourceService = settingType22ResourceService;
        }

        protected formatModelValue(modelValue: number, scope: INumericInputScope) {
            if (modelValue === 0) {
                return this.settingType22ResourceService.getLabel("Unlimited");
            } else {
                return super.formatModelValue(modelValue, scope);
            }
        }

        protected parseAndFormatViewValue(viewValue: string, precision: number, rounding: boolean): IOriginalAndFormattedValuePair {
            var { formattedValue, value }: IOriginalAndFormattedValuePair = super.parseAndFormatViewValue(viewValue, precision, rounding);

            if (value === 0) {
                formattedValue = "";
                value = null;
            }

            return {
                formattedValue: formattedValue,
                value: value
            };
        }
    }

    ModuleRegister.directive("settingtype22LossLimitInput", SettingType22LossLimitInputDirective);

    interface ISettingType22UnlimitedCheckboxDirectiveScope extends ng.IScope {
        item: SettingType22Item;
        check(): void;
    }

    class SettingType22UnlimitedCheckboxDirective implements ng.IDirective {
        public restrict: string = "A";

        public controller: any = ["$scope", ($scope: ISettingType22UnlimitedCheckboxDirectiveScope) => {
            $scope.item.isUnlimited = $scope.item.lossLimit === 0;
            $scope.check = () => {
                $scope.item.lossLimit = $scope.item.isUnlimited ? 0 : null;

                // Hide validation error if there is any.
                if ($scope.item.isUnlimited && $scope.item.validationState.fieldName === "lossLimit") {
                    $scope.item.resetValidationState();
                }
            };
        }];
    }

    ModuleRegister.directive("settingtype22UnlimitedCheckbox", SettingType22UnlimitedCheckboxDirective);
}