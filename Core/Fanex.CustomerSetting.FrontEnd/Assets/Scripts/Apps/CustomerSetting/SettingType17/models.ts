/// <reference path="../core/common.ts" />
/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    const numberOfCellsOnEachRow: number = 5;

    export class SettingType17Item extends DefaultSettingItem {
        public stakeRanges: Array<SettingType17StakeRange>;
        public stakeRangeGroups: Array<Array<SettingType17StakeRange>>;
        public selectStakeLevelId: number;
        public tooltipMessage: string;
        public showIconTooltipMessage: boolean;
        public maxChoice: number;

        private systemId: number;
        private customerLevel: Level;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean, customerLevel: Level, systemId: number);
        constructor(settingItem?: any, backup: boolean = false, customerLevel: Level = null, systemId: number = 0) {
            super(settingItem);

            if (settingItem) {
                this.stakeRanges = new Array<SettingType17StakeRange>();

                if (settingItem.stakeRanges) {
                    this.stakeRanges = settingItem.stakeRanges.map((stakeLevelValue: any) => {
                        return new SettingType17StakeRange(stakeLevelValue, true);
                    });
                }

                this.systemId = systemId;
                this.customerLevel = customerLevel;
                this.stakeRangeGroups = this.groupStakeRanges();
                this.tooltipMessage = settingItem.tooltipMessage;
                this.showIconTooltipMessage = settingItem.showIconTooltipMessage;
                this.maxChoice = settingItem.maxChoice;
            }

            if (backup) {
                this.oldSetting = new DefaultSettingItem(this);
                (<SettingType17Item>this.oldSetting).selectStakeLevelId = this.selectStakeLevelId;
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var hasChange: boolean;

            hasChange = this.stakeRanges.some((stakeLevelValue: SettingType17StakeRange) => {
                return stakeLevelValue.isChanged();
            });

            return isChanged || hasChange;
        }

        public validate(): boolean {
            var isViolated: boolean = false;
            if (this.customerLevel === Level.member || System.isY7(this.systemId)) {
                isViolated = this.stakeRanges.filter((stakeRange: SettingType17StakeRange) => stakeRange.selected).length === 0;

                if (isViolated) {
                    this.validationState = new ValidationResult("Y7StakeLevelRequired", this.customerLevel === Level.member ? this.stakeRanges[0].stakeLevelId.toString() : "checkAll");

                    return false;
                }
            }

            this.resetValidationState();

            return true;
        }

        public toJson(): {} {
            var settingItem: any = {
                sportTypeId: this.sportTypeId,
                sportTypeName: this.sportTypeName,
                betTypeId: this.betTypeId,
                betTypeName: this.betTypeName,
                typeName: this.typeName,
                maxChoice: this.maxChoice,
                stakeRanges: []
            };

            if (this.isLive !== undefined) {
                settingItem.isLive = this.isLive;
            }

            this.stakeRanges.forEach((stakeLevel: SettingType17StakeRange) => {
                if (stakeLevel.stakeLevelId) {
                    settingItem.stakeRanges.push(stakeLevel.toJson());
                }
            });

            return settingItem;
        }

        private groupStakeRanges(): Array<Array<SettingType17StakeRange>> {
            var mod: number = this.stakeRanges.length % numberOfCellsOnEachRow;
            var numberOfEmptyCells: number = 0;
            var groups: Array<Array<SettingType17StakeRange>> = new Array<Array<SettingType17StakeRange>>();
            var group: Array<SettingType17StakeRange>;

            if (mod !== 0 || this.stakeRanges.length === 0) {
                numberOfEmptyCells = numberOfCellsOnEachRow - mod;
            }

            this.stakeRanges.forEach((stakeRange: SettingType17StakeRange, index: number) => {
                if (index % numberOfCellsOnEachRow === 0) {
                    group = new Array<SettingType17StakeRange>();
                    groups.push(group);
                }

                group.push(stakeRange);
            });

            for (var counter = 0; counter < numberOfEmptyCells; counter++) {
                group.push(new SettingType17StakeRange());
            }

            return groups;
        };
    }

    export class SettingType17StakeRange {
        public stakeLevelId: number;
        public stakeLevelRange: string;
        public selected: boolean;
        public maxChoice: number;
        public disabled: boolean;

        private oldStakeLevelValue: SettingType17StakeRange;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean);
        constructor(settingItem?: any, backup: boolean = false) {
            if (settingItem) {
                this.stakeLevelId = settingItem.stakeLevelId;
                this.stakeLevelRange = settingItem.stakeLevelRange;
                this.selected = settingItem.selected;
                this.maxChoice = settingItem.maxChoice;
                this.disabled = false;
            }

            if (backup) {
                this.oldStakeLevelValue = new SettingType17StakeRange(this);
            }
        }

        public isChanged(): boolean {
            return this.selected !== this.oldStakeLevelValue.selected;
        }

        public toJson(): {} {
            return {
                stakeLevelId: this.stakeLevelId,
                stakeLevelRange: this.stakeLevelRange,
                selected: this.selected,
                maxChoice: this.maxChoice
            };
        }
    }
}