(function () {
    'use strict';
    var injectParams = ['fanexDirectiveBlock'];
    var fanexStatusBlockDirective = function (DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-status-block.htm');
    };

    fanexStatusBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexStatusBlock', fanexStatusBlockDirective);
})();