/// <reference path="../core/common.ts" />
/// <reference path="../core/models.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType15Item extends DefaultSettingItem {
        static roundingFactor: number = 100;

        public winningShare: number;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.winningShare = this.modifyWinningShareToDisplayForm(settingItem.winningShare);
            }

            if (backup) {
                this.oldSetting = new SettingType15Item(this);
            }
        }

        public toJson(): {} {
            var settingItem: any = {
                sportTypeId: this.sportTypeId,
                sportTypeName: this.sportTypeName,
                betTypeId: this.betTypeId,
                betTypeName: this.betTypeName,
                typeName: this.typeName,
                winningShare: this.winningShare
            };

            return settingItem;
        }

        private modifyWinningShareToDisplayForm(rate: number): number {
            return parseFloat((rate * SettingType15Item.roundingFactor).toFixed(2));
        }
    }
}