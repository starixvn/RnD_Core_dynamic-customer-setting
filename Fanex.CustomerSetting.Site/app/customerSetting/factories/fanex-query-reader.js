(function () {
    'use strict';
    var injectParams = [];
    var fanexQueryReader = function () {
        function QueryData(response) {
            var requestContext = response.RequestContext;
            var customerSetting = response.CustomerSetting;
            var settingContext = customerSetting.SettingContext;
            var productName = requestContext.ProductName;
            var language = requestContext.Language;

            return {
                RequestContext: requestContext,
                CustomerSetting: customerSetting,
                SettingContext: settingContext,
                ProductName: productName,
                Language: language
            };
        }

        function QueryReader() {
            this.read = function (response) {
                return new QueryData(response);
            };
        }

        return QueryReader;
    };

    fanexQueryReader.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexQueryReader', fanexQueryReader);
})();