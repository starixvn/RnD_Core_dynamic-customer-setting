'use strict';

(function () {
    var injectParams = ['$http', '$rootScope', 'messageService'];
    var dataService = function ($http, $rootScope, messageService) {
        function isSuccess(response) {
            if (!response.IsSuccess) {
                messageService.showError(response.ErrMsg);
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
                    messageService.showSuccess(response.ErrMsg);
                    successCallback(response);
                }
            });
            return request;
        };
    };

    dataService.$inject = injectParams;
    angular.module('customerSettingApp').service('dataService', dataService);
})();