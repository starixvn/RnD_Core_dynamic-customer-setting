/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="constants.ts" />
/// <reference path="models.ts" />
/// <reference path="service.ts" />

module CustomerSetting {
    export class CustomerSettingController {
        constructor(
            private $rootScope: ng.IRootScopeService) { }

        public setting: Setting;

        public customerInfo: CustomerInfo;
    }
}