/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    interface ISettingResultScope extends ng.IScope {
        result: Results;
    }

    class SettingResultDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                result: "="
            };
        }

        public templateUrl: string = "SettingResult/setting-result.htm";

        public controller: any = ["$scope", "$window", ($scope: ISettingResultScope, $window: ng.IWindowService) => {
            $scope.$watch("result", (result: UpdatingSettingResult) => {
                if (angular.isDefined(result)) {
                    $window.scrollTo(0, 0);

                    if (angular.isDefined(result.subResults) && angular.isArray(result.subResults)) {
                        angular.forEach(result.subResults, (subResult: CustomerSettingResult) => {
                            if (subResult.settingResults && angular.isArray(subResult.settingResults)) {
                                angular.forEach(subResult.settingResults, (settingResult: SettingResult) => {
                                    settingResult.msgType = MessageType[settingResult.msgType.toString()];
                                });
                            }
                        });
                    } else {
                        result.msgType = MessageType[result.msgType.toString()];
                    }
                }
            });
        }];
    }

    ModuleRegister.directive("settingResult", SettingResultDirective);
}