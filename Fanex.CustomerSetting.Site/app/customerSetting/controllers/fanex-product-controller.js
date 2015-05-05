'use strict';

(function () {
    var injectParams = ['$rootScope', '$scope', 'fanexAppSettings', 'fanexModelService', 'fanexCommonService',
            'fanexTemplateService', 'fanexDataService', 'fanexControllerService', 'fanexMessageService', 'fanexResourceService', 'fanexResponseReader'];
    var fanexProductController = function ($rootScope, $scope, appSettings, modelService, commonService,
            templateService, dataService, controllerService, messageService, resourceService, ResponseReader) {
        var selectModel = modelService.createSelectModel();
        dataService.select(selectModel, function (response) {
            controllerService.updateResponse(response);
            var requestContext = controllerService.getRequestContext();
            var customerSetting = controllerService.getCustomerSetting();
            var settingContext = controllerService.getSettingContext();
            resourceService.setLanguage(requestContext.Language);

            var multipleInfo = modelService.getMultipleInfo();
            if (multipleInfo.IsMultiple) {
                $scope.custNames = multipleInfo.CustNames.join(',');
            } else {
                $scope.information = customerSetting.Information;
            }

            $scope.formType = selectModel.FormType;

            $scope.statuses = customerSetting.Statuses;
            $scope.statusTemplate = templateService.resolveStatus();

            $scope.discounts = customerSetting.Discounts;
            $scope.discountTemplate = templateService.resolveDiscount();
            $scope.hasUpdateDiscount = $scope.formType !== appSettings.formType.SuperEditMember
                && $scope.formType !== appSettings.formType.MasterEditMember;
            
            $scope.betLimits = customerSetting.BetLimits;
            $scope.betLimitTemplate = templateService.resolveBetLimit();
            $scope.hasUpdateBetLimit = $scope.hasUpdateDiscount;
            
            $scope.positionTakings = customerSetting.PositionTakings;
            $scope.positionTakingTemplate = templateService.resolvePositionTaking();
            if (requestContext.FormType === appSettings.formType.MasterEditAgent) {
                $scope.disableAutoMPT = settingContext.DisableAutoMPT;
            }

            $scope.hasUpdateAll = modelService.isValid();
        });

        var selectedUpdateType;
        $scope.updateStatuses = function () {
            selectedUpdateType = appSettings.settingType.Status;
            update();
        };

        $scope.updateDiscounts = function () {
            selectedUpdateType = appSettings.settingType.Discount;
            update();
        };

        $scope.updateBetLimits = function () {
            selectedUpdateType = appSettings.settingType.BetLimit;
            update();
        };

        $scope.updatePositionTakings = function () {
            selectedUpdateType = appSettings.settingType.PositionTaking;
            update();
        };

        $scope.updateAll = function () {
            selectedUpdateType = appSettings.settingType.All;
            update();
        };

        $rootScope.$on('update-member-position-taking', function (event, updateMemberPT) {
            update(updateMemberPT);
        });

        function update(updateMemberPT) {
            var requestContext = controllerService.getRequestContext();
            var settingContext = controllerService.getSettingContext();

            function hasUpdate(data, isShowNoChange) {
                if (angular.isUndefined(data)) {
                    return false; // form invalid
                }

                if (data.length === 0) {
                    if (isShowNoChange) {
                        messageService.showNoChange();
                    }
                    return false;
                }

                return true;
            }

            function hasUncheckAutoPT(data) {
                var isAutoPTName, ptName;
                switch (requestContext.FormType) {
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
                            var name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, pt);
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

            messageService.hide();
            var positionTakings;
            var betLimits;
            var discounts;
            var statuses;
            switch (selectedUpdateType) {
                case appSettings.settingType.All:
                    statuses = controllerService.getUpdateData(appSettings.settingType.Status);
                    discounts = controllerService.getUpdateData(appSettings.settingType.Discount);
                    betLimits = controllerService.getUpdateData(appSettings.settingType.BetLimit);
                    positionTakings = controllerService.getUpdateData(appSettings.settingType.PositionTaking);
                    var hasUpdatePT;
                    if (!hasUpdate(statuses, false)
                        && !hasUpdate(discounts, false)
                        && !hasUpdate(betLimits, false)
                        && !(hasUpdatePT = hasUpdate(positionTakings, true))) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUpdatePT && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }

                    break;

                case appSettings.settingType.PositionTaking:
                    positionTakings = controllerService.getUpdateData(appSettings.settingType.PositionTaking);
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }
                    break;

                case appSettings.settingType.BetLimit:
                    betLimits = controllerService.getUpdateData(appSettings.settingType.BetLimit);
                    if (!hasUpdate(betLimits, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Discount:
                    discounts = controllerService.getUpdateData(appSettings.settingType.Discount);
                    if (!hasUpdate(discounts, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Status:
                    statuses = controllerService.getUpdateData(appSettings.settingType.Status);
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

            function reload(oldData, newData) {
                angular.forEach(newData, function (newItem) {
                    angular.forEach(oldData, function (oldItem) {
                        var isFound = false;
                        if (!isFound
                            && newItem.SportType === oldItem.SportType
                            && newItem.BetType === oldItem.BetType
                            && newItem.IsLive === oldItem.IsLive) {
                            angular.copy(newItem, oldItem);
                            isFound = true;
                        }
                    });
                });
            }

            var selectModel = modelService.createSelectModel();
            var multipleInfo = modelService.getMultipleInfo();
            var updateModel = angular.extend({
                SettingType: selectedUpdateType,
                StatusLevel: settingContext.StatusLevel,
                StatusString: JSON.stringify(statuses),
                DiscountLevel: settingContext.DiscountLevel,
                DiscountString: JSON.stringify(discounts),
                BetLimitLevel: settingContext.BetLimitLevel,
                BetLimitString: JSON.stringify(betLimits),
                PositionTakingLevel: settingContext.PositionTakingLevel,
                PositionTakingString: JSON.stringify(positionTakings)
            }, selectModel);
            if (multipleInfo.IsMultiple) {
                updateModel = angular.extend({
                    TargetCustIds: JSON.stringify(multipleInfo.CustIds)
                }, updateModel);
            }

            dataService.update(updateModel, function (response) {
                var responseReader = new ResponseReader(updateModel.SettingType);
                var settingResults = responseReader.read(response);
                angular.forEach(settingResults, function (settingResult) {
                    var result = settingResult.result;
                    if (result.IsSuccess) {
                        var settingType = settingResult.settingType;
                        switch (settingType) {
                            case appSettings.settingType.Status:
                                reload($scope.statuses, statuses);
                                break;

                            case appSettings.settingType.Discount:
                                reload($scope.discounts, discounts);
                                break;

                            case appSettings.settingType.BetLimit:
                                reload($scope.betLimits, betLimits);
                                break;

                            case appSettings.settingType.PositionTaking:
                                reload($scope.positionTakings, positionTakings);
                                break;
                        }
                    }
                });
            });
        }
    };

    fanexProductController.$inject = injectParams;
    angular.module('customerSettingApp').controller('FanexProductController', fanexProductController);
} ());