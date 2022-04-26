/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype7-resource.ts" />

module CustomerSetting {
    class SettingType7Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype7.htm";

        public get scope(): any {
            return {
                settingType: "=",
                update: "&"
            };
        }

        public controller: any = [
            "$scope",
            "settingTypesSharedValuesService",
            "parametersService",
            "settingType7ResourceService",
            (
                $scope: IDefaultSettingTypeDirectiveScope,
                settingTypesSharedValuesService: SettingTypesSharedValuesService,
                parametersService: ParametersService,
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

                $scope.togglableSettingBlocks = true;
                $scope.$watch("settingType", (settingType: SettingType) => {
                    $scope.normalMode = settingTypesSharedValuesService.normalMode;

                    if (settingType && angular.isArray(settingType.settingItems)) {
                        if (settingType.isError) {
                            if (settingType.settingItems.length > 0) {
                                settingType.settingItems[0] = new ErrorSettingItem(settingType.settingItems[0]);
                            }
                        } else {
                            settingType.settingItems.forEach((item: any, index: number) => {
                                if (item) {
                                    settingType.settingItems[index] = new SettingType7Item(
                                        item,
                                        true,
                                        $scope.customerLevel,
                                        parametersService.systemId);
                                }
                            });

                            if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                                $scope.sportGroups = this.groupSettingItemsBySportType(settingType.settingItems);
                                if ($scope.togglableSettingBlocks) {
                                    $scope.togglableSharedStates = new TogglableSettingBlockSharedStates($scope.sportGroups.length, settingType.settingLevel === SettingLevel.sportTypeBetType);
                                }
                            } else {
                                $scope.sportGroups = <DefaultSettingItem[][]>[settingType.settingItems];
                                if ($scope.togglableSettingBlocks) {
                                    $scope.togglableSharedStates = new TogglableSettingBlockSharedStates(1, settingType.settingLevel === SettingLevel.sportTypeBetType);
                                }
                            }
                        }
                    }
                });
            }];
    }

    ModuleRegister.directive("settingtype7", SettingType7Directive);
}