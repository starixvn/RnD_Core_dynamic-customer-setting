'use strict';

(function () {
    var injectParams = ['fanexTooltipService'];
    var fanexTooltipDirective = function (tooltipService) {
        return {
            restrict: 'AE',
            priority: 100,
            replace: true,
            templateUrl: 'templates/fanex-tooltip.htm',
            scope: {},
            link: function (scope, element) {
                tooltipService.setup(scope, element);
                scope.$watch('message', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        tooltipService.calculatePosition();
                    }
                });
            }
        };
    };

    fanexTooltipDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexTooltip', fanexTooltipDirective);
})();