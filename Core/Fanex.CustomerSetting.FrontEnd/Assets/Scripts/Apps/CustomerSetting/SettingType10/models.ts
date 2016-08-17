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
                if (settingItem.rate !== null && settingItem.rate !== undefined) {
                    this.rate = settingItem.rate;
                }

                if (customerLevel === Level.member) {
                    this.groupId = settingItem.groupId;
                    this.groupValue = settingItem.groupValue;
                }

                if (settingItem.groups && settingItem.groups.length > 0) {
                    this.groups = settingItem.groups.map((group: any) => new CommissionGroup(group));
                }

                if (settingItem.upline) {
                    this.upline = new SettingType10Item(settingItem.upline);
                }
            }

            if (backup) {
                this.oldSetting = new SettingType10Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType10Item = <SettingType10Item>this.oldSetting;

            if (this.rate !== null && this.rate !== undefined) {
                isChanged = isChanged || this.rate !== oldSetting.rate;
            }

            if (this.groupId) {
                isChanged = isChanged || this.groupId !== oldSetting.groupId;
            }

            if (this.groups) {
                isChanged = isChanged || this.groups.some((group: CommissionGroup) => group.isChanged());
            }

            return isChanged;
        }

        public validate(): boolean {
            if (this.rate !== null && this.rate !== undefined) {
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

        public toJson(): {} {
            var settingItem: any = {
                sportTypeId: this.sportTypeId,
                sportTypeName: this.sportTypeName,
                betTypeId: this.betTypeId,
                betTypeName: this.betTypeName,
                typeName: this.typeName
            };

            if (this.isLive !== undefined) {
                settingItem.isLive = this.isLive;
            }

            if (this.rate !== null && this.rate !== undefined) {
                settingItem.rate = this.rate;
            }

            if (this.groupId) {
                settingItem.groupId = this.groupId;
                settingItem.groupValue = this.groupValue;
            }

            if (this.groups) {
                settingItem.groups = [];
                this.groups.forEach((group: CommissionGroup) => {
                    if (group.isChanged()) {
                        settingItem.groups.push(group.toJson());
                    }
                });
            }

            if (this.upline) {
                settingItem.upline = this.upline.toJson();
            }

            return settingItem;
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

        constructor()
        constructor(group: any);
        constructor(group?: any) {
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
                value: this.value,
                rate: this.rate,
                maxRate: this.maxRate
            };
        }
    }
}