/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/requesting-server-service.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="settingtype10-resources.ts" />

module CustomerSetting {
    const settingTypeId: number = 10;

    interface ISettingType10DirectiveScope extends IDefaultSettingTypeDirectiveScope {
        availableGroups: string[],
        userGroup: string;
        getSelectedUserGroupCommissions: () => void;
        groupIntoRows(groups: CommissionGroup[]): CommissionGroup[][];
    }

    class SettingType10Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype10.htm";

        public get scope(): any {
            return {
                settingType: "=",
                update: "&"
            };
        }

        public controller: any = [
            "$scope",
            "appConfigService",
            "requestService",
            "settingTypesSharedValuesService",
            (
                $scope: ISettingType10DirectiveScope,
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
                    var availableGroups: KeyValuePair<string, any>[] = settingType.extraData.filter((item: KeyValuePair<string, any>) => item.key === "availableGroups");
                    var userGroup: KeyValuePair<string, any>[] = settingType.extraData.filter((item: KeyValuePair<string, any>) => item.key === "userGroup");

                    $scope.normalMode = settingTypesSharedValuesService.normalMode;

                    if (availableGroups && availableGroups.length > 0) {
                        $scope.availableGroups = availableGroups[0].value;
                    }

                    if (userGroup) {
                        $scope.userGroup = userGroup[0].value;
                    }

                    if (angular.isDefined(settingType) && angular.isArray(settingType.settingItems)) {
                        if (settingType.isError) {
                            if (settingType.settingItems.length > 0) {
                                settingType.settingItems[0] = new ErrorSettingItem(settingType.settingItems[0]);
                            }
                        } else {
                            settingType.settingItems.forEach((item: any, index: number) => {
                                if (angular.isDefined(item)) {
                                    settingType.settingItems[index] = new SettingType10Item(item, true, $scope.customerLevel);
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

                $scope.getSelectedUserGroupCommissions = () => {
                    var command: RedirectCommand = new RedirectCommand(
                        settingTypeId,
                        0,
                        ActionType.query,
                        {
                            memberId: $scope.customerId,
                            userGroup: $scope.userGroup
                        });

                    requestService.request(appConfigService.getSettingTypeActionPath(), command.toJson(), (response: ng.IHttpPromiseCallbackArg<any>) => {
                        if (response.data && response.data.length > 0) {
                            var updatedSettingItems: SettingType10Item[] = response.data.map((item: any) => new SettingType10Item(item, true, $scope.customerLevel));

                            $scope.settingType.settingItems.forEach((item: SettingType10Item) => {
                                var matchingItems: SettingType10Item[] = updatedSettingItems.filter((updatedItem: SettingType10Item) => {
                                    return item.sportTypeId === updatedItem.sportTypeId && item.betTypeId === updatedItem.betTypeId
                                });

                                if (matchingItems && matchingItems.length === 0) {
                                    updatedSettingItems.push(item);
                                }
                            });
                            $scope.settingType.settingItems = updatedSettingItems;
                        }
                    });
                };

                $scope.groupIntoRows = (groups: CommissionGroup[]) => {
                    var rows: CommissionGroup[][] = [];
                    var row: CommissionGroup[];

                    if (groups) {
                        for (var index: number = 0; index < groups.length; index++) {
                            if (index % 2 === 0) {
                                row = [];
                                rows.push(row);
                            }

                            row.push(groups[index]);
                        }

                        if (groups.length % 2 === 1) {
                            row.push(new CommissionGroup());
                        }
                    }

                    return rows;
                };
            }];
    }

    ModuleRegister.directive("settingtype10", SettingType10Directive);
}