/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ICheckboxGroupScope extends ng.IScope {
        checked: boolean;
        items: Array<SettingType7Item | SettingType7StakeRange>;
        checkAll: Function;
    }

    class SettingType7CheckboxGroupDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype7-checkbox-group.htm";

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

        private changeStateOfAllItems(items: Array<SettingType7Item | SettingType7StakeRange>, state: boolean): void {
            if (items) {
                _.forEach(items, (item: SettingType7Item | SettingType7StakeRange) => {
                    if (item instanceof SettingType7Item) {
                        _.forEach(item.stakeRanges, (stakeLevel: SettingType7StakeRange) => {
                            if (stakeLevel.stakeLevelId) {
                                stakeLevel.selected = state;
                            }
                        });
                    } else {
                        item.selected = state;
                    }
                });
            }
        }

        private checkItemsState(items: Array<SettingType7Item | SettingType7StakeRange>): boolean {
            var allChecked: boolean = false;

            if (items) {
                items = _.filter(items, (item: SettingType7Item | SettingType7StakeRange) => {
                    if (item instanceof SettingType7StakeRange) {
                        return !!item.stakeLevelId;
                    } else {
                        return true;
                    }
                });

                if (items.length > 0) {
                    allChecked = _.every(items, (item: SettingType7Item | SettingType7StakeRange) => {
                        var itemChecked: boolean = false;

                        if (item && item instanceof SettingType7Item) {
                            itemChecked = _.every(_.filter(item.stakeRanges, (stakeLevel: SettingType7StakeRange) => {
                                return !!stakeLevel.stakeLevelId;
                            }), (stakeLevel: SettingType7StakeRange) => {
                                return stakeLevel.selected;
                            });
                        } else {
                            itemChecked = (<SettingType7StakeRange>item).selected;
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