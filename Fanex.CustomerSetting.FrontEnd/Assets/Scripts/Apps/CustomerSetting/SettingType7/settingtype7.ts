/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    interface ISettingType7Scope extends IDefaultSettingTypeDirectiveScope {
        numberOfCellsOnEachRow: number;
    }

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
                update: "&"
            };
        }

        public controller: any = ["$scope", ($scope: ISettingType7Scope) => {
            $scope.avaiCustomerLevels = Level;
            $scope.avaiSettingLevels = SettingLevel;
            $scope.numberOfCellsOnEachRow = this.numberOfCellsOnEachRow;
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

                        $scope.sportGroups = this.groupAndAddEmptyItems($scope.settingType);
                    }
                }
            });
        }];

        private settingType7ResourceService: SettingType7ResourceService;

        constructor(settingType7ResourceService: SettingType7ResourceService) {
            super();
            this.settingType7ResourceService = settingType7ResourceService;
        }

        private numberOfCellsOnEachRow: number = 5;

        private groupAndAddEmptyItems(settingType: SettingType): DefaultSettingItem[][] {
            var groups: DefaultSettingItem[][];

            if (settingType) {
                settingType.settingItems.forEach((settingItem: SettingType7Item) => {
                    var mod: number = settingItem.stakeLevelValues.length % this.numberOfCellsOnEachRow;
                    var numberOfEmptyCells: number = 0;

                    if (mod !== 0 || settingItem.stakeLevelValues.length === 0) {
                        numberOfEmptyCells = this.numberOfCellsOnEachRow - mod;
                    }

                    for (var counter = 0; counter < numberOfEmptyCells; counter++) {
                        settingItem.stakeLevelValues.push(new SettingType7StakeLevelValue());
                    }
                });

                if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                    groups = this.groupSettingItemsBySportType(settingType.settingItems);
                } else {
                    groups = <DefaultSettingItem[][]>[settingType.settingItems];
                }
            }

            return groups;
        }
    }

    ModuleRegister.directive("settingtype7", SettingType7Directive);
}