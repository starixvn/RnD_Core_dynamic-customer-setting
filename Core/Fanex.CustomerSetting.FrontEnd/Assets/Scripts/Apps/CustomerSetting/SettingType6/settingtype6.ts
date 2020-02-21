/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/blockui/block-ui.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/requesting-server-service.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype6-resource.ts" />

module CustomerSetting {
    const settingTypeId: number = 6;

    interface ISettingType6DirectviceScope extends IDefaultSettingTypeDirectiveScope {
        highestPercentageRange: number;
        getCurrentWinLoss: (settingItem: SettingType6Item) => void;
    }

    class SettingType6Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype6.htm";

        public get scope(): any {
            return {
                settingType: "=",
                update: "&"
            };
        }

        public controller: any = [
            "$scope",
            "$http",
            "appConfigService",
            "requestService",
            "settingTypesSharedValuesService",
            "settingType6ResourceService",
            (
                $scope: ISettingType6DirectviceScope,
                $http: ng.IHttpService,
                appConfigService: AppConfigService,
                requestService: RequestService,
                settingTypesSharedValuesService: SettingTypesSharedValuesService,
                settingType6ResourceService: SettingType6ResourceService
            ) => {
                $scope.customerId = settingTypesSharedValuesService.customerId;
                $scope.uplineLevel = settingTypesSharedValuesService.uplineLevel;
                $scope.customerLevel = settingTypesSharedValuesService.customerLevel;
                $scope.productId = settingTypesSharedValuesService.productId;
                $scope.productName = settingTypesSharedValuesService.productName;
                $scope.avaiCustomerLevels = Level;
                $scope.avaiSettingLevels = SettingLevel;
                $scope.settingTypeName = settingType6ResourceService.getLabel("WinLossLimit");
                $scope.highestPercentageRange = 100;

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
                                    settingType.settingItems[index] = new SettingType6Item(item, true);
                                }
                            });

                            if (settingType.settingLevel === SettingLevel.sportType || settingType.settingLevel === SettingLevel.sportTypeBetType) {
                                $scope.sportGroups = this.groupSettingItemsBySportType(settingType.settingItems);
                            } else {
                                $scope.sportGroups = <DefaultSettingItem[][]>[settingType.settingItems];
                            }
                        }
                    }
                });

                $scope.getCurrentWinLoss = (settingItem: SettingType6Item) => {
                    var command: RedirectCommand = new RedirectCommand(
                        $scope.customerId,
                        $scope.customerLevel,
                        $scope.productId,
                        $scope.uplineLevel,
                        settingTypeId,
                        0,
                        ActionType.query,
                        {
                            sportTypeId: settingItem.sportTypeId,
                            betTypeId: settingItem.betTypeId
                        });

                    requestService.request(appConfigService.getSettingTypeActionPath(), command.toJson(), (response: ng.IHttpPromiseCallbackArg<any>) => {
                        settingItem.currentWinLossAmount = new CurrentWinLossAmountItem(response.data);
                    });
                };
            }];
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