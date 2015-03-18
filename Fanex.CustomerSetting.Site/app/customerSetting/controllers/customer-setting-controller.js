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

        function getPTDisplayName(sportType, betType, isLive) {
            var name;
            angular.forEach($scope.positionTakings, function (pt) {
                if (angular.isUndefined(name)) {
                    if (pt.SportType === sportType && pt.BetType === betType && pt.IsLive === isLive) {
                        name = resourceService.getDisplayName($scope.setting.PositionTakingLevel, $scope.settingContext, pt);
                    }
                }
            });

            return name;
        }

        function hasUncheckAutoPT(data) {
            var isAutoPTName, ptName;
            switch ($scope.formType) {
                case appSettings.formType.SuperEditMaster:
                    isAutoPTName = 'IS3';
                    ptName = 'S3';
                    break;
            }

            if (angular.isDefined(isAutoPTName) && angular.isDefined(ptName)) {
                var memberPTs = [];
                angular.forEach(data, function (pt) {
                    var isAuto = pt[isAutoPTName];
                    if (isAuto === false) {
                        var ptValue = pt[ptName];
                        memberPTs.push({
                            SportType: pt.SportType,
                            BetType: pt.BetType,
                            IsLive: pt.IsLive,
                            Name: getPTDisplayName(pt.SportType, pt.BetType, pt.IsLive),
                            Pt: ptValue
                        });
                    }
                });

                if (memberPTs.length > 0) {
                    $rootScope.updateModal = {
                        MemberPTs: memberPTs
                    };
                    return true;
                }
            }

            return false;
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
                    var hasUpdatePT = hasUpdate(positionTakings, true);
                    if (!hasUpdate(statuses, false) && !hasUpdate(discounts, false) && !hasUpdate(betLimits, false) && !hasUpdatePT) {
                        return;
                    }

                    if (hasUpdatePT && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }

                    break;

                case appSettings.settingType.PositionTaking:
                    positionTakings = linkingService.positionTakings();
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }

                    if (hasUncheckAutoPT(positionTakings)) {
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
                UserName: $scope.information.UserName,
                SettingType: settingType,
                StatusLevel: $scope.setting.StatusLevel,
                StatusString: JSON.stringify(statuses),
                DiscountLevel: $scope.setting.DiscountLevel,
                DiscountString: JSON.stringify(discounts),
                BetLimitLevel: $scope.setting.BetLimitLevel,
                BetLimitString: JSON.stringify(betLimits),
                PositionTakingLevel: $scope.setting.PositionTakingLevel,
                PositionTakingString: JSON.stringify(positionTakings)
            }, selectModel);
            dataService.update(updateModel);
        }
    };

    customerSettingController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingController', customerSettingController);
} ());