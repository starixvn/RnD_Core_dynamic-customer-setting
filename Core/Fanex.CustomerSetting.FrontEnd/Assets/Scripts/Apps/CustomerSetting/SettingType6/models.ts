/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType6Item extends DefaultSettingItem {
        public winLimit: number;
        public maxWinRange: number;
        public lossLimit: number;
        public minLossRange: number;
        public maxLossRange: number;
        public isAutoReset: boolean;
        public isAutoResetUpline: boolean;
        public isUnlimited: boolean;
        public currentWinLossAmount: CurrentWinLossAmountItem;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean);
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.winLimit = settingItem.winLimit;
                this.maxWinRange = settingItem.maxWinRange;
                this.lossLimit = settingItem.lossLimit;
                this.minLossRange = settingItem.minLossRange;
                this.maxLossRange = settingItem.maxLossRange;
                this.isAutoReset = settingItem.isAutoReset;
                this.isAutoResetUpline = settingItem.isAutoResetUpline;
                this.isUnlimited = settingItem.lossLimit === 0;
            }

            if (backup) {
                this.oldSetting = new SettingType6Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType6Item = <SettingType6Item>this.oldSetting;

            isChanged =
                isChanged
                || this.winLimit !== oldSetting.winLimit
                || this.lossLimit !== oldSetting.lossLimit
                || this.isAutoReset !== oldSetting.isAutoReset;

            return isChanged;
        }

        public copySettingFrom(source: SettingType6Item): boolean {
            var isSuccessful: boolean = true;
            var backupWinLimit: number = this.winLimit;
            var backupLossLimit: number = this.lossLimit;
            var backupIsAutoReset: boolean = this.isAutoReset;
            var backupIsUnlimited: boolean = this.isUnlimited;

            if (source) {
                this.winLimit = source.winLimit;
                this.lossLimit = source.lossLimit;
                this.isAutoReset = source.isAutoReset;
                this.isUnlimited = source.isUnlimited;
                isSuccessful = this.validate();
                
                if (!isSuccessful) {
                    this.winLimit = backupWinLimit;
                    this.lossLimit = backupLossLimit;
                    this.isAutoReset = backupIsAutoReset;
                    this.isUnlimited = backupIsUnlimited;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        winLimit: backupWinLimit,
                        lossLimit: backupLossLimit,
                        isAutoReset: backupIsAutoReset
                    });
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (this.winLimit < 0) {
                this.validationState = new ValidationResult("MaximumWinShouldNotBeLessThanZero", "winLimit");

                return false;
            }

            if (0 < this.maxWinRange && (this.winLimit === 0 || this.maxWinRange < this.winLimit)) {
                this.validationState = new ValidationResult("MaximumWinShouldBotExceedThanUplineMaximumWin", "winLimit");

                return false;
            }

            if (this.lossLimit === null) {
                this.validationState = new ValidationResult("MaximumLossValueIsRequired", "lossLimit");

                return false;
            }

            if ((this.maxLossRange === 0 && this.lossLimit !== 0 && this.lossLimit < this.minLossRange)
                || (this.maxLossRange > 0 && (this.lossLimit < this.minLossRange || this.maxLossRange < this.lossLimit))
                || (this.lossLimit < 0)) {
                this.validationState = new ValidationResult(
                    "MaximumLossValueShouldBeInRange",
                    "lossLimit",
                    [this.minLossRange, this.maxLossRange]);

                return false;
            }

            if (!this.isAutoResetUpline && this.isAutoReset) {
                this.validationState = new ValidationResult("AutoResetIsViolatedWithUpline", "isAutoReset");

                return false;
            }

            this.resetValidationState();

            return true;
        }

        public toJson(): {} {
            return {
                sportTypeId: this.sportTypeId,
                sportTypeName: this.sportTypeName,
                betTypeId: this.betTypeId,
                betTypeName: this.betTypeName,
                typeName: this.typeName,
                winLimit: this.winLimit,
                maxWinRange: this.maxWinRange,
                lossLimit: this.lossLimit,
                minLossRange: this.minLossRange,
                maxLossRange: this.maxLossRange,
                isAutoReset: this.isAutoReset,
                isAutoResetUpline: this.isAutoResetUpline
            };
        }
    }

    export class CurrentWinLossAmountItem {
        public currentAmount: number;
        public winPercent: number;
        public maxWinAmount: number;
        public lossPercent: number;
        public maxLossAmount: number;

        constructor(currentWinningLosingAmount: any) {
            if (currentWinningLosingAmount) {
                this.currentAmount = currentWinningLosingAmount.currentAmount;
                this.winPercent = currentWinningLosingAmount.winPercent;
                this.maxWinAmount = currentWinningLosingAmount.maxWinAmount;
                this.lossPercent = currentWinningLosingAmount.lossPercent;
                this.maxLossAmount = currentWinningLosingAmount.maxLossAmount;
            }
        }
    }
}