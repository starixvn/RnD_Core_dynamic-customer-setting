/// <reference path="../core/models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType7Item extends DefaultSettingItem {
        public chargedCommission: number;
        public upline: SettingType7Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.chargedCommission = settingItem.chargedCommission;

                if (settingItem.upline) {
                    this.upline = new SettingType7Item(settingItem.upline);
                }
            }

            if (backup) {
                this.oldSetting = new SettingType7Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType7Item = <SettingType7Item>this.oldSetting;

            return isChanged || this.chargedCommission !== oldSetting.chargedCommission;
        }

        public copySettingFrom(source: SettingType7Item): boolean {
            var isSuccessful: boolean = true;
            var backupChargedCommission: number;

            if (source) {
                backupChargedCommission = this.chargedCommission;

                this.chargedCommission = source.chargedCommission;
                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.chargedCommission = backupChargedCommission;
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (this.chargedCommission > this.upline.chargedCommission) {
                this.validationState = new ValidationResult("ChargedCommissionIsGreaterThanUplineChargedCommissionError", "chargedCommission");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }
    }
}