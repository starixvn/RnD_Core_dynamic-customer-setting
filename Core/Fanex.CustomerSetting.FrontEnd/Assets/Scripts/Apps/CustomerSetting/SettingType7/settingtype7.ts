/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype7-resource.ts" />

module CustomerSetting {
    class SettingType7Directive extends DefaultSettingTypeGroupingDirective {
        public static $inject: string[] = ["settingType7ResourceService"];

        public replace: boolean = true;
        public templateUrl: string = "settingtype7.htm";

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

        public controller: any = ["$scope", ($scope: IDefaultSettingTypeDirectiveScope) => {
            $scope.avaiCustomerLevels = Level;
            $scope.avaiSettingLevels = SettingLevel;
            $scope.settingTypeName = this.settingType7ResourceService.getLabel("BetLimit");

            $scope.$watch("settingType", (settingType: SettingType) => {
                if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                    if (settingType.isError) {
                        if (settingType.settingItems.length > 0) {
                            settingType.settingItems[0] = new ErrorSettingItem(settingType.settingItems[0]);
                        }
                    } else {
                        settingType.settingItems.forEach((item: any, index: number) => {
                            if (angular.isDefined(item)) {
                                settingType.settingItems[index] = new SettingType7Item(item, $scope.customerLevel, true);
                            }
                        });

                        if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                            $scope.sportGroups = this.groupSettingItemsBySportType(settingType.settingItems);
                        } else {
                            $scope.sportGroups = <DefaultSettingItem[][]>[settingType.settingItems];
                        }
                    }
                }
            });
        }];

        private settingType7ResourceService: SettingType7ResourceService;

        constructor(settingType7ResourceService: SettingType7ResourceService) {
            super();
            this.settingType7ResourceService = settingType7ResourceService;
        }
    }

    ModuleRegister.directive("settingtype7", SettingType7Directive);
}