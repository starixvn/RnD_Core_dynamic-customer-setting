'use strict';

(function () {
    var injectParams = ['$rootScope', '$scope', 'fanexAppSettings', 'fanexModelService', 'fanexUpdateMemberPtService',
            'fanexTemplateService', 'fanexRequestService', 'fanexMessageService', 'fanexResourceService', 'fanexSettingTypeHandler'];
    var fanexDefaultController = function ($rootScope, $scope, appSettings, modelService, updateMemberPtService,
            templateService, requestService, messageService, resourceService, SettingTypeHandler) {
        var queryModel = modelService.getQueryModel();
        requestService.query(queryModel, function (queryData) {
            var requestContext = queryData.RequestContext;
            var customerSetting = queryData.CustomerSetting;
            var settingContext = queryData.SettingContext;
            resourceService.setCurrentProductName(queryData.ProductName);
            resourceService.setLanguage(queryData.Language);

            var multipleInfo = queryModel.getMultipleInfo();
            if (multipleInfo.IsMultiple) {
                $scope.custNames = multipleInfo.CustNames.join(',');
            } else {
                $scope.information = customerSetting.Information;
            }

            $scope.formType = queryModel.FormType;

            // status
            $scope.statuses = customerSetting.Statuses;
            $scope.statusLevel = settingContext.StatusLevel;
            $scope.statusTemplate = templateService.resolveStatus();

            // discount
            $scope.discounts = customerSetting.Discounts;
            $scope.discountLevel = settingContext.DiscountLevel;
            $scope.discountTemplate = templateService.resolveDiscount();
            $scope.hasUpdateDiscount = $scope.formType !== appSettings.formType.SuperEditMember
                && $scope.formType !== appSettings.formType.MasterEditMember;

            // bet limit
            $scope.betLimits = customerSetting.BetLimits;
            $scope.betLimitLevel = settingContext.BetLimitLevel;
            $scope.betLimitTemplate = templateService.resolveBetLimit();
            $scope.hasUpdateBetLimit = $scope.hasUpdateDiscount;

            // position taking
            $scope.positionTakings = customerSetting.PositionTakings;
            $scope.positionTakingLevel = settingContext.PositionTakingLevel;
            $scope.positionTakingTemplate = templateService.resolvePositionTaking();
            if (requestContext.FormType === appSettings.formType.MasterEditAgent) {
                $scope.disableAutoMPT = settingContext.DisableAutoMPT;
            }

            $scope.hasUpdateAll = queryModel;
        });

        var commandType;
        $scope.updateStatuses = function () {
            commandType = appSettings.settingType.Status;
            update();
        };

        $scope.updateDiscounts = function () {
            commandType = appSettings.settingType.Discount;
            update();
        };

        $scope.updateBetLimits = function () {
            commandType = appSettings.settingType.BetLimit;
            update();
        };

        $scope.updatePositionTakings = function () {
            commandType = appSettings.settingType.PositionTaking;
            update();
        };

        $scope.updateAll = function () {
            commandType = appSettings.settingType.All;
            update();
        };

        $rootScope.$on('update-member-position-taking', function (event, updateMemberPT) {
            update(updateMemberPT);
        });

        function getUpdateData(settingType) {
            var settingTypeHandler = new SettingTypeHandler(settingType);
            var scopes = settingTypeHandler.getScopes();
            var data = [];
            var isFormInvalid = false;
            angular.forEach(scopes, function (scope) {
                if (!isFormInvalid) {
                    if (angular.isFunction(scope.validate)) {
                        var result = scope.validate();
                        if (result.errCode === appSettings.errorType.Success) {
                            data.push(result.data);
                        } else if (result.errCode === appSettings.errorType.FormInvalid) {
                            isFormInvalid = true;
                        }
                    }
                }
            });

            return isFormInvalid
                ? undefined
                : data;
        }

        function update(updateMemberPT) {
            var queryData = requestService.getQueryData();
            var requestContext = queryData.RequestContext;
            var settingContext = queryData.SettingContext;

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
                            var name = resourceService.getDisplayName($scope.positionTakingLevel, pt);
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
                        updateMemberPtService.show(memberPTs);
                        return true;
                    }
                }

                return false;
            }

            var positionTakings;
            var betLimits;
            var discounts;
            var statuses;
            switch (commandType) {
                case appSettings.settingType.All:
                    statuses = getUpdateData(appSettings.settingType.Status);
                    discounts = getUpdateData(appSettings.settingType.Discount);
                    betLimits = getUpdateData(appSettings.settingType.BetLimit);
                    positionTakings = getUpdateData(appSettings.settingType.PositionTaking);
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
                    positionTakings = getUpdateData(appSettings.settingType.PositionTaking);
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }
                    break;

                case appSettings.settingType.BetLimit:
                    betLimits = getUpdateData(appSettings.settingType.BetLimit);
                    if (!hasUpdate(betLimits, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Discount:
                    discounts = getUpdateData(appSettings.settingType.Discount);
                    if (!hasUpdate(discounts, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Status:
                    statuses = getUpdateData(appSettings.settingType.Status);
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

            var multipleInfo = queryModel.getMultipleInfo();
            var commandModel = angular.extend({
                CommandType: commandType,
                StatusLevel: settingContext.StatusLevel,
                StatusString: JSON.stringify(statuses),
                DiscountLevel: settingContext.DiscountLevel,
                DiscountString: JSON.stringify(discounts),
                BetLimitLevel: settingContext.BetLimitLevel,
                BetLimitString: JSON.stringify(betLimits),
                PositionTakingLevel: settingContext.PositionTakingLevel,
                PositionTakingString: JSON.stringify(positionTakings)
            }, queryModel);
            if (multipleInfo.IsMultiple) {
                commandModel = angular.extend({
                    TargetCustIds: JSON.stringify(multipleInfo.CustIds)
                }, commandModel);
            }

            requestService.command(commandModel, function (commandData) {
                if (multipleInfo.IsMultiple) {
                    var custNames = multipleInfo.CustNames;
                    var usersSettingResults = commandData;
                    messageService.showMultiple(custNames, usersSettingResults);
                    return;
                }

                var userSettingResults = commandData;
                messageService.showSingle(userSettingResults);
                angular.forEach(userSettingResults, function (settingResult) {
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

    fanexDefaultController.$inject = injectParams;
    angular.module('customerSettingApp').controller('FanexDefaultController', fanexDefaultController);
} ());