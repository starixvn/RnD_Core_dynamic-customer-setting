/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="common.ts" />
/// <reference path="helpers.ts" />
/// <reference path="models.ts" />
/// <reference path="module-registrater.ts" />

module CustomerSetting {
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

        public toJson(): any {
            var params: any = {
                customerId: this.customerId,
                customerLevel: this.customerLevel,
                productId: this.productId
            }

            if (this.uplineLevel) {
                params.uplineLevel = this.uplineLevel;
            }

            return params;
        }
    }

    export class MultipleSettingQuery extends AbstractSettingQuery {
        private custIds: Array<number>;
        private custNames: Array<string>;

        public getCustomerIds(): Array<number> {
            return this.custIds;
        }

        public set customerIds(custIds: string) {
            if (!custIds) {
                throw Exceptions.invalidParam("customerIds");
            }

            this.custIds = custIds.split(",").map((custId: string) => parseInt(custId.trim()));
        }

        public getCustomerNames(): Array<string> {
            return this.custNames;
        }

        public set customerNames(names: string) {
            if (!names) {
                throw Exceptions.invalidParam("customerNames");
            }

            this.custNames = names.split(",").map((name: string) => name.trim());
        }

        public get customerNames(): string {
            return this.custNames.join();
        }

        constructor(productId: number, uplineLevel: Level, customerLevel: Level, customerIds: string, customerNames: string) {
            super(productId, uplineLevel, customerLevel);
            this.customerIds = customerIds;
            this.customerNames = customerNames;
        }

        public getFirstCustomerSettingQuery(): SettingQuery {
            var query: SettingQuery = undefined;

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
        constructor(
            public uplineLevel: Level,
            public setting: Setting) { }

        public validate(): Result {
            if (this.uplineLevel && (this.uplineLevel <= Level.member || this.uplineLevel >= Level.company)
                || (!this.setting)) {
                return Result.invalidParameters;
            }

            return Result.normal;
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
    }
}