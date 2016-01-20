/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType7Item extends DefaultSettingItem {
        public stakeLevelValues: Array<SettingType7StakeLevelValue>;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, customerLevel: Level);
        constructor(settingItem: any, customerLevel: Level, backup: boolean)
        constructor(settingItem?: any, customerLevel?: Level, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.stakeLevelValues = new Array<SettingType7StakeLevelValue>();

                if (settingItem.stakeLevelValues) {
                    this.stakeLevelValues = settingItem.stakeLevelValues.map((stakeLevelValue: any) => {
                        return new SettingType7StakeLevelValue(stakeLevelValue, true);
                    });
                }
            }

            if (backup) {
                this.oldSetting = new DefaultSettingItem(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var hasChange: boolean;

            hasChange = this.stakeLevelValues.some((stakeLevelValue: SettingType7StakeLevelValue) => {
                return stakeLevelValue.isChanged();
            });

            return isChanged || hasChange;
        }

        public toJson(): {} {
            var settingItem: any = {
                sportTypeId: this.sportTypeId,
                sportTypeName: this.sportTypeName,
                betTypeId: this.betTypeId,
                betTypeName: this.betTypeName,
                typeName: this.typeName,
                stakeLevelValues: []
            };

            if (this.isLive !== undefined) {
                settingItem.isLive = this.isLive;
            }

            this.stakeLevelValues.forEach((stakeLevel: SettingType7StakeLevelValue) => {
                if (stakeLevel.stakeLevelId) {
                    settingItem.stakeLevelValues.push(stakeLevel.toJson());
                }
            });

            return settingItem;
        }
    }

    export class SettingType7StakeLevelValue {
        public stakeLevelId: number;
        public stakeLevelRange: string;
        public selected: boolean;

        private oldStakeLevelValue: SettingType7StakeLevelValue;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            if (settingItem) {
                this.stakeLevelId = settingItem.stakeLevelId;
                this.stakeLevelRange = settingItem.stakeLevelRange;
                this.selected = settingItem.selected;
            }

            if (backup) {
                this.oldStakeLevelValue = new SettingType7StakeLevelValue(this);
            }
        }

        public isChanged(): boolean {
            return this.selected !== this.oldStakeLevelValue.selected;
        }

        public toJson(): {} {
            return {
                stakeLevelId: this.stakeLevelId,
                stakeLevelRange: this.stakeLevelRange,
                selected: this.selected
            };
        }
    }
}