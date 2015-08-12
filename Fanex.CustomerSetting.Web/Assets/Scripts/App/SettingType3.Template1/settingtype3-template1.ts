/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/models.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="commission-resources.ts" />

module CustomerSetting {
    "use strict";

    class Settingtype3Template1Item extends DefaultSettingItem {
        public commRate: number;
        public upline: Settingtype3Template1Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.commRate = settingItem.commRate;

                if (settingItem.upline) {
                    this.upline = new Settingtype3Template1Item(settingItem.upline);
                }
            }

            if (backup) {
                this.oldSetting = new Settingtype3Template1Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: Settingtype3Template1Item = <Settingtype3Template1Item>this.oldSetting;

            return isChanged
                || this.commRate !== oldSetting.commRate;
        }

        public copySettingFrom(source: Settingtype3Template1Item): boolean {
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

    class SettingType3Template1Directive extends SettingTypeDirective {
        public static $inject: string[] = ["appConfigService", "commissionResourceService"];

        public templateUrl: Function = () => {
            return this.appConfigService.resolveRootUrl("app/customerSetting/settingtype3.template1/settingtype3-template1.htm");
        };

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtyp3-template1-value.html";
            scope.settingTypeName = this.commissionResourceService.getLabel("Commission");
        };

        private appConfigService: AppConfigService;
        private commissionResourceService: CommissionResourceService;

        constructor(appConfigService: AppConfigService, commissionResourceService: CommissionResourceService) {
            super((settingItem: any, backup?: boolean) => new Settingtype3Template1Item(settingItem, backup));

            this.appConfigService = appConfigService;
            this.commissionResourceService = commissionResourceService;
        }
    }

    ModuleRegister.directive("settingtype3Template1", SettingType3Template1Directive);

    interface ISettingType3Template1ItemScope extends ng.IScope {
        settingItem: Settingtype3Template1Item;
        range: Array<KeyValuePair<number, number>>;
    }

    class CommRateDirective implements ng.IDirective {
        public static $inject: string[] = ["valueRangeService"];

        public replace: boolean = true;

        public get scope(): any {
            return {
                settingItem: "="
            };
        }

        public templateUrl: string = "settingtyp3-template1-drop.html";

        public controller: any = ["$scope", ($scope: ISettingType3Template1ItemScope) => {
            $scope.$watch("settingItem", () => {
                $scope.range = this.valueRangeService.generateRanges(
                    $scope.settingItem.commRate,
                    0,
                    $scope.settingItem.upline.commRate,
                    Settingtype3Template1Config.step,
                    Settingtype3Template1Config.roundingFactor);
            });
        }];

        private valueRangeService: ValueRangeService;

        constructor(valueRangeService: ValueRangeService) {
            this.valueRangeService = valueRangeService;
        }
    }

    ModuleRegister.directive("commRate", CommRateDirective);

    class Settingtype3Template1Config {
        public static get step(): number {
            return 0.0005;
        }

        public static get roundingFactor(): number {
            return 10000;
        }
    }
}