(function () {
    'use strict';
    var injectParams = ['$rootScope', '$scope', 'fanexAppSettings', 'fanexModelService', 'fanexUpdateMemberPtService',
            'fanexRequestService', 'fanexMessageService', 'fanexResourceService', 'fanexSettingTypeHandler', 'fanexCommonService'];
    var fanexDefaultController = function ($rootScope, $scope, appSettings, modelService, updateMemberPtService,
            requestService, messageService, resourceService, SettingTypeHandler, commonService) {
        var queryModel = modelService.getQueryModel();
        requestService.query(queryModel, function (queryData) {
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
            if (customerSetting.Statuses) {
                $scope.statusModel = {
                    Collection: customerSetting.Statuses,
                    SettingLevel: settingContext.StatusLevel,
                    HasUpdate: true,
                    Update: updateStatuses
                };
            }

            // discount
            if (customerSetting.Discounts) {
                $scope.discountModel = {
                    Collection: customerSetting.Discounts,
                    SettingLevel: settingContext.DiscountLevel,
                    HasUpdate: $scope.formType !== appSettings.formType.SuperEditMember
                    && $scope.formType !== appSettings.formType.MasterEditMember,
                    Update: updateDiscounts
                };
            }

            // bet limit
            if (customerSetting.BetLimits) {
                $scope.betLimitModel = {
                    Collection: customerSetting.BetLimits,
                    SettingLevel: settingContext.BetLimitLevel,
                    HasUpdate: $scope.discountModel.HasUpdate,
                    Update: updateBetLimits
                };
            }

            // position taking
            if (customerSetting.PositionTakings) {
                $scope.positionTakingModel = {
                    Collection: customerSetting.PositionTakings,
                    SettingLevel: settingContext.PositionTakingLevel,
                    HasUpdate: true,
                    Update: updatePositionTakings
                };
            }

            $scope.hasUpdateAll = queryModel;
        });

        var commandType;
        function updateStatuses() {
            commandType = appSettings.commandType.Status;
            update();
        }

        function updateDiscounts() {
            commandType = appSettings.commandType.Discount;
            update();
        }

        function updateBetLimits() {
            commandType = appSettings.commandType.BetLimit;
            update();
        }

        function updatePositionTakings() {
            commandType = appSettings.commandType.PositionTaking;
            update();
        }

        $scope.updateAll = function () {
            commandType = appSettings.commandType.All;
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
                            var name = resourceService.getDisplayName($scope.positionTakingModel.SettingLevel, pt);
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
                case appSettings.commandType.All:
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

                case appSettings.commandType.PositionTaking:
                    positionTakings = getUpdateData(appSettings.settingType.PositionTaking);
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }
                    break;

                case appSettings.commandType.BetLimit:
                    betLimits = getUpdateData(appSettings.settingType.BetLimit);
                    if (!hasUpdate(betLimits, true)) {
                        return;
                    }
                    break;

                case appSettings.commandType.Discount:
                    discounts = getUpdateData(appSettings.settingType.Discount);
                    if (!hasUpdate(discounts, true)) {
                        return;
                    }
                    break;

                case appSettings.commandType.Status:
                    statuses = getUpdateData(appSettings.settingType.Status);
                    if (!hasUpdate(statuses, true)) {
                        return;
                    }
                    break;

                default:
                    throw 'Not implemented yet.';
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
                            commonService.copyProperties(newItem, oldItem);
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
                    var commandResults = commandData;
                    messageService.showMultiple(custNames, commandResults);
                    return;
                }

                var commandResult = commandData;
                messageService.showSingle(commandResult);
                angular.forEach(commandResult.SettingResults, function (settingResult) {
                    if (settingResult.IsSuccess) {
                        var settingType = settingResult.SettingType;
                        switch (settingType) {
                            case appSettings.settingType.Status:
                                reload($scope.statusModel.Collection, statuses);
                                break;

                            case appSettings.settingType.Discount:
                                reload($scope.discountModel.Collection, discounts);
                                break;

                            case appSettings.settingType.BetLimit:
                                reload($scope.betLimitModel.Collection, betLimits);
                                break;

                            case appSettings.settingType.PositionTaking:
                                reload($scope.positionTakingModel.Collection, positionTakings);
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