'use strict';

(function () {
    var injectParams = ['fanexDataService'];
    var fanexQueryReader = function (dataService) {
        function QueryReader() {
            this.read = function (response) {
                var requestContext = response.RequestContext;
                var customerSetting = response.CustomerSetting;
                var settingContext = customerSetting.SettingContext;
                var productName = requestContext.ProductName;
                dataService.productName = productName;
                dataService.queryData = {
                    RequestContext: requestContext,
                    CustomerSetting: customerSetting,
                    SettingContext: settingContext
                };

                return dataService.queryData;
            };
        }

        return QueryReader;
    };

    fanexQueryReader.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexQueryReader', fanexQueryReader);
})();