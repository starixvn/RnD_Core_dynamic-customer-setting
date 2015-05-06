'use strict';

(function () {
    var injectParams = ['fanexDirectiveLinker', 'fanexSelectBuilder'];
    var fanexDiscountService = function (DirectiveLinker, SelectBuilder) {
        var service = new DirectiveLinker();

        service.createSelect = function (min, max, value) {
            var selectBuilder = new SelectBuilder(0.0005, 10000);
            return selectBuilder.build(min, max, value);
        };

        return service;
    };

    fanexDiscountService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexDiscountService', fanexDiscountService);
})();