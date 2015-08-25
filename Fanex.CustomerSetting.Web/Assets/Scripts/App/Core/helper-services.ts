/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="module-registrater.ts" />
/// <reference path="parameters.ts" />

module CustomerSetting {
    "use strict";

    export class AppConfigService {
        public static $inject: string[] = ["$window"];

        private rootUrl: string;
        private debug: boolean;
        private version: string;
        private antiForgeryToken: string;

        private $window: ng.IWindowService;

        constructor($window: ng.IWindowService) {
            var appConfig: any;

            this.$window = $window;

            /* tslint:disable:no-string-literal */
            appConfig = this.$window["appConfig"] || {};
            /* tslint:enable:no-string-literal */

            if (appConfig) {
                this.rootUrl = appConfig.rootUrl;
                this.debug = appConfig.isDebug;
                this.version = appConfig.scriptVersion;
                this.antiForgeryToken = appConfig.antiForgeryToken;
            }
        }

        public resolveRootUrl(url: string): string {
            return this.rootUrl + url;
        }

        public debugMode(): boolean {
            return this.debug;
        }

        public scriptVersion(): string {
            return this.version;
        }

        public requestVerificationToken(): string {
            return this.antiForgeryToken;
        }
    }

    ModuleRegister.service("appConfigService", AppConfigService);

    export class ParametersService {
        public static $inject: string[] = ["$window"];

        public get language(): string {
            return this.lang;
        }

        public get customerNames(): string {
            return (<MultipleSettingQuery>this.query).customerNames;
        }

        //// Jason please review here
        public get customerIds(): string {
            return (<MultipleSettingQuery>this.query).customerIds;
        }

        public get uplineLevel(): Level {
            return this.query.uplineLevel;
        }

        private query: SettingQuery | MultipleSettingQuery;
        private lang: string;
        public isMultipleUpdates: boolean; //// Jason please review here

        private $window: ng.IWindowService;

        constructor($window: ng.IWindowService) {
            this.$window = $window;
            this.fetchParams();
        }

        public getSettingQuery(): SettingQuery {
            if (this.isMultipleUpdates) {
                var multipleQuery: MultipleSettingQuery = <MultipleSettingQuery>this.query;

                if (!multipleQuery.validate()) {
                    throw Exceptions.customerIdsNotMatchCustomerNames();
                }

                return multipleQuery.getFirstCustomerSettingQuery();
            } else {
                return <SettingQuery>this.query;
            }
        }

        public createSettingCommand(setting: Setting): SettingCommand {
            if (this.isMultipleUpdates) {
                var multipleQuery: MultipleSettingQuery = <MultipleSettingQuery>this.query;

                if (!multipleQuery.validate()) {
                    throw Exceptions.customerIdsNotMatchCustomerNames();
                }

                return new MultipleSettingCommand(
                    multipleQuery.getCustomerIds(),
                    multipleQuery.getCustomerNames(),
                    multipleQuery.uplineLevel,
                    setting);
            } else {
                return new SettingCommand(this.query.uplineLevel, setting);
            }
        }

        private fetchParams(): void {
            /* tslint:disable:no-string-literal */
            var params: any = this.$window["params"];
            /* tslint:enable:no-string-literal */

            if (params) {
                var uplineLevel = params.uplineLevel ? params.uplineLevel : undefined;
                var customerLevel = params.customerLevel ? params.customerLevel : undefined;

                this.isMultipleUpdates = params.isMultiple || false;
                this.lang = params.language || Defaults.language;

                if (this.isMultipleUpdates) {
                    this.query = new MultipleSettingQuery(
                        params.productId,
                        uplineLevel,
                        customerLevel,
                        params.customerIds,
                        params.customerNames);
                } else {
                    this.query = new SettingQuery(
                        params.productId,
                        uplineLevel,
                        customerLevel,
                        params.customerId);
                }
            }
        }
    }

    ModuleRegister.service("parametersService", ParametersService);

    export class ValueRangeService {
        public generateRanges(value: number, min: number, max: number, step: number, roundingFactor: number): Array<KeyValuePair<number, number>> {
            var ranges: Array<KeyValuePair<number, number>> = new Array<KeyValuePair<number, number>>();

            min = Math.max(0, min);
            max = Math.max(0, max);

            if (value > max) {
                ranges.push(new KeyValuePair<number, number>(value, value));
            }

            while (max >= min) {
                ranges.push(new KeyValuePair<number, number>(max, max));
                max -= step;
                max = Math.round(max * roundingFactor) / roundingFactor;
            }

            if (value < min) {
                ranges.push(new KeyValuePair<number, number>(value, value));
            }

            return ranges;
        }
    }

    ModuleRegister.service("valueRangeService", ValueRangeService);

    function NAValue() {
        return (value: string) => {
            return value || "---";
        };
    }

    ModuleRegister.filter("naValue", NAValue);

    function MinValue() {
        return (numbers: number[]) => {
            return Math.min.apply(Math, numbers);
        };
    }

    ModuleRegister.filter("minValue", MinValue);

    function MaxValue() {
        return (numbers: number[]) => {
            return Math.max.apply(Math, numbers);
        };
    }

    ModuleRegister.filter("maxValue", MaxValue);
}