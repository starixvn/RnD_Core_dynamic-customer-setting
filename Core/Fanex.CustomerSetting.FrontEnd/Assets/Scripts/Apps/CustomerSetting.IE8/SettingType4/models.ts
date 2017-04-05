/// <reference path="../core/models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType4Item extends DefaultSettingItem {
        public isDisabled: boolean;
        public upline: SettingType4Item;
        public downline: SettingType4Item;

        // Additional fields tracing the state of controls. I added those fields due to the state of controls can be cloned via copying function.
        public readonly: boolean;
        public isChecked: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.isDisabled = settingItem.isDisabled;
                this.isChecked = !this.isDisabled;

                if (settingItem.upline) {
                    this.upline = new SettingType4Item(settingItem.upline);
                    this.readonly = settingItem.upline.isDisabled;
                } else {
                    this.readonly = false;
                }

                if (settingItem.downline) {
                    this.downline = new SettingType4Item(settingItem.downline, true);
                    this.downline.readonly = true;
                }
            }

            if (backup) {
                this.oldSetting = new SettingType4Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType4Item = <SettingType4Item>this.oldSetting;

            return isChanged
                || this.isDisabled !== oldSetting.isDisabled
                || (this.downline && this.downline.isDisabled !== oldSetting.downline.isDisabled);
        }

        public copySettingFrom(source: SettingType4Item): boolean {
            var isSuccessful: boolean = true;

            if (source) {
                var backupIsChecked: boolean = this.isDisabled;
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

                this.toggleCheck();
                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.isChecked = backupIsChecked;

                    if (this.downline && source.downline) {
                        this.downline.isChecked = backupDownlineIsChecked;
                    }

                    this.toggleCheck();
                    this.highlightUnablyCopiedField();
                } else {
                    // Since highlightCopiedFields can't work with complex rule, then we must call those commands manually.
                    if (this.isChecked !== backupIsChecked) {
                        this.highlightState["isDisabled"] = NotificationType.success;
                    } else if (this.highlightState["isDisabled"]) {
                        this.highlightState["isDisabled"] = NotificationType.none;
                    }

                    if (this.downline.isChecked !== backupDownlineIsChecked) {
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

            if (this.downline
                && this.isDisabled === (<SettingType4Item>this.oldSetting).isDisabled
                && this.downline.isDisabled !== (<SettingType4Item>this.oldSetting).downline.isDisabled) {
                this.validationState = new ValidationResult("DownlineStatusCannotBeChanged", "downline.isDisabled");

                return false;
            }

            if (this.upline && this.upline.isDisabled && !this.isDisabled) {
                this.validationState = new ValidationResult("DownlineCannotBeEnabledWhenUplineIsDisabled", "isDisabled");

                return false;
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

        public toggleCheck(isDownline: boolean = false): void {
            if (isDownline) {
                this.downline.isDisabled = !this.downline.isChecked;
            } else {
                this.isDisabled = !this.isChecked;
            }

            if (this.downline) {
                if (this.isChecked && !(<SettingType4Item>this.oldSetting).isChecked) {
                    this.downline.readonly = false;
                } else {
                    if ((<SettingType4Item>this.oldSetting).isChecked && (<SettingType4Item>this.downline.oldSetting).isChecked) {
                        this.downline.isChecked = this.isChecked;
                    } else {
                        this.downline.isDisabled = true;
                        this.resetFieldState("downline.isDisabled");
                    }

                    this.downline.readonly = true;
                }

                this.downline.isDisabled = !this.downline.isChecked;
                this.resetFieldState("isDisabled");
            } else {
                this.resetFieldState("downline.isDisabled");
            }
        }

        private resetFieldState(fieldName: string): void {
            if (this.validationState && !this.validationState.isSuccessful()) {
                if (this.validationState.fieldName === fieldName) {
                    this.resetValidationState();
                }
            } else {
                this.resetFieldHighlightState(fieldName);
            }
        }
    }
}