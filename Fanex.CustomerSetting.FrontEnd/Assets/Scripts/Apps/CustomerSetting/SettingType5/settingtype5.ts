/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    interface ISettingType5Scope extends IDefaultSettingTypeDirectiveScope {
        numberOfCellsOnEachRow: number;
    }

    class SettingType5Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype5.htm";

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

        public controller: any = ["$scope", ($scope: ISettingType5Scope) => {
            $scope.avaiCustomerLevels = Level;
            $scope.avaiSettingLevels = SettingLevel;
            $scope.numberOfCellsOnEachRow = this.numberOfCellsOnEachRow;

            $scope.$watch("settingType", (settingType: SettingType) => {
                if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                    settingType.settingItems.forEach((item: ISettingItem, index: number) => {
                        if (angular.isDefined(item)) {
                            settingType.settingItems[index] = new SettingType5Item(item, true);
                        }

                        $scope.sportGroups = this.groupAndAddEmptyItems($scope.settingType);
                    });
                }
            });
        }];

        private numberOfCellsOnEachRow: number = 5;

        private groupAndAddEmptyItems(settingType: SettingType): DefaultSettingItem[][] {
            var groups: DefaultSettingItem[][];

            if (settingType) {
                settingType.settingItems.forEach((settingItem: SettingType5Item) => {
                    var mod: number = settingItem.stakeLevels.length % this.numberOfCellsOnEachRow;
                    var numberOfEmptyCells: number = 0;

                    if (mod !== 0 || settingItem.stakeLevels.length === 0) {
                        numberOfEmptyCells = this.numberOfCellsOnEachRow - mod;
                    }

                    for (var counter = 0; counter < numberOfEmptyCells; counter++) {
                        settingItem.stakeLevels.push(new StakeLevel());
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

    ModuleRegister.directive("settingtype5", SettingType5Directive);
}