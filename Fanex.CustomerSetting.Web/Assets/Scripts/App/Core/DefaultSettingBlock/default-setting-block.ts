/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    export interface ISettingTypeDirectiveSharedScope extends ng.IScope {
        uplineLevel: Level;
        customerLevel: Level;
        productId: number;
        productName: string;
        settingType: SettingType;
        settingTypeName: string;
        update: (settingTypeId: { settingTypeId: number }) => void;
        headerTemplateUrl: string;
        valueTemplateUrl: string;
        avaiCustomerLevels: Object;
    }

    export class SettingTypeDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                uplineLevel: "=",
                customerLevel: "=",
                productId: "=",
                productName: "=",
                settingType: "=",
                update: "&"
            };
        }

        public controller: any = ["$scope", ($scope: ISettingTypeDirectiveSharedScope) => {
            $scope.avaiCustomerLevels = Level;

            $scope.$watch("settingType", (settingType: SettingType) => {
                if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                    angular.forEach(settingType.settingItems, (item: ISettingItem, index: number) => {
                        if (angular.isDefined(item)) {
                            settingType.settingItems[index] = this.settingItemType(item, true, $scope.customerLevel, $scope.uplineLevel);
                        }
                    });
                }
            });
        }];

        private settingItemType: (settingItem: any, backup?: boolean, customerLevel?: Level, uplineLevel?: Level) => ISettingItem;

        constructor(settingItemType: (settingItem: any, backup?: boolean, customerLevel?: Level, uplineLevel?: Level) => ISettingItem) {
            this.settingItemType = settingItemType;
        }
    }

    interface IDefaultSettingTypeDirectiveScope extends ISettingTypeDirectiveSharedScope {
        avaiSettingLevels: Object;
        sportGroups: DefaultSettingItem[][];
    }

    class DefaultSettingTypeGroupingDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "DefaultSettingBlock/default-setting-block.htm";

        public controller: any = ["$scope", ($scope: IDefaultSettingTypeDirectiveScope) => {
            $scope.avaiSettingLevels = SettingLevel;

            $scope.$watch("settingType", (settingType: SettingType) => {
                if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                    if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                        $scope.sportGroups = this.groupSettingItemsBySportType(settingType.settingItems);
                    } else {
                        $scope.sportGroups = <DefaultSettingItem[][]>[settingType.settingItems];
                    }
                }
            });
        }];

        private groupSettingItemsBySportType(settingItems: ISettingItem[]): DefaultSettingItem[][] {
            var currentSportTypeId: number = -1;
            var result: Array<Array<DefaultSettingItem>> = new Array<Array<DefaultSettingItem>>();
            var group: Array<DefaultSettingItem>;

            angular.forEach(settingItems, (item: ISettingItem) => {
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
}