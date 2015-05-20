'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDirectiveBlock'];
    var fanexPtSuperEditMasterBlockDirective = function (appSettings, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-super-edit-master-block.htm');
    };

    fanexPtSuperEditMasterBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtSuperEditMasterBlock', fanexPtSuperEditMasterBlockDirective);
})();