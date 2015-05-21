(function () {
    'use strict';
    var injectParams = ['fanexDirectiveBlock'];
    var fanexPtMasterEditMemberBlockDirective = function (DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-master-edit-member-block.htm');
    };

    fanexPtMasterEditMemberBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtMasterEditMemberBlock', fanexPtMasterEditMemberBlockDirective);
})();