'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexModelService', 'fanexDataService', 'fanexSettingResult'];
    // this service reads response and translate into data to be useed by fanexMessageService.
    var fanexCommandReader = function (appSettings, modelService, dataService, SettingResult) {
        function SingleCommandReader() {
            this.read = function (response) {
                var userResult = response;
                var userSettingResults = SettingResult.fromUserResult(dataService.commandType, userResult);
                return userSettingResults;
            };
        }

        function MultipleCommandReader() {
            this.read = function (response) {
                var userResults = response.Results;
                var usersSettingResults = [];
                angular.forEach(userResults, function (userResult) {
                    var userSettingResults = SettingResult.fromUserResult(dataService.commandType, userResult);
                    usersSettingResults.push(userSettingResults);
                });

                return usersSettingResults;
            };
        }

        function CommandReader() {
            this.read = function (response) {
                this.isSuccess = response.IsSuccess;
                var multipleInfo = modelService.getMultipleInfo();
                var reader;
                if (multipleInfo.IsMultiple) {
                    reader = new MultipleCommandReader(multipleInfo.UserNames);
                } else {
                    reader = new SingleCommandReader();
                }

                dataService.commandData = reader.read(response);
                return dataService.commandData;
            };
        }

        return CommandReader;
    };

    fanexCommandReader.$inject = injectParams;
    angular.module('customerSettingApp').factory('FanexCommandReader', fanexCommandReader);
})();