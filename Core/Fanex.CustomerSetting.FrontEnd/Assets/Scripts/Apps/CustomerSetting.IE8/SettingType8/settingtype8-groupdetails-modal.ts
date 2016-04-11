/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ISettingType8GroupDetailsScope extends ng.IScope {
        hostItem: SettingType8Item;
        cancel: Function;
    }

    class SettingType8GroupDetailsDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype8-groupdetails-modal.htm";

        public scope: any = {
            hostItem: "="
        };

        public link: ng.IDirectiveLinkFn = (scope: ISettingType8GroupDetailsScope) => {
            scope.cancel = () => {
                scope.hostItem.showingDetails = false;
            }
        };
    }

    ModuleRegister.directive("settingtype8GroupDetailsModal", SettingType8GroupDetailsDirective);
}