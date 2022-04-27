/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/requesting-server-service.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
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
            "parametersService",
            (
                $scope: ISettingType8DirectiveScope,
                appConfigService: AppConfigService,
                requestService: RequestService,
                settingTypesSharedValuesService: SettingTypesSharedValuesService,
                parametersService: ParametersService
            ) => {
                $scope.customerId = settingTypesSharedValuesService.customerId;
                $scope.uplineLevel = settingTypesSharedValuesService.uplineLevel;
                $scope.customerLevel = settingTypesSharedValuesService.customerLevel;
                $scope.productId = settingTypesSharedValuesService.productId;
                $scope.productName = settingTypesSharedValuesService.productName;
                $scope.avaiCustomerLevels = Level;
                $scope.avaiSettingLevels = SettingLevel;

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
                                    settingType.settingItems[index] = new SettingType8Item(
                                        item,
                                        true,
                                        $scope.customerLevel,
                                        parametersService.systemId);
                                }
                            });
                        }

                        if ($scope.togglableSettingBlocks) {
                            $scope.togglableSharedStates = new TogglableSettingBlockSharedStates(1, false);
                        }
                    }
                });

                $scope.getLevelGroupDetails = (settingItem: SettingType8Item) => {
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
                            settingLevel: $scope.settingType.settingLevel
                        });

                    if (!settingItem.allDetails) {
                        requestService.request(appConfigService.getSettingTypeActionPath(), command.toJson(), (response: ng.IHttpPromiseCallbackArg<any>) => {
                            settingItem.allDetails = new SettingType(response.data);
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