/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helpers.ts" />

module CustomerSetting {
    class SettingType2Template1Directive extends SettingTypeDirective {
        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype2.template1/settingtype2-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingTypeDirectiveSharedScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            scope.headerTemplateUrl = "settingtyp2-template1-header.html";
            scope.valueTemplateUrl = "settingtyp2-template1-value.html";
        }

        constructor(private appConfigService: AppConfigService) {
            super();
        }
    }

    angular.module("customerSetting").directive("settingtype2Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType2Template1Directive, args));
    }]);
}