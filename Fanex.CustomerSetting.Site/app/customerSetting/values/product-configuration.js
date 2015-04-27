'use strict';

(function () {
    angular.module('customerSettingApp').value('productConfiguration', {
        errorController: 'CustomerSettingErrorController',
        errorTemplateUrl: 'templates/customer-setting-error-page.htm',
        productSettings: [
            {
                id: 255,
                name: 'demo',
                controller: 'CustomerSettingProductController',
                templateUrl: 'templates/customer-setting-product-page.htm'
            },
            {
                id: 254,
                name: 'demo',
                controller: 'CustomerSettingProductController',
                templateUrl: 'templates/customer-setting-product-page.htm'
            }
        ]
    });
})();