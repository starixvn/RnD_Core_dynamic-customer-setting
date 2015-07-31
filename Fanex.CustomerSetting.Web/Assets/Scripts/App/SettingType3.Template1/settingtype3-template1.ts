/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />

module CustomerSetting {
    class Settingtype3Template1Item extends DefaultSettingItem {
        public commRate: number;
        public upline: Settingtype3Template1Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = true) {
            super(settingItem);

            if (settingItem) {
                this.commRate = settingItem.commRate;

                if (settingItem.upline) {
                    this.upline = new Settingtype3Template1Item(settingItem.upline, false);
                }
            }

            if (backup) {
                this.settingType3Template1Backup();
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: Settingtype3Template1Item = <Settingtype3Template1Item>this.oldSetting;

            return isChanged
                || this.commRate !== oldSetting.commRate;
        }

        private settingType3Template1Backup(): void {
            var oldSetting: Settingtype3Template1Item;

            this.oldSetting = new Settingtype3Template1Item(undefined, false);
            super.backup();
            oldSetting = <Settingtype3Template1Item>this.oldSetting;
            oldSetting.commRate = this.commRate;

            if (this.upline) {
                oldSetting.upline = new Settingtype3Template1Item(this.upline, false);
            }
        }
    }

    class SettingType3Template1Directive extends SettingTypeDirective {
        public templateUrl: string = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype3.template1/settingtype3-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtyp3-template1-value.html";
        }

        constructor(private appConfigService: AppConfigService) {
            super(Settingtype3Template1Item);
        }
    }

    angular.module("customerSetting").directive("settingtype3Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType3Template1Directive, args));
    }]);

    interface SettingType3Template1ItemScope extends ng.IScope {
        settingItem: Settingtype3Template1Item;
        range: Array<KeyValuePair<string, number>>;
    }

    class CommRateDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                settingItem: "="
            }
        }

        public templateUrl: string = "settingtyp3-template1-drop.html";

        public controller: any = ["$scope", "$rootScope", ($scope: SettingType3Template1ItemScope) => {
            var step: number = Settingtype3Template1Config.step;
            var divisor: number = Settingtype3Template1Config.divisor;

            $scope.$watch("settingItem", () => {
                $scope.range = this.valueRangeGenerator.generateRanges(
                    $scope.settingItem.commRate,
                    0,
                    $scope.settingItem.upline.commRate,
                    step,
                    divisor);
            });
        }];

        constructor(private valueRangeGenerator: ValueRangeGenerator) { }
    }

    angular.module("customerSetting").directive("commRate", ["valueRangeGenerator", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(CommRateDirective, args));
    }]);

    class Settingtype3Template1Config {
        public static get step(): number {
            return 0.0005;
        }

        public static get divisor(): number {
            return 10000;
        }
    }
}