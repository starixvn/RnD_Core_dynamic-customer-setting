'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', '$timeout', 'modelService', 'templateService', 'messageService'];
    var customerSettingViewController = function ($scope, $rootScope, $timeout, modelService, templateService, messageService) {
        $rootScope.$on('$locationChangeStart', function () {
            $scope.view = undefined;
            messageService.hide();
        });

        $rootScope.$on('$locationChangeSuccess', function () {
            if (!modelService.isValid) {
                messageService.showError('Please check your URL');
            } else {
                $timeout(function () {
                    var modelParams = modelService.params;
                    $scope.view = templateService.resolveView(modelParams.productId);
                });
            }
        });
    };

    customerSettingViewController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingViewController', customerSettingViewController);
} ());