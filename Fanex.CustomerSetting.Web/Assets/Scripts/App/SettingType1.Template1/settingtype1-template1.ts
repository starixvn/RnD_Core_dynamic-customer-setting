/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/helpers.ts" />

module CustomerSetting {
    class SettingType1Template1Directive implements ng.IDirective {
        //public static $inject = ["$templateCache"];

        public replace: boolean = true;

        public get scope(): any {
            return {
                productId: "=",
                productName: "=",
                settingType: "="
            }
        }

        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype1.template1/settingtype1-template1.htm");

        constructor(private appConfigService: AppConfigService) { }
    }

    angular.module("customerSetting").directive("settingtype1Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType1Template1Directive, args));
    }]);
}