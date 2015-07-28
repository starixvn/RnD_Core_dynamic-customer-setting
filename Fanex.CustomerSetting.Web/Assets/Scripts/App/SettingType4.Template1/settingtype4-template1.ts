/// <reference path="../core/module-registrater.ts" />
/// <reference path="../../../typings/angularjs/angular.d.ts" />

module CustomerSetting {
    class SettingType4Template1Directive implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                productId: "=",
                productName: "=",
                settingType: "="
            }
        }

        public templateUrl: string = "settingtype4-template1.htm";
    }

    ModuleRegister.directive("settingtype4-template1", SettingType4Template1Directive);
}