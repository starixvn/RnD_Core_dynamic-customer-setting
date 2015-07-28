/// <reference path="../core/module-registrater.ts" />
/// <reference path="../../../typings/angularjs/angular.d.ts" />

module CustomerSetting {
    class SettingType3Template1Directive implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                productId: "=",
                productName: "=",
                settingType: "="
            }
        }

        public templateUrl: string = "settingtype3-template1.htm";
    }

    ModuleRegister.directive("settingtype3-template1", SettingType3Template1Directive);
}