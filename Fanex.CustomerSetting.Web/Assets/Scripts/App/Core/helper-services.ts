/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="module-registrater.ts" />
/// <reference path="parameters.ts" />

module CustomerSetting {
    export class AppConfigService {
        public static $inject: string[] = ["$rootScope"];

        private rootUrl: string;
        private debug: boolean;
        private version: string;

        constructor(private $rootScope: ng.IRootScopeService) { }

        public resolveRootUrl(url: string): string {
            if (!this.rootUrl) {
                this.rootUrl = this.$rootScope["rootUrl"];
            }

            return this.rootUrl + url;
        }

        public debugMode(): boolean {
            if (!this.debug) {
                this.debug = this.$rootScope["debug"] === "true";
            }

            return this.debug;
        }

        public scriptVersion(): string {
            if (!this.version) {
                this.version = this.$rootScope["scriptVersion"];
            }

            return this.version;
        }
    }

    ModuleRegister.service("appConfigService", AppConfigService);

    export class ParametersService {
        private params: AbstractSettingQuery;
        private isMultiple: boolean;
        private lang: string;
        private custNames: string;
        private uplineLvl: Level;

        public static $inject: string[] = ["$location"];

        public get language(): string {
            return this.lang;
        }

        public get customerNames(): string {
            return this.custNames;
        }

        public get uplineLevel(): Level {
            return this.uplineLvl;
        }

        constructor(private $location: ng.ILocationService) {
            this.searchForParams();
        }

        public getSettingQuery(): SettingQuery {
            if (this.isMultiple) {
                var multipleQuery: MultipleSettingQuery = new MultipleSettingQuery(this.params);

                if (!multipleQuery.validate()) {
                    throw Exceptions.customerIdsNotMatchCustomerNames();
                }

                return multipleQuery.getFirstCustomerSettingQuery();
            } else {
                return new SettingQuery(this.params);
            }
        }

        public createSettingCommand(setting: Setting): SettingCommand {
            if (this.isMultiple) {
                var query: MultipleSettingQuery = new MultipleSettingQuery(this.params);

                if (!query.validate()) {
                    throw Exceptions.customerIdsNotMatchCustomerNames();
                }

                return new MultipleSettingCommand(
                    query.getCustomerIds(),
                    query.getCustomerNames(),
                    query.uplineLevel,
                    setting);
            } else {
                return new SettingCommand(this.params.uplineLevel, setting);
            }
        }

        private searchForParams(): void {
            var params: any = this.$location.search();

            if (params) {
                this.params = params;
                this.isMultiple = params.isMultiple || false;
                this.lang = params.language;
                this.custNames = params.customerNames;
                this.uplineLvl = parseInt(params.uplineLevel);
            } else {
                this.params = undefined;
            }
        }
    }

    ModuleRegister.service("parametersService", ParametersService);

    export function NAValue() {
        return (value: string) => {
            return value || "---";
        }
    }

    ModuleRegister.filter("naValue", NAValue);

    export class ValueRangeGenerator {
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

    ModuleRegister.service("valueRangeGenerator", ValueRangeGenerator);
}