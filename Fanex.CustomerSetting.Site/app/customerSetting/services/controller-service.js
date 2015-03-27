'use strict';

(function () {
    var injectParams = ['$rootScope', 'appSettings', 'modelService', 'dataService', 'resourceService', 'messageService'];
    var controllerService = function ($rootScope, appSettings, modelService, dataService, resourceService, messageService) {
        var self = this;
        var statusScopes = [];
        var discountScopes = [];
        var betLimitScopes = [];
        var positionTakingScopes = [];
        this.linkStatus = function (scope) {
            statusScopes.push(scope);
        };
        this.linkDiscount = function (scope) {
            discountScopes.push(scope);
        };
        this.linkBetLimit = function (scope) {
            betLimitScopes.push(scope);
        };
        this.linkPositionTaking = function (scope) {
            positionTakingScopes.push(scope);
        };

        this.getDisplayName = function (settingType, data) {
            var liveName = data.IsLive === true ? ' (' + resourceService.getString('Live') + ')' : '';
            var settingLevel;
            switch (settingType) {
                case appSettings.settingType.PositionTaking:
                    settingLevel = self.positionTakingLevel;
                    break;

                case appSettings.settingType.BetLimit:
                    settingLevel = self.betLimitLevel;
                    break;

                case appSettings.settingType.Discount:
                    settingLevel = self.discountLevel;
                    break;

                case appSettings.settingType.Status:
                    settingLevel = self.statusLevel;
                    break;
            }

            switch (settingLevel) {
                case appSettings.settingLevel.Product:
                    return self.productName;
                case appSettings.settingLevel.SportType:
                    return data.SportTypeName;
                case appSettings.settingLevel.BetType:
                    return data.BetTypeName + liveName;
                case appSettings.settingLevel.SportTypeBetType:
                    return data.BetTypeName + liveName;
            }

            return undefined;
        };

        this.select = function (callback) {
            var selectModel = modelService.createSelectModel();
            dataService.select(selectModel, function (response) {
                var requestContext = response.RequestContext;
                self.requestContext = requestContext;
                self.productId = requestContext.ProductId;
                self.productName = requestContext.ProductName;
                self.formType = requestContext.FormType;
                self.language = requestContext.Language;

                var customerSetting = response.CustomerSetting;
                self.customerSetting = customerSetting;
                self.information = customerSetting.Information;
                self.statuses = customerSetting.Statuses;
                self.discounts = customerSetting.Discounts;
                self.betLimits = customerSetting.BetLimits;
                self.positionTakings = customerSetting.PositionTakings;

                var settingContext = customerSetting.SettingContext;
                self.settingContext = settingContext;
                self.statusLevel = settingContext.StatusLevel;
                self.discountLevel = settingContext.DiscountLevel;
                self.betLimitLevel = settingContext.BetLimitLevel;
                self.positionTakingLevel = settingContext.PositionTakingLevel;
                self.disableAutoMPT = settingContext.DisableAutoMPT;

                callback();
            });
        };

        var selectSettingType;
        this.update = function (settingType) {
            selectSettingType = settingType;
            update();
        };

        $rootScope.$on('update-member-position-taking', function (event, updateMemberPT) {
            update(updateMemberPT);
        });

        function update(updateMemberPT) {
            function getData(scopes) {
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
                switch (self.formType) {
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
                            var name = self.getDisplayName(appSettings.settingType.PositionTaking, pt);
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
            switch (selectSettingType) {
                case appSettings.settingType.All:
                    statuses = getData(statusScopes);
                    discounts = getData(discountScopes);
                    betLimits = getData(betLimitScopes);
                    positionTakings = getData(positionTakingScopes);
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
                    positionTakings = getData(positionTakingScopes);
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }
                    break;

                case appSettings.settingType.BetLimit:
                    betLimits = getData(betLimitScopes);
                    if (!hasUpdate(betLimits, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Discount:
                    discounts = getData(discountScopes);
                    if (!hasUpdate(discounts, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Status:
                    statuses = getData(statusScopes);
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

            var selectModel = modelService.createSelectModel();
            var updateModel = angular.extend({
                UserName: self.information.UserName,
                SettingType: selectSettingType,
                StatusLevel: self.statusLevel,
                StatusString: JSON.stringify(statuses),
                DiscountLevel: self.discountLevel,
                DiscountString: JSON.stringify(discounts),
                BetLimitLevel: self.betLimitLevel,
                BetLimitString: JSON.stringify(betLimits),
                PositionTakingLevel: self.positionTakingLevel,
                PositionTakingString: JSON.stringify(positionTakings)
            }, selectModel);
            dataService.update(updateModel);
        }
    };

    controllerService.$inject = injectParams;
    angular.module('customerSettingApp').service('controllerService', controllerService);
})();