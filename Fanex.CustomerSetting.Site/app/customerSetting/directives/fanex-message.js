(function () {
    'use strict';
    var injectParams = ['fanexMessageService'];
    var fanexMessageDirective = function (fanexMessageService) {
        return {
            restrict: 'A',
            replace: true,
            templateUrl: 'templates/fanex-message.htm',
            controller: ['$scope', function ($scope) {
                fanexMessageService.setup($scope);
            } ]
        };
    };

    fanexMessageDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexMessage', fanexMessageDirective);
})();