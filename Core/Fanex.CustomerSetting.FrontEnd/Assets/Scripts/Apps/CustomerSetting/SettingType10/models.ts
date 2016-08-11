/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType10Config {
        public static get step(): number {
            return 0.0005;
        }

        public static get roundingFactor(): number {
            return 10000;
        }
    }

    export class SettingType10Item extends DefaultSettingItem {
        public rate: number;
        public upline: SettingType10Item;
        public groupId: number;
        public groupValue: string;
        public groups: CommissionGroup[];

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup?: boolean, customerLevel?: Level)
        constructor(settingItem?: any, backup: boolean = false, customerLevel?: Level) {
            super(settingItem);

            if (settingItem) {
                if (settingItem.rate) {
                    this.rate = settingItem.rate;
                }

                if (settingItem.upline) {
                    this.upline = new SettingType10Item(settingItem.upline);
                }

                if (customerLevel === Level.member) {
                    this.groupId = settingItem.groupId;
                    this.groupValue = settingItem.groupValue;
                }

                if (settingItem.groups && settingItem.groups.length > 0) {
                    this.groups = settingItem.groups.map((group: any) => new CommissionGroup(group));
                }
            }

            if (backup) {
                this.oldSetting = new SettingType10Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType10Item = <SettingType10Item>this.oldSetting;

            if (this.rate !== undefined) {
                isChanged = isChanged || this.rate !== oldSetting.rate;
            }

            if (this.groups) {
                isChanged = isChanged || this.groups.some((group: CommissionGroup) => group.isChanged());
            }

            return isChanged;
        }

        public validate(): boolean {
            if (this.rate !== undefined) {
                if (this.rate < 0) {
                    this.validationState = new ValidationResult("CommissionCannotBeNegative", "rate");

                    return false;
                }

                if (this.upline && this.rate > this.upline.rate) {
                    this.validationState = new ValidationResult("CommissionMustBeLessThan", "rate", [this.upline.rate]);

                    return false;
                }
            }

            if (this.groups) {
                if (this.groups.some((group: CommissionGroup) => !group.validate())) {
                    return false;
                }
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }
    }

    export class CommissionGroup {
        public id: number;
        public value: string;
        public displayingName: string;
        public rate: number;
        public maxRate: number;
        public oldRate: number;
        public validationState: ValidationResult;

        constructor(group: any) {
            if (group) {
                this.id = group.id;
                this.value = group.value;
                this.displayingName = group.displayingName;
                this.rate = group.rate;
                this.maxRate = group.maxRate;
                this.oldRate = this.rate;
            }
        }

        public isChanged(): boolean {
            return this.rate !== this.oldRate;
        }

        public validate(): boolean {
            if (this.rate < 0) {
                this.validationState = new ValidationResult("CommissionCannotBeNegative", "rate");

                return false;
            }

            if (this.rate > this.maxRate) {
                this.validationState = new ValidationResult("CommissionMustBeLessThan", "rate", [this.maxRate]);

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        public toJson(): {} {
            return {
                id: this.id,
                rate: this.rate,
                maxRate: this.maxRate
            };
        }
    }
}