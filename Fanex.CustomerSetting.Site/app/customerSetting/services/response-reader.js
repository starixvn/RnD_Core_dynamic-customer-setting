'use strict';

(function () {
    var injectParams = ['appSettings'];
    var responseReaderFactory = function (appSettings) {
        var SettingResult = function (settingType, result) {
            this.settingType = settingType;
            this.result = result;
        };

        var ResponseReader = function (settingType) {
            this.settingType = settingType;
            this.isSuccess = appSettings.errorCode.Success;
            this.settingResults = [];
        };

        ResponseReader.prototype.read = function (response) {
            this.isSuccess = response.IsSuccess;
            var settingResults = [];
            switch (this.settingType) {
                case appSettings.settingType.All:
                    settingResults.push(new SettingResult(appSettings.settingType.Status, response.StatusResult));
                    settingResults.push(new SettingResult(appSettings.settingType.Discount, response.DiscountResult));
                    settingResults.push(new SettingResult(appSettings.settingType.BetLimit, response.BetLimitResult));
                    settingResults.push(new SettingResult(appSettings.settingType.PositionTaking, response.PositionTakingResult));
                    break;

                case appSettings.settingType.Status:
                    settingResults.push(new SettingResult(appSettings.settingType.Status, response.StatusResult));
                    break;

                case appSettings.settingType.Discount:
                    settingResults.push(new SettingResult(appSettings.settingType.Discount, response.DiscountResult));
                    break;

                case appSettings.settingType.BetLimit:
                    settingResults.push(new SettingResult(appSettings.settingType.BetLimit, response.BetLimitResult));
                    break;

                case appSettings.settingType.PositionTaking:
                    settingResults.push(new SettingResult(appSettings.settingType.PositionTaking, response.PositionTakingResult));
                    break;
            }

            return settingResults;
        };

        return ResponseReader;
    };

    responseReaderFactory.$inject = injectParams;
    angular.module('customerSettingApp').factory('ResponseReader', responseReaderFactory);
})();