/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />
/// <reference path="../../resource/resource-translation.ts" />

module CustomerSetting {
    class CustomerInfoDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                info: "="
            }
        }

        public templateUrl: string = "CustomerInfo/customer-info.htm";
    }

    ModuleRegister.directive("customerInfo", CustomerInfoDirective);
}