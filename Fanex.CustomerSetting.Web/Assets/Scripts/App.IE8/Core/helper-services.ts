/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="../../typings/underscore/underscore.d.ts" />
/// <reference path="module-registrater.ts" />
/// <reference path="parameters.ts" />

module CustomerSetting.IE8 {
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

        private query: SettingQuery | MultipleSettingQuery;
        private lang: string;
        private isMultipleUpdates: boolean;

        private $window: ng.IWindowService;

        constructor($window: ng.IWindowService) {
            this.$window = $window;
            this.fetchParams();
        }

        public getLanguage(): string {
            return this.lang;
        }

        public getCustomerNames(): string {
            if (this.isMultipleUpdates) {
                return (<MultipleSettingQuery>this.query).getCustomerNames();
            } else {
                return undefined;
            }
        }

        public getUplineLevel(): Level {
            return this.query.getUplineLevel();
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
                    multipleQuery.getUplineLevel(),
                    setting);
            } else {
                return new SettingCommand(this.query.getUplineLevel(), setting);
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

    export interface IUpdatingSettingTypePromise {
        (settingPartialCommand: SettingPartialCommand): ng.IPromise<void>;
    }

    export class UpdatingSettingTypePromiseChain {
        public static $inject: string[] = ["$q"];

        private promises: Array<KeyValuePair<number, IUpdatingSettingTypePromise>>;

        private $q: ng.IQService;

        constructor($q: ng.IQService) {
            this.$q = $q;
            this.promises = new Array<KeyValuePair<number, IUpdatingSettingTypePromise>>();
        }

        public reset(): void {
            this.promises = new Array<KeyValuePair<number, IUpdatingSettingTypePromise>>();
        }

        public register(settingTypeId: number, promise: IUpdatingSettingTypePromise): void {
            var isNew: boolean = _.filter(this.promises, (promise: KeyValuePair<number, IUpdatingSettingTypePromise>) => {
                return settingTypeId === promise.key;
            }).length <= 0;

            if (isNew) {
                this.promises.push(new KeyValuePair<number, IUpdatingSettingTypePromise>(settingTypeId, promise));
            }
        }

        public execute(settingCommand: SettingCommand): ng.IPromise<void> {
            var deferred: ng.IDeferred<void> = this.$q.defer<void>();

            if (settingCommand) {
                var current: ng.IPromise<void>;

                _.forEach(this.promises, (promise: KeyValuePair<number, IUpdatingSettingTypePromise>) => {
                    var matchedSettingTypes: SettingType[] = _.filter(settingCommand.setting.settingTypes, (settingType: SettingType) => {
                        return promise.key === settingType.id;
                    });

                    if (matchedSettingTypes.length > 0) {
                        var settingPartialCommand = new SettingPartialCommand(
                            settingCommand.uplineLevel,
                            settingCommand.setting.customerLevel,
                            settingCommand.setting.productId,
                            settingCommand.setting.productName,
                            matchedSettingTypes[0]);

                        if (!current) {
                            current = promise.value(settingPartialCommand);
                        } else {
                            current = current.then<void>(() => promise.value(settingPartialCommand));
                        }
                    }
                });

                if (current) {
                    return current;
                }
            }

            deferred.resolve();

            return deferred.promise;
        }
    }

    ModuleRegister.service("updatingSettingTypePromiseChain", UpdatingSettingTypePromiseChain);

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