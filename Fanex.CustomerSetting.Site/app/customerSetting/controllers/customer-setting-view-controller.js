'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', '$timeout', 'locationService', 'templateService', 'messageService'];
    var customerSettingViewController = function ($scope, $rootScope, $timeout, locationService, templateService, messageService) {
        $rootScope.$on('$locationChangeStart', function () {
            $scope.view = undefined;
            messageService.hide();
        });

        $rootScope.$on('$locationChangeSuccess', function () {
            if (!locationService.isValid) {
                messageService.showError('Please check your URL');
            } else {
                $timeout(function () {
                    $scope.view = templateService.resolveView(locationService.productId);
                });
            }
        });
    };

    customerSettingViewController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingViewController', customerSettingViewController);
} ());