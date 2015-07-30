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

    interface SettingType3Template1Scope extends SettingTypeDirectiveSharedScope {
        step: number;
        divisor: number;
    }

    class SettingType3Template1Directive extends SettingTypeDirective {
        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype3.template1/settingtype3-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingType3Template1Scope) => {
            scope.valueTemplateUrl = "settingtyp3-template1-value.html";
            scope.step = 0.0005;
            scope.divisor = 10000;
        }

        constructor(private appConfigService: AppConfigService) {
            super(Settingtype3Template1Item);
        }
    }

    angular.module("customerSetting").directive("settingtype3Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType3Template1Directive, args));
    }]);
}