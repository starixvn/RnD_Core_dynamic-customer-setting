/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType18Config {
        public static get step(): number {
            return 1;
        }

        public static get roundingFactor(): number {
            return 100;
        }
    }

    export class SettingType18Item {
        public static factory(
            settingItem?: any,
            backup?: boolean,
            customerLevel?: Level,
            uplineLevel?: Level,
            extraData?: Array<KeyValuePair<string, any>>,
            multipleUpdates?: boolean): ISettingItem {
            if (customerLevel === Level.super) {
                return new SuperPositionTakingPartialMinPT(settingItem, backup, multipleUpdates);
            } else if (customerLevel === Level.master) {
                return new MasterPositionTakingPartialMinPT(settingItem, backup, multipleUpdates);
            } else if (customerLevel === Level.agent) {
                return new AgentPositionTakingPartialMinPT(settingItem, backup, multipleUpdates);
            } else if (customerLevel === Level.member && uplineLevel === Level.super) {
                return new SuperPositionTakingOnMemberPartialMinPT(settingItem, backup, multipleUpdates);
            } else if (customerLevel === Level.member && uplineLevel === Level.master) {
                return new MasterPositionTakingOnMemberPartialMinPT(settingItem, backup, extraData, multipleUpdates);
            } else if (customerLevel === Level.member && uplineLevel === Level.agent) {
                return new AgentPositionTakingOnMemberPartialMinPT(settingItem, backup, multipleUpdates);
            }
        }

        public static fetch(rawData: any, settingItem: any): void {
            var addedProperties: string[] = ["sportTypeId", "sportTypeName", "betTypeId", "betTypeName", "typeName", "isLive", "isRowCopied", "isCheckedUpdateRow", "isCheckedUpdateMinPtOnly"];

            if (rawData && settingItem) {
                Object.keys(rawData).filter((propName: string) => {
                    return addedProperties.indexOf(propName) < 0;
                }).forEach((propName: string) => {
                    settingItem[propName] = rawData[propName];
                });
            }
        }

        public static detechChanges(settingItem: CopyWithCheckboxSettingItem): boolean {
            var reservedProperties: string[] = ["oldSetting", "validationState", "highlightState"];
            if (!settingItem.isMultiple)
                reservedProperties.push("isRowCopied", "isCheckedUpdateRow", "isCheckedUpdateMinPtOnly", "isMultiple");

            if (settingItem) {
                return Object.keys(settingItem).some((prop: string) => {
                    return reservedProperties.indexOf(prop) < 0
                        && settingItem.oldSetting[prop] !== undefined
                        && settingItem[prop] !== settingItem.oldSetting[prop];
                });
            }

            return false;
        }
    }

    export class SuperPositionTakingPartialMinPT extends CopyWithCheckboxSettingItem {
        public s4: number;
        public m4: number;
        public a4: number;
        public n4: number;
        public defaultMaxPT: number;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean);
        constructor(settingItem: any, backup: boolean, multipleUpdates: boolean);
        constructor(settingItem?: any, backup: boolean = false, multipleUpdates?: boolean) {
            super(settingItem, multipleUpdates);
            SettingType18Item.fetch(settingItem, <SuperPositionTakingPartialMinPT>this);

            if (backup) {
                this.oldSetting = new SuperPositionTakingPartialMinPT(this, false, multipleUpdates);
            }
        }

        public isChanged(): boolean {
            return SettingType18Item.detechChanges(this);
        }

        public copySettingFrom(source: SuperPositionTakingPartialMinPT): boolean {
            var isSuccessful: boolean = true;
            var isCopySuccess: boolean = false;
            var backupS4: number;
            var backupM4: number;
            var backupA4: number;
            var backupN4: number;
            var backupisCheckedUpdateRow = this.isCheckedUpdateRow;

            if (source) {
                backupS4 = this.s4;
                backupM4 = this.m4;
                backupA4 = this.a4;
                backupN4 = this.n4;

                this.s4 = source.s4;
                this.m4 = source.m4;
                this.a4 = source.a4;
                this.n4 = source.n4;

                this.isCheckedUpdateRow = true;
                isSuccessful = this.validate();
                this.isCheckedUpdateRow = backupisCheckedUpdateRow;

                if (!isSuccessful) {
                    this.s4 = backupS4;
                    this.m4 = backupM4;
                    this.a4 = backupA4;
                    this.n4 = backupN4;
                    this.highlightUnablyCopiedField();

                } else {
                    this.highlightCopiedFields({
                        s4: backupS4,
                        m4: backupM4,
                        a4: backupA4,
                        n4: backupN4
                    });
                }

                this.isRowCopied = true;
                isCopySuccess = !this.hasHighlightField(NotificationType.error);
                if (this.isRowCopied)
                    this.isCheckedUpdateRow = this.isCheckedUpdateMinPtOnly = isCopySuccess;
            }

            return isCopySuccess;
        }

        public validate(): boolean {
            if (!this.isMultiple || this.isCheckedUpdateRow) {
                if (this.s4 > this.defaultMaxPT) {
                    this.validationState = new ValidationResult("SuperMaxPTCannotBeGreaterThan", "s4", [this.defaultMaxPT / SettingType18Config.roundingFactor]);

                    return false;
                }

                if (this.m4 > this.s4) {
                    this.validationState = new ValidationResult("MasterMaxPTCannotBeGreaterThanSuperMaxPT", "m4");

                    return false;
                }

                if (this.a4 > this.m4) {
                    this.validationState = new ValidationResult("AgentMaxPTCannotBeGreaterThanMasterMaxPT", "a4");

                    return false;
                }

                if (this.n4 > this.s4) {
                    this.validationState = new ValidationResult("SuperMinPTCannotBeGreaterThanSuperMaxPT", "n4");

                    return false;
                }
            }

            this.resetValidationState();

            return true;
        }
    }

    export class MasterPositionTakingPartialMinPT extends CopyWithCheckboxSettingItem {
        public s3: number;
        public uS3: number;
        public iS3: boolean;
        public m3: number;
        public n3: number;
        public s4: number;
        public m4: number;
        public n4: number;
        public superPTOnMembers: number;
        public isUpdateSuperPTOnMembers: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean);
        constructor(settingItem: any, backup: boolean, multipleUpdates: boolean);
        constructor(settingItem?: any, backup: boolean = false, multipleUpdates?: boolean) {
            super(settingItem, multipleUpdates);
            SettingType18Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new MasterPositionTakingPartialMinPT(this, false, multipleUpdates);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = SettingType18Item.detechChanges(this);

            if (isChanged) {
                this.correctAutoPtValues();
            }

            return isChanged;
        }

        public copySettingFrom(source: MasterPositionTakingPartialMinPT): boolean {
            var isSuccessful: boolean = true;
            var isCopySuccess: boolean = false;
            var backupIs3: boolean;
            var backupN3: number;
            var backupM3: number;
            var backupUs3: number;
            var backupS3: number;
            var backupisCheckedUpdateRow = this.isCheckedUpdateRow;

            if (source) {
                backupIs3 = this.iS3;
                backupN3 = this.n3;
                backupM3 = this.m3;
                backupUs3 = this.uS3;
                backupS3 = this.s3;

                this.iS3 = source.iS3;
                this.n3 = source.n3;
                this.m3 = source.m3;
                this.uS3 = source.uS3;
                this.s3 = source.s3;

                this.isCheckedUpdateRow = true;
                isSuccessful = this.validate();
                this.isCheckedUpdateRow = backupisCheckedUpdateRow;
                if (!isSuccessful) {
                    // Cheating: force the validationState to point to [SuperPT] field when restoring the old data
                    // in the case that the new [SuperAutoPT] violates the constraints.
                    if (this.iS3 && !backupIs3 && this.validationState.fieldName === "uS3") {
                        this.validationState.fieldName = "s3";
                    } else if (!this.iS3 && backupIs3 && this.validationState.fieldName === "s3") {
                        this.validationState.fieldName = "uS3";
                    }

                    this.iS3 = backupIs3;
                    this.n3 = backupN3;
                    this.m3 = backupM3;
                    this.uS3 = backupUs3;
                    this.s3 = backupS3;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        iS3: backupIs3,
                        n3: backupN3,
                        m3: backupM3,
                        uS3: backupUs3,
                        s3: backupS3
                    });
                }

                this.isRowCopied = true;
                isCopySuccess = !this.hasHighlightField(NotificationType.error);
                if (this.isRowCopied)
                    this.isCheckedUpdateRow = this.isCheckedUpdateMinPtOnly = isCopySuccess;
            }

            return isCopySuccess;
        }

        public copyPtOnMembersFrom(source: MasterPositionTakingPartialMinPT): boolean {
            var isSuccessful: boolean = true;
            var backupSuperPTOnMembers: number;

            if (source) {
                backupSuperPTOnMembers = this.superPTOnMembers;
                this.superPTOnMembers = source.superPTOnMembers;
                isSuccessful = this.validatePtOnMembers();

                if (!isSuccessful) {
                    this.superPTOnMembers = backupSuperPTOnMembers;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        superPTOnMembers: backupSuperPTOnMembers
                    });
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (!this.isMultiple || this.isCheckedUpdateRow) {
                if (this.iS3) {
                    if (!this.validateAutoPt()) {
                        return false;
                    }
                } else {
                    if (!this.validateManualPt()) {
                        return false;
                    }
                }

                if (this.n3 > this.m3) {
                    this.validationState = new ValidationResult("MasterMinPTCannotBeGreaterThanMasterMaxPT", "n3");

                    return false;
                }
            }

            this.resetValidationState();

            return true;
        }

        private validateAutoPt(): boolean {
            var minOfMasterMaxPt: number = this.n4 - this.uS3;
            var maxOfMasterMaxPt: number = Math.min(this.s4, this.m4);

            if (this.uS3 > this.s4) {
                this.validationState = new ValidationResult("SuperAutoPTCannotBeGreaterThanSuperMaxPT", "uS3");

                return false;
            }

            if (this.m3 < minOfMasterMaxPt) {
                this.validationState = new ValidationResult("MasterMaxPTCannotBeLessThan", "m3", [minOfMasterMaxPt / SettingType18Config.roundingFactor]);

                return false;
            }

            if (this.m3 > maxOfMasterMaxPt) {
                this.validationState = new ValidationResult("MasterMaxPTCannotBeGreaterThan", "m3", [maxOfMasterMaxPt / SettingType18Config.roundingFactor]);

                return false;
            }

            return true;
        }

        private validateManualPt(): boolean {
            var minOfSuperPt: number = Math.max(0, this.n4 - this.m4);
            var minOfMasterMaxPt: number = Math.max(0, this.n4 - this.s3);
            var maxOfMasterMaxPt: number = Math.min(this.s4 - this.s3, this.m4);

            if (this.s3 < minOfSuperPt) {
                this.validationState = new ValidationResult("SuperPTCannotBeLessThan", "s3", [minOfSuperPt / SettingType18Config.roundingFactor]);

                return false;
            }

            if (this.s3 > this.s4) {
                this.validationState = new ValidationResult("SuperPTCannotBeGreaterThanSuperMaxPT", "s3");

                return false;
            }

            if (this.m3 < minOfMasterMaxPt) {
                this.validationState = new ValidationResult("MasterMaxPTCannotBeLessThan", "m3", [minOfMasterMaxPt / SettingType18Config.roundingFactor]);

                return false;
            }

            if (this.m3 > maxOfMasterMaxPt) {
                this.validationState = new ValidationResult("MasterMaxPTCannotBeGreaterThan", "m3", [maxOfMasterMaxPt / SettingType18Config.roundingFactor]);

                return false;
            }

            return true;
        }

        private validatePtOnMembers(): boolean {
            var maxOfSuperPtOnMembers: number = Math.max(0, this.s4 - this.m3);

            if (this.superPTOnMembers < 0) {
                this.validationState = new ValidationResult("SuperPTOnMemberCannotBeLessThan", "superPTOnMembers", [0]);

                return false;
            }

            if (this.superPTOnMembers > maxOfSuperPtOnMembers) {
                this.validationState = new ValidationResult(
                    "SuperPTOnMemberCannotBeGreaterThan",
                    "superPTOnMembers",
                    [maxOfSuperPtOnMembers / SettingType18Config.roundingFactor]);

                return false;
            }

            return true;
        }

        private correctAutoPtValues(): void {
            if (this.iS3 && this.s3 > 0) {
                this.s3 = 0;
            }

            if (!this.iS3 && this.uS3 > 0) {
                this.uS3 = 0;
            }
        }
    }

    export class AgentPositionTakingPartialMinPT extends CopyWithCheckboxSettingItem {
        public m2: number;
        public uM2: number;
        public iM2: boolean;
        public a2: number;
        public n2: number;
        public m3: number;
        public n3: number;
        public a4: number;
        public masterPTOnMembers: number;
        public isUpdateMasterPTOnMembers: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean);
        constructor(settingItem: any, backup: boolean, multipleUpdates: boolean);
        constructor(settingItem?: any, backup: boolean = false, multipleUpdates?: boolean) {
            super(settingItem, multipleUpdates);
            SettingType18Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new AgentPositionTakingPartialMinPT(this, false, multipleUpdates);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = SettingType18Item.detechChanges(this);

            if (isChanged) {
                this.correctAutoPtValues();
            }

            return isChanged;
        }

        public copySettingFrom(source: AgentPositionTakingPartialMinPT): boolean {
            var isSuccessful: boolean = true;
            var isCopySuccess: boolean = false;
            var backupIm2: boolean;
            var backupN2: number;
            var backupA2: number;
            var backupUm2: number;
            var backupM2: number;
            var backupisCheckedUpdateRow = this.isCheckedUpdateRow;

            if (source) {
                backupIm2 = this.iM2;
                backupN2 = this.n2;
                backupA2 = this.a2;
                backupUm2 = this.uM2;
                backupM2 = this.m2;

                this.iM2 = source.iM2;
                this.n2 = source.n2;
                this.a2 = source.a2;
                this.uM2 = source.uM2;
                this.m2 = source.m2;

                this.isCheckedUpdateRow = true;
                isSuccessful = this.validate();
                this.isCheckedUpdateRow = backupisCheckedUpdateRow;

                if (!isSuccessful) {
                    // Cheating: force the validationState to point to [MasterPT] field when restoring the old data
                    // in the case that the new [MasterAutoPT] violates the constraints.
                    if (this.iM2 && !backupIm2 && this.validationState.fieldName === "uM2") {
                        this.validationState.fieldName = "m2";
                    } else if (!this.iM2 && backupIm2 && this.validationState.fieldName === "m2") {
                        this.validationState.fieldName = "uM2";
                    }

                    this.iM2 = backupIm2;
                    this.n2 = backupN2;
                    this.a2 = backupA2;
                    this.uM2 = backupUm2;
                    this.m2 = backupM2;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        iM2: backupIm2,
                        n2: backupN2,
                        a2: backupA2,
                        uM2: backupUm2,
                        m2: backupM2
                    });
                }

                this.isRowCopied = true;
                isCopySuccess = !this.hasHighlightField(NotificationType.error);
                if (this.isRowCopied)
                    this.isCheckedUpdateRow = this.isCheckedUpdateMinPtOnly = isCopySuccess;
            }

            return isCopySuccess;
        }

        public copyPtOnMembersFrom(source: AgentPositionTakingPartialMinPT): boolean {
            var isSuccessful: boolean = true;
            var backupMasterPTOnMembers: number;

            if (source) {
                backupMasterPTOnMembers = this.masterPTOnMembers;
                this.masterPTOnMembers = source.masterPTOnMembers;
                isSuccessful = this.validatePtOnMembers();

                if (!isSuccessful) {
                    this.masterPTOnMembers = backupMasterPTOnMembers;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        masterPTOnMembers: backupMasterPTOnMembers
                    });
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (!this.isMultiple || this.isCheckedUpdateRow) {
                if (this.iM2) {
                    if (!this.validateAutoPt()) {
                        return false;
                    }
                } else {
                    if (!this.validateManualPt()) {
                        return false;
                    }
                }

                if (this.n2 > this.a2) {
                    this.validationState = new ValidationResult("AgentMinPTCannotBeGreaterThanAgentMaxPT", "n2");

                    return false;
                }
            }

            this.resetValidationState();

            return true;
        }

        private validateAutoPt(): boolean {
            var minOfAgentMaxPt: number = this.n3 - this.uM2;
            var maxOfAgentMaxPt: number = Math.min(this.m3, this.a4);

            if (this.uM2 > this.m3) {
                this.validationState = new ValidationResult("MasterAutoPTCannotBeGreaterThanMasterMaxPT", "uM2");

                return false;
            }

            if (this.a2 < minOfAgentMaxPt) {
                this.validationState = new ValidationResult("AgentMaxPTCannotBeLessThan", "a2", [minOfAgentMaxPt / SettingType18Config.roundingFactor]);

                return false;
            }

            if (this.a2 > maxOfAgentMaxPt) {
                this.validationState = new ValidationResult("AgentMaxPTCannotBeGreaterThan", "a2", [maxOfAgentMaxPt / SettingType18Config.roundingFactor]);

                return false;
            }

            return true;
        }

        private validateManualPt(): boolean {
            var minOfMasterPt: number = Math.max(0, this.n3 - this.a4);
            var minOfAgentMaxPt: number = Math.max(0, this.n3 - this.m2);
            var maxOfAgentMaxPt: number = Math.min(this.m3 - this.m2, this.a4);

            if (this.m2 < minOfMasterPt) {
                this.validationState = new ValidationResult("MasterPTCannotBeLessThan", "m2", [minOfMasterPt / SettingType18Config.roundingFactor]);

                return false;
            }

            if (this.m2 > this.m3) {
                this.validationState = new ValidationResult("MasterPTCannotBeGreaterThanMasterMaxPT", "m2");

                return false;
            }

            if (this.a2 < minOfAgentMaxPt) {
                this.validationState = new ValidationResult("AgentMaxPTCannotBeLessThan", "a2", [minOfAgentMaxPt / SettingType18Config.roundingFactor]);

                return false;
            }

            if (this.a2 > maxOfAgentMaxPt) {
                this.validationState = new ValidationResult("AgentMaxPTCannotBeGreaterThan", "a2", [maxOfAgentMaxPt / SettingType18Config.roundingFactor]);

                return false;
            }

            return true;
        }

        private validatePtOnMembers(): boolean {
            var maxOfMasterPtOnMembers: number = Math.max(0, this.m3 - this.a2);

            if (this.masterPTOnMembers < 0) {
                this.validationState = new ValidationResult("MasterPTOnMemberCannotBeLessThan", "masterPTOnMembers", [0]);

                return false;
            }

            if (this.masterPTOnMembers > maxOfMasterPtOnMembers) {
                this.validationState = new ValidationResult(
                    "MasterPTOnMemberCannotBeGreaterThan",
                    "masterPTOnMembers",
                    [maxOfMasterPtOnMembers / SettingType18Config.roundingFactor]);

                return false;
            }

            return true;
        }

        private correctAutoPtValues(): void {
            if (this.iM2 && this.m2 > 0) {
                this.m2 = 0;
            }

            if (!this.iM2 && this.uM2 > 0) {
                this.uM2 = 0;
            }
        }
    }

    export class SuperPositionTakingOnMemberPartialMinPT extends CopyWithCheckboxSettingItem {
        public a1: number;
        public m1: number;
        public s1: number;
        public uS1: number;
        public iS1: boolean;
        public iS3: boolean;
        public s4: number;
        public n4: number;
        public m3: number;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean);
        constructor(settingItem: any, backup: boolean, multipleUpdates: boolean);
        constructor(settingItem?: any, backup: boolean = false, multipleUpdates?: boolean) {
            super(settingItem, multipleUpdates);
            SettingType18Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new SuperPositionTakingOnMemberPartialMinPT(this, false, multipleUpdates);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = SettingType18Item.detechChanges(this);

            if (isChanged) {
                this.correctAutoPtValues();
            }

            return isChanged;
        }

        public copySettingFrom(source: SuperPositionTakingOnMemberPartialMinPT): boolean {
            var isSuccessful: boolean = true;
            var isCopySuccess: boolean = false;
            var backupIs1: boolean;
            var backupUs1: number;
            var backups1: number;
            var isS1Disabled;
            var backupisCheckedUpdateRow = this.isCheckedUpdateRow;

            if (source) {
                backupIs1 = this.iS1;
                backupUs1 = this.uS1;
                backups1 = this.s1;

                this.iS1 = source.iS1;

                if (this.iS1) {
                    this.uS1 = source.uS1;
                } else if (!this.iS3) {
                    this.s1 = source.s1;
                }

                this.isCheckedUpdateRow = true;
                isSuccessful = this.validate();
                this.isCheckedUpdateRow = backupisCheckedUpdateRow;

                if (!isSuccessful) {
                    // Cheating: force the validationState to point to [SuperPTOnMember] field when restoring the old data
                    // in the case that the new [SuperAutoPTOnMember] violates the constraints.
                    if (this.iS1 && !backupIs1 && this.validationState.fieldName === "uS1") {
                        this.validationState.fieldName = "s1";
                    } else if (!this.iS1 && backupIs1 && this.validationState.fieldName === "s1") {
                        this.validationState.fieldName = "uS1";
                    }

                    this.iS1 = backupIs1;
                    this.uS1 = backupUs1;
                    this.s1 = backups1;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        iS1: backupIs1,
                        uS1: backupUs1,
                        s1: backups1
                    });
                }

                this.isRowCopied = true;
                isCopySuccess = !this.hasHighlightField(NotificationType.error);
                if (this.isRowCopied)
                    this.isCheckedUpdateRow = this.isCheckedUpdateMinPtOnly = isCopySuccess;
            }

            return isCopySuccess;
        }

        public validate(): boolean {
            if (!this.isMultiple || this.isCheckedUpdateRow) {
                if (this.iS1) {
                    var minOfSuperAutoPtOnMember: number = Math.max(0, this.n4 - this.m1 - this.a1);

                    if (this.uS1 < minOfSuperAutoPtOnMember) {
                        this.validationState = new ValidationResult(
                            "SuperAutoPTOnMemberCannotBeLessThan",
                            "uS1",
                            [minOfSuperAutoPtOnMember / SettingType18Config.roundingFactor]);

                        return false;
                    }

                    if (this.uS1 > this.s4) {
                        this.validationState = new ValidationResult("SuperAutoPTOnMemberCannotBeGreaterThanSuperMaxPT", "uS1");

                        return false;
                    }
                } else if (!this.iS3) {
                    var minOfSuperPtOnMember: number = Math.max(0, this.n4 - this.m1 - this.a1);
                    var maxOfSuperPtOnMember: number = Math.max(this.n4 - this.m1 - this.a1, this.s4 - this.m3);

                    if (this.s1 < minOfSuperPtOnMember) {
                        this.validationState = new ValidationResult(
                            "SuperPTOnMemberCannotBeLessThan",
                            "s1",
                            [minOfSuperPtOnMember / SettingType18Config.roundingFactor]);

                        return false;
                    }

                    if (this.s1 > maxOfSuperPtOnMember) {
                        this.validationState = new ValidationResult(
                            "SuperPTOnMemberCannotBeGreaterThan",
                            "s1",
                            [maxOfSuperPtOnMember / SettingType18Config.roundingFactor]);

                        return false;
                    }
                }
            }

            this.resetValidationState();

            return true;
        }

        private correctAutoPtValues(): void {
            if (this.iS1 && this.s1 > 0) {
                this.s1 = 0;
            }

            if (!this.iS1 && this.uS1 > 0) {
                this.uS1 = 0;
            }
        }
    }

    export class MasterPositionTakingOnMemberPartialMinPT extends CopyWithCheckboxSettingItem {
        public a1: number;
        public m1: number;
        public iM1: boolean;
        public uM1: number;
        public iM2: boolean;
        public m3: number;
        public n3: number;
        public isDisableMasterAutoPT: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean, extraData: Array<KeyValuePair<string, any>>, multipleUpdates: boolean);
        constructor(settingItem?: any, backup: boolean = false, extraData?: Array<KeyValuePair<string, any>>, multipleUpdates?: boolean) {
            super(settingItem, multipleUpdates);
            SettingType18Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new MasterPositionTakingOnMemberPartialMinPT(this, false, extraData, multipleUpdates);
            }

            if (extraData) {
                var isDisableMasterAutoPTValues: Array<KeyValuePair<string, any>> = extraData.filter((item: KeyValuePair<string, any>) => {
                    return item.key === "isDisableMasterAutoPT";
                });

                if (isDisableMasterAutoPTValues.length > 0) {
                    this.isDisableMasterAutoPT = !!isDisableMasterAutoPTValues[0].value;
                }
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = SettingType18Item.detechChanges(this);

            if (isChanged) {
                this.correctAutoPtValues();
            }

            return isChanged;
        }

        public copySettingFrom(source: MasterPositionTakingOnMemberPartialMinPT): boolean {
            var isSuccessful: boolean = true;
            var isCopySuccess: boolean = false;
            var backupIm1: boolean;
            var backupUm1: number;
            var backupM1: number;
            var backupisCheckedUpdateRow = this.isCheckedUpdateRow;

            if (source) {
                backupIm1 = this.iM1;
                backupUm1 = this.uM1;
                backupM1 = this.m1;

                this.iM1 = source.iM1;

                if (this.iM1) {
                    this.uM1 = source.uM1;
                } else if (!this.iM2) {
                    this.m1 = source.m1;
                }

                this.isCheckedUpdateRow = true;
                isSuccessful = this.validate();
                this.isCheckedUpdateRow = backupisCheckedUpdateRow;

                if (!isSuccessful) {
                    // Cheating: force the validationState to point to [MasterPTOnMember] field when restoring the old data
                    // in the case that the new [MasterAutoPTOnMember] violates the constraints.
                    if (this.iM1 && !backupIm1 && this.validationState.fieldName === "uM1") {
                        this.validationState.fieldName = "m1";
                    } else if (!this.iM1 && backupIm1 && this.validationState.fieldName === "m1") {
                        this.validationState.fieldName = "uM1";
                    }

                    this.iM1 = backupIm1;
                    this.uM1 = backupUm1;
                    this.m1 = backupM1;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        iM1: backupIm1,
                        uM1: backupUm1,
                        m1: backupM1
                    });
                }

                this.isRowCopied = true;
                isCopySuccess = !this.hasHighlightField(NotificationType.error);
                if (this.isRowCopied)
                    this.isCheckedUpdateRow = this.isCheckedUpdateMinPtOnly = isCopySuccess;
            }

            return isCopySuccess;
        }

        public validate(): boolean {
            if (!this.isMultiple || this.isCheckedUpdateRow) {
                if (this.iM1) {
                    var minOfMasterAutoPtOnMember: number = Math.max(0, this.n3 - this.a1);

                    if (this.uM1 < minOfMasterAutoPtOnMember) {
                        this.validationState = new ValidationResult(
                            "MasterAutoPTOnMemberCannotBeLessThan",
                            "uM1",
                            [minOfMasterAutoPtOnMember / SettingType18Config.roundingFactor]);

                        return false;
                    }

                    if (this.uM1 > this.m3) {
                        this.validationState = new ValidationResult("MasterAutoPTOnMemberCannotBeGreaterThanMasterMaxPT", "uM1");

                        return false;
                    }
                } else if (!this.iM2 || this.isDisableMasterAutoPT) {
                    var minOfMasterPtOnMember: number = Math.max(0, this.n3 - this.a1);
                    var maxOfMasterPtOnMember: number = this.m3 - this.a1;

                    if (this.m1 < minOfMasterPtOnMember) {
                        this.validationState = new ValidationResult(
                            "MasterPTOnMemberCannotBeLessThan",
                            "m1",
                            [minOfMasterPtOnMember / SettingType18Config.roundingFactor]);

                        return false;
                    }

                    if (this.m1 > maxOfMasterPtOnMember) {
                        this.validationState = new ValidationResult(
                            "MasterPTOnMemberCannotBeGreaterThan",
                            "m1",
                            [maxOfMasterPtOnMember / SettingType18Config.roundingFactor]);

                        return false;
                    }
                }
            }

            this.resetValidationState();

            return true;
        }

        private correctAutoPtValues(): void {
            if (this.iM1 && this.m1 > 0) {
                this.m1 = 0;
            }

            if (!this.iM1 && this.uM1 > 0) {
                this.uM1 = 0;
            }
        }
    }

    export class AgentPositionTakingOnMemberPartialMinPT extends CopyWithCheckboxSettingItem {
        public a1: number;
        public a2: number;
        public n2: number;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean);
        constructor(settingItem: any, backup: boolean, multipleUpdates: boolean);
        constructor(settingItem?: any, backup: boolean = false, multipleUpdates?: boolean) {
            super(settingItem, multipleUpdates);
            SettingType18Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new AgentPositionTakingOnMemberPartialMinPT(this, false, multipleUpdates);
            }
        }

        public isChanged(): boolean {
            return SettingType18Item.detechChanges(this);
        }

        public copySettingFrom(source: AgentPositionTakingOnMemberPartialMinPT): boolean {
            var isSuccessful: boolean = true;
            var isCopySuccess: boolean = false;
            var backupA1: number;
            var backupisCheckedUpdateRow = this.isCheckedUpdateRow;

            if (source) {
                backupA1 = this.a1;

                this.a1 = source.a1;
                this.isCheckedUpdateRow = true;
                isSuccessful = this.validate();
                this.isCheckedUpdateRow = backupisCheckedUpdateRow;
                if (!isSuccessful) {
                    this.a1 = backupA1;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({
                        a1: backupA1
                    });
                }

                this.isRowCopied = true;
                isCopySuccess = !this.hasHighlightField(NotificationType.error);
                if (this.isRowCopied)
                    this.isCheckedUpdateRow = this.isCheckedUpdateMinPtOnly = isCopySuccess;
            }

            return isCopySuccess;
        }

        public validate(isForceValidate: boolean = false): boolean {
            if (!this.isMultiple || this.isCheckedUpdateRow) {
                if (this.a1 < this.n2) {
                    this.validationState = new ValidationResult("AgentPTCannotBeLessThanAgentMinPT", "a1");

                    return false;
                }

                if (this.a1 > this.a2) {
                    this.validationState = new ValidationResult("AgentPTCannotBeGreaterThanAgentMaxPT", "a1");

                    return false;
                }
            }

            this.resetValidationState();

            return true;
        }
    }
}