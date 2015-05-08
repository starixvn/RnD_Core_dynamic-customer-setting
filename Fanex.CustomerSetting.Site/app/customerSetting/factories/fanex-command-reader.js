'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexModelService', 'fanexSettingResult'];
    var fanexCommandReader = function (appSettings, modelService, SettingResult) {
        function SingleCommandReader(commandType) {
            this.read = function (response) {
                var userResult = response;
                var userSettingResults = SettingResult.fromUserResult(commandType, userResult);
                return userSettingResults;
            };
        }

        function MultipleCommandReader(commandType) {
            this.read = function (response) {
                var userResults = response.Results;
                var usersSettingResults = [];
                angular.forEach(userResults, function (userResult) {
                    var userSettingResults = SettingResult.fromUserResult(commandType, userResult);
                    usersSettingResults.push(userSettingResults);
                });

                return usersSettingResults;
            };
        }

        function CommandData(isSuccess, result) {
            return {
                IsSuccess: isSuccess,
                Result: result
            };
        }

        function CommandReader(commandType) {
            this.read = function (response) {
                var isSuccess = response.IsSuccess;
                var multipleInfo = modelService.getMultipleInfo();
                var reader;
                if (multipleInfo.IsMultiple) {
                    reader = new MultipleCommandReader(commandType);
                } else {
                    reader = new SingleCommandReader(commandType);
                }

                var result = reader.read(response);
                return new CommandData(isSuccess, result);
            };
        }

        return CommandReader;
    };

    fanexCommandReader.$inject = injectParams;
    angular.module('customerSettingApp').factory('FanexCommandReader', fanexCommandReader);
})();