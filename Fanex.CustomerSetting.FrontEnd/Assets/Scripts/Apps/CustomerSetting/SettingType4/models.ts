/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType4Item extends DefaultSettingItem {
        public isDisabled: boolean;
        public upline: SettingType4Item;
        public downline: SettingType4Item;

        // Additional fields tracing the state of controls. I added those fields due to the state of controls can be cloned via copying function.
        public readonly: boolean;

        public get isChecked(): boolean {
            return !this.isDisabled;
        }

        public set isChecked(isChecked: boolean) {
            this.isDisabled = !isChecked;

            if (this.downline) {
                if (isChecked && !(<SettingType4Item>this.oldSetting).isChecked) {
                    this.downline.readonly = false;
                } else {
                    this.downline.isChecked = this.isChecked;
                    this.downline.readonly = true;
                }
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
                    this.upline = new SettingType4Item(settingItem.upline);
                    this.readonly = settingItem.upline.isDisabled;
                } else {
                    this.readonly = false;
                }

                if (settingItem.downline) {
                    this.downline = new SettingType4Item(settingItem.downline);
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
                var backupDisabled: boolean = this.isDisabled;
                var backupReadonly: boolean = this.readonly;
                var backupDisabledDownline: boolean;
                var backupReadonlyDownline: boolean;

                this.isDisabled = source.isDisabled;
                this.readonly = source.readonly;

                if (this.downline && source.downline) {
                    backupDisabledDownline = this.downline.isDisabled;
                    backupReadonlyDownline = this.downline.readonly;
                    this.downline.isDisabled = source.downline.isDisabled;
                    this.downline.readonly = source.downline.readonly;
                }

                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.isDisabled = backupDisabled;
                    this.readonly = backupReadonly;

                    if (this.downline && source.downline) {
                        this.downline.isDisabled = backupDisabledDownline;
                        this.downline.readonly = backupReadonlyDownline;
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
    }
}