'use strict';

(function () {
    var injectParams = ['$http', '$rootScope', 'messageService', 'appSettings'];
    var dataService = function ($http, $rootScope, messageService, appSettings) {
        function isSuccessResponse(response) {
            if (!response.IsSuccess) {
                messageService.showError(response.ErrMsg);
                return false;
            }

            return true;
        }

        this.select = function (selectModel, successCallback) {
            var request = $http.post('/CustomerSetting/Select', selectModel);
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
            var request = $http.post('/CustomerSetting/Update', updateModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                var results = [];
                switch (updateModel.SettingType) {
                    case appSettings.settingType.All:
                        results.push(response.StatusResult);
                        results.push(response.DiscountResult);
                        results.push(response.BetLimitResult);
                        results.push(response.PositionTakingResult);
                        break;

                    case appSettings.settingType.Status:
                        results.push(response.StatusResult);
                        break;

                    case appSettings.settingType.Discount:
                        results.push(response.DiscountResult);
                        break;

                    case appSettings.settingType.BetLimit:
                        results.push(response.BetLimitResult);
                        break;

                    case appSettings.settingType.PositionTaking:
                        results.push(response.PositionTakingResult);
                        break;
                }

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