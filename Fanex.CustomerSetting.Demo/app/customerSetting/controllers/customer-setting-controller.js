'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', 'locationService', 'dataService', 'resourceService', 'templateService'];
    var customerSettingController = function ($scope, $rootScope, locationService, dataService, resourceService, templateService) {
        var selectModel = locationService.createSelectModel();
        dataService.select(selectModel, function (response) {
            var settingContext = response.SettingContext;
            resourceService.setLanguage(settingContext.Language);
            var customerSetting = response.CustomerSetting;
            $scope.formType = 1;
            $scope.settingContext = settingContext;
            $scope.setting = customerSetting.Setting;
            $scope.information = customerSetting.Information;
            $scope.statuses = customerSetting.Statuses;
            $scope.statusTemplate = templateService.resolveStatus(
                $scope.settingContext.ProductId,
                $scope.formType,
                $scope.setting.StatusLevel);
            $scope.discounts = customerSetting.Discounts;
            $scope.discountTemplate = templateService.resolveDiscount(
                $scope.settingContext.ProductId,
                $scope.formType,
                $scope.setting.DiscountLevel);
            $scope.betLimits = customerSetting.BetLimits;
            $scope.betLimitTemplate = templateService.resolveBetLimit(
                $scope.settingContext.ProductId,
                $scope.formType,
                $scope.setting.BetLimitLevel);
            $scope.positionTakings = customerSetting.PositionTakings;
            $scope.positionTakingTemplate = templateService.resolvePositionTaking(
                $scope.settingContext.ProductId,
                $scope.formType,
                $scope.setting.PositionTakingLevel);
        });

        $scope.updateStatuses = function () {
            console.log($scope.statuses);
        };

        $scope.updateDiscounts = function () {
            console.log($scope.discounts);
        };

        $scope.updateBetLimits = function () {
            console.log($scope.betLimits);
        };

        $scope.updatePositionTakings = function () {
            console.log($scope.positionTakings);
        };

        $scope.updateAll = function () {
            console.log('updateAll');
        };
    };

    customerSettingController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingController', customerSettingController);
} ());