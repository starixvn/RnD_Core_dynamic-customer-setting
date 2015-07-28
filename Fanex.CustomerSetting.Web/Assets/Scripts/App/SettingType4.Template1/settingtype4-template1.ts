/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/helpers.ts" />

module CustomerSetting {
    class SettingType4Template1Directive implements ng.IDirective {
        //public static $inject = ["$templateCache"];

        public replace: boolean = true;

        public get scope(): any {
            return {
                productId: "=",
                productName: "=",
                settingType: "="
            }
        }

        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype4.template1/settingtype4-template1.htm");

        constructor(private appConfigService: AppConfigService) { }
    }

    angular.module("customerSetting").directive("settingtype4Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType4Template1Directive, args));
    }]);
}