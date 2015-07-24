/// <reference path="constants.ts" />

module CustomerSetting {
    export interface ISettingItem {
        typeName: string;

        oldSetting: ISettingItem;
    }

    export class CustomerInfo {
        public username: string;

        public username2: string;

        public firstName: string;

        public lastName: string;

        public phone: string;

        public mobilePhone: string;

        public fax: string;

        public email: string;
    }

    export class DefaultSettingItem implements ISettingItem {
        public sportTypeId: number;

        public sportTypeName: string;

        public betTypeId: number;

        public betTypeName: string;

        public typeName: string;

        public oldSetting: DefaultSettingItem;
    }

    export class SettingType {
        public id: number;

        public name: string;

        public settingTemplate: number;

        public settingLevel: number;
    }

    export class Setting {
        private _customerLevel: Level;

        public customerId: number;

        public customerInfo: CustomerInfo;

        public get customerLevel(): Level {
            return this._customerLevel;
        }

        public set customerLevel(level: Level) {
            if (level === Level.company) {
                throw CommonErrorMessages.wrongCustomerLevel;
            }

            this._customerLevel = level;
        }

        public productId: number;

        public productName: string;

        public settingType: Array<SettingType>;
    }
}