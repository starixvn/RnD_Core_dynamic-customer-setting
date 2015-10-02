/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType5Item extends DefaultSettingItem {
        public stakeLevelId: number;
        public min: number;
        public max: number;
        public selected: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.stakeLevelId = settingItem.stakeLevelId;
                this.min = settingItem.min;
                this.max = settingItem.max;
                this.selected = settingItem.selected;
            }

            if (backup) {
                this.oldSetting = new SettingType5Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: SettingType5Item = <SettingType5Item>this.oldSetting;

            return isChanged
                || this.selected !== oldSetting.selected;
        }
    }
}