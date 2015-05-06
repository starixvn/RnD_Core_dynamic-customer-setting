'use strict';

(function () {
    var injectParams = ['fanexDirectiveLinker'];
    var fanexBetLimitService = function (DirectiveLinker) {
        var service = new DirectiveLinker();
        return service;
    };

    fanexBetLimitService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexBetLimitService', fanexBetLimitService);
})();