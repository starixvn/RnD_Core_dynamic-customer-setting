(function () {
    'use strict';
    var injectParams = ['fanexDirectiveBlock'];
    var fanexBetLimitBlockDirective = function (DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-bet-limit-block.htm');
    };

    fanexBetLimitBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBetLimitBlock', fanexBetLimitBlockDirective);
})();