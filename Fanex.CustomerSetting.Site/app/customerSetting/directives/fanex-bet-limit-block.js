'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexModelService', 'fanexDirectiveBlock'];
    var fanexBetLimitBlockDirective = function (appSettings, modelService, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-bet-limit-block.htm', function (scope) {
            var queryModel = modelService.getQueryModel();
            scope.formType = queryModel.FormType;
        });
    };

    fanexBetLimitBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBetLimitBlock', fanexBetLimitBlockDirective);
})();