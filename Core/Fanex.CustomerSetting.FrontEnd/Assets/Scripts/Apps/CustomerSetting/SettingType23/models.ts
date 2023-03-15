/// <reference path="../core/common.ts" />
/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType23Item extends DefaultSettingItem {
        public minBet: number;
        public maxBet: number;
        public maxPerBall: number;
        public maxPerMatch: number;
        public subType: number;
        public upline: SettingType23Item;

        private $minConstraint: number;
        private $specialCasesAllowed: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean, systemId: number);
        constructor(settingItem?: any, backup: boolean = false, systemId: number = 0) {
            super(settingItem);

            if (settingItem) {
                this.subType = settingItem.subType;
                this.minBet = settingItem.minBet;
                this.maxBet = settingItem.maxBet;
                this.maxPerBall = settingItem.maxPerBall;
                this.maxPerMatch = settingItem.maxPerMatch;

                if (settingItem.upline) {
                    this.upline = new SettingType23Item(settingItem.upline);
                }
            }

            if (backup) {
                this.oldSetting = new SettingType23Item(this);
            }

            var minConstraintBySystem = System.isY7(systemId) ? 1 : 0;
            var uplineMinBet = this.upline ? this.upline.minBet : 0;

            this.$minConstraint = Math.max(minConstraintBySystem, uplineMinBet);
            this.$specialCasesAllowed = !System.isY7(systemId);
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType23Item = <SettingType23Item>this.oldSetting;

            return isChanged
                || this.minBet !== oldSetting.minBet
                || this.maxBet !== oldSetting.maxBet
                || this.maxPerBall !== oldSetting.maxPerBall
                || this.maxPerMatch !== oldSetting.maxPerMatch;
        }

        public copySettingFrom(source: SettingType23Item): boolean {
            var isSuccessful: boolean = true;
            var backupMinBet: number;
            var backupMaxBet: number;
            var backupMaxPerBall: number;
            var backupMaxPerMatch: number;

            if (source) {
                backupMinBet = this.minBet;
                backupMaxBet = this.maxBet;
                backupMaxPerBall = this.maxPerBall;
                backupMaxPerMatch = this.maxPerMatch;

                this.minBet = source.minBet;
                this.maxBet = source.maxBet;
                this.maxPerBall = this.maxPerBall ? source.maxPerBall : this.maxPerBall;
                this.maxPerMatch = source.maxPerMatch;

                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.minBet = backupMinBet;
                    this.maxBet = backupMaxBet;
                    this.maxPerBall = backupMaxPerBall;
                    this.maxPerMatch = backupMaxPerMatch;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        minBet: backupMinBet,
                        maxBet: backupMaxBet,
                        maxPerBall: backupMaxPerBall,
                        maxPerMatch: backupMaxPerMatch
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

            if (!this.validateMaxPerMatch()) {
                return false;
            }

            if (this.maxPerBall && !this.validateMaxPerBall()) {
                return false;
            }

            return true;
        }

        public validateRequiredFields(): boolean {
            this.resetValidationState();
            var oldSetting: SettingType23Item = <SettingType23Item>this.oldSetting;

            if (this.minBet === null) {
                this.validationState = new ValidationResult("RequiredField", "minBet");

                return false;
            }

            if (this.maxBet === null) {
                this.validationState = new ValidationResult("RequiredField", "maxBet");

                return false;
            }

            if (this.maxPerMatch === null) {
                this.validationState = new ValidationResult("RequiredField", "maxPerMatch");

                return false;
            }

            if (oldSetting.maxPerBall !== null && this.maxPerBall === null) {
                this.validationState = new ValidationResult("RequiredField", "maxPerBall");

                return false;
            }

            return true;
        }

        public validateSpecialCase(): boolean {
            if (this.$specialCasesAllowed
                && this.minBet === 0
                && this.maxBet >= 0 && this.maxBet <= this.maxPerMatch
                && this.maxPerMatch < 1) {
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

                if (this.maxBet > this.maxPerMatch) {
                    this.validationState = new ValidationResult("MaxBetLessThanOrEqualMaxPerMatch", "maxBet");

                    return false;
                }

                if (this.maxBet < this.minBet) {
                    this.validationState = new ValidationResult("MaxBetMoreThanOrEqualMinBet", "maxBet");

                    return false;
                }
            }

            return true;
        }

        public validateMaxPerMatch(): boolean {
            this.resetValidationState();

            if (this.maxPerMatch !== null && !this.validateSpecialCase()) {
                if (this.upline.maxPerMatch > 0 && this.maxPerMatch > this.upline.maxPerMatch) {
                    this.validationState = new ValidationResult("MaxPerMatchLessThanOrEqualTo", "maxPerMatch", [this.upline.maxPerMatch]);

                    return false;
                }

                if (this.maxBet > this.maxPerMatch) {
                    this.validationState = new ValidationResult("MaxPerMatchMoreThanOrEqualMaxBet", "maxPerMatch");

                    return false;
                }
            }

            return true;
        }

        public validateMaxPerBall(): boolean {
            this.resetValidationState();

            if (this.maxPerMatch !== null && !this.validateSpecialCase()) {
                if (this.upline.maxPerBall > 0 && this.maxPerBall > this.upline.maxPerBall) {
                    this.validationState = new ValidationResult("MaxPerBallLessThanOrEqualTo", "maxPerBall", [this.upline.maxPerBall]);

                    return false;
                }

                if (this.maxBet > this.maxPerBall) {
                    this.validationState = new ValidationResult("MaxPerBallMoreThanOrEqualMaxBet", "maxPerBall");

                    return false;
                }

                if (this.maxPerBall > this.maxPerMatch) {
                    this.validationState = new ValidationResult("MaxPerBallLessThanOrEqualMaxPerMatch", "maxPerBall");

                    return false;
                }
            }

            return true;
        }
    }
}