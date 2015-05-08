'use strict';

(function () {
    var injectParams = ['fanexProductSettings'];
    var fanexProductSettingService = function (productSettings) {
        function getViewSetting(productId) {
            var viewSettings = productSettings.viewSettings;
            var viewSetting = viewSettings['default'];
            angular.forEach(viewSettings.products, function (productViewSetting) {
                if (productViewSetting.id === productId) {
                    viewSetting = productViewSetting;
                }
            });

            return viewSetting;
        }

        return {
            getViewSetting: getViewSetting,
            errorViewSetting: productSettings.viewSettings.error
        };
    };

    fanexProductSettingService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexProductSettingService', fanexProductSettingService);
})();