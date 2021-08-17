/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/numericinput/numeric-input.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype6-resource.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType6InputScope extends ng.IScope {
        item: SettingType6Item;
        updateValue(): void;
    }

    class SettingType6LossLimitInputDirective extends NumericInputDirective {
        public static $inject: string[] = ["$filter", "appConfigService", "settingType6ResourceService"];

        public controller: any = ["$scope", ($scope: ISettingType6InputScope) => {
            $scope.updateValue = () => {
                $scope.item.validate();
            };
        }];

        private settingType6ResourceService: SettingType6ResourceService;

        constructor($filter: ng.IFilterService, appConfigService: AppConfigService, settingType6ResourceService: SettingType6ResourceService) {
            super($filter, appConfigService);
            this.settingType6ResourceService = settingType6ResourceService;
        }

        protected formatModelValue(modelValue: number, scope: INumericInputScope) {
            if (modelValue === 0) {
                return this.settingType6ResourceService.getLabel("Unlimited");
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

    ModuleRegister.directive("settingtype6LossLimitInput", SettingType6LossLimitInputDirective);

    interface ISettingType6UnlimitedCheckboxDirectiveScope extends ng.IScope {
        item: SettingType6Item;
        check(): void;
    }

    class SettingType6UnlimitedCheckboxDirective implements ng.IDirective {
        public restrict: string = "A";

        public controller: any = ["$scope", ($scope: ISettingType6UnlimitedCheckboxDirectiveScope) => {
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

    ModuleRegister.directive("settingtype6UnlimitedCheckbox", SettingType6UnlimitedCheckboxDirective);
}