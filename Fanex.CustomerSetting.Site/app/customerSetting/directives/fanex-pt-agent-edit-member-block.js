'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDirectiveBlock'];
    var fanexPtAgentEditMemberBlockDirective = function (appSettings, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-agent-edit-member-block.htm');
    };

    fanexPtAgentEditMemberBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtAgentEditMemberBlock', fanexPtAgentEditMemberBlockDirective);
})();