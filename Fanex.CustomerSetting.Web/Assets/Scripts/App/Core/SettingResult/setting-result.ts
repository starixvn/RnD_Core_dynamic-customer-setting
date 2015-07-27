/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../core/module-registrater.ts" />

module CustomerSetting {
    class SettingResultDirective implements ng.IDirective {
        public static $inject = ["$window"];

        public replace: boolean = true;

        public get scope(): any {
            return {
                result: "="
            }
        }

        public templateUrl: string = "SettingResult/setting-result.htm";

        public controller: Function = ($scope: ng.IScope) => {
            $scope.$watch("result", (result) => {
                if (angular.isDefined(result)) {
                    this.$window.scrollTo(0, 0);
                }
            });
        }

        constructor(private $window: ng.IWindowService) { }
    }

    ModuleRegister.directive("settingResult", SettingResultDirective);
}