/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    class SettingType8GroupsDetailsDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype8-groupsdetails-modal.htm";
    }

    ModuleRegister.directive("settingtype8GroupsDetails", SettingType8GroupsDetailsDirective);
}