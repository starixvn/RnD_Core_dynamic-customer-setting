'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDirectiveBlock'];
    var fanexDiscountSuperEditMemberBlockDirective = function (appSettings, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-discount-super-edit-member-block.htm');
    };

    fanexDiscountSuperEditMemberBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountSuperEditMemberBlock', fanexDiscountSuperEditMemberBlockDirective);
})();