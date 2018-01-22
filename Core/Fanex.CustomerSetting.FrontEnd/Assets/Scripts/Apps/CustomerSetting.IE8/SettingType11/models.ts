/// <reference path="../core/models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType11Item extends DefaultSettingItem {
        private systemId: number;
        private customerLevel: Level;

        public isDisabled: boolean;
        public upline: SettingType11Item;

        // Additional fields tracing the state of controls.
        // I added those fields due to the state of controls can be cloned via copying function.
        public readonly: boolean;
        public isChecked: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean, customerLevel: Level, systemId: number)
        constructor(settingItem?: any, backup: boolean = false, customerLevel: Level = null, systemId: number = 0) {
            super(settingItem);

            this.systemId = systemId;
            this.customerLevel = customerLevel;

            if (settingItem) {
                this.isDisabled = settingItem.isDisabled;
                this.isChecked = !this.isDisabled;
                this.readonly = false;

                if (settingItem.upline) {
                    this.upline = new SettingType11Item(settingItem.upline);
                    this.readonly = settingItem.upline.isDisabled;
                }

                if ((this.systemId === System.alpha || this.systemId === System.internalAlpha)
                    && this.customerLevel !== Level.member) {
                    this.readonly = this.readonly || this.isChecked;
                }
            }

            if (backup) {
                this.oldSetting = new SettingType11Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType11Item = <SettingType11Item>this.oldSetting;

            return isChanged || this.isDisabled !== oldSetting.isDisabled;
        }

        public copySettingFrom(source: SettingType11Item): boolean {
            var isSuccessful: boolean = true;

            if (source) {
                var backupIsChecked: boolean = this.isChecked;

                if (!this.readonly) {
                    this.isChecked = source.isChecked;
                }

                this.toggleCheck();
                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.isChecked = backupIsChecked;
                    this.toggleCheck();
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({ isDisabled: !backupIsChecked });
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
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

            return settingItem;
        }

        public toggleCheck(isDownline: boolean = false): void {
            this.isDisabled = !this.isChecked;
        }
    }
}