'use strict';

(function () {
    var injectParams = [];
    var fanexBetLimitContainerDirective = function () {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                model: '=fanexBetLimitContainer'
            },
            templateUrl: 'templates/fanex-bet-limit-container.htm'
        };
    };

    fanexBetLimitContainerDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBetLimitContainer', fanexBetLimitContainerDirective);
})();