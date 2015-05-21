(function () {
    'use strict';
    var injectParams = ['fanexModelService', 'fanexDirectiveBlock'];
    var fanexBetLimitBlockDirective = function (modelService, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-bet-limit-block.htm', function (scope) {
            var queryModel = modelService.getQueryModel();
            scope.formType = queryModel.FormType;
        });
    };

    fanexBetLimitBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBetLimitBlock', fanexBetLimitBlockDirective);
})();