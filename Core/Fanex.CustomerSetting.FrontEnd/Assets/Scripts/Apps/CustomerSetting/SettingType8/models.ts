/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType8Item extends DefaultSettingItem {
        public levelGroups: Array<SettingType8StakeLevelGroup>;
        public stakeLevelDetailedItems: Array<SettingType8StakeLevelDetailedItem>;
        public selectedLevelId: number;
        public numberOfDetailedItems: number;

        private customerLevel: Level;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, customerLevel: Level);
        constructor(settingItem: any, customerLevel: Level, backup: boolean)
        constructor(settingItem?: any, customerLevel?: Level, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.levelGroups = new Array<SettingType8StakeLevelGroup>();

                if (settingItem.levelGroups) {
                    this.levelGroups = settingItem.levelGroups.map((levelGroup: any) => {
                        return new SettingType8StakeLevelGroup(levelGroup, true);
                    });
                }

                if (settingItem.stakeLevelDetailedItems) {
                    this.stakeLevelDetailedItems = settingItem.stakeLevelDetailedItems.map((stakeLevelDetailedItem: any) => {
                        return new SettingType8StakeLevelDetailedItem(stakeLevelDetailedItem, this.levelGroups);
                    });
                }

                this.numberOfDetailedItems = settingItem.numberOfDetailedItems;
                this.customerLevel = customerLevel;

                if (this.customerLevel && this.customerLevel === Level.member) {
                    var selectedLevelGroups: SettingType8StakeLevelGroup[]
                        = this.levelGroups.filter((levelGroup: SettingType8StakeLevelGroup) => levelGroup.selected);

                    if (selectedLevelGroups && selectedLevelGroups.length > 0) {
                        this.selectedLevelId = selectedLevelGroups[0].levelId;
                    }
                }
            }

            if (backup) {
                this.oldSetting = new SettingType8Item(this, this.customerLevel);
                (<SettingType8Item>this.oldSetting).selectedLevelId = this.selectedLevelId;
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var hasChange: boolean;

            if (this.customerLevel && this.customerLevel === Level.member) {
                hasChange = this.selectedLevelId !== (<SettingType8Item>this.oldSetting).selectedLevelId;
            } else {
                hasChange = this.levelGroups.some((stakeLevelGroup: SettingType8StakeLevelGroup) => {
                    return stakeLevelGroup.isChanged();
                });
            }

            return isChanged || hasChange;
        }

        public validate(): boolean {
            if (this.customerLevel && this.customerLevel === Level.member) {
                var isViolated: boolean
                    = this.levelGroups.filter((levelGroup: SettingType8StakeLevelGroup) => levelGroup.selected).length !== 1;

                if (isViolated) {
                    this.validationState = new ValidationResult("MemberStakeLevelGroupMustBeSelected");

                    return false;
                }
            }

            return true;
        }

        public toJson(): {} {
            var settingItem: any = {
                sportTypeId: this.sportTypeId,
                sportTypeName: this.sportTypeName,
                betTypeId: this.betTypeId,
                betTypeName: this.betTypeName,
                typeName: this.typeName,
                levelGroups: []
            };

            if (this.isLive !== undefined) {
                settingItem.isLive = this.isLive;
            }

            if (this.customerLevel && this.customerLevel === Level.member) {
                this.levelGroups.forEach((levelGroup: SettingType8StakeLevelGroup) => {
                    levelGroup.selected = this.selectedLevelId === levelGroup.levelId;
                });
            }

            this.levelGroups.forEach((levelGroup: SettingType8StakeLevelGroup) => {
                if (levelGroup.levelId) {
                    settingItem.levelGroups.push(levelGroup.toJson());
                }
            });

            return settingItem;
        }
    }

    export class SettingType8StakeLevelGroup {
        public levelId: number;
        public levelGroupId: number;
        public levelGroupName: string;
        public selected: boolean;

        private oldStakeLevelGroupValue: SettingType8StakeLevelGroup;

        constructor();
        constructor(stakeLevelGroup: any);
        constructor(stakeLevelGroup: any, backup: boolean)
        constructor(stakeLevelGroup?: any, backup: boolean = false) {
            if (stakeLevelGroup) {
                this.levelId = stakeLevelGroup.levelId;
                this.levelGroupId = stakeLevelGroup.levelGroupId;
                this.levelGroupName = stakeLevelGroup.levelGroupName;
                this.selected = stakeLevelGroup.selected;
            }

            if (backup) {
                this.oldStakeLevelGroupValue = new SettingType8StakeLevelGroup(this);
            }
        }

        public isChanged(): boolean {
            return this.selected !== this.oldStakeLevelGroupValue.selected;
        }

        public toJson(): {} {
            return {
                levelId: this.levelId,
                levelGroupId: this.levelGroupId,
                selected: this.selected
            };
        }
    }

    export class SettingType8StakeLevelDetailedItem extends DefaultSettingItem {
        public stakeRanges: Array<SettingType8StakeRange>;
        public rowHeight: number;
        public expectedRowHeight: number;

        constructor();
        constructor(stakeLevelDetailedItem: any, levelGroups: SettingType8StakeLevelGroup[])
        constructor(stakeLevelDetailedItem?: any, levelGroups?: SettingType8StakeLevelGroup[]) {
            super(stakeLevelDetailedItem);
            this.expectedRowHeight = 23; // default row height is 23px.

            if (stakeLevelDetailedItem) {
                this.stakeRanges = this.fillLevelGroupDetails(stakeLevelDetailedItem.stakeRanges, levelGroups);
            }
        }

        private fillLevelGroupDetails(rawStakeRanges: any[], levelGroups: SettingType8StakeLevelGroup[]): SettingType8StakeRange[] {
            var stakeRanges: SettingType8StakeRange[];

            if (levelGroups && levelGroups.length > 0 && rawStakeRanges && rawStakeRanges.length > 0) {
                stakeRanges = levelGroups.map((levelGroup: SettingType8StakeLevelGroup) => {
                    var matchingRanges: any = rawStakeRanges.filter((range: any) => range.id === levelGroup.levelGroupId);

                    if (matchingRanges && matchingRanges.length > 0) {
                        var range: SettingType8StakeRange = new SettingType8StakeRange(matchingRanges[0]);

                        return range;
                    } else {
                        return new SettingType8StakeRange();
                    }
                });
            }

            return stakeRanges || new Array<SettingType8StakeRange>();
        }
    }

    export class SettingType8StakeRange {
        public id: number;
        public value: string;

        constructor();
        constructor(stakeRange: any)
        constructor(stakeRange?: any) {
            if (stakeRange) {
                this.id = stakeRange.id;
                this.value = stakeRange.value || "";
            }
        }
    }
}