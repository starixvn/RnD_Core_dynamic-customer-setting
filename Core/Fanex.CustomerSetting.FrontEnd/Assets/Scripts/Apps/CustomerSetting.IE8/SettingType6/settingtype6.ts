/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="../core/blockui/block-ui.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype6-resource.ts" />

module CustomerSetting.IE8 {
    const settingTypeId: number = 6;

    interface ISettingType6DirectviceScope extends IDefaultSettingTypeDirectiveScope {
        highestPercentageRange: number;
        getCurrentWinLoss: (settingItem: SettingType6Item) => void;
    }

    class SettingType6Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype6.htm";

        public scope: any = {
            settingType: "=",
            update: "&"
        };

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
                $scope.normalMode = settingTypesSharedValuesService.normalMode;
                $scope.avaiCustomerLevels = Level;
                $scope.avaiSettingLevels = SettingLevel;
                $scope.settingTypeName = settingType6ResourceService.getLabel("WinLossLimit");
                $scope.highestPercentageRange = 100;

                $scope.$watch("settingType", (settingType: SettingType) => {
                    if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                        if (settingType.isError) {
                            if (settingType.settingItems.length > 0) {
                                settingType.settingItems[0] = new ErrorSettingItem(settingType.settingItems[0]);
                            }
                        } else {
                            _.forEach(settingType.settingItems, (item: any, index: number) => {
                                if (angular.isDefined(item)) {
                                    settingType.settingItems[index] = new SettingType6Item(item, true);
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

                $scope.getCurrentWinLoss = (settingItem: SettingType6Item) => {
                    var command: RedirectCommand = new RedirectCommand(
                        settingTypeId,
                        0,
                        ActionType.query,
                        {
                            customerId: $scope.customerId,
                            sportTypeId: settingItem.sportTypeId,
                            betTypeId: settingItem.betTypeId
                        });

                    requestService.request(appConfigService.getSettingTypeActionPath(), command.toJson(), (response: ng.IHttpPromiseCallbackArg<any>) => {
                        settingItem.currentWinLossAmount = new CurrentWinLossAmountItem(response.data);
                    });
                };
            }];

        private settingType6ResourceService: SettingType6ResourceService;

        constructor(settingType6ResourceService: SettingType6ResourceService) {
            super();
            this.settingType6ResourceService = settingType6ResourceService;
        }
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