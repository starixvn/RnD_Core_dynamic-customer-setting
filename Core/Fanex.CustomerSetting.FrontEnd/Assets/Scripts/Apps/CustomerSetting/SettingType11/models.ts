/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType11Item extends DefaultSettingItem {
        private systemId: number;
        private customerLevel: Level;

        public isDisabled: boolean;
        public upline: SettingType11Item;
        public downline: SettingType11Item;

        // Additional fields tracing the state of controls.
        // I added those fields due to the state of controls can be cloned via copying function.
        public readonly: boolean;

        public get isChecked(): boolean {
            return !this.isDisabled;
        }

        public set isChecked(isChecked: boolean) {
            this.isDisabled = !isChecked;

            if (this.downline) {
                if ((<SettingType11Item>this.oldSetting).isChecked && (<SettingType11Item>this.downline.oldSetting).isChecked) {
                    this.downline.isChecked = this.isChecked;
                    this.downline.readonly = true;
                } else {
                    if (!this.isChecked) {
                        this.downline.isChecked = false;
                    }

                    this.downline.readonly = !isChecked;
                }

                this.resetFieldState("isDisabled");
            } else {
                this.resetFieldState("downline.isDisabled");
            }
        }

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean, customerLevel: Level, systemId: number)
        constructor(settingItem?: any, backup: boolean = false, customerLevel: Level = null, systemId: number = 0) {
            super(settingItem);

            this.systemId = systemId;
            this.customerLevel = customerLevel;

            if (settingItem) {
                this.isDisabled = settingItem.isDisabled;
                this.readonly = false;

                if (settingItem.upline) {
                    this.upline = new SettingType11Item(settingItem.upline);
                    this.readonly = settingItem.upline.isDisabled;
                }

                if (this.systemId !== System.nap && this.customerLevel !== Level.member) {
                    this.readonly = this.readonly || this.isChecked;
                }

                if (settingItem.downline) {
                    this.downline = new SettingType11Item(settingItem.downline, true, customerLevel, systemId);
                    this.downline.readonly = !this.isChecked || this.downline.isChecked;
                }
            }

            if (backup) {
                this.oldSetting = new SettingType11Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType11Item = <SettingType11Item>this.oldSetting;

            return isChanged
                || this.isDisabled !== oldSetting.isDisabled
                || (this.downline && this.downline.isDisabled !== oldSetting.downline.isDisabled);
        }

        public copySettingFrom(source: SettingType11Item): boolean {
            var isSuccessful: boolean = true;

            if (source) {
                var backupIsChecked: boolean = this.isChecked;
                var backupDownlineIsChecked: boolean;

                if (!this.readonly) {
                    this.isChecked = source.isChecked;
                }

                if (this.downline && source.downline) {
                    backupDownlineIsChecked = this.downline.isChecked;

                    if (!this.downline.readonly) {
                        this.downline.isChecked = source.downline.isChecked;
                    }
                }

                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.isChecked = backupIsChecked;

                    if (this.downline) {
                        this.downline.isChecked = backupDownlineIsChecked;
                    }

                    this.highlightUnablyCopiedField();
                } else {
                    // Since highlightCopiedFields can't work with complex rule, then we must call those commands manually.
                    if (this.isChecked !== backupIsChecked) {
                        this.highlightState["isDisabled"] = NotificationType.success;
                    } else if (this.highlightState["isDisabled"]) {
                        this.highlightState["isDisabled"] = NotificationType.none;
                    }

                    if (this.downline && this.downline.isChecked !== backupDownlineIsChecked) {
                        this.highlightState["downline.isDisabled"] = NotificationType.success;
                    } else if (this.highlightState["downline.isDisabled"]) {
                        this.highlightState["downline.isDisabled"] = NotificationType.none;
                    }
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (this.isDisabled && this.downline && !this.downline.isDisabled) {
                this.validationState = new ValidationResult("DownlineStatusIsInvalid", "downline.isDisabled");

                return false;
            }

            if (this.upline && this.upline.isDisabled && !this.isDisabled) {
                this.validationState = new ValidationResult("DownlineCannotBeEnabledWhenUplineIsDisabled", "isDisabled");

                return false;
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
                isDisabled: this.isDisabled
            };

            if (this.upline) {
                settingItem.upline = this.upline.toJson();
            }

            if (this.downline) {
                settingItem.downline = this.downline.toJson();
            }

            return settingItem;
        }

        private resetFieldState(fieldName: string): void {
            if (this.validationState && !this.validationState.isSuccessful) {
                if (this.validationState.fieldName === fieldName) {
                    this.resetValidationState();
                }
            } else {
                this.resetFieldHighlightState(fieldName);
            }
        }
    }
}