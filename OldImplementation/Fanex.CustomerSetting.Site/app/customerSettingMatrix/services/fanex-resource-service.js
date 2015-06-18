(function () {
    'use strict';
    var injectParams = [];
    var fanexResourceService = function () {
        var productName;
        return {
            ProductName: productName
        };
    };

    fanexResourceService.$inject = injectParams;
    angular.module('customerSettingMatrixApp').service('fanexResourceService', fanexResourceService);
})();