/// <reference path="../../../typings/underscore/underscore.d.ts" />
/// <reference path="common.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export interface ISettingItem {
        typeName: string;
        oldSetting: ISettingItem;
        isChanged(): boolean;
        validate(): boolean;
        toJson(): {};
    }

    export class Level {
        public static company: number = 5;
        public static superAgent: number = 4;
        public static masterAgent: number = 3;
        public static agent: number = 2;
        public static member: number = 1;
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

        public copySettingTo(targets: Array<DefaultSettingItem>): boolean {
            var isSuccessful = true;

            if (targets) {
                _.forEach(targets, (item: DefaultSettingItem) => {
                    var validItem: boolean = true;

                    if (this.sportTypeId !== item.sportTypeId || this.betTypeId !== item.betTypeId || this.isLive !== item.isLive) {
                        validItem = item.copySettingFrom(this);
                        isSuccessful = isSuccessful ? validItem : isSuccessful;

                        if (!validItem) {
                            item.validationState.isWarning = true;
                        }
                    }
                });
            }

            return isSuccessful;
        }

        public copySettingFrom(source: DefaultSettingItem): boolean {
            throw Exceptions.notImplemented("copyFrom");
        }

        public validate(): boolean {
            return true;
        }

        public toJson(): {} {
            var settingItem: any = {};
            var reservedProperties: string[] = ["oldSetting", "validationState"];

            _.chain(this).keys().forEach((prop: string) => {
                if (this[prop] !== (undefined || null) && _.indexOf(reservedProperties, prop) < 0) {
                    if (this[prop].toJson) {
                        settingItem[prop] = this[prop].toJson();
                    } else {
                        settingItem[prop] = this[prop];
                    }
                }
            });

            return settingItem;
        }
    }

    export class SettingType {
        public id: number;
        public name: string;
        public settingLevel: number;
        public settingItems: Array<ISettingItem>;
        public typeName: string;
        public displayOrder: number;
        public displayMode: number;
        public extraData: Array<KeyValuePair<string, any>>;

        constructor();
        constructor(settingType: any)
        constructor(settingType?: any) {
            if (settingType) {
                this.id = settingType.id;
                this.name = settingType.name;
                this.settingLevel = settingType.settingLevel;
                this.settingItems = settingType.settingItems;
                this.typeName = settingType.typeName;
                this.displayOrder = settingType.displayOrder;
                this.displayMode = settingType.displayMode;
                this.extraData = this.getExtraData(settingType);
            }
        }

        public getChanges(): SettingType {
            var updatedSettingType: SettingType;
            var changedSettingItems: Array<ISettingItem> = _.filter(this.settingItems, (item: ISettingItem) => {
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
                updatedSettingType.settingLevel = this.settingLevel;
                updatedSettingType.settingItems = changedSettingItems;
                updatedSettingType.typeName = this.typeName;
                updatedSettingType.displayOrder = this.displayOrder;
                updatedSettingType.extraData = this.extraData;
            }

            return updatedSettingType;
        }

        public validate(): boolean {
            var validSettingItems: boolean = _.every(this.settingItems, (item: ISettingItem) => {
                return item.validate();
            });

            return this.id > 0
                && this.settingLevel > 0
                && this.typeName !== undefined
                && validSettingItems;
        }

        public toJson(): {} {
            var settingType: any = {
                id: this.id,
                name: this.name,
                settingLevel: this.settingLevel,
                settingItems: [],
                typeName: this.typeName,
                displayOrder: this.displayOrder
            };

            _.forEach(this.settingItems, (settingItem: ISettingItem) => {
                settingType.settingItems.push(settingItem.toJson());
            });

            if (this.extraData && this.extraData.length > 0) {
                _.forEach(this.extraData, (extraItem: KeyValuePair<string, any>) => {
                    settingType[extraItem.key] = extraItem.value;
                });
            }

            return settingType;
        }

        private getExtraData(rawData: any): Array<KeyValuePair<string, any>> {
            var extraData: Array<KeyValuePair<string, any>> = new Array<KeyValuePair<string, any>>();
            var addedProperties: string[] = _.chain(this).keys().filter((prop: string) => {
                return prop !== "extraData";
            }).value();

            extraData = _.chain(rawData).keys().filter((propName: string) => {
                return _.indexOf(addedProperties, propName) < 0;
            }).map((prop: string) => {
                return new KeyValuePair<string, any>(prop, rawData[prop]);
            }).value();

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

                if (setting.settingTypes) {
                    this.settingTypes = _.map(setting.settingTypes, (settingType: any) => new SettingType(settingType));
                } else {
                    this.settingTypes = new Array<SettingType>();
                }
            }
        }

        public getChanges(settingTypeId?: number): Setting {
            var newSetting: Setting;
            var matchedSettingTypes: Array<SettingType>;
            var changedSettingTypes: Array<SettingType> = new Array<SettingType>();

            if (settingTypeId) {
                matchedSettingTypes = _.filter(this.settingTypes, (item: SettingType) => {
                    return item.id === settingTypeId;
                });
            } else {
                matchedSettingTypes = this.settingTypes;
            }

            if (matchedSettingTypes && matchedSettingTypes.length > 0) {
                _.forEach(matchedSettingTypes, (settingType: SettingType) => {
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
            var validSettingTypes: boolean = _.every(this.settingTypes, (item: SettingType) => {
                return item.validate();
            });

            return this.customerLevel < Level.company
                && this.customerId > 0
                && this.productId > 0
                && validSettingTypes;
        }

        public toJson(): {} {
            var setting: any = {
                customerId: this.customerId,
                customerInfo: this.customerInfo,
                customerLevel: {
                    value: this.customerLevel
                },
                productId: this.productId,
                productName: this.productName,
                settingTypes: []
            };

            _.forEach(this.settingTypes, (settingType: SettingType) => {
                setting.settingTypes.push(settingType.toJson());
            });

            return setting;
        }
    }

    export class Result {
        public static normal: Result = new Result(0, "", MessageType.success);
        public static systemError: Result = new Result(-1, "SystemError", MessageType.error);
        public static noChangeToUpdate: Result = new Result(-2, "NoChangeToUpdate", MessageType.error);
        public static invalidParameters: Result = new Result(-3, Exceptions.invalidParams(), MessageType.error);

        public code: number;
        public message: string;
        public msgType: MessageType;

        constructor(code: number, message: string, msgType: MessageType) {
            this.code = code;
            this.message = message;
            this.msgType = msgType;
            this.msgType = this.code !== 0 ? MessageType.error : this.msgType;
        }

        public isSuccessful(): boolean {
            return this.code === 0;
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

        public isSuccessful(): boolean {
            return !this.settingResults
                || _.every(this.settingResults, (result: SettingResult) => {
                    return result.isSuccessful();
                });
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
                this.settingResults = _.filter(this.settingResults, (result: SettingResult) => {
                    return !result.isSuccessful();
                });
            }

            return this.settingResults || new Array<SettingResult>();
        }

        public removeDuplicatedMessages(): void {
            var uniquedResults: Array<SettingResult> = new Array<SettingResult>();

            _.forEach(this.settingResults, (item: SettingResult) => {
                if (!item.messages && uniquedResults.length > 0) {
                    var isDuplicated = _.some(uniquedResults, (anotherItem: SettingResult) => {
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

                    _.forEach(result.subResults, (subResult: any) => {
                        var existingCustomerResult: CustomerSettingResult = _.filter(this.subResults, (item: CustomerSettingResult) => {
                            return item.customerId === subResult.customerId;
                        })[0];

                        if (existingCustomerResult) {
                            existingCustomerResult.pushSettingResult(subResult);
                        } else {
                            this.subResults.push(new CustomerSettingResult(subResult));
                        }
                    });

                    _.forEach(this.subResults, (subResult: CustomerSettingResult) => {
                        subResult.removeDuplicatedMessages();
                    });
                }
            }
        }

        public isSuccessful(): boolean {
            return !this.subResults
                || _.every(this.subResults, (result: CustomerSettingResult) => {
                    return result.isSuccessful();
                });
        }

        public removeSuccessResults(): Array<CustomerSettingResult> {
            if (this.subResults) {
                this.subResults = _.filter(this.subResults, (result: CustomerSettingResult) => {
                    return !result.isSuccessful() && result.removeSuccessResults().length > 0;
                });
            }

            return this.subResults || new Array<CustomerSettingResult>();
        }
    }

    export class ValidationResult extends Result {
        public fieldName: string;
        public msgParams: any[];
        public isWarning: boolean;

        constructor(message: string, fieldName?: string, msgParams?: any[], isWarning: boolean = false) {
            super(-8000, message, MessageType.error);
            this.fieldName = fieldName;
            this.msgParams = msgParams;
            this.isWarning = isWarning;
        }
    }
}