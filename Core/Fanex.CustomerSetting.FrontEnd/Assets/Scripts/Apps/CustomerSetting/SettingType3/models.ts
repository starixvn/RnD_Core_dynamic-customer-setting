/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType3Item extends DefaultSettingItem {
        public commRate: number;
        public intervalStep: number;
        public upline: SettingType3Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.commRate = 0.1;//settingItem.commRate;
                this.intervalStep = settingItem.intervalStep;

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
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        commRate: backupCommRate
                    });
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (this.commRate > this.upline.commRate) {
                this.validationState = new ValidationResult("CommRateCannotBeExceedMaxValue", "commRate");

                return false;
            }

            this.resetValidationState();

            return true;
        }
    }
}