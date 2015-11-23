/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType2Item extends DefaultSettingItem {
        public minBet: number;
        public maxBet: number;
        public maxPerMatch: number;
        public upline: SettingType2Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.minBet = settingItem.minBet;
                this.maxBet = settingItem.maxBet;
                this.maxPerMatch = settingItem.maxPerMatch;

                if (settingItem.upline) {
                    this.upline = new SettingType2Item(settingItem.upline);
                }
            }

            if (backup) {
                this.oldSetting = new SettingType2Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType2Item = <SettingType2Item>this.oldSetting;

            return isChanged
                || this.minBet !== oldSetting.minBet
                || this.maxBet !== oldSetting.maxBet
                || this.maxPerMatch !== oldSetting.maxPerMatch;
        }

        public copySettingFrom(source: SettingType2Item): boolean {
            var isSuccessful: boolean = true;
            var backupMinBet: number;
            var backupMaxBet: number;
            var backupMaxPerMatchBet: number;

            if (source) {
                backupMinBet = this.minBet;
                backupMaxBet = this.maxBet;
                backupMaxPerMatchBet = this.maxPerMatch;

                this.minBet = source.minBet;
                this.maxBet = source.maxBet;
                this.maxPerMatch = source.maxPerMatch;

                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.minBet = backupMinBet;
                    this.maxBet = backupMaxBet;
                    this.maxPerMatch = backupMaxPerMatchBet;
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

            if (!this.validateMaxPerMatch()) {
                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        public validateSpecialCase(): boolean {
            if (this.minBet === this.maxBet && this.maxBet === this.maxPerMatch && this.maxPerMatch === 0) {
                this.validationState = <ValidationResult>Result.normal;

                return true;
            }
        }

        public validateMinBet(): boolean {
            if (this.validateSpecialCase()) {
                return true;
            }

            if (this.minBet < this.upline.minBet) {
                this.validationState = new ValidationResult("MinBetMoreThanOrEqualTo", "minBet", [this.upline.minBet]);

                return false;
            }

            if (this.minBet > this.maxBet) {
                this.validationState = new ValidationResult("MinBetLessThanOrEqualMaxBet", "minBet");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        public validateMaxBet(): boolean {
            var unlimitedUpline: boolean = this.upline.minBet === 0 && this.upline.maxBet === 0 && this.upline.maxPerMatch === 0;

            if (this.validateSpecialCase()) {
                return true;
            }

            if (!unlimitedUpline && this.maxBet > this.upline.maxBet) {
                this.validationState = new ValidationResult("MaxBetLessThanOrEqualTo", "maxBet", [this.upline.maxBet]);

                return false;
            }

            if (this.maxBet > this.maxPerMatch) {
                this.validationState = new ValidationResult("MaxBetLessThanOrEqualMaxPerMatch", "maxBet");

                return false;
            }

            if (this.maxBet < this.minBet) {
                this.validationState = new ValidationResult("MaxBetMoreThanOrEqualMinBet", "maxBet");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        public validateMaxPerMatch(): boolean {
            var unlimitedUpline: boolean = this.upline.minBet === 0 && this.upline.maxBet === 0 && this.upline.maxPerMatch === 0;

            if (this.validateSpecialCase()) {
                return true;
            }

            if (!unlimitedUpline && this.maxPerMatch > this.upline.maxPerMatch) {
                this.validationState = new ValidationResult("MaxPerMatchLessThanOrEqualTo", "maxPerMatch", [this.upline.maxPerMatch]);

                return false;
            }

            if (this.maxBet > this.maxPerMatch) {
                this.validationState = new ValidationResult("MaxPerMatchMoreThanOrEqualMaxBet", "maxPerMatch");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }
    }
}