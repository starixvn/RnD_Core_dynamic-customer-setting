(function () {
    'use strict';
    var injectParams = ['fanexDirectiveBlock'];
    var fanexPtAgentEditMemberBlockDirective = function (DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-agent-edit-member-block.htm');
    };

    fanexPtAgentEditMemberBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtAgentEditMemberBlock', fanexPtAgentEditMemberBlockDirective);
})();