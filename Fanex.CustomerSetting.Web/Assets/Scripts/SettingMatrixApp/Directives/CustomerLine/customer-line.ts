/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../helper-services.ts" />
/// <reference path="../../models.ts" />
/// <reference path="../../module-register.ts" />

module CustomerSettingMatrix {
    "use strict";

    export class CustomerLineDirective implements ng.IDirective {
        public restrict: string;
        public templateUrl: string;
        public scope =
        {
            customerLine: "=",
            customerName: "=",
            querySettingMatrix: "&"
        };

        constructor() {
            this.restrict = "A";
            this.templateUrl = "Directives/CustomerLine/customer-line.htm";
        }
    }

    ModuleRegister.directive("customerLineDirective", CustomerLineDirective);
}