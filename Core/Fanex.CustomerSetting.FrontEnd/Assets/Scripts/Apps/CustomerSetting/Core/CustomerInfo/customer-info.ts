/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    "use strict";
    export interface ICustomerInfoScope extends ng.IScope {
        info: CustomerInfo;
        names: string;
        togglableSharedStates: TogglableSettingBlockSharedStates;
    }

    export class CustomerInfoDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                info: "=",
                names: "="
            };
        }

        public templateUrl: string = "CustomerInfo/customer-info.htm";
        public controller: any = ["$scope", ($scope: ICustomerInfoScope) => {
            $scope.$watch("info", (info: CustomerInfo) => {
                if (info && info.isEnableCollapseExpand) {
                    $scope.togglableSharedStates = new TogglableSettingBlockSharedStates(1, info.isCollapsed);
                }
            });
            
        }];
    }

    ModuleRegister.directive("customerInfo", CustomerInfoDirective);
}