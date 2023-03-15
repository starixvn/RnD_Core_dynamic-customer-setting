/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="settingtype23-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType23DirectviceScope extends IDefaultSettingTypeDirectiveScope {
        headerTemplateUrlSubType2: string;
        valueTemplateUrlSubType2: string;
    }

    class SettingType23Directive extends DefaultSettingTypeGroupingDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype23.htm";
        public headerTemplateUrl: string;
        public headerTemplateUrlSubType2: string;
        public valueTemplateUrl: string;
        public valueTemplateUrlSubType2: string;

        public get scope(): any {
            return {
                settingType: "=",
                update: "&",
                index: "@"
            };
        }

        public link: ng.IDirectiveLinkFn = (scope: ISettingType23DirectviceScope) => {
            scope.headerTemplateUrl = "settingtype23-header.htm";
            scope.valueTemplateUrl = "settingtype23-value.htm";
            scope.headerTemplateUrlSubType2 = "settingtype23-header-2.htm";
            scope.valueTemplateUrlSubType2 = "settingtype23-value-2.htm";
        };

        public controller: any = [
            "$scope",
            "$http",
            "appConfigService",
            "requestService",
            "settingTypesSharedValuesService",
            "settingType23ResourceService",
            (
                $scope: ISettingType23DirectviceScope,
                $http: ng.IHttpService,
                appConfigService: AppConfigService,
                requestService: RequestService,
                settingTypesSharedValuesService: SettingTypesSharedValuesService,
                settingType23ResourceService: SettingType23ResourceService
            ) => {
                $scope.customerId = settingTypesSharedValuesService.customerId;
                $scope.uplineLevel = settingTypesSharedValuesService.uplineLevel;
                $scope.customerLevel = settingTypesSharedValuesService.customerLevel;
                $scope.productId = settingTypesSharedValuesService.productId;
                $scope.productName = settingTypesSharedValuesService.productName;
                $scope.avaiCustomerLevels = Level;
                $scope.avaiSettingLevels = SettingLevel;
                $scope.settingTypeName = settingType23ResourceService.getLabel("BetSetting");
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
                                    settingType.settingItems[index] = new SettingType23Item(item, true, settingTypesSharedValuesService.systemId);
                                }
                            });

                            $scope.sportGroups = this.groupSettingItemsBySubType(settingType.settingItems);

                            if ($scope.togglableSettingBlocks) {
                                $scope.togglableSharedStates = new TogglableSettingBlockSharedStates(1, settingType.isCollapsed);
                            }
                        }
                    }
                });
            }];

        protected groupSettingItemsBySubType(settingItems: ISettingItem[]): SettingType23Item[][] {
            var subType: number = -1;
            var result: Array<Array<SettingType23Item>> = new Array<Array<SettingType23Item>>();
            var group: Array<SettingType23Item>;

            settingItems.forEach((item: ISettingItem) => {
                var settingItem = <SettingType23Item>item;

                if (subType !== settingItem.subType) {
                    if (group) {
                        result.push(group);
                    }

                    subType = settingItem.subType;
                    group = new Array<SettingType23Item>();
                }

                group.push(settingItem);
            });

            if (group) {
                result.push(group);
            }

            return result;
        }
    }

    ModuleRegister.directive("settingtype23", SettingType23Directive);
}