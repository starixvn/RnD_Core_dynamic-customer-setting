'use strict';

(function () {
    var injectParams = ['appSettings', 'resourceService'];
    var controllerService = function (appSettings, resourceService) {
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

        this.getScopes = function (settingType) {
            switch (settingType) {
                case appSettings.settingType.Status:
                    return statusScopes;

                case appSettings.settingType.Discount:
                    return discountScopes;

                case appSettings.settingType.BetLimit:
                    return betLimitScopes;

                case appSettings.settingType.PositionTaking:
                    return positionTakingScopes;
            }

            return undefined;
        };

        this.getUpdateData = function (settingType) {
            var scopes = this.getScopes(settingType);
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
        };

        this.getDisplayName = function (settingType, data) {
            var liveName = data.IsLive === true ? ' (' + resourceService.getString('Live') + ')' : '';
            var settingLevel;
            switch (settingType) {
                case appSettings.settingType.PositionTaking:
                    settingLevel = settingContext.PositionTakingLevel;
                    break;

                case appSettings.settingType.BetLimit:
                    settingLevel = settingContext.BetLimitLevel;
                    break;

                case appSettings.settingType.Discount:
                    settingLevel = settingContext.DiscountLevel;
                    break;

                case appSettings.settingType.Status:
                    settingLevel = settingContext.StatusLevel;
                    break;
            }

            switch (settingLevel) {
                case appSettings.settingLevel.Product:
                    return requestContext.ProductName;
                case appSettings.settingLevel.SportType:
                    return data.SportTypeName;
                case appSettings.settingLevel.BetType:
                    return data.BetTypeName + liveName;
                case appSettings.settingLevel.SportTypeBetType:
                    return data.BetTypeName + liveName;
            }

            return undefined;
        };

        function RequestContext() {
            this.TargetCustId = 0;
            this.FormType = undefined;
            this.ProductId = 0;
            this.ProductName = undefined;
            this.Language = undefined;
        }
        function SettingContext() {
            this.PositionTakingLevel = undefined;
            this.BetLimitLevel = undefined;
            this.DiscountLevel = undefined;
            this.StatusLevel = undefined;
            this.DisableAutoMPT = undefined;
        }
        function CustomerSetting() {
            this.Information = undefined;
            this.Statuses = undefined;
            this.Discounts = undefined;
            this.BetLimits = undefined;
            this.PositionTakings = undefined;
        }

        var requestContext = new RequestContext();
        var settingContext = new SettingContext();
        var customerSetting = new CustomerSetting();
        this.updateResponse = function (response) {
            angular.copy(response.RequestContext, requestContext);
            angular.copy(response.CustomerSetting, customerSetting);
            angular.copy(response.CustomerSetting.SettingContext, settingContext);
        };

        this.getRequestContext = function () {
            return requestContext;
        };
        this.getSettingContext = function () {
            return settingContext;
        };
        this.getCustomerSetting = function () {
            return customerSetting;
        };
    };

    controllerService.$inject = injectParams;
    angular.module('customerSettingApp').service('controllerService', controllerService);
})();