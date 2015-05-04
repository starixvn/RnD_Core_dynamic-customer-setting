'use strict';

(function () {
    angular.module('customerSettingApp').value('fanexProductConfiguration', {
        errorController: 'FanexErrorController',
        errorTemplateUrl: 'templates/fanex-error-page.htm',
        productSettings: [
            {
                id: 255,
                name: 'demo',
                controller: 'FanexProductController',
                templateUrl: 'templates/fanex-product-page.htm'
            },
            {
                id: 254,
                name: 'demo',
                controller: 'FanexProductController',
                templateUrl: 'templates/fanex-product-page.htm'
            }
        ]
    });
})();