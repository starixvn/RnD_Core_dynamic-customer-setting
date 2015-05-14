'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexModelService'];
    var fanexCommandReader = function (appSettings, modelService) {
        function SingleCommandReader() {
            this.read = function (response) {
                var commandResult = response;
                return commandResult;
            };
        }

        function MultipleCommandReader() {
            this.read = function (response) {
                var commandResults = response;
                return commandResults;
            };
        }

        function CommandReader() {
            this.read = function (response) {
                var queryModel = modelService.getQueryModel();
                var multipleInfo = queryModel.getMultipleInfo();
                var reader;
                if (multipleInfo.IsMultiple) {
                    reader = new MultipleCommandReader();
                } else {
                    reader = new SingleCommandReader();
                }

                return reader.read(response);
            };
        }

        return CommandReader;
    };

    fanexCommandReader.$inject = injectParams;
    angular.module('customerSettingApp').factory('FanexCommandReader', fanexCommandReader);
})();