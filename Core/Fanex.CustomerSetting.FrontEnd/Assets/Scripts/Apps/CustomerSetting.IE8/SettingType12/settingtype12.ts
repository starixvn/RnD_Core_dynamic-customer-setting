/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="../core/blockui/block-ui.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/requesting-server-service.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype12-resource.ts" />

module CustomerSetting.IE8 {
    const settingTypeId: number = 12;

    interface ISettingType12DirectviceScope extends IDefaultSettingTypeDirectiveScope {
        highestPercentageRange: number;
        getCurrentLoss: (settingItem: SettingType12Item) => void;
    }

    class SettingType12Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype12.htm";

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
            "settingType12ResourceService",
            (
                $scope: ISettingType12DirectviceScope,
                $http: ng.IHttpService,
                appConfigService: AppConfigService,
                requestService: RequestService,
                settingTypesSharedValuesService: SettingTypesSharedValuesService,
                settingType12ResourceService: SettingType12ResourceService
            ) => {
                $scope.customerId = settingTypesSharedValuesService.customerId;
                $scope.uplineLevel = settingTypesSharedValuesService.uplineLevel;
                $scope.customerLevel = settingTypesSharedValuesService.customerLevel;
                $scope.productId = settingTypesSharedValuesService.productId;
                $scope.productName = settingTypesSharedValuesService.productName;
                $scope.avaiCustomerLevels = Level;
                $scope.avaiSettingLevels = SettingLevel;
                $scope.settingTypeName = settingType12ResourceService.getLabel("LossLimit");
                $scope.highestPercentageRange = 100;

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
                                    settingType.settingItems[index] = new SettingType12Item(item, true);
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

                $scope.getCurrentLoss = (settingItem: SettingType12Item) => {
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
                        settingItem.currentLossAmount = new CurrentLossAmountItem(response.data);
                    });
                };
            }];

        private settingType12ResourceService: SettingType12ResourceService;

        constructor(settingType12ResourceService: SettingType12ResourceService) {
            super();
            this.settingType12ResourceService = settingType12ResourceService;
        }
    }

    ModuleRegister.directive("settingtype12", SettingType12Directive);

    function UnlimitedLimitFilter(settingType12ResourceService: SettingType12ResourceService) {
        return (value: any) => {
            return parseFloat(value) > 0 ? value : settingType12ResourceService.getLabel("Unlimited");
        };
    }

    UnlimitedLimitFilter.$inject = ["settingType12ResourceService"];
    ModuleRegister.filter("unlimitedLimit", UnlimitedLimitFilter);
}