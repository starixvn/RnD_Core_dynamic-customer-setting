/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="common.ts" />
/// <reference path="helpers.ts" />
/// <reference path="models.ts" />
/// <reference path="module-registrater.ts" />

module CustomerSetting {
    "use strict";

    export class AbstractSettingQuery {
        private prodId: number;
        private uplineLv: Level;
        private custLv: Level;

        public get productId(): number {
            return this.prodId;
        }

        public set productId(productId: number) {
            if (productId <= 0) {
                throw Exceptions.negativeParam("productId");
            }

            this.prodId = productId;
        }

        public get uplineLevel(): Level {
            return this.uplineLv;
        }

        public set uplineLevel(level: Level) {
            if (level < Level.agent) {
                throw Exceptions.invalidParam("uplineLevel");
            }

            if (level === Level.company) {
                level = null;
            }

            this.uplineLv = level;
        }

        public get customerLevel(): Level {
            return this.custLv;
        }

        public set customerLevel(level: Level) {
            if (level < Level.member || level > Level.super) {
                throw Exceptions.wrongCustomerLevel;
            }

            this.custLv = level;
        }

        constructor(productId: number, uplineLevel: Level, customerLevel: Level) {
            this.productId = productId;
            this.uplineLevel = uplineLevel;
            this.customerLevel = customerLevel;
        }
    }

    export class SettingQuery extends AbstractSettingQuery {
        private custId: number;

        public get customerId(): number {
            return this.custId;
        }

        public set customerId(custId: number) {
            if (custId <= 0) {
                throw Exceptions.negativeParam("customerId");
            }

            this.custId = custId;
        }

        constructor(productId: number, uplineLevel: Level, customerLevel: Level, customerId: number) {
            super(productId, uplineLevel, customerLevel);
            this.customerId = customerId;
        }

        public toJson(): {} {
            var params: any = {
                customerId: this.customerId,
                customerLevel: this.customerLevel,
                productId: this.productId
            };

            if (this.uplineLevel) {
                params.uplineLevel = this.uplineLevel;
            }

            return params;
        }
    }

    export class MultipleSettingQuery extends AbstractSettingQuery {
        private custIds: Array<number>;
        private custNames: Array<string>;

        public get customerIds(): string {
            return this.custIds.join();
        }

        public set customerIds(custIds: string) {
            if (!custIds) {
                throw Exceptions.invalidParam("customerIds");
            }

            this.custIds = custIds.split(",").map((custId: string) => parseInt(custId.trim(), 10));
        }

        public get customerNames(): string {
            return this.custNames.join();
        }

        public set customerNames(names: string) {
            if (!names) {
                throw Exceptions.invalidParam("customerNames");
            }

            this.custNames = names.split(",").map((name: string) => name.trim());
        }

        constructor(productId: number, uplineLevel: Level, customerLevel: Level, customerIds: string, customerNames: string) {
            super(productId, uplineLevel, customerLevel);
            this.customerIds = customerIds;
            this.customerNames = customerNames;
        }

        public getFirstCustomerSettingQuery(): SettingQuery {
            var query: SettingQuery;

            if (this.custIds.length > 0 && this.validate()) {
                query = new SettingQuery(this.productId, this.uplineLevel, this.customerLevel, this.custIds[0]);
            }

            return query;
        }

        public validate(): boolean {
            return this.custIds.length === this.custNames.length;
        }
    }

    export class SettingCommand {
        public uplineLevel: Level;
        public setting: Setting;

        constructor(uplineLevel: Level, setting: Setting) {
            this.uplineLevel = uplineLevel;
            this.setting = setting;
        }

        public validate(): Result {
            if ((this.uplineLevel && (this.uplineLevel <= Level.member || this.uplineLevel >= Level.company)) || !this.setting) {
                return Result.invalidParameters;
            }

            return Result.normal;
        }

        public toJson(): {} {
            return {
                uplineLevel: this.uplineLevel,
                setting: JSON.stringify(this.setting.toJson())
            };
        }
    }

    export class MultipleSettingCommand extends SettingCommand {
        public customerIds: string;
        public customerNames: string;

        constructor(customerIds: string, customerNames: string, uplineLevel: Level, setting: Setting) {
            super(uplineLevel, setting);
            this.customerIds = customerIds;
            this.customerNames = customerNames;
        }

        public validate(): Result {
            return super.validate() || (() => {
                if (!this.customerIds || !this.customerNames) {
                    return Result.invalidParameters;
                }

                return Result.normal;
            })();
        }

        public toJson(): {} {
            return {
                uplineLevel: this.uplineLevel,
                customerIds: this.customerIds,
                customerNames: this.customerNames,
                setting: JSON.stringify(this.setting.toJson())
            };
        }
    }

    export class SettingPartialCommand {
        public uplineLevel: Level;
        public customerLevel: Level;
        public productId: number;
        public productName: string;
        public settingType: SettingType;

        constructor(uplineLevel: Level, customerLevel: Level, productId: number, productName: string, settingType: SettingType) {
            this.uplineLevel = uplineLevel;
            this.customerLevel = customerLevel;
            this.productId = productId;
            this.productName = productName;
            this.settingType = settingType;
        }
    }
}