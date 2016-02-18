/// <reference path="../core/models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType5Item extends DefaultSettingItem {
        static roundingFactor: number = 100;

        public chargeCommission: number;
        public remain: number;
        public upline: SettingType5Item;

        private customerLevel: Level;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup?: boolean, customerLevel?: Level)
        constructor(settingItem?: any, backup: boolean = false, customerLevel?: Level) {
            super(settingItem);

            if (settingItem) {
                this.chargeCommission = this.modifyChargeCommissionToDisplayForm(settingItem.chargeCommission);
                this.customerLevel = customerLevel;

                if (settingItem.upline) {
                    this.upline = new SettingType5Item(settingItem.upline);
                    this.remain = this.corectData(this.upline.chargeCommission - this.chargeCommission);
                }
            }

            if (backup) {
                this.oldSetting = new SettingType5Item(settingItem);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType5Item = <SettingType5Item>this.oldSetting;

            isChanged = isChanged || this.chargeCommission !== oldSetting.chargeCommission;

            return isChanged;
        }

        public copySettingFrom(source: SettingType5Item): boolean {
            var isSuccessful: boolean = true;
            var backupChargeCommission: number;

            if (source) {
                backupChargeCommission = this.chargeCommission;

                this.chargeCommission = source.chargeCommission;
                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.chargeCommission = backupChargeCommission;
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (this.chargeCommission > SettingType5Item.roundingFactor) {
                this.validationState = new ValidationResult("ShareCannotBeExceed100Percent", "chargeCommission");

                return false;
            }

            if (this.chargeCommission < 0) {
                this.validationState = new ValidationResult("ShareMustBePositive", "chargeCommission");

                return false;
            }

            if (this.chargeCommission > this.upline.chargeCommission && this.customerLevel !== Level.member) {
                this.validationState = new ValidationResult("ShareMustBeLessThanOrEqualToGivenCommissionError", "chargeCommission");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

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

            settingItem.chargeCommission = this.revertPecentageToRate(this.chargeCommission);

            if (this.upline) {
                settingItem.upline = this.upline.toJson();
            }

            return settingItem;
        }

        public corectData(rate: number): number {
            return parseFloat(rate.toFixed(2));
        }

        private revertPecentageToRate(rate: number): number {
            return parseFloat((rate / SettingType5Item.roundingFactor).toFixed(4));
        }

        private modifyChargeCommissionToDisplayForm(rate: number): number {
            return parseFloat((rate * SettingType5Item.roundingFactor).toFixed(2));
        }
    }
}