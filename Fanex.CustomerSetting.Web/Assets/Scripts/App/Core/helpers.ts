/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="parameters.ts" />
/// <reference path="../core/module-registrater.ts" />

module CustomerSetting {
    export class AppConfigService {
        public static $inject = ["$rootScope"];

        private rootUrl: string;

        constructor(private $rootScope: ng.IRootScopeService) { }

        public resolveRootUrl(url: string): string {
            if (angular.isUndefined(this.rootUrl)) {
                this.rootUrl = this.$rootScope["rootUrl"];
            }

            return this.rootUrl + url;
        }
    }

    ModuleRegister.service("appConfigService", AppConfigService);

    export class ParametersService {
        private params: AbstractSettingQuery;
        private isMultiple: boolean;
        private lang: string;
        private custNames: string;

        public static $inject: string[] = ["$location"];

        public get language(): string {
            return this.lang;
        }

        public get customerNames(): string {
            return this.custNames;
        }

        constructor(private $location: ng.ILocationService) {
            this.searchForParams();
        }

        public getSettingQuery(): SettingQuery {
            if (this.isMultiple) {
                var multipleQuery = new MultipleSettingQuery(this.params);

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
                var query = new MultipleSettingQuery(this.params);

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

            if (params !== undefined) {
                this.params = params;
                this.isMultiple = params.isMultiple || false;
                this.lang = params.language;
                this.custNames = params.customerNames;
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
}