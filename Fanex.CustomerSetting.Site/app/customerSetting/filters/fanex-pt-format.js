(function () {
    'use strict';
    var injectParams = ['fanexAppSettings'];
    var fanexPtFormat = function (appSettings) {
        return function (value) {
            if (angular.isNumber(value)) {
                return value / appSettings.positionTakingDivider;
            }

            return undefined;
        };
    };

    fanexPtFormat.$inject = injectParams;
    angular.module('customerSettingApp').filter('fanexPtFormat', fanexPtFormat);
})();