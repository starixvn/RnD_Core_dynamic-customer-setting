'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', 'locationService', 'dataService', 'resourceService', 'templateService', 'linkingService', 'componentService'];
    var customerSettingController = function ($scope, $rootScope, locationService, dataService, resourceService, templateService, linkingService, componentService) {
        var selectModel = locationService.createSelectModel();
        dataService.select(selectModel, function (response) {
            var settingContext = response.SettingContext;
            resourceService.setLanguage(settingContext.Language);
            var customerSetting = response.CustomerSetting;
            $scope.formType = selectModel.FormType;
            $scope.settingContext = settingContext;
            $scope.setting = customerSetting.Setting;
            $scope.information = customerSetting.Information;

            var productId = $scope.settingContext.ProductId;
            var formType = $scope.formType;
            $scope.statuses = customerSetting.Statuses;
            $scope.statusTemplate = templateService.resolveStatus(productId, formType, $scope.setting.StatusLevel);
            $scope.discounts = customerSetting.Discounts;
            $scope.discountTemplate = templateService.resolveDiscount(productId, formType, $scope.setting.DiscountLevel);
            $scope.betLimits = customerSetting.BetLimits;
            $scope.betLimitTemplate = templateService.resolveBetLimit(productId, formType, $scope.setting.BetLimitLevel);
            $scope.positionTakings = customerSetting.PositionTakings;
            $scope.positionTakingTemplate = templateService.resolvePositionTaking(productId, formType, $scope.setting.PositionTakingLevel);
        });

        $scope.updateStatuses = function () {
            var statuses = linkingService.statuses();
            if (angular.isUndefined(statuses)) {
                return; // form invalid
            }

            if (statuses.length === 0) {
                $rootScope.hasError = true;
                $rootScope.errorMessage = 'No change';
                componentService.scrollToTop();
                return;
            }

            console.log(statuses);
        };

        $scope.updateDiscounts = function () {
            var discounts = linkingService.discounts();
            console.log(discounts);
        };

        $scope.updateBetLimits = function () {
            var betLimits = linkingService.betLimits();
            console.log(betLimits);
        };

        $scope.updatePositionTakings = function () {
            var positionTakings = linkingService.positionTakings();
            console.log(positionTakings);
        };

        $scope.updateAll = function () {
            console.log('updateAll');
        };
    };

    customerSettingController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingController', customerSettingController);
} ());