(function () {
    'use strict';
    var injectParams = ['fanexModelService', 'fanexDirectiveBlock'];
    var fanexStatusBlockDirective = function (modelService, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-status-block.htm', function (scope) {
            var queryModel = modelService.getQueryModel();
            scope.formType = queryModel.FormType;
        });
    };

    fanexStatusBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexStatusBlock', fanexStatusBlockDirective);
})();