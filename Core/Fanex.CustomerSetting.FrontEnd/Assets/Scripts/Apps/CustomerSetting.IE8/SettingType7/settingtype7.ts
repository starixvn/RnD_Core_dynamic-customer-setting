/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype7-resource.ts" />

module CustomerSetting.IE8 {
    class SettingType7Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype7.htm";

        public scope: any = {
            settingType: "=",
            update: "&"
        };

        public controller: any = [
            "$scope",
            "settingTypesSharedValuesService",
            "settingType7ResourceService",
            (
                $scope: IDefaultSettingTypeDirectiveScope,
                settingTypesSharedValuesService: SettingTypesSharedValuesService,
                settingType7ResourceService: SettingType7ResourceService
            ) => {
                $scope.customerId = settingTypesSharedValuesService.customerId;
                $scope.uplineLevel = settingTypesSharedValuesService.uplineLevel;
                $scope.customerLevel = settingTypesSharedValuesService.customerLevel;
                $scope.productId = settingTypesSharedValuesService.productId;
                $scope.productName = settingTypesSharedValuesService.productName;
                $scope.avaiCustomerLevels = Level;
                $scope.avaiSettingLevels = SettingLevel;
                $scope.settingTypeName = settingType7ResourceService.getLabel("BetLimit");

                $scope.$watch("settingType", (settingType: SettingType) => {
                    $scope.normalMode = settingTypesSharedValuesService.normalMode;

                    if (settingType && angular.isArray(settingType.settingItems)) {
                        if (settingType.isError) {
                            if (settingType.settingItems.length > 0) {
                                settingType.settingItems[0] = new ErrorSettingItem(settingType.settingItems[0]);
                            }
                        } else {
                            _.forEach(settingType.settingItems, (item: any, index: number) => {
                                if (item) {
                                    settingType.settingItems[index] = new SettingType7Item(item, true, $scope.customerLevel);
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