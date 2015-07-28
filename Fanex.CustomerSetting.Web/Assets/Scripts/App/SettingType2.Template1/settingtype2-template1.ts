/// <reference path="../core/module-registrater.ts" />
/// <reference path="../../../typings/angularjs/angular.d.ts" />

module CustomerSetting {
    class SettingType2Template1Directive implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                productId: "=",
                productName: "=",
                settingType: "="
            }
        }

        public templateUrl: string = "settingtype2-template1.htm";
    }

    ModuleRegister.directive("settingtype2-template1", SettingType2Template1Directive);
}