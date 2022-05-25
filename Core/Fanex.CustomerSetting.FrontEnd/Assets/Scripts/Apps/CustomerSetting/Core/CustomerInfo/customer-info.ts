/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    "use strict";
    export interface ICustomerInfoScope extends ng.IScope {
        setting: Setting;
        names: string;
        togglableSharedStates: TogglableSettingBlockSharedStates;
    }

    export class CustomerInfoDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                setting: "=",
                names: "=",
            };
        }

        public templateUrl: string = "CustomerInfo/customer-info.htm";
        public controller: any = ["$scope", ($scope: ICustomerInfoScope) => {
            $scope.$watch("setting", (setting: Setting) => {
                if (setting && setting.customerInfo && setting.customerInfo.isEnableCollapseExpand) {
                    var togglableSharedStates = new TogglableSettingBlockSharedStates(setting.settingTypes.length, setting.customerInfo.isCollapsed);
                    for (var i = 0; i < setting.settingTypes.length; i++) {
                        if (setting.settingTypes[i].isCollapsed) {
                            togglableSharedStates.collapseBlock(i);
                        }
                        else {
                            togglableSharedStates.expandBlock(i);
                        }
                    }

                    $scope.togglableSharedStates = togglableSharedStates;
                }
            });
        }];
    }

    ModuleRegister.directive("customerInfo", CustomerInfoDirective);
}