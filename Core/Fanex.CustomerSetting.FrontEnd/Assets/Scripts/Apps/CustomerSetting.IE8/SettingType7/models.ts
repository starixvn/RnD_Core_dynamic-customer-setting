/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="../core/models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    const numberOfCellsOnEachRow: number = 5;

    export class SettingType7Item extends DefaultSettingItem {
        public stakeRanges: Array<SettingType7StakeRange>;
        public stakeRangeGroups: Array<Array<SettingType7StakeRange>>;
        public selectStakeLevelId: number;

        private customerLevel: Level;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, customerLevel: Level);
        constructor(settingItem: any, customerLevel: Level, backup: boolean)
        constructor(settingItem?: any, customerLevel?: Level, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.stakeRanges = new Array<SettingType7StakeRange>();

                if (settingItem.stakeRanges) {
                    this.stakeRanges = _.map(settingItem.stakeRanges, (stakeLevelValue: any) => {
                        return new SettingType7StakeRange(stakeLevelValue, true);
                    });
                }

                this.customerLevel = customerLevel;

                if (this.customerLevel && this.customerLevel === Level.member) {
                    var selectedStakeRanges: SettingType7StakeRange[] = _.filter(this.stakeRanges, (stakeRange: SettingType7StakeRange) => stakeRange.selected);

                    if (selectedStakeRanges && selectedStakeRanges.length > 0) {
                        this.selectStakeLevelId = selectedStakeRanges[0].stakeLevelId;
                    }
                }

                this.stakeRangeGroups = this.groupStakeRanges();
            }

            if (backup) {
                this.oldSetting = new DefaultSettingItem(this);
                (<SettingType7Item>this.oldSetting).selectStakeLevelId = this.selectStakeLevelId;
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var hasChange: boolean;

            hasChange = _.some(this.stakeRanges, (stakeLevelValue: SettingType7StakeRange) => {
                return stakeLevelValue.isChanged();
            });

            if (this.customerLevel && this.customerLevel === Level.member) {
                hasChange = hasChange || (this.selectStakeLevelId !== (<SettingType7Item>this.oldSetting).selectStakeLevelId);
            }

            return isChanged || hasChange;
        }

        public validate(): boolean {
            if (this.customerLevel && this.customerLevel === Level.member) {
                var isViolated: boolean = _.filter(this.stakeRanges, (stakeRange: SettingType7StakeRange) => stakeRange.selected).length != 1;

                if (isViolated) {
                    this.validationState = new ValidationResult("MemberStakeLevelMustBeSelectedForBetType");

                    return false;
                }
            }

            return true;
        };

        public toJson(): {} {
            var settingItem: any = {
                sportTypeId: this.sportTypeId,
                sportTypeName: this.sportTypeName,
                betTypeId: this.betTypeId,
                betTypeName: this.betTypeName,
                typeName: this.typeName,
                stakeRanges: []
            };

            if (this.isLive !== undefined) {
                settingItem.isLive = this.isLive;
            }

            if (this.customerLevel && this.customerLevel === Level.member) {
                _.forEach(this.stakeRanges, (stakeLevel: SettingType7StakeRange) => {
                    stakeLevel.selected = this.selectStakeLevelId === stakeLevel.stakeLevelId;
                });
            }

            _.forEach(this.stakeRanges, (stakeLevel: SettingType7StakeRange) => {
                if (stakeLevel.stakeLevelId) {
                    settingItem.stakeRanges.push(stakeLevel.toJson());
                }
            });

            return settingItem;
        }

        private groupStakeRanges(): Array<Array<SettingType7StakeRange>> {
            var mod: number = this.stakeRanges.length % numberOfCellsOnEachRow;
            var numberOfEmptyCells: number = 0;
            var groups: Array<Array<SettingType7StakeRange>> = new Array<Array<SettingType7StakeRange>>();
            var group: Array<SettingType7StakeRange>;

            if (mod !== 0 || this.stakeRanges.length === 0) {
                numberOfEmptyCells = numberOfCellsOnEachRow - mod;
            }

            _.forEach(this.stakeRanges, (stakeRange: SettingType7StakeRange, index: number) => {
                if (index % numberOfCellsOnEachRow === 0) {
                    group = new Array<SettingType7StakeRange>();
                    groups.push(group);
                }

                group.push(stakeRange);
            });

            for (var counter = 0; counter < numberOfEmptyCells; counter++) {
                group.push(new SettingType7StakeRange());
            }

            return groups;
        };
    }

    export class SettingType7StakeRange {
        public stakeLevelId: number;
        public stakeLevelRange: string;
        public selected: boolean;

        private oldStakeLevelValue: SettingType7StakeRange;

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
                this.oldStakeLevelValue = new SettingType7StakeRange(this);
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