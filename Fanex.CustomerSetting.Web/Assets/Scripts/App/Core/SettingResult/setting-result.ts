/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    interface SettingResultScope extends ng.IScope {
        result: Results;
    }

    class SettingResultDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                result: "="
            }
        }

        public templateUrl: string = "SettingResult/setting-result.htm";

        public controller: any = ["$scope", "$window", ($scope: SettingResultScope, $window: ng.IWindowService) => {
            $scope.$watch("result", (result: Results) => {
                if (angular.isDefined(result)) {
                    $window.scrollTo(0, 0);

                    if (angular.isDefined(result.subResults) && angular.isArray(result.subResults)) {
                        angular.forEach(result.subResults, subResult => {
                            subResult.msgType = MessageType[subResult.msgType.toString()];
                        });
                    } else {
                        result.msgType = MessageType[result.msgType.toString()];
                    }
                }
            })
        }];
    }

    ModuleRegister.directive("settingResult", SettingResultDirective);
}