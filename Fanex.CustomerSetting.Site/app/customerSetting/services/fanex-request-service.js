'use strict';

(function () {
    var injectParams = ['$injector', '$http', '$rootScope', 'fanexAppSettings', 'fanexCommonService', 'fanexMessageService'];
    var fanexRequestService = function ($injector, $http, $rootScope, appSettings, commonService, messageService) {
        function isSuccessResponse(response) {
            if (!response.IsSuccess) {
                messageService.showError(response.ErrMsg);
                return false;
            }

            return true;
        }

        this.query = function (queryModel, successCallback) {
            var url = appSettings.rootUrl + '/CustomerSetting/Query';
            var request = $http.post(url, queryModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }
                
                if (angular.isFunction(successCallback)) {
                    var productId = queryModel.ProductId;
                    var productSetting = commonService.getProductSetting(productId);
                    var QueryReader = $injector.get(productSetting.queryReader);
                    var queryReaderObj = new QueryReader();
                    var data = queryReaderObj.read(response);
                    successCallback(data);
                }
            });

            return request;
        };

        this.command = function (commandModel, successCallback) {
            var url = appSettings.rootUrl + '/CustomerSetting/Command';
            var request = $http.post(url, commandModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }
                
                if (angular.isFunction(successCallback)) {
                    var productId = commandModel.ProductId;
                    var productSetting = commonService.getProductSetting(productId);
                    var CommandReader = $injector.get(productSetting.commandReader);
                    var commandReaderObj = new CommandReader();
                    var data = commandReaderObj.read(response);
                    successCallback(data);
                }
            });

            return request;
        };
    };

    fanexRequestService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexRequestService', fanexRequestService);
})();