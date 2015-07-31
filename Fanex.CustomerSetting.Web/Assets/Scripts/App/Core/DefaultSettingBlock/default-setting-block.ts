/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    export interface SettingTypeDirectiveSharedScope extends ng.IScope {
        uplineLevel: Level;
        customerLevel: Level;
        productId: number;
        productName: string;
        settingType: SettingType;
        update: (settingTypeId: { settingTypeId: number }) => void;
        headerTemplateUrl: string;
        valueTemplateUrl: string;
        levelIdentifier: Level;
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
                update: "&",
                headerTemplateUrl: "=",
                valueTemplateUrl: "=",
                levelIdentifier: "="
            }
        }

        public controller: any = ["$scope", "levelIdentifier", ($scope: SettingTypeDirectiveSharedScope, levelIdentifier: Level) => {
            $scope.levelIdentifier = levelIdentifier;
            $scope.$watch("settingType", (settingType: SettingType) => {
                if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                    angular.forEach(settingType.settingItems, (item, index) => {
                        if (angular.isDefined(item)) {
                            settingType.settingItems[index] = new this.settingItemType(item);
                        }
                    });
                }
            });
        }];

        constructor(private settingItemType: new (settingItem: any) => SettingItem) { }
    }

    interface DefaultSettingTypeDirectiveScope extends SettingTypeDirectiveSharedScope {
        settingLevel: SettingLevel;
        sportGroups: DefaultSettingItem[][];
    }

    class DefaultSettingTypeDirective extends SettingTypeDirective {
        public templateUrl: string = "DefaultSettingBlock/default-setting-block.htm";

        public controller: any = [
            "$scope",
            "settingLevel",
            ($scope: DefaultSettingTypeDirectiveScope, settingLevel: SettingLevel) => {
                $scope.settingLevel = settingLevel;

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

        private groupSettingItemsBySportType(settingItems: SettingItem[]): DefaultSettingItem[][] {
            var currentSportTypeId: number = -1;
            var result: Array<Array<DefaultSettingItem>> = new Array<Array<DefaultSettingItem>>();
            var group: Array<DefaultSettingItem>;

            angular.forEach(settingItems, (item, index) => {
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

    ModuleRegister.directive("defaultSettingTypeBlock", DefaultSettingTypeDirective);
}