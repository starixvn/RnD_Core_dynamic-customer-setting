/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType5Item extends DefaultSettingItem {
        public stakeLevels: Array<StakeLevel>;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.stakeLevels = new Array<StakeLevel>();

                if (settingItem.stakeLevels) {
                    this.stakeLevels = settingItem.stakeLevels.map((stakeLevel: any) => {
                        return new StakeLevel(stakeLevel, true);
                    });
                }
            }

            if (backup) {
                this.oldSetting = new DefaultSettingItem(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var hasChange: boolean = this.stakeLevels.some((stakeLevel: StakeLevel) => {
                return stakeLevel.isChanged();
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
                stakeLevels: []
            };

            if (this.isLive !== undefined) {
                settingItem.isLive = this.isLive;
            }

            this.stakeLevels.forEach((stakeLevel: StakeLevel) => {
                if (stakeLevel.stakeLevelId) {
                    settingItem.stakeLevels.push(stakeLevel.toJson());
                }
            });

            return settingItem;
        }
    }

    export class StakeLevel {
        public stakeLevelId: number;
        public min: number;
        public max: number;
        public selected: boolean;

        private oldStakeLevel: StakeLevel;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            if (settingItem) {
                this.stakeLevelId = settingItem.stakeLevelId;
                this.min = settingItem.min;
                this.max = settingItem.max;
                this.selected = settingItem.selected;
            }

            if (backup) {
                this.oldStakeLevel = new StakeLevel(this);
            }
        }

        public isChanged(): boolean {
            return this.selected !== this.oldStakeLevel.selected;
        }

        public toJson(): {} {
            return {
                stakeLevelId: this.stakeLevelId,
                min: this.min,
                max: this.max,
                selected: this.selected
            };
        }
    }
}