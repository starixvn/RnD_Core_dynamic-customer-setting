/// <reference path="common.ts" />

module CustomerSetting {
    export interface SettingItem {
        typeName: string;
        oldSetting: SettingItem;
    }

    export enum Level {
        company = 5,
        super = 4,
        master = 3,
        agent = 2,
        member = 1
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

    export class DefaultSettingItem implements SettingItem {
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
                throw Exceptions.wrongCustomerLevel;
            }

            this._customerLevel = level;
        }

        public productId: number;
        public productName: string;
        public settingType: Array<SettingType>;
    }

    export class Result {
        constructor(
            public code: number,
            public message: string,
            public messageType: MessageType) { }

        public get systemError(): Result {
            return new Result(-1, "SystemError", MessageType.error);
        }

        public get noChangeToUpdate(): Result {
            return new Result(-2, "NoChangeToUpdate", MessageType.error);
        }
    }
}