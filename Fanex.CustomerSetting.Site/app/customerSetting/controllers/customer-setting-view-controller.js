'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', 'locationService', 'templateService', 'messageService'];
    var customerSettingViewController = function ($scope, $rootScope, locationService, templateService, messageService) {
        if (!locationService.isValid) {
            messageService.showError('Please check your URL');
        } else {
            $scope.view = templateService.resolveView(locationService.productId);
        }
    };

    customerSettingViewController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingViewController', customerSettingViewController);
} ());