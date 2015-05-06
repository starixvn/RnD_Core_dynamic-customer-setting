'use strict';

(function () {
    var injectParams = ['fanexBetLimitService'];
    var fanexBetLimitCopyDirective = function (betLimitService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                hasCopy: '=fanexBetLimitCopy'
            },
            templateUrl: 'templates/fanex-bet-limit-copy.htm',
            link: function (scope) {
                if (!scope.hasCopy) {
                    return;
                }

                scope.copy = function () {
                    var scopes = betLimitService.getScopes();
                    var source = scopes[0];
                    for (var i = 1; i < scopes.length; i++) {
                        var scope = scopes[i];
                        scope.copy(source);
                    }
                };
            }
        };
    };

    fanexBetLimitCopyDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBetLimitCopy', fanexBetLimitCopyDirective);
})();