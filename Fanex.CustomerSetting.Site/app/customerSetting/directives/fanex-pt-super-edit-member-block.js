(function () {
    'use strict';
    var injectParams = ['fanexDirectiveBlock'];
    var fanexPtSuperEditMemberBlockDirective = function (DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-super-edit-member-block.htm');
    };

    fanexPtSuperEditMemberBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtSuperEditMemberBlock', fanexPtSuperEditMemberBlockDirective);
})();