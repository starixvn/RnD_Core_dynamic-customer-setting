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

        public controller: any = ["$scope", "$rootScope", ($scope: IDefaultSettingTypeDirectiveScope, $rootScope: ng.IRootScopeService) => {
            $scope.avaiSettingLevels = SettingLevel;
            $scope.$watch("settingType", (settingType: SettingType) => {
                $scope.togglableSettingBlocks = settingType.isEnableCollapseExpand;
                if (settingType && angular.isArray(settingType.settingItems)) {
                    if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                        $scope.sportGroups = this.groupSettingItemsBySportType(settingType.settingItems);
                        if ($scope.togglableSettingBlocks) {
                            $scope.togglableSharedStates = new TogglableSettingBlockSharedStates($scope.sportGroups.length, settingType.isCollapsed);
                        }
                    } else {
                        $scope.sportGroups = <DefaultSettingItem[][]>[settingType.settingItems];
                        if ($scope.togglableSettingBlocks) {
                            $scope.togglableSharedStates = new TogglableSettingBlockSharedStates(1, settingType.isCollapsed);
                        }
                    }
                }
            });

            $rootScope.$on("checkedChanged", (event, args) => {
                if ($scope.settingType && angular.isArray($scope.settingType.settingItems) && $scope.settingType.id === args.settingTypeId) {
                    this.disableChildByParentState($scope);
                }
            })
        }];

        private disableChildByParentState($scope: IDefaultSettingTypeDirectiveScope) {
            const parentGame = $scope.settingType.settingItems.find(e => e["parentBetTypeId"] === 0 && e["parentSportTypeId"] === 0);
            const childGames = $scope.settingType.settingItems.filter(e => e["parentBetTypeId"] === parentGame["betTypeId"] && e["parentSportTypeId"] === parentGame["sportTypeId"]);
            if (parentGame["isDisabled"]) {
                childGames.forEach(e => {
                    e["isDisabled"] = true;
                    e["readonly"] = true;
                })
            } else {
                childGames.forEach(e => {
                    e["readonly"] = false;
                })
            }
        }

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