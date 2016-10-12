/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType12Item extends DefaultSettingItem {
        public lossLimit: number;
        public minLossRange: number;
        public maxLossRange: number;
        public isAutoReset: boolean;
        public isAutoResetUpline: boolean;
        public currentLossAmount: CurrentLossAmountItem;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.lossLimit = settingItem.lossLimit;
                this.minLossRange = settingItem.minLossRange;
                this.maxLossRange = settingItem.maxLossRange;
                this.isAutoReset = settingItem.isAutoReset;
                this.isAutoResetUpline = settingItem.isAutoResetUpline;
            }

            if (backup) {
                this.oldSetting = new SettingType12Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType12Item = <SettingType12Item>this.oldSetting;

            isChanged =
                isChanged
                || this.lossLimit !== oldSetting.lossLimit
                || this.isAutoReset !== oldSetting.isAutoReset;

            return isChanged;
        }

        public validate(): boolean {
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
                lossLimit: this.lossLimit,
                minLossRange: this.minLossRange,
                maxLossRange: this.maxLossRange,
                isAutoReset: this.isAutoReset,
                isAutoResetUpline: this.isAutoResetUpline
            };
        }
    }

    export class CurrentLossAmountItem {
        public lossPercent: number;
        public maxLossAmount: number;

        constructor(currentLosingAmount: any) {
            if (currentLosingAmount) {
                this.lossPercent = currentLosingAmount.lossPercent;
                this.maxLossAmount = currentLosingAmount.maxLossAmount;
            }
        }
    }
}