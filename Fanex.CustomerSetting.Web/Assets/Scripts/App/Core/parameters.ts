/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="common.ts" />
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

        constructor();
        constructor(param: any);
        constructor(param?: any) {
            if (param) {
                this.productId = param.productId;
                this.uplineLevel = param.uplineLevel;
                this.customerLevel = param.customerLevel;
            }
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

        constructor();
        constructor(param: any);
        constructor(param?: any) {
            super(param);

            if (param) {
                this.customerId = param.customerId;
            }
        }

        public toParamString(): string {
            var paramString: string = "customerId=" + this.customerId + "&customerLevel=" + this.customerLevel + "&productId=" + this.productId;

            if (this.uplineLevel) {
                paramString += "&uplineLevel=" + this.uplineLevel;
            }

            return paramString;
        }
    }

    export class MultipleSettingQuery extends AbstractSettingQuery {
        private custIds: Array<number>;
        private custNames: Array<string>;

        public getCustomerIds(): Array<number> {
            return this.custIds;
        }

        public set customerIds(custIds: string) {
            if (!custIds || custIds === "") {
                throw Exceptions.invalidParam("customerIds");
            }

            this.custIds = custIds.split(",").map(custId => parseInt(custId.trim()));
        }

        public getCustomerNames(): Array<string> {
            return this.custNames;
        }

        public set customerNames(names: string) {
            if (!names || names === "") {
                throw Exceptions.invalidParam("customerNames");
            }

            this.custNames = names.split(",").map(name => name.trim());
        }

        constructor();
        constructor(param: any);
        constructor(param?: any) {
            super(param);

            if (param) {
                this.customerIds = param.customerIds;
                this.customerNames = param.customerNames;
            }
        }

        public getFirstCustomerSettingQuery(): SettingQuery {
            var query: SettingQuery = undefined;

            if (this.custIds.length > 0 && this.validate()) {
                query = new SettingQuery();

                query.customerId = this.custIds[0];
                query.customerLevel = this.customerLevel;
                query.uplineLevel = this.uplineLevel;
                query.productId = this.productId;
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
    }

    export class MultipleSettingCommand extends SettingCommand {
        public customerIds: Array<number>;
        public customerNames: Array<string>;

        constructor(customerIds: Array<number>, customerNames: Array<string>, uplineLevel: Level, setting: Setting) {
            super(uplineLevel, setting);
            this.customerIds = customerIds;
            this.customerNames = customerNames;
        }
    }
}