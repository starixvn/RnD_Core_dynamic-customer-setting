/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/helpers.ts" />

module CustomerSetting {
    class SettingType2Template1Directive implements ng.IDirective {
        //public static $inject = ["$templateCache"];

        public replace: boolean = true;

        public get scope(): any {
            return {
                productId: "=",
                productName: "=",
                settingType: "="
            }
        }

        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype2.template1/settingtype2-template1.htm");

        constructor(private appConfigService: AppConfigService) { }
    }

    angular.module("customerSetting").directive("settingtype2Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType2Template1Directive, args));
    }]);
}