/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface IStatusCheckboxScope extends ng.IScope {
        item: SettingType4Item;
        change: (fieldName: string) => void;
    }

    class StatusCheckboxDirective implements ng.IDirective {
        public restrict: string = "A";

        public controller: any = ["$scope", ($scope: IStatusCheckboxScope) => {
            $scope.change = (fieldName: string) => {
                $scope.item.isDisabled = !$scope.item.isChecked;

                if ($scope.item.downline) {
                    if ($scope.item.isChecked && !(<SettingType4Item>$scope.item.oldSetting).isChecked) {
                        $scope.item.downline.readonly = false;
                    } else {
                        if ((<SettingType4Item>$scope.item.oldSetting).isChecked && (<SettingType4Item>$scope.item.downline.oldSetting).isChecked) {
                            $scope.item.downline.isChecked = $scope.item.isChecked;
                        } else {
                            $scope.item.downline.isChecked = false;
                        }

                        $scope.item.downline.readonly = true;
                    }

                    $scope.item.downline.isDisabled = !$scope.item.downline.isChecked;
                }

                this.resetFieldState($scope.item, fieldName);
            };
        }];

        private resetFieldState(item: SettingType4Item, fieldName: string): void {
            item.resetFieldHighlightState(fieldName);

            if (item.validationState && !item.validationState.isSuccessful()) {
                if (item.validationState.fieldName === fieldName) {
                    item.resetValidationState();
                }
            }
        }
    }

    ModuleRegister.directive("statusCheckbox", StatusCheckboxDirective);
}