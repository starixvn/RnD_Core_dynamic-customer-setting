(function () {
    'use strict';
    var injectParams = ['fanexDirectiveBlock'];
    var fanexDiscountBlockDirective = function (DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-discount-block.htm');
    };

    fanexDiscountBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountBlock', fanexDiscountBlockDirective);
})();