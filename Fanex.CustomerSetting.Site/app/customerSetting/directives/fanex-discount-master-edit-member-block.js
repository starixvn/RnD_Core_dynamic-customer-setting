'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDirectiveBlock'];
    var fanexDiscountMasterEditMemberBlockDirective = function (appSettings, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-discount-master-edit-member-block.htm');
    };

    fanexDiscountMasterEditMemberBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountMasterEditMemberBlock', fanexDiscountMasterEditMemberBlockDirective);
})();