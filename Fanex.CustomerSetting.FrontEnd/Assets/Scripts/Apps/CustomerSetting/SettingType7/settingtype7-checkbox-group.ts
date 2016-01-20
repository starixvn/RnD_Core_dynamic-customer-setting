/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ICheckboxGroupScope extends ng.IScope {
        checked: boolean;
        items: Array<SettingType7Item | SettingType7StakeLevelValue>;
        checkAll: Function;
    }

    class SettingType7CheckboxGroupDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype7-checkbox-group.htm";

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

        private changeStateOfAllItems(items: Array<SettingType7Item | SettingType7StakeLevelValue>, state: boolean): void {
            if (items) {
                items.forEach((item: SettingType7Item | SettingType7StakeLevelValue) => {
                    if (item instanceof SettingType7Item) {
                        item.stakeLevelValues.forEach((stakeLevel: SettingType7StakeLevelValue) => {
                            if (stakeLevel.stakeLevelId) {
                                stakeLevel.selected = state;
                            }
                        });
                    } else {
                        (<SettingType7StakeLevelValue>item).selected = state;
                    }
                });
            }
        }

        private checkItemsState(items: Array<SettingType7Item | SettingType7StakeLevelValue>): boolean {
            var allChecked: boolean = false;

            if (items) {
                items = items.filter((item: SettingType7Item | SettingType7StakeLevelValue) => {
                    if (item instanceof SettingType7StakeLevelValue) {
                        return !!item.stakeLevelId;
                    } else {
                        return true;
                    }
                });

                if (items.length > 0) {
                    allChecked = items.every((item: SettingType7Item | SettingType7StakeLevelValue) => {
                        var itemChecked: boolean = false;

                        if (item && item instanceof SettingType7Item) {
                            itemChecked = item.stakeLevelValues.filter((stakeLevel: SettingType7StakeLevelValue) => {
                                return !!stakeLevel.stakeLevelId;
                            }).every((stakeLevel: SettingType7StakeLevelValue) => {
                                return stakeLevel.selected;
                            });
                        } else {
                            itemChecked = (<SettingType7StakeLevelValue>item).selected;
                        }

                        return itemChecked;
                    });
                }
            }

            return allChecked;
        }
    }

    ModuleRegister.directive("settingType7CheckboxGroup", SettingType7CheckboxGroupDirective);
}