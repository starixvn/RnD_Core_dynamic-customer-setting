/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingType22MemberWinLossAmountScope extends ng.IScope {
        hostItem: SettingType22Item;
        cancel: Function;
    }

    class SettingType22MemberWinLossAmountDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype22-memberwinlossamount-modal.htm";

        public get scope(): any {
            return {
                hostItem: "="
            };
        }

        public link: ng.IDirectiveLinkFn = (scope: ISettingType22MemberWinLossAmountScope) => {
            scope.cancel = () => {
                scope.hostItem.showWinLossAmount = false;
            }
        };
    }

    ModuleRegister.directive("settingtype22MemberWinLossAmountModal", SettingType22MemberWinLossAmountDirective);
}