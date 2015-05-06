'use strict';

(function () {
    var injectParams = [];
    var fanexCustomerInformationDirective = function () {
        return {
            restrict: 'AE',
            scope: {
                information: '=fanexCustomerInformation'
            },
            templateUrl: 'templates/fanex-customer-information.htm',
            controller: ['$scope', function ($scope) {
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

    fanexCustomerInformationDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexCustomerInformation', fanexCustomerInformationDirective);
})();