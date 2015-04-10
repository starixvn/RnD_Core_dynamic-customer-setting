'use strict';

(function () {
    var injectParams = ['$http', '$rootScope', 'messageService', 'appSettings', 'ResponseReader'];
    var dataService = function ($http, $rootScope, messageService, appSettings, ResponseReader) {
        function isSuccessResponse(response) {
            if (!response.IsSuccess) {
                messageService.showError(response.ErrMsg);
                return false;
            }

            return true;
        }

        this.select = function (selectModel, successCallback) {
            var url = appSettings.rootUrl + '/CustomerSetting/Select';
            var request = $http.post(url, selectModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                if (angular.isFunction(successCallback)) {
                    successCallback(response);
                }
            });

            return request;
        };

        this.update = function (updateModel, successCallback) {
            var url = appSettings.rootUrl + '/CustomerSetting/Update';
            var request = $http.post(url, updateModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                var responseReader = new ResponseReader(updateModel.SettingType);
                var settingResults = responseReader.read(response);
                var results = [];
                angular.forEach(settingResults, function (settingResult) {
                    results.push(settingResult.result);
                });
                messageService.showResults(results);

                if (angular.isFunction(successCallback)) {
                    successCallback(response);
                }
            });

            return request;
        };
    };

    dataService.$inject = injectParams;
    angular.module('customerSettingApp').service('dataService', dataService);
})();