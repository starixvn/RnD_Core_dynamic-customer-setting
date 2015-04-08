'use strict';

(function () {
    var injectParams = ['resourceService'];
    var customerSettingInformationDirective = function (resourceService) {
        return {
            restrict: 'AE',
            scope: {
                information: '=customerSettingInformation'
            },
            templateUrl: 'templates/customer-setting-information.htm',
            controller: ['$scope', function ($scope) {
                $scope.resourceService = resourceService;
                $scope.$watch('information', function () {
                    if (angular.isUndefined($scope.information)) {
                        return;
                    }

                    $scope.userName = $scope.information.UserName;
                    $scope.nickName = $scope.information.UserName2;
                    $scope.firstName = $scope.information.FirstName;
                    $scope.lastName = $scope.information.LastName;
                    $scope.phone = $scope.information.Phone;
                    $scope.mobilePhone = $scope.information.MobilePhone;
                    $scope.fax = $scope.information.Fax;
                });
            } ]
        };
    };

    customerSettingInformationDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingInformation', customerSettingInformationDirective);
})();