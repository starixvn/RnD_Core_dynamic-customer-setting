'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexModelService', 'fanexDirectiveContainer'];
    var fanexStatusContainerDirective = function (appSettings, modelService, DirectiveContainer) {
        var queryModel = modelService.getQueryModel();
        var formType = queryModel.FormType;
        return new DirectiveContainer('templates/fanex-status-container.htm', {
            settingType: appSettings.settingType.Status,
            formType: formType
        });
    };

    fanexStatusContainerDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexStatusContainer', fanexStatusContainerDirective);
})();