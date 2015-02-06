'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', 'locationService', 'templateService'];
    var customerSettingViewController = function ($scope, $rootScope, locationService, templateService) {
        $rootScope.hasError = !locationService.isValid;
        if ($rootScope.hasError) {
            $rootScope.errorMessage = 'Please check your URL';
        } else {
            $scope.view = templateService.resolveView(locationService.productId);
        }
    };

    customerSettingViewController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingViewController', customerSettingViewController);
} ());