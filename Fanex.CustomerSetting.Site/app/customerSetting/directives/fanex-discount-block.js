'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexModelService', 'fanexDirectiveBlock'];
    var fanexDiscountBlockDirective = function (appSettings, modelService, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-discount-block.htm', function (scope) {
            var queryModel = modelService.getQueryModel();
            scope.formType = queryModel.FormType;
        });
    };

    fanexDiscountBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountBlock', fanexDiscountBlockDirective);
})();