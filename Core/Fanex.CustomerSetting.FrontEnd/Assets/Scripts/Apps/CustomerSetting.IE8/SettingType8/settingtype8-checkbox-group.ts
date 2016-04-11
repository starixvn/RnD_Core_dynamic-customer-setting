/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ICheckboxGroupScope extends ng.IScope {
        checked: boolean;
        items: Array<SettingType8LevelGroup>;
        checkAll: Function;
    }

    class SettingType8CheckboxGroupDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype8-checkbox-group.htm";

        public scope: any = {
            items: "="
        };

        public controller: any = ["$scope", ($scope: ICheckboxGroupScope) => {
            $scope.checked = this.checkItemsState($scope.items);

            $scope.checkAll = () => {
                this.changeStateOfAllItems($scope.items, $scope.checked);
            };

            $scope.$watch("items", () => {
                $scope.checked = this.checkItemsState($scope.items);
            }, true);
        }];

        private changeStateOfAllItems(items: Array<SettingType8LevelGroup>, state: boolean): void {
            if (items) {
                _.forEach(items, (item: SettingType8LevelGroup) => {
                    item.selected = state;
                });
            }
        }

        private checkItemsState(items: Array<SettingType8LevelGroup>): boolean {
            var allChecked: boolean = false;

            if (items) {
                if (items.length > 0) {
                    allChecked = _.every(items, (item: SettingType8LevelGroup) => {
                        return item.selected;
                    });
                }
            }

            return allChecked;
        }
    }

    ModuleRegister.directive("settingType8CheckboxGroup", SettingType8CheckboxGroupDirective);
}