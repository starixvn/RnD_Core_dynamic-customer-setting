/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType8GroupsDetailsScope extends ng.IScope {
        hostItem: SettingType8Item;
        cancel: Function;
    }

    class SettingType8GroupsDetailsDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype8-groupsdetails-modal.htm";

        public get scope(): any {
            return {
                hostItem: "="
            };
        }

        public link: ng.IDirectiveLinkFn = (scope: ISettingType8GroupsDetailsScope) => {
            scope.cancel = () => {
                scope.hostItem.showingDetails = false;
            }
        };
    }

    ModuleRegister.directive("settingtype8GroupsDetails", SettingType8GroupsDetailsDirective);
}