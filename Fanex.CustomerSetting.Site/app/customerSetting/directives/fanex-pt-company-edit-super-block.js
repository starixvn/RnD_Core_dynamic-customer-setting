(function () {
    'use strict';
    var injectParams = ['fanexDirectiveBlock'];
    var fanexPtCompanyEditSuperBlockDirective = function (DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-company-edit-super-block.htm');
    };

    fanexPtCompanyEditSuperBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtCompanyEditSuperBlock', fanexPtCompanyEditSuperBlockDirective);
})();