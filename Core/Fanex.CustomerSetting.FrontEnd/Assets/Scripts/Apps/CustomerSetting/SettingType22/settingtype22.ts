/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/blockui/block-ui.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/requesting-server-service.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype22-resource.ts" />

module CustomerSetting {
    const settingTypeId: number = 22;

    interface ISettingType22DirectviceScope extends IDefaultSettingTypeDirectiveScope {
        highestPercentageRange: number;
        getCurrentWinLoss: (settingItem: SettingType22Item) => void;
    }

    class SettingType22Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype22.htm";

        public get scope(): any {
            return {
                settingType: "=",
                update: "&",
                index: "@"
            };
        }

        public controller: any = [
            "$scope",
            "$http",
            "appConfigService",
            "requestService",
            "settingTypesSharedValuesService",
            "settingType22ResourceService",
            (
                $scope: ISettingType22DirectviceScope,
                $http: ng.IHttpService,
                appConfigService: AppConfigService,
                requestService: RequestService,
                settingTypesSharedValuesService: SettingTypesSharedValuesService,
                settingType22ResourceService: SettingType22ResourceService
            ) => {
                $scope.customerId = settingTypesSharedValuesService.customerId;
                $scope.uplineLevel = settingTypesSharedValuesService.uplineLevel;
                $scope.customerLevel = settingTypesSharedValuesService.customerLevel;
                $scope.productId = settingTypesSharedValuesService.productId;
                $scope.productName = settingTypesSharedValuesService.productName;
                $scope.avaiCustomerLevels = Level;
                $scope.avaiSettingLevels = SettingLevel;
                $scope.settingTypeName = settingType22ResourceService.getLabel("WinLossLimit");
                $scope.highestPercentageRange = 100;
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
                                    settingType.settingItems[index] = new SettingType22Item(item, true);
                                }
                            });

                            if (settingType.settingLevel === SettingLevel.sportType || settingType.settingLevel === SettingLevel.sportTypeBetType) {
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

                $scope.getCurrentWinLoss = (settingItem: SettingType22Item) => {
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
                        settingItem.currentWinLossAmount = new Array<CurrentWinLossAmountItem>();
                        response.data.forEach((winlossAmountItem) => {
                            settingItem.currentWinLossAmount.push(new CurrentWinLossAmountItem(winlossAmountItem))
                        });


                        settingItem.showWinLossAmount = true;
                    });
                };
            }];
    }

    ModuleRegister.directive("settingtype22", SettingType22Directive);

    function UnlimitedLimitFilter(settingType22ResourceService: SettingType22ResourceService) {
        return (value: any) => {
            return parseFloat(value) > 0 ? value : settingType22ResourceService.getLabel("Unlimited");
        };
    }

    UnlimitedLimitFilter.$inject = ["settingType22ResourceService"];
    ModuleRegister.filter("unlimitedLimit", UnlimitedLimitFilter);
}