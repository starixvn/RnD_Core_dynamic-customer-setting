/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype17-resource.ts" />

module CustomerSetting {
    class SettingType17Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype17.htm";

        public get scope(): any {
            return {
                settingType: "=",
                update: "&",
                index: "@"
            };
        }

        public controller: any = [
            "$scope",
            "settingTypesSharedValuesService",
            "parametersService",
            "settingType17ResourceService",
            (
                $scope: IDefaultSettingTypeDirectiveScope,
                settingTypesSharedValuesService: SettingTypesSharedValuesService,
                parametersService: ParametersService,
                settingType17ResourceService: SettingType17ResourceService
            ) => {
                $scope.customerId = settingTypesSharedValuesService.customerId;
                $scope.uplineLevel = settingTypesSharedValuesService.uplineLevel;
                $scope.customerLevel = settingTypesSharedValuesService.customerLevel;
                $scope.productId = settingTypesSharedValuesService.productId;
                $scope.productName = settingTypesSharedValuesService.productName;
                $scope.avaiCustomerLevels = Level;
                $scope.avaiSettingLevels = SettingLevel;
                $scope.settingTypeName = settingType17ResourceService.getLabel("BetLimit");

                $scope.$watch("settingType", (settingType: SettingType) => {
                    $scope.togglableSettingBlocks = settingType.isEnableCollapseExpand;
                    $scope.normalMode = settingTypesSharedValuesService.normalMode;

                    if (settingType && angular.isArray(settingType.settingItems)) {
                        if (settingType.isError) {
                            if (settingType.settingItems.length > 0) {
                                settingType.settingItems[0] = new ErrorSettingItem(settingType.settingItems[0]);
                            }
                        } else {
                            settingType.settingItems.forEach((item: any, index: number) => {
                                if (item) {
                                    settingType.settingItems[index] = new SettingType17Item(
                                        item,
                                        true,
                                        $scope.customerLevel,
                                        parametersService.systemId);
                                }
                            });

                            if (settingType.settingLevel === SettingLevel.sportTypeBetType) {
                                $scope.sportGroups = this.groupSettingItemsBySportType(settingType.settingItems);
                                if ($scope.togglableSettingBlocks) {
                                    $scope.togglableSharedStates = new TogglableSettingBlockSharedStates($scope.sportGroups.length, settingType.isCollapsed);
                                }
                            } else {
                                $scope.sportGroups = <DefaultSettingItem[][]>[settingType.settingItems];
                                if ($scope.togglableSettingBlocks) {
                                    $scope.togglableSharedStates = new TogglableSettingBlockSharedStates(1, settingType.isCollapsed);
                                }
                            }
                        }
                    }
                });
            }];
    }

    ModuleRegister.directive("settingtype17", SettingType17Directive);
}