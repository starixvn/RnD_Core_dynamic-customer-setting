'use strict';

(function () {
    var injectParams = ['$scope', 'templateService', 'controllerService', 'appSettings'];
    var customerSettingController = function ($scope, templateService, controllerService, appSettings) {
        controllerService.select(function () {
            $scope.information = controllerService.information;
            $scope.statuses = controllerService.statuses;
            $scope.statusTemplate = templateService.resolveStatus();
            $scope.discounts = controllerService.discounts;
            $scope.discountTemplate = templateService.resolveDiscount();
            $scope.betLimits = controllerService.betLimits;
            $scope.betLimitTemplate = templateService.resolveBetLimit();
            $scope.positionTakings = controllerService.positionTakings;
            $scope.positionTakingTemplate = templateService.resolvePositionTaking();
            if (controllerService.formType === appSettings.formType.MasterEditAgent) {
                $scope.disableAutoMPT = controllerService.settingContext.DisableAutoMPT;
            }
        });

        $scope.updateStatuses = function () {
            controllerService.update(appSettings.settingType.Status);
        };

        $scope.updateDiscounts = function () {
            controllerService.update(appSettings.settingType.Discount);
        };

        $scope.updateBetLimits = function () {
            controllerService.update(appSettings.settingType.BetLimit);
        };

        $scope.updatePositionTakings = function () {
            controllerService.update(appSettings.settingType.PositionTaking);
        };

        $scope.updateAll = function () {
            controllerService.update(appSettings.settingType.All);
        };
    };

    customerSettingController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingController', customerSettingController);
} ());