/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/models.ts" />

module CustomerSetting {
    interface SettingType4Template1Scope extends ng.IScope {
        productId: number;
        productName: string;
        settingType: SettingType;
        update: (settingTypeId: { settingTypeId: number }) => void;
    }

    class SettingType4Template1Directive implements ng.IDirective {
        //public static $inject = ["$templateCache"];

        public replace: boolean = true;

        public get scope(): any {
            return {
                productId: "=",
                productName: "=",
                settingType: "=",
                update: "&"
            }
        }

        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype4.template1/settingtype4-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingType4Template1Scope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
        }

        constructor(private appConfigService: AppConfigService) { }
    }

    angular.module("customerSetting").directive("settingtype4Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType4Template1Directive, args));
    }]);
}