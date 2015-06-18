(function () {
    'use strict';
    var injectParams = ['fanexDirectiveBlock'];
    var fanexDiscountSuperEditMemberBlockDirective = function (DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-discount-super-edit-member-block.htm');
    };

    fanexDiscountSuperEditMemberBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountSuperEditMemberBlock', fanexDiscountSuperEditMemberBlockDirective);
})();