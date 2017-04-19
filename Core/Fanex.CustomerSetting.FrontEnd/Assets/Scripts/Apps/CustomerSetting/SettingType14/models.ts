/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType14Item extends DefaultSettingItem {
        static roundingFactor: number = 100;

        public commission: number;
        public uplineCommission: number;
        public nearestIndirectUplineCommission: number;

        public get remain(): number {
            return this.correctData(this.uplineCommission - this.commission);
        }

        private customerLevel: Level;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup?: boolean, customerLevel?: Level)
        constructor(settingItem?: any, backup: boolean = false, customerLevel?: Level) {
            super(settingItem);

            if (settingItem) {
                this.customerLevel = customerLevel;

                if (settingItem.commission || settingItem.commission === 0) {
                    this.commission = this.convertRateToPercentage(settingItem.commission);
                }

                if (settingItem.uplineCommission || settingItem.uplineCommission === 0) {
                    this.uplineCommission = this.convertRateToPercentage(settingItem.uplineCommission);
                }

                if (settingItem.nearestIndirectUplineCommission || settingItem.nearestIndirectUplineCommission === 0) {
                    this.nearestIndirectUplineCommission = this.convertRateToPercentage(settingItem.nearestIndirectUplineCommission);
                }
            }

            if (backup) {
                this.oldSetting = new SettingType14Item(settingItem);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType14Item = <SettingType14Item>this.oldSetting;

            isChanged = isChanged || this.commission !== oldSetting.commission;

            return isChanged;
        }

        public copySettingFrom(source: SettingType14Item): boolean {
            var isSuccessful: boolean = true;
            var backupCommission: number;

            if (source) {
                backupCommission = this.commission;

                this.commission = source.commission;
                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.commission = backupCommission;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({ commission: backupCommission });
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (this.commission) {
                if (this.commission < 0) {
                    this.validationState = new ValidationResult("CommissionMustBePositive", "commission");

                    return false;
                }

                if (this.commission > this.uplineCommission && this.customerLevel !== Level.member) {
                    this.validationState = new ValidationResult("CommissionMustBeLessThanOrEqualToGivenCommission", "commission");

                    return false;
                }

                if (this.commission > SettingType14Item.roundingFactor) {
                    this.validationState = new ValidationResult("CommissionCannotBeExceed100Percent", "commission");

                    return false;
                }
            }

            this.resetValidationState();

            return true;
        }

        public toJson(): {} {
            var settingItem: any = {
                sportTypeId: this.sportTypeId,
                sportTypeName: this.sportTypeName,
                betTypeId: this.betTypeId,
                betTypeName: this.betTypeName,
                typeName: this.typeName
            };

            settingItem.commission = this.convertPercentageToRate(this.commission);

            if (this.commission) {
                settingItem.commission = this.convertPercentageToRate(this.commission);
            }

            if (this.uplineCommission) {
                settingItem.uplineCommission = this.convertPercentageToRate(this.uplineCommission);
            }

            return settingItem;
        }

        private convertPercentageToRate(rate: number): number {
            return parseFloat((rate / SettingType14Item.roundingFactor).toFixed(5));
        }

        private convertRateToPercentage(rate: number): number {
            return this.correctData(rate * SettingType14Item.roundingFactor);
        }

        private correctData(rate: number): number {
            return parseFloat(rate.toFixed(3));
        }
    }
}