/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    class CustomerInfoDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                info: "=",
                names: "="
            };
        }

        public templateUrl: string = "CustomerInfo/customer-info.htm";
        public controller: any = ["$scope", ($scope: IDefaultSettingTypeDirectiveScope) => {
            $scope.togglableSharedStates = new TogglableSettingBlockSharedStates(1);
        }];
    }

    ModuleRegister.directive("customerInfo", CustomerInfoDirective);
}