/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helpers.ts" />

module CustomerSetting {
    interface SettingType3Template1Scope extends SettingTypeDirectiveSharedScope {
        rateRanges: Array<KeyValuePair<number, number>>;
    }

    class Settingtype3Item extends DefaultSettingItem {
        public commRate: number;
        public upline: Settingtype3Item;

        constructor(settingItem: any) {
            super(settingItem);

            if (settingItem) {
                this.commRate = settingItem.commRate;

                if (settingItem.upline) {
                    this.upline = new Settingtype3Item(settingItem.upline);
                }
            }
        }

        public backup(): void {
            var oldSetting: Settingtype3Item;

            super.backup();
            oldSetting = <Settingtype3Item>this.oldSetting;

            oldSetting.commRate = this.commRate;

            if (this.upline) {
                oldSetting.upline = new Settingtype3Item(this.upline);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: Settingtype3Item = <Settingtype3Item>this.oldSetting;

            return isChanged
                || this.commRate !== oldSetting.commRate
                || (this.upline && this.upline.isChanged());
        }
    }

    class SettingType3Template1Directive extends SettingTypeDirective {
        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype3.template1/settingtype3-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingType3Template1Scope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            scope.valueTemplateUrl = "settingtyp3-template1-value.html";
        }

        constructor(private appConfigService: AppConfigService, private valueRangeGenerator: ValueRangeGenerator) {
            super();
        }
    }

    angular.module("customerSetting").directive("settingtype3Template1", ["appConfigService", "valueRangeGenerator", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType3Template1Directive, args));
    }]);
}