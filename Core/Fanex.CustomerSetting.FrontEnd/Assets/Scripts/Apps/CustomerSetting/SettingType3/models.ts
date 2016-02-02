/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType3Item extends DefaultSettingItem {
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

    export class SettingType3Config {
        public static get step(): number {
            return 0.0005;
        }

        public static get roundingFactor(): number {
            return 10000;
        }
    }
}