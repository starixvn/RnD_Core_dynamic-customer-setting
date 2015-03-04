'use strict';

(function () {
    var injectParams = ['appSettings'];
    var linkingService = function (appSettings) {
        function getData(scopes) {
            var data = [];
            var isFormInvalid = false;
            angular.forEach(scopes, function (scope) {
                if (!isFormInvalid) {
                    var result = scope.validate();
                    if (result.errCode === appSettings.errorType.Success) {
                        data.push(result.data);
                    } else if (result.errCode === appSettings.errorType.FormInvalid) {
                        isFormInvalid = true;
                    }
                }
            });

            return isFormInvalid
                ? undefined
                : data;
        }

        var statusScopes = [];
        this.linkStatus = function (scope) {
            statusScopes.push(scope);
        };
        this.statuses = function () {
            return getData(statusScopes);
        };

        var discountScopes = [];
        this.linkDiscount = function (scope) {
            discountScopes.push(scope);
        };
        this.discounts = function () {
            return getData(discountScopes);
        };

        var betLimitScopes = [];
        this.linkBetLimit = function (scope) {
            betLimitScopes.push(scope);
        };
        this.betLimits = function () {
            return getData(betLimitScopes);
        };

        var positionTakingScopes = [];
        this.linkPositionTaking = function (scope) {
            positionTakingScopes.push(scope);
        };
        this.positionTakings = function () {
            return getData(positionTakingScopes);
        };
    };

    linkingService.$inject = injectParams;
    angular.module('customerSettingApp').service('linkingService', linkingService);
})();