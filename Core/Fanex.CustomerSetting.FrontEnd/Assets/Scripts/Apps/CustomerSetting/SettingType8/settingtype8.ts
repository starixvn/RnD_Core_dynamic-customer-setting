/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype8-resource.ts" />

module CustomerSetting {
    const settingTypeId: number = 8;

    interface ISettingType8DirectiveScope extends IDefaultSettingTypeDirectiveScope {
        popupMode: boolean;
        getLevelGroupDetails: Function;
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
            "appConfigService",
            "requestService",
            "settingTypesSharedValuesService",
            (
                $scope: ISettingType8DirectiveScope,
                appConfigService: AppConfigService,
                requestService: RequestService,
                settingTypesSharedValuesService: SettingTypesSharedValuesService
            ) => {
                $scope.customerId = settingTypesSharedValuesService.customerId;
                $scope.uplineLevel = settingTypesSharedValuesService.uplineLevel;
                $scope.customerLevel = settingTypesSharedValuesService.customerLevel;
                $scope.productId = settingTypesSharedValuesService.productId;
                $scope.productName = settingTypesSharedValuesService.productName;
                $scope.avaiCustomerLevels = Level;
                $scope.avaiSettingLevels = SettingLevel;

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

                $scope.getLevelGroupDetails = (settingItem: SettingType8Item) => {
                    var command: RedirectCommand = new RedirectCommand(
                        settingTypeId,
                        0,
                        ActionType.query,
                        {
                            customerId: $scope.customerId,
                            productId: $scope.productId,
                            sportTypeId: settingItem.sportTypeId
                        });

                    if (!settingItem.allDetails) {
                        requestService.request(appConfigService.getSettingTypeActionPath(), command.toJson(), (response: ng.IHttpPromiseCallbackArg<any>) => {
                            settingItem.allDetails = new SettingType8Item(response.data);
                            settingItem.showingDetails = true;
                        });
                    }

                    if (settingItem.allDetails) {
                        settingItem.showingDetails = true;
                    }
                };
            }];
    }

    ModuleRegister.directive("settingtype8", SettingType8Directive);
}