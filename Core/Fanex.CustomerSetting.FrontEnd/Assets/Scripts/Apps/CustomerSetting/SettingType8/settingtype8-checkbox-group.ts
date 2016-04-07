/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ICheckboxGroupScope extends ng.IScope {
        checked: boolean;
        items: Array<SettingType8StakeLevelGroup>;
        checkAll: Function;
    }

    class SettingType8CheckboxGroupDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype8-checkbox-group.htm";

        public get scope(): any {
            return {
                items: "="
            };
        }

        public controller: any = ["$scope", ($scope: ICheckboxGroupScope) => {
            $scope.checked = this.checkItemsState($scope.items);

            $scope.checkAll = () => {
                this.changeStateOfAllItems($scope.items, $scope.checked);
            };

            $scope.$watch("items", () => {
                $scope.checked = this.checkItemsState($scope.items);
            }, true);
        }];

        private changeStateOfAllItems(items: Array<SettingType8StakeLevelGroup>, state: boolean): void {
            if (items) {
                items.forEach((item: SettingType8StakeLevelGroup) => {
                    item.selected = state;
                });
            }
        }

        private checkItemsState(items: Array<SettingType8StakeLevelGroup>): boolean {
            var allChecked: boolean = false;

            if (items) {
                if (items.length > 0) {
                    allChecked = items.every((item: SettingType8StakeLevelGroup) => {
                        return item.selected;
                    });
                }
            }

            return allChecked;
        }
    }

    ModuleRegister.directive("settingType8CheckboxGroup", SettingType8CheckboxGroupDirective);
}