'use strict';

(function () {
    var injectParams = ['$http', '$rootScope', 'errorService'];
    var dataService = function ($http, $rootScope, errorService) {
        function isSuccess(response) {
            if (!response.IsSuccess) {
                errorService.show(response.ErrMsg);
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