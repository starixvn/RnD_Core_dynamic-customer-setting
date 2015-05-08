'use strict';

(function () {
    var injectParams = ['fanexAppSettings'];
    var fanexSettingResult = function (appSettings) {
        function SettingResult(settingType, result) {
            this.settingType = settingType;
            this.result = result;
        }

        // return array of setting results of an user.
        SettingResult.fromUserResult = function (commandType, userResult) {
            var settingResults = [];
            switch (commandType) {
                case appSettings.settingType.All:
                    settingResults.push(new SettingResult(appSettings.settingType.Status, userResult.StatusResult));
                    settingResults.push(new SettingResult(appSettings.settingType.Discount, userResult.DiscountResult));
                    settingResults.push(new SettingResult(appSettings.settingType.BetLimit, userResult.BetLimitResult));
                    settingResults.push(new SettingResult(appSettings.settingType.PositionTaking, userResult.PositionTakingResult));
                    break;

                case appSettings.settingType.Status:
                    settingResults.push(new SettingResult(appSettings.settingType.Status, userResult.StatusResult));
                    break;

                case appSettings.settingType.Discount:
                    settingResults.push(new SettingResult(appSettings.settingType.Discount, userResult.DiscountResult));
                    break;

                case appSettings.settingType.BetLimit:
                    settingResults.push(new SettingResult(appSettings.settingType.BetLimit, userResult.BetLimitResult));
                    break;

                case appSettings.settingType.PositionTaking:
                    settingResults.push(new SettingResult(appSettings.settingType.PositionTaking, userResult.PositionTakingResult));
                    break;
            }

            return settingResults;
        };

        return SettingResult;
    };

    fanexSettingResult.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexSettingResult', fanexSettingResult);
})();