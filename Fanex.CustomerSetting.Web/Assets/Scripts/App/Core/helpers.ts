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
        private multiple: boolean;

        public static $inject: string[] = ["$location"];

        public get isMultiple(): boolean {
            return this.multiple;
        }

        constructor(private $location: ng.ILocationService) {
            this.searchForParams();
        }

        public getSettingQuery(): SettingQuery {
            if (this.isMultiple) {
                var multipleQuery = <MultipleSettingQuery>this.params;

                if (!multipleQuery.validate()) {
                    throw Exceptions.customerIdsNotMatchCustomerNames();
                }

                return multipleQuery.getFirstCustomerSettingQuery();
            } else {
                return <SettingQuery>this.params;
            }
        }

        public getMultipleSetting(): MultipleSettingQuery {
            if (this.isMultiple) {
                var query = <MultipleSettingQuery>this.params;

                if (!query.validate()) {
                    throw Exceptions.customerIdsNotMatchCustomerNames();
                }

                return query;
            }

            return undefined;
        }

        public createSettingCommand(setting: Setting): SettingCommand {
            if (this.isMultiple) {
                var query = <MultipleSettingQuery>this.params;

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
                this.multiple = params.isMultiple || false;
                this.params = params;
            } else {
                this.params = undefined;
            }
        }
    }

    ModuleRegister.service("parametersService", ParametersService);
}