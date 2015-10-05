/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
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

        public controller: any = ["$scope", ($scope: IDefaultSettingTypeDirectiveScope) => {
            $scope.avaiCustomerLevels = Level;
            $scope.avaiSettingLevels = SettingLevel;

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

            if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                groups = this.groupSettingItemsBySportType(settingType.settingItems);
            } else {
                groups = <DefaultSettingItem[][]>[settingType.settingItems];
            }

            groups.forEach((group: DefaultSettingItem[]) => {
                var mod: number = group.length % this.numberOfCellsOnEachRow;
                var numberOfEmptyCells: number = 0;

                if (mod !== 0) {
                    numberOfEmptyCells = this.numberOfCellsOnEachRow - mod;
                }

                if (numberOfEmptyCells > 0) {
                    for (var counter = 0; counter < numberOfEmptyCells; counter++) {
                        group.push(new DefaultSettingItem());
                    }
                }
            });

            return groups;
        }
    }

    ModuleRegister.directive("settingtype5", SettingType5Directive);
}