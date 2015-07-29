/// <reference path="common.ts" />

module CustomerSetting {
    export interface SettingItem {
        typeName: string;
        oldSetting: SettingItem;
        backup(): void;
        isChanged(): boolean;
    }

    export enum Level {
        company = 5,
        super = 4,
        master = 3,
        agent = 2,
        member = 1
    }

    export class CustomerInfo {
        public userName: string;
        public userName2: string;
        public firstName: string;
        public lastName: string;
        public phone: string;
        public mobilePhone: string;
        public fax: string;
        public email: string;

        constructor(info: any) {
            if (info) {
                this.userName = info.userName;
                this.userName2 = info.userName2;
                this.firstName = info.firstName;
                this.lastName = info.lastName;
                this.phone = info.phone;
                this.mobilePhone = info.mobilePhone;
                this.fax = info.fax;
                this.email = info.email;
            }
        }
    }

    export class DefaultSettingItem implements SettingItem {
        public sportTypeId: number;
        public sportTypeName: string;
        public betTypeId: number;
        public betTypeName: string;
        public typeName: string;
        public isLive: boolean;
        public oldSetting: DefaultSettingItem;

        constructor(settingItem: any) {
            if (settingItem) {
                this.sportTypeId = settingItem.sportTypeId;
                this.sportTypeName = settingItem.sportTypeName;
                this.betTypeId = settingItem.betTypeId;
                this.betTypeName = settingItem.betTypeName;
                this.typeName = settingItem.typeName;

                if (settingItem.isLive !== undefined) {
                    this.isLive = settingItem.isLive;
                }
            }
        }

        public backup(): void {
            this.oldSetting = new DefaultSettingItem(this);
        }

        public isChanged(): boolean {
            return (this.sportTypeId !== this.oldSetting.sportTypeId)
                || (this.sportTypeName !== this.oldSetting.sportTypeName)
                || (this.betTypeId !== this.oldSetting.betTypeId)
                || (this.betTypeName !== this.oldSetting.betTypeName);
        }
    }

    export class SettingType {
        public id: number;
        public name: string;
        public settingTemplate: number;
        public settingLevel: number;
        public settingItems: Array<SettingItem>;
        public typeName: string;

        constructor(settingType: any) {
            if (settingType) {
                this.id = settingType.id;
                this.name = settingType.name;
                this.settingTemplate = settingType.settingTemplate;
                this.settingLevel = settingType.settingLevel;
                this.settingItems = settingType.settingItems;
                this.typeName = settingType.typeName;
            }
        }
    }

    export class Setting {
        private custLevel: Level;

        public customerId: number;
        public customerInfo: CustomerInfo;

        public get customerLevel(): Level {
            return this.custLevel;
        }

        public set customerLevel(level: Level) {
            if (level === Level.company) {
                throw Exceptions.wrongCustomerLevel;
            }

            this.custLevel = level;
        }

        public productId: number;
        public productName: string;
        public settingTypes: Array<SettingType>;

        constructor();
        constructor(setting: any)
        constructor(setting?: any) {
            if (setting) {
                this.customerId = setting.productId;
                this.customerInfo = new CustomerInfo(setting.customerInfo);
                this.customerLevel = setting.customerLevel.value;
                this.productId = setting.productId;
                this.productName = setting.productName;
                this.settingTypes = setting.settingTypes.map((settingType) => new SettingType(settingType));
            }
        }

        public getSettingType(settingTypeId: number): Setting {
            var newSetting: Setting = new Setting();

            newSetting.customerId = this.productId;
            newSetting.customerInfo = this.customerInfo;
            newSetting.customerLevel = this.customerLevel;
            newSetting.productId = this.productId;
            newSetting.productName = this.productName;
            newSetting.settingTypes = this.settingTypes.filter((item) => {
                return item.id === settingTypeId;
            });

            return newSetting;
        }
    }

    export class Result {
        constructor(
            public code: number,
            public message: string,
            public msgType: MessageType) { }

        public static get systemError(): Result {
            return new Result(-1, "SystemError", MessageType.error);
        }

        public static get noChangeToUpdate(): Result {
            return new Result(-2, "NoChangeToUpdate", MessageType.error);
        }
    }

    export class SettingResult extends Result {
        constructor(
            public code?: number,
            public message?: string,
            public msgType?: MessageType,
            public subResults?: Array<Result>) {
            super(code, message, msgType);
        }
    }
}