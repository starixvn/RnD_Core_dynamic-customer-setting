(function () {
    'use strict';
    angular.module('customerSettingApp').value('fanexProductSettings', {
        viewSettings: {
            'default': {
                controller: 'FanexDefaultController',
                templateUrl: 'templates/fanex-default-page.htm',
                queryReader: 'fanexQueryReader',
                commandReader: 'FanexCommandReader'
            },
            error: {
                controller: 'FanexErrorController',
                templateUrl: 'templates/fanex-error-page.htm'
            },
            products: [
                {
                    id: 0,
                    name: 'demo',
                    templateUrl: 'templates/fanex-demo-page.htm'
                }
            ]
        }
    });
})();