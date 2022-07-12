/// <reference path="../core/common.ts" />
/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType21Item extends DefaultSettingItem {
        private systemId: number;
        private customerLevel: Level;
        public parentSportTypeId: number;
        public parentBetTypeId: number;
        public isDisabled: boolean;
        public upline: SettingType21Item;
        // Additional fields tracing the state of controls.
        // I added those fields due to the state of controls can be cloned via copying function.
        public readonly: boolean;
        private settingTypeId = 21;

        public get isChecked(): boolean {
            return !this.isDisabled;
        }

        public set isChecked(isChecked: boolean) {
            this.isDisabled = !isChecked;
            if (this.parentSportTypeId == 0 && this.parentBetTypeId == 0) {
                this.$rootScope.$emit("checkedChanged", {isDisable: this.isDisabled, settingTypeId: this.settingTypeId});
            }

        }

        private $rootScope: ng.IRootScopeService

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean, customerLevel: Level, systemId: number, $rootScope: ng.IRootScopeService);
        constructor(settingItem?: any, backup: boolean = false, customerLevel: Level = null, systemId: number = 0, $rootScope: ng.IRootScopeService = null) {
            super(settingItem);
            this.$rootScope = $rootScope;
            this.systemId = systemId;
            this.customerLevel = customerLevel;
            this.parentSportTypeId = settingItem.parentSportTypeId;
            this.parentBetTypeId = settingItem.parentBetTypeId;

            if (settingItem) {
                this.isDisabled = settingItem.isDisabled;
                this.readonly = false;

                if (settingItem.upline) {
                    this.upline = new SettingType21Item(settingItem.upline);
                    this.readonly = settingItem.upline.isDisabled;
                }

                if ((this.systemId === System.alpha || this.systemId === System.internalAlpha)
                    && this.customerLevel !== Level.member) {
                    this.readonly = this.readonly || this.isChecked;
                }
            }

            if (backup) {
                this.oldSetting = new SettingType21Item(this);
            }

        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType21Item = <SettingType21Item>this.oldSetting;

            return isChanged || this.isDisabled !== oldSetting.isDisabled;
        }

        public copySettingFrom(source: SettingType21Item): boolean {
            var isSuccessful: boolean = true;

            if (source) {
                var backupIsChecked: boolean = this.isChecked;

                if (!this.readonly) {
                    this.isChecked = source.isChecked;
                }

                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.isChecked = backupIsChecked;
                    this.highlightUnablyCopiedField();
                } else {
                    this.highlightCopiedFields({isDisabled: !backupIsChecked});
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
    }
}