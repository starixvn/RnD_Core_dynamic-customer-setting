(function () {
    'use strict';
    var injectParams = ['$rootScope'];
    var fanexPtSelectDirective = function ($rootScope) {
        return {
            restrict: 'AE',
            scope: {
                data: '=fanexPtSelect'
            },
            templateUrl: 'templates/fanex-pt-select.htm',
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

    fanexPtSelectDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtSelect', fanexPtSelectDirective);
})();