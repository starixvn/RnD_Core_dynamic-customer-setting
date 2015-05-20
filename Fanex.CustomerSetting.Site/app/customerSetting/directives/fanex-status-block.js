'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexModelService', 'fanexDirectiveBlock'];
    var fanexStatusBlockDirective = function (appSettings, modelService, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-status-block.htm', function (scope) {
            var queryModel = modelService.getQueryModel();
            scope.formType = queryModel.FormType;
        });
    };

    fanexStatusBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexStatusBlock', fanexStatusBlockDirective);
})();