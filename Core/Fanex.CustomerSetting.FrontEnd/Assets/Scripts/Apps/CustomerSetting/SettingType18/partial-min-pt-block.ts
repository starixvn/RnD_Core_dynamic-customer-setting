/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="models.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/defaultsettingblock/togglesettingblock/togglable-setting-block-shared-states.ts" />

module CustomerSetting {
    "use strict";

    export interface IPartialMinPTSettingTypeDirectiveScope extends ISettingTypeDirectiveSharedScope {
        avaiSettingLevels: Object;
        sportGroups: PositionTalkingPartialMinPt[][];
        togglableSettingBlocks: boolean;
        togglableSharedStates: TogglableSettingBlockSharedStates;
        isMultiple: boolean;
    }

    export class PartialMinPtSettingTypeDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "partial-min-pt-block.htm";

        public controller: any = ["$scope", ($scope: IPartialMinPTSettingTypeDirectiveScope) => {
            $scope.avaiSettingLevels = SettingLevel;

            $scope.$watch("settingType", (settingType: SettingType) => {
                if (settingType && angular.isArray(settingType.settingItems)) {
                    if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                        $scope.sportGroups = this.groupSettingItemsBySportType(settingType.settingItems);

                        if ($scope.togglableSettingBlocks) {
                            $scope.togglableSharedStates = new TogglableSettingBlockSharedStates($scope.sportGroups.length);
                        }
                    } else {
                        $scope.sportGroups = <PositionTalkingPartialMinPt[][]>[settingType.settingItems];
                    }
                }
            });
        }];

        protected groupSettingItemsBySportType(settingItems: ISettingItem[]): PositionTalkingPartialMinPt[][] {
            var currentSportTypeId: number = -1;
            var result: Array<Array<PositionTalkingPartialMinPt>> = new Array<Array<PositionTalkingPartialMinPt>>();
            var group: Array<PositionTalkingPartialMinPt>;

            settingItems.forEach((item: ISettingItem) => {
                var settingItem = <PositionTalkingPartialMinPt>item;

                if (currentSportTypeId !== settingItem.sportTypeId) {
                    if (group) {
                        result.push(group);
                    }

                    currentSportTypeId = settingItem.sportTypeId;
                    group = new Array<PositionTalkingPartialMinPt>();
                }

                group.push(settingItem);
            });

            if (group) {
                result.push(group);
            }

            return result;
        }
    }

    ModuleRegister.directive("partialMinPtSettingType", PartialMinPtSettingTypeDirective);

    function ShowingName(): Function {
        return (settingItem: PositionTalkingPartialMinPt, settingLevel: SettingLevel, productName: string) => {
            return settingLevel === SettingLevel.product
                ? productName
                : (settingLevel === SettingLevel.sportType
                    ? settingItem.sportTypeName
                    : settingItem.betTypeName);
        };
    }

    ModuleRegister.filter("showingName", ShowingName);
}