/// <reference path="../../../typings/tsd.d.ts" />
/// <reference path="module-registration.ts" />

module CustomerSetting {
    enum UserLevel {
        company = 5,
        super = 4,
        master = 3,
        agent = 2,
        member = 1
    }

    class CustomerInfo {
        public username: string;

        public username2: string;

        public firstName: string;

        public lastName: string;

        public phone: string;

        public mobilePhone: string;

        public fax: string;

        public email: string;
    }

    export interface ISettingItem {
        typeName: string;

        oldSetting: ISettingItem;
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
        private _customerLevel: UserLevel;

        public customerId: number;

        public customerInfo: CustomerInfo;

        public get customerLevel(): UserLevel {
            return this._customerLevel;
        }

        public set customerLevel(level: UserLevel) {
            if (level === UserLevel.company) {
                throw "Wrong customer level.";
            }

            this._customerLevel = level;
        }

        public productId: number;

        public productName: string;

        public settingType: Array<SettingType>;
    }
} 