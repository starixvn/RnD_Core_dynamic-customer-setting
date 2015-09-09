/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/underscore/underscore.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />
/// <reference path="../settingtype/setting-type-base-directive.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export interface IDefaultSettingTypeDirectiveScope extends ISettingTypeDirectiveSharedScope {
        avaiSettingLevels: Object;
        sportGroups: DefaultSettingItem[][];
    }

    export class DefaultSettingTypeGroupingDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "DefaultSettingBlock/default-setting-block.htm";

        public controller: any = ["$scope", ($scope: IDefaultSettingTypeDirectiveScope) => {
            $scope.avaiSettingLevels = SettingLevel;

            $scope.$watch("settingType", (settingType: SettingType) => {
                if (angular.isDefined(settingType) && angular.isDefined(settingType.settingItems) && angular.isArray(settingType.settingItems)) {
                    if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                        $scope.sportGroups = this.groupSettingItemsBySportType(settingType.settingItems);
                    } else {
                        $scope.sportGroups = <DefaultSettingItem[][]>[settingType.settingItems];
                    }
                }
            });
        }];

        protected groupSettingItemsBySportType(settingItems: ISettingItem[]): DefaultSettingItem[][] {
            var currentSportTypeId: number = -1;
            var result: Array<Array<DefaultSettingItem>> = new Array<Array<DefaultSettingItem>>();
            var group: Array<DefaultSettingItem>;

            _.forEach(settingItems, (item: ISettingItem) => {
                var settingItem = <DefaultSettingItem>item;

                if (currentSportTypeId !== settingItem.sportTypeId) {
                    if (angular.isDefined(group)) {
                        result.push(group);
                    }

                    currentSportTypeId = settingItem.sportTypeId;
                    group = new Array<DefaultSettingItem>();
                }

                group.push(settingItem);
            });

            if (angular.isDefined(group)) {
                result.push(group);
            }

            return result;
        }
    }

    ModuleRegister.directive("defaultSettingTypeGrouping", DefaultSettingTypeGroupingDirective);

    function ShowingName() {
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