/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="common.ts" />
/// <reference path="module-registrater.ts" />
/// <reference path="parameters.ts" />

module CustomerSetting {
    "use strict";

    export class AppConfigService {
        public static $inject: string[] = ["$window"];

        private applicationPath: string;
        private antiForgeryToken: string;
        private editable: boolean;

        private $window: ng.IWindowService;

        constructor($window: ng.IWindowService) {
            var appConfig: any;

            this.$window = $window;

            /* tslint:disable:no-string-literal */
            appConfig = this.$window["appConfig"] || {};
            /* tslint:enable:no-string-literal */

            if (appConfig) {
                this.applicationPath = appConfig.applicationPath;
                this.antiForgeryToken = appConfig.antiForgeryToken;
                this.editable = appConfig.editable;

                if (this.applicationPath && this.applicationPath[this.applicationPath.length - 1] !== "/") {
                    this.applicationPath += "/";
                }
            }
        }

        public getQueryPath(): string {
            return this.applicationPath + "viewCustomerSetting/querySetting?";
        }

        public getUpdatePath(): string {
            return this.applicationPath + "updateCustomerSetting/updateSetting";
        }

        public getUpdateMultiplePath(): string {
            return this.applicationPath + "updateCustomerSetting/updateMultipleSetting";
        }

        public getSettingTypeActionPath(): string {
            return this.applicationPath + "route/redirect";
        }

        public requestVerificationToken(): string {
            return this.antiForgeryToken;
        }

        public canUpdate(): boolean {
            return this.editable;
        }
    }

    ModuleRegister.service("appConfigService", AppConfigService);

    export class ParametersService {
        public static $inject: string[] = ["$window"];

        public get systemId(): number {
            return this.system;
        }

        public get language(): string {
            return this.lang;
        }

        public get customerNames(): string {
            return (<MultipleSettingQuery>this.query).customerNames;
        }

        public get uplineLevel(): Level {
            return this.query.uplineLevel;
        }

        public get forcingMode(): boolean {
            return this.isForcingMode;
        }

        private query: SettingQuery | MultipleSettingQuery;
        private lang: string;
        private isMultipleUpdates: boolean;
        private system: number;
        private isForcingMode: boolean;

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
                    multipleQuery.customerIds,
                    multipleQuery.customerNames,
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
                this.system = params.system || Defaults.system;
                this.isForcingMode = params.isForcingMode || false;

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
            var isNew: boolean = this.promises.filter((promise: KeyValuePair<number, IUpdatingSettingTypePromise>) => {
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

                this.promises.forEach((promise: KeyValuePair<number, IUpdatingSettingTypePromise>) => {
                    var matchedSettingTypes: SettingType[] = settingCommand.setting.settingTypes.filter((settingType: SettingType) => {
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

    export class SettingTypesSharedValuesService {
        public static $inject: string[] = ["parametersService"];

        public customerId: number
        public uplineLevel: Level;
        public customerLevel: Level;
        public productId: number;
        public productName: string;
        public normalMode: boolean;

        private parametersService: ParametersService;

        constructor(parametersService: ParametersService) {
            this.parametersService = parametersService;
        }

        public register(setting: Setting, uplineLevel: Level): void {
            this.uplineLevel = uplineLevel;

            if (setting) {
                this.customerId = setting.customerId;
                this.customerLevel = setting.customerLevel;
                this.productId = setting.productId;
                this.productName = setting.productName;
                this.normalMode = setting.isNormalMode;

                if (this.parametersService.forcingMode) {
                    this.normalMode = false;
                }
            }
        }
    }

    ModuleRegister.service("settingTypesSharedValuesService", SettingTypesSharedValuesService);

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

    function FormatNumber($filter: ng.IFilterService): Function {
        var numberFilter = $filter("number");

        return (value: number, factor?: number) => {
            factor = factor || value.toString().indexOf(".") > 0 ? 2 : 0;

            return numberFilter(value, factor);
        };
    }

    FormatNumber.$inject = ["$filter"];
    ModuleRegister.filter("formatNumber", FormatNumber);

    function NAValue(): Function {
        return (value: string) => {
            return value || "---";
        };
    }

    ModuleRegister.filter("naValue", NAValue);

    function MinValue(): Function {
        return (numbers: number[]) => {
            return Math.min.apply(Math, numbers);
        };
    }

    ModuleRegister.filter("minValue", MinValue);

    function MaxValue(): Function {
        return (numbers: number[]) => {
            return Math.max.apply(Math, numbers);
        };
    }

    ModuleRegister.filter("maxValue", MaxValue);

    function EditableSettingTypeFilter(appConfigService: AppConfigService): Function {
        return (item: { displayMode: number }) => {
            return appConfigService.canUpdate()
                && item
                && item.displayMode === DisplayMode.editable;
        };
    }

    EditableSettingTypeFilter.$inject = ["appConfigService"];
    ModuleRegister.filter("editableSettingType", EditableSettingTypeFilter);

    function EditableFormFilter(appConfigService: AppConfigService): Function {
        return (setting: { settingTypes: Array<{ displayMode: number }> }) => {
            var hasEditableSettingType: boolean = false;

            if (setting && setting.settingTypes) {
                hasEditableSettingType = setting.settingTypes.some((item: { displayMode: number }) => {
                    return item.displayMode === DisplayMode.editable;
                });
            }

            return appConfigService.canUpdate() && hasEditableSettingType;
        };
    }

    EditableFormFilter.$inject = ["appConfigService"];
    ModuleRegister.filter("editableForm", EditableFormFilter);

    function ValidSettingTypeFilter(): Function {
        return (settingType: { isError: boolean }) => {
            return !settingType.isError;
        };
    }

    ModuleRegister.filter("validSettingType", ValidSettingTypeFilter);

    function Unsafe($sce: ng.ISCEService): Function {
        return (value: string) => {
            return $sce.trustAsHtml(value);
        };
    }

    Unsafe.$inject = ["$sce"];
    ModuleRegister.filter("unsafe", Unsafe);
}