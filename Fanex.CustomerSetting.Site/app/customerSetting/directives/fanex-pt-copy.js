'use strict';

(function () {
    var injectParams = ['fanexPtService', 'fanexResourceService', 'fanexTooltipService'];
    var fanexPtCopyDirective = function (ptService, resourceService, tooltipService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                hasCopy: '=fanexPtCopy'
            },
            templateUrl: 'templates/fanex-pt-copy.htm',
            link: function (scope, element) {
                if (!scope.hasCopy) {
                    return;
                }

                scope.copy = function () {
                    var count = 0;
                    var scopes = ptService.getScopes();
                    var source = scopes[0];
                    for (var i = 1; i < scopes.length; i++) {
                        var scope = scopes[i];
                        var result = scope.copy(source);
                        if (!result) {
                            count++;
                        }
                    }

                    if (count > 0) {
                        tooltipService.show(element, resourceService.getString('DuplicateValuesAlertMsg'), 'left');
                        element.on('mouseleave', function () {
                            tooltipService.hide();
                            element.off('mouseleave');
                        });
                    } else {
                        tooltipService.hide();
                    }
                };
            }
        };
    };

    fanexPtCopyDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtCopy', fanexPtCopyDirective);
})();