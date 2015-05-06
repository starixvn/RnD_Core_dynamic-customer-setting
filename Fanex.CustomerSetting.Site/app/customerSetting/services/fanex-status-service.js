'use strict';

(function () {
    var injectParams = ['fanexDirectiveLinker'];
    var fanexStatusService = function (DirectiveLinker) {
        var service = new DirectiveLinker();
        return service;
    };

    fanexStatusService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexStatusService', fanexStatusService);
})();