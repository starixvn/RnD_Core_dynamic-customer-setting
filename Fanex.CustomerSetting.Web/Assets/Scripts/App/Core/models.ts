/// <reference path="common.ts" />

module CustomerSetting {
    export interface SettingItem {
        typeName: string;
        oldSetting: SettingItem;
        isChanged(): boolean;
        validate(): boolean;
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
        public validationState: ValidationResult;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem?: any) {
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

            this.validationState = <ValidationResult>ValidationResult.normal;
        }

        public isChanged(): boolean {
            return (this.sportTypeId !== this.oldSetting.sportTypeId)
                || (this.sportTypeName !== this.oldSetting.sportTypeName)
                || (this.betTypeId !== this.oldSetting.betTypeId)
                || (this.betTypeName !== this.oldSetting.betTypeName);
        }

        public validate(): boolean {
            return true;
        }
    }

    export class SettingType {
        public id: number;
        public name: string;
        public settingTemplate: number;
        public settingLevel: number;
        public settingItems: Array<SettingItem>;
        public typeName: string;
        public extraData: Array<KeyValuePair<string, any>>;

        constructor();
        constructor(settingType: any)
        constructor(settingType?: any) {
            if (settingType) {
                this.id = settingType.id;
                this.name = settingType.name;
                this.settingTemplate = settingType.settingTemplate;
                this.settingLevel = settingType.settingLevel;
                this.settingItems = settingType.settingItems;
                this.typeName = settingType.typeName;
                this.extraData = this.getExtraData(settingType);
            }
        }

        public getChanges(): SettingType {
            var updatedSettingType: SettingType;
            var changedSettingItems: Array<SettingItem> = this.settingItems.filter(item => {
                if (item.isChanged) {
                    return item.isChanged();
                } else {
                    return false;
                }
            });

            if (changedSettingItems && changedSettingItems.length > 0) {
                updatedSettingType = new SettingType();

                updatedSettingType.id = this.id;
                updatedSettingType.name = this.name;
                updatedSettingType.settingTemplate = this.settingTemplate;
                updatedSettingType.settingLevel = this.settingLevel;
                updatedSettingType.settingItems = changedSettingItems;
                updatedSettingType.typeName = this.typeName;

                // Workaround for the setting type that has some its own properties but we don't want to make new class for it.
                // We use the dynamic aspect of JavaScript in this case to keep the process be simple.
                if (this.extraData && this.extraData.length > 0) {
                    this.extraData.forEach(extraItem => {
                        (<any>updatedSettingType)[extraItem.key] = extraItem.value;
                    });
                }
            }

            return updatedSettingType;
        }

        public validate(): boolean {
            var validSettingItems: boolean = this.settingItems.every(item => {
                return item.validate();
            });

            return this.id > 0
                && this.settingTemplate > 0
                && this.settingLevel > 0
                && this.typeName !== undefined
                && validSettingItems;
        }

        private getExtraData(rawData: any): Array<KeyValuePair<string, any>> {
            var extraData: Array<KeyValuePair<string, any>> = new Array<KeyValuePair<string, any>>();
            var addedProperties: string[] = Object.keys(this).filter(prop => {
                return prop !== "extraData";
            });

            extraData = Object.keys(rawData).filter(propName => {
                return addedProperties.indexOf(propName) < 0;
            }).map(prop => {
                return new KeyValuePair<string, any>(prop, rawData[prop]);
            });

            return extraData;
        }
    }

    export class Setting {
        public customerId: number;
        public customerInfo: CustomerInfo;
        public customerLevel: Level;
        public productId: number;
        public productName: string;
        public settingTypes: Array<SettingType>;

        constructor();
        constructor(setting: any)
        constructor(setting?: any) {
            if (setting) {
                this.customerId = setting.customerId;
                this.customerInfo = new CustomerInfo(setting.customerInfo);
                this.customerLevel = setting.customerLevel.value;
                this.productId = setting.productId;
                this.productName = setting.productName;
                this.settingTypes = setting.settingTypes.map(settingType => new SettingType(settingType));
            }
        }

        public getChanges(settingTypeId?: number): Setting {
            var newSetting: Setting;
            var matchedSettingTypes: Array<SettingType>;
            var changedSettingTypes: Array<SettingType> = new Array<SettingType>();

            if (settingTypeId) {
                matchedSettingTypes = this.settingTypes.filter(item => {
                    return item.id === settingTypeId;
                });
            } else {
                matchedSettingTypes = this.settingTypes;
            }

            if (matchedSettingTypes && matchedSettingTypes.length > 0) {
                matchedSettingTypes.forEach(settingType => {
                    var changedSettingType: SettingType = settingType.getChanges();

                    if (changedSettingType) {
                        changedSettingTypes.push(changedSettingType);
                    }
                });
            }

            if (changedSettingTypes.length > 0) {
                newSetting = new Setting();

                newSetting.customerId = this.customerId;
                newSetting.customerInfo = this.customerInfo;
                newSetting.customerLevel = this.customerLevel;
                newSetting.productId = this.productId;
                newSetting.productName = this.productName;
                newSetting.settingTypes = changedSettingTypes;
            }

            return newSetting;
        }

        public validate(): boolean {
            var validSettingTypes: boolean = this.settingTypes.every(item => {
                return item.validate();
            });

            return this.customerLevel < Level.company
                && this.customerId > 0
                && this.productId > 0
                && validSettingTypes;
        }
    }

    export class Result {
        public get isSuccessful(): boolean {
            return this.code === 0;
        }

        constructor(
            public code: number,
            public message: string,
            public msgType: MessageType) {
            this.msgType = this.code !== 0 ? MessageType.error : this.msgType;
        }

        public static get normal(): Result {
            return new Result(0, "", MessageType.success);
        }

        public static get systemError(): Result {
            return new Result(-1, "SystemError", MessageType.error);
        }

        public static get noChangeToUpdate(): Result {
            return new Result(-2, "NoChangeToUpdate", MessageType.error);
        }

        public static get invalidParameters(): Result {
            return new Result(-3, Exceptions.invalidParams(), MessageType.error);
        }
    }

    export class Results extends Result {
        public subResults: Array<Result>

        public get isSuccessful(): boolean {
            return this.code === 0
                && (!this.subResults || this.subResults.every(result => {
                    return result.isSuccessful;
                }));
        }

        constructor(
            code?: number,
            message?: string,
            msgType?: MessageType,
            subResults?: any[]) {
            super(code, message, msgType);
            this.subResults = subResults.map(item => new Result(item.code, item.message, item.msgType));
        }

        public removeSuccessResults(): Array<Result> {
            if (this.subResults) {
                this.subResults = this.subResults.filter(result => {
                    return !result.isSuccessful;
                });
            }

            return this.subResults || new Array<Result>();
        }
    }

    export class ValidationResult extends Result {
        constructor(
            public message: string,
            public fieldName?: string,
            public msgParams?: any[]) {
            super(-8000, message, MessageType.error);
        }
    }
}