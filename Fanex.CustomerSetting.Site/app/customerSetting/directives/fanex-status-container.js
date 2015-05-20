'use strict';

(function () {
    var injectParams = [];
    var fanexStatusContainerDirective = function () {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                model: '=fanexStatusContainer'
            },
            templateUrl: 'templates/fanex-status-container.htm'
        };
    };

    fanexStatusContainerDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexStatusContainer', fanexStatusContainerDirective);
})();