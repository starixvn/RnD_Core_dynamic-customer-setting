'use strict';

(function () {
    var injectParams = ['fanexModelService'];
    var fanexDiscountContainerDirective = function (modelService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                model: '=fanexDiscountContainer'
            },
            templateUrl: 'templates/fanex-discount-container.htm',
            controller: ['$scope', function ($scope) {
                var queryModel = modelService.getQueryModel();
                $scope.formType = queryModel.FormType;
            }]
        };
    };

    fanexDiscountContainerDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountContainer', fanexDiscountContainerDirective);
})();