(function () {
    'use strict';
    var injectParams = ['$filter'];
    var fanexPtFormat = function ($filter) {
        return function (value) {
            if (angular.isNumber(value)) {
                return $filter('number')(value / 100, 2);
            }

            return undefined;
        };
    };

    fanexPtFormat.$inject = injectParams;
    angular.module('customerSettingMatrixApp').filter('fanexPtFormat', fanexPtFormat);
})();