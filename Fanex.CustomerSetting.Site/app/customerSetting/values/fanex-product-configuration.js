'use strict';

(function () {
    angular.module('customerSettingApp').value('fanexProductConfiguration', {
        errorController: 'FanexErrorController',
        errorTemplateUrl: 'templates/fanex-error-page.htm',
        defaultProductSetting: {
            controller: 'FanexProductController',
            templateUrl: 'templates/fanex-product-page.htm',
            queryReader: 'fanexQueryReader',
            commandReader: 'FanexCommandReader'
        },
        productSettings: [
            {
                id: 0,
                name: 'demo',
                controller: 'FanexProductController',
                templateUrl: 'templates/fanex-demo-page.htm',
                queryReader: 'FanexQueryReader',
                commandReader: 'FanexCommandReader'
            }
        ]
    });
})();