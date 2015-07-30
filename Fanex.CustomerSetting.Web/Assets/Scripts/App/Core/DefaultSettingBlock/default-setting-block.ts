/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    export interface SettingTypeDirectiveSharedScope extends ng.IScope {
        productId: number;
        productName: string;
        settingType: SettingType;
        update: (settingTypeId: { settingTypeId: number }) => void;
        headerTemplateUrl: string;
        valueTemplateUrl: string;
    }

    export class SettingTypeDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                productId: "=",
                productName: "=",
                settingType: "=",
                update: "&",
                headerTemplateUrl: "=",
                valueTemplateUrl: "="
            }
        }

        public controller: any = ["$scope", "$rootScope", ($scope: SettingTypeDirectiveSharedScope, $rootScope: ng.IRootScopeService) => {
            $scope.$watch("settingType", (settingType: SettingType) => {
                if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                    angular.forEach(settingType.settingItems, (item, index) => {
                        if (angular.isDefined(item)) {
                            settingType.settingItems[index] = new this.settingItemType(item);
                        }
                    });

                    console.log(settingType.settingItems[0]);
                }

                $rootScope.$emit("customerSetting.settingTypeChanged");
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
            "$rootScope",
            "settingLevel",
            ($scope: DefaultSettingTypeDirectiveScope, $rootScope: ng.IRootScopeService, settingLevel: SettingLevel) => {
                $scope.settingLevel = settingLevel;

                $rootScope.$on("customerSetting.settingTypeChanged", () => {
                    if (angular.isDefined($scope.settingType) && angular.isArray($scope.settingType.settingItems)) {
                        if ($scope.settingType.settingLevel === SettingLevel.sportTypeBetType) {
                            $scope.sportGroups = this.groupSettingItemsBySportType($scope.settingType.settingItems);
                        } else {
                            $scope.sportGroups = <DefaultSettingItem[][]>[$scope.settingType.settingItems];
                        }
                    }

                    $rootScope.$emit("customerSetting.settingItemsGrouped");
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