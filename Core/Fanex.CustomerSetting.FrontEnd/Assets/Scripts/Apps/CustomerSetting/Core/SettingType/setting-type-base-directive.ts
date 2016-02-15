/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../models.ts" />

module CustomerSetting {
    "use strict";

    interface INewSettingItem {
        (settingItem: any, backup?: boolean, customerLevel?: Level, uplineLevel?: Level, extraData?: Array<KeyValuePair<string, any>>): ISettingItem;
    }

    export interface ISettingTypeDirectiveSharedScope extends ng.IScope {
        uplineLevel: Level;
        customerLevel: Level;
        productId: number;
        productName: string;
        settingType: SettingType;
        settingTypeName: string;
        normalMode: boolean;
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
                normalMode: "=",
                update: "&"
            };
        }

        public controller: any = ["$scope", ($scope: ISettingTypeDirectiveSharedScope) => {
            $scope.avaiCustomerLevels = Level;

            $scope.$watch("settingType", (settingType: SettingType) => {
                if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                    if (settingType.isError) {
                        if (settingType.settingItems.length > 0) {
                            settingType.settingItems[0] = new ErrorSettingItem(settingType.settingItems[0]);
                        }
                    } else {
                        settingType.settingItems.forEach((item: any, index: number) => {
                            if (angular.isDefined(item)) {
                                settingType.settingItems[index] = this.settingItemType(
                                    item,
                                    true,
                                    $scope.customerLevel,
                                    $scope.uplineLevel,
                                    settingType.extraData);
                            }
                        });
                    }
                }
            });
        }];

        private settingItemType: INewSettingItem;

        constructor(settingItemType: INewSettingItem) {
            this.settingItemType = settingItemType;
        }
    }
}