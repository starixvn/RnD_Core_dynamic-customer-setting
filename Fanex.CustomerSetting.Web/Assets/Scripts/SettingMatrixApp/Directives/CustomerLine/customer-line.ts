/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../helper-services.ts" />
/// <reference path="../../models.ts" />
/// <reference path="../../module-register.ts" />

module CustomerSettingMatrix {

    export class CustomerLineDirective implements ng.IDirective {
        public static $inject: string[] = ["appConfigService"];

        public restrict: string;
        public templateUrl: string;
        public scope =
        {
            customerLine: '='
        };

        private appConfigService: AppConfigService;

        constructor(appConfigService: AppConfigService) {

            this.appConfigService = appConfigService;
            this.restrict = "A";
            this.templateUrl = this.appConfigService.resolveRootUrl("app/customerSettingMatrix/directives/customerline/customer-line.htm");
        }
    }

    ModuleRegister.directive("customerLineDirective", CustomerLineDirective);

}