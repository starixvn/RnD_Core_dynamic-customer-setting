/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="constants.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    export class AbstractSettingQuery {
        private _productId: number;
        private _uplineLevel: Level;
        private _customerLevel: Level;

        public get productId(): number {
            return this._productId;
        }

        public set productId(productId: number) {
            if (productId <= 0) {
                throw CommonErrorMessages.negativeParam("productId");
            }

            this._productId = productId;
        }

        public get uplineLevel(): Level {
            return this._uplineLevel;
        }

        public set uplineLevel(level: Level) {
            if (level < Level.agent) {
                throw CommonErrorMessages.invalidParam("uplineLevel");
            }

            if (level === Level.company) {
                level = null;
            }

            this._uplineLevel = level;
        }

        public get customerLevel(): Level {
            return this._customerLevel;
        }

        public set customerLevel(level: Level) {
            if (level < Level.member || level > Level.super) {
                throw CommonErrorMessages.wrongCustomerLevel;
            }

            this._customerLevel = level;
        }
    }

    export class SettingQuery extends AbstractSettingQuery {
        private _customerId: number;

        public get customerId(): number {
            return this._customerId;
        }

        public set customerId(custId: number) {
            if (custId <= 0) {
                throw CommonErrorMessages.negativeParam("customerId");
            }

            this._customerId = custId;
        }
    }

    export class MultipleSettingQuery extends AbstractSettingQuery {
        private _customerIds: Array<number>;
        private _customerNames: Array<string>;

        public getCustomerIds(): Array<number> {
            return this._customerIds;
        }

        public set customerIds(custIds: string) {
            if (custIds === undefined || custIds === "") {
                throw CommonErrorMessages.invalidParam("customerIds");
            }

            this._customerIds = custIds.split(",").map(custId => parseInt(custId.trim()));
        }

        public getCustomerNames(): Array<string> {
            return this._customerNames;
        }

        public set customerNames(names: string) {
            if (names === undefined || names === "") {
                throw CommonErrorMessages.invalidParam("customerNames");
            }

            this._customerNames = names.split(",").map(name => name.trim());
        }

        public getFirstCustomerSettingQuery(): SettingQuery {
            var query: SettingQuery = undefined;

            if (this._customerIds.length > 0) {
                query = new SettingQuery();

                query.customerId = this._customerIds[0];
                query.customerLevel = this.customerLevel;
                query.uplineLevel = this.uplineLevel;
                query.productId = this.productId;
            }

            return query;
        }

        public validate(): boolean {
            return this._customerIds.length === this._customerNames.length;
        }
    }

    export class SettingCommand {
        constructor(
            public uplineLevel: Level,
            public setting: Setting) { }
    }

    export class MultipleSettingCommand extends SettingCommand {
        constructor(public customerIds: Array<number>, public customerNames: Array<string>, uplineLevel: Level, setting: Setting) {
            super(uplineLevel, setting);
        }
    }

    export class ParametersFactory {
        private _isMultuiple: boolean;
        private _params: AbstractSettingQuery;

        constructor(private $location: ng.ILocationService, private $rootScope: ng.IRootScopeService) {
            this.searchForParams();

            $rootScope.$on("$locationChangeStart", () => {
                this.searchForParams();
                $rootScope.$emit("customer-setting-path-changed");
            });
        }

        public getSettingQuery(): SettingQuery {
            if (this._isMultuiple) {
                var multipleQuery = <MultipleSettingQuery>this._params;

                if (!multipleQuery.validate()) {
                    throw CommonErrorMessages.customerIdsNotMatchCustomerNames();
                }

                return multipleQuery.getFirstCustomerSettingQuery();
            } else {
                return <SettingQuery>this._params;
            }
        }

        public createSettingCommand(setting: Setting): SettingCommand {
            if (this._isMultuiple) {
                var query = <MultipleSettingQuery>this._params;

                return new MultipleSettingCommand(
                    query.getCustomerIds(),
                    query.getCustomerNames(),
                    query.uplineLevel,
                    setting);
            } else {
                return new SettingCommand(this._params.uplineLevel, setting);
            }
        }

        private searchForParams(): void {
            var params: any = this.$location.search();

            if (params !== undefined) {
                this._isMultuiple = params.isMultiple;
                this._params = params;
            }

            this._params = undefined;
        }
    }
}