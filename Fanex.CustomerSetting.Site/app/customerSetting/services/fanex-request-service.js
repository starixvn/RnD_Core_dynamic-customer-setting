(function () {
    'use strict';
    var injectParams = ['$injector', '$http', 'fanexAppSettings', 'fanexProductSettingService', 'fanexMessageService'];
    var fanexRequestService = function ($injector, $http, appSettings, productSettingService, messageService) {
        var queryData;
        var commandData;

        function isSuccessResponse(response) {
            if (angular.isDefined(response.IsSuccess) && response.IsSuccess === false) {
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
                    var queryReader = new QueryReader();
                    queryData = queryReader.read(response);
                    successCallback(queryData);
                }
            });

            return request;
        }

        function getQueryData() {
            return queryData;
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
                    var commandReader = new CommandReader();
                    commandData = commandReader.read(response);
                    successCallback(commandData);
                }
            });

            return request;
        }

        function getCommandData() {
            return commandData;
        }

        return {
            query: query,
            getQueryData: getQueryData,
            command: command,
            getCommandData: getCommandData
        };
    };

    fanexRequestService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexRequestService', fanexRequestService);
})();