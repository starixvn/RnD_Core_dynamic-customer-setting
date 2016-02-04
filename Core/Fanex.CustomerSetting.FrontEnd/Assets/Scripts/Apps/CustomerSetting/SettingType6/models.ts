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

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.winLimit = settingItem.winLimit;
                this.maxWinRange = settingItem.maxWinRange;
                this.lossLimit = settingItem.lossLimit;
                this.minLossRange = settingItem.minLossRange;
                this.maxLossRange = settingItem.maxLossRange;
                this.isAutoReset = settingItem.isAutoReset;
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

        public validate(): boolean {
            if (this.winLimit < 0) {
                this.validationState = new ValidationResult("MaximumWinShouldNotBeLessThanZero", "winLimit");

                return false;
            }

            if (0 < this.maxWinRange && this.maxWinRange < this.winLimit) {
                this.validationState = new ValidationResult("MaximumWinShouldBotExceedThanUplineMaximumWin", "winLimit");

                return false;
            }

            if (this.lossLimit < this.minLossRange || (0 < this.maxLossRange && this.maxLossRange < this.lossLimit)) {
                this.validationState = new ValidationResult(
                    "MaximumLossValueShouldBeInRange",
                    "lossLimit",
                    [this.minLossRange, this.maxLossRange]);

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

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
                isAutoReset: this.isAutoReset
            };
        }
    }

    export class CurrentWinLossAmountItem {
        public winPercent: number;
        public maxWinAmountt: number;
        public lossPercent: number;
        public maxLossAmount: number;
    }
}