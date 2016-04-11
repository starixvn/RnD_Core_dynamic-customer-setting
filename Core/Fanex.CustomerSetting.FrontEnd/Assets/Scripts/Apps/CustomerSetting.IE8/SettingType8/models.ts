/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="../core/models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType8Item extends DefaultSettingItem {
        public groups: Array<SettingType8LevelGroup>;
        public details: Array<SettingType8StakeLevel>;
        public selectedLevelId: number;
        public hasMoreDetails: boolean;
        public allDetails: SettingType;
        public showingDetails: boolean;
        public styles: {};

        private customerLevel: Level;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, customerLevel: Level);
        constructor(settingItem: any, customerLevel: Level, backup: boolean)
        constructor(settingItem?: any, customerLevel?: Level, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.groups = new Array<SettingType8LevelGroup>();

                if (settingItem.groups) {
                    this.groups = _.map(settingItem.groups, (group: any) => {
                        return new SettingType8LevelGroup(group, true);
                    });
                }

                if (settingItem.details) {
                    this.details = _.map(settingItem.details, (item: any) => {
                        return new SettingType8StakeLevel(item, this.groups);
                    });
                }

                this.hasMoreDetails = settingItem.hasMoreDetails;
                this.customerLevel = customerLevel;
                this.styles = {
                    "min-width": (100 / this.groups.length) + "%"
                };

                if (this.customerLevel && this.customerLevel === Level.member) {
                    var selectedGroups: SettingType8LevelGroup[]
                        = _.filter(this.groups, (group: SettingType8LevelGroup) => group.selected);

                    if (selectedGroups && selectedGroups.length > 0) {
                        this.selectedLevelId = selectedGroups[0].levelId;
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
                hasChange = _.some(this.groups, (group: SettingType8LevelGroup) => {
                    return group.isChanged();
                });
            }

            return isChanged || hasChange;
        }

        public validate(): boolean {
            if (this.customerLevel && this.customerLevel === Level.member) {
                var isViolated: boolean
                    = _.filter(this.groups, (group: SettingType8LevelGroup) => group.selected).length !== 1;

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
                groups: []
            };

            if (this.isLive !== undefined) {
                settingItem.isLive = this.isLive;
            }

            if (this.customerLevel && this.customerLevel === Level.member) {
                _.forEach(this.groups, (group: SettingType8LevelGroup) => {
                    group.selected = this.selectedLevelId === group.levelId;
                });
            }

            _.forEach(this.groups, (group: SettingType8LevelGroup) => {
                if (group.levelId) {
                    settingItem.groups.push(group.toJson());
                }
            });

            return settingItem;
        }
    }

    export class SettingType8LevelGroup {
        public levelId: number;
        public levelGroupId: number;
        public levelGroupName: string;
        public selected: boolean;

        private oldStakeLevelGroupValue: SettingType8LevelGroup;

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
                this.oldStakeLevelGroupValue = new SettingType8LevelGroup(this);
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

    export class SettingType8StakeLevel extends DefaultSettingItem {
        public stakeRanges: Array<SettingType8StakeRange>;

        constructor();
        constructor(stakeLevelDetailedItem: any, levelGroups: SettingType8LevelGroup[])
        constructor(stakeLevelDetailedItem?: any, levelGroups?: SettingType8LevelGroup[]) {
            super(stakeLevelDetailedItem);

            if (stakeLevelDetailedItem) {
                this.stakeRanges = this.fillLevelGroupDetails(stakeLevelDetailedItem.stakeRanges, levelGroups);
            }
        }

        private fillLevelGroupDetails(rawStakeRanges: any[], levelGroups: SettingType8LevelGroup[]): SettingType8StakeRange[] {
            var stakeRanges: SettingType8StakeRange[];

            if (levelGroups && levelGroups.length > 0 && rawStakeRanges && rawStakeRanges.length > 0) {
                stakeRanges = _.map(levelGroups, (levelGroup: SettingType8LevelGroup) => {
                    var matchingRanges: any = _.filter(rawStakeRanges, (range: any) => range.id === levelGroup.levelGroupId);

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