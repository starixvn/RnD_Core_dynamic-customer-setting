'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDirectiveBlock'];
    var fanexPtMasterEditMemberBlockDirective = function (appSettings, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-master-edit-member-block.htm');
    };

    fanexPtMasterEditMemberBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtMasterEditMemberBlock', fanexPtMasterEditMemberBlockDirective);
})();