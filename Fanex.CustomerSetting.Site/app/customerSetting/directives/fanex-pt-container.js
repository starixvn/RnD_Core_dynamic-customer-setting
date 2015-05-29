(function () {
    'use strict';
    var injectParams = [];
    var fanexPtContainerDirective = function () {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                model: '=fanexPtContainer'
            },
            templateUrl: 'templates/fanex-pt-container.htm'
        };
    };

    fanexPtContainerDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtContainer', fanexPtContainerDirective);
})();