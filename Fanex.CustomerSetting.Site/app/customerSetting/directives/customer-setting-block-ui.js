'use strict';

(function () {
    var injectParams = ['blockUiService'];
    var customerSettingBlockUiDirective = function (blockUiService) {
        return {
            restrict: 'AE',
            replace: true,
            templateUrl: 'templates/customer-setting-block-ui.htm',
            controller: ['$scope', function ($scope) {
                blockUiService.setup($scope);
            }]
        };
    };

    customerSettingBlockUiDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingBlockUi', customerSettingBlockUiDirective);
})();