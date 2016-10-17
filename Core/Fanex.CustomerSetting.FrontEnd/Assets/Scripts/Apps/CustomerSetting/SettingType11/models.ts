/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType11Item extends DefaultSettingItem {
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
                if (isChecked && !(<SettingType11Item>this.oldSetting).isChecked) {
                    this.downline.readonly = false;
                } else {
                    if ((<SettingType11Item>this.oldSetting).isChecked && (<SettingType11Item>this.downline.oldSetting).isChecked) {
                        this.downline.isChecked = this.isChecked;
                    } else {
                        this.downline.isChecked = false;
                    }

                    this.downline.readonly = true;
                }

                this.resetFieldState("isDisabled");
            } else {
                this.resetFieldState("downline.isDisabled");
            }
        }

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.isDisabled = settingItem.isDisabled;

                if (settingItem.upline) {
                    this.upline = new SettingType11Item(settingItem.upline);
                    this.readonly = settingItem.upline.isDisabled;
                } else {
                    this.readonly = false;
                }

                if (settingItem.downline) {
                    this.downline = new SettingType11Item(settingItem.downline, true);
                    this.downline.readonly = true;
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
                var backupChecked: boolean = this.isChecked;
                var backupDownlineChecked: boolean;

                if (!this.readonly) {
                    this.isChecked = source.isChecked;
                }

                if (this.downline && source.downline) {
                    backupDownlineChecked = this.downline.isChecked;

                    if (!this.downline.readonly) {
                        this.downline.isChecked = source.downline.isChecked;
                    }
                }

                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.isChecked = backupChecked;

                    if (this.downline) {
                        this.downline.isChecked = backupDownlineChecked;
                    }

                    this.highlightUnablyCopiedField();
                } else {
                    // Since highlightCopiedFields can't work with complex rule, then we must call those commands manually.
                    if (this.isChecked !== backupChecked) {
                        this.highlightState["isDisabled"] = NotificationType.success;
                    } else if (this.highlightState["isDisabled"]) {
                        this.highlightState["isDisabled"] = NotificationType.none;
                    }

                    if (this.downline && this.downline.isChecked !== backupDownlineChecked) {
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
                && this.isDisabled === (<SettingType11Item>this.oldSetting).isDisabled
                && this.downline.isDisabled !== (<SettingType11Item>this.oldSetting).downline.isDisabled) {
                this.validationState = new ValidationResult("DownlineStatusCannotBeChanged", "downline.isDisabled");

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