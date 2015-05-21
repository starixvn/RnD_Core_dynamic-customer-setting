(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexStatusService', 'fanexDiscountService',
            'fanexBetLimitService', 'fanexPtService'];
    var fanexSettingTypeHandler = function (appSettings, statusService, discountService,
            betLimitService, ptService) {
        var StatusHandler = function () {
            this.settingType = appSettings.settingType.Status;
            this.getScopes = statusService.getScopes;
        };

        var DiscountHandler = function () {
            this.settingType = appSettings.settingType.Discount;
            this.getScopes = discountService.getScopes;
        };

        var BetLimitHandler = function () {
            this.settingType = appSettings.settingType.BetLimit;
            this.getScopes = betLimitService.getScopes;
        };

        var PositionTakingHandler = function () {
            this.settingType = appSettings.settingType.PositionTaking;
            this.getScopes = ptService.getScopes;
        };

        function SettingTypeHandler(settingType) {
            switch (settingType) {
                case appSettings.settingType.Status:
                    return new StatusHandler();

                case appSettings.settingType.Discount:
                    return new DiscountHandler();

                case appSettings.settingType.BetLimit:
                    return new BetLimitHandler();

                case appSettings.settingType.PositionTaking:
                    return new PositionTakingHandler();

                default:
                    throw 'Not implemented yet.';
            }
        }

        return SettingTypeHandler;
    };

    fanexSettingTypeHandler.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexSettingTypeHandler', fanexSettingTypeHandler);
})();