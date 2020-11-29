/// <reference path="../core/common.ts" />
/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType20Item extends DefaultSettingItem {
        public minBet: number;
        public maxBet: number;
        public upline: SettingType20Item;

        private $minConstraint: number;
        private $specialCasesAllowed: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean, systemId: number, isY7Sites: boolean);
        constructor(settingItem?: any, backup: boolean = false, systemId: number = 0, isY7Sites: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.minBet = settingItem.minBet;
                this.maxBet = settingItem.maxBet;

                if (settingItem.upline) {
                    this.upline = new SettingType20Item(settingItem.upline, false, systemId, isY7Sites);
                }
            }

            if (backup) {
                this.oldSetting = new SettingType20Item(this, false, systemId, isY7Sites);
            }

            var minConstraintBySystem = System.isY7(systemId) ? 1 : 0;
            var uplineMinBet = this.upline ? this.upline.minBet : 0;

            this.$minConstraint = Math.max(minConstraintBySystem, uplineMinBet);
            this.$specialCasesAllowed = !System.isY7(systemId);
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType20Item = <SettingType20Item>this.oldSetting;

            return isChanged
                || this.minBet !== oldSetting.minBet
                || this.maxBet !== oldSetting.maxBet;
        }

        public copySettingFrom(source: SettingType20Item): boolean {
            var isSuccessful: boolean = true;
            var backupMinBet: number;
            var backupMaxBet: number;

            if (source) {
                backupMinBet = this.minBet;
                backupMaxBet = this.maxBet;

                this.minBet = source.minBet;
                this.maxBet = source.maxBet;

                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.minBet = backupMinBet;
                    this.maxBet = backupMaxBet;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        minBet: backupMinBet,
                        maxBet: backupMaxBet
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

            return true;
        }

        public validateSpecialCase(): boolean {
            if (this.$specialCasesAllowed
                && this.minBet === 0
                && this.maxBet >= 0
                && this.maxBet < 1) {
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

                if (this.maxBet < this.minBet) {
                    this.validationState = new ValidationResult("MaxBetMoreThanOrEqualMinBet", "maxBet");

                    return false;
                }
            }

            return true;
        }
    }
}