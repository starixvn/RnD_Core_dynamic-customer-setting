'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexModelService', 'fanexDirectiveContainer'];
    var fanexBetLimitContainerDirective = function (appSettings, modelService, DirectiveContainer) {
        var queryModel = modelService.getQueryModel();
        var formType = queryModel.FormType;
        return new DirectiveContainer('templates/fanex-bet-limit-container.htm', {
            settingType: appSettings.settingType.BetLimit,
            formType: formType
        });
    };

    fanexBetLimitContainerDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBetLimitContainer', fanexBetLimitContainerDirective);
})();