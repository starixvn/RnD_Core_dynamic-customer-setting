/// <reference path="common.ts" />

module CustomerSetting {
    "use strict";

    export interface ISettingItem {
        typeName: string;
        oldSetting: ISettingItem;
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

    export class DefaultSettingItem implements ISettingItem {
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
        public settingItems: Array<ISettingItem>;
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
            var changedSettingItems: Array<ISettingItem> = this.settingItems.filter((item: ISettingItem) => {
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
                    this.extraData.forEach((extraItem: KeyValuePair<string, any>) => {
                        (<any>updatedSettingType)[extraItem.key] = extraItem.value;
                    });
                }
            }

            return updatedSettingType;
        }

        public validate(): boolean {
            var validSettingItems: boolean = this.settingItems.every((item: ISettingItem) => {
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
            var addedProperties: string[] = Object.keys(this).filter((prop: string) => {
                return prop !== "extraData";
            });

            extraData = Object.keys(rawData).filter((propName: string) => {
                return addedProperties.indexOf(propName) < 0;
            }).map((prop: string) => {
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
                this.settingTypes = setting.settingTypes.map((settingType: any) => new SettingType(settingType));
            }
        }

        public getChanges(settingTypeId?: number): Setting {
            var newSetting: Setting;
            var matchedSettingTypes: Array<SettingType>;
            var changedSettingTypes: Array<SettingType> = new Array<SettingType>();

            if (settingTypeId) {
                matchedSettingTypes = this.settingTypes.filter((item: SettingType) => {
                    return item.id === settingTypeId;
                });
            } else {
                matchedSettingTypes = this.settingTypes;
            }

            if (matchedSettingTypes && matchedSettingTypes.length > 0) {
                matchedSettingTypes.forEach((settingType: SettingType) => {
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
            var validSettingTypes: boolean = this.settingTypes.every((item: SettingType) => {
                return item.validate();
            });

            return this.customerLevel < Level.company
                && this.customerId > 0
                && this.productId > 0
                && validSettingTypes;
        }
    }

    export class Result {
        public code: number;
        public message: string;
        public msgType: MessageType;

        public get isSuccessful(): boolean {
            return this.code === 0;
        }

        constructor(code: number, message: string, msgType: MessageType) {
            this.code = code;
            this.message = message;
            this.msgType = msgType;
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

    export class SettingResult extends Result {
        public settingTypeId: number;
        public settingTypeName: string;
        public messages: Array<string>;

        constructor(
            code: number,
            message: string,
            msgType: MessageType,
            settingTypeId: number,
            settingTypeName: string,
            messages: Array<string>) {
            super(code, message, msgType);
            this.settingTypeId = settingTypeId;
            this.settingTypeName = settingTypeName;
            this.messages = messages;
        }
    }

    export class CustomerSettingResult {
        public customerId: number;
        public customerName: string;
        public settingResults: Array<SettingResult>;

        public get isSuccessful(): boolean {
            return !this.settingResults
                || this.settingResults.every((result: SettingResult) => {
                    return result.isSuccessful;
                });
        }

        constructor(settingResult: any) {
            if (settingResult) {
                this.customerId = settingResult.customerId;
                this.customerName = settingResult.customerName;
                this.settingResults = new Array<SettingResult>();
                this.settingResults.push(new SettingResult(
                    settingResult.code,
                    settingResult.message,
                    settingResult.msgType,
                    settingResult.settingTypeId,
                    settingResult.settingTypeName,
                    settingResult.subMessages));
            }
        }

        public pushSettingResult(settingResult: any) {
            if (settingResult) {
                this.settingResults.push(new SettingResult(
                    settingResult.code,
                    settingResult.message,
                    settingResult.msgType,
                    settingResult.settingTypeId,
                    settingResult.settingTypeName,
                    settingResult.subMessages));
            }
        }

        public removeSuccessResults(): Array<SettingResult> {
            if (this.settingResults) {
                this.settingResults = this.settingResults.filter((result: SettingResult) => {
                    return !result.isSuccessful;
                });
            }

            return this.settingResults || new Array<SettingResult>();
        }

        public removeDuplicatedMessages(): void {
            var uniquedResults: Array<SettingResult> = new Array<SettingResult>();

            this.settingResults.forEach((item: SettingResult) => {
                if (!item.messages && uniquedResults.length > 0) {
                    var isDuplicated = uniquedResults.some((anotherItem: SettingResult) => {
                        return !anotherItem.messages
                            && item.code === anotherItem.code
                            && item.message === anotherItem.message;
                    });

                    if (!isDuplicated) {
                        uniquedResults.push(item);
                    }
                } else {
                    uniquedResults.push(item);
                }
            });

            this.settingResults = uniquedResults;
        }
    }

    export class UpdatingSettingResult extends Result {
        public subResults: Array<CustomerSettingResult>;

        constructor(result: any) {
            if (result) {
                super(result.code, result.message, result.msgType);

                if (result.subResults) {
                    this.subResults = new Array<CustomerSettingResult>();

                    result.subResults.forEach((subResult: any) => {
                        var existingCustomerResult: CustomerSettingResult = this.subResults.filter((item: CustomerSettingResult) => {
                            return item.customerId === subResult.customerId;
                        })[0];

                        if (existingCustomerResult) {
                            existingCustomerResult.pushSettingResult(subResult);
                        } else {
                            this.subResults.push(new CustomerSettingResult(subResult));
                        }
                    });

                    this.subResults.forEach((subResult: CustomerSettingResult) => {
                        subResult.removeDuplicatedMessages();
                    });
                }
            }
        }

        public removeSuccessResults(): Array<CustomerSettingResult> {
            if (this.subResults) {
                this.subResults = this.subResults.filter((result: CustomerSettingResult) => {
                    return !result.isSuccessful && result.removeSuccessResults().length > 0;
                });
            }

            return this.subResults || new Array<CustomerSettingResult>();
        }
    }

    export class Results extends Result {
        public subResults: Array<Result>;

        public get isSuccessful(): boolean {
            return this.code === 0
                && (!this.subResults || this.subResults.every((result: Result) => {
                    return result.isSuccessful;
                }));
        }

        constructor(
            code?: number,
            message?: string,
            msgType?: MessageType,
            subResults?: any[]) {
            super(code, message, msgType);
            this.subResults = subResults.map((item: any) => new Result(item.code, item.message, item.msgType));
        }

        public removeSuccessResults(): Array<Result> {
            if (this.subResults) {
                this.subResults = this.subResults.filter((result: Result) => {
                    return !result.isSuccessful;
                });
            }

            return this.subResults || new Array<Result>();
        }
    }

    export class ValidationResult extends Result {
        public fieldName: string;
        public msgParams: any[];

        constructor(message: string, fieldName?: string, msgParams?: any[]) {
            super(-8000, message, MessageType.error);
            this.fieldName = fieldName;
            this.msgParams = msgParams;
        }
    }
}