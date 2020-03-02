/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/numericinput/numeric-input.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype12-resource.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType12InputScope extends ng.IScope {
        item: SettingType12Item;
        updateValue(): void;
    }

    class SettingType12LossLimitInputDirective extends NumericInputDirective {
        public static $inject: string[] = ["$filter", "appConfigService", "settingType12ResourceService"];

        public controller: any = ["$scope", ($scope: ISettingType12InputScope) => {
            $scope.updateValue = () => {
                $scope.item.validate();
            };
        }];

        private settingType12ResourceService: SettingType12ResourceService;

        constructor($filter: ng.IFilterService, appConfigService: AppConfigService, settingType12ResourceService: SettingType12ResourceService) {
            super($filter, appConfigService);
            this.settingType12ResourceService = settingType12ResourceService;
        }

        protected formatModelValue(modelValue: number, scope: INumericInputScope) {
            if (modelValue === 0) {
                return this.settingType12ResourceService.getLabel("Unlimited");
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

                // Hide validation error if there is any.
                if ($scope.isUnlimited && $scope.item.validationState.fieldName === "lossLimit") {
                    $scope.item.resetValidationState();
                }
            };
        }];
    }

    ModuleRegister.directive("settingtype12UnlimitedCheckbox", SettingType12UnlimitedCheckboxDirective);
}