'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', '$timeout', 'locationService', 'dataService', 'resourceService',
                        'templateService', 'linkingService', 'appSettings', 'messageService'];
    var customerSettingController = function ($scope, $rootScope, $timeout, locationService, dataService, resourceService,
                        templateService, linkingService, appSettings, messageService) {
        var selectModel = locationService.createSelectModel();
        function init() {
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
        }

        init();
        
        var selectSettingType;

        $scope.updateStatuses = function () {
            selectSettingType = appSettings.settingType.Status;
            update(appSettings.settingType.Status);
        };

        $scope.updateDiscounts = function () {
            selectSettingType = appSettings.settingType.Discount;
            update(appSettings.settingType.Discount);
        };

        $scope.updateBetLimits = function () {
            selectSettingType = appSettings.settingType.BetLimit;
            update(appSettings.settingType.BetLimit);
        };

        $scope.updatePositionTakings = function () {
            selectSettingType = appSettings.settingType.PositionTaking;
            update(appSettings.settingType.PositionTaking);
        };

        $scope.updateAll = function () {
            selectSettingType = appSettings.settingType.All;
            update(appSettings.settingType.All);
        };

        $rootScope.$on('update-member-position-taking', function (event, updateMemberPT) {
            update(selectSettingType, updateMemberPT);
        });

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

        function hasUncheckAutoPT(data) {
            var isAutoPTName, ptName;
            switch ($scope.formType) {
                case appSettings.formType.SuperEditMaster:
                    isAutoPTName = 'IS3';
                    ptName = 'S3';
                    break;

                case appSettings.formType.MasterEditAgent:
                    isAutoPTName = 'IM2';
                    ptName = 'M2';
                    break;
            }

            if (angular.isDefined(isAutoPTName) && angular.isDefined(ptName)) {
                var memberPTs = [];
                angular.forEach(data, function (pt) {
                    var isAuto = pt[isAutoPTName];
                    if (isAuto === false) {
                        var ptValue = pt[ptName];
                        var name = resourceService.getDisplayName($scope.setting.PositionTakingLevel, $scope.settingContext, pt);
                        memberPTs.push({
                            SportType: pt.SportType,
                            BetType: pt.BetType,
                            IsLive: pt.IsLive,
                            Name: name,
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

        function update(settingType, updateMemberPT) {
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

                    if (angular.isUndefined(updateMemberPT) && hasUpdatePT && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }

                    break;

                case appSettings.settingType.PositionTaking:
                    positionTakings = linkingService.positionTakings();
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUncheckAutoPT(positionTakings)) {
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

            if (angular.isDefined(updateMemberPT)) {
                var isUpdateAllMembers = updateMemberPT.IsUpdateAllMembers;
                var memberPTs = updateMemberPT.MemberPTs;
                var newPositionTakings = [];
                angular.forEach(positionTakings, function (positionTaking) {
                    var isFound = false;
                    var newPositionTaking = positionTaking;
                    angular.forEach(memberPTs, function (memberPt) {
                        if (!isFound
                            && newPositionTaking.SportType === memberPt.SportType
                            && newPositionTaking.BetType === memberPt.BetType
                            && newPositionTaking.IsLive === memberPt.IsLive) {
                            newPositionTaking = angular.extend({
                                IsUpdateAllMembers: isUpdateAllMembers,
                                MemberPositionTaking: memberPt.Pt
                            }, newPositionTaking);
                            newPositionTakings.push(newPositionTaking);
                            isFound = true;
                        }
                    });
                });

                positionTakings = newPositionTakings;
            }

            // delete un-used properties
            angular.forEach(positionTakings, function (positionTaking) {
                delete (positionTaking.SportTypeName);
                delete (positionTaking.BetTypeName);
            });

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