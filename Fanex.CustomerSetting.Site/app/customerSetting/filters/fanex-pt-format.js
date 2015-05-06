'use strict';

(function () {
    var injectParams = ['fanexAppSettings'];
    var fanexPtFormat = function (appSettings) {
        return function (value) {
            return value / appSettings.positionTakingDivider;
        };
    };

    fanexPtFormat.$inject = injectParams;
    angular.module('customerSettingApp').filter('fanexPtFormat', fanexPtFormat);
})();