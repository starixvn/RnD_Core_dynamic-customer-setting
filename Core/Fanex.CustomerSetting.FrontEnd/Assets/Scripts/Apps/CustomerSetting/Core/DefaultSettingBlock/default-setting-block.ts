/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />
/// <reference path="../settingtype/setting-type-base-directive.ts" />
/// <reference path="togglesettingblock/togglable-setting-block-shared-states.ts" />

module CustomerSetting {
    "use strict";

    export interface IDefaultSettingTypeDirectiveScope extends ISettingTypeDirectiveSharedScope {
        avaiSettingLevels: Object;
        sportGroups: DefaultSettingItem[][];
        togglableSettingBlocks: boolean;
        togglableSharedStates: TogglableSettingBlockSharedStates;
        isMultiple: boolean;
    }

    export class DefaultSettingTypeGroupingDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "DefaultSettingBlock/default-setting-block.htm";

        public controller: any = ["$scope", ($scope: IDefaultSettingTypeDirectiveScope) => {
            $scope.avaiSettingLevels = SettingLevel;
            $scope.togglableSettingBlocks = true;
            $scope.$watch("settingType", (settingType: SettingType) => {
                if (settingType && angular.isArray(settingType.settingItems)) {

                    if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                        $scope.sportGroups = this.groupSettingItemsBySportType(settingType.settingItems);
                        if ($scope.togglableSettingBlocks) {
                            $scope.togglableSharedStates = new TogglableSettingBlockSharedStates($scope.sportGroups.length);
                        }
                    } else {
                        $scope.sportGroups = <DefaultSettingItem[][]>[settingType.settingItems];
                        if ($scope.togglableSettingBlocks) {
                            $scope.togglableSharedStates = new TogglableSettingBlockSharedStates(1);
                        }
                    }
                }
            });
        }];

        protected groupSettingItemsBySportType(settingItems: ISettingItem[]): DefaultSettingItem[][] {
            var currentSportTypeId: number = -1;
            var result: Array<Array<DefaultSettingItem>> = new Array<Array<DefaultSettingItem>>();
            var group: Array<DefaultSettingItem>;

            settingItems.forEach((item: ISettingItem) => {
                var settingItem = <DefaultSettingItem>item;

                if (currentSportTypeId !== settingItem.sportTypeId) {
                    if (group) {
                        result.push(group);
                    }

                    currentSportTypeId = settingItem.sportTypeId;
                    group = new Array<DefaultSettingItem>();
                }

                group.push(settingItem);
            });

            if (group) {
                result.push(group);
            }

            return result;
        }
    }

    ModuleRegister.directive("defaultSettingTypeGrouping", DefaultSettingTypeGroupingDirective);

    function ShowingName(): Function {
        return (settingItem: DefaultSettingItem, settingLevel: SettingLevel, productName: string) => {
            return settingLevel === SettingLevel.product
                ? productName
                : (settingLevel === SettingLevel.sportType
                    ? settingItem.sportTypeName
                    : settingItem.betTypeName);
        };
    }

    ModuleRegister.filter("showingName", ShowingName);
}