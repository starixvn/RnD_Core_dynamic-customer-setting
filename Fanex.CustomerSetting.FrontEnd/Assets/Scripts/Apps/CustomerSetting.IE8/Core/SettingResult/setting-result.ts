/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/underscore/underscore.d.ts" />
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

                    if (angular.isDefined(result.subResults) && angular.isArray(result.subResults)) {
                        _.forEach(result.subResults, (subResult: CustomerSettingResult) => {
                            if (subResult.settingResults && angular.isArray(subResult.settingResults)) {
                                _.forEach(subResult.settingResults, (settingResult: SettingResult) => {
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