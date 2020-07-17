/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType19Item extends CopyWithCheckboxSettingItem {
        public commRate: number;
        public intervalStep: number;
        public upline: SettingType19Item;
        
        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean);
        constructor(settingItem: any, backup: boolean, multipleUpdates: boolean);
        constructor(settingItem?: any, backup: boolean = false, multipleUpdates?: boolean) {
            super(settingItem, multipleUpdates);

            if (settingItem) {
                this.commRate = settingItem.commRate;
                this.intervalStep = settingItem.intervalStep;

                if (settingItem.upline) {
                    this.upline = new SettingType19Item(settingItem.upline);
                }
            }

            if (backup) {
                this.oldSetting = new SettingType19Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType19Item = <SettingType19Item>this.oldSetting;

            return isChanged
                || this.commRate !== oldSetting.commRate;
        }

        public copySettingFrom(source: SettingType19Item): boolean {
            var isSuccessful: boolean = true;
            var isCopySuccess: boolean = false;
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

                this.isRowCopied = this.hasHighlightField(NotificationType.success) || this.hasHighlightField(NotificationType.error);
                isCopySuccess = this.hasHighlightField(NotificationType.success) && !this.hasHighlightField(NotificationType.error);
                if (this.isRowCopied)
                    this.isCheckedUpdateRow = this.isCheckedUpdateMinPtOnly = isCopySuccess;
            }

            return isCopySuccess;
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