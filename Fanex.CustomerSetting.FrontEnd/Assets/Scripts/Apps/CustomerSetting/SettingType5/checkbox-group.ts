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
            $scope.checked = this.checkItemsState($scope.items);

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
                        item.stakeLevels.forEach((stakeLevel: StakeLevel) => {
                            if (stakeLevel.stakeLevelId) {
                                stakeLevel.selected = state;
                            }
                        });
                    } else {
                        (<StakeLevel>item).selected = state;
                    }
                });
            }
        }

        private checkItemsState(items: Array<SettingType5Item | StakeLevel>): boolean {
            var allChecked: boolean = false;

            if (items) {
                items = items.filter((item: SettingType5Item | StakeLevel) => {
                    if (item instanceof StakeLevel) {
                        return !!item.stakeLevelId;
                    } else {
                        return true;
                    }
                });

                if (items.length > 0) {
                    allChecked = items.every((item: SettingType5Item | StakeLevel) => {
                        var itemChecked: boolean = false;

                        if (item && item instanceof SettingType5Item) {
                            itemChecked = item.stakeLevels.filter((stakeLevel: StakeLevel) => {
                                return !!stakeLevel.stakeLevelId;
                            }).every((stakeLevel: StakeLevel) => {
                                return stakeLevel.selected;
                            });
                        } else {
                            itemChecked = (<StakeLevel>item).selected;
                        }

                        return itemChecked;
                    });
                }
            }

            return allChecked;
        }
    }

    ModuleRegister.directive("checkboxGroup", CheckboxGroupDirective);
}