'use strict';

(function () {
    var injectParams = ['$http', '$rootScope'];
    var dataService = function ($http, $rootScope) {
        function isSuccess(response) {
            if (!response.IsSuccess) {
                $rootScope.hasError = true;
                $rootScope.errorMessage = response.ErrMsg;
                return false;
            }

            return true;
        }

        this.select = function (selectModel, successCallback) {
            var request = $http.post('/CustomerSetting/Select', selectModel);
            request.success(function (response) {
                if (isSuccess(response)) {
                    successCallback(response);
                }
            });

            return request;
        };

        this.update = function (updateModel, successCallback) {
            var request = $http.post('/CustomerSetting/Update', updateModel);
            request.success(function (response) {
                if (isSuccess(response)) {
                    successCallback(response);
                }
            });
            return request;
        };
    };

    dataService.$inject = injectParams;
    angular.module('customerSettingApp').service('dataService', dataService);
})();