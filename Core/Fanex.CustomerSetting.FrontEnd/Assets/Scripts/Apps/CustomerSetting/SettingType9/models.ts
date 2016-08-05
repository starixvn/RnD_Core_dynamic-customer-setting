/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType9Item extends DefaultSettingItem {
        public minBet: number;
        public maxBet: number;
        public maxPerNumber: number;
        public upline: SettingType9Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.minBet = settingItem.minBet;
                this.maxBet = settingItem.maxBet;
                this.maxPerNumber = settingItem.maxPerNumber;

                if (settingItem.upline) {
                    this.upline = new SettingType9Item(settingItem.upline);
                }
            }

            if (backup) {
                this.oldSetting = new SettingType9Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType9Item = <SettingType9Item>this.oldSetting;

            return isChanged
                || this.minBet !== oldSetting.minBet
                || this.maxBet !== oldSetting.maxBet
                || this.maxPerNumber !== oldSetting.maxPerNumber;
        }

        public copySettingFrom(source: SettingType9Item): boolean {
            var isSuccessful: boolean = true;
            var backupMinBet: number;
            var backupMaxBet: number;
            var backupMaxPerNumber: number;

            if (source) {
                backupMinBet = this.minBet;
                backupMaxBet = this.maxBet;
                backupMaxPerNumber = this.maxPerNumber;

                this.minBet = source.minBet;
                this.maxBet = source.maxBet;
                this.maxPerNumber = source.maxPerNumber;

                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.minBet = backupMinBet;
                    this.maxBet = backupMaxBet;
                    this.maxPerNumber = backupMaxPerNumber;
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (!this.validateMinBet()) {
                return false;
            }

            if (!this.validateMaxBet()) {
                return false;
            }

            if (!this.validateMaxPerNumber()) {
                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        public validateSpecialCase(): boolean {
            if (this.minBet === 0 && this.maxBet === 0 && this.maxPerNumber === 0) {
                this.validationState = <ValidationResult>Result.normal;

                return true;
            }
        }

        public validateMinBet(): boolean {
            if (this.validateSpecialCase()) {
                return true;
            }

            if (this.minBet !== 0) {
                if (this.minBet < this.upline.minBet) {
                    this.validationState = new ValidationResult("MinBetMoreThanOrEqualTo", "minBet", [this.upline.minBet]);

                    return false;
                }

                if (this.minBet > this.maxBet) {
                    this.validationState = new ValidationResult("MinBetLessThanOrEqualMaxBet", "minBet");

                    return false;
                }
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        public validateMaxBet(): boolean {
            if (this.validateSpecialCase()) {
                return true;
            }

            if (this.maxBet !== 0) {
                if (this.upline.maxBet > 0 && this.maxBet > this.upline.maxBet) {
                    this.validationState = new ValidationResult("MaxBetLessThanOrEqualTo", "maxBet", [this.upline.maxBet]);

                    return false;
                }

                if (this.maxBet > this.maxPerNumber) {
                    this.validationState = new ValidationResult("MaxBetLessThanOrEqualMaxPerNumber", "maxBet");

                    return false;
                }

                if (this.maxBet < this.minBet) {
                    this.validationState = new ValidationResult("MaxBetMoreThanOrEqualMinBet", "maxBet");

                    return false;
                }
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        public validateMaxPerNumber(): boolean {
            if (this.validateSpecialCase()) {
                return true;
            }

            if (this.maxPerNumber !== 0) {
                if (this.upline.maxPerNumber > 0 && this.maxPerNumber > this.upline.maxPerNumber) {
                    this.validationState = new ValidationResult("MaxPerNumberLessThanOrEqualTo", "maxPerNumber", [this.upline.maxPerNumber]);

                    return false;
                }

                if (this.maxBet > this.maxPerNumber) {
                    this.validationState = new ValidationResult("MaxPerNumberMoreThanOrEqualMaxBet", "maxPerNumber");

                    return false;
                }
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }
    }
}