/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helpers.ts" />

module CustomerSetting {
    class SettingType1Template1Directive extends SettingTypeDirective {
        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype1.template1/settingtype1-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingTypeDirectiveSharedScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            scope.headerTemplateUrl = "settingtyp1-template1-header.html";
            scope.valueTemplateUrl = "settingtyp1-template1-value.html";
        }

        constructor(private appConfigService: AppConfigService) {
            super();
        }
    }

    angular.module("customerSetting").directive("settingtype1Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType1Template1Directive, args));
    }]);
}