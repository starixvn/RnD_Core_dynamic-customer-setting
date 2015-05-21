(function () {
    'use strict';
    var injectParams = ['fanexDirectiveBlock'];
    var fanexDiscountMasterEditMemberBlockDirective = function (DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-discount-master-edit-member-block.htm');
    };

    fanexDiscountMasterEditMemberBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountMasterEditMemberBlock', fanexDiscountMasterEditMemberBlockDirective);
})();