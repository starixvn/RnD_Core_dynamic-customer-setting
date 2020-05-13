/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ICheckboxGroupScope extends ng.IScope {
        checked: boolean;
        items: Array<SettingType17Item | SettingType17StakeRange>;
        tooltipMessage: string;
        highlightState: string;
        checkAll: Function;
    }

    class SettingType17CheckboxGroupDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype17-checkbox-group.htm";

        public get scope(): any {
            return {
                items: "=",
                tooltipMessage: "@?",
                highlightState: "=?"
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

        private changeStateOfAllItems(items: Array<SettingType17Item | SettingType17StakeRange>, state: boolean): void {
            if (items) {
                items.forEach((item: SettingType17Item | SettingType17StakeRange) => {
                    if (item instanceof SettingType17Item) {
                        item.stakeRanges.forEach((stakeLevel: SettingType17StakeRange) => {
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

        private checkItemsState(items: Array<SettingType17Item | SettingType17StakeRange>): boolean {
            var allChecked: boolean = false;

            if (items) {
                items = items.filter((item: SettingType17Item | SettingType17StakeRange) => {
                    if (item instanceof SettingType17StakeRange) {
                        return !!item.stakeLevelId;
                    } else {
                        return true;
                    }
                });

                if (items.length > 0) {
                    allChecked = items.every((item: SettingType17Item | SettingType17StakeRange) => {
                        var itemChecked: boolean = false;

                        if (item && item instanceof SettingType17Item) {
                            itemChecked = item.stakeRanges.filter((stakeLevel: SettingType17StakeRange) => {
                                return !!stakeLevel.stakeLevelId;
                            }).every((stakeLevel: SettingType17StakeRange) => {
                                return stakeLevel.selected;
                            });
                        } else {
                            itemChecked = (<SettingType17StakeRange>item).selected;
                        }

                        return itemChecked;
                    });
                }
            }

            return allChecked;
        }
    }

    ModuleRegister.directive("settingType17CheckboxGroup", SettingType17CheckboxGroupDirective);
}