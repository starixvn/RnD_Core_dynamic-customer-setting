/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ISettingType11CheckboxScope extends ng.IScope {
        item: SettingType11Item;
        change: () => void;
    }

    class SettingType11CheckboxDirective implements ng.IDirective {
        public restrict: string = "A";

        public controller: any = ["$scope", ($scope: ISettingType11CheckboxScope) => {
            $scope.change = () => {
                $scope.item.isDisabled = !$scope.item.isChecked;

                if ($scope.item.downline) {
                    if ($scope.item.isChecked && !(<SettingType11Item>$scope.item.oldSetting).isChecked) {
                        $scope.item.downline.readonly = false;
                    } else {
                        if ((<SettingType11Item>$scope.item.oldSetting).isChecked && (<SettingType11Item>$scope.item.downline.oldSetting).isChecked) {
                            $scope.item.downline.isChecked = $scope.item.isChecked;
                        } else {
                            $scope.item.downline.isChecked = false;
                        }

                        $scope.item.downline.isChecked = (<SettingType11Item>$scope.item.oldSetting).isChecked ? $scope.item.isChecked : false;
                        $scope.item.downline.readonly = true;
                    }
                }
            };
        }];
    }

    ModuleRegister.directive("settingtype11Checkbox", SettingType11CheckboxDirective);
}