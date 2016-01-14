/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ISettingResultScope extends ng.IScope {
        result: UpdatingSettingResult;
    }

    class SettingResultDirective implements ng.IDirective {
        public replace: boolean = true;

        public scope: any = {
            result: "="
        };

        public templateUrl: string = "SettingResult/setting-result.htm";

        public controller: any = ["$scope", "$window", ($scope: ISettingResultScope, $window: ng.IWindowService) => {
            $scope.$watch("result", (result: UpdatingSettingResult) => {
                if (angular.isDefined(result)) {
                    $window.scrollTo(0, 0);
                }
            });
        }];
    }

    ModuleRegister.directive("settingResult", SettingResultDirective);
}