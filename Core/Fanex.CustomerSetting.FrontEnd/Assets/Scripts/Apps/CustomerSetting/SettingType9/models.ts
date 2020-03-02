/// <reference path="../core/common.ts" />
/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType9Item extends DefaultSettingItem {
        public minBet: number;
        public maxBet: number;
        public maxPerNumber: number;
        public upline: SettingType9Item;

        private $minConstraint: number;
        private $specialCasesAllowed: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean, systemId: number);
        constructor(settingItem?: any, backup: boolean = false, systemId: number = 0) {
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

            var minConstraintBySystem = System.isY7(systemId) ? 1 : 0;
            var uplineMinBet = this.upline ? this.upline.minBet : 0;

            this.$minConstraint = Math.max(minConstraintBySystem, uplineMinBet);
            this.$specialCasesAllowed = !System.isY7(systemId);
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
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        minBet: backupMinBet,
                        maxBet: backupMaxBet,
                        maxPerNumber: backupMaxPerNumber
                    });
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (!this.validateRequiredFields()) {
                return false;
            }

            if (!this.validateMinBet()) {
                return false;
            }

            if (!this.validateMaxBet()) {
                return false;
            }

            if (!this.validateMaxPerNumber()) {
                return false;
            }

            return true;
        }

        public validateRequiredFields(): boolean {
            this.resetValidationState();

            if (this.minBet === null) {
                this.validationState = new ValidationResult("RequiredField", "minBet");

                return false;
            }

            if (this.maxBet === null) {
                this.validationState = new ValidationResult("RequiredField", "maxBet");

                return false;
            }

            if (this.maxPerNumber === null) {
                this.validationState = new ValidationResult("RequiredField", "maxPerNumber");

                return false;
            }

            return true;
        }

        public validateSpecialCase(): boolean {
            if (this.$specialCasesAllowed
                && this.minBet === 0
                && this.maxBet >= 0 && this.maxBet <= this.maxPerNumber
                && this.maxPerNumber < 1) {
                    return true;
            }

            return false;
        }

        public validateMinBet(): boolean {
            this.resetValidationState();

            if (this.minBet !== null && !this.validateSpecialCase()) {
                if (this.minBet < this.$minConstraint) {
                    this.validationState = new ValidationResult("MinBetMoreThanOrEqualTo", "minBet", [this.$minConstraint]);

                    return false;
                }

                if (this.minBet > this.maxBet) {
                    this.validationState = new ValidationResult("MinBetLessThanOrEqualMaxBet", "minBet");

                    return false;
                }
            }

            return true;
        }

        public validateMaxBet(): boolean {
            this.resetValidationState();

            if (this.maxBet !== null && !this.validateSpecialCase()) {
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

            return true;
        }

        public validateMaxPerNumber(): boolean {
            this.resetValidationState();

            if (this.maxPerNumber !== null && !this.validateSpecialCase()) {
                if (this.upline.maxPerNumber > 0 && this.maxPerNumber > this.upline.maxPerNumber) {
                    this.validationState = new ValidationResult("MaxPerNumberLessThanOrEqualTo", "maxPerNumber", [this.upline.maxPerNumber]);

                    return false;
                }

                if (this.maxBet > this.maxPerNumber) {
                    this.validationState = new ValidationResult("MaxPerNumberGreaterThanOrEqualMaxBet", "maxPerNumber");

                    return false;
                }
            }

            return true;
        }
    }
}