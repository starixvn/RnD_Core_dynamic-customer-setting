'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', 'locationService', 'dataService', 'resourceService',
                        'templateService', 'linkingService', 'appSettings', 'messageService'];
    var customerSettingController = function ($scope, $rootScope, locationService, dataService, resourceService,
                        templateService, linkingService, appSettings, messageService) {
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
            update(appSettings.settingType.Status);
        };

        $scope.updateDiscounts = function () {
            update(appSettings.settingType.Discount);
        };

        $scope.updateBetLimits = function () {
            update(appSettings.settingType.BetLimit);
        };

        $scope.updatePositionTakings = function () {
            update(appSettings.settingType.PositionTaking);
        };

        $scope.updateAll = function () {
            update(appSettings.settingType.All);
        };

        function hasUpdate(data, isShowNoChange) {
            if (angular.isUndefined(data)) {
                return false; // form invalid
            }

            if (data.length === 0) {
                if (isShowNoChange) {
                    messageService.showError(resourceService.getString('NoChangeToUpdate'));
                }
                return false;
            }

            return true;
        }

        function update(settingType) {
            messageService.hide();
            var positionTakings;
            var betLimits;
            var discounts;
            var statuses;
            switch (settingType) {
                case appSettings.settingType.All:
                    statuses = linkingService.statuses();
                    discounts = linkingService.discounts();
                    betLimits = linkingService.betLimits();
                    positionTakings = linkingService.positionTakings();
                    if (!hasUpdate(statuses, false) && !hasUpdate(discounts, false) && !hasUpdate(betLimits, false) && !hasUpdate(positionTakings, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.PositionTaking:
                    positionTakings = linkingService.positionTakings();
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.BetLimit:
                    betLimits = linkingService.betLimits();
                    if (!hasUpdate(betLimits, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Discount:
                    discounts = linkingService.discounts();
                    if (!hasUpdate(discounts, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Status:
                    statuses = linkingService.statuses();
                    if (!hasUpdate(statuses, true)) {
                        return;
                    }
                    break;
            }

            var updateModel = angular.extend({
                SettingType: settingType,
                StatusString: JSON.stringify(statuses),
                DiscountString: JSON.stringify(discounts),
                BetLimitString: JSON.stringify(betLimits),
                PositionTakingString: JSON.stringify(positionTakings)
            }, selectModel);
            dataService.update(updateModel, function (response) {
                console.log(response);
            });
        }
    };

    customerSettingController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingController', customerSettingController);
} ());