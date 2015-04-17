'use strict';

(function () {
    var injectParams = ['$rootScope'];
    var positionTakingSelectDirective = function ($rootScope) {
        return {
            restrict: 'AE',
            scope: {
                data: '=positionTakingSelect'
            },
            templateUrl: 'templates/position-taking-select.htm',
            link: function (scope, element) {
                element.on('click', function () {
                    var click = function () {
                        scope.data.valid = true;
                    };

                    if ($rootScope.$$phase) {
                        scope.$evalAsync(click);
                    } else {
                        scope.$apply(click);
                    }
                });
            }
        };
    };

    positionTakingSelectDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('positionTakingSelect', positionTakingSelectDirective);
})();