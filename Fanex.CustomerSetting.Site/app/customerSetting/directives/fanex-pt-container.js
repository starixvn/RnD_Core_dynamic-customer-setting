'use strict';

(function () {
    var injectParams = ['fanexModelService'];
    var fanexPtContainerDirective = function (modelService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                model: '=fanexPtContainer'
            },
            templateUrl: 'templates/fanex-pt-container.htm',
            controller: ['$scope', function ($scope) {
                var queryModel = modelService.getQueryModel();
                $scope.formType = queryModel.FormType;
            }]
        };
    };

    fanexPtContainerDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtContainer', fanexPtContainerDirective);
})();