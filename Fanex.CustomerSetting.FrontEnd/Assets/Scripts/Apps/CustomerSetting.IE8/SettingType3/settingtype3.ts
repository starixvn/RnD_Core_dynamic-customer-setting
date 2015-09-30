/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/models.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="commission-resources.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class SettingType3Item extends DefaultSettingItem {
        public commRate: number;
        public upline: SettingType3Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.commRate = settingItem.commRate;

                if (settingItem.upline) {
                    this.upline = new SettingType3Item(settingItem.upline);
                }
            }

            if (backup) {
                this.oldSetting = new SettingType3Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType3Item = <SettingType3Item>this.oldSetting;

            return isChanged
                || this.commRate !== oldSetting.commRate;
        }

        public copySettingFrom(source: SettingType3Item): boolean {
            var isSuccessful: boolean = true;
            var backupCommRate: number;

            if (source) {
                backupCommRate = this.commRate;

                this.commRate = source.commRate;
                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.commRate = backupCommRate;
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (this.commRate > this.upline.commRate) {
                this.validationState = new ValidationResult("CommRateCannotBeExceedMaxValue", "commRate");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }
    }

    class SettingType3Directive extends SettingTypeDirective {
        public static $inject: string[] = ["commissionResourceService"];

        public templateUrl: string = "settingtype3.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.headerTemplateUrl = "settingtype3-header.htm";
            scope.valueTemplateUrl = "settingtype3-value.htm";
            scope.settingTypeName = this.commissionResourceService.getLabel("Commission");
        };

        private commissionResourceService: CommissionResourceService;

        constructor(commissionResourceService: CommissionResourceService) {
            super((settingItem: any, backup?: boolean) => new SettingType3Item(settingItem, backup));

            this.commissionResourceService = commissionResourceService;
        }
    }

    ModuleRegister.directive("settingtype3", SettingType3Directive);

    interface ISettingType3ItemScope extends ng.IScope {
        settingItem: SettingType3Item;
        range: Array<KeyValuePair<number, number>>;
    }

    class CommRateDirective implements ng.IDirective {
        public static $inject: string[] = ["valueRangeService"];

        public replace: boolean = true;

        public scope: any = {
            settingItem: "="
        };

        public templateUrl: string = "settingtype3-drop.htm";

        public controller: any = ["$scope", ($scope: ISettingType3ItemScope) => {
            $scope.$watch("settingItem", () => {
                $scope.range = this.valueRangeService.generateRanges(
                    $scope.settingItem.commRate,
                    0,
                    $scope.settingItem.upline.commRate,
                    SettingType3Config.step,
                    SettingType3Config.roundingFactor);
            });
        }];

        private valueRangeService: ValueRangeService;

        constructor(valueRangeService: ValueRangeService) {
            this.valueRangeService = valueRangeService;
        }
    }

    ModuleRegister.directive("commRate", CommRateDirective);

    class SettingType3Config {
        public static step: number = 0.0005;
        public static roundingFactor: number = 10000;
    }
}