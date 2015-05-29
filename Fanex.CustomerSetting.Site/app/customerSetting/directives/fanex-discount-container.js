(function () {
    'use strict';
    var injectParams = [];
    var fanexDiscountContainerDirective = function () {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                model: '=fanexDiscountContainer'
            },
            templateUrl: 'templates/fanex-discount-container.htm'
        };
    };

    fanexDiscountContainerDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountContainer', fanexDiscountContainerDirective);
})();