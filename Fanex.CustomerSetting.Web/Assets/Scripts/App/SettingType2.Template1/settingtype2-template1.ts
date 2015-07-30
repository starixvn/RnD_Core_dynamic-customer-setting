/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />

module CustomerSetting {
    class Settingtype2Template1Item extends DefaultSettingItem {
        public minBet: number;
        public maxBet: number;
        public maxPerMatch: number;
        public upline: Settingtype2Template1Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = true) {
            super(settingItem);

            if (settingItem) {
                this.minBet = settingItem.minBet;
                this.maxBet = settingItem.maxBet;
                this.maxPerMatch = settingItem.maxPerMatch;

                if (settingItem.upline) {
                    this.upline = new Settingtype2Template1Item(settingItem.upline, false);
                }
            }

            if (backup) {
                this.settingType2Template1Backup();
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: Settingtype2Template1Item = <Settingtype2Template1Item>this.oldSetting;

            return isChanged
                || this.minBet !== oldSetting.minBet
                || this.maxBet !== oldSetting.maxBet
                || this.maxPerMatch !== oldSetting.maxPerMatch;
        }

        private settingType2Template1Backup(): void {
            var oldSetting: Settingtype2Template1Item;

            this.oldSetting = new Settingtype2Template1Item(undefined, false);
            super.backup();
            oldSetting = <Settingtype2Template1Item>this.oldSetting;
            oldSetting.minBet = this.minBet;
            oldSetting.maxBet = this.maxBet;
            oldSetting.maxPerMatch = this.maxPerMatch;

            if (this.upline) {
                oldSetting.upline = new Settingtype2Template1Item(this.upline, false);
            }
        }
    }

    class SettingType2Template1Directive extends SettingTypeDirective {
        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype2.template1/settingtype2-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingTypeDirectiveSharedScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            scope.headerTemplateUrl = "settingtyp2-template1-header.html";
            scope.valueTemplateUrl = "settingtyp2-template1-value.html";
        }

        constructor(private appConfigService: AppConfigService) {
            super(Settingtype2Template1Item);
        }
    }

    angular.module("customerSetting").directive("settingtype2Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType2Template1Directive, args));
    }]);
}