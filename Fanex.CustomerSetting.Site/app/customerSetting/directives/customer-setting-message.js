'use strict';

(function () {
    var injectParams = [];
    var customerSettingMessageDirective = function () {
        return {
            restrict: 'A',
            replace: true,
            scope: {
                messages: '=customerSettingMessage'
            },
            templateUrl: 'templates/customer-setting-message.htm'
        };
    };

    customerSettingMessageDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingMessage', customerSettingMessageDirective);
})();