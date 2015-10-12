/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ICheckboxGroupScope extends ng.IScope {
        checked: boolean;
        items: Array<SettingType5Item | StakeLevel>;
        checkAll: Function;
    }

    class CheckboxGroupDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "checkbox-group.htm";

        public get scope(): any {
            return {
                items: "="
            };
        }

        public controller: any = ["$scope", ($scope: ICheckboxGroupScope) => {
            $scope.checkAll = () => {
                this.changeStateOfAllItems($scope.items, $scope.checked);
            };

            $scope.$watch("items", () => {
                $scope.checked = this.checkItemsState($scope.items);
            }, true);
        }];

        private changeStateOfAllItems(items: Array<SettingType5Item | StakeLevel>, state: boolean): void {
            if (items) {
                items.forEach((item: SettingType5Item | StakeLevel) => {
                    if (item instanceof SettingType5Item) {
                        item.selectedAll = state;
                    } else {
                        (<StakeLevel>item).selected = state;
                    }
                });
            }
        }

        private checkItemsState(items: Array<SettingType5Item | StakeLevel>): boolean {
            var checked: boolean = false;

            if (items) {
                checked = items.every((item: SettingType5Item | StakeLevel) => {
                    var allChecked: boolean = false;

                    if (item instanceof SettingType5Item) {
                        allChecked = item.selectedAll;
                    } else {
                        allChecked = (<StakeLevel>item).selected;
                    }

                    return allChecked;
                });
            }

            return checked;
        }
    }

    ModuleRegister.directive("checkboxGroup", CheckboxGroupDirective);
}