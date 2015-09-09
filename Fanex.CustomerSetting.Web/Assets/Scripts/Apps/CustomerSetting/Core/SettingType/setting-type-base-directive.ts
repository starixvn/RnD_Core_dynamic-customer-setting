/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../models.ts" />

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
                    settingType.settingItems.forEach((item: ISettingItem, index: number) => {
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
}