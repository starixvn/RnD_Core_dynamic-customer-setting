(function () {
    'use strict';
    var injectParams = ['fanexModelService', 'fanexDirectiveBlock'];
    var fanexDiscountBlockDirective = function (modelService, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-discount-block.htm', function (scope) {
            var queryModel = modelService.getQueryModel();
            scope.formType = queryModel.FormType;
        });
    };

    fanexDiscountBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountBlock', fanexDiscountBlockDirective);
})();