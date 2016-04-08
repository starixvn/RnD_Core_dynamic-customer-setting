/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype8-resource.ts" />

module CustomerSetting {
    interface ISettingType8DirectiveScope extends IDefaultSettingTypeDirectiveScope {
        popupMode: boolean;
    }

    class SettingType8Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype8.htm";

        public get scope(): any {
            return {
                settingType: "=",
                popupMode: "=",
                update: "&"
            };
        }

        public controller: any = [
            "$scope",
            "settingTypesSharedValuesService",
            "settingType8ResourceService",
            (
                $scope: ISettingType8DirectiveScope,
                settingTypesSharedValuesService: SettingTypesSharedValuesService,
                settingType8ResourceService: SettingType8ResourceService
            ) => {
                $scope.customerId = settingTypesSharedValuesService.customerId;
                $scope.uplineLevel = settingTypesSharedValuesService.uplineLevel;
                $scope.customerLevel = settingTypesSharedValuesService.customerLevel;
                $scope.productId = settingTypesSharedValuesService.productId;
                $scope.productName = settingTypesSharedValuesService.productName;
                $scope.avaiCustomerLevels = Level;
                $scope.avaiSettingLevels = SettingLevel;
                $scope.settingTypeName = settingType8ResourceService.getLabel("BetLimit");

                $scope.$watch("settingType", (settingType: SettingType) => {
                    $scope.normalMode = settingTypesSharedValuesService.normalMode;

                    if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                        if (settingType.isError) {
                            if (settingType.settingItems.length > 0) {
                                settingType.settingItems[0] = new ErrorSettingItem(settingType.settingItems[0]);
                            }
                        } else {
                            settingType.settingItems.forEach((item: any, index: number) => {
                                if (angular.isDefined(item)) {
                                    settingType.settingItems[index] = new SettingType8Item(item, $scope.customerLevel, true);
                                }
                            });
                        }
                    }
                });
            }];
    }

    ModuleRegister.directive("settingtype8", SettingType8Directive);
}