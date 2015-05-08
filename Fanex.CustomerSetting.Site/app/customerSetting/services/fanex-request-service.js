'use strict';

(function () {
    var injectParams = ['$injector', '$http', '$rootScope', 'fanexAppSettings', 'fanexProductSettingService', 'fanexMessageService'];
    var fanexRequestService = function ($injector, $http, $rootScope, appSettings, productSettingService, messageService) {
        function isSuccessResponse(response) {
            if (!response.IsSuccess) {
                messageService.showError(response.ErrMsg);
                return false;
            }

            return true;
        }

        function query(queryModel, successCallback) {
            var url = appSettings.rootUrl + '/CustomerSetting/Query';
            var request = $http.post(url, queryModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                if (angular.isFunction(successCallback)) {
                    var productId = queryModel.ProductId;
                    var productViewSetting = productSettingService.getViewSetting(productId);
                    var QueryReader = $injector.get(productViewSetting.queryReader);
                    var queryReaderObj = new QueryReader();
                    var data = queryReaderObj.read(response);
                    successCallback(data);
                }
            });

            return request;
        }

        function command(commandModel, successCallback) {
            var url = appSettings.rootUrl + '/CustomerSetting/Command';
            var request = $http.post(url, commandModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                if (angular.isFunction(successCallback)) {
                    var productId = commandModel.ProductId;
                    var productViewSetting = productSettingService.getViewSetting(productId);
                    var CommandReader = $injector.get(productViewSetting.commandReader);
                    var commandReaderObj = new CommandReader();
                    var data = commandReaderObj.read(response);
                    successCallback(data);
                }
            });

            return request;
        }

        return {
            query: query,
            command: command
        };
    };

    fanexRequestService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexRequestService', fanexRequestService);
})();