'use strict';

(function () {
    var injectParams = ['tooltipService'];
    var customerSettingTooltipDirective = function (tooltipService) {
        return {
            restrict: 'AE',
            replace: true,
            templateUrl: '/app/customerSetting/templates/customer-setting-tooltip.htm',
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

    customerSettingTooltipDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingTooltip', customerSettingTooltipDirective);
})();