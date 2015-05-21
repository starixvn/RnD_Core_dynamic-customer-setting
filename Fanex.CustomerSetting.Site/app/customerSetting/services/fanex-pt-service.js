(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexDirectiveLinker', 'fanexSelectBuilder'];
    var fanexPtService = function (appSettings, DirectiveLinker, SelectBuilder) {
        var service = new DirectiveLinker();

        service.copyValue = function (source, destination, field, generator) {
            destination[field].value = source[field].value;
            destination[field] = generator();
            var isCopied = source[field].value === destination[field].value;
            destination[field].valid = isCopied;
            return isCopied;
        };

        service.correctValue = function (min, max, value) {
            return Math.max(min, Math.min(max, value));
        };

        service.createSelect = function (min, max, value) {
            var selectBuilder = new SelectBuilder(1, 1);
            return selectBuilder.build(min, max, value);
        };

        return service;
    };

    fanexPtService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexPtService', fanexPtService);
})();