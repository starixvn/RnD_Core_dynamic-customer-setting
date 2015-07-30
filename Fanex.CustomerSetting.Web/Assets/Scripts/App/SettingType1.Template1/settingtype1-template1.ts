/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />

module CustomerSetting {
    class Settingtype1Template1Item extends DefaultSettingItem {
        public s4: number;
        public s3: number;
        public s1: number;
        public m4: number;
        public m3: number;
        public m2: number;
        public m1: number;
        public a4: number;
        public a2: number;
        public a1: number;
        public n4: number;
        public n3: number;
        public n2: number;
        public us3: number;
        public us1: number;
        public um2: number;
        public um1: number;
        public is3: number;
        public is1: number;
        public im2: number;
        public im1: number;
        public defaultMaxPT: number;
        public superPTOnMembers: number;
        public masterPTOnMembers: number;
        public isUpdateSuperPTOnMembers: boolean;
        public isUpdateMasterPTOnMembers: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = true) {
            super(settingItem);

            if (settingItem) {
                this.fetchData(settingItem);
            }

            if (backup) {
                this.backupSettingtype1Template1();
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: Settingtype1Template1Item = <Settingtype1Template1Item>this.oldSetting;

            return isChanged;
        }

        private backupSettingtype1Template1(): void {
            var oldSetting: Settingtype1Template1Item;

            this.oldSetting = new Settingtype1Template1Item(undefined, false);
            super.backup();
            oldSetting = <Settingtype1Template1Item>this.oldSetting;
        }

        private fetchData(rawData: any): void {
            var addedProperties: string[] = ["sportTypeId", "sportTypeName", "betTypeId", "betTypeName", "typeName", "isLive"];

            Object.keys(this).filter(propName => {
                return addedProperties.indexOf(propName) < 0 && rawData[propName];
            }).forEach(prop => {
                this[prop] = rawData[prop];
            });
        }
    }

    interface SettingType1Template1Scope extends SettingTypeDirectiveSharedScope {
        step: number;
        divisor: number;
    }

    class SettingType1Template1Directive extends SettingTypeDirective {
        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype1.template1/settingtype1-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingType1Template1Scope) => {
            scope.headerTemplateUrl = "settingtyp1-template1-header.html";
            scope.valueTemplateUrl = "settingtyp1-template1-value.html";
            scope.step = 0.0005;
            scope.divisor = 10000;
        }

        constructor(private appConfigService: AppConfigService) {
            super(Settingtype1Template1Item);
        }
    }

    angular.module("customerSetting").directive("settingtype1Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType1Template1Directive, args));
    }]);
}