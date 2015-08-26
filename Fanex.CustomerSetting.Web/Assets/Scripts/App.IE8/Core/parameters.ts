/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="common.ts" />
/// <reference path="helpers.ts" />
/// <reference path="models.ts" />
/// <reference path="module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class AbstractSettingQuery {
        protected productId: number;
        protected uplineLevel: Level;
        protected customerLevel: Level;

        constructor(productId: number, uplineLevel: Level, customerLevel: Level) {
            if (productId <= 0) {
                throw Exceptions.negativeParam("productId");
            }

            this.productId = productId;

            if (uplineLevel < Level.agent) {
                throw Exceptions.invalidParam("uplineLevel");
            }

            if (uplineLevel === Level.company) {
                uplineLevel = null;
            }

            this.uplineLevel = uplineLevel;

            if (customerLevel < Level.member || customerLevel > Level.superAgent) {
                throw Exceptions.wrongCustomerLevel;
            }

            this.customerLevel = customerLevel;
        }

        public getUplineLevel(): Level {
            return this.uplineLevel;
        }
    }

    export class SettingQuery extends AbstractSettingQuery {
        protected customerId: number;

        constructor(productId: number, uplineLevel: Level, customerLevel: Level, customerId: number) {
            super(productId, uplineLevel, customerLevel);

            if (customerId <= 0) {
                throw Exceptions.negativeParam("customerId");
            }

            this.customerId = customerId;
        }

        public toJson(): {} {
            var params: any = {
                customerId: this.customerId,
                customerLevel: this.customerLevel,
                productId: this.productId
            };

            if (this.getUplineLevel()) {
                params.uplineLevel = this.uplineLevel;
            }

            return params;
        }
    }

    export class MultipleSettingQuery extends AbstractSettingQuery {
        private customerIds: Array<number>;
        private customerNames: Array<string>;

        constructor(productId: number, uplineLevel: Level, customerLevel: Level, customerIds: string, customerNames: string) {
            super(productId, uplineLevel, customerLevel);
            if (!customerIds) {
                throw Exceptions.invalidParam("customerIds");
            }

            this.customerIds = customerIds.split(",").map((custId: string) => parseInt(custId.trim(), 10));

            if (!customerNames) {
                throw Exceptions.invalidParam("customerNames");
            }

            this.customerNames = customerNames.split(",").map((name: string) => name.trim());
        }

        public getCustomerIds(): Array<number> {
            return this.customerIds;
        }

        public getCustomerNames(): Array<string> {
            return this.customerNames;
        }

        public getFirstCustomerSettingQuery(): SettingQuery {
            var query: SettingQuery;

            if (this.customerIds.length > 0 && this.validate()) {
                query = new SettingQuery(this.productId, this.uplineLevel, this.customerLevel, this.customerIds[0]);
            }

            return query;
        }

        public validate(): boolean {
            return this.customerIds.length === this.customerNames.length;
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
        public customerIds: Array<number>;
        public customerNames: Array<string>;

        constructor(customerIds: Array<number>, customerNames: Array<string>, uplineLevel: Level, setting: Setting) {
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
}