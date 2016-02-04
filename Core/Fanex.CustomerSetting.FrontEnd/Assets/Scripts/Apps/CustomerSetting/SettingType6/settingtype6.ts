/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype6-resource.ts" />

module CustomerSetting {
    interface ISettingType7DirectviceScope extends IDefaultSettingTypeDirectiveScope {
        getCurrentWinLoss: (settingItem: SettingType6Item) => void;
    }

    class SettingType6Directive extends DefaultSettingTypeGroupingDirective {
        public static $inject: string[] = ["settingType6ResourceService"];

        public replace: boolean = true;
        public templateUrl: string = "settingtype6.htm";

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

        public controller: any = ["$scope", ($scope: ISettingType7DirectviceScope) => {
            $scope.avaiCustomerLevels = Level;
            $scope.avaiSettingLevels = SettingLevel;
            $scope.settingTypeName = this.settingType6ResourceService.getLabel("WinLossLimit");

            $scope.$watch("settingType", (settingType: SettingType) => {
                if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                    if (settingType.isError) {
                        if (settingType.settingItems.length > 0) {
                            settingType.settingItems[0] = new ErrorSettingItem(settingType.settingItems[0]);
                        }
                    } else {
                        settingType.settingItems.forEach((item: any, index: number) => {
                            if (angular.isDefined(item)) {
                                settingType.settingItems[index] = new SettingType6Item(item, true);
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

        private settingType6ResourceService: SettingType6ResourceService;

        constructor(settingType6ResourceService: SettingType6ResourceService) {
            super();
            this.settingType6ResourceService = settingType6ResourceService;
        }
    }

    ModuleRegister.directive("settingtype6", SettingType6Directive);

    function UnlimitedLimitFilter(settingType6ResourceService: SettingType6ResourceService) {
        return (value: any) => {
            return parseFloat(value) > 0 ? value : settingType6ResourceService.getLabel("Unlimited");
        };
    }

    UnlimitedLimitFilter.$inject = ["settingType6ResourceService"];
    ModuleRegister.filter("unlimitedLimit", UnlimitedLimitFilter);
}