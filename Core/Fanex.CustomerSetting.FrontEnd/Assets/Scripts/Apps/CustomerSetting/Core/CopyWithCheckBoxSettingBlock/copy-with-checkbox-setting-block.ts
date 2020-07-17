/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    export interface ICopyWithCheckboxSettingTypeDirectiveScope extends ISettingTypeDirectiveSharedScope {
        avaiSettingLevels: Object;
        sportGroups: CopyWithCheckboxSettingItem[][];
        togglableSettingBlocks: boolean;
        togglableSharedStates: TogglableSettingBlockSharedStates;
        isAutoChanged: (item: CopyWithCheckboxSettingItem, fieldName: string, isFirst: boolean) => void;
    }

    export class CopyWithCheckboxSettingTypeDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "CopyWithCheckBoxSettingBlock/copy-with-checkbox-setting-block.htm";

        public controller: any = ["$scope", "$timeout", ($scope: ICopyWithCheckboxSettingTypeDirectiveScope, $timeout: ng.ITimeoutService) => {
            $scope.avaiSettingLevels = SettingLevel;

            $scope.$watch("settingType", (settingType: SettingType) => {
                if (settingType && angular.isArray(settingType.settingItems)) {
                    if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                        $scope.sportGroups = this.groupSettingItemsBySportType(settingType.settingItems);

                        if ($scope.togglableSettingBlocks) {
                            $scope.togglableSharedStates = new TogglableSettingBlockSharedStates($scope.sportGroups.length);
                        }
                    } else {
                        $scope.sportGroups = <CopyWithCheckboxSettingItem[][]>[settingType.settingItems];
                    }
                }
            });

            $scope.isAutoChanged = ((item: CopyWithCheckboxSettingItem, fieldName: string, isFirst: boolean) => {
                item.resetFieldHighlightState(fieldName);

                $timeout(() => {
                    if (isFirst && item.isCheckedUpdateRow) {
                        $scope.$emit("copySettingTypeWithCheckbox");
                    }
                }, 100);
            });
        }];

        protected groupSettingItemsBySportType(settingItems: ISettingItem[]): CopyWithCheckboxSettingItem[][] {
            var currentSportTypeId: number = -1;
            var result: Array<Array<CopyWithCheckboxSettingItem>> = new Array<Array<CopyWithCheckboxSettingItem>>();
            var group: Array<CopyWithCheckboxSettingItem>;

            settingItems.forEach((item: ISettingItem) => {
                var settingItem = <CopyWithCheckboxSettingItem>item;

                if (currentSportTypeId !== settingItem.sportTypeId) {
                    if (group) {
                        result.push(group);
                    }

                    currentSportTypeId = settingItem.sportTypeId;
                    group = new Array<CopyWithCheckboxSettingItem>();
                }

                group.push(settingItem);
            });

            if (group) {
                result.push(group);
            }

            return result;
        }
    }

    ModuleRegister.directive("copyWithCheckboxSettingType", CopyWithCheckboxSettingTypeDirective);

    function ShowingName(): Function {
        return (settingItem: CopyWithCheckboxSettingItem, settingLevel: SettingLevel, productName: string) => {
            return settingLevel === SettingLevel.product
                ? productName
                : (settingLevel === SettingLevel.sportType
                    ? settingItem.sportTypeName
                    : settingItem.betTypeName);
        };
    }

    ModuleRegister.filter("showingName", ShowingName);
}