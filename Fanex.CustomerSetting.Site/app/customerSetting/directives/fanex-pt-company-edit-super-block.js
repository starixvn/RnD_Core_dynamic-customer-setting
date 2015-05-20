'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDirectiveBlock'];
    var fanexPtCompanyEditSuperBlockDirective = function (appSettings, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-company-edit-super-block.htm');
    };

    fanexPtCompanyEditSuperBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtCompanyEditSuperBlock', fanexPtCompanyEditSuperBlockDirective);
})();