/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType8Item extends DefaultSettingItem {
        public stakeLevelGroups: Array<SettingType8StakeLevelGroup>;
        public stakeLevelGroupsDetails: Array<SettingType8StakeLevelGroupDetailItem>;
        public selectedStakeLevelGroupId: number;

        private customerLevel: Level;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, customerLevel: Level);
        constructor(settingItem: any, customerLevel: Level, backup: boolean)
        constructor(settingItem?: any, customerLevel?: Level, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.stakeLevelGroups = new Array<SettingType8StakeLevelGroup>();

                if (settingItem.stakeLevelGroups) {
                    this.stakeLevelGroups = settingItem.stakeLevelGroups.map((stakeLevelGroup: any) => {
                        return new SettingType8StakeLevelGroup(stakeLevelGroup, true);
                    });
                }

                if (settingItem.stakeLevelGroupsDetails) {
                    this.stakeLevelGroupsDetails = this.fillStakeLevelGroupsDetails(settingItem.stakeLevelGroupsDetails);
                }

                this.customerLevel = customerLevel;

                if (this.customerLevel && this.customerLevel === Level.member) {
                    var selectedStakeRanges: SettingType8StakeLevelGroup[]
                        = this.stakeLevelGroups.filter((stakeLevelGroup: SettingType8StakeLevelGroup) => stakeLevelGroup.selected);

                    if (selectedStakeRanges && selectedStakeRanges.length > 0) {
                        this.selectedStakeLevelGroupId = selectedStakeRanges[0].stakeLevelGroupId;
                    }
                }
            }

            if (backup) {
                this.oldSetting = new DefaultSettingItem(this);
                (<SettingType8Item>this.oldSetting).selectedStakeLevelGroupId = this.selectedStakeLevelGroupId;
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var hasChange: boolean;

            hasChange = this.stakeLevelGroups.some((stakeLevelGroup: SettingType8StakeLevelGroup) => {
                return stakeLevelGroup.isChanged();
            });

            if (this.customerLevel && this.customerLevel === Level.member) {
                hasChange = hasChange || (this.selectedStakeLevelGroupId !== (<SettingType8Item>this.oldSetting).selectedStakeLevelGroupId);
            }

            return isChanged || hasChange;
        }

        public validate(): boolean {
            if (this.customerLevel && this.customerLevel === Level.member) {
                var isViolated: boolean
                    = this.stakeLevelGroups.filter((stakeLevelGroup: SettingType8StakeLevelGroup) => stakeLevelGroup.selected).length !== 1;

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
                stakeLevelGroups: []
            };

            if (this.isLive !== undefined) {
                settingItem.isLive = this.isLive;
            }

            if (this.customerLevel && this.customerLevel === Level.member) {
                this.stakeLevelGroups.forEach((stakeLevelGroup: SettingType8StakeLevelGroup) => {
                    stakeLevelGroup.selected = this.selectedStakeLevelGroupId === stakeLevelGroup.stakeLevelGroupId;
                });
            }

            this.stakeLevelGroups.forEach((stakeLevelGroup: SettingType8StakeLevelGroup) => {
                if (stakeLevelGroup.stakeLevelGroupId) {
                    settingItem.stakeLevelGroups.push(stakeLevelGroup.toJson());
                }
            });

            return settingItem;
        }

        private fillStakeLevelGroupsDetails(stakeLevelGroupsDetails: any): SettingType8StakeLevelGroupDetailItem[] {
            var groupDetails: Array<SettingType8StakeLevelGroupDetailItem>;

            if (this.stakeLevelGroups && this.stakeLevelGroups.length > 0 && stakeLevelGroupsDetails) {
                groupDetails = new Array<SettingType8StakeLevelGroupDetailItem>();

                groupDetails = this.stakeLevelGroups.map((stakeLevelGroup: SettingType8StakeLevelGroup) => {
                    var matchingItems: any = stakeLevelGroupsDetails.filter((item: any) => item.stakeLevelId === stakeLevelGroup.stakeLevelGroupId);

                    if (matchingItems && matchingItems.length > 0) {
                        return new SettingType8StakeLevelGroupDetailItem(matchingItems[0]);
                    } else {
                        return new SettingType8StakeLevelGroupDetailItem();
                    }
                });
            }

            return groupDetails;
        }
    }

    export class SettingType8StakeLevelGroup {
        public stakeLevelGroupId: number;
        public stakeLevelGroupName: string;
        public selected: boolean;

        private oldStakeLevelGroupValue: SettingType8StakeLevelGroup;

        constructor();
        constructor(stakeLevelGroup: any);
        constructor(stakeLevelGroup: any, backup: boolean)
        constructor(stakeLevelGroup?: any, backup: boolean = false) {
            if (stakeLevelGroup) {
                this.stakeLevelGroupId = stakeLevelGroup.stakeLevelGroupId;
                this.stakeLevelGroupName = stakeLevelGroup.stakeLevelGroupName;
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
                stakeLevelGroupId: this.stakeLevelGroupId,
                selected: this.selected
            };
        }
    }

    export class SettingType8StakeLevelGroupDetailItem extends DefaultSettingItem {
        public stakeLevelId: number;
        public stakeLevelRange: string;

        constructor();
        constructor(settingItem: any)
        constructor(settingItem?: any) {
            super(settingItem);

            if (settingItem) {
                this.stakeLevelId = settingItem.stakeLevelId;
                this.stakeLevelRange = settingItem.stakeLevelRange;
            }
        }
    }
}