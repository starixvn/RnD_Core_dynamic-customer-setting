'use strict';

(function () {
    var injectParams = ['fanexResourceService'];
    var fanexTranslate = function (resourceService) {
        return function (key) {
            return resourceService.getString(key);
        };
    };

    fanexTranslate.$inject = injectParams;
    angular.module('customerSettingApp').filter('fanexTranslate', fanexTranslate);
})();