(function () {
    'use strict';
    var injectParams = ['fanexDirectiveBlock'];
    var fanexPtSuperEditMasterBlockDirective = function (DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-super-edit-master-block.htm');
    };

    fanexPtSuperEditMasterBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtSuperEditMasterBlock', fanexPtSuperEditMasterBlockDirective);
})();