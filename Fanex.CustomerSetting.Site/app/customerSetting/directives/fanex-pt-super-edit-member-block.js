'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDirectiveBlock'];
    var fanexPtSuperEditMemberBlockDirective = function (appSettings, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-super-edit-member-block.htm');
    };

    fanexPtSuperEditMemberBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtSuperEditMemberBlock', fanexPtSuperEditMemberBlockDirective);
})();