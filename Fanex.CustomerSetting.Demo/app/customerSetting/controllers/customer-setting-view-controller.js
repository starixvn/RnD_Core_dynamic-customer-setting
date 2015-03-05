'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', 'locationService', 'templateService', 'errorService'];
    var customerSettingViewController = function ($scope, $rootScope, locationService, templateService, errorService) {
        if (!locationService.isValid) {
            errorService.show('Please check your URL');
        } else {
            $scope.view = templateService.resolveView(locationService.productId);
        }
    };

    customerSettingViewController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingViewController', customerSettingViewController);
} ());